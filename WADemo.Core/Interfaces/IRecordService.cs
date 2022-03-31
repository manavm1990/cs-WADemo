using WADemo.Core.Models;

namespace WADemo.Core.Interfaces;

public interface IRecordService
{
  Result<List<WeatherRecord>> GetRecordsByRange(DateOnly startDate, DateOnly endDate);
  Result<WeatherRecord> GetRecordByDate(DateOnly date);
  Result<WeatherRecord> AddRecord(WeatherRecord record);
  Result<WeatherRecord> UpdateRecord(WeatherRecord record);
  Result<WeatherRecord> DeleteRecord(DateOnly date);
  Result<StatReport> GetStatReport(DateOnly startDate, DateOnly endDate);
}
