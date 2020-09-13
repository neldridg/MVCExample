using Microsoft.EntityFrameworkCore;
using Web.Entities.Models;

namespace Web 
{
    public class CatPicContext : DbContext 
    {

        // This is done to allow dependency injection of DbContex in Startup.cs.
        // If you were hardcoding this, you can set most of your parameters here.
        public CatPicContext(DbContextOptions<CatPicContext> options) : base(options)
        {
        }
        
        // The general formula for creating an Entity is to create an object,
        // then create a DbSet typed with that object. This registers it with
        // Entity Framework when you create migrations or access your Database Context.
        public DbSet<CatPic> CatPics { get; set; }
    }

}