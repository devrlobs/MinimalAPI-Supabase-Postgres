using MinimalAPI_Supabase_Postgres;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Supabase;
using System.Globalization;
using System.Text;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);
var url = builder.Configuration["Supabase:Url"] ?? string.Empty;
var key = builder.Configuration["Supabase:Key"] ?? string.Empty;
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

// builder.Services.AddSession(options => 
// {
//     options.Cookie.Name = ".test.Session";
//     options.IdleTimeout = TimeSpan.FromSeconds(10);
//     options.Cookie.IsEssential = true;
// });

// builder.Services.AddControllers().AddNewtonsoftJson(options =>
//     {
//         //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
//         options.SerializerSettings.ContractResolver  =new DefaultContractResolver();
//     });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.Use(async (context, next) => 
{
    await context.Response.WriteAsync("Hello 1");
    await next(context);
    await context.Response.WriteAsync("Hello 2");
});

app.MapGet("/", () => 
{
    return "Hello 3";
});

// app.MapPost("/signin", async (Client client, SignInRequest request, ISession session) =>
// {
//     //var test = await client.Auth.SignInWithPassword(request.Email, request.Password);
//     //session.SetString("AuthKey", JsonConvert.SerializeObject(test?.AccessToken) ?? "");


//     var auth = client.Auth;
//     var result = await auth.SignInWithPassword(request.Email, request.Password);

//     var accessToken = result?.AccessToken ?? "";
//     Cookies.Set("supabase-auth-token", accessToken);

//     return Results.Ok();
// })
// .WithName("signin")
// .WithOpenApi();


app.MapGet("/gettodo", async  (Client client) =>
{
    await client.Auth.SignInWithPassword("testuser@gmail.com", "test1234");
    var response = await client.From<Todo>().Get();
    var storage = response.Models;
    var storage2 = JsonConvert.SerializeObject(storage);
    return JsonConvert.SerializeObject(response.Models);
})
.WithName("gettodo")
.WithOpenApi();




app.Run();

Client client;


// app.MapPost("/signin", async (Client client, SignInRequest request) =>
// {
//     await client.Auth.SignInWithPassword(request.Email, request.Password);
// })
// .WithName("signin")
// .WithOpenApi();


// app.MapPost("/insertodo", async (TodoRequest request, Client client) =>
// {
//     await client.Auth.SignInWithPassword("testuser@gmail.com", "test1234");
//     Todo todo = new()
//     {
//         Name = request.Name,
//         IsDone = request.IsDone
//     };
//     var res = await client.From<Todo>().Insert(todo);
//     var newTodo = res.Models.First();
//     return Results.Ok(newTodo.Id);
// })
// .WithName("insertodo")
// .WithOpenApi();

// app.MapPut("/updatetodo/{id}", async (Client client, TodoRequest request, int id) =>
// {
//     await client.Auth.SignInWithPassword("testuser@gmail.com", "test1234");

//     var update = await client
//                     .From<Todo>()
//                     .Where(x => x.Id == id)
//                     .Set(x => x.Name, request.Name).Set(x => x.IsDone, request.IsDone)
//                     .Update();
//     if (update.Models.Count <= 0)
//     {
//         return Results.NotFound("Todo ID supplied not found.");
//     }
//     return Results.Ok("Successfully updated!");
// })
// .WithName("updatetodo")
// .WithOpenApi();

// app.MapDelete("/deletetodo/{id}", async (Client client, int id) =>
// {
//     await client.Auth.SignInWithPassword("testuser@gmail.com", "test1234");
//     await client.From<Todo>().Where(x => x.Id == id).Delete();
//     return Results.Ok("Successfully Deleted!");
// })
// .WithName("deletetodo")
// .WithOpenApi();