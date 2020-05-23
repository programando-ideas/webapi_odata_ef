using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Edm;
using odatawebapi.Entidades;

namespace odatawebapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Log para Entity Framework a Consola
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            // Agregamos el DBContext
            string cstr = Configuration.GetConnectionString("PersonasDbContext");
            services.AddDbContext<PersonasDbContext>(options =>
                    options.UseSqlServer(cstr)
                           .UseLoggerFactory(loggerFactory));

            // Agregamos los servicios de OData
            services.AddOData();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // Esta funcion sirve para poder activar el decorador 
                // [EnableQuery]
                endpoints.EnableDependencyInjection();

                // Define las funciones que vamos a activar de OData
                endpoints.Select().Filter().OrderBy()
                         .Count().MaxTop(50).Expand();

                // Define la ruta de los controllers de OData
                endpoints.MapODataRoute("odata", "odata", GetEdmModel());
            });
        }

        private IEdmModel GetEdmModel()
        {
            // Registra las entidades que vamos a utilizar con OData
            ODataModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Persona>("Personas");
            modelBuilder.EntitySet<Telefono>("Telefonos");
            modelBuilder.EntitySet<Direccion>("Direcciones");

            // Registra una accion que luego utilizaremos para crear una Persona
            var crearPersonaAction = modelBuilder.EntityType<Persona>().Action("CrearPersona");
            crearPersonaAction.Parameter<string>("NombreYApellido");
            crearPersonaAction.Parameter<int>("Edad");
            crearPersonaAction.CollectionParameter<Telefono>("Telefonos");
            crearPersonaAction.CollectionParameter<Direccion>("Direcciones");
            crearPersonaAction.ReturnsFromEntitySet<Persona>("Persona");
            modelBuilder.Namespace = typeof(Persona).Namespace;
            return modelBuilder.GetEdmModel();
        }
    }
}