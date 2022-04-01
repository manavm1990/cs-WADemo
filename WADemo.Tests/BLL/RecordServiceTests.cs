﻿using System;
using System.Linq;
using NUnit.Framework;
using WADemo.BLL;
using WADemo.Core.Interfaces;
using WADemo.Core.Models;
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
  public void Index_WithJan2019Range_ReturnsJan2019Records()
  {
    // Act
    var result = _recordService!.GetRecordsByRange(DateOnly.Parse("01/01/2019"), DateOnly.Parse("01/31/2019"));

    // Assert
    Assert.IsTrue(result.IsSuccess);

    // The 🔑 is always the first date of the month so we can sort by month/year (date doesn't matter).
    // Then the list has the real dates.
    Assert.AreEqual(2, result.Data![DateOnly.Parse("01/01/2019")].Count);
    Assert.AreEqual(80, result.Data[DateOnly.Parse("01/01/2019")][0].HighTemp);
  }

  [Test]
  public void Index_WithNonJan2019Range_ReturnsNoRecordsFoundInRangeMessage()
  {
    // Act
    var result = _recordService!.GetRecordsByRange(DateOnly.Parse("02/01/2020"), DateOnly.Parse("02/28/2020"));

    // Assert
    Assert.IsFalse(result.IsSuccess);
    Assert.AreEqual("No records found in the range!", result.Message);
  }

  [Test]
  public void GetRecordByDate_WithJan12019_ReturnsRecordWithHighTempOf80()
  {
    var result = _recordService!.GetRecordByDate(DateOnly.Parse("01/01/2019"));

    Assert.IsTrue(result.IsSuccess);
    Assert.AreEqual(80, result.Data!.HighTemp);
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
      Date = DateOnly.Parse("01/22/2019"),

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
    Assert.AreEqual(3, records.Data![DateOnly.Parse("01/01/2019")].Count);

    // All keys are set to first day of month - only care about month and year
    Assert.AreEqual(85, records.Data[DateOnly.Parse("01/01/2019")][2].HighTemp);
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
      Date = DateOnly.Parse("02/06/2019"),
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
    var records = _recordService!.GetRecordsByRange(DateOnly.Parse("01/01/2019"),
      DateOnly.Parse("01/31/2019"));

    Assert.IsTrue(updateResult.IsSuccess);
    Assert.AreEqual("Record for 01/01/2019 updated successfully!", updateResult.Message);

    // TODO: Add more asserts for additional fields
    Assert.AreEqual(95, updatedRecordByDate.Data!.HighTemp);
    Assert.AreEqual(2, records.Data![DateOnly.Parse("01/01/2019")].Count);
  }

  [Test]
  public void DeleteRecord_ReturnsSuccess()
  {
    var deleteResult = _recordService!.DeleteRecord(DateOnly.Parse("01/01/2019"));

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

  [Test]
  public void GetStats4Jan2019_ReturnsStatsForJan2019()
  {
    var expected = new StatReport
    {
      AvgHighTemp = 75,
      MinHighTemp = 70,
      MaxHighTemp = 80,
      AvgLowTemp = 55,
      MinLowTemp = 50,
      MaxLowTemp = 60,
      AvgHumidity = 50,
      MinHumidity = 40,
      MaxHumidity = 60
    };
    var actual = _recordService!.GetStatReport(DateOnly.Parse("01/01/2019"), DateOnly.Parse("01/31/2019"));

    Assert.AreEqual(expected.AvgHighTemp, actual.Data!.AvgHighTemp);
    Assert.AreEqual(expected.MinHighTemp, actual.Data!.MinHighTemp);
    Assert.AreEqual(expected.MaxHighTemp, actual.Data!.MaxHighTemp);
    Assert.AreEqual(expected.AvgLowTemp, actual.Data!.AvgLowTemp);
    Assert.AreEqual(expected.MinLowTemp, actual.Data!.MinLowTemp);
    Assert.AreEqual(expected.MaxLowTemp, actual.Data!.MaxLowTemp);
    Assert.AreEqual(expected.AvgHumidity, actual.Data!.AvgHumidity);
    Assert.AreEqual(expected.MinHumidity, actual.Data!.MinHumidity);
    Assert.AreEqual(expected.MaxHumidity, actual.Data!.MaxHumidity);
  }

  [Test]
  public void SearchRecords_WithSunny_Returns3RecordsWithSunnyInDescription()
  {
    var records = _recordService!.SearchRecords("Sunny");

    Assert.AreEqual(3, records.Data!.Count);
    Assert.That(records.Data!.All(record => record.Description.ToLower().Contains("sunny")));
  }
}
