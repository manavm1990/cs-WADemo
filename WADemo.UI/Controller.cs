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
      var choice = (MenuChoice)View.GetMainChoice();

      switch (choice)
      {
        case MenuChoice.ViewRecord:
          ViewRecord();
          break;
        case MenuChoice.ViewRecords:
          ViewRecords();
          break;
        case MenuChoice.AddRecord:
          AddRecord();
          break;
        case MenuChoice.EditRecord:
          EditRecord();
          break;
        case MenuChoice.DeleteRecord:
          DeleteRecord();
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
      if (record.Data != null)
      {
        View.DisplayRecord(record.Data);
      }
    }
    else
    {
      View.Display(record.Message);
    }
  }

  private void ViewRecords()
  {
    var startDate = View.GetWeatherDate("Enter start date");
    var endDate = View.GetWeatherDate("Enter end date");

    var records = _recordService.GetRecordsByRange(startDate, endDate);
    if (records.IsSuccess)
    {
      // We know that there is Data if isSuccess
      View.DisplayRecords(records.Data!);
    }
    else
    {
      View.Display(records.Message);
    }
  }

  private void AddRecord()
  {
    var newRecord = View.AddWeatherRecord();
    var result = _recordService.AddRecord(newRecord);

    if (!result.IsSuccess)
    {
      // Display error message
      var message = result.Message;
      View.Display(!string.IsNullOrEmpty(message) ? message : "Error updating record!");
    }
    else
    {
      View.Display($"Record for {newRecord.Date} added successfully!");
    }
  }

  private void EditRecord()
  {
    var date2Lookup = View.GetWeatherDate();

    // Assert that this record does have Data! 👇🏾
    var record2Update = _recordService.GetRecordByDate(date2Lookup);

    if (!record2Update.IsSuccess)
    {
      // Display error message
      var message = record2Update.Message;
      View.Display(string.IsNullOrEmpty(message) ? "Error updating record!" : message);
    }
    else
    {
      var updatedRecord = View.UpdateWeatherRecord(record2Update.Data!);
      var updatedResult = _recordService.UpdateRecord(updatedRecord);

      if (!updatedResult.IsSuccess)
      {
        var message = updatedResult.Message;
        View.Display(string.IsNullOrEmpty(message) ? "Error updating record!" : message);
      }
      else
        View.Display($"Record for {record2Update.Data!.Date} updated");
    }
  }

  private void DeleteRecord()
  {
    var date2Lookup = View.GetWeatherDate();
    var record2Delete = _recordService.GetRecordByDate(date2Lookup);

    if (record2Delete.IsSuccess)
    {
      View.DisplayRecord(record2Delete.Data!);
      if (!View.Confirm("Are you sure you want to delete this record?"))
      {
        return;
      }

      var deleteResult = _recordService.DeleteRecord(date2Lookup);
      View.Display(deleteResult.IsSuccess ? $"Record for {record2Delete.Data!.Date} deleted" : deleteResult.Message);
    }

    // Probably not found
    else
    {
      var message = record2Delete.Message;
      View.Display(string.IsNullOrEmpty(message) ? "Error deleting record!" : message);
    }
  }
}
