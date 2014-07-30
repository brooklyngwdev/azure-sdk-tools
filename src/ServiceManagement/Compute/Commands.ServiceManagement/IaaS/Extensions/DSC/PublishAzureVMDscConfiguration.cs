﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Commands.ServiceManagement.IaaS.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using Commands.Common.Storage;
    using Microsoft.WindowsAzure.Commands.ServiceManagement.IaaS.Extensions.DSC;
    using Microsoft.WindowsAzure.Commands.ServiceManagement.Properties;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Utilities.Common;

    /// <summary>
    /// Uploads a Desired State Configuration script to Azure blob storage, which 
    /// later can be applied to Azure Virtual Machines using the 
    /// Set-AzureVMDscExtension cmdlet.
    /// </summary>
    [Cmdlet(VerbsData.Publish, "AzureVMDscConfiguration", SupportsShouldProcess = true, DefaultParameterSetName = UploadArchiveParameterSetName)]
    public class PublishAzureVMDscConfigurationCommand : ServiceManagementBaseCmdlet
    {
        private const string CreateArchiveParameterSetName = "CreateArchive";
        private const string UploadArchiveParameterSetName = "UploadArchive";

        /// <summary>
        /// Path to a file containing one or more configurations; the file can be a 
        /// PowerShell script (*.ps1) or MOF interface (*.mof).
        /// </summary>
        [Parameter(Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Path to a file containing one or more configurations")]
        [ValidateNotNullOrEmpty]
        public string ConfigurationPath { get; set; }

        /// <summary>
        /// Name of the Azure Storage Container the configuration is uploaded to.
        /// </summary>
        [Parameter(
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = UploadArchiveParameterSetName,
            HelpMessage = "Name of the Azure Storage Container the configuration is uploaded to")]
        [ValidateNotNullOrEmpty]
        public string ContainerName { get; set; }

        /// <summary>
        /// By default Publish-AzureVMDscConfiguration will not overwrite any existing blobs. 
        /// Use -Force to overwrite them.
        /// </summary>
        [Parameter(HelpMessage = "By default Publish-AzureVMDscConfiguration will not overwrite any existing blobs")]
        public SwitchParameter Force { get; set; }

        /// <summary>
        /// The Azure Storage Context that provides the security settings used to upload 
        /// the configuration script to the container specified by ContainerName. This 
        /// context should provide write access to the container.
        /// </summary>
        [Parameter(
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = UploadArchiveParameterSetName,
            HelpMessage = "The Azure Storage Context that provides the security settings used to upload " +
                          "the configuration script to the container specified by ContainerName")]
        [ValidateNotNullOrEmpty]
        public AzureStorageContext StorageContext { get; set; }

        /// <summary>
        /// Path to a local ZIP file to write the configuration archive to.
        /// When using this parameter, Publish-AzureVMDscConfiguration creates a
        /// local ZIP archive instead of uploading it to blob storage..
        /// </summary>
        [Parameter(
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = CreateArchiveParameterSetName,
            HelpMessage = "Path to a local ZIP file to write the configuration archive to.")]
        [ValidateNotNullOrEmpty]
        public string ConfigurationArchivePath { get; set; }

        /// <summary>
        /// Credentials used to access Azure Storage
        /// </summary>
        private StorageCredentials _storageCredentials;

        private const string Ps1FileExtension = ".ps1";
        private const string Psm1FileExtension = ".psm1";
        private const string ZipFileExtension = ".zip";
        private static readonly HashSet<String> UploadArchiveAllowedFileExtensions = new HashSet<String>(StringComparer.OrdinalIgnoreCase) { Ps1FileExtension, Psm1FileExtension, ZipFileExtension };
        private static readonly HashSet<String> CreateArchiveAllowedFileExtensions = new HashSet<String>(StringComparer.OrdinalIgnoreCase) { Ps1FileExtension, Psm1FileExtension};

        private const int MinMajorPowerShellVersion = 4;

        private List<string> _temporaryFilesToDelete = new List<string>();
        private List<string> _temporaryDirectoriesToDelete = new List<string>();

        protected override void ProcessRecord()
        {
            try
            {
                base.ProcessRecord();
                ExecuteCommand();
            }
            finally
            {
                foreach (var file in this._temporaryFilesToDelete)
                {
                    try
                    {
                        File.Delete(file);
                        WriteVerbose(string.Format(CultureInfo.CurrentUICulture, Resources.PublishVMDscExtensionDeletedFileMessage, file)); 
                    }
                    catch (Exception e)
                    {
                        WriteVerbose(string.Format(CultureInfo.CurrentUICulture, Resources.PublishVMDscExtensionDeleteErrorMessage, file, e.Message));
                    }
                }
                foreach (var directory in this._temporaryDirectoriesToDelete)
                {
                    try
                    {
                        Directory.Delete(directory, true);
                        WriteVerbose(string.Format(CultureInfo.CurrentUICulture, Resources.PublishVMDscExtensionDeletedFileMessage, directory));
                    }
                    catch (Exception e)
                    {
                        WriteVerbose(string.Format(CultureInfo.CurrentUICulture, Resources.PublishVMDscExtensionDeleteErrorMessage, directory, e.Message));
                    }
                }
            }
        }

        internal void ExecuteCommand()
        {
            ValidatePsVersion();
            ValidateParameters();
            PublishConfiguration();
        }

        protected void ValidatePsVersion()
        {
            using (PowerShell powershell = PowerShell.Create())
            {
                powershell.AddScript("$PSVersionTable.PSVersion.Major");
                int major = powershell.Invoke<int>().FirstOrDefault();
                if (major < MinMajorPowerShellVersion)
                {
                    this.ThrowTerminatingError(
                        new ErrorRecord(
                            new InvalidOperationException(
                                string.Format(CultureInfo.CurrentUICulture, Resources.PublishVMDscExtensionRequiredPsVersion, MinMajorPowerShellVersion, major)), 
                                string.Empty,
                                ErrorCategory.InvalidOperation,
                                null));
                }
            }
        }

        protected void ValidateParameters()
        {
            this.ConfigurationPath = this.GetUnresolvedProviderPathFromPSPath(this.ConfigurationPath);

            var configurationFileExtension = Path.GetExtension(this.ConfigurationPath);

            if (this.ParameterSetName == UploadArchiveParameterSetName)
            { 
                // Check that ConfigurationPath points to a valid file
                if (!File.Exists(this.ConfigurationPath))
                {
                    this.ThrowInvalidArgumentError(Resources.PublishVMDscExtensionConfigFileNotFound, this.ConfigurationPath);
                }
                if (!UploadArchiveAllowedFileExtensions.Contains(Path.GetExtension(configurationFileExtension)))
                {
                    this.ThrowInvalidArgumentError(Resources.PublishVMDscExtensionUploadArchiveConfigFileInvalidExtension, this.ConfigurationPath);
                }

                this._storageCredentials = this.GetStorageCredentials(this.StorageContext);

                if (this.ContainerName == null)
                {
                    this.ContainerName = VirtualMachineDscExtensionCmdletBase.DefaultContainerName;
                }
            } 
            else if (this.ParameterSetName == CreateArchiveParameterSetName)
            {
                if (!CreateArchiveAllowedFileExtensions.Contains(Path.GetExtension(configurationFileExtension)))
                {
                    this.ThrowInvalidArgumentError(Resources.PublishVMDscExtensionCreateArchiveConfigFileInvalidExtension, this.ConfigurationPath);
                }

                this.ConfigurationArchivePath = this.GetUnresolvedProviderPathFromPSPath(this.ConfigurationArchivePath);
            }
        }

        /// <summary>
        /// Publish the configuration and its modules
        /// </summary>
        protected void PublishConfiguration()
        {
            if (this.ParameterSetName == CreateArchiveParameterSetName)
            {
                this.ConfirmAction(true, string.Empty, Resources.AzureVMDscCreateArchiveAction, this.ConfigurationArchivePath, ()=> CreateConfigurationArchive());
            }
            else
            {
                var archivePath = string.Compare(Path.GetExtension(this.ConfigurationPath), ZipFileExtension, StringComparison.OrdinalIgnoreCase) == 0 ?
                    this.ConfigurationPath
                    :
                    CreateConfigurationArchive();

                UploadConfigurationArchive(archivePath);
            }
        }

        private string CreateConfigurationArchive()
        {
            WriteVerbose(String.Format(CultureInfo.CurrentUICulture, Resources.AzureVMDscParsingConfiguration, this.ConfigurationPath));
            ConfigurationParseResult parseResult = ConfigurationParsingHelper.ExtractConfigurationNames(this.ConfigurationPath);
            if (parseResult.Errors.Any())
            {
                ThrowTerminatingError(
                    new ErrorRecord(
                        new ParseException(
                            String.Format(
                                CultureInfo.CurrentUICulture,
                                Resources.PublishVMDscExtensionStorageParserErrors,
                                this.ConfigurationPath,
                                String.Join("\n", parseResult.Errors.Select(error => error.ToString())))),
                        string.Empty,
                        ErrorCategory.ParserError,
                        null));
            }
            List<string> requiredModules = parseResult.RequiredModules;
            WriteVerbose(String.Format(CultureInfo.CurrentUICulture, Resources.PublishVMDscExtensionRequiredModulesVerbose, String.Join(", ", requiredModules)));

            // Create a temporary directory for uploaded zip file
            string tempZipFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            WriteVerbose(String.Format(CultureInfo.CurrentUICulture, Resources.PublishVMDscExtensionTempFolderVerbose, tempZipFolder));
            Directory.CreateDirectory(tempZipFolder);
            this._temporaryDirectoriesToDelete.Add(tempZipFolder);
            
            // CopyConfiguration
            string configurationName = Path.GetFileName(this.ConfigurationPath);
            string configurationDestination = Path.Combine(tempZipFolder, configurationName);
            WriteVerbose(String.Format(
                CultureInfo.CurrentUICulture, 
                Resources.PublishVMDscExtensionCopyFileVerbose, 
                this.ConfigurationPath, 
                configurationDestination));
            File.Copy(this.ConfigurationPath, configurationDestination);
            
            // CopyRequiredModules
            foreach (var module in requiredModules)
            {
                using (PowerShell powershell = PowerShell.Create())
                {
                    // Wrapping script in a function to prevent script injection via $module variable.
                    powershell.AddScript(
                        @"function Copy-Module([string]$module, [string]$tempZipFolder) 
                        {
                            $mi = Get-Module -List -Name $module;
                            $moduleFolder = Split-Path -Parent $mi.Path;
                            Copy-Item -Recurse -Path $moduleFolder -Destination $tempZipFolder;
                        }"
                        );
                    powershell.Invoke();
                    powershell.Commands.Clear();
                    powershell.AddCommand("Copy-Module")
                        .AddParameter("module", module)
                        .AddParameter("tempZipFolder", tempZipFolder);
                    WriteVerbose(String.Format(
                        CultureInfo.CurrentUICulture,
                        Resources.PublishVMDscExtensionCopyModuleVerbose,
                        module,
                        tempZipFolder));
                    powershell.Invoke();
                }
            }

            //
			// Zip the directory
            //
            string archive;

            if (this.ParameterSetName == CreateArchiveParameterSetName)
            {
                archive = this.ConfigurationArchivePath;

                if (!this.Force && System.IO.File.Exists(archive))
                {
                    this.ThrowTerminatingError(
                        new ErrorRecord(
                            new UnauthorizedAccessException(string.Format(CultureInfo.CurrentUICulture, Resources.AzureVMDscArchiveAlreadyExists, archive)),
                            string.Empty,
                            ErrorCategory.PermissionDenied,
                            null));
                }
            }
            else
            {
                archive = Path.Combine(Path.GetTempPath(), configurationName + ZipFileExtension);

                if (File.Exists(archive))
                {
                    File.Delete(archive);
                }

                this._temporaryFilesToDelete.Add(archive);
            }

            // azure-sdk-tools uses .net framework 4.0
            // System.IO.Compression.ZipFile was added in .net 4.5
            // Since support for DSC require powershell 4.0+, which require .net 4.5+
            // we assume that created powershell session will have access to System.IO.Compression.FileSystem assembly
            // from version 4.5. We load it to create a zip archive from a directory.
            using (var powershell = System.Management.Automation.PowerShell.Create())
            {
				var script = 
					@"Add-Type -AssemblyName System.IO.Compression.FileSystem > $null;" +
                    @"[void] [System.IO.Compression.ZipFile]::CreateFromDirectory('" + tempZipFolder + "', '" + archive + "');";

                powershell.AddScript(script);
                WriteVerbose(String.Format(
                        CultureInfo.CurrentUICulture,
                        Resources.PublishVMDscExtensionCreateZipVerbose,
                        archive,
                        tempZipFolder));
                powershell.Invoke();
            }

            return archive;
        }

        private void UploadConfigurationArchive(string archivePath)
        {
            CloudBlobContainer cloudBlobContainer = GetStorageContainier();

            var blobName = Path.GetFileName(archivePath);

            CloudBlockBlob modulesBlob = cloudBlobContainer.GetBlockBlobReference(blobName);

            this.ConfirmAction(true, string.Empty, string.Format(CultureInfo.CurrentUICulture, Resources.AzureVMDscUploadToBlobStorageAction, archivePath), modulesBlob.Uri.AbsoluteUri, () =>
            {
                if (!this.Force && modulesBlob.Exists())
                {
                    this.ThrowTerminatingError(
                        new ErrorRecord(
                            new UnauthorizedAccessException(string.Format(CultureInfo.CurrentUICulture, Resources.AzureVMDscStorageBlobAlreadyExists, modulesBlob)),
                            string.Empty,
                            ErrorCategory.PermissionDenied,
                            null));
                }

                modulesBlob.UploadFromFile(archivePath, FileMode.Open);
            });
        }

        private CloudBlobContainer GetStorageContainier()
        {
            var storageAccount = new CloudStorageAccount(this._storageCredentials, true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer containerReference = blobClient.GetContainerReference(this.ContainerName);
            containerReference.CreateIfNotExists();
            return containerReference;
        }
    }
}

