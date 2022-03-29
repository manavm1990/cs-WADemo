using WADemo.Core.Interfaces;

namespace WADemo.Core.Models;

public class NullLogger : ILogger
{
  public void Log(string message)
  {
  }
}
