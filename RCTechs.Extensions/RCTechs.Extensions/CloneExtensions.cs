using RCTechs.Extensions.Helpers;
using System.IO;

namespace RCTechs.Extensions
{
    public static class CloneExtensions
    {
        public static T Clone<T>(this T source)
        {
            var serializer = CachingXmlSerializerFactory.Create<T>();
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms, source);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)serializer.Deserialize(ms);
            }
        }
    }
}
