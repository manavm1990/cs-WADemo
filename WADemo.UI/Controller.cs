using WADemo.Core.Interfaces;

namespace WADemo.UI;

public class Controller
{
    private readonly IRecordService _recordService;

    public Controller(IRecordService recordService)
    {
        _recordService = recordService;
    }

    public void Run()
    {
        var isRunning = true;

        while (isRunning)
        {
            var choice = (MenuChoice) View.GetMainChoice();

            switch (choice)
            {
                case MenuChoice.ViewRecord:
                    ViewRecord();
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

    private void ViewRecord()
    {
        var date2Lookup = View.GetWeatherDate();
        var record = _recordService.GetRecordByDate(date2Lookup);

        if (record.IsSuccess)
        {
            View.DisplayRecord(record.Data);
        }
        else
        {
            View.Display(record.Message);
        }
    }
}
