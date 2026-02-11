using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        [Required] // NETCORE VALIDATION
        public required string Nombre { get; set; } // C SHARP VALIDATION
        public List<Libro> Libros { get; set; } = new List<Libro>();
    }
}