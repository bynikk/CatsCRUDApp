using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
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
            await catService.CreateCat(model);

            return Ok("Add successfully");
        }

        // Put api/Cat
        [HttpPut]
        public async Task<IActionResult> Put(CatViewModel model)
        {
            await catService.UpdateCat(model);

            return Ok($"Object by {model.Id} id was updated successfully");
        }

        // DELETE api/Cat
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var existingCat = await catService.GetCat(id);

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