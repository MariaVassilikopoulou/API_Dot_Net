global using Microsoft.EntityFrameworkCore;
using API_med_dotNET.Models;

namespace API_med_dotNET
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {
            
        }

        public DbSet<Products> Products { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            // options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=API_med_dotNET;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }


    }

}

