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
    using Management.Network.Models;
    using Utilities.Common;

    [Cmdlet(VerbsLifecycle.Stop, "AzureVNetGatewayDiagnostics"), OutputType(typeof(ManagementOperationContext))]
    public class StopAzureVnetGatewayDiagnostics : ServiceManagementBaseCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "Virtual network name.")]
        public string VirtualNetworkName { get; set; }

        protected override void OnProcessRecord()
        {
            UpdateGatewayPublicDiagnostics parameters = new UpdateGatewayPublicDiagnostics()
            {
                Operation = UpdateGatewayPublicDiagnosticsOperation.StopDiagnostics,
            };

            ServiceManagementProfile.Initialize();
            ExecuteClientActionNewSM(
                null,
                this.CommandRuntime.ToString(),
                () => this.NetworkClient.Gateways.UpdateDiagnostics(VirtualNetworkName, parameters));
        }
    }
}
