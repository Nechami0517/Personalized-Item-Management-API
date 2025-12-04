// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using project.Interfaces;
// using project.Models;

// public class BookDb : IGeneric
// {
//     [Key]
//     public int Id { get; set; }

//     [Required]
//     [MaxLength(200)]
//     public string? Name { get; set; }

//     [Required]
//     [ForeignKey("Author")]
//     public int AuthorId { get; set; } // מזהה המחבר

//     [Required]
//     public double Price { get; set; }

//     [Required]
//     public DateOnly Date { get; set; }

//     public virtual AuthorDb? Author { get; set; } // יצירת קשר למחבר
// }
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models
{
    public class BookDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public virtual AuthorDb? Author { get; set; }
    }
}