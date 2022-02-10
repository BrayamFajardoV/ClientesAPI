using ClientesAPI.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientesAPI.Repository
{
    public interface IClienteRepository
    {
        Task<List<ClienteDTO>> GetClientes();
        Task<ClienteDTO> GetClientesById(int id);
        Task<ClienteDTO> CreateUpdate(ClienteDTO clienteDTO);
        Task<bool> DeleteCliente(int id);


    }
}
