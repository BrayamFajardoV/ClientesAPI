using AutoMapper;
using ClientesAPI.Models;
using ClientesAPI.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientesAPI
{
    public class MappingConfiguration
    {
        public static MapperConfiguration RegisterMaps() 
        {
            var mappingConfig = new MapperConfiguration(config => 
            {
                config.CreateMap<ClienteDTO, Cliente>();
                config.CreateMap<Cliente, ClienteDTO>();
            });

            return mappingConfig;
        
        }


    }
}
