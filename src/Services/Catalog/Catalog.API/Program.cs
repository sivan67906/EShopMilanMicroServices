var builder = WebApplication.CreateBuilder(args);

// Add Services to the Container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

var connectionString = builder.Configuration.GetConnectionString("Database");
Console.WriteLine($"=== CONNECTION DEBUG ===");
Console.WriteLine($"Connection String: {connectionString}");

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

builder.Services.AddMarten(opt =>
{
    opt.Connection(connectionString!);
}).UseLightweightSessions();

var app = builder.Build();
app.MapCarter();
app.Run();