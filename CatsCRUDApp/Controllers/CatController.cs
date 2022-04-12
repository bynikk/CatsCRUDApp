using AutoMapper;
using BLL.Entities;
using BLL.Interfaces;
using BLL.Mediator;
using BLL.Mediator.Components;
using CatsCRUDApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatsCRUDApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatController : ControllerBase
    {
        ICatService catService;
        IMapper mapper;
        IMediator mediator;

        CreateComponent createComponent;
        DeleteComponent deleteComponent;
        UpdateComponent updateComponent;
        GetComponent getComponent;


        public CatController(
            ICatService catService,
            IMapper mapper)
        {
            this.catService = catService;
            this.mapper = mapper;

            this.createComponent = new CreateComponent();
            this.deleteComponent = new DeleteComponent();
            this.updateComponent = new UpdateComponent();
            this.getComponent = new GetComponent();

            this.mediator = new ConcreteMediator(
                createComponent,
                updateComponent,
                deleteComponent,
                getComponent,
                catService);
        }

        // GET api/Cat
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cats = await getComponent.Get();

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

            createComponent.Create(cat);

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

            updateComponent.Update(cat);

            return Ok($"Object by {model.Id} id was updated successfully");
        }

        // DELETE api/Cat
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            deleteComponent.Delete(id);
            return Ok($"Object by {id} id  was removed successfully");
        }
    }
}