using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using CatsCRUDApp.Models;
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
        public async Task<IActionResult> Get()
        {
            return Ok(await catService.GetCats());
        }

        // POST api/Cat
        [HttpPost]
        public async Task<IActionResult> Post(CatViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CatViewModel, CatDTO>()).CreateMapper();
            CatDTO catDto = mapper.Map<CatViewModel, CatDTO>(model);

            await catService.CreateCat(catDto);

            return Ok("Add successfully");
        }

        // Put api/Cat
        [HttpPut]
        public async Task<IActionResult> Put(CatViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CatViewModel, CatDTO>()).CreateMapper();
            CatDTO catDto = mapper.Map<CatViewModel, CatDTO>(model);

            await catService.UpdateCat(catDto);

            return Ok($"Object by {model.Id} id was updated successfully");
        }

        // DELETE api/Cat
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            CatDTO existingCat = await catService.GetCat(id);
            if (Equals(existingCat, null)) return NotFound();

            if (existingCat != null)
            {
                await catService.DeleteCat(existingCat.Id);
            }
            else
            {
                return NotFound();
            }

            return Ok($"Object by {id} id  was removed successfully");
        }
    }
}