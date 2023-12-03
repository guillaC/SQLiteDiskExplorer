namespace SQLiteDiskExplorer.Utils
{
    public static class ReadSave
    {
        public static byte[]? ReadFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return File.ReadAllBytes(filePath);
                }
                else
                {
                    Console.WriteLine("Le fichier n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite : " + ex.Message);
            }

            return null;
        }

        public static bool CopyFile(string sourceFilePath, string destinationFilePath)
        {
            try
            {
                if (File.Exists(sourceFilePath))
                {
                    File.Copy(sourceFilePath, destinationFilePath);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite : " + ex.Message);
            }

            return false;
        }

    }
}
