using WADemo.Core;
using WADemo.Core.Interfaces;

namespace WADemo.DAL;

public class MockRecordRepository : IRecordRepository
{
  private readonly List<WeatherRecord> _records;

  public MockRecordRepository()
  {
    // Creating a single record in our list for this mock repository
    _records = new List<WeatherRecord>
    {
      // Create
      new()

        // Set the fields for the WeatherRecord
        {
          Date = DateOnly.Parse("1/1/2019"),
          Description = "Sunny",
          HighTemp = 75,
          LowTemp = 55,
          Humidity = 60,
        }
    };
  }

  public Result<List<WeatherRecord>> Index()
  {
    // Create a new list of WeatherRecords and immediately set the Data property to our list of records
    return new Result<List<WeatherRecord>> {IsSuccess = true, Data = _records};
  }

  public Result<WeatherRecord> Add(WeatherRecord newRecord)
  {
    _records.Add(newRecord);
    return new Result<WeatherRecord> {IsSuccess = true, Message = "Record added successfully!"};
  }

  public Result<WeatherRecord> Update(WeatherRecord updatedRecord)
  {
    var result = new Result<WeatherRecord>();

    for (var i = 0; i < _records.Count; i++)
    {
      if (_records[i].Date != updatedRecord.Date)
      {
        continue;
      }

      _records[i] = updatedRecord;
      result.IsSuccess = true;
      result.Message = $"Record for {updatedRecord.Date} updated successfully!";
      return result;
    }

    result.Message = "Record not found";
    result.IsSuccess = false;
    return result;
  }

  public Result<WeatherRecord> Delete(DateOnly date)
  {
    var result = new Result<WeatherRecord>();

    // LINQ query to find the record with the matching date
    foreach (var record in _records.Where(record => record.Date == date))
    {
      _records.Remove(record);
      result.IsSuccess = true;
      return result;
    }

    result.Message = $"Record not found for date: {date.ToString("MM/dd/yyyy")}!";
    ;
    result.IsSuccess = false;
    return result;
  }
}
