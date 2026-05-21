using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
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

            var valido = Validator.TryValidateObject(
                cliente,
                contexto,
                erros,
                true
                );

            //validação do CPF
            if (list.Any(x => x.CPF == cliente.CPF))
            {
                erros.Add(new ValidationResult(
                    "Já existe outro cliente utilizando esse CPF!",
                    new[] { "CPF" }
                    ));

                valido = false;
            }
            //validação do email
            if (!string.IsNullOrEmpty(cliente.Email) && !cliente.Email.Contains("@"))
            {
                erros.Add(new ValidationResult(
                    "E-mail inválido, por favor, insira um e-mail válido!",
                    new[] {"Email"}
                    ));

                valido = false;
            }

            foreach(var erro in erros)
            {
                Console.WriteLine("{0}: {1}",
                    erro.MemberNames.First(),
                    erro.ErrorMessage);
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
                .Where(x => x.Nome.Contains(texto) ||
                x.Email.Contains(texto) ||
                x.CPF == texto
                )
                .ToList();
        }
        //listar cliente
        public List<Cliente> Listar(int pageSize, int page)
        {
            var take = pageSize;
            var skip = (page - 1) * pageSize;
            return list.Skip(skip).Take(take).ToList();
        }
    }
}
