using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Data.Data;
using Data.Model;
using System.Text;
using onlineShopping.Repsitory.Interfaces;
using onlineShopping.Repsitory;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace onlineShopping
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add AutoMapper services
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(
                builder.Configuration.GetConnectionString("conn"),
                b => b.MigrationsAssembly("onlineShopping")));

            builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
                options.SignIn.RequireConfirmedEmail = true;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddControllers();

            // Register custom email sender
            //builder.Services.AddScoped<IEmailService, EmailRepository>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
          
            //---------------------------------- Add scoped repositories ---------------------------
            builder.Services.AddScoped<IRepstory<Product>, ProductRepsitory>();
            builder.Services.AddScoped<ICartItem, CartItemRepsitory>();
            builder.Services.AddScoped<ICart, CartRepsitory>();
            builder.Services.AddScoped<Iorder, OrderRepostory>();
            builder.Services.AddScoped<IOrderItem, OrderItemRepsitory>();
            builder.Services.AddScoped<Iproduct, ProductRepsitory>();
            builder.Services.AddScoped<IRepstory<Category>, CategoryRepsitory>();
            builder.Services.AddScoped<IRepstory<Coupon>, CouponRegsitory>();
            builder.Services.AddScoped<ICoupon, CouponRegsitory>();



            builder.Services.AddHttpClient();

            //------------------------------------------------------------

            // Authentication
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.RequireHttpsMetadata = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:valid_issur"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:valdid_audiance"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(builder.Configuration["JWT:secret"].PadRight(48)))
                };
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddCors(x =>
            {
                x.AddPolicy("mypolicy", y =>
                {
                    y.WithOrigins("http://localhost:4200")  // Replace with your frontend URL
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

            // Swagger configuration
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("mypolicy");
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
