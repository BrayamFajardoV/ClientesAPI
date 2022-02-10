using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientesAPI.Models.Dto
{
    /*ESTA CLASE SE VA A UTILIZAR PARA INDICAR AL USUARIO CUANDO SE HAGA UNA SOLICITUD*/
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; } 
        public string DisplayMessage { get; set; } 
        public List<string> ErrorMessage { get; set; } 

    }
}
