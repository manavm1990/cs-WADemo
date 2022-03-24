using System.ComponentModel.Design;
using WADemo.UI;

namespace WADemo.App;

public static class Startup
{
    internal static void Run()
    {
        View.DisplayHeader("Welcome to Weather Almanac");

        // TODO: Create a instance of a Service (BLL)
        // TODO: Inject the Service into the Controller
        var controller = new Controller();
        Controller.Run();
    }
}
