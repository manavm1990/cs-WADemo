// The repository will actually update our data

namespace WADemo.Core.Interfaces;

public interface IRecordRepository
{
  Result<List<WeatherRecord>> Index();
  Result<WeatherRecord> Add(WeatherRecord record);
  Result<WeatherRecord> Update(WeatherRecord record);
  Result<WeatherRecord> Delete(DateTime date);
}
