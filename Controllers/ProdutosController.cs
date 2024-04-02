using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            try
            {
                var produtos = await _context.Produtos.ToListAsync();
                return produtos;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProdutoById(long id)
        {
            try
            {
                var produto = await _context.Produtos.FindAsync(id);

                if (produto == null)
                {
                    return NotFound();
                }

                return produto;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> PostProdutos(Produto produto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProdutoById), new { id = produto.Id }, produto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(long id, Produto produto)
        {
            if (id != produto.Id)
            {
                return BadRequest();
            }

            var produtoExistente = await _context.Produtos.FindAsync(id);
            if (produtoExistente == null)
            {
                return NotFound();
            }

            produtoExistente.Name = produto.Name;
            produtoExistente.Preco = produto.Preco;
            produtoExistente.Categoria = produto.Categoria;
            produtoExistente.Quantidade = produto.Quantidade;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ProdutoExists(id))
            {
                return NotFound();
            }

            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(long id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool ProdutoExists(long id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }

    }
}
