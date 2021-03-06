namespace SyncPro.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SyncPro.Adapters;
    using SyncPro.Tracing;

    public class AnalyzeRelationshipResult
    {
        /// <summary>
        /// Contains the mapping of AdapterId -> AnalyzeAdapterResult.
        /// </summary>
        /// <remarks>
        /// Each adapter that is analyzed will have an entry in this dictionary. The key is the configuration
        /// ID of the adapter, and the value contains the results analyzing the items exposed by that adapter.
        /// </remarks>
        public Dictionary<int, AnalyzeAdapterResult> AdapterResults { get; }

        /// <summary>
        /// Indicates whether the analyze process ran to completion.
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Gets or sets the job sync state
        /// </summary>
        public JobResult SyncJobResult { get; set; }

        /// <summary>
        /// Indicates that there are not changed needed for any of the adapters in this sync relationship.
        /// </summary>
        public bool IsUpToDate
        {
            get { return this.AdapterResults.All(r => !r.Value.EntryResults.Any()); }
        }

        /// <summary>
        /// The total number of sync entries known to this relationship.
        /// </summary>
        public int TotalSyncEntries { get; set; }

        public int TotalChangedEntriesCount => this.TotalSyncEntries - (this.UnchangedFileCount + this.UnchangedFolderCount);

        /// <summary>
        /// The number of unchanged files
        /// </summary>
        public int UnchangedFileCount { get; set; }

        /// <summary>
        /// The number of unchanged files
        /// </summary>
        public long UnchangedFileBytes { get; set; }

        /// <summary>
        /// The number of unchanged folders
        /// </summary>
        public int UnchangedFolderCount { get; set; }

        public Guid Id { get; }

        public AnalyzeRelationshipResult()
        {
            this.Id = Guid.NewGuid();
            this.AdapterResults = new Dictionary<int, AnalyzeAdapterResult>();

            this.SyncJobResult = JobResult.Undefined;
        }

        public async Task CommitTrackedChangesAsync(SyncRelationship relationship)
        {
            foreach (KeyValuePair<int, AnalyzeAdapterResult> adapterResult in AdapterResults)
            {
                if (adapterResult.Value.TrackedChanges != null)
                {
                    AdapterBase adapterBase = 
                        relationship.Adapters.First(a => a.Configuration.Id == adapterResult.Key);

                    Logger.Info("Committing tracked change for adapter {0}", adapterBase.Configuration.Id);

                    IChangeTracking changeTracking = (IChangeTracking)adapterBase;
                    await changeTracking.CommitChangesAsync(adapterResult.Value.TrackedChanges).ConfigureAwait(false);

                }
            }
        }
    }
}