# Sistema de Eleição

Aplicação de console em .NET para cadastrar candidatos e eleitores, registrar votos e exibir a apuração.

## Requisitos

- .NET 10 SDK

## Executar

Na raiz do repositório:

```powershell
dotnet run --project .\SistemaEleicao.Console\SistemaEleicao.Console.csproj
```

No menu, cadastre os candidatos e eleitores antes de registrar votos. Ao votar, informe o número do candidato, `0` para voto branco ou `-1` para voto nulo. Um número de candidato não cadastrado é contado como voto nulo. Cada eleitor cadastrado pode votar uma única vez.

A opção **Apurar votos** mostra o total por candidato, os votos nulos e brancos e o vencedor. Em caso de empate ou ausência de votos válidos, não há vencedor.

## Testes

Execute a suíte xUnit na raiz:

```powershell
dotnet test
```
