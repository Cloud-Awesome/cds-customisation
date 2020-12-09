using System.IO;
using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation
{
    public static class SerialisationWrapper
    {
        public static T DeserialiseFromFile<T>(string path)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (FileStream fs = File.OpenRead(path))
            {
                return (T)xmlSerializer.Deserialize(fs);
            }
        }
    }
}
