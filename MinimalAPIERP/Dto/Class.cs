namespace ERP.Api.Models
{
    public class RaincheckDto
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public ProductDto Product { get; set; }
        public StoreDto Store { get; set; }
    }

    public class ProductDto
    {
        public string Name { get; set; }
        public CategoryDto Category { get; set; }
    }

    public class StoreDto
    {
        public string Name { get; set; }
    }

    public class CategoryDto
    {
        public string Name { get; set; }
    }

    public class Class
    {
        public string Name { get; set; }
        public string Count { get; set; }
        public int SalePrice { get; set; } 
        public string Store { get; set; }
        public string Product { get; set; }

    }
}
