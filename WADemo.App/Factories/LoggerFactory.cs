using WADemo.Core;
using WADemo.Core.Interfaces;
using WADemo.Core.Models;

namespace WADemo.App.Factories;

internal static class LoggerFactory
{
  private const string LogFile = "log.error.csv";

  internal static ILogger GetLogger(LoggingMode mode)
  {
    return mode switch
    {
      LoggingMode.None => new NullLogger(),
      LoggingMode.Console => new ConsoleLogger(),
      LoggingMode.File => new CSVLogger(LogFile),
      _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
    };
  }
}
