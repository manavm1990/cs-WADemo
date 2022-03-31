namespace WADemo.Core.Models;

public class StatReport
{
  public double AvgHighTemp { get; set; }
  public decimal MaxHighTemp { get; set; }
  public decimal MinHighTemp { get; set; }

  public double AvgLowTemp { get; set; }
  public decimal MaxLowTemp { get; set; }
  public decimal MinLowTemp { get; set; }

  public decimal AvgHumidity { get; set; }
  public decimal MaxHumidity { get; set; }
  public decimal MinHumidity { get; set; }

  // TODO: Override ToString() to return a string representation of the object (and same for WeatherRecord)
}
