using SistemaEleicao;

var eleicao = new EleicaoManager();
var executando = true;

while (executando)
{
    Console.WriteLine();
    Console.WriteLine("=== Sistema de Eleição ===");
    Console.WriteLine("1. Cadastrar candidato");
    Console.WriteLine("2. Cadastrar eleitor");
    Console.WriteLine("3. Votar");
    Console.WriteLine("4. Apurar votos");
    Console.WriteLine("0. Sair");
    Console.Write("Escolha uma opção: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1":
            CadastrarCandidato(eleicao);
            break;
        case "2":
            CadastrarEleitor(eleicao);
            break;
        case "3":
            RegistrarVoto(eleicao);
            break;
        case "4":
            ExibirApuracao(eleicao);
            break;
        case "0":
            executando = false;
            break;
        default:
            Console.WriteLine("Opção inválida.");
            break;
    }
}

static void CadastrarCandidato(EleicaoManager eleicao)
{
    Console.Write("Nome do candidato: ");
    var nome = Console.ReadLine() ?? string.Empty;
    if (!LerInteiro("Número do candidato: ", out var numero))
    {
        return;
    }

    ExecutarAcao(() => eleicao.CadastrarCandidato(nome, numero), "Candidato cadastrado.");
}

static void CadastrarEleitor(EleicaoManager eleicao)
{
    Console.Write("Número do título/CPF: ");
    var identificacao = Console.ReadLine() ?? string.Empty;
    ExecutarAcao(() => eleicao.CadastrarEleitor(identificacao), "Eleitor cadastrado.");
}

static void RegistrarVoto(EleicaoManager eleicao)
{
    Console.Write("Número do título/CPF: ");
    var identificacao = Console.ReadLine() ?? string.Empty;
    Console.WriteLine("Digite o número do candidato, 0 para branco ou -1 para nulo.");
    if (!LerInteiro("Seu voto: ", out var escolha))
    {
        return;
    }

    var tipo = escolha switch
    {
        0 => TipoVoto.Branco,
        -1 => TipoVoto.Nulo,
        _ => TipoVoto.Candidato
    };
    int? numeroCandidato = tipo == TipoVoto.Candidato ? escolha : null;
    ExecutarAcao(
        () => eleicao.RegistrarVoto(identificacao, tipo, numeroCandidato),
        "Voto registrado.");
}

static void ExibirApuracao(EleicaoManager eleicao)
{
    var resultado = eleicao.Apuracao();
    Console.WriteLine();
    Console.WriteLine("=== Apuração ===");
    foreach (var candidato in eleicao.Candidatos.OrderBy(candidato => candidato.Numero))
    {
        Console.WriteLine(
            $"{candidato.Numero} - {candidato.Nome}: {resultado.VotosPorCandidato[candidato.Numero]} voto(s)");
    }

    Console.WriteLine($"Votos nulos: {resultado.VotosNulos}");
    Console.WriteLine($"Votos brancos: {resultado.VotosBrancos}");
    Console.WriteLine(
        resultado.Vencedor is null
            ? "Sem vencedor (nenhum voto válido ou empate)."
            : $"Vencedor: {resultado.Vencedor.Nome} ({resultado.Vencedor.Numero})");
}

static bool LerInteiro(string mensagem, out int valor)
{
    Console.Write(mensagem);
    if (int.TryParse(Console.ReadLine(), out valor))
    {
        return true;
    }

    Console.WriteLine("Digite um número inteiro válido.");
    return false;
}

static void ExecutarAcao(Action acao, string mensagemSucesso)
{
    try
    {
        acao();
        Console.WriteLine(mensagemSucesso);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Dados inválidos: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Não foi possível concluir: {ex.Message}");
    }
}
