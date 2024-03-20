using System.IO;

namespace EchoOfTheTimes.Utils
{
    public static class BinaryReaderExtensions
    {
        public static SerializableGuid Read(this BinaryReader reader)
        {
            return new SerializableGuid(reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32());
        }
    }
}