using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Internal;
using WADemo.Core;
using WADemo.DAL;

namespace WADemo.Tests.DAL;

[TestFixture]
public class RecordRepoTests
{
  [SetUp]
  public void Setup()
  {
    // Delete any Test Data File
    if (File.Exists(TestDataFile)) File.Delete(TestDataFile);

    // Copy the seed contents to a newly created Test Data File
    File.Copy(SeedFile, TestDataFile);

    _repo = new CsvRecordRepository(TestDataFile);
  }

  // TODO: Look for a way to avoid all the silly ../ and get net6 to read the dang file!
  private const string SeedFile = "../../../DAL/test_data/test.seed.csv";
  private const string TestDataFile = "../../../DAL/test_data/test.data.csv";

  private CsvRecordRepository? _repo;

  [Test]
  public void CanCreateTestFile()
  {
    Assert.IsTrue(File.Exists(TestDataFile));
  }

  [Test]
  public void Index_Returns3RecordsWithFirstHighTempOf32()
  {
    var records = _repo!.Index();

    Assert.AreEqual(3, records.Data!.Count);
    Assert.AreEqual(32, records.Data[0].HighTemp);
  }

  [Test]
  public void Add_AddsRecordToFileWithHighTempOf42()
  {
    var record = new WeatherRecord()
    {
      Date = DateOnly.FromDateTime(DateTime.Now), HighTemp = 42, LowTemp = 19, Humidity = 5
    };

    _repo!.Add(record);

    var records = _repo!.Index();
    Assert.AreEqual(4, records.Data!.Count);

    // TODO: Assert equals on other fields
    Assert.AreEqual(42, records.Data[3].HighTemp);
  }

  [Test]
  public void Update_UpdatesFirstRecordWithHighTempOf90()
  {
    var record = new WeatherRecord() {Date = DateOnly.Parse("01/02/2015"), HighTemp = 90, LowTemp = 19, Humidity = 5};

    _repo!.Update(record);
    var records = _repo!.Index();

    Assert.AreEqual(90, records.Data![0].HighTemp);
    Assert.AreEqual(3, records.Data.Count);
  }

  [Test]
  public void Delete_DeletesFirstRecord()
  {
    _repo!.Delete(DateOnly.Parse("01/02/2015"));
    var records = _repo!.Index();

    Assert.AreEqual(2, records.Data!.Count);
    // TODO: Pull the record for that same date and make sure it fails
  }
}
