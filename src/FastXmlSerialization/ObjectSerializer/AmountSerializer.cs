namespace FastXmlSerialization
{
    internal class AmountSerializer : DecimalSerializer<decimal>
    {
        public AmountSerializer(bool isDetail) : base(isDetail)
        {
        }
    }

    internal class NullableAmountSerializer : DecimalSerializer<decimal?>
    {
        public NullableAmountSerializer(bool isDetail) : base(isDetail)
        {
        }
    }
}