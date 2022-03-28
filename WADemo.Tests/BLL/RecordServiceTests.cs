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
  public void Index_WithJan2019Range_ReturnsOneRecordWithHighTempOf75()
  {
    // Act
    var result = _recordService!.GetRecordsByRange(DateOnly.Parse("01/01/2019"), DateOnly.Parse("01/01/2019"));

    // Assert
    Assert.IsTrue(result.IsSuccess);
    Assert.AreEqual(1, result.Data!.Count);
    Assert.AreEqual(75, result.Data[0].HighTemp);
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

  [Test]
  public void GetRecordByDate_WithJan12019_ReturnsRecordWithHighTempOf75()
  {
    var result = _recordService!.GetRecordByDate(DateOnly.Parse("01/01/2019"));

    Assert.IsTrue(result.IsSuccess);
    Assert.AreEqual(75, result.Data!.HighTemp);
  }

  [Test]
  public void GetRecordByDate_WithNonJan2019_ReturnsNoRecordsFound4DateMessage()
  {
    var result = _recordService!.GetRecordByDate(DateOnly.Parse("02/02/2019"));

    Assert.IsFalse(result.IsSuccess);
    Assert.AreEqual("No record found for the date: 02/02/2019!", result.Message);
  }
}
