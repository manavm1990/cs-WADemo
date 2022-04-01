using WADemo.App.Factories;
using WADemo.Core;
using WADemo.UI;

namespace WADemo.App;

public static class Startup
{
  internal static void Run()
  {
    var controller = new Controller(ServiceFactory.GetRecordService((ApplicationMode)View.GetApplicationMode(),
      (LoggingMode)View.GetLoggingMode()));
    controller.Run();
  }
}
