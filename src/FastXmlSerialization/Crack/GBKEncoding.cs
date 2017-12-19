using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FastXmlSerialization
{
    /// <summary>
    /// .Net 不是序列化出 encoding="GBK" Xml的格式，用此解决。
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class GBKEncoding : Encoding
    {
        public static Encoding Instance = new GBKEncoding();

        private GBKEncoding()
        {
        }

        public override string BodyName
        {
            get { return "GBK"; }
        }

        public override string WebName
        {
            get { return this.BodyName; }
        }

        private static Encoding encoding = Encoding.GetEncoding(936);

        public override int GetByteCount(char[] chars, int index, int count)
        {
            return encoding.GetByteCount(chars, index, count);
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            return encoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return encoding.GetCharCount(bytes, index, count);
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            return encoding.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
        }

        public override int GetMaxByteCount(int charCount)
        {
            return encoding.GetMaxByteCount(charCount);
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return encoding.GetMaxCharCount(byteCount);
        }
    }
}