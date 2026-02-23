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

        [HttpGet("/listado-de-autores")]
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

        [HttpGet("{id:int}")] // api/autores/1
        public async Task<ActionResult> Get([FromRoute] int id)
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

        [HttpGet("{parametro1}/{parametro2?}")]
        public ActionResult Get(string parametro1, string parametro2 = "Default")
        {
            return Ok(new { parametro1, parametro2 });
        }

        [HttpGet("{nombre:alpha}")]
        public async Task<ActionResult<IEnumerable<Autor>>> Get(string nombre)
        {
            var autores = await context.Autores
                .Where(x => x.Nombre.Contains(nombre))
                .ToListAsync();

            if (!autores.Any())
            {
                return NotFound(new { status = 404, message = $"No se encontraron autores con el nombre '{nombre}'." });
            }

            return Ok(new { status = 200, info = autores });
        }


        [HttpPost]
        // public async Task<ActionResult> Post([FromHeader] Autor autor)
        public async Task<ActionResult> Post([FromBody] Autor autor)
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
