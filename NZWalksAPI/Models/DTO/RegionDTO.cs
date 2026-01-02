using NZWalksAPI.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class RegionDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }

        // Optional factory method
        public static RegionDTO FromEntity(Region region) => new RegionDTO
        {
            Id = region.Id,
            Code = region.Code,
            Name = region.Name,
            RegionImageUrl = region.RegionImageUrl
        };
    }

    public class RegionRequestDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be minimum of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be maximum of 3 characters")]
        public required string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be maximum of 100 characters")]
        public required string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
 
  
}
