using BackendConsultorioSeguros.DTOs;
using BackendConsultorioSeguros.Helpers;
using BackendConsultorioSeguros.Models;
using BackendConsultorioSeguros.Servicios.Contrato;
using BackendConsultorioSeguros.Servicios.Implementacion;
using BackendConsultorioSeguros.Utility;
using Microsoft.EntityFrameworkCore;

namespace BackendConsultorioSeguros
{
    public class Startup
    {
        public IConfiguration configuration { get;}
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            //services.AddAutoMapper(typeof(AutoMapperProfilesSeguro), typeof(AutoMapperProfilesCliente));
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // dbcontext
            services.AddDbContext<DBSEGUROSCHUBBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("cadenaSQL")).EnableSensitiveDataLogging();

            });

            // cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            // inyeccion de dependencias
            services.AddScoped<DataImportDAOService>();
            services.AddScoped<ISeguroService, SeguroService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IAseguradoService, AseguradoService>();

            // Registro de DataImportService con tipos específicos
            services.AddScoped<DataImportService<SeguroDto, Seguro>>();
            services.AddScoped<DataImportService<ClienteDto, Cliente>>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("AllowAll");
            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllers();
            });
        }
    }
}
