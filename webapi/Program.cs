using Hangfire;
using WebApi.BackgroundServices;
using WebApi.Hubs;
using WebApi.Queues;
using WebApi.Queues.Implementation;
using WebApi.Services;
using WebApi.Services.Implementation;
using WebApi.Storage;
using WebApi.Storage.Implementation;
using OutputService = WebApi.Services.Implementation.OutputService;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseStorage(new Hangfire.InMemory.InMemoryStorage()));

builder.Services.AddHangfireServer();

var logger = builder.Build().Logger;

builder.Services.AddSingleton<ITaskStorage, InMemoryStorage>();

EncodingQueue encodingQueue = new(logger);
builder.Services.AddSingleton<IEncodingQueueReader>(encodingQueue);
builder.Services.AddSingleton<IEncodingQueueWriter>(encodingQueue);

builder.Services.AddSingleton<IBase64Encoder, Base64Encoder>();
builder.Services.AddHostedService<EncodingService>();

builder.Services.AddSingleton<IOutputScheduler, OutputScheduler>();

OutputQueue outputQueue = new(logger);
builder.Services.AddSingleton<IOutputQueueReader>(outputQueue);
builder.Services.AddSingleton<IOutputQueueScheduler>(outputQueue);

builder.Services.AddHostedService<WebApi.BackgroundServices.OutputService>();

builder.Services.AddSingleton<IOutputService, OutputService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapHub<TaskHub>("/tasks");

app.Run();
