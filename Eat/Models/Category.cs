namespace Eat.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Story> Stories { get; set; }
    }

}
