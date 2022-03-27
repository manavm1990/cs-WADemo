using WADemo.Core;
using WADemo.Core.Interfaces;

namespace WADemo.BLL;

public class RecordService : IRecordService
{
  private readonly IRecordRepository _recordRepository;

  public RecordService(IRecordRepository recordRepository)
  {
    _recordRepository = recordRepository;
  }

  public Result<List<WeatherRecord>> GetRecordsByRange(DateOnly startDate, DateOnly endDate)
  {
    if (startDate > endDate)
      return new Result<List<WeatherRecord>>
      {
        IsSuccess = false, Message = "Start date cannot be greater than end date"
      };

    // We get the records from the Data property that was set in the repository on the Result
    var allRecords = _recordRepository.Index().Data;

    // This empty list will be used to add the records that are in the range
    if (allRecords == null)
    {
      return new Result<List<WeatherRecord>> {IsSuccess = false, Message = "No records found!"};
    }

    var ret = allRecords.Where(weatherRecord => weatherRecord.Date >= startDate && weatherRecord.Date
        <= endDate)
      .ToList();

    return ret.Count == 0
      ? new Result<List<WeatherRecord>> {IsSuccess = false, Message = "No records found in the range"}
      : new Result<List<WeatherRecord>> {IsSuccess = true, Data = ret};
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
      ? new Result<WeatherRecord> {IsSuccess = false, Message = "No record found for the date"}
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
        IsSuccess = false, Message = "A record already exists for the date " + newRecord.Date.ToString("yyyy-MM-dd")
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

  private static string ValidateRecord(WeatherRecord record)
  {
    // Validation is light b/c most of it is handled by the view validation when getting inputs.
    return record.HighTemp < record.LowTemp ? "High temp should not be less than low temp." : String.Empty;
  }
}
