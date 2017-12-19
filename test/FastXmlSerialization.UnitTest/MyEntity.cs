using System;

namespace FastXmlSerialization.UnitTest
{
    public class MyEntity
    {
        public string StringValue { get; set; }

        public Int32 Int32Value { get; set; }
        public Int64 Int64Value { get; set; }
        public DateTime DateTimeValue { get; set; }
        [Amount(false)]
        public Decimal DecimalValue { get; set; }

        public Int32? NullableInt32Value { get; set; }
        public Int64? NullableInt64Value { get; set; }
        public DateTime? NullableDateTimeValue { get; set; }
        [Amount(false)]
        public Decimal? NullableDecimalValue { get; set; }


        public override string ToString()
        {
            return $"{nameof(StringValue)}: {StringValue}, {nameof(Int32Value)}: {Int32Value}, {nameof(Int64Value)}: {Int64Value}, {nameof(DateTimeValue)}: {DateTimeValue}, {nameof(DecimalValue)}: {DecimalValue}, {nameof(NullableInt32Value)}: {NullableInt32Value}, {nameof(NullableInt64Value)}: {NullableInt64Value}, {nameof(NullableDateTimeValue)}: {NullableDateTimeValue}, {nameof(NullableDecimalValue)}: {NullableDecimalValue}";
        }

    }
}