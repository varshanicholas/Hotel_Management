using Hotel_Management.Model;
using HotelManagement.Repositories;
using HotelManagementNew.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddControllers().AddJsonOptions(opts => {
                opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddDbContext<HotelManagementContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("PropelAug24Connection")));
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();

            builder.Services.AddScoped<IGuestRepository, GuestRepository>();



            var app = builder.Build();

            

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
