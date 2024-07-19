using System.IO;

namespace NgocRongGold.Application.Helper
{
    public static class StreamEOF
    {
        public static bool IsAvailable( this BinaryReader binaryReader ) {
            var bs = binaryReader.BaseStream;
            return ( bs.Position == bs.Length);
        }
    }
}