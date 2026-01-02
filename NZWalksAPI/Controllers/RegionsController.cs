using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;
using System.Text.Json;


namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        [HttpGet]
        //[Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAllRegions()
        {

            try {
                 var regions = await regionRepository.GetAllAsync();

                logger.LogInformation($"Finished getting data of regions with data {JsonSerializer.Serialize(regions)}");


                return Ok(mapper.Map<List<RegionDTO>>(regions));

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }



            //var regionsDTO = new List<RegionDTO>();

            //foreach(var region in regions)
            //{
            //    regionsDTO.Add(new RegionDTO
            //    {
            //        Id = region.Id,
            //        Name = region.Name,
            //        Code = region.Code,
            //        RegionImageUrl = region.RegionImageUrl
            //    });
            //}


        }

        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            var region = await regionRepository.GetByIdAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            //var regionDTO = new RegionDTO
            //{
            //    Id = region.Id,
            //    Name = region.Name,
            //    Code = region.Code,
            //    RegionImageUrl = region.RegionImageUrl
            //};


            return Ok(mapper.Map<RegionDTO>(region));
        }
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> AddRegion([FromBody] RegionRequestDTO regionRequestDTO)
        {

            //var region = new Region
            //{
            //    Name = regionRequestDTO.Name,
            //    Code = regionRequestDTO.Code,
            //    RegionImageUrl = regionRequestDTO.RegionImageUrl
            //};

            var region = mapper.Map<Region>(regionRequestDTO);
            region = await regionRepository.CreateAsync(region);

            var regionDTO = mapper.Map<RegionDTO>(region);

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDTO.Id }, regionDTO);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RegionRequestDTO regionRequestDTO)
        {


            var region = await regionRepository.UpdateAsync(id, regionRequestDTO);

            if (region == null)
            {
                return NotFound();
            }


            var regionDTO = RegionDTO.FromEntity(region);

            return Ok(regionDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {

            //var region = dbContext.Regions.Find(id);
            var region = await regionRepository.DeleteAsync(id);

            if (region == null)
            {
                return NotFound();
            }


            var regionDto = RegionDTO.FromEntity(region);


            return Ok(regionDto);

        }

    }
}
