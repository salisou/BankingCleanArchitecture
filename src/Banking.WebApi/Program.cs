using Banking.Application.Interfaces;
using Banking.Application.Services;
using Banking.Infrastructure.Data;
using Banking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configura Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/banking-api-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Avvio dell'applicazione Banking API...");

    // 1. Configurazione CORS
    var corsPolicyName = "AllowAllOrigins"; // puoi personalizzare il nome e le origini
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: corsPolicyName,
            policy =>
            {
                policy
                    .AllowAnyOrigin()    // In produzione valuta di specificare gli origin consentiti con .WithOrigins("https://tuo-dominio.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    });

    // 2. Configurazione DbContext
    builder.Services.AddDbContext<BankingDbContext>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    // 3. Registrazione Repository e Unit of Work
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IFilialeRepository, FilialeRepository>();
    builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
    builder.Services.AddScoped<IContoCorrenteRepository, ContoCorrenteRepository>();
    builder.Services.AddScoped<ICartaRepository, CartaRepository>();
    builder.Services.AddScoped<IDipendenteRepository, DipendenteRepository>();
    builder.Services.AddScoped<IDossierTitoliRepository, DossierTitoliRepository>();
    builder.Services.AddScoped<ITransazioneRepository, TransazioneRepository>();
    builder.Services.AddScoped<IPrestitoRepository, PrestitoRepository>();
    builder.Services.AddScoped<IRataPrestitoRepository, RataPrestitoRepository>();
    builder.Services.AddScoped<IPortafoglioTitoliRepository, PortafoglioTitoliRepository>();

    // 4. Registrazione Servizi di Business
    builder.Services.AddScoped<IClienteService, ClienteService>();
    builder.Services.AddScoped<IContoCorrenteService, ContoCorrenteService>();
    builder.Services.AddScoped<IDipendenteService, DipendenteService>();
    builder.Services.AddScoped<IDossierTitoliService, DossierTitoliService>();
    builder.Services.AddScoped<IFilialeService, FilialeService>();
    builder.Services.AddScoped<IPrestitoService, PrestitoService>();
    builder.Services.AddScoped<ICartaService, CartaService>();
    builder.Services.AddScoped<ITransazioneService, TransazioneService>();
    builder.Services.AddScoped<IRataPrestitoService, RataPrestitoService>();
    builder.Services.AddScoped<IPortafoglioTitoliService, PortafoglioTitoliService>();

    // 5. Registrazione Controller
    builder.Services.AddControllers();

    // 6. Autorizzazione
    builder.Services.AddAuthorization();

    // 7. AutoMapper
    builder.Services.AddAutoMapper(typeof(Banking.Application.Mappings.MappingProfile));

    // 8. OpenAPI / Swagger
    builder.Services.AddOpenApi();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Banking API",
            Version = "v1",
            Description = "API per la gestione di operazioni bancarie"
        });
    });

    var app = builder.Build();

    // 9. Middleware pipeline

    // Usa CORS prima di qualsiasi middleware che gestisce le richieste
    app.UseCors(corsPolicyName);

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking API v1");
            c.RoutePrefix = string.Empty; // Swagger UI sulla root
        });
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    Log.Information("Applicazione avviata con successo!");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Applicazione terminata a causa di un errore non gestito");
}
finally
{
    Log.CloseAndFlush();
}
