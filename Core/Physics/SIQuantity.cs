namespace Core.Physics
{
    public class SIQuantity(SIBaseType type, double quantity)
    {
        public SIBaseType Type { get; set; } = type;
        public double Quantity { get; set; } = quantity;
    }
}
