using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using NSwag.Generation.Processors.Security;
using RestfulAPI.Filter;
using RestfulAPI.Interceptor;
using RestfulAPI.Model;
using RestfulAPI.Repository.Repository;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RestfulAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static IContainer ApplicationContainer { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // 獲取資料庫連接字串
            var connectionStr = Configuration.GetConnectionString("SqlServer");
            
            // 加入 AutoMapper
            services.AddAutoMapper(typeof(Mappings));

            //調用前面的靜態方法，將映射關係註冊
            ColumnMapper.SetMapper();

            //services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            // 加入本機快取MemoryCache
            services.AddMemoryCache();

            services.AddMvc(e =>
            {
                e.Filters.Add<CheckModel>();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .ConfigureApiBehaviorOptions(e =>
            {
                // 關閉預設模型驗證
                e.SuppressModelStateInvalidFilter = true;
            })
            .AddNewtonsoftJson(options =>
            {
                // Pascal Case
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                // Ignore Null
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }); //.AddControllersAsServices();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
            });

            //注入Swagger服務
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Restful API", Version = "v1" });

            //    // knife4j Skin
            //    //c.AddServer(new OpenApiServer()
            //    //{
            //    //    Url = "",
            //    //    Description = "vvv"
            //    //});
            //    //c.CustomOperationIds(apiDesc =>
            //    //{
            //    //    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
            //    //    return controllerAction.ControllerName + "-" + controllerAction.ActionName;
            //    //});

            //    // Authorization
            //    c.AddSecurityDefinition("Bearer",
            //    new OpenApiSecurityScheme
            //    {
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.ApiKey,
            //        Scheme = "Bearer",
            //        BearerFormat = "JWT",
            //        In = ParameterLocation.Header,
            //        Description = "JWT Authorization"
            //    });

            //    c.AddSecurityRequirement(
            //    new OpenApiSecurityRequirement
            //    {
            //        {
            //            new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference
            //                {
            //                    Type = ReferenceType.SecurityScheme,
            //                    Id = "Bearer"
            //                }
            //            },
            //            new string[] {}
            //        }
            //    });
            //});
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "User Restful API";
                    document.Info.Description = "A Simple ASP.NET Core Web API";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Prox",
                        Email = string.Empty,
                        Url = ""
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Prox",
                        Url = ""
                    };
                };

                config.AddSecurity("輸入身份認證Token", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme()
                {
                    Description = "JWT認證 請輸入Bearer {token}",
                    Name = "Authorization",
                    In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                    Type = NSwag.OpenApiSecuritySchemeType.ApiKey
                });

                config.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT Token"));
            });

            //#region JWT
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Audience:Secret"]));
            services.AddAuthentication("Bearer").AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    //是否開啟金鑰認證和key值
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,

                    //是否開啟發行人認證和發行人
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Audience:Issuer"],

                    //是否開啟訂閱人認證和訂閱人
                    ValidateAudience = true,
                    ValidAudience = Configuration["Audience:Audience"],

                    //認證時間的偏移量
                    ClockSkew = TimeSpan.Zero,
                    //是否開啟時間認證
                    ValidateLifetime = true,
                    //是否該權杖必須帶有過期時間
                    RequireExpirationTime = true
                };
            });
            //#endregion

            //單一注入Service
            // services.AddScoped<IUserService, UserService>();

            //單一注入Repository
            // services.AddTransient<IUserRepository, UserRepository>();

            //注入DbContext
            services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connectionStr, e => e.MigrationsAssembly("NetCoreWebApi.Model")));
            //注入Uow依賴
            services.AddScoped<IUnitOfWork, UnitOfWork<MyDbContext>>();

            //Scoped：注入的物件在同一Request中，參考的都是相同物件(你在Controller、View中注入的IDbConnection指向相同參考)
            services.AddTransient<IDbConnection>(sp => new SqlConnection(connectionStr));
            services.AddScoped<IDapperUnitOfWork, DapperUnitOfWork>();

            //AutoFac初始化容器
            var builder = new ContainerBuilder();
            //將services中的服務填充到Autofac中.
            builder.Populate(services);

            //註冊攔截器
            builder.RegisterType<LoggingInterceptor>().AsSelf();

            //業務邏輯層所在程式集命名空間
            Assembly service = Assembly.Load("RestfulAPI.Service");
            //Assembly service = Assembly.GetExecutingAssembly();
            //介面層所在程式集命名空間
            //Assembly repository = Assembly.Load("RestfulAPI.Repository");

            //自動注入
            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(service).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope().EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                   .InterceptedBy(new Type[] { typeof(LoggingInterceptor) });//這裡只有同步的，因為非同步方法攔截器還是先走同步攔截器
            //builder.RegisterAssemblyTypes(repository).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces();

            //如果需要在Controller中使用屬性注入，需要在ConfigureContainer中添加如下代碼
            //var controllerBaseType = typeof(ControllerBase);

            //builder.RegisterAssemblyTypes(typeof(Program).Assembly)
            //    .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
            //    .PropertiesAutowired()//允許屬性注入
            //    .EnableClassInterceptors().InterceptedBy(new Type[] { typeof(LoggingInterceptor) });// 允許在Controller類上使用攔截器

            //註冊倉儲，所有IRepository介面到Repository的映射
            //builder.RegisterGeneric(typeof(Repository<>))
            //       //InstancePerDependency：預設模式，每次調用，都會重新產生實體物件；每次請求都創建一個新的物件；
            //       .As(typeof(IRepository<>)).InstancePerDependency();

            //構造
            ApplicationContainer = builder.Build();
            //將AutoFac回饋到容器中
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            //HTTP管道是有先後順序的，一定要寫在 app.Mvc() 之前，否則不起作用。
            app.UseAuthentication();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3(setting => 
            {
                setting.DefaultModelsExpandDepth = -1;//設置為 - 1 可不顯示models
                setting.DocExpansion = "list"; // 展開方法
            });

            app.UseReDoc(config =>  // serve ReDoc UI
            {
                // 這裡的 Path 用來設定 ReDoc UI 的路由 (網址路徑) (一定要以 / 斜線開頭)
                config.Path = "/redoc";
            });

            app.UseRouting();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=User}/{action=Index}/{id?}");
            //    endpoints.MapRazorPages();
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //啟用Swagger服務
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restful API V1");
            //    c.DefaultModelsExpandDepth(-1); //設置為 - 1 可不顯示models
            //    c.DocExpansion(DocExpansion.Full); //設置為none可折疊所有方法
            //});

            //app.UseKnife4UI(c =>
            //{
            //    c.RoutePrefix = string.Empty; ; // serve the UI at root
            //    c.SwaggerEndpoint("/v1/api-docs", "LT.PropertyManage.WebApi v1");
            //});

            // 異常處理仲介軟體
            app.UseMiddleware(typeof(ExceptionMiddleWare));
        }

    }
}