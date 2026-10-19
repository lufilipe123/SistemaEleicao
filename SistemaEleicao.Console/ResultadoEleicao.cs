namespace SistemaEleicao;

public sealed class ResultadoEleicao(
    IReadOnlyDictionary<int, int> votosPorCandidato,
    int votosNulos,
    int votosBrancos,
    Candidato? vencedor)
{
    public IReadOnlyDictionary<int, int> VotosPorCandidato { get; } = votosPorCandidato;

    public int VotosNulos { get; } = votosNulos;

    public int VotosBrancos { get; } = votosBrancos;

    public Candidato? Vencedor { get; } = vencedor;
}
