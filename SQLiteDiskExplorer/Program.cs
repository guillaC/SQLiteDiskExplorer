using SQLiteDiskExplorer;

internal class Program
{
    private static void Main(string[] args)
    {
        RenderControllerClass mainForm = new RenderControllerClass();
        mainForm.Start().Wait();
    }
}