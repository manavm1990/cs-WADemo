using WADemo.Core.Interfaces;
using WADemo.Core.Models;

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
          Description = "Sunny 🌞",
          HighTemp = 80,
          LowTemp = 50,
          Humidity = 60,
        },
      new()
      {
        Date = DateOnly.Parse("1/2/2019"),
        Description = "Still Sunny 🌞",
        HighTemp = 70,
        LowTemp = 60,
        Humidity = 40
      },
      new()
      {
        Date = DateOnly.Parse("2/3/2019"),
        Description = "Rainy 🌧️",
        HighTemp = 90,
        LowTemp = 60,
        Humidity = 100
      },
      new()
      {
        Date = DateOnly.Parse("2/5/2019"),
        Description = "Rainy and sunny 🌦️️",
        HighTemp = 50,
        LowTemp = 40,
        Humidity = 90
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
      result.Message = $"Record for {updatedRecord.Date.ToString("MM/dd/yyyy")} updated successfully!";
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
