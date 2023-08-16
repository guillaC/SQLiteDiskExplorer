using SQLiteDiskExplorer.UI;

internal class Program
{
    private static void Main(string[] args)
    {
        MainUI mainForm = new MainUI();
        mainForm.Start().Wait();
    }
}