﻿using AutoMapper;
using BLL.Entities;
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
            var cats = await catService.Get();

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Cat, CatViewModel>()).CreateMapper();
            var catsViewModel = mapper.Map<IEnumerable<Cat>, IEnumerable<CatViewModel>>(cats);

            return Ok(catsViewModel);
        }

        // POST api/Cat
        [HttpPost]
        public async Task<IActionResult> Post(CatViewModel model)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CatViewModel, Cat>()).CreateMapper();
            var cat = mapper.Map<CatViewModel, Cat>(model);

            await catService.Create(cat);

            return Ok("Add successfully");
        }

        // Put api/Cat
        [HttpPut]
        public async Task<IActionResult> Put(CatViewModel model)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CatViewModel, Cat>()).CreateMapper();
            var cat = mapper.Map<CatViewModel, Cat>(model);

            await catService.Update(cat);

            return Ok($"Object by {model.Id} id was updated successfully");
        }

        // DELETE api/Cat
        [HttpDelete]
        public async Task<IActionResult> Delete(CatViewModel model)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CatViewModel, Cat>()).CreateMapper();
            var cat = mapper.Map<CatViewModel, Cat>(model);

            var existingCat = await catService.GetCatById(cat);

            if (Equals(existingCat, null)) return NotFound();

            await catService.Delete(existingCat);

            return Ok($"Object by {cat.Id} id  was removed successfully");
        }
    }
}