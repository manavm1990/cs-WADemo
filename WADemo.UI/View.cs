using WADemo.Core;

namespace WADemo.UI;

public class View
{
    private static void Display(string message)
    {
        Console.WriteLine(message);
    }

    private static void DisplayHeader(string header)
    {
        Display(header);
        Display("------------------------------------------");
    }

    private static void DisplayRecord(WeatherRecord record)
    {
        Display(@$"Date: {record.Date:MMMM dd, yyyy}
High: {record.HighTemp} 
Low: {record.LowTemp}
Humidity: {record.Humidity}%
Description: {record.Description}
");
    }

    private static void DisplayRecords(List<WeatherRecord> records)
    {
        DisplayHeader("Weather Records");
        foreach (var record in records)
        {
            DisplayRecord(record);
            Console.WriteLine("--------------");
        }
    }
}
