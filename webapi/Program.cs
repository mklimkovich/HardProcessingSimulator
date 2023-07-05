using Hangfire;
using WebApi.BackgroundServices;
using WebApi.Hubs;
using WebApi.Queues;
using WebApi.Queues.Implementation;
using WebApi.Services;
using WebApi.Services.Implementation;
using WebApi.Storage;
using WebApi.Storage.Implementation;

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

builder.Services.AddHangfireServer(o => o.SchedulePollingInterval = TimeSpan.FromSeconds(1));

builder.Services.AddSingleton<ITaskStorage>(new InMemoryStorage());

builder.Services.AddSingleton<IEncodingQueueReader>(
    sp => new EncodingQueue(sp.GetRequiredService<ILogger<EncodingQueue>>()));
builder.Services.AddSingleton<IEncodingQueueWriter>(
    sp => (IEncodingQueueWriter) sp.GetRequiredService<IEncodingQueueReader>());

builder.Services.AddSingleton<IBase64Encoder, Base64Encoder>();
builder.Services.AddHostedService<EncodingService>();

builder.Services.AddSingleton<IOutputScheduler, OutputScheduler>();

builder.Services.AddSingleton(sp => new OutputQueue(sp.GetRequiredService<ILogger<OutputQueue>>()));
builder.Services.AddSingleton<IOutputQueueReader>(sp => sp.GetRequiredService<OutputQueue>());
builder.Services.AddSingleton<IOutputQueueScheduler>(sp => sp.GetRequiredService<OutputQueue>());

builder.Services.AddHostedService<OutputService>();

builder.Services.AddSingleton<ITaskHubService, TaskHubService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
