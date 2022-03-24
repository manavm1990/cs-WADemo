namespace WADemo.Core.Interfaces;

public interface IRecordService
{
   Result<List<WeatherRecord>> GetRecordsByRange(DateTime startDate, DateTime endDate);
   Result<WeatherRecord> GetRecordByDate(DateTime date);
   Result<WeatherRecord> AddRecord(WeatherRecord record);
   Result<WeatherRecord> UpdateRecord(WeatherRecord record);
   Result<WeatherRecord> DeleteRecord(WeatherRecord record);
}
