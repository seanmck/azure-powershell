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

namespace Microsoft.Azure.Commands.DataShare.SynchronizationSetting
{
    using System;
    using System.Management.Automation;
    using Microsoft.Azure.Commands.DataShare.Common;
    using Microsoft.Azure.Commands.DataShare.Helpers;
    using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
    using Microsoft.Azure.Management.DataShare;
    using Microsoft.Azure.Management.DataShare.Models;
    using Microsoft.Azure.Management.Internal.Resources.Utilities.Models;
    using Microsoft.Azure.PowerShell.Cmdlets.DataShare.Extensions;
    using Microsoft.Azure.PowerShell.Cmdlets.DataShare.Models;
 
    /// <summary>
    /// Defines the New-AzDataShareSynchronizationSetting cmdlet.
    /// </summary>
    [Cmdlet("New",
        ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "DataShareSynchronizationSetting", DefaultParameterSetName = ParameterSetNames.FieldsParameterSet),
        OutputType(typeof(PSSynchronizationSetting))]
    public class NewAzDataShareSynchronizationSetting : AzureDataShareCmdletBase
    {
        /// <summary>
        /// The resource group name of azure data share account.
        /// </summary>
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Resource group name of azure data share account",
            ParameterSetName = ParameterSetNames.FieldsParameterSet)]
        [ValidateNotNullOrEmpty]
        [ResourceGroupCompleter()]
        public string ResourceGroupName { get; set; }

        /// <summary>
        /// Name of azure data share account.
        /// </summary>
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Azure data share account name",
            ParameterSetName = ParameterSetNames.FieldsParameterSet)]
        [ValidateNotNullOrEmpty]
        public string AccountName { get; set; }

        /// <summary>
        /// Name of azure data share
        /// </summary>
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Azure data share name",
            ParameterSetName = ParameterSetNames.FieldsParameterSet)]
        [ValidateNotNullOrEmpty]
        public string ShareName { get; set; }

        /// <summary>
        /// Name of synchronization settings to add
        /// </summary>
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Synchronization setting name",
            ParameterSetName = ParameterSetNames.FieldsParameterSet)]
        [ValidateNotNullOrEmpty]
        [ResourceNameCompleter(ResourceTypes.SynchronizationSetting, "ResourceGroupName", "AccountName", "ShareName")]
        public string Name { get; set; }

        /// <summary>
        /// Interval at which to synchronize the azure data share
        /// </summary>
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The recurrence interval for the synchronization setting (Day or Hour)",
            ParameterSetName = ParameterSetNames.FieldsParameterSet)]
        [ValidateNotNullOrEmpty]
        [PSArgumentCompleter("Hour", "Day")]
        public string RecurrenceInterval { get; set; }

        /// <summary>
        /// The time of the first synchronization
        /// </summary>
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The start time of the scheduled synchronization setting",
            ParameterSetName = ParameterSetNames.FieldsParameterSet)]
        [ValidateNotNullOrEmpty]
        public DateTime SynchronizationTime { get; set; }

        public override void ExecuteCmdlet()
        {
            if (this.ShouldProcess(this.Name, "Create"))
            {
                var setting = new ScheduledSynchronizationSetting(
                    recurrenceInterval: this.RecurrenceInterval,
                    synchronizationTime: this.SynchronizationTime);

                var synchronizationSetting =
                    this.DataShareManagementClient.SynchronizationSettings.Create(
                        resourceGroupName: this.ResourceGroupName,
                        accountName: this.AccountName,
                        shareName: this.ShareName,
                        synchronizationSettingName: this.Name,
                        synchronizationSetting: setting
                    ) as ScheduledSynchronizationSetting;
                this.WriteObject(synchronizationSetting.ToPsObject());
            }
        }
    }
}
