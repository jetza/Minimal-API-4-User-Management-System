using MinimalAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Get to Try out the routing
app.MapGet("/", () => "User Management System minimal APIs");


//USERS

//Get all Users
app.MapGet("/api/Users", async (DataContext context) => await context.Users.Take(10).ToListAsync());

//Get Users by id
app.MapGet("/api/Users/{id}", async (DataContext context, int id) =>
    await context.Users.FindAsync(id) is User todoItem ? Results.Ok(todoItem) : Results.NotFound("User not found ./"));

//Create Users
app.MapPost("/api/Users", async (DataContext context, User user) =>
{
    context.Users.Add(user);
    await context.SaveChangesAsync();
    return Results.Created($"/api/Users/{user.Id}", user);
});

//Updating Users
app.MapPut("/api/Users/{id}", async (DataContext context, User user, int id) =>
{
    var usersFromDb = await context.Users.FindAsync(id);

    if (usersFromDb != null)
    {
        usersFromDb.Email = user.Email;
        usersFromDb.UserName = user.UserName;
        usersFromDb.Password = user.Password;
        usersFromDb.Password = user.Password;
        usersFromDb.Password = user.Password;

        await context.SaveChangesAsync();
        return Results.Ok(user);
    }
    return Results.NotFound("TodoItem not found");
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




app.Run();
