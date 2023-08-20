using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.Core
{
    public class SQliteScan
    {
        EnumerationOptions options = new EnumerationOptions()
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = true,
        };

        IEnumerable<string> paths = Enumerable.Empty<string>();

        List<string> result = new List<string>();

        private int remainingThreads;
        private readonly ManualResetEventSlim resetEvent = new ManualResetEventSlim(false);

        public SQliteScan(DriveInfo drive)
        {
            var paths = Directory.EnumerateFiles(drive.Name, "*", options);
            paths = paths.Where(file => !file.Contains("\\Windows\\")).ToList();
            remainingThreads = paths.Count();
        }

        public void Scan()
        {
            int maxThreads = Environment.ProcessorCount; // Number of threads

            foreach (string file in paths)
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    if (IsSQLiteFile(file))
                    {
                        lock (result)
                        {
                            result.Add(file);
                        }
                    }

                    if (Interlocked.Decrement(ref remainingThreads) == 0)
                    {
                        resetEvent.Set();
                    }
                });
            }

            resetEvent.Wait();
        }

        private bool IsSQLiteFile(string file)
        {
            byte[] header = new byte[16];

            try
            {
                using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    stream.Read(header, 0, 16);
                }
            }
            catch
            {
                return false;
            }

            string headerText = Encoding.UTF8.GetString(header);
            return headerText.StartsWith("SQLite format");
        }
    }
}
