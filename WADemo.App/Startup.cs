using WADemo.BLL;
using WADemo.DAL;
using WADemo.UI;

namespace WADemo.App;

public static class Startup
{
  internal static void Run()
  {
    View.DisplayHeader("Welcome to Weather Almanac");

    // TODO: Ask what type of repository to run (test/mock, live/file) and create the correct one.

    // var repository = new CsvRecordRepository("weather.csv");
    var repository = new MockRecordRepository();
    var service = new RecordService(repository);
    var controller = new Controller(service);
    controller.Run();
  }
}
