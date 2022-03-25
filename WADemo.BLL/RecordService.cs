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

        // We get the records from the Data property that was set in the repository on the Result
        var allRecords = _recordRepository.Index().Data;

        // This empty list will be used to add the records that are in the range
        var ret = allRecords.Where(weatherRecord => weatherRecord.Date >= startDate && weatherRecord.Date <= endDate).ToList();

        if (ret.Count == 0)
            return new Result<List<WeatherRecord>>
                {IsSuccess = false, Message = "No records found in the range"};

        return new Result<List<WeatherRecord>> {IsSuccess = true, Data = ret};
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
