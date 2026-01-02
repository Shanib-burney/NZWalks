using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;


namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walkRepository;

        public IMapper mapper { get; }

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }



        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] WalkRequestDTO addWalkRequest)
        {

            var walk = mapper.Map<Walk>(addWalkRequest);
            await walkRepository.CreateAsync(walk);

            return Ok(mapper.Map<WalkDTO>(walk));
        }



        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] WalksQueryParameters query)
        {
            var walk = await walkRepository.GetAllAsync(query.filterOn, query.filterQuery, query.sortBy,
              query.isAscending ?? true, query.pageNumber, query.pageSize);

            return Ok(mapper.Map<List<WalkDTO>>(walk));

        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {

            var walk = await walkRepository.GetByIdAsync(id);

            if (walk == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDTO>(walk));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] WalkRequestDTO walkRequestDTO)
        {
            var walk = await walkRepository.UpdateAsync(id, walkRequestDTO);

            if (walk == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDTO>(walk));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {

            var walk = await walkRepository.DeleteAsync(id);

            if (walk == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDTO>(walk));

        }
    }
}
