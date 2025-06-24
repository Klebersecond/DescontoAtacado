class Program
{
    class Produto
    {
        public string Gtin;
        public string Descricao;
        public decimal PrecoVarejo;
        public decimal? PrecoAtacado;
        public int? UnidAtacado;
    }

    class Venda
    {
        public string Gtin;
        public int Quantidade;
        public decimal Parcial;
    }

    static void Main()
    {
        var catalogo = new Dictionary<string, Produto>
        {
            ["7891024110348"] = new Produto { Gtin="7891024110348", Descricao="SABONETE", PrecoVarejo=2.88m, PrecoAtacado=2.51m, UnidAtacado=12 },
            ["7891048038017"] = new Produto { Gtin="7891048038017", Descricao="CHÁ CAMOMILA", PrecoVarejo=4.40m, PrecoAtacado=4.37m, UnidAtacado=3 },
            ["7896066334509"] = new Produto { Gtin="7896066334509", Descricao="TORRADA", PrecoVarejo=5.19m },
            ["7891700203142"] = new Produto { Gtin="7891700203142", Descricao="SOJA MAÇÃ ADES", PrecoVarejo=2.39m, PrecoAtacado=2.38m, UnidAtacado=6 },
            ["7894321711263"] = new Produto { Gtin="7894321711263", Descricao="TODDY", PrecoVarejo=9.79m },
            ["7896001250611"] = new Produto { Gtin="7896001250611", Descricao="ADOÇANTE", PrecoVarejo=9.89m, PrecoAtacado=9.10m, UnidAtacado=10 },
            ["7793306013029"] = new Produto { Gtin="7793306013029", Descricao="SUCRILHOS", PrecoVarejo=12.79m, PrecoAtacado=12.35m, UnidAtacado=3 },
            ["7896004400914"] = new Produto { Gtin="7896004400914", Descricao="COCO RALADO", PrecoVarejo=4.20m, PrecoAtacado=4.05m, UnidAtacado=6 },
            ["7898080640017"] = new Produto { Gtin="7898080640017", Descricao="LEITE ITALAC", PrecoVarejo=6.99m, PrecoAtacado=6.89m, UnidAtacado=12 },
            ["7891025301516"] = new Produto { Gtin="7891025301516", Descricao="DANONINHO", PrecoVarejo=12.99m },
            ["7891030003115"] = new Produto { Gtin="7891030003115", Descricao="CREME LEITE", PrecoVarejo=3.12m, PrecoAtacado=3.09m, UnidAtacado=4 }
        };

        var vendas = new List<Venda>
        {
            new Venda { Gtin="7891048038017", Quantidade=1, Parcial=4.40m },
            new Venda { Gtin="7896004400914", Quantidade=4, Parcial=16.80m },
            new Venda { Gtin="7891030003115", Quantidade=1, Parcial=3.12m },
            new Venda { Gtin="7891024110348", Quantidade=6, Parcial=17.28m },
            new Venda { Gtin="7898080640017", Quantidade=24, Parcial=167.76m },
            new Venda { Gtin="7896004400914", Quantidade=8, Parcial=33.60m },
            new Venda { Gtin="7891700203142", Quantidade=8, Parcial=19.12m },
            new Venda { Gtin="7891048038017", Quantidade=1, Parcial=4.40m },
            new Venda { Gtin="7793306013029", Quantidade=3, Parcial=38.37m },
            new Venda { Gtin="7896066334509", Quantidade=2, Parcial=10.38m },
        };

        var descontos = new Dictionary<string, decimal>();
        var totalPorProduto = new Dictionary<string, int>();
        decimal subtotal = 0;

        foreach (var venda in vendas)
        {
            subtotal += venda.Parcial;

            if (!totalPorProduto.ContainsKey(venda.Gtin))
                totalPorProduto[venda.Gtin] = 0;

            totalPorProduto[venda.Gtin] += venda.Quantidade;
        }

        decimal totalDesconto = 0;

        foreach (var item in totalPorProduto)
        {
            var gtin = item.Key;
            var qtde = item.Value;

            if (catalogo.TryGetValue(gtin, out Produto produto) && produto.PrecoAtacado.HasValue && produto.UnidAtacado.HasValue)
            {
                int pacotes = qtde / produto.UnidAtacado.Value;
                decimal desconto = pacotes * produto.UnidAtacado.Value * (produto.PrecoVarejo - produto.PrecoAtacado.Value);
                if (desconto > 0)
                {
                    descontos[gtin] = desconto;
                    totalDesconto += desconto;
                }
            }
        }

        Console.WriteLine("\n--- Desconto no Atacado ---\n");
        Console.WriteLine("Descontos:");
        foreach (var d in descontos)
            Console.WriteLine($"{d.Key}\tR$ {d.Value:F2}");

        Console.WriteLine($"\n(+) Subtotal  =\tR$ {subtotal:F2}");
        Console.WriteLine($"(-) Descontos =\t  R$ {totalDesconto:F2}");
        Console.WriteLine($"(=) Total     =\tR$ {(subtotal - totalDesconto):F2}");
    }
}