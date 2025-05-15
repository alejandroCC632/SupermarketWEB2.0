namespace SupermarketWEB.Models
{
    public class Category
    {
        public int Id { get; set; } //Sera la llave primaria 
        public String Name { get; set; }
        public String? Description { get; set; }
        public ICollection<Product>? Products { get; set; } = default; //Propiedad de navegacion 
    }
}
