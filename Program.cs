public class Produto
{
    public string NomeProduto { get; set; }
    public string CodigoBarras { get; set; }
    public int QtdDisponivel { get; set; }
    public decimal Preco { get; set; }
    public DateTime DataValidade { get; set; }

    public decimal ValorEstoque => QtdDisponivel * Preco;
}

public class GerenciadorInventario
{
    private List<Produto> _produtos = new List<Produto>();

    public void AdicionarProduto(Produto produto)
    {
        _produtos.Add(produto);
    }

    public void AtualizarProduto(string codigoBarras, Produto atualizarProduto)
    {
        var produto = _produtos.FirstOrDefault(p => p.CodigoBarras == codigoBarras);
        if (produto != null)
        {
            produto.NomeProduto = atualizarProduto.NomeProduto;
            produto.QtdDisponivel = atualizarProduto.QtdDisponivel;
            produto.Preco = atualizarProduto.Preco;
            produto.DataValidade = atualizarProduto.DataValidade;
        }
    }

    public void RemoverProduto(string codigoBarras)
    {
        _produtos.RemoveAll(p => p.CodigoBarras == codigoBarras);
    }

    public Produto BuscaCodigoBarras(string codigoBarras)
    {
        var produto = _produtos.FirstOrDefault(p => p.CodigoBarras == codigoBarras);
        return produto;
    }


    public decimal CalcularValorEstoque()
    {
        return _produtos.Sum(p => p.ValorEstoque);
    }

    public List<Produto> GerarRelatorioVencimento(int diasRestantes)
    {
        var dataLimite = DateTime.Now.AddDays(diasRestantes);
        return _produtos.Where(p => p.DataValidade <= dataLimite).OrderBy(p => p.DataValidade).ToList();
    }
}

public class Programa
{
    static GerenciadorInventario gerenciador = new GerenciadorInventario();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Escolha uma das opções abaixo: ");
            Console.WriteLine("1- Adicionar Produtos. ");
            Console.WriteLine("2- Atualizar Produtos. ");
            Console.WriteLine("3- Remover Produtos. ");
            Console.WriteLine("4- Buscar Produtos. ");
            Console.WriteLine("5- Valor Total de Estoque. ");
            Console.WriteLine("6- Relatório de Vencimento do Estoque. ");
            Console.WriteLine("7- Sair. ");

            var opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1":
                    try
                    {
                        AdicionarProduto();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro: {ex.Message}");
                    }
                    break;
                case "2":
                    try
                    {
                        AtualizarProduto();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro: {ex.Message}");
                    }
                    break;
                case "3":
                    try
                    {
                        RemoverProduto();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro: {ex.Message}");
                    }
                    break;
                case "4":
                    try
                    {
                        BuscaProduto();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro: {ex.Message}");
                    }
                    break;
                case "5":
                    try
                    {
                        CalcularValor();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro: {ex.Message}");
                    }
                    break;
                case "6":
                    try
                    {
                        DisplayProdutosQuaseVencido();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro: {ex.Message}");
                    }
                    break;
                case "7":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }
    static void AdicionarProduto()
    {
        Console.WriteLine("Digite o nome do produto: ");
        string nome = Console.ReadLine();
        Console.WriteLine("Digite o  código de barras do produto: ");
        string codigoBarras = Console.ReadLine();
        Console.WriteLine("Digite a quantidade disponível: ");
        int quantidade = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Digite o preço do produto: ");
        decimal preco = Convert.ToDecimal(Console.ReadLine());
        Console.WriteLine("Digite a data de validade (dd/MM/yyyy): ");
        DateTime dataValidade = Convert.ToDateTime(Console.ReadLine());

        var produto = new Produto
        {
            NomeProduto = nome,
            CodigoBarras = codigoBarras,
            QtdDisponivel = quantidade,
            Preco = preco,
            DataValidade = dataValidade
        };
        gerenciador.AdicionarProduto(produto);
        Console.WriteLine("Produto adicionado!");
    }

    static void AtualizarProduto()
    {
        Console.Write("Código de barras do produto a ser atualizado: ");
        string codigoBarras = Console.ReadLine();
        var produtoAtual = gerenciador.BuscaCodigoBarras(codigoBarras);
        if (produtoAtual == null)
        {
            Console.WriteLine("Produto não encontrado!");
            return;
        }

        Console.Write($"Nome do produto ({produtoAtual.NomeProduto}): ");
        string nome = Console.ReadLine();
        Console.Write($"Quantidade ({produtoAtual.QtdDisponivel}): ");
        int quantidade = int.Parse(Console.ReadLine());
        Console.Write($"Preço unitário ({produtoAtual.Preco}): ");
        decimal preco = decimal.Parse(Console.ReadLine());
        Console.Write($"Data de validade ({produtoAtual.DataValidade:dd/MM/yyyy}): ");
        DateTime dataValidade = DateTime.Parse(Console.ReadLine());

        var atualizarProduto = new Produto
        {
            NomeProduto = string.IsNullOrWhiteSpace(nome) ? produtoAtual.NomeProduto : nome,
            QtdDisponivel = quantidade,
            Preco = preco,
            DataValidade = dataValidade
        };

        gerenciador.AtualizarProduto(codigoBarras, atualizarProduto);
        Console.WriteLine("Produto atualizado com sucesso!");
    }

    static void RemoverProduto()
    {
        Console.Write("Código de barras do produto a ser removido: ");
        string codigoBarras = Console.ReadLine();
        gerenciador.RemoverProduto(codigoBarras);
        Console.WriteLine("Produto removido com sucesso!");
    }

    static void BuscaProduto()
    {
        List<Produto> produtos = null;
        Console.WriteLine("Digite o código de barras para a busca: ");
        string codigoProduto = Console.ReadLine();
        var produtoAtual = gerenciador.BuscaCodigoBarras(codigoProduto);
        if (produtoAtual != null) produtos = new List<Produto> { produtoAtual };
        Console.WriteLine("Produtos encontrados:");

        foreach (var produto in produtos)
        {
            DisplayProduto(produto);

        }
    }

    static void CalcularValor()
    {
        decimal total = gerenciador.CalcularValorEstoque();
        Console.WriteLine($"Valor total do estoque: {total:C2}");
    }

    static void DisplayProdutosQuaseVencido()
    {
        Console.Write("Dias até a data de validade: ");
        int days = Convert.ToInt32(Console.ReadLine());
        var produtos = gerenciador.GerarRelatorioVencimento(days);

        Console.WriteLine($"Produtos prestes a vencer em {days} dias:");
        foreach (var produto in produtos)
        {
            DisplayProduto(produto);
        }
    }

    static void DisplayProduto(Produto produto)
    {
        Console.WriteLine($"\nNome: {produto.NomeProduto}");
        Console.WriteLine($"Código de Barras: {produto.CodigoBarras}");
        Console.WriteLine($"Quantidade: {produto.QtdDisponivel}");
        Console.WriteLine($"Preço Unitário: {produto.Preco:C2}");
        Console.WriteLine($"Data de Validade: {produto.DataValidade:dd/MM/yyyy}");
    }
}
