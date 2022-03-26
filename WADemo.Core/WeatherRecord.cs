namespace WADemo.Core;

public class WeatherRecord
{
  public WeatherRecord(string description = "")
  {
    Description = description;
  }

  public DateTime Date { get; set; }
  public string Description { get; set; }
  public decimal HighTemp { get; set; }
  public decimal Humidity { get; set; }
  public decimal LowTemp { get; set; }
}
