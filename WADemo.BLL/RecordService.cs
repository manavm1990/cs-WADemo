using WADemo.Core.Interfaces;
using WADemo.Core.Models;

namespace WADemo.BLL;

public class RecordService : IRecordService
{
  private readonly IRecordRepository _recordRepository;

  public RecordService(IRecordRepository recordRepository)
  {
    _recordRepository = recordRepository;
  }

  public Result<Dictionary<DateOnly, List<WeatherRecord>>> GetRecordsByRange(DateOnly startDate, DateOnly endDate)
  {
    // TODO: Consider moving this to Validation from the Console
    if (startDate > endDate)
      return new Result<Dictionary<DateOnly, List<WeatherRecord>>>
      {
        IsSuccess = false, Message = "Start date cannot be greater than end date"
      };

    // We get the records from the Data property that was set in the repository on the Result
    // TODO: Consider allowing the repo to do the filtering by the date ranges (if data size gets large enough).
    var allRecords = _recordRepository.Index().Data;

    // This empty list will be used to add the records that are in the range
    if (allRecords == null)
    {
      return new Result<Dictionary<DateOnly, List<WeatherRecord>>> {IsSuccess = false, Message = "No records found!"};
    }

    var ret = allRecords.Where(weatherRecord => weatherRecord.Date >= startDate && weatherRecord.Date <= endDate)

      // Group the records by the date
      .GroupBy(record => new DateOnly(record.Date.Year, record.Date.Month, record.Date.Day))

      // Order the records by the date, by month and then by year
      .OrderBy(record => record.Key.Month).ThenBy(record => record.Key.Year)

      // Convert the records to a dictionary using the Key (DateOnly) and assigning a value for the entire group for
      // that DateOnly
      .ToDictionary(record => record.Key, g => g.ToList());

    return ret.Count == 0
      ? new Result<Dictionary<DateOnly, List<WeatherRecord>>>
      {
        IsSuccess = false, Message = "No records found in the range!"
      }
      : new Result<Dictionary<DateOnly, List<WeatherRecord>>> {IsSuccess = true, Data = ret};
  }

  public Result<WeatherRecord> GetRecordByDate(DateOnly date)
  {
    // We get the records from the Data property that was set in the repository on the Result
    var allRecords = _recordRepository.Index().Data;

    // This empty list will be used to add the records that are in the range
    if (allRecords == null)
    {
      return new Result<WeatherRecord> {IsSuccess = false, Message = "No records found!"};
    }

    var ret = allRecords.FirstOrDefault(weatherRecord => weatherRecord.Date == date);

    return ret == null
      ? new Result<WeatherRecord>
      {
        IsSuccess = false, Message = $"No record found for the date: {date.ToString("MM/dd/yyyy")}!"
      }
      : new Result<WeatherRecord> {IsSuccess = true, Data = ret};
  }

  public Result<WeatherRecord> AddRecord(WeatherRecord newRecord)
  {
    var error = ValidateRecord(newRecord);

    // Make sure we don't duplicate records
    var existingRecords = _recordRepository.Index().Data;

    if (existingRecords != null && existingRecords.Any(record => record.Date == newRecord.Date))
    {
      return new Result<WeatherRecord>
      {
        IsSuccess = false,
        Message = "A record already exists for the date: " + newRecord.Date.ToString("MM/dd/yyyy") + "!"
      };
    }

    return string.IsNullOrEmpty(error)
      ? _recordRepository.Add(newRecord)
      : new Result<WeatherRecord> {IsSuccess = false, Message = error};
  }

  public Result<WeatherRecord> UpdateRecord(WeatherRecord record2Update)
  {
    var error = ValidateRecord(record2Update);

    return string.IsNullOrEmpty(error)
      ? _recordRepository.Update(record2Update)
      : new Result<WeatherRecord> {IsSuccess = false, Message = error};
  }

  public Result<WeatherRecord> DeleteRecord(DateOnly date2Delete)
  {
    return _recordRepository.Delete(date2Delete);
  }

  public Result<StatReport> GetStatReport(DateOnly startDate, DateOnly endDate)
  {
    var allRecordsInRange = GetRecordsByRange(startDate, endDate).Data;

    if (allRecordsInRange == null)
    {
      return new Result<StatReport> {IsSuccess = false, Message = "No records found!"};
    }

    // We just need the values from the dictionary
    var records = allRecordsInRange.Aggregate(new List<WeatherRecord>(),
      (current, record) => current.Concat(record.Value).ToList());

    var stats = new StatReport
    {
      AvgHighTemp = records.Average(record => record.HighTemp),
      MaxHighTemp = records.Max(record => record.HighTemp),
      MinHighTemp = records.Min(record => record.HighTemp),
      AvgLowTemp = records.Average(record => record.LowTemp),
      MaxLowTemp = records.Max(record => record.LowTemp),
      MinLowTemp = records.Min(record => record.LowTemp),
      AvgHumidity = records.Average(record => record.Humidity),
      MaxHumidity = records.Max(record => record.Humidity),
      MinHumidity = records.Min(record => record.Humidity)
    };

    return new Result<StatReport> {IsSuccess = true, Data = stats};
  }

  private static string ValidateRecord(WeatherRecord record)
  {
    // Validation is light b/c most of it is handled by the view validation when getting inputs.
    return record.HighTemp < record.LowTemp ? "High temp should not be less than low temp." : String.Empty;
  }
}
