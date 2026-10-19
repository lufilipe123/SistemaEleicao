namespace SistemaEleicao;

public sealed record Candidato
{
    public Candidato(string nome, int numero)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("O nome do candidato é obrigatório.", nameof(nome));
        }

        if (numero <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(numero), "O número deve ser positivo.");
        }

        Nome = nome.Trim();
        Numero = numero;
    }

    public string Nome { get; }

    public int Numero { get; }
}
