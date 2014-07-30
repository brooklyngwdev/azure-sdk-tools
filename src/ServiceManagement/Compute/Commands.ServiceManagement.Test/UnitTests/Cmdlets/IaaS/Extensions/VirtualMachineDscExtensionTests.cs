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

namespace Microsoft.WindowsAzure.Commands.ServiceManagement.Test.UnitTests.Cmdlets.IaaS.Extensions
{
    using System;
    using System.Linq;
    using Commands.Test.Utilities.Common;
    using ServiceManagement.IaaS.Extensions.DSC;
    using VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for DSC ConfigurationParsingHelper class.
    /// </summary>
    /// <remarks>
    /// ConfigurationParsingHelper.ExtractConfigurationNames() API requires tests to be run in x64 host.
    /// These tests also require presents of some DSC resource modules on the test machine.
    /// That cannot be ommit, because the language Parser need to load each module on parsing, so additional
    /// dynamic keywords that descrite Configuration can be handled appropriately.
    /// List of required modules:
    /// xComputerManagement
    /// xNetworking
    /// xPSDesiredStateConfiguration
    /// xActiveDirectory
    /// </remarks>
    [TestClass]
    public class VirtualMachineDscExtensionTests : TestBase
    {
        private const string CorporateClientConfigurationPath = @"DSC\Configurations\CorporateClientConfiguration.ps1";
        private const string DomainControllerConfigurationPath = @"DSC\Configurations\DomainControllerConfiguration.ps1";
        private const string SHMulptiConfigurationsPath = @"DSC\Configurations\SHMulptiConfigurations.ps1";
        private const string VisualStudioPath = @"DSC\Configurations\VisualStudio.ps1";
        private const string VisualStudio2Path = @"DSC\Configurations\VisualStudio2.ps1";

        [TestMethod]
        [TestCategory("Scenario")]
        public void TestGetModuleNameForDscResourceXComputer()
        {
            string moduleName = ConfigurationParsingHelper.GetModuleNameForDscResource("MSFT_xComputer");
            Assert.AreEqual("xComputerManagement", moduleName);
        }

        [TestMethod]
        [TestCategory("Scenario")]
        public void TestGetModuleNameForDscResourceXADDomain()
        {
            string moduleName = ConfigurationParsingHelper.GetModuleNameForDscResource("MSFT_xADDomain");
            Assert.AreEqual("xActiveDirectory", moduleName);
        }

        [TestMethod]
        [TestCategory("Scenario")]
        [DeploymentItem(CorporateClientConfigurationPath)]
        public void TestExtractConfigurationNames1()
        {
            ConfigurationParseResult results = ConfigurationParsingHelper.ExtractConfigurationNames(CorporateClientConfigurationPath);
            Assert.AreEqual(0, results.Errors.Count());
            Assert.AreEqual(1, results.RequiredModules.Count);
            Assert.AreEqual("xComputerManagement", results.RequiredModules[0]);
        }

        [TestMethod]
        [TestCategory("Scenario")]
        [DeploymentItem(DomainControllerConfigurationPath)]
        public void TestExtractConfigurationNames2()
        {
            ConfigurationParseResult results = ConfigurationParsingHelper.ExtractConfigurationNames(DomainControllerConfigurationPath);
            Assert.AreEqual(0, results.Errors.Count());
            Assert.AreEqual(2, results.RequiredModules.Count);
            Assert.AreEqual("xComputerManagement", results.RequiredModules[0]);
            Assert.AreEqual("xActiveDirectory", results.RequiredModules[1]);
        }

        [TestMethod]
        [TestCategory("Scenario")]
        [DeploymentItem(VisualStudioPath)]
        public void TestExtractConfigurationNames3()
        {
            ConfigurationParseResult results = ConfigurationParsingHelper.ExtractConfigurationNames(VisualStudioPath);
            Assert.AreEqual(0, results.Errors.Count());
            Assert.AreEqual(1, results.RequiredModules.Count);
            Assert.AreEqual("xPSDesiredStateConfiguration", results.RequiredModules[0]);
        }

        [TestMethod]
        [TestCategory("Scenario")]
        [DeploymentItem(VisualStudio2Path)]
        public void TestExtractConfigurationNames4()
        {
            ConfigurationParseResult results = ConfigurationParsingHelper.ExtractConfigurationNames(VisualStudio2Path);
            Assert.AreEqual(0, results.Errors.Count());
            Assert.AreEqual(1, results.RequiredModules.Count);
            Assert.AreEqual("xPSDesiredStateConfiguration", results.RequiredModules[0]);
        }

        [TestMethod]
        [TestCategory("Scenario")]
        [DeploymentItem(SHMulptiConfigurationsPath)]
        public void TestExtractConfigurationNamesMulti()
        {
            ConfigurationParseResult results = ConfigurationParsingHelper.ExtractConfigurationNames(SHMulptiConfigurationsPath);
            Assert.AreEqual(0, results.Errors.Count());
            Assert.AreEqual(3, results.RequiredModules.Count);
            Assert.AreEqual("xComputerManagement", results.RequiredModules[0]);
            Assert.AreEqual("xNetworking", results.RequiredModules[1]);
            Assert.AreEqual("xPSDesiredStateConfiguration", results.RequiredModules[2]);
        }
    }
}
