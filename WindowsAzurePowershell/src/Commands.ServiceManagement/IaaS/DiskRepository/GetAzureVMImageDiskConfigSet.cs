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

namespace Microsoft.WindowsAzure.Commands.ServiceManagement.IaaS
{
    using System.Management.Automation;
    using Model;

    [Cmdlet(
        VerbsCommon.Get,
        AzureDiskConfigSetNoun),
    OutputType(typeof(VirtualMachineImageDiskConfigSet))]
    public class GetAzureVMImageDiskConfigSet : PSCmdlet
    {
        protected const string AzureDiskConfigSetNoun = "AzureVMImageDiskConfigSet";

        [Parameter(
            Position = 1,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The OS/VM Image Context")]
        public OSImageContext ImageContext { get; set; }

        protected override void ProcessRecord()
        {
            ServiceManagementProfile.Initialize();

            var image = ImageContext as VMImageContext;

            var diskConfig = image == null ? null : new VirtualMachineImageDiskConfigSet
            {
                OSDiskConfiguration = image.OSDiskConfiguration,
                DataDiskConfigurations = image.DataDiskConfigurations
            };

            WriteObject(diskConfig);
        }
    }
}
