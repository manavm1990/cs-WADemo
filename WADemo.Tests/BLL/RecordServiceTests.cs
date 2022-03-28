using System;
using NUnit.Framework;
using WADemo.BLL;
using WADemo.Core.Interfaces;
using WADemo.DAL;

namespace WADemo.Tests.BLL;

public class RecordServiceTests
{
  [SetUp]
  public void Setup()
  {
    _recordRepository = new MockRecordRepository();
  }

  private IRecordRepository? _recordRepository;

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
}
