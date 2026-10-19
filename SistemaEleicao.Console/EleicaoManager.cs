namespace SistemaEleicao;

public sealed class EleicaoManager
{
    private readonly Dictionary<int, Candidato> _candidatos = [];
    private readonly Dictionary<string, bool> _eleitores = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<int, int> _votosPorCandidato = [];
    private int _votosNulos;
    private int _votosBrancos;

    public IReadOnlyCollection<Candidato> Candidatos => _candidatos.Values;

    public void CadastrarCandidato(string nome, int numero)
    {
        var candidato = new Candidato(nome, numero);
        if (!_candidatos.TryAdd(numero, candidato))
        {
            throw new InvalidOperationException($"Já existe um candidato com o número {numero}.");
        }

        _votosPorCandidato.Add(numero, 0);
    }

    public void CadastrarEleitor(string identificacao)
    {
        var eleitor = NormalizarIdentificacao(identificacao);
        if (!_eleitores.TryAdd(eleitor, false))
        {
            throw new InvalidOperationException("Este eleitor já está cadastrado.");
        }
    }

    public void RegistrarVoto(string identificacao, TipoVoto tipo, int? numeroCandidato = null)
    {
        var eleitor = NormalizarIdentificacao(identificacao);
        if (!_eleitores.TryGetValue(eleitor, out var jaVotou))
        {
            throw new InvalidOperationException("Eleitor não cadastrado.");
        }

        if (jaVotou)
        {
            throw new InvalidOperationException("Este eleitor já votou.");
        }

        switch (tipo)
        {
            case TipoVoto.Candidato when numeroCandidato is int numero
                && _votosPorCandidato.ContainsKey(numero):
                _votosPorCandidato[numero]++;
                break;
            case TipoVoto.Candidato:
            case TipoVoto.Nulo:
                _votosNulos++;
                break;
            case TipoVoto.Branco:
                _votosBrancos++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tipo), "Tipo de voto inválido.");
        }

        _eleitores[eleitor] = true;
    }

    public ResultadoEleicao Apuracao()
    {
        var votos = new Dictionary<int, int>(_votosPorCandidato);
        var maiorTotal = votos.Count == 0 ? 0 : votos.Values.Max();
        var vencedores = _candidatos.Values
            .Where(candidato => votos[candidato.Numero] == maiorTotal && maiorTotal > 0)
            .ToArray();
        var vencedor = vencedores.Length == 1 ? vencedores[0] : null;

        return new ResultadoEleicao(votos, _votosNulos, _votosBrancos, vencedor);
    }

    private static string NormalizarIdentificacao(string identificacao)
    {
        if (string.IsNullOrWhiteSpace(identificacao))
        {
            throw new ArgumentException("A identificação do eleitor é obrigatória.", nameof(identificacao));
        }

        return identificacao.Trim();
    }
}
