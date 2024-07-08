using API.Context;
using API.Services.Implement;
using API.Services.Interfaces;
using UI.Models;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add DbContext
            builder.Services.AddDbContext<FoodShopDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Register Services
            // Admins 
            builder.Services.AddTransient<IAddable<Admin>, AdminSvc>();
            builder.Services.AddScoped<IReadable<Admin>, AdminSvc>();
            builder.Services.AddTransient<ILookupSvc<string, Admin>, AdminSvc>();
            builder.Services.AddTransient<IEditable<Admin>, AdminSvc>();
            builder.Services.AddTransient<IDeletable<string, Admin>, AdminSvc>();
            //FoodCategory
            builder.Services.AddTransient<IAddable<FoodCategory>, FoodCategorySvc>();
            builder.Services.AddScoped<IReadable<FoodCategory>, FoodCategorySvc>();
            builder.Services.AddTransient<ILookupSvc<string, FoodCategory>, FoodCategorySvc>();
            builder.Services.AddTransient<IEditable<FoodCategory>, FoodCategorySvc>();
            builder.Services.AddTransient<IDeletable<string, FoodCategory>, FoodCategorySvc>();
            //Food
            builder.Services.AddTransient<IAddable<Food>, FoodSvc>();
            builder.Services.AddScoped<IReadable<Food>, FoodSvc>();
            builder.Services.AddTransient<ILookupSvc<string, Food>, FoodSvc>();
            builder.Services.AddTransient<IEditable<Food>, FoodSvc>();
            builder.Services.AddTransient<IDeletable<string, Food>, FoodSvc>();
            //FoodType
            builder.Services.AddScoped<IReadable<FoodType>, FoodTypeSvc>();
            //Customer
            builder.Services.AddTransient<IAddable<Customer>, CustomerSvc>();
            builder.Services.AddScoped<IReadable<Customer>, CustomerSvc>();
            builder.Services.AddTransient<ILookupSvc<string, Customer>, CustomerSvc>();
            builder.Services.AddTransient<IEditable<Customer>, CustomerSvc>();
            builder.Services.AddTransient<IDeletable<string, Customer>, CustomerSvc>();
            //CustomerInformation
            builder.Services.AddScoped<ILookupSvc<int, CustomerInformation>, CustomerInformationSvc>();
            builder.Services.AddTransient<IAddable<CustomerInformation>, CustomerInformationSvc>();
            builder.Services.AddScoped<IReadable<CustomerInformation>, CustomerInformationSvc>();
            builder.Services.AddTransient<IDeletable<int, CustomerInformation>, CustomerInformationSvc>();
            builder.Services.AddScoped<IReadableHasWhere<string, CustomerInformation>, CustomerInformationSvc>();
            //Order
            builder.Services.AddScoped<IReadableHasWhere<string, Order>, OrderSvc>();
            builder.Services.AddTransient<ILookupSvc<int, Order>, OrderSvc>();
            builder.Services.AddTransient<IEditable<Order> , OrderSvc>();
            builder.Services.AddTransient<IAddable<Order>, OrderSvc>();
            //OrderItem
            builder.Services.AddScoped<IReadableHasWhere<int, OrderItem>, OrderItemSvc>();
            builder.Services.AddTransient<IAddable<OrderItem>, OrderItemSvc>();
            //Login
            builder.Services.AddTransient<ILoginSvc<Admin>, AdminLoginSvc>();
            builder.Services.AddTransient<ILoginSvc<Customer>, CustomerLoginSvc>();
            //Cart
            builder.Services.AddTransient<ILookupSvc<string, Cart>, CartSvc>();
            //Cart Item
            builder.Services.AddTransient<IAddable<CartItem>, CartItemSvc>();
            builder.Services.AddTransient<IReadableHasWhere<int, CartItem>, CartItemSvc>();
            builder.Services.AddTransient<IDeletable<List<CartItem>, CartItem>, CartItemSvc>();
            builder.Services.AddTransient<IEditable<CartItem>, CartItemSvc>();
            //Rate
            builder.Services.AddTransient<IAddable<Rating>, RateSvc>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
