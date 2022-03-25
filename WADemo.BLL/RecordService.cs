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
        throw new NotImplementedException();
    }

    public Result<WeatherRecord> GetRecordByDate(DateTime date)
    {
        throw new NotImplementedException();
    }

    public Result<WeatherRecord> AddRecord(WeatherRecord record)
    {
        throw new NotImplementedException();
    }

    public Result<WeatherRecord> UpdateRecord(WeatherRecord record)
    {
        throw new NotImplementedException();
    }

    public Result<WeatherRecord> DeleteRecord(WeatherRecord record)
    {
        throw new NotImplementedException();
    }
}
