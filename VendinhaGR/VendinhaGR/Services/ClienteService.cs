using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;
using VendinhaGR.Models;

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
    }
}
