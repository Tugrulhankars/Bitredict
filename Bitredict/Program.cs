using Bitredict.DataAccess.Abstract;
using Bitredict.DataAccess.Concrete;
using Bitredict.DataAccess;
using Bitredict.Models;
using Bitredict.Services.Abstracts;
using Bitredict.Services.Manager;
using Bitredict.Services.Profiles;
using System.Text.Json;
using Quartz;
using Bitredict.Job;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.IgnoreNullValues = true; // Null deðerleri yoksay


    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Mapper));
builder.Services.AddScoped<IMatchService, MatchManager>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IHomePageStatisticsRepository,HomePageStatisticsRepository>();
builder.Services.AddScoped<IMatchCenterStatisticsRepository, MatchCenterStatisticsRepository>();
builder.Services.AddScoped<IHomePageStatisticsService,HomePageStatisticsManager>();
builder.Services.AddScoped<IMatchCenterStatisticsService,MatchCenterStatisticsManager>();
builder.Services.AddScoped<IAccuracyRepository,AccuracyRepository>();
builder.Services.AddScoped<IAccuracyService, AccuracyManager>();
builder.Services.AddQuartz(q =>
{
    //var jobKey = new JobKey("HomePageStatisticsJob");
    //q.AddJob<HomePageStatisticsJob>(opts => opts.WithIdentity(jobKey));
    //q.AddTrigger(opts => opts
    //    .ForJob(jobKey)
    //    .WithIdentity("HomePageStatisticsJob-trigger")
    //    .StartNow()
    //    .WithSimpleSchedule(x => x
    //        .WithInterval(TimeSpan.FromHours(12))
    //        .RepeatForever()));

    //var matchCenterJobKey = new JobKey("MatchCenterStatisticsJob");
    //q.AddJob<MatchCenterStatisticsJob>(opts => opts.WithIdentity(matchCenterJobKey));
    //q.AddTrigger(opts => opts
    //    .ForJob(matchCenterJobKey)
    //    .WithIdentity("MatchCenterStatisticsJob-trigger")
    //    .StartNow()
    //    .WithSimpleSchedule(x => x
    //        .WithInterval(TimeSpan.FromHours(12))
    //        .RepeatForever()));

    //var matchJobKey = new JobKey("MatchJob");
    //q.AddJob<MatchJob>(opts => opts.WithIdentity(matchJobKey));
    //q.AddTrigger(opts => opts
    //    .ForJob(matchJobKey)
    //    .WithIdentity("MatchJob-trigger")
    //    .StartNow()
    //    .WithSimpleSchedule(x => x
    //        .WithInterval(TimeSpan.FromHours(24))
    //        .RepeatForever()));
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddDbContext<BaseDbContext>();
var app = builder.Build();
app.UseSwagger();
   app.UseSwaggerUI();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
