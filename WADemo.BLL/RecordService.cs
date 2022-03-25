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
        var ret = allRecords.Where(weatherRecord => weatherRecord.Date >= startDate && weatherRecord.Date <= endDate)
            .ToList();

        if (ret.Count == 0)
            return new Result<List<WeatherRecord>>
                {IsSuccess = false, Message = "No records found in the range"};

        return new Result<List<WeatherRecord>> {IsSuccess = true, Data = ret};
    }

    public Result<WeatherRecord> GetRecordByDate(DateTime date)
    {
        // We get the records from the Data property that was set in the repository on the Result
        var allRecords = _recordRepository.Index().Data;

        // This empty list will be used to add the records that are in the range
        var ret = allRecords.FirstOrDefault(weatherRecord => weatherRecord.Date == date);

        if (ret == null)
            return new Result<WeatherRecord>
                {IsSuccess = false, Message = "No record found for the date"};

        return new Result<WeatherRecord> {IsSuccess = true, Data = ret};
    }

    public Result<WeatherRecord> AddRecord(WeatherRecord newRecord)
    {
        if (newRecord.Date > DateTime.Now)
            return new Result<WeatherRecord>
                {IsSuccess = false, Message = "Date cannot be in the future"};

        if (newRecord.HighTemp < newRecord.LowTemp)
            return new Result<WeatherRecord>
                {IsSuccess = false, Message = "High temp should not be less than low temp."};

        // TODO: Add validation for reasonable temps

        return _recordRepository.Add(newRecord);
    }

    public Result<WeatherRecord> UpdateRecord(WeatherRecord record)
    {

        return _recordRepository.Update(record);
    }

    public Result<WeatherRecord> DeleteRecord(DateTime date2Delete)
    {
        return _recordRepository.Delete(date2Delete);
    }

    // TODO: The validations here are very similar to what we did in AddRecord.
    // We should refactor this code to use the same logic.
    private bool IsValid(WeatherRecord record)
    {
        return true;
    }
}
