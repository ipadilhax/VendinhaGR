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
    public class DividaService
    {
        private readonly AppDbContext _context;

        // puxando o banco de dados pra ca tbm
        public DividaService(AppDbContext context)
        {
            _context = context;
        }

        public bool Criar(Divida divida, out List<ValidationResult> erros)
        {
            if (!Validar(divida, out erros))
            {
                return false;
            }

            // forçando os valores iniciais pq acabou de criar a conta
            divida.Paga = false;
            divida.DataCriacao = DateTime.Now;

            _context.Dividas.Add(divida);
            _context.SaveChanges(); // salva no banco de vdd

            return true;
        }


        public bool Validar(Divida divida, out List<ValidationResult> erros)
        {
            var contexto = new ValidationContext(divida);
            erros = new List<ValidationResult>();

            var valido = Validator.TryValidateObject(divida, contexto, erros, true);

            // checando se o cliente existe msm (pra n dar erro de chave estrangeira no banco)
            bool clienteExiste = _context.Clientes.Any(x => x.Id == divida.ClienteId);
            if (!clienteExiste)
            {
                erros.Add(new ValidationResult(
                    "cliente não encontrado",
                    new[] { "ClienteId" }
                ));
                valido = false;
            }

            if (divida.Valor <= 0)
            {
                erros.Add(new ValidationResult(
                    "O valor da dívida deve ser maior do que zero",
                    new[] { "Valor" }
                ));

                valido = false;
            }


            // ignora a propria divida na hora de atualizar (x.Id != divida.Id)
            bool jaTaDevendo = _context.Dividas.Any(x => x.ClienteId == divida.ClienteId && x.Paga == false && x.Id != divida.Id);

            if (jaTaDevendo)
            {
                erros.Add(new ValidationResult(
                    "O cliente já possui uma dívida em aberto, tem que pagar a anterior primeiro",
                    new[] { "ClienteId" }
                ));

                valido = false;
            }

            return valido;
        }



        public List<Divida> Listar(int pageSize, int page)
        {
            return _context.Dividas
                .OrderByDescending(x => x.Valor) 
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        // buscar uma divida especifica pelo id
        public Divida Buscar(int id)
        {
            return _context.Dividas.FirstOrDefault(x => x.Id == id);
        }


        public List<Divida> BuscarPorCliente(int clienteId, int pageSize, int page)
        {
            return _context.Dividas
                .Where(x => x.ClienteId == clienteId)
                .OrderByDescending(x => x.Valor) 
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        public bool Pagar(int id)
        {
            var divida = Buscar(id);
            if (divida == null)
            {
                return false; 
            }

            divida.Paga = true;
            divida.DataPagamento = DateTime.Now; 

            _context.SaveChanges(); // atualiza no banco
            return true;
        }


        public bool Atualizar(UpdateDividaDto dto, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();

            var dividaExistente = Buscar(dto.Id);
            if (dividaExistente == null)
            {
                erros.Add(new ValidationResult("divida não encontrada", new[] { "Id" }));
                return false;
            }

            // checando se a regra continua batendo
            var dividaTemp = new Divida
            {
                Id = dto.Id,
                ClienteId = dividaExistente.ClienteId,
                Valor = dividaExistente.Valor,
                Paga = dto.Paga
            };

            if (!Validar(dividaTemp, out erros))
            {
                return false;
            }

            // se ele marcou como pago mas antes n tava, poe a data de hj
            if (dto.Paga && !dividaExistente.Paga)
            {
                dividaExistente.DataPagamento = DateTime.Now;
            }
            // se ele desmarcou o pagamento (voltou a dever), apaga a data
            else if (!dto.Paga)
            {
                dividaExistente.DataPagamento = null;
            }

            dividaExistente.Paga = dto.Paga;

            _context.SaveChanges();
            return true;
        }


        public bool Excluir(int id, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();

            var divida = Buscar(id);
            if (divida == null)
            {
                erros.Add(new ValidationResult("divida não encontrada", new[] { "Id" }));
                return false;
            }

            _context.Dividas.Remove(divida);
            _context.SaveChanges();
            return true;
        }
    }
}