using WADemo.Core;
using WADemo.Core.Interfaces;

namespace WADemo.DAL;

public class CsvRecordRepository : IRecordRepository
{
  private readonly List<WeatherRecord> _records;
  private readonly string _fileName;

  public CsvRecordRepository(string fileName)
  {
    _fileName = fileName;
    _records = new List<WeatherRecord>();
    Init();
  }

  // TODO: Implement this method directly from the interface (I think).
  public Result<List<WeatherRecord>> Index()
  {
    return new Result<List<WeatherRecord>> {Data = _records};
  }

  public Result<WeatherRecord> Add(WeatherRecord record)
  {
    _records.Add(record);
    SaveAllRecords2File();
    return new Result<WeatherRecord> {Data = record};
  }

  public Result<WeatherRecord> Update(WeatherRecord record2Update)
  {
    var result = new Result<WeatherRecord>();

    for (var i = 0; i < _records.Count; i++)
    {
      if (_records[i].Date != record2Update.Date) continue;

      _records[i] = record2Update;
      SaveAllRecords2File();
      result.Data = record2Update;
      return result;
    }

    result.IsSuccess = false;
    result.Message = "Record not found";
    return result;
  }

  public Result<WeatherRecord> Delete(DateTime date)
  {
    var result = new Result<WeatherRecord>();

    // TODO: Refactor to use foreach.
    for (var i = 0; i < _records.Count; i++)
    {
      if (_records[i].Date != date) continue;

      _records.RemoveAt(i);
      SaveAllRecords2File();
      result.Data = new WeatherRecord {Date = date};
      return result;
    }

    result.IsSuccess = false;
    result.Message = "Record not found";
    return result;
  }

  private void Init()
  {
    if (!File.Exists(_fileName))
    {
      File.Create(_fileName).Close();
      return;
    }

    using (var sr = new StreamReader(_fileName))
    {
      string row = null;
      while ((row = sr.ReadLine()) != null)
      {
        _records.Add(Deserialize(row));
      }
    }
  }

  private WeatherRecord Deserialize(string row)
  {
    var record = new WeatherRecord();
    var values = row.Split(',');
    record.Date = DateTime.Parse(values[0]);

    // TODO: The description might also have commas, so we have to deal with that
    record.Description = values[1];
    record.HighTemp = int.Parse(values[2]);
    record.Humidity = int.Parse(values[3]);
    record.LowTemp = int.Parse(values[4]);

    return record;
  }

  private void SaveAllRecords2File()
  {
    using (var sw = new StreamWriter(_fileName))

      // Go over all of the WeatherRecords in _records and serialize them to a string.
      foreach (var record in _records)
      {
        sw.WriteLine(
          $"{record.Date},{record.Description},{record.HighTemp},{record.Humidity},{record.LowTemp}");
        ;
      }
  }
}
