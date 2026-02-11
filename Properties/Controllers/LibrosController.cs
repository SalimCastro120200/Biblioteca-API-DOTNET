using BibliotecaAPI.Datos;
using BibliotecaAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var libros = await context.Libros.ToListAsync();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Lista de libros obtenida correctamente",
                info = libros
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var libro = await context.Libros
                .Include(x => x.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libro is null)
            {
                return NotFound(new
                {
                    status = StatusCodes.Status404NotFound,
                    message = "Libro no encontrado"
                });
            }

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Libro obtenido correctamente",
                info = libro
            });
        }

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existeAutor = await context.Autores
                .AnyAsync(x => x.Id == libro.AutorId);

            if (!existeAutor)
            {
                return BadRequest(new
                {
                    status = StatusCodes.Status400BadRequest,
                    message = $"El autor de id {libro.AutorId} no existe"
                });
            }

            context.Add(libro);
            await context.SaveChangesAsync();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Libro creado correctamente"
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest(new
                {
                    status = StatusCodes.Status400BadRequest,
                    message = "Los ID's deben de coincidir"
                });
            }

            var existeAutor = await context.Autores
                .AnyAsync(x => x.Id == libro.AutorId);

            if (!existeAutor)
            {
                return BadRequest(new
                {
                    status = StatusCodes.Status400BadRequest,
                    message = $"El autor de id {libro.AutorId} no existe"
                });
            }

            context.Update(libro);
            await context.SaveChangesAsync();

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Libro actualizado correctamente"
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var registrosBorrados = await context.Libros
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            if (registrosBorrados == 0)
            {
                return NotFound(new
                {
                    status = StatusCodes.Status404NotFound,
                    message = "Libro no encontrado"
                });
            }

            return Ok(new
            {
                status = StatusCodes.Status200OK,
                message = "Libro eliminado correctamente"
            });
        }
    }
}
