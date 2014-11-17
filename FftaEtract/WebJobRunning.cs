namespace FftaExtract
{
    using System;
    using System.IO;

    public class WebJobRunning
    {
        private string shutdownFile;

        public bool IsRunning { get; private set; }

        public WebJobRunning()
        {
            this.IsRunning = true;

            // Get the shutdown file path from the environment
            this.shutdownFile = Environment.GetEnvironmentVariable("WEBJOBS_SHUTDOWN_FILE")
                                ?? new FileInfo("stop.txt").FullName;

            // Setup a file system watcher on that file's directory to know when the file is created
            var fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(this.shutdownFile));
            fileSystemWatcher.Created += this.OnChanged;
            fileSystemWatcher.Changed += this.OnChanged;
            fileSystemWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite;
            fileSystemWatcher.IncludeSubdirectories = false;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.IndexOf(Path.GetFileName(this.shutdownFile), StringComparison.OrdinalIgnoreCase) >= 0)
            {
                // Found the file mark this WebJob as finished
                this.IsRunning = false;
            }
        }
    }
}