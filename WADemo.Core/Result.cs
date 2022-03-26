namespace WADemo.Core;

// The DTO/Model wraps any type of data, hence the <T>
public class Result<T>
{
  public Result(string message = "")
  {
    Message = message;
  }

  public bool IsSuccess { get; set; }
  public string Message { get; set; }

  // Whatever type this generic ends up being, it will match the type of Data
  public T? Data { get; set; }
}
