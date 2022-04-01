using Ninject;
using WADemo.BLL;
using WADemo.Core;
using WADemo.Core.Interfaces;
using WADemo.Core.Models;
using WADemo.DAL;
using WADemo.UI;

namespace WADemo.App.Factories;

internal static class ControllerFactory
{
  private const string DataDir = "../data/";
  private const string DataFile = "almanac.csv";
  private const string LogFile = "log.error.csv";

  internal static Controller GetController(ApplicationMode applicationMode, LoggingMode loggingMode)
  {
    var kernel = new StandardKernel();

    switch (loggingMode)
    {
      case LoggingMode.None:
        kernel.Bind<ILogger>().To<NullLogger>();
        break;
      case LoggingMode.Console:
        kernel.Bind<ILogger>().To<ConsoleLogger>();
        break;
      case LoggingMode.File:
        kernel.Bind<ILogger>().To<CSVLogger>().WithConstructorArgument("filePath", LogFile);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(loggingMode), loggingMode, null);
    }

    if (applicationMode == ApplicationMode.Live)
    {
      kernel.Bind<IRecordRepository>().To<CsvRecordRepository>()
        .WithConstructorArgument("filename", DataDir + DataFile);
    }
    else
    {
      kernel.Bind<IRecordRepository>().To<MockRecordRepository>();
    }

    kernel.Bind<IRecordService>().To<RecordService>();

    return kernel.Get<Controller>();
  }
}
