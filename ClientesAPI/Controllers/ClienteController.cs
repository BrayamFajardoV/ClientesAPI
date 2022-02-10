using ClientesAPI.Data;
using ClientesAPI.Models;
using ClientesAPI.Models.Dto;
using ClientesAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClientesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _IClienteRepository;
        protected ResponseDTO _response;
        public ClienteController(IClienteRepository IClienteRepository )
        {
            _IClienteRepository = IClienteRepository;
            _response = new ResponseDTO();
        }


        // GET: api/<ClienteController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            try
            {
                var lista = await _IClienteRepository.GetClientes();
                _response.Result = lista;
                _response.DisplayMessage = "Lista de clientes";

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }

            return Ok(_response);
        }

        // GET api/<ClienteController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetClientes(int id) 
        {
            var cliente = await _IClienteRepository.GetClientesById(id);
            if (cliente == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Cliente no encontrado";
                return NotFound(_response);
            }
            _response.Result = cliente;
            _response.DisplayMessage = "Informacion del cliente";
            return Ok(_response);


        } 

        // POST api/<ClienteController>
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostClientes(ClienteDTO clientedto)
        {
            try
            {
                ClienteDTO model = await _IClienteRepository.CreateUpdate(clientedto);
                _response.Result = model;

                return CreatedAtAction("GetClientes", new { id = model.ClienteID }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error al crear el cliente";
                _response.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }

        }

        // PUT api/<ClienteController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientes(int id, ClienteDTO clientedto)
        {
            try
            {
                ClienteDTO model = await _IClienteRepository.CreateUpdate(clientedto);
                _response.Result = model;
                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.DisplayMessage = "Error al actualizar al cliente";
                _response.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }

       

        // DELETE api/<ClienteController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientes(int id)
        {
            try
            {
                bool estaEliminado = await _IClienteRepository.DeleteCliente(id);
                if (estaEliminado)
                {
                    _response.Result = estaEliminado;
                    _response.DisplayMessage = "Cliente eliminado con exito";
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.DisplayMessage = "Error al eliminar el cliente ";
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };

                return BadRequest(_response);
            }
          
        }
    }
}
