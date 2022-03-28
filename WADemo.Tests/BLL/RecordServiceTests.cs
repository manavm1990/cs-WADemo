using System;
using NUnit.Framework;
using WADemo.BLL;
using WADemo.Core.Interfaces;
using WADemo.DAL;

namespace WADemo.Tests.BLL;

public class RecordServiceTests
{
  private IRecordService? _recordService;

  [SetUp]
  public void Setup()
  {
    // Arrange
    _recordService = new RecordService(new MockRecordRepository());
  }

  [Test]
  public void Index_WithJan2019Range_ReturnsOneRecord()
  {
    // Act
    var result = _recordService!.GetRecordsByRange(DateOnly.Parse("01/01/2019"), DateOnly.Parse("01/01/2019"));

    // Assert
    Assert.IsTrue(result.IsSuccess);
    Assert.AreEqual(1, result.Data!.Count);
  }

  [Test]
  public void Index_WithNonJan2019Range_ReturnsNoRecordsFoundInRangeMessage()
  {
    // Act
    var result = _recordService!.GetRecordsByRange(DateOnly.Parse("02/02/2019"), DateOnly.Parse("02/02/2019"));

    // Assert
    Assert.IsFalse(result.IsSuccess);
    Assert.AreEqual("No records found in the range!", result.Message);
  }
}
