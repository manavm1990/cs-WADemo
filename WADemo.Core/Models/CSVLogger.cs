using WADemo.Core.Interfaces;

namespace WADemo.Core.Models;

public class CSVLogger : ILogger
{
  private readonly string _filePath;

  public CSVLogger(string filePath)
  {
    _filePath = filePath;
  }

  public void Log(string message)
  {
    using var writer = new StreamWriter(_filePath, true);
    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{message}");
    ;
  }
}
