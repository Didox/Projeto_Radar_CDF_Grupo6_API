using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RadarG6.Repositorios.Interfaces;
using RadarG6.Servicos;
using RadarWebApi.DTOs;
using RadarWebApi.Models;

namespace RadarG6_webAPI.Controllers;


[Route("pedidos")]
[ApiController]
    public class PedidosController : ControllerBase
    {
        private IServico<Pedido> _servico;

        public PedidosController(IServico<Pedido> servico)
        {
            _servico = servico;
        }

        // GET: Pedidos

        [HttpGet("")]
        [Authorize(Roles = "adm,editor")]
        public async Task<IActionResult> Index()
        {
            var pedidos = await _servico.TodosAsync();
            return StatusCode(200, pedidos);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "adm,editor")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var pedido = (await _servico.TodosAsync()).Find(c => c.Id == id);

            return StatusCode(200, pedido);
        }

        // POST: Pedidos
        [HttpPost("")]
        [Authorize(Roles = "adm,editor")]
        public async Task<IActionResult> Create([FromBody] PedidoDTO pedidoDTO)
        {
            var pedido = BuilderServico<Pedido>.Builder(pedidoDTO);
            await _servico.IncluirAsync(pedido);
            return StatusCode(201, pedido);
        }


        // PUT: Pedidos/5
        [HttpPut("{id}")]
        [Authorize(Roles = "adm")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return StatusCode(400, new
                {
                    Mensagem = "O Id do pedido precisa bater com o id da URL"
                });
            }

            var pedidoDb = await _servico.AtualizarAsync(pedido);

            return StatusCode(200, pedidoDb);
        }

        // DELETE: Pedidos/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "adm")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var pedidoDb = (await _servico.TodosAsync()).Find(c => c.Id == id);
            if (pedidoDb is null)
            {
                return StatusCode(404, new
                {
                    Mensagem = "O pedido informado não existe"
                });
            }

            await _servico.ApagarAsync(pedidoDb);

            return StatusCode(204);
        }
    }