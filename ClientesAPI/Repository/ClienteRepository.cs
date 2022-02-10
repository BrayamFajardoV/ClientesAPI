using AutoMapper;
using ClientesAPI.Data;
using ClientesAPI.Models;
using ClientesAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientesAPI.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;


        public ClienteRepository(ApplicationDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }


        public async Task<ClienteDTO> CreateUpdate(ClienteDTO clienteDTO)
        {
            Cliente cliente = _mapper.Map<ClienteDTO, Cliente>(clienteDTO);
            if (cliente.ClienteID > 0)
            {
                _db.cliente.Update(cliente);
            }
            else
            {
                await _db.cliente.AddAsync(cliente);
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<Cliente, ClienteDTO>(cliente);
        }

        public async Task<bool> DeleteCliente(int id)
        {
            try
            {
                Cliente cliente = await _db.cliente.FindAsync(id);
                if (cliente == null)
                {
                    return false;

                }
                _db.cliente.Remove(cliente);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public async Task<List<ClienteDTO>> GetClientes()
        {
            List<Cliente> lista = await _db.cliente.ToListAsync();

            return _mapper.Map<List<ClienteDTO>>(lista); 
        }

        public async Task<ClienteDTO> GetClientesById(int id)
        {
            Cliente cliente = await _db.cliente.FindAsync(id);

            return _mapper.Map<ClienteDTO>(cliente);
        }   
    }
}
