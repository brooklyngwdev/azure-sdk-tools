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

namespace Microsoft.WindowsAzure.Commands.ScenarioTest.WebsitesTests
{
    using Common;
    using Xunit;

    public class WebsitesTests : WebsitesTestsBase
    {
        #region Remove-AzureWebsite Scenario Tests

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestRemoveAzureWebsiteWithInvalidCredentials()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-WithInvalidCredentials {Remove-AzureWebsite $(Get-WebsiteName) -Force }} 'TestRemoveAzureWebsiteWithInvalidCredentials'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestRemoveAzureServiceWithValidName()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-RemoveAzureServiceWithValidName} 'TestRemoveAzureServiceWithValidName'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestRemoveAzureServiceWithNonExistingName()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-RemoveAzureServiceWithNonExistingName} 'TestRemoveAzureServiceWithNonExistingName'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestRemoveAzureServiceWithWhatIf()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-RemoveAzureServiceWithWhatIf} 'TestRemoveAzureServiceWithWhatIf'");
        }

        #endregion

        #region Get-AzureWebsiteLog Scenario Tests

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestGetAzureWebsiteLogWithInvalidCredentials()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-WithInvalidCredentials { Get-AzureWebsiteLog -Tail -Name $(Get-WebsiteName) }} 'TestGetAzureWebsiteLogWithInvalidCredentials'");
        }

        [Fact(Skip = "TODO: Fix the failing tests.")]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestGetAzureWebsiteLogTail()
        {
            RunPowerShellTest("Test-GetAzureWebsiteLogTail");
        }

        [Fact(Skip = "TODO: Fix the failing tests.")]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestGetAzureWebsiteLogTailPath()
        {
            RunPowerShellTest("Test-GetAzureWebsiteLogTailPath");
        }

        [Fact(Skip = "TODO: Fix the failing tests.")]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestGetAzureWebsiteLogTailUriEncoding()
        {
            RunPowerShellTest("Test-GetAzureWebsiteLogTailUriEncoding");
        }

        [Fact(Skip = "TODO: Fix the failing tests.")]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestGetAzureWebsiteLogListPath()
        {
            RunPowerShellTest("Test-GetAzureWebsiteLogListPath");
        }

        #endregion

        #region Get-AzureWebsite Scenario Tests

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestGetAzureWebsite()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-GetAzureWebsite} 'TestGetAzureWebsite'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestGetAzureWebsiteWithStoppedSite()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-GetAzureWebsiteWithStoppedSite} 'TestGetAzureWebsiteWithStoppedSite'");
        }

        #endregion

        #region Start-AzureWebsite Scenario Tests

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestStartAzureWebsite()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-StartAzureWebsite} 'TestStartAzureWebsite'");
        }

        #endregion

        #region Stop-AzureWebsite Scenario Tests

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestStopAzureWebsite()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-StopAzureWebsite} 'TestStopAzureWebsite'");
        }

        #endregion

        #region Restart-AzureWebsite Scenario Tests

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestRestartAzureWebsite()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-RestartAzureWebsite} 'TestRestartAzureWebsite'");
        }

        #endregion

        #region Enable-AzureWebsiteApplicationDiagnostic Scenario Tests

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        public void TestEnableApplicationDiagnosticOnTableStorage()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-EnableApplicationDiagnosticOnTableStorage} 'TestEnableApplicationDiagnosticOnTableStorage'");
        }

        [Fact]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestEnableApplicationDiagnosticOnFileSystem()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-EnableApplicationDiagnosticOnFileSystem} 'TestEnableApplicationDiagnosticOnFileSystem'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestUpdateTheDiagnositicLogLevel()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-UpdateTheDiagnositicLogLevel} 'TestUpdateTheDiagnositicLogLevel'");
        }
        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        public void TestReconfigureStorageAppDiagnostics()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-ReconfigureStorageAppDiagnostics} 'TestReconfigureStorageAppDiagnostics'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        public void TestThrowsForInvalidStorageAccountName()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-ThrowsForInvalidStorageAccountName} 'TestThrowsForInvalidStorageAccountName'");
        }

        #endregion

        #region Disable-AzureWebsiteApplicationDiagnostic Scenario Tests

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        public void TestDisableApplicationDiagnosticOnTableStorage()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-DisableApplicationDiagnosticOnTableStorage} 'TestDisableApplicationDiagnosticOnTableStorage'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestDisableApplicationDiagnosticOnFileSystem()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-DisableApplicationDiagnosticOnFileSystem} 'TestDisableApplicationDiagnosticOnFileSystem'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        public void TestDisableApplicationDiagnosticOnTableStorageAndFile()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-DisableApplicationDiagnosticOnTableStorageAndFile} 'TestDisableApplicationDiagnosticOnTableStorageAndFile'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        public void TestDisablesFileOnly()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-DisablesFileOnly} 'TestDisablesFileOnly'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        public void TestDisablesStorageOnly()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-DisablesStorageOnly} 'TestDisablesStorageOnly'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        public void TestDisablesBothByDefault()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-DisablesBothByDefault} 'TestDisablesBothByDefault'");
        }

        #endregion

        #region Get-AzureWebsiteLocation Scenario Tests

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestGetAzureWebsiteLocation()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-GetAzureWebsiteLocation} 'TestGetAzureWebsiteLocation'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestKuduAppsExpressApp()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-KuduAppsExpressApp} 'TestKuduAppsExpressApp'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestGetAzureWebSiteListNone()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-GetAzureWebSiteListNone} 'TestGetAzureWebSiteListNone'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestAzureWebSiteListAll()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-AzureWebSiteListAll} 'TestAzureWebSiteListAll'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestAzureWebSiteShowSingleSite()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-AzureWebSiteShowSingleSite} 'TestAzureWebSiteShowSingleSite'");
        }

        #endregion

        #region AzureWebSiteGitHubAllParms Scenario Tests

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestNewAzureWebSiteMultipleCreds()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-NewAzureWebSiteMultipleCreds} 'TestNewAzureWebSiteMultipleCreds'");
        }

        [Fact (Skip = "TODO: Fix failing test.")]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestNewAzureWebSiteGitHubAllParms()
        {
            RunPowerShellTest("Test-NewAzureWebSiteGitHubAllParms");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestNewAzureWebSiteUpdateGit()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-NewAzureWebSiteUpdateGit} 'TestNewAzureWebSiteUpdateGit'");
        }
        #endregion

        #region Set-AzureWebSite Scenario Tests

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        [Trait(Category.Environment, Category.WAPack)]
        public void TestSetAzureWebsite()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-SetAzureWebsite} 'TestSetAzureWebsite'");
        }

        #endregion

        #region WebJob Scenario Tests

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        public void TestRemoveAzureWebsiteTriggeredJob()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-RemoveAzureWebsiteTriggeredJob} 'TestRemoveAzureWebsiteTriggeredJob'");
        }

        [Fact (Skip = "TODO: Fix failing test.")]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        public void TestRemoveAzureWebsiteContinuousJob()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-RemoveAzureWebsiteContinuousJob} 'TestRemoveAzureWebsiteContinuousJob'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        public void TestRemoveNonExistingAzureWebsiteJob()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-RemoveNonExistingAzureWebsiteJob} 'TestRemoveNonExistingAzureWebsiteJob'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        public void TestStartAzureWebsiteTriggeredJob()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-StartAzureWebsiteTriggeredJob} 'TestStartAzureWebsiteTriggeredJob'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        public void TestStartAndStopAzureWebsiteContinuousJob()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-StartAndStopAzureWebsiteContinuousJob} 'TestStartAndStopAzureWebsiteContinuousJob'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        public void GettingWebsiteJobs()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-GettingWebsiteJobs} 'GettingWebsiteJobs'");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.BVT)]
        [Trait(Category.Service, Category.Websites)]
        public void TestGetsJobHistory()
        {
            RunPowerShellTest("Run-WebsiteTest {Test-GettingJobHistory} 'TestGetsJobHistory'");
        }

        #endregion
    }
}
