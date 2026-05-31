Vendinha Plena - API de controle de Fiados

Olá! Esta é uma API RESTful desenvolvida em .NET Core para informatizar o controle de contas e dívidas (fiado) dos clientes de uma pequena venda, substituindo o controle de dívidas pagas no papel.

* Tecnologias Utilizadas

Linguagem: C#

Framework: .NET 8.0 (ASP.NET Core Web API)

ORM: Entity Framework Core

Banco de Dados: SQLite

Documentação de API: Swagger / OpenAPI

Arquitetura do Projeto

A solução foi dividida em dois projetos para garantir uma melhor separação de responsabilidades. As duas partes são:

VendinhaAPI: Onde o Projeto Web API ficou responsável pela apresentação dos Controllers e a documentação (Swagger).
VendinhaGR: Onde o projeto atua como núcleo da aplicação, contendo os Models, DTOs, Banco de Dados e o trabalho árduo dentro dos Services (cliente e dívida).

Funcionalidades Implementadas:

O sistema atende a todos os requisitos solicitados.

* Clientes:

-Cadastro completo: Com validação de campos obrigatórios.

-Validação de CPF: Onde construímos um algoritmo matemático real implementado para garantir que não entrem CPFs inválidos ou CPFs duplicados.

-Cálculo de idade: A idade do cliente é calculada dinamicamente na hora da listagem, baseada na sua data de nascimento.

-Listagem inteligente: Os clientes retornam paginados de 10 em 10 e ordenados automaticamente do maior devedor para o menor devedor, exibindo o montante total de suas dívidas (histórico completo).

-Filtro de pesquisa: Busca por nome do cliente que também é paginado e ordenado.

* Dívidas:

-Criação de dívidas: Com valor referente a um cliente.

-Regra de bloqueio: O sistema bloqueia a criação de uma nova dívida se o cliente já possuir uma dívida em aberto.

-Validação de existência: O sistema barra tentativas de criar dívidas para clientes que não existem no banco, evitando falhas de Foreign Keys.

-Baixa de pagamento automática: Registrando a data em que a conta foi paga.

-Listagem de dívidas: Gerais ou por cliente, com paginação de 10 em 10 e ordenação por valor (os valores mais altos vêm primeiro).

* Como executar/testar o projeto:

* Pré-requisitos:
.NET 8 SDK instalado em sua máquina.

* Passo a passo:

1- Clone o repositório: git clone https://github.com/SEU_USUARIO/VendinhaPlena.git

2- Navegue até a pasta raiz della solução: cd VendinhaPlena

3- Restaure os pacotes NuGet: dotnet restore

4- Preparação e inicialização do Banco de Dados: O projeto utiliza o SQLite como mecanismo de persistência, armazenando os dados localmente no arquivo vendinha.db.
A aplicação foi configurada para criar o banco de dados e aplicar as tabelas automaticamente na primeira execução. Sendo assim, basta iniciar o projeto para que a base seja estruturada.

5- Inicie a API: dotnet run --project VendinhaAPI

6 - Acesse o Swagger: Assim que o terminal indicar que o serviço está de pé, no Visual Studio, vá até o canto inferior direito e procure por uma aba chamada "Saída". Assim que estiver lá, suba um pouco até encontrar o link da aplicação. Pressione Ctrl + clique para abri-lo e, na barra de URL do seu navegador, adicione /swagger no final para abrir o Swagger. Lá você poderá realizar as requisições.

7- Script do Banco de Dados: Para fins de avaliação, um script de criação do banco de dados chamado script_banco.sql foi gerado via Entity Framework e ele fica na raiz deste repositório. Ele contém todas as instruções DDL para a criação de tabelas (Clientes, Dívidas) com seus respectivos relacionamentos e restrições.

Arquivo .gitignore gerado por: https://www.toptal.com/developers/gitignore


