namespace Our_decor.Models
{
    public class ProductType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public decimal Coefficient { get; set; }

        public override string ToString()
        {
            return TypeName;
        }
    }
}