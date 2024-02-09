using InForm.Server.Db;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

const string corsPolicy = "";

builder.Services.AddCors(ops =>
{
	ops.AddPolicy(corsPolicy, policy =>
	{
		var origins = new List<string>();
		config.GetSection("Cors:Origins").Bind(origins);
		policy.WithOrigins([.. origins])
			  .AllowAnyMethod()
			  .AllowAnyHeader();
	});
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(ops =>
{
	var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	ops.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
	ops.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "InForm.Server.Core.xml"));
});
builder.Services.AddControllers();

builder.Services.AddDbContext<InFormDbContext>(ops =>
{
	ops.UseNpgsql(config.GetConnectionString("InFormDb"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(policyName: corsPolicy);
app.MapControllers();

await app.RunAsync();
