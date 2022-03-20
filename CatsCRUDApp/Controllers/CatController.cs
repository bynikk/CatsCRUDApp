﻿using AutoMapper;
using BLL.Entities;
using BLL.Interfaces;
using CatsCRUDApp.Models;
using DAL.MongoDb;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace CatsCRUDApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatController : ControllerBase
    {
        ICatService catService;
        IMapper mapper;

        public CatController(ICatService catService,
            IMapper mapper)
        {
            this.catService = catService;
            this.mapper = mapper;
        }

        // GET api/Cat
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cats = await catService.Get();

            var catsViewModels = mapper.Map<IEnumerable<Cat>, IEnumerable<CatViewModel>>(cats);

            return Ok(catsViewModels);
        }

        // POST api/Cat
        [HttpPost]
        public async Task<IActionResult> Post(CatViewModel model)
        {
            var cat = mapper.Map<CatViewModel, Cat>(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            await catService.Create(cat);

            return Ok("Add successfully");
        }

        // Put api/Cat
        [HttpPut]
        public async Task<IActionResult> Put(CatViewModel model)
        {
            var cat = mapper.Map<CatViewModel, Cat>(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            await catService.Update(cat);

            return Ok($"Object by {model.Id} id was updated successfully");
        }

        // DELETE api/Cat
        [HttpDelete]
        public async Task<IActionResult> Delete(CatViewModel model)
        {
            var cat = mapper.Map<CatViewModel, Cat>(model);
             
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var existingCat = await catService.GetCatById(cat.Id);

            if (existingCat == null) return NotFound();

            await catService.Delete(existingCat);

            return Ok($"Object by {cat.Id} id  was removed successfully");
        }
    }
}