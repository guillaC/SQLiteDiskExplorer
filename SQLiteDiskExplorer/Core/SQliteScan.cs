﻿using SQLiteDiskExplorer.Model;
using SQLiteDiskExplorer.Utils;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace SQLiteDiskExplorer.Core
{
    [Serializable]
    public class SQliteScan
    {
        public enum State { Waiting, Error, Canceled, Enumerating, Scanning, Done };
        public State WorkerState = State.Waiting;

        EnumerationOptions options;

        private List<string> paths = new();
        private List<FileInfo> result = new();

        private object lockObject = new object();
        private CancellationTokenSource cancellationTkn = new CancellationTokenSource();

        private int totalNbFiles, totalProcessedFiles = 0;

        public SQliteScan(DriveInfo drive)
        {
            loadConfigVar();

            Task.Run(() =>
            {
                EnumerateAndScanFiles(drive);
            });
        }

        public List<FileInfo> returnResult()
        {
            List<FileInfo> copy;
            lock (lockObject)
            {
                copy = new List<FileInfo>(result);
            }
            return copy;
        }

        public void StopScan()
        {
            cancellationTkn.Cancel();
        }

        public float GetScanProgress()
        {
            if (totalNbFiles == 0) return 0.0f;
            return (float)Math.Round((double)totalProcessedFiles / totalNbFiles, 2);
        }

        private void ScanFiles()
        {
            Console.WriteLine("Scanning...");
            WorkerState = State.Scanning;

            Console.WriteLine($"{paths.Count()} files to scan");
            Parallel.ForEach(paths, (file, parallelLoop) =>
                {
                    totalProcessedFiles++;
                    if (IsSQLiteFile(file))
                    {
                        lock (lockObject)
                        {
                            result.Add(new FileInfo(file));
                            Console.WriteLine($"{totalNbFiles} / {totalProcessedFiles}");
                        }
                    }
                    if (cancellationTkn.Token.IsCancellationRequested)
                    {
                        parallelLoop.Break();
                    }
                });

            if (cancellationTkn.Token.IsCancellationRequested)
            {
                WorkerState = State.Canceled;
                Console.WriteLine("Scanning canceled.");
            }
            else
            {
                WorkerState = State.Done;
                Console.WriteLine("All files processed.");
            }
        }

        private void loadConfigVar()
        {
            AppConfig config = ConfigurationManager.LoadConfiguration();

            options = new EnumerationOptions()
            {
                IgnoreInaccessible = config.IgnoreInaccessible,
                RecurseSubdirectories = config.RecurseSubdirectories,
            };
        }

        private void EnumerateAndScanFiles(DriveInfo drive)
        {
            Thread.Sleep(2000);
            Console.WriteLine("Enumerating...");
            WorkerState = State.Enumerating;
            try
            {
                paths = Directory.EnumerateFiles(drive.Name, "*", options).ToList();
                Console.WriteLine($"Done. {paths.Count()}");
                totalNbFiles = paths.Count();
                ScanFiles();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enumerating files: {ex.Message}");
                WorkerState = State.Error;
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
            catch (Exception ex)
            {
                Console.WriteLine(file);
                Console.WriteLine(ex.Message);
                return false;
            }

            string headerText = Encoding.UTF8.GetString(header);
            return headerText.StartsWith("SQLite format");
        }

        private SQLiteFileHeader? GetHeader(string file)
        {
            byte[] header = new byte[100];

            try
            {
                using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    stream.Read(header, 0, 100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(file);
                Console.WriteLine(ex.Message);
                return null;
            }

            return new SQLiteFileHeader(header);
        }
    }
}
