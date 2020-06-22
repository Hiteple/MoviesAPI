using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "The field with name {0} is required")]
        [StringLength(10)]
        public string Name { get; set; }

        [Range(18, 120)]
        public int Age { get; set; }
        
        [CreditCard]
        public string CreditCard { get; set; }
        
        [Url]
        public string Url { get; set; }
    }
}