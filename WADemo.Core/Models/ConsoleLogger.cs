using WADemo.Core.Interfaces;

namespace WADemo.Core.Models;

public class ConsoleLogger : ILogger
{
  public void Log(string message)
  {
    Console.WriteLine(message);
  }
}
