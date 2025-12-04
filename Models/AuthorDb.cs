// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using project.Interfaces;

// namespace project.Models
// {
//    public class AuthorDb : User, IGeneric
// {
//     [Key]
//     public int Id { get; set; }

//     [Required]
//     [MaxLength(100)]
//     public string Name { get; set; }

//     [Required]
//     public string Password { get; set; } = string.Empty;

//     [Required]
//     public Role Role { get; set; } = Role.Author;

//     public string? Address { get; set; }

//     [Required]
//     public DateOnly BirthDate { get; set; }

//     public virtual ICollection<BookDb>? Books { get; set; } // קשר אחד-לרבים לספרים

//     public override string ToString()
//     {
//         return $"Id: {Id}, Name: {Name}, Address: {Address}, BirthDate: {BirthDate}";
//     }
// }
// }
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models
{
    public class AuthorDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = "Author";

        public string? Address { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public virtual ICollection<BookDb>? Books { get; set; }
    }
}