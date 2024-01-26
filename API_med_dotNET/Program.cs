
using API_med_dotNET.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace API_med_dotNET
{
	public class Program
	{
		public static void Main(string[] args)
		{


			var builder = WebApplication.CreateBuilder(args);
			//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
			});

			//

			// Add services to the container.
			builder.Services.AddAuthorization();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<DataContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




			//builder.Services.AddDbContext<DataContext>(options =>
			//   options.UseSqlServer("DefaultConnection"));//"Server=.\\SQLExpress;Database=API_med_dotNET;Trusted_Connection=true;TrustServerCertificate=true;"));//("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=API_med_dotNET;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"));
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "API_med_dotNet");
					c.RoutePrefix = "swagger";
				});
			}

			app.UseHttpsRedirection();
		
			app.UseAuthorization();

			//        var products = new List<Products>
			//        {
			//            new Products { Id =1, ProductName= "Cheescake", ProductPrice= 125, ProductType= "cake"
			//}
			//        };

			app.MapGet("/products", async (DataContext context) =>
				await context.Products.ToListAsync());


			app.MapGet("/product/{id}", async (DataContext context, int id) => await context.Products.FindAsync(id) is Products product ?

				//var product = products.Find(p => p.Id == id);
				//if (product == null)
				//{
				//   return Results.NotFound("Sorry no Product with that Id!");
				//}
				Results.Ok(product) :
				Results.NotFound("Sorry no Product with that Id!")
			);

			app.MapPost("/product", async (DataContext context, Products product) =>
			{
				context.Products.Add(product);
				await context.SaveChangesAsync();
				return Results.Ok(await context.Products.ToListAsync());
			});

			app.MapPut("/product/{id}", async (DataContext context, int id, Products product) =>
			{
				var productToUpdate = await context.Products.FindAsync(id);
				if (productToUpdate == null)
				{
					return Results.NotFound("No product to Update with tha Id!");
				}

				productToUpdate.ProductName = product.ProductName;
				productToUpdate.ProductPrice = product.ProductPrice;
				productToUpdate.ProductType = product.ProductType;
				await context.SaveChangesAsync();
				return Results.Ok(await context.Products.ToListAsync());
			});


			app.MapDelete("/product/{id}", async (DataContext context, int id) =>
			{
				var productToDelete = await context.Products.FindAsync(id);
				if (productToDelete == null)
				{
					return Results.NotFound("There is no product to Delete with that Id!");
				}
				context.Products.Remove(productToDelete);
				await context.SaveChangesAsync();
				return Results.Ok(await context.Products.ToListAsync());
			});
			app.Run();
		}
		}
	}
