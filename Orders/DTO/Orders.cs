namespace OrdersAPI.DTO
{
    public class Orders
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int SizeId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }
}
