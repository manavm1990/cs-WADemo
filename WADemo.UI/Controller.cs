namespace WADemo.UI;

public class Controller
{
    // TODO: Add a private property for the service
    public static void Run()
    {
        var isRunning = true;

        while (isRunning)
        {
            var choice = (MenuChoice) View.GetMainChoice();

            switch (choice)
            {
                case MenuChoice.ViewRecord:
                    Console.WriteLine("Viewing record");
                    break;
                case MenuChoice.ViewRecords:
                    Console.WriteLine("Viewing records");
                    break;
                case MenuChoice.AddRecord:
                    Console.WriteLine("Adding record");
                    break;
                case MenuChoice.EditRecord:
                    Console.WriteLine("Editing record");
                    break;
                case MenuChoice.DeleteRecord:
                    Console.WriteLine("Deleting record");
                    break;
                case MenuChoice.Exit:
                    isRunning = false;
                    break;
                default:
                    View.Display("Invalid choice");
                    break;
            }
        }
    }
}
