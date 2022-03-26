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

  public Result<List<WeatherRecord>> GetRecordsByRange(DateTime startDate, DateTime endDate)
  {
    if (startDate > endDate)
      return new Result<List<WeatherRecord>>
      {
        IsSuccess = false, Message = "Start date cannot be greater than end date"
      };

    // We get the records from the Data property that was set in the repository on the Result
    var allRecords = _recordRepository.Index().Data;

    // This empty list will be used to add the records that are in the range
    var ret = allRecords.Where(weatherRecord => weatherRecord.Date >= startDate && weatherRecord.Date <= endDate)
      .ToList();

    return ret.Count == 0 ? new Result<List<WeatherRecord>> {IsSuccess = false, Message = "No records found in the range"} : new Result<List<WeatherRecord>> {IsSuccess = true, Data = ret};
  }

  public Result<WeatherRecord> GetRecordByDate(DateTime date)
  {
    // We get the records from the Data property that was set in the repository on the Result
    var allRecords = _recordRepository.Index().Data;

    // This empty list will be used to add the records that are in the range
    var ret = allRecords.FirstOrDefault(weatherRecord => weatherRecord.Date == date);

    return ret == null ? new Result<WeatherRecord> {IsSuccess = false, Message = "No record found for the date"} : new Result<WeatherRecord> {IsSuccess = true, Data = ret};
  }

  public Result<WeatherRecord> AddRecord(WeatherRecord newRecord)
  {
    var error = ValidateRecord(newRecord);

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

  public Result<WeatherRecord> DeleteRecord(DateTime date2Delete)
  {
    return _recordRepository.Delete(date2Delete);
  }

  private static string ValidateRecord(WeatherRecord record)
  {
    if (record.Date > DateTime.Now)
      return "Date cannot be in the future";

    if (record.HighTemp < record.LowTemp)
      return "High temp should not be less than low temp.";

    if (!(record.HighTemp <= 140 && record.LowTemp >= -50))
      return "Temperature range must be between -150 to 150";

    return record.Humidity is not (<= 100 and >= 0) ? "Humidity must be between 0 and 100" : String.Empty;
  }
}
