using WADemo.BLL;
using WADemo.Core;
using WADemo.Core.Interfaces;
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

    IRecordRepository repository;
    View.DisplayHeader("Welcome to Weather Almanac");

    switch ((ApplicationMode)View.GetApplicationMode())
    {
      case ApplicationMode.Live:
        Directory.CreateDirectory(dataDir);
        repository = new CsvRecordRepository(dataDir + dataFile);
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
