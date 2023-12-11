namespace SQLiteDiskExplorer.Utils
{
    public static class ReadSave
    {
        public static byte[]? ReadFile(string filePath)
        {
            try
            {
                return File.ReadAllBytes(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return null;
        }

        public static bool CopyFile(string sourceFilePath, string destinationFilePath)
        {
            try
            {
                string destinationDirectory = Path.GetDirectoryName(destinationFilePath) ?? "";
                if (string.IsNullOrEmpty(destinationDirectory)) return false;

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                File.Copy(sourceFilePath, destinationFilePath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return false;
        }
    }
}
