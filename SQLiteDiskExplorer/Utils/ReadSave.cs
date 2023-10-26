namespace SQLiteDiskExplorer.Utils
{
    public static class ReadSave
    {
        public static byte[] ReadFile(string filePath)
        {
            try
            {
                // Vérifiez si le fichier existe
                if (File.Exists(filePath))
                {
                    // Lisez le contenu du fichier en bytes
                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    return fileBytes;
                }
                else
                {
                    Console.WriteLine("Le fichier n'existe pas.");
                    return null; // Ou vous pouvez choisir de lever une exception ici
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite : " + ex.Message);
                return null; // Ou vous pouvez choisir de lever une exception ici
            }
        }
    }
}
