using System.ComponentModel.DataAnnotations;
using BibliotecaAPI.Validaciones;

namespace BibliotecaAPI.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        [PrimeraLetraMayuscula]
        public required string Title { get; set; }
        public int AutorId { get; set; }
        public Autor? Autor { get; set; }
    }
}