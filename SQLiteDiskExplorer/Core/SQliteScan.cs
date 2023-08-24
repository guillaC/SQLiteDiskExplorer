using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.Core
{
    public class SQliteScan
    {
        EnumerationOptions options = new EnumerationOptions()
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = true, // for testing only
        };

        IEnumerable<string> paths = Enumerable.Empty<string>();

        List<FileInfo> result = new List<FileInfo>();

        private int remainingThreads;
        private readonly ManualResetEventSlim resetEvent = new ManualResetEventSlim(false);

        public SQliteScan(DriveInfo drive)
        {
            remainingThreads = paths.Count();
            Task.Run(() => Enumerate(drive));
        }

        public List<FileInfo> returnResult()
        {
            // ici lock, utiliser un object autre à lock
            return new List<FileInfo>(result);
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
                            result.Add(new FileInfo(file));
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

        private void Enumerate(DriveInfo drive)
        {
            SearchOption searchOption = SearchOption.TopDirectoryOnly;
            if (options.RecurseSubdirectories) searchOption = SearchOption.AllDirectories;

            Console.WriteLine("Enumerating..");

            try
            {
                var paths = Directory.EnumerateFiles(drive.Name, "*", searchOption);
                foreach (string path in paths)// Only For Testing
                {
                    Console.WriteLine(path);
                    result.Add(new FileInfo(path));
                }
                Console.WriteLine("Enumerate OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enumerating files: {ex.Message}");
            }
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
