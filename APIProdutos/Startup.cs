using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using APIProdutos.Data;
using APIProdutos.Models;
using APIProdutos.Security;
using APIProdutos.Business;

namespace APIProdutos
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
            // Ativando o uso de cache via Redis
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration =
                    Configuration.GetConnectionString("ConexaoRedis");
                options.InstanceName = "APIProdutos";
            });

            // Configurando o acesso a dados de produtos
            services.AddDbContext<CatalogoDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryDatabase"));
            services.AddScoped<ProdutoService>();

            // Configurando o uso da classe de contexto para
            // acesso �s tabelas do ASP.NET Identity Core
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryDatabase"));

            // Ativando a utiliza��o do ASP.NET Identity, a fim de
            // permitir a recupera��o de seus objetos via inje��o de
            // depend�ncias
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configurando a depend�ncia para a classe de valida��o
            // de credenciais e gera��o de tokens
            services.AddScoped<AccessManager>();

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            // Aciona a extens�o que ir� configurar o uso de
            // autentica��o e autoriza��o via tokens
            services.AddJwtSecurity(
                signingConfigurations, tokenConfigurations);

            services.AddCors();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Cria��o de estruturas, usu�rios e permiss�es
            // na base do ASP.NET Identity Core (caso ainda n�o
            // existam)
            new IdentityInitializer(context, userManager, roleManager)
                .Initialize();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}