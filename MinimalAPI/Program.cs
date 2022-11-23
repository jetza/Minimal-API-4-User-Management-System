using Microsoft.AspNetCore.Diagnostics;
using MinimalAPI;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var allowedOrigins = builder.Configuration.GetValue("Cors:AllowedOrigins", string.Empty).Split(';');

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000");
                          builder.WithMethods("GET", "POST", "PUT", "DELETE");
                          builder.AllowAnyHeader();
                      });
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAllHeaders"); ;
}

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();

//Get to Try out the routing
app.MapGet("/", () => "User Management System minimal APIs");

//USERS

//Get all Users
app.MapGet("/api/Users", async (DataContext context) => await context.Users.Take(10).ToListAsync());

//Get Users by id
app.MapGet("/api/Users/{id}", async (DataContext context, int id) =>
    await context.Users.FindAsync(id) is User user ? Results.Ok(user) : Results.NotFound("User not found ./"));

//Create Users
app.MapPost("/api/Users", async (DataContext context, UserModel model) =>
{
    var user = new User
    {
        FirstName = model.FirstName,
        LastName = model.LastName,
        Email = model.Email,
        UserName = model.UserName,
        Password = model.Password,
        Status = model.Status
};
    context.Users.Add(user);
    await context.SaveChangesAsync();
    return Results.Created($"/api/Users/{user.Id}", user);
});

//Updating Users
app.MapPut("/api/Users/{id}", async (DataContext context, UserModel model, int id) =>
{
    var user = await context.Users.FindAsync(id);

    if (user != null)
    {
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.Status = model.Status;

        await context.SaveChangesAsync();
        return Results.Ok(user);//
    }
    return Results.NotFound("User not found");
});

//Deleting Users
app.MapDelete("/api/User/{id}", async (DataContext context, int id) =>
{
    var usersFromDb = await context.Users.FindAsync(id);

    if (usersFromDb != null)
    {
        context.Remove(usersFromDb);
        await context.SaveChangesAsync();
        return Results.Ok();
    }
    return Results.NotFound("User not found");
});

//PERMISSIONS
app.MapGet("/api/Permissions", async (DataContext context) => await context.Permissions.ToListAsync());


app.MapGet("/api/User/{id}/Permissions", async (DataContext context, int id) =>
{
    var user = await context.Users.Where(x => x.Id == id).Include(x => x.Permissions).SingleAsync();
    if (user != null)
    {
        var userPermissions = user.Permissions.Select(x => x.Id);
        return Results.Ok(userPermissions);
    }
    return Results.NotFound("Permission not found");
});


app.MapPut("/api/User/{id}/Permissions", async (DataContext context, PermissionsModel model, int id) =>
{
    var user = await context.Users.Where(x => x.Id == id).Include(x => x.Permissions).SingleAsync();
    var permissions = await context.Permissions.ToListAsync();

    if (user != null)
    {
        user.Permissions.Clear();
        var userPermissions = permissions.Where(x => model.PermissionIds.Contains(x.Id));

        foreach(var permission in userPermissions)
        {
            user.Permissions.Add(permission);
        }
        
        await context.SaveChangesAsync();
        return Results.Ok();
    }
    return Results.NotFound("Permission not found");
});


app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerFeature>()
        ?.Error;
    if (exception is not null)
    {
        var response = new { error = exception.Message };
        context.Response.StatusCode = 400;

        await context.Response.WriteAsJsonAsync(response);
    }
}));
app.Run();
