using ClientesAPI.Models;
using ClientesAPI.Models.Dto;
using ClientesAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _UserRepository;
        protected ResponseDTO _responseDTO;
        public UsersController(IUserRepository userRepository)
        {
            _UserRepository = userRepository;
            _responseDTO = new ResponseDTO();
        }


        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserDTO userDTO) 
        {

            var respuesta = await _UserRepository.Register(
                new User
                {
                    UserName = userDTO.UserName

                }, userDTO.Password);

            if (respuesta == -1)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.DisplayMessage = ("Usuario ya existente");
                return BadRequest(_responseDTO);
            }
            if (respuesta == -500)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.DisplayMessage = "Error al crear el usuario";
                return BadRequest(_responseDTO);

            }
            _responseDTO.DisplayMessage = "Usuario creado con exito";
            _responseDTO.Result = respuesta;
            return Ok(_responseDTO);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserDTO user) 
        {
            var respuesta = await _UserRepository.Login(user.UserName, user.Password);

            if (respuesta == "Nouser")
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.DisplayMessage = "Usuario no existente";
                return BadRequest(_responseDTO);

            }
            if (respuesta == "wrong password")
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.DisplayMessage = "Clave Incorrecta";
                return BadRequest(_responseDTO);

            }
            _responseDTO.Result = respuesta;
            _responseDTO.DisplayMessage = "Usuario Conectado";
            return Ok(_responseDTO);
        }

    }
}
