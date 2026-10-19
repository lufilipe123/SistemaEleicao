using SistemaEleicao;

namespace SistemaEleicao.Tests;

public class EleicaoManagerTests
{
    [Fact]
    public void CadastrarCandidato_ComNumeroDuplicado_RejeitaCadastro()
    {
        var eleicao = new EleicaoManager();
        eleicao.CadastrarCandidato("Ana", 10);

        Assert.Throws<InvalidOperationException>(
            () => eleicao.CadastrarCandidato("Bruno", 10));
    }

    [Fact]
    public void CadastrarCandidato_ComNomeOuNumeroInvalido_RejeitaCadastro()
    {
        var eleicao = new EleicaoManager();

        Assert.Throws<ArgumentException>(
            () => eleicao.CadastrarCandidato(" ", 10));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => eleicao.CadastrarCandidato("Ana", 0));
    }

    [Fact]
    public void RegistrarVoto_EleitorNaoCadastrado_RejeitaVoto()
    {
        var eleicao = new EleicaoManager();

        Assert.Throws<InvalidOperationException>(
            () => eleicao.RegistrarVoto("123", TipoVoto.Branco));
    }

    [Fact]
    public void RegistrarVoto_ImpedeEleitorDeVotarDuasVezes()
    {
        var eleicao = new EleicaoManager();
        eleicao.CadastrarEleitor("123");
        eleicao.CadastrarCandidato("Ana", 10);
        eleicao.RegistrarVoto("123", TipoVoto.Candidato, 10);

        Assert.Throws<InvalidOperationException>(
            () => eleicao.RegistrarVoto("123", TipoVoto.Branco));
    }

    [Fact]
    public void RegistrarVoto_ParaCandidatoInexistente_ContaComoNulo()
    {
        var eleicao = new EleicaoManager();
        eleicao.CadastrarEleitor("123");

        eleicao.RegistrarVoto("123", TipoVoto.Candidato, 99);

        Assert.Equal(1, eleicao.Apuracao().VotosNulos);
    }

    [Fact]
    public void Apuracao_ContaVotosEIdentificaVencedor()
    {
        var eleicao = new EleicaoManager();
        eleicao.CadastrarCandidato("Ana", 10);
        eleicao.CadastrarCandidato("Bruno", 20);
        eleicao.CadastrarEleitor("123");
        eleicao.CadastrarEleitor("456");
        eleicao.CadastrarEleitor("789");
        eleicao.CadastrarEleitor("321");

        eleicao.RegistrarVoto("123", TipoVoto.Candidato, 10);
        eleicao.RegistrarVoto("456", TipoVoto.Candidato, 10);
        eleicao.RegistrarVoto("789", TipoVoto.Branco);
        eleicao.RegistrarVoto("321", TipoVoto.Nulo);

        var resultado = eleicao.Apuracao();

        Assert.Equal(2, resultado.VotosPorCandidato[10]);
        Assert.Equal(0, resultado.VotosPorCandidato[20]);
        Assert.Equal(1, resultado.VotosBrancos);
        Assert.Equal(1, resultado.VotosNulos);
        Assert.Equal("Ana", resultado.Vencedor?.Nome);
    }

    [Fact]
    public void Apuracao_SemVotosOuComEmpate_NaoDefineVencedor()
    {
        var eleicao = new EleicaoManager();
        eleicao.CadastrarCandidato("Ana", 10);
        eleicao.CadastrarCandidato("Bruno", 20);

        Assert.Null(eleicao.Apuracao().Vencedor);

        eleicao.CadastrarEleitor("123");
        eleicao.CadastrarEleitor("456");
        eleicao.RegistrarVoto("123", TipoVoto.Candidato, 10);
        eleicao.RegistrarVoto("456", TipoVoto.Candidato, 20);

        Assert.Null(eleicao.Apuracao().Vencedor);
    }
}
