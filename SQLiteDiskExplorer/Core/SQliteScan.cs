﻿using SQLiteDiskExplorer.Model;
using SQLiteDiskExplorer.Utils;
using System.Text;

namespace SQLiteDiskExplorer.Core
{
    public class SQliteScan
    {
        public enum State { Waiting, Error, Canceled, Enumerating, Scanning, Done };
        public State WorkerState = State.Waiting;

        EnumerationOptions options;
        AppConfig config;

        private List<string> paths = new();
        private readonly List<FileItem> result = new();

        private readonly object lockObject = new object();
        private readonly CancellationTokenSource cancellationTkn = new CancellationTokenSource();

        private int totalNbFiles, totalProcessedFiles = 0;

        public SQliteScan(DriveInfo drive)
        {
            LoadConfigVar();

            Task.Run(() =>
            {
                EnumerateAndScanFiles(drive);
            });
        }

        public List<FileItem> ReturnResult()
        {
            List<FileItem> copy;
            lock (lockObject)
            {
                copy = new List<FileItem>(result);
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
                        FileItem fItem = new(new FileInfo(file), GetHeader(file));
                        fItem.ColumnKeywordIsPresence = (config.CheckColumnKeywordPresence && IdentifyTermInColumn(fItem));

                        lock (lockObject)
                        {
                            result.Add(fItem);
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

        private void LoadConfigVar()
        {
            config = ConfigurationManager.LoadConfiguration();

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
                if (options.RecurseSubdirectories && options.IgnoreInaccessible)
                {
                    Console.WriteLine(SearchOption.AllDirectories);
                    paths = CustomEnumerateFiles(drive.Name);
                }
                else
                {
                    Console.WriteLine(options.ToString());
                    paths = Directory.EnumerateFiles(drive.Name, "*", options).ToList();
                }
                Console.WriteLine($"Done. {paths.Count}");
                totalNbFiles = paths.Count;
                ScanFiles();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enumerating files: {ex.Message}");
                WorkerState = State.Error;
            }
        }

        /// <summary>
        /// Impossible d'utiliser SearchOption.AllDirectories et EnumerationOptions.IgnoreInaccessible en même temps dans la méthode Directory.EnumerateFiles de .NET. 
        /// De fait, obliger de faire de faire sa propre méthode d'enum de fichier pour pouvoir accéder aux répertoires spéciaux (comme APPDATA exclut par défaut de la méthode Directory.EnumerateFiles.
        /// </summary>
        static List<string> CustomEnumerateFiles(string directory)
        {
            List<string> fileList = new List<string>();

            try
            {
                IEnumerable<string> files = Directory.EnumerateFiles(directory, "*.*");
                fileList.AddRange(files);

                IEnumerable<string> subDirectories = Directory.EnumerateDirectories(directory);
                foreach (string subDir in subDirectories)
                {
                    List<string> subdirectoryFiles = CustomEnumerateFiles(subDir);
                    fileList.AddRange(subdirectoryFiles);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return fileList;
        }

        private static bool IsSQLiteFile(string file)
        {
            byte[] header = new byte[16];

            try
            {
                using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    stream.Read(header, 0, 16);
                }
                string headerText = Encoding.UTF8.GetString(header);
                return headerText.StartsWith("SQLite format");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} ");
            }
            return false;
        }

        private static SQLiteFileHeader? GetHeader(string file)
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

        private static bool IdentifyTermInColumn(FileItem fItem)
        {
            //TODO
            return false;
        }
    }
}
