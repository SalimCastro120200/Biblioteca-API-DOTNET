using BibliotecaAPI.Datos;
using BibliotecaAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var autores = await context.Autores.ToListAsync();

            return Ok(new
            {
                status = 200,
                message = "Lista de autores obtenida correctamente",
                info = autores
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var autor = await context.Autores
                .Include(x => x.Libros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (autor is null)
            {
                return NotFound(new
                {
                    status = 404,
                    message = "Autor no encontrado"
                });
            }

            return Ok(new
            {
                status = 200,
                message = "Autor obtenido correctamente",
                info = autor
            });
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            context.Add(autor);
            await context.SaveChangesAsync();

            return Ok(new
            {
                status = 200,
                message = "Autor creado correctamente"
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "Los ID's deben de coincidir"
                });
            }

            context.Update(autor);
            await context.SaveChangesAsync();

            return Ok(new
            {
                status = 200,
                message = "Autor actualizado correctamente"
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var registrosBorrados = await context.Autores
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            if (registrosBorrados == 0)
            {
                return NotFound(new
                {
                    status = 404,
                    message = "Autor no encontrado"
                });
            }

            return Ok(new
            {
                status = 200,
                message = "Autor eliminado correctamente"
            });
        }
    }
}
