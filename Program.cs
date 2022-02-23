using DataService;
using MassTransit;
using TokenService;
using FileService;
using FileService.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IFileUpload,LocalFileUpload>();
builder.Services.AddScoped<IDataAccess,ServiceDataAccess>();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(x =>
            {
                //x.AddConsumers(Assembly.GetExecutingAssembly());



                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMq:host"], "/", h =>
                    {
                        h.Username(builder.Configuration["RabbitMq:user"]);
                        h.Password(builder.Configuration["RabbitMq:password"]);
                    });
                    cfg.ConfigureEndpoints(context);
                });

               //data service
                x.AddRequestClient<DataServiceContract>();        
                //token service
                x.AddRequestClient<TokenServiceContract>();


            }).AddMassTransitHostedService();
            var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
