using System;

namespace FastXmlSerialization
{
    internal class DateTimeValueEncoder : BaseValueEncoder<DateTime>
    {
        //3.	文档内但凡日期格式的处理均传入年月日，带时分秒的处理，且全部为YYYY-MM-DD 24HH:MM:SS格式。DATE型类型，需要在格式后添加时分秒后进行传入。

        public override bool Nullable
        {
            get { return false; }
        }

        public override DateTime Decode(string value)
        {
            return Convert.ToDateTime(value);
        }

        public override string Encode(DateTime value)
        {
            //todo:增加单测
            return value.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}