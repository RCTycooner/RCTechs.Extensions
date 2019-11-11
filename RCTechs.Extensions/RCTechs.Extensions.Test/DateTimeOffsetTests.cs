using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RCTechs.Extensions.Test
{
    [TestClass]
    public class DateTimeOffsetTests
    {
        [TestMethod]
        public void ToIso8601StringTest_DateTimes()
        {
            DateTime dt1 = new DateTime(2019, 10, 1);
            DateTime dt2 = new DateTime(2019, 10, 1, 0, 5, 6);
            DateTime dt3 = new DateTime(2019, 10, 1, 0, 5, 6, DateTimeKind.Utc);

            Assert.AreEqual("2019-10-01T00:00:00.0000000", dt1.ToIso8601String());
            Assert.AreEqual("2019-10-01T00:05:06.0000000", dt2.ToIso8601String());
            Assert.AreEqual("2019-10-01T00:05:06.0000000Z", dt3.ToIso8601String());
            Assert.AreEqual(dt1, dt1.ToIso8601String().ParseDateTimeOffset());
            Assert.AreEqual(dt2, dt2.ToIso8601String().ParseDateTimeOffset());
            Assert.AreEqual(dt3, dt3.ToIso8601String().ParseDateTimeOffset());
        }
        [TestMethod]
        public void ToIso8601StringTest_NullableDateTimes()
        {
            DateTime? dt1 = new DateTime(2019, 10, 1);
            DateTime? dt2 = new DateTime(2019, 10, 1, 0, 5, 6);
            DateTime? dt3 = new DateTime(2019, 10, 1, 0, 5, 6, DateTimeKind.Utc);
            DateTime? dt4 = null;

            Assert.AreEqual("2019-10-01T00:00:00.0000000", dt1.ToIso8601String());
            Assert.AreEqual("2019-10-01T00:05:06.0000000", dt2.ToIso8601String());
            Assert.AreEqual("2019-10-01T00:05:06.0000000Z", dt3.ToIso8601String());
            Assert.AreEqual(null, dt4.ToIso8601String());
            Assert.AreEqual(new DateTimeOffset(dt1.Value), dt1.ToIso8601String().ParseDateTimeOffset());
            Assert.AreEqual(new DateTimeOffset(dt2.Value), dt2.ToIso8601String().ParseDateTimeOffset());
            Assert.AreEqual(new DateTimeOffset(dt3.Value), dt3.ToIso8601String().ParseDateTimeOffset());
            Assert.AreEqual(DateTimeOffset.MinValue, dt4.ToIso8601String().ParseDateTimeOffset());
        }

        [TestMethod]
        public void ToIso8601StringTest_DateTimeOffsets()
        {
            DateTimeOffset dt1 = new DateTimeOffset(new DateTime(2019, 10, 1));
            DateTimeOffset dt2 = new DateTimeOffset(new DateTime(2019, 10, 1, 0, 5, 6));
            DateTimeOffset dt3 = new DateTimeOffset(new DateTime(2019, 10, 1, 0, 5, 6, DateTimeKind.Utc));

            Assert.AreEqual(dt1, dt1.ToIso8601String().ParseDateTimeOffset());
            Assert.AreEqual(dt2, dt2.ToIso8601String().ParseDateTimeOffset());
            Assert.AreEqual(dt3, dt3.ToIso8601String().ParseDateTimeOffset());
        }
        [TestMethod]
        public void ToIso8601StringTest_NullableDateTimeOffsets()
        {
            DateTimeOffset? dt1 = new DateTimeOffset(2019, 10, 1, 0, 0, 0, TimeSpan.FromHours(0));
            DateTimeOffset? dt2 = new DateTimeOffset(2019, 10, 1, 0, 0, 0, TimeSpan.FromHours(-10));
            DateTimeOffset? dt3 = new DateTimeOffset(2019, 10, 1, 0, 0, 0, TimeSpan.FromHours(2));
            DateTimeOffset? dt4 = null;


            Assert.AreEqual("2019-10-01T00:00:00.0000000+00:00", dt1.ToIso8601String());
            Assert.AreEqual("2019-10-01T00:00:00.0000000-10:00", dt2.ToIso8601String());
            Assert.AreEqual("2019-10-01T00:00:00.0000000+02:00", dt3.ToIso8601String());
            Assert.AreEqual(null, dt4.ToIso8601String());
            Assert.AreEqual(dt1.Value, dt1.ToIso8601String().ParseDateTimeOffset());
            Assert.AreEqual(dt2.Value, dt2.ToIso8601String().ParseDateTimeOffset());
            Assert.AreEqual(dt3.Value, dt3.ToIso8601String().ParseDateTimeOffset());
            Assert.AreEqual(DateTimeOffset.MinValue, dt4.ToIso8601String().ParseDateTimeOffset());
        }
    }
}
