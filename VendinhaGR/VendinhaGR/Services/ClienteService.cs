using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;
using VendinhaGR.Models;
using VendinhaGR.Dtos;

namespace VendinhaGR.Services
{
    public class ClienteService
    {
        private List<Cliente> list = new List<Cliente>();

        //criação cliente
        public bool Criar(Cliente cliente, out List<ValidationResult> erros)
        {
            if (!Validar(cliente, out erros))
            {
                return false;
            }

            list.Add(cliente);
            return true;
        }

        //validação cliente
        public bool Validar(Cliente cliente, out List<ValidationResult> erros)
        {
            var contexto = new ValidationContext(cliente);
            erros = new List<ValidationResult>();

            bool valido = Validator.TryValidateObject(
                cliente,
                contexto,
                erros,
                true
            );

            bool cpfJaCadastrado = list.Any(x => x.CPF == cliente.CPF);

            if  (cpfJaCadastrado)
            {
                erros.Add(new ValidationResult(
                    "Já existe outro cliente utilizando esse CPF!",
                    new[] { "CPF" }
                ));

                valido = false;
            }

            if (!string.IsNullOrEmpty(cliente.Email) &&
                !new EmailAddressAttribute().IsValid(cliente.Email))
            {
                erros.Add(new ValidationResult(
                    "E-mail inválido!",
                    new[] { "Email" }
                ));

                valido = false;
            }

            return valido;
        }

        public List<Cliente> Listar()
        {
            return list.ToList();
        }

        //buscando cliente
        public Cliente Buscar(string cpf)
        {
            return list.FirstOrDefault(x => x.CPF == cpf);
        }

        //pesquisar cliente
        public List<Cliente> Pesquisa(string texto)
        {
            return list
                .Where(x => x.Nome.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                x.Email != null && x.Email.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                x.CPF == texto
                )
                .ToList();
        }

        //listar cliente
        public List<Cliente> Listar(int pageSize, int page)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        //atualizar cliente
        public bool Atualizar(UpdateClienteDto dto, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();

            //procurando o cliente na lista pelo id do cliente
            var clienteExistente = list.FirstOrDefault(x => x.Id == dto.Id);
            if (clienteExistente == null)
            {
                erros.Add(new ValidationResult(
                    "Cliente não encontrado",
                    new[] {"Id"}
                    ));
                return false;
            }
            //criando um temporario para rodar na função validar
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
                //aqui fazendo uma variavel que so bloqueia se o CPF for de outro cliente
                bool cpfDeOutroCliente = list.Any(x => x.CPF == dto.CPF && x.Id != dto.Id);
                //agora aqui preciso usar essa variavel para fazer a verificação
                if (cpfDeOutroCliente) //se o cpf de outro cliente for true
                {
                    return false;
                }
                else //se o cpf de outro cliente for false
                {
                    erros.RemoveAll(x => x.ErrorMessage != null && x.ErrorMessage.Contains("CPF"));
                    if (erros.Count > 0) return false;
                }
            }
            //se passou em tudo salva as novas alterações que o cliente fez na lista
            clienteExistente.Nome = dto.Nome;
            clienteExistente.CPF = dto.CPF;
            clienteExistente.DataNascimento = dto.DataNascimento;
            clienteExistente.Email = dto.Email;

            return true;
            
            
        }
        //excluir cliente
        public bool Excluir(int id, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();

            var cliente = list.FirstOrDefault(x => x.Id == id);
            if (cliente == null)
            {
                erros.Add(new ValidationResult(
                    "Cliente não encontrado",
                    new[] {"Id"}
                    ));
                return false;
            }
            list.Remove(cliente);
            return true;

        }
    }
}
