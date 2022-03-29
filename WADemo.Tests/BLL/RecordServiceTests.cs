using System;
using NUnit.Framework;
using WADemo.BLL;
using WADemo.Core;
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

  [Test]
  public void AddRecord_WithValidRecord_ReturnsSuccessMessageAfterAddingRecordWithHighTempOf85()
  {
    var record = new WeatherRecord
    {
      // ⚠️ Make sure it's not a duplicate date
      Date = DateOnly.Parse("01/02/2019"),

      // ⚠️ Keep this distinctly different from the seed 🌱 data in MockRecordRepository.
      HighTemp = 85,
      LowTemp = 65,
      Humidity = 70,
      Description = "Sunny"
    };

    var result = _recordService!.AddRecord(record);
    var records = _recordService!.GetRecordsByRange(DateOnly.Parse("01/01/2019"), DateOnly.Parse("01/31/2019"));

    Assert.IsTrue(result.IsSuccess);
    Assert.AreEqual("Record added successfully!", result.Message);
    Assert.AreEqual(2, records.Data!.Count);
    Assert.AreEqual(85, records.Data[1].HighTemp);
  }

  [Test]
  public void AddRecord_WithDuplicateDate_ReturnsDuplicateDateMessage()
  {
    var record = new WeatherRecord
    {
      // ⚠️ Make sure it's a duplicate date
      Date = DateOnly.Parse("01/01/2019")
    };

    var result = _recordService!.AddRecord(record);

    Assert.IsFalse(result.IsSuccess);
    Assert.AreEqual("A record already exists for the date: 01/01/2019!", result.Message);
  }

  [Test]
  public void AddRecord_WithLowTempHigherThanHighTemp_ReturnsLowTempHigherThanHighTempMessage()
  {
    var record = new WeatherRecord
    {
      // ⚠️ Make sure it's not a duplicate date
      Date = DateOnly.Parse("01/02/2019"),
      HighTemp = 85,
      LowTemp = 95,
      Humidity = 70,
      Description = "Sunny"
    };

    var result = _recordService!.AddRecord(record);

    Assert.IsFalse(result.IsSuccess);
    Assert.AreEqual("High temp should not be less than low temp.", result.Message);
  }

  [Test]
  public void UpdateRecord_ReturnsUpdatedRecordWithoutChangingRecordsCount()
  {
    var updatedRecord = new WeatherRecord
    {
      // ⚠️ Make sure date is same
      Date = DateOnly.Parse("01/01/2019"),
      HighTemp = 95,
      LowTemp = 65,
      Humidity = 70,
      Description = "Sunny"
    };

    var updateResult = _recordService!.UpdateRecord(updatedRecord);
    var updatedRecordByDate = _recordService!.GetRecordByDate(DateOnly.Parse("01/01/2019"));
    var records = _recordService!.GetRecordsByRange(DateOnly.Parse("01/01/2019"), DateOnly.Parse("01/31/2019"));

    Assert.IsTrue(updateResult.IsSuccess);
    Assert.AreEqual("Record for 01/01/2019 updated successfully!", updateResult.Message);
    Assert.AreEqual(95, updatedRecordByDate.Data!.HighTemp);
    Assert.AreEqual(1, records.Data!.Count);
  }

  [Test]
  public void DeleteRecord_ReturnsSuccess()
  {
    var deleteResult = _recordService!.DeleteRecord(DateOnly.Parse("01/01/2019"));

    var records = _recordService!.GetRecordsByRange(DateOnly.Parse("01/01/2019"), DateOnly.Parse("01/31/2019"));
    var recordByDate = _recordService!.GetRecordByDate(DateOnly.Parse("01/01/2019"));

    Assert.IsTrue(deleteResult.IsSuccess);
    Assert.IsFalse(recordByDate.IsSuccess); // Means no records found
    Assert.IsNull(recordByDate.Data);
  }

  [Test]
  public void DeleteRecord_WithNonExistentRecord_ReturnsNoRecordFoundMessage()
  {
    var deleteResult = _recordService!.DeleteRecord(DateOnly.Parse("11/01/2019"));

    Assert.IsFalse(deleteResult.IsSuccess);
    Assert.AreEqual("Record not found for date: 11/01/2019!", deleteResult.Message);
  }
}
