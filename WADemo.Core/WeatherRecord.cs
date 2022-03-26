namespace WADemo.Core;

public class WeatherRecord
{
  public const int MaxTemperature = 150;
  public const int MinTemperature = -150;

  public WeatherRecord(string description = "")
  {
    Description = description;
  }

  public DateOnly Date { get; set; }
  public string Description { get; set; }
  public int HighTemp { get; set; }
  public decimal Humidity { get; set; }
  public int LowTemp { get; set; }
}
