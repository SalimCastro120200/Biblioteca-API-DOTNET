using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; }
        public int AutorId { get; set; }
        public Autor? Autor { get; set; }
    }
}