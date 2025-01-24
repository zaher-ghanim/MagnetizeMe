namespace SizesAPI.DTO
{
    public class SizeDTO
    {
        public int ItemId { get; set; }

        public string SizeDesc { get; set; } = null!;

        public string Dimension { get; set; } = null!;

        public int Quantity { get; set; }

        public int StepQuantity { get; set; }

        public decimal Price { get; set; }
    }
}