using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace RCTechs.Extensions.Helpers
{
    public static class CachingXmlSerializerFactory
    {
        private static readonly Dictionary<string, XmlSerializer> Cache = new Dictionary<string, XmlSerializer>();

        private static readonly object SyncRoot = new object();

        private static XmlSerializer InternalCreate(Type type, XmlRootAttribute root = null, string defaultNameSpace = null)
        {
            if (type == null) throw new ArgumentNullException("type");

            string key;
            if (root == null && string.IsNullOrEmpty(defaultNameSpace))
                key = string.Format(CultureInfo.InvariantCulture, "{0}", type);
            else if (root != null && string.IsNullOrEmpty(defaultNameSpace))
                key = String.Format(CultureInfo.InvariantCulture, "{0}:{1}", type, root.ElementName);
            else if (root == null && string.IsNullOrEmpty(defaultNameSpace))
                key = String.Format(CultureInfo.InvariantCulture, "{0}:NS:{1}", type, defaultNameSpace);
            else
                throw new ArgumentException("Either root or defaultNameSpace should be null, or only one of them must be set, but not both!");

            lock (SyncRoot)
            {
                if (!Cache.ContainsKey(key))
                {
                    XmlSerializer ser = null;
                    if (root == null && string.IsNullOrEmpty(defaultNameSpace))
                        ser = new XmlSerializer(type);
                    else if (root != null && string.IsNullOrEmpty(defaultNameSpace))
                        ser = new XmlSerializer(type, root);
                    else if (root == null && string.IsNullOrEmpty(defaultNameSpace))
                        ser = new XmlSerializer(type, defaultNameSpace);

                    if (ser != null)
                        Cache.Add(key, ser);
                }
            }

            return Cache[key];
        }

        public static XmlSerializer Create<T>(XmlRootAttribute root)
        {
            return InternalCreate(typeof(T), root, null);
        }

        public static XmlSerializer Create<T>()
        {
            return InternalCreate(typeof(T), null, null);
        }
        public static XmlSerializer Create(Type type)
        {
            return InternalCreate(type, null, null);
        }

        public static XmlSerializer Create<T>(string defaultNamespace)
        {
            return InternalCreate(typeof(T), null, defaultNamespace);
        }
    }
}
