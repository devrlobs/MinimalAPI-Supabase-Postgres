using MinimalAPI_Supabase_Postgres;
using Supabase;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);
var url = builder.Configuration["Supabase:Url"] ?? string.Empty;
var key = builder.Configuration["Supabase:Key"] ?? string.Empty;
var bearerToken = builder.Configuration["bearerToken"] ?? string.Empty;
var options = new SupabaseOptions
{
    AutoRefreshToken = true,
    AutoConnectRealtime = true,
};
builder.Services.AddScoped(provider => new Client(url, key, options));
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/signin", async (Client client, SignInRequest request) =>
{
    await client.Auth.SignInWithPassword(request.Email, request.Password);
})
.WithName("signin")
.WithOpenApi();


app.MapPost("/insertodo", async (TodoRequest request, Client client) =>
{
    await client.Auth.SignInWithPassword("testuser@gmail.com", "test1234");

    Todo todo = new()
    {
        Name = request.Name,
        IsDone = request.IsDone
    };

    var res = await client.From<Todo>().Insert(todo);

    var newTodo = res.Models.First();

    return Results.Ok(newTodo.Id);
})
.WithName("insertodo")
.WithOpenApi();

app.MapGet("/gettodo", async (Client client) =>
{
    await client.Auth.SignInWithPassword("testuser@gmail.com", "test1234");

    // var query = client.From<Todo>().Select("*");

    // var response = await query.Get();

    // var response = await client.From<Todo>().Get();
    // List<TodoResponse> models = response.Models
    //                                 .Select(x => new TodoResponse 
    //                                     { 
    //                                         Id = x.Id, 
    //                                         Name = x.Name, 
    //                                         IsDone = x.IsDone, 
    //                                         CreatedBy = x.CreatedBy
    //                                     }).ToList();

    // return Results.Ok(models);

    var response = await client.From<Todo>().Get();
   // return JsonSerializer.Serialize(response.Models);
   return response.Content;


    //var response = await client.From<Todo>();

})
.WithName("gettodo")
.WithOpenApi();

app.MapPut("/todoupdate/{id}", async (Client client, TodoRequest request, int id) =>
{
    await client.Auth.SignInWithPassword("testuser@gmail.com", "test1234");

    var update = await client
.From<Todo>()
.Where(x => x.Id == id)
.Set(x => x, request)
.Update();

    return Results.Ok("Successfully updated!");
})
.WithName("updatetodo")
.WithOpenApi();

app.MapDelete("/deletetodo/{id}", async (Client client, int id) =>
{
    await client.Auth.SignInWithPassword("testuser@gmail.com", "test1234");
    //find todo using id
    await client.From<Todo>().Where(x => x.Id == id).Delete();

    return Results.Ok("Successfully Deleted!");
})
.WithName("deletetodo")
.WithOpenApi();


app.Run();
