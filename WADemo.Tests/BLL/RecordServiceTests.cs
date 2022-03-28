using System;
using NUnit.Framework;
using WADemo.BLL;
using WADemo.Core.Interfaces;
using WADemo.DAL;

namespace WADemo.Tests.BLL;

public class RecordServiceTests
{
  private IRecordRepository? _recordRepository;

  [SetUp]
  public void Setup()
  {
    _recordRepository = new MockRecordRepository();
  }

  [Test]
  public void Index_ReturnsSuccessWithOneRecord4Jan2019()
  {
    // Arrange
    var recordService = new RecordService(_recordRepository!);

    // Act
    var result = recordService.GetRecordsByRange(DateOnly.Parse("01/01/2019"), DateOnly.Parse("01/01/2019"));

    // Assert
    Assert.IsTrue(result.IsSuccess);
    Assert.AreEqual(1, result.Data!.Count);
  }

  [Test]
  public void Index_ReturnsNoRecordsMessage4NonJan2019()
  {
    // Arrange
    var recordService = new RecordService(_recordRepository!);

    // Act
    var result = recordService.GetRecordsByRange(DateOnly.Parse("02/02/2019"), DateOnly.Parse("02/02/2019"));

    // Assert
    Assert.IsFalse(result.IsSuccess);
    Assert.AreEqual("No records found in the range!", result.Message);
  }
}
