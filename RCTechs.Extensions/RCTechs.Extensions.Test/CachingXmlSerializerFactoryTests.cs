using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCTechs.Extensions.Helpers;
using System.Collections.Generic;

namespace RCTechs.Extensions.Test
{
    [TestClass]
    public class CachingXmlSerializerFactoryTests
    {
        [TestMethod]
        public void TestCachingSerializer_Caching()
        {
            var ser1 = CachingXmlSerializerFactory.Create<List<int>>();

            var ser2 = CachingXmlSerializerFactory.Create<List<int>>();

            Assert.AreEqual(ser1, ser2);
        }
    }
}
