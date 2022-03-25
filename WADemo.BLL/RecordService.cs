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
                {IsSuccess = false, Message = "Start date cannot be greater than end date"};

        // TODO: If the dates are 🆗, get the records from the repository
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
