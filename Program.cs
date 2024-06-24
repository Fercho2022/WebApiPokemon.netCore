using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApiPokemon;
using WebApiPokemon.Data;
using WebApiPokemon.Interfaces;
using WebApiPokemon.Repository;

//WebApplication: Es una clase proporcionada por ASP.NET Core que
//contiene métodos estáticos para inicializar y configurar una aplicación
//web.

//CreateBuilder(args): Es un método estático que crea y configura un
//nuevo WebApplicationBuilder utilizando los argumentos de la línea de
//comandos (args). Este método prepara el entorno y las configuraciones
//necesarias para construir la aplicación.

var builder = WebApplication.CreateBuilder(args);

//builder.Services: Accede al contenedor de servicios donde se registran
//todos los servicios que la aplicación puede inyectar y usar. 

//AddDbContext<DataContext>: Método de extensión que se utiliza para
//registrar el contexto de base de datos DataContext en el contenedor
//de servicios. DataContext es una clase que hereda de DbContext y
//representa una sesión con la base de datos, permitiendo realizar
//consultas y guardar cambios.

//options => { ... }: Lambda que configura opciones adicionales para el
//contexto de base de datos.

//options.UseSqlServer(...): Especifica que se debe usar SQL Server como
//el proveedor de base de datos. Este método de extensión configura
//Entity Framework Core para conectarse a una base de datos SQL Server.

//builder.Configuration.GetConnectionString("DefaultConnection"): Obtiene
//la cadena de conexión llamada "DefaultConnection" desde la configuración
//de la aplicación (normalmente desde appsettings.json). La cadena de
//conexión contiene la información necesaria para conectar a la base de
//datos, como el servidor, la base de datos, el usuario y la contraseña

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//builder.Services: Accede al contenedor de servicios donde se registran
//todos los servicios que la aplicación puede inyectar y usar. 

//AddControllers(): Este método de extensión agrega los servicios necesarios para que los
//controladores funcionen. Esto incluye el registro de todos los
//componentes necesarios para la funcionalidad del patrón MVC, como el
//enrutamiento, los filtros, la vinculación de modelos, la validación de
//modelos, y más.


builder.Services.AddControllers();

// builder.Services: Se refiere al contenedor de servicios utilizado
// para registrar dependencias en una aplicación ASP.NET Core.

//AddTransient<TService>(): Este método registra el servicio especificado
//(Seed en este caso) con un tiempo de vida transitorio. Esto significa
//que cada vez que se solicita una instancia de este servicio, se crea
//una nueva instancia.

builder.Services.AddTransient<Seed>();

// builder.Services: Se refiere al contenedor de servicios utilizado
// para registrar dependencias en una aplicación ASP.NET Core.

// builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()):
// se utiliza para configurar y registrar el servicio de AutoMapper en una aplicación
// ASP.NET Core. AutoMapper es una biblioteca popular que facilita el
// mapeo de objetos de un tipo a otro, especialmente útil en la
// transformación de datos entre capas de una aplicación (por ejemplo,
// entre la capa de datos y la capa de presentación).

//AddAutoMapper(): método de extensión que agrega AutoMapper al contenedor
//de servicios. Al llamarlo, se configura AutoMapper como un servicio
//disponible para ser inyectado en controladores y otros servicios de la
//aplicación.



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// AddScoped: se utiliza para registrar la implementación concreta de una interfaz
// en el contenedor de dependencias de ASP.NET Core. Esto es fundamental
// para la inyección de dependencias (DI, Dependency Injection) en la
// aplicación. En ASP.NET Core, la inyección de dependencias es un
// patrón de diseño fundamental que permite la separación de la
// creación de objetos y su uso. En lugar de que un componente
// instancie directamente sus dependencias, las dependencias se
// proporcionan al componente a través de un contenedor de
// inyección de dependencias.

builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
//
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Inyecto servicio en el que se sembrara el contexto de
// datos antes que la aplicación antes de que se inicie la
// aplicación real..

if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }
}

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

