namespace WebApi.Models;

public class Produto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public int Preco { get; set; }
    public string? Categoria { get; set; }
    public int Quantidade { get; set; }
}