using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VendinhaGR.Models;

namespace VendinhaGR.Services
{
    public class DividaService
    {
        private List<Divida> list = new List<Divida>();

        //criação
        public bool Criar(Divida divida, out List<ValidationResult> erros)
        {
            if (!Validar(divida, out erros))
            {
                return false;
            }
            divida.DataCriacao = DateTime.Now;

            list.Add(divida);

            return true;
            
        }

        //validação

        public bool Validar(Divida divida, out List<ValidationResult> erros)
        {
            var contexto = new ValidationContext(divida);
            erros = new List<ValidationResult>();

            var valido = Validator.TryValidateObject(
                divida,
                contexto,
                erros,
                true
             );
             
            if(divida.Valor < -0)
            {
                erros.Add(new ValidationResult(
                    "O valor da dívida deve ser maior do que zero.",
                    new[] {"Valor"}
                    ));

                valido = false;
            }

            //so pode uma divida em aberto
            if(list.Any(x => x.ClienteId == divida.ClienteId && x.Situacao == false))
            {
                erros.Add(new ValidationResult(
                    "O cliente já passui uma dívida em aberto!",
                    new[] {"ClienteId"}
                    ));

                valido=false;
            }

            foreach(var erro in erros)
            {
                Console.WriteLine("{0}: {1}",
                    erro.MemberNames.First(),
                    erro.ErrorMessage);
            }

            return valido;
        }

        //listar dividas
        public List<Divida> Listar()
        {
            return list.ToList();
        }
        
        //buscar divida
        public Divida Buscar(int id)
        {
            return list.FirstOrDefault(x => x.Id == id);
        }

        //listar dividas por cliente
        public List<Divida> BuscarPorCliente(int clienteId)
        {
            return list
                .Where(x=> x.ClienteId == clienteId).ToList();
        }
        //marcar pagamento
        public bool Pagar(int id)
        {
            var divida = Buscar(id);
            if (divida == null)
            {
                return false;
            }

            divida.Situacao = true;
            divida.DataPagamento = DateTime.Now;

            return true;
        }

    }
}
