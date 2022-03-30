using WADemo.BLL;
using WADemo.Core;
using WADemo.Core.Interfaces;
using WADemo.Core.Models;
using WADemo.DAL;
using WADemo.UI;

namespace WADemo.App;

public static class Startup
{
  internal static void Run()
  {
    // Update this as needed to match your project
    // ⚠️ Can't use var with const - have to specify type explicitly
    const string dataDir = "../data/";
    const string dataFile = "almanac.csv";
    const string logFile = "log.error.csv";

    ILogger logger;
    IRecordRepository repository;
    View.DisplayHeader("Welcome to Weather Almanac");

    switch ((LoggingMode)View.GetLoggingMode())
    {
      case LoggingMode.None:
        logger = new NullLogger();
        break;
      case LoggingMode.Console:
        logger = new ConsoleLogger();
        break;
      case LoggingMode.File:
        logger = new CSVLogger(logFile);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }

    switch ((ApplicationMode)View.GetApplicationMode())
    {
      case ApplicationMode.Live:
        // TODO: Consider using factories to separate concerns and not have this Startup do all this stuff
        Directory.CreateDirectory(dataDir);
        repository = new CsvRecordRepository(dataDir + dataFile, logger);
        break;
      case ApplicationMode.Test:
        repository = new MockRecordRepository();
        break;

      // Don't really need this as View is validating the input for 1 or 2.
      default:
        throw new ArgumentOutOfRangeException();
    }

    var service = new RecordService(repository);
    var controller = new Controller(service);
    controller.Run();
  }
}
