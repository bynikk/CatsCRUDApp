using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using CatsCRUDApp.Models;
using DAL.EF;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CatsCRUDApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatController : ControllerBase
    {
        ICatService catService;

        public CatController(ICatService serv)
        {
            catService = serv;
        }

        // GET api/Cat
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(catService.GetCats());
        }

        // POST api/Cat
        [HttpPost]
        public ActionResult Post(CatViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CatViewModel, CatDTO>()).CreateMapper();
            CatDTO catDto = mapper.Map<CatViewModel, CatDTO>(model);

            CatDTO existingCat = catService.GetCat(catDto.Id);
            if (existingCat != null)
            {
                catService.CreateCat(catDto);
            }
            else
            {
                return NotFound();
            }

            return Ok("Add successfully");
        }

        // Put api/Cat
        [HttpPut]
        public ActionResult Put(CatViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CatViewModel, CatDTO>()).CreateMapper();
            CatDTO catDto = mapper.Map<CatViewModel, CatDTO>(model);
            catService.UpdateCat(catDto);

            return Ok($"Object by {model.Id} id was updated successfully");
        }

        // DELETE api/Cat
        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            CatDTO existingCat = catService.GetCat(id);
            if (Equals(existingCat, null)) return NotFound();

            if (existingCat != null)
            {
                catService.DeleteCat(existingCat.Id);
            }
            else
            {
                return NotFound();
            }

            return Ok($"Object by {id} id  was removed successfully");
        }
    }
}