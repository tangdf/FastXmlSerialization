namespace FastXmlSerialization.UnitTest
{
   public  class AmountEntity
    {

        [Amount(true)]
        public decimal DetailAmount { get; set; }
        [Amount(false)]
        public decimal SettlementAmount { get; set; }
    }
}
