using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParkingAPI.Helpers;
using ParkingAPI.V1.Models;
using ParkingAPI.V1.Models.DTO;
using ParkingAPI.V1.Models.Enums;
using ParkingAPI.V1.Repositories.Contracts;
using System;

namespace ParkingAPI.V1.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/parking")]
    [ApiVersion("1.0")]
    public class ControllersParkingSpace : ControllerBase
    {
        private readonly IParkingSpaceRepository _repository;
        private readonly IMapper _mapper;

        public ControllersParkingSpace(IParkingSpaceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        /// <summary>
        /// Oeração que retorna do banco de dados todas as vagas existentes, sejam elas ativas ou desativadas
        /// </summary>
        /// <param name="query"> Usado para paginação </param>
        /// <returns> Listagem das vagas cadastradas </returns>
        [HttpGet("", Name = "NameGetAll")]
        public ActionResult GetAll([FromQuery] ParkingSpaceUrlQuery query)
        {
            var items = _repository.GetAll(query);

            if (items.Results.Count == 0)
                return NotFound();

            var lista = _mapper.Map<PaginationList<ParkingSpace>, PaginationList<ParkingSpaceDTO>>(items);

            foreach(ParkingSpaceDTO parkingSpaceDTO in lista.Results)
            {
                parkingSpaceDTO.Links.Add(new LinkDTO("self", Url.Link("NameGetOne", new { id = parkingSpaceDTO.Id }), "GET"));
            }

            lista.Links.Add(new LinkDTO("self", Url.Link("NameGetAll", query), "GET"));

            if (items.Pagination != null)
            {
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(items.Pagination));

                if (!items.Pagination.IsValidPage(query.PageNumber.Value))
                    return NotFound();
                else
                {
                    if((query.PageNumber + 1) <= items.Pagination.TotalPages)
                    {
                        ParkingSpaceUrlQuery nextPage = new ParkingSpaceUrlQuery { PageNumber = query.PageNumber + 1, RegisterQtt = query.RegisterQtt };
                        lista.Links.Add(new LinkDTO("next", Url.Link("NameGetAll", nextPage), "GET"));
                    }
                    
                    if((query.PageNumber - 1) > 0)
                    {
                        ParkingSpaceUrlQuery prevPage = new ParkingSpaceUrlQuery { PageNumber = query.PageNumber - 1, RegisterQtt = query.RegisterQtt };
                        lista.Links.Add(new LinkDTO("prev", Url.Link("NameGetAll", prevPage), "GET"));
                    }

                }
            }

            return Ok(lista);
        }


        /// <summary>
        ///     Solicita as informações relativa a apenas uma vaga.
        /// </summary>
        /// <param name="id"> Código identificador </param>
        /// <returns> Retorna um objeto ParkingSpace </returns>
        [HttpGet("{id}", Name = "NameGetOne")]
        public ActionResult GetOne(int id)
        {
            ParkingSpace parkingSpace = _repository.GetOne(id);

            if (parkingSpace == null)
                return StatusCode(404);

            ParkingSpaceDTO parkingSpaceDTO = _mapper.Map<ParkingSpace, ParkingSpaceDTO>(parkingSpace);

            parkingSpaceDTO.Links.Add(new LinkDTO("self", Url.Link("NameGetOne", new { id = parkingSpaceDTO.Id }), "GET"));
            parkingSpaceDTO.Links.Add(new LinkDTO("remove", Url.Link("NameRemove", new { id = parkingSpaceDTO.Id }), "DELETE"));
            parkingSpaceDTO.Links.Add(new LinkDTO("update", Url.Link("NameUpdate", new { id = parkingSpaceDTO.Id }), "PUT"));
            parkingSpaceDTO.Links.Add(new LinkDTO("parkControl", Url.Link("NameControlPark", new { id = parkingSpaceDTO.Id }), "PATCH"));

            return Ok(parkingSpaceDTO);
        }


        /// <summary>
        ///     Operacão que cadastra uma nova vaga para o estacionamento
        /// </summary>
        /// <param name="parkingSpace"> Um objeto ParkingSpace</param>
        /// <returns> Retorna o objeto ParkingSpace com seu ID</returns>
        [HttpPost("", Name = "NameRegister")]
        public ActionResult Register([FromBody] ParkingSpace parkingSpace)
        {
            if (parkingSpace == null)
                return BadRequest();

            parkingSpace.LastVacancy = DateTime.Now;
            parkingSpace.LastOccupation = null;

            _repository.Register(parkingSpace);

            ParkingSpaceDTO parkingSpaceDTO = _mapper.Map<ParkingSpace, ParkingSpaceDTO>(parkingSpace);
            parkingSpaceDTO.Links.Add(new LinkDTO("self", Url.Link("NameRegister", new { id = parkingSpaceDTO.Id }), "POST"));

            return Created($"api/parking/{parkingSpace.Id}", parkingSpaceDTO);
        }


        /// <summary>
        ///     Realiza a desativacao de uma vaga no sistema.
        /// </summary>
        /// <param name="id"> Código identificador da vaga</param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "NameRemove")]
        public ActionResult Remove(int id)
        {
            ParkingSpace parkingSpace = _repository.GetOne(id);

            if (parkingSpace == null)
                return StatusCode(404);
            _repository.Remove(id);

            return NoContent();
        }


        /// <summary>
        ///     Realiza a atualização relacionada a ativacão e desativação de uma vaga específica
        /// </summary>
        /// <param name="id"> Código identificador da vaga a ser alterada </param>
        /// <param name="parkingSpace"> Objeto ParkingSpace com os campos para alteracao</param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "NameUpdate")]
        public ActionResult Update(int id, [FromBody] ParkingSpace parkingSpace)
        {
            ParkingSpace parkingSpaceUpdate = _repository.GetOne(id);

            if (parkingSpaceUpdate == null)
                return StatusCode(404);

            if (parkingSpace == null)
                return BadRequest();

            if (parkingSpace.Id < 1)
                return NotFound();

            parkingSpace.Id = id;

            parkingSpace.LastOccupation = parkingSpaceUpdate.LastOccupation;
            parkingSpace.LastVacancy = parkingSpaceUpdate.LastVacancy;
            parkingSpace.ParkingStatus = parkingSpaceUpdate.ParkingStatus;

            _repository.Update(parkingSpace);

            ParkingSpaceDTO parkingSpaceDTO = _mapper.Map<ParkingSpace, ParkingSpaceDTO>(parkingSpace);
            parkingSpaceDTO.Links.Add(new LinkDTO("self", Url.Link("NameUpdate", new { id = parkingSpaceDTO.Id }), "PUT"));

            return Ok();
        }


        /// <summary>
        ///     Realiza o controle de ocupação da vaga modificando o estado atual ao ser chamada.
        /// </summary>
        /// <param name="id"> Código identificador da vaga que será manipulada</param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "NameControlPark")]
        public ActionResult ControlPark(int id)
        {
            ParkingSpace parkingSpace = _repository.GetOne(id);

            if (parkingSpace == null || parkingSpace.ParkingSapceActive == false)
                return StatusCode(404);

            if (parkingSpace.ParkingStatus == ParkingStatus.Free)
            {
                parkingSpace.ParkingStatus = ParkingStatus.Busy;
                parkingSpace.LastOccupation = DateTime.Now;
            }
            else
            {
                parkingSpace.ParkingStatus = ParkingStatus.Free;
                parkingSpace.LastVacancy = DateTime.Now;
            }

            _repository.ControlPark(parkingSpace);

            ParkingSpaceDTO parkingSpaceDTO = _mapper.Map<ParkingSpace, ParkingSpaceDTO>(parkingSpace);
            parkingSpaceDTO.Links.Add(new LinkDTO("self", Url.Link("NameControlPark", new { id = parkingSpaceDTO.Id }), "PATCH"));

            return Ok();
        }
    }
}
