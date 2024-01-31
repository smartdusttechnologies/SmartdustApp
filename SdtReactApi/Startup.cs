using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.IdentityModel.Tokens;

using SmartdustApp.Business.Core.Model;
using SmartdustApp.Business.Data.Repository.Interfaces.Security;
using SmartdustApp.Business.Data.Repository.Interfaces;
using SmartdustApp.Business.Infrastructure;
using SmartdustApp.Business.Repository;
using SmartdustApp.Business.Services;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Data.Repository;
using TestingAndCalibrationLabs.Business.Core.Interfaces;
using TestingAndCalibrationLabs.Business.Services;
using SmartdustApp.Common;
using Microsoft.AspNetCore.Authorization;
using SmartdustApp.Business.Services.Security;
using SmartdustApp.Web.Common;
using System.Transactions;
using Microsoft.AspNetCore.Diagnostics;
using System.Text;

namespace SmartdustApp
{
    public class Startup
    {
        public static TokenValidationParameters tokenValidationParameters;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
          
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyTypes.Users.Manage, policy => { policy.RequireClaim(CustomClaimType.ApplicationPermission.ToString(), Permissions.UsersPermissions.Add); });
                options.AddPolicy(PolicyTypes.Users.Manage, policy => { policy.RequireClaim(CustomClaimType.ApplicationPermission.ToString(), Permissions.UsersPermissions.Edit); });
                options.AddPolicy(PolicyTypes.Users.Manage, policy => { policy.RequireClaim(CustomClaimType.ApplicationPermission.ToString(), Permissions.UsersPermissions.Read); });
                //options.AddPolicy(PolicyTypes.Users.Manage, policy => { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.UsersPermissions.Delete); });
                options.AddPolicy(PolicyTypes.Users.EditRole, policy => { policy.RequireClaim(CustomClaimType.ApplicationPermission.ToString(), Permissions.UsersPermissions.EditRole); });
            });
            // In production, the React files will be served from this directory
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    //configuration.RootPath = "ClientApp/build";
            //});
            //Repository DI
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<ILeaveRepository, LeaveRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            //Service DI
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<ILeaveService, LeaveService>();
            services.AddScoped<IDocumentService, DocumentService>();
            //

            services.AddScoped<Business.Core.Interfaces.ILogger, Logger>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IRoleService, RoleService>();
            //Authorization Handler Initalization Start
            services.AddScoped<Business.Core.Interfaces.IAuthorizationService, AuthorizationService>();
            //Authorization Handler Initalization End
            services.AddScoped<ISecurityParameterService, SecurityParameterService>();
            //Repository
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<ILoggerRepository, LoggerRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISecurityParameterRepository, SecurityParameterRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<ISecurityParameterService, SecurityParameterService>();

            //Email service
            services.AddScoped<IEmailService, EmailService>();

            //Authorization Handler Initalization Start
            services.AddScoped<IAuthorizationHandler, LeaveBalanceAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, EmployeeTableAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, EmployeeLeaveAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, LeaveAuthorizationHandler>();
            //Authorization Handler Initalization End
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine(env.IsDevelopment());
            Console.WriteLine("testing dtdgdfg");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp => { 
                    errorApp.Run(async context =>
                {
                    // Set the status code to 500 (Internal Server Error)
                    context.Response.StatusCode = 200;

                    // Get the exception details
                    var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>();

                    if (exceptionHandlerPathFeature?.Error != null)
                    {
                        // Get the exception message
                        var errorMessage = exceptionHandlerPathFeature.Error.Message;

                        // You can also include additional details like stack trace if needed
                        var stackTrace = exceptionHandlerPathFeature.Error.StackTrace;

                        // Construct a detailed error message
                        var detailedErrorMessage = $"An internal server error occurred: {errorMessage}\n\n{stackTrace}";

                        // Write the detailed error message to the response
                        await context.Response.WriteAsync(detailedErrorMessage);
                    }
                });
            });
                app.UseHsts();
            }
            tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JWT:Secret"])),

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = Configuration["JWT:ValidIssuer"],

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = Configuration["JWT:ValidAudience"],

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseMiddleware<SdtAuthenticationMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapFallbackToFile("index.html");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            
            //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = "ClientApp";
            //    if (env.IsDevelopment())
            //    {
            //        spa.UseReactDevelopmentServer(npmScript: "start");
            //    }
            //});
            
        }
    }
}
