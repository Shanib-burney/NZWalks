using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalksAPI.Data
{
    public class NZWalksAuthDbContext: IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options) { 
        

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "e35ac09b-49be-4d3c-a3ba-0d7034f243bb";
            var writerRoleId = "37cc2a70-f7a5-41eb-ae45-6f696f76e34b";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id=readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name= "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                 new IdentityRole
                {
                    Id=writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name= "Writer",
                    NormalizedName = "Writer".ToUpper()
                }

            };
            builder.Entity<IdentityRole>().HasData(roles);
        }




    }
}
