namespace SyncPro.Runtime
{
    public class RestoreJobProgressInfo
    {
        /// <summary>
        /// Indicates the value of progress from 0.00 to 1.00. A value of  -1.0 indicates that progress is indeterminate.
        /// </summary>
        public double ProgressValue { get; }

        /// <summary>
        /// The total number of files to be synced.
        /// </summary>
        public int FilesTotal { get; }

        /// <summary>
        /// The completed number of files to be synced.
        /// </summary>
        public int FilesCompleted { get; }

        /// <summary>
        /// The total number of bytes to be synced.
        /// </summary>
        public long BytesTotal { get; }

        /// <summary>
        /// The completed number of bytes to be synced.
        /// </summary>
        public long BytesCompleted { get; }

        public int BytesPerSecond { get; }

        public RestoreJobProgressInfo(
            int filesTotal, 
            int filesCompleted, 
            long bytesTotal, 
            long bytesCompleted, 
            int bytesPerSecond)
        {
            this.FilesTotal = filesTotal;
            this.FilesCompleted = filesCompleted;
            this.BytesTotal = bytesTotal;
            this.BytesCompleted = bytesCompleted;
            this.BytesPerSecond = bytesPerSecond;

            this.ProgressValue = (double)bytesCompleted / (double)bytesTotal;
        }
    }
}