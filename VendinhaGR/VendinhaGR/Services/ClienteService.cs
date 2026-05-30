using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VendinhaGR.Models;
using VendinhaGR.Dtos;
using VendinhaGR.Data;
using Microsoft.EntityFrameworkCore;

namespace VendinhaGR.Services
{
    public class ClienteService
    {
        private readonly AppDbContext _context;

        // puxando o banco de dados pra ca
        public ClienteService(AppDbContext context)
        {
            _context = context;
        }

        // criando cliente novo
        public bool Criar(Cliente cliente, out List<ValidationResult> erros)
        {
            if (!Validar(cliente, out erros))
            {
                return false;
            }

            _context.Clientes.Add(cliente);
            _context.SaveChanges(); // salva no banco real agora
            return true;
        }

        // checando se ta tudo certo com o cliente
        public bool Validar(Cliente cliente, out List<ValidationResult> erros)
        {
            var contexto = new ValidationContext(cliente);
            erros = new List<ValidationResult>();

            bool valido = Validator.TryValidateObject(cliente, contexto, erros, true);

            // testando se o cpf é de vdd msm
            if (!CpfValido(cliente.CPF))
            {
                erros.Add(new ValidationResult("O CPF informado não é válido!", new[] { "CPF" }));
                valido = false;
            }

            // travando se ja tiver outro mano com esse cpf
            bool cpfJaCadastrado = _context.Clientes.Any(x => x.CPF == cliente.CPF && x.Id != cliente.Id);
            if (cpfJaCadastrado)
            {
                erros.Add(new ValidationResult("Já existe outro cliente utilizando esse CPF!", new[] { "CPF" }));
                valido = false;
            }

            // checando o email
            if (!string.IsNullOrEmpty(cliente.Email) && !new EmailAddressAttribute().IsValid(cliente.Email))
            {
                erros.Add(new ValidationResult("E-mail inválido!", new[] { "Email" }));
                valido = false;
            }

            return valido;
        }


        public List<ClienteResumoDto> Listar(int pageSize, int page)
        {
            // busca no banco e ja soma todas as dividas (historico total)
            var query = _context.Clientes
                .Select(c => new
                {
                    Cliente = c,
                    // pra somar tudo de uma vez
                    TotalDividas = c.Dividas.Sum(d => (decimal?)d.Valor) ?? 0
                })
                .OrderByDescending(x => x.TotalDividas) 
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return query.Select(x => new ClienteResumoDto
            {
                Id = x.Cliente.Id,
                Nome = x.Cliente.Nome,
                CPF = x.Cliente.CPF,
                Idade = x.Cliente.Idade,
                Email = x.Cliente.Email,
                TotalDividas = x.TotalDividas
            }).ToList();
        }

        // pesquisa por nome tipo um filtro 
        public List<ClienteResumoDto> Pesquisa(string texto, int pageSize, int page)
        {
            var query = _context.Clientes
                .Where(x => EF.Functions.Like(x.Nome, $"%{texto}%"))
                .Select(c => new
                {
                    Cliente = c,
                    // somando todas as dividas do cara tbm
                    TotalDividas = c.Dividas.Sum(d => (decimal?)d.Valor) ?? 0
                })
                .OrderByDescending(x => x.TotalDividas)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return query.Select(x => new ClienteResumoDto
            {
                Id = x.Cliente.Id,
                Nome = x.Cliente.Nome,
                CPF = x.Cliente.CPF,
                Idade = x.Cliente.Idade,
                Email = x.Cliente.Email,
                TotalDividas = x.TotalDividas
            }).ToList();
        }

        // atualizando os dados do cliente
        public bool Atualizar(UpdateClienteDto dto, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();

            // acha o cliente no banco primeiro pelo id
            var clienteExistente = _context.Clientes.FirstOrDefault(x => x.Id == dto.Id);
            if (clienteExistente == null)
            {
                erros.Add(new ValidationResult("Cliente não encontrado", new[] { "Id" }));
                return false;
            }

            // cria um fake so pra rodar na validacao
            var clienteValida = new Cliente()
            {
                Id = dto.Id,
                Nome = dto.Nome,
                CPF = dto.CPF,
                DataNascimento = dto.DataNascimento,
                Email = dto.Email,
            };

            if (!Validar(clienteValida, out erros))
            {
                return false;
            }

            // se passou em tudo salva as novas alteracoes do cliente
            clienteExistente.Nome = dto.Nome;
            clienteExistente.CPF = dto.CPF;
            clienteExistente.DataNascimento = dto.DataNascimento;
            clienteExistente.Email = dto.Email;

            _context.SaveChanges();
            return true;
        }

        // apagar cliente
        public bool Excluir(int id, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();

            var cliente = _context.Clientes.FirstOrDefault(x => x.Id == id);
            if (cliente == null)
            {
                erros.Add(new ValidationResult("Cliente não encontrado", new[] { "Id" }));
                return false;
            }

            _context.Clientes.Remove(cliente);
            _context.SaveChanges();
            return true;
        }

        // continha chata pra validar o cpf msm (pesquisa)
        private static bool CpfValido(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            if (cpf.Length != 11) return false;

            string[] invalidos = {
                "00000000000", "11111111111", "22222222222", "33333333333", "44444444444",
                "55555555555", "66666666666", "77777777777", "88888888888", "99999999999"
            };
            if (invalidos.Contains(cpf)) return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}