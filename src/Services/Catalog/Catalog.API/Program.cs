var builder = WebApplication.CreateBuilder(args);

// Add Services to the Container
var assembly = typeof(Program).Assembly;
var connectionString = builder.Configuration.GetConnectionString("Database");

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddCarter();

builder.Services.AddMarten(opt =>
{
    opt.Connection(connectionString!);
}).UseLightweightSessions();

//Console.WriteLine($"=== CONNECTION DEBUG ===");
//Console.WriteLine($"Connection String: {connectionString}");

// Try a direct connection test
//try
//{
//    using var conn = new Npgsql.NpgsqlConnection(connectionString);
//    await conn.OpenAsync();
//    Console.WriteLine("? Direct connection successful!");

//    using var cmd = new Npgsql.NpgsqlCommand("SELECT current_database(), current_user", conn);
//    using var reader = await cmd.ExecuteReaderAsync();
//    if (await reader.ReadAsync())
//    {
//        Console.WriteLine($"Connected to database: {reader.GetString(0)} as user: {reader.GetString(1)}");
//    }
//    conn.Close();
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"? Direct connection failed: {ex.Message}");
//    if (ex is Npgsql.PostgresException pgEx)
//    {
//        Console.WriteLine($"PostgreSQL Error Code: {pgEx.SqlState}");
//    }
//}

//Handle exception using IExceptionHandler
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
var app = builder.Build();
app.MapCarter();

////global exception handling
//app.UseExceptionHandler(exceptionHandlerApp =>
//{
//    exceptionHandlerApp.Run(async context =>
//    {
//        var exception =
//            context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

//        if (exception is null)
//        {
//            return;
//        }

//        var problemDetails = new ProblemDetails()
//        {
//            Title = exception.Message,
//            Status = StatusCodes.Status500InternalServerError,
//            Detail = exception.StackTrace
//        };

//        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
//        logger.LogError(exception, exception.Message);

//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//        context.Response.ContentType = "application/problem+json";

//        await context.Response.WriteAsJsonAsync(problemDetails);
//    });
//});

//Handle exception using IExceptionHandler
app.UseExceptionHandler(opt =>
{

});
app.Run();