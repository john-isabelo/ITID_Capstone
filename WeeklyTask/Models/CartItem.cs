using System.Drawing;

namespace WeeklyTask.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total
        {
            get { return Quantity * Price; }
        }
        public string Image { get; set; }

        public CartItem() { 
        }

        public CartItem(Food food)
        {
            ProductId = food.Id;
            ProductName = food.Name;
            Quantity = 1;
            Price = food.Price;
            Image = food.ImagePath;
        }
    }
}
