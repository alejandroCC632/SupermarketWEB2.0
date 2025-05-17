using System.ComponentModel.DataAnnotations;

namespace SupermarketWEB.Models
{
    public class Users
    {
       
            public int Id { get; set; }

            [Required] // verificar que se importo usin System.ComponentModel.DataAnootations
            [DataType(DataType.EmailAddress)]
            public String Email { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public  string Salt;
        
    }
}
