using BookAPI.Data;
using BookAPI.Repositories.Clientes;
using BookAPI.Repositories.Livros;
using BookAPI.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BookDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			 .AddJwtBearer(option =>
			 {
				 option.TokenValidationParameters = new TokenValidationParameters
				 {
					 ValidateIssuer = false,
					 ValidateAudience = false,
					 ValidateLifetime = true,
					 ValidateIssuerSigningKey = true,

					 ValidIssuer = "Teste.Securiry.Bearer",
					 ValidAudience = "Teste.Securiry.Bearer",
					 IssuerSigningKey = JwtSecurityKey.Create("Secret_Key-12345678")
				 };

				 option.Events = new JwtBearerEvents
				 {
					 OnAuthenticationFailed = context =>
					 {
						 Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
						 return Task.CompletedTask;
					 },
					 OnTokenValidated = context =>
					 {
						 Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
						 return Task.CompletedTask;
					 }
				 };
			 });

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAngularApp", policy =>
	{
		policy.WithOrigins("http://localhost:4200")
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});

//builder.Services.AddCors(options => options.AddPolicy("AllowAngularApp", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

app.UseCors("AllowAngularApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
