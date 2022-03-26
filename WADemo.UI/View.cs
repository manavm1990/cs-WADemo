using WADemo.Core;

namespace WADemo.UI;

public class View
{
  internal static void Display(string message)
  {
    Console.WriteLine(message);
  }

  public static void DisplayHeader(string header)
  {
    Display(header);
    Display("------------------------------------------");
  }

  internal static void DisplayRecord(WeatherRecord record)
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

  internal static int GetMainChoice()
  {
    DisplayHeader("Main Menu");

    return Validation.PromptUser4Int(@"
1. Load a Record
2. View Records by Date Range
3. Add Record
4. Edit Record
5. Delete Record
6. Quit

Enter Choice: 
", 1, 6);
  }

  private static int GetMode()
  {
    DisplayHeader("Welcome to the Weather Almanac. Please select an option:");
    return Validation.PromptUser4Int(@"What mode would you like to run in?

1. Live
2. Test

Select mode:", 1, 2);
  }

  // TODO: Consider reusing this method to also get date ranges for start and end dates
  internal static DateTime GetWeatherDate(string message = "Enter Record Date:")
  {
    return Validation.PromptUser4Date(message);
  }

  internal static WeatherRecord AddWeatherRecord()
  {
    var date = Validation.PromptUser4Date("Date (MM/dd/yyyy): ");
    var high = Validation.PromptUser4Decimal("High (F): ");
    var low = Validation.PromptUser4Decimal("Low (F): ");
    var humidity = Validation.PromptUser4Decimal("Humidity (%): ");
    var description = Validation.PromptRequired("Description: ");

    return new WeatherRecord
    {
      Date = date,
      HighTemp = high,
      LowTemp = low,
      Humidity = humidity,
      Description = description
    };
  }

  internal static WeatherRecord UpdateWeatherRecord(WeatherRecord originalRecord)
  {
    var updatedRecord = new WeatherRecord {Date = originalRecord.Date};

    var newHigh = Validation.PromptUser($"High {originalRecord.HighTemp} (F): ");
    if (string.IsNullOrEmpty(newHigh)) updatedRecord.HighTemp = originalRecord.HighTemp;
    else
    {
      if (!decimal.TryParse(newHigh, out var newHighTemp))
      {
        updatedRecord.HighTemp = originalRecord.HighTemp;
      }

      updatedRecord.HighTemp = newHighTemp;
    }

    var newLow = Validation.PromptUser($"Low {originalRecord.LowTemp} (F): ");
    if (string.IsNullOrEmpty(newLow)) updatedRecord.LowTemp = originalRecord.LowTemp;
    else
    {
      if (!decimal.TryParse(newLow, out var newLowTemp))
      {
        updatedRecord.LowTemp = originalRecord.LowTemp;
      }

      updatedRecord.LowTemp = newLowTemp;
    }

    var newHumidity = Validation.PromptUser($"Humidity {originalRecord.Humidity} (%): ");
    if (string.IsNullOrEmpty(newHumidity)) updatedRecord.Humidity = originalRecord.Humidity;
    else
    {
      if (!decimal.TryParse(newHumidity, out var newHumidityTemp))
      {
        updatedRecord.Humidity = originalRecord.Humidity;
      }

      updatedRecord.Humidity = newHumidityTemp;
    }

    Display("Old Description: " + originalRecord.Description);
    var newDescription = Validation.PromptUser("New Description: ");
    updatedRecord.Description = string.IsNullOrEmpty(newDescription) ? originalRecord.Description : newDescription;

    return updatedRecord;
  }
}
