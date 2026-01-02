using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class WalkRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [Range(0, 100)]
        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }
    }
    public class WalkDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        public DifficultyDTO Difficulty { get; set; }

        public RegionDTO Region {  get; set; }
     
    }

    public class DifficultyDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class WalksQueryParameters
    {
        public string? filterOn { get; set; }
        public string? filterQuery { get; set; }
        public string? sortBy { get; set; }
        public bool? isAscending { get; set; }

        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 100;
    }

}
