using Microsoft.Azure.Cosmos;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configurationManager = builder.Configuration;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(_ =>new CosmosClient(configurationManager.GetConnectionString("cosmosdb")));
var app = builder.Build();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();
app.MapPost("/todo",async (CosmosClient client, TodoDto todo)=>
{
    try
    {
        Container container = client.GetContainer(configurationManager["databaseId"], configurationManager["containerId"]);
        var result = await container.CreateItemAsync<TodoDto>(todo, PartitionKey.None);
        if(result.StatusCode == System.Net.HttpStatusCode.Created)
        {
            return Results.Ok();
        }
        else
        {
            return Results.Problem();
        }

    }
    catch(Exception ex) {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/todo", async (CosmosClient client, IConfiguration config) =>
{
    var container = client.GetContainer(config["databaseId"], config["containerId"]);
    var query = new QueryDefinition("SELECT * FROM c");
    var iterator = container.GetItemQueryIterator<TodoDto>(query);

    var todos = new List<TodoDto>();
    while (iterator.HasMoreResults)
    {
        var response = await iterator.ReadNextAsync();
        todos.AddRange(response.ToList());
    }

    return Results.Ok(todos);
});
app.MapPut("/todo/{id}", async (CosmosClient client, IConfiguration config, string id, TodoDto updatedTodo) =>
{
    var container = client.GetContainer(config["databaseId"], config["containerId"]);
    try
    {
        var response = await container.ReadItemAsync<TodoDto>(id, PartitionKey.None);
        var existingTodo = response.Resource;

        if(!string.IsNullOrEmpty(updatedTodo.Title))
            existingTodo.Title = updatedTodo.Title;
        if (!string.IsNullOrEmpty(updatedTodo.Description))
            existingTodo.Description = updatedTodo.Description;
        if (updatedTodo.CompletedTime is not null)
            existingTodo.CompletedTime = updatedTodo.CompletedTime;
        if (existingTodo.Completed != updatedTodo.Completed)
            existingTodo.Completed = updatedTodo.Completed;

        await container.ReplaceItemAsync(existingTodo, id, PartitionKey.None);

        return Results.Ok(existingTodo);
    }
    catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
        return Results.NotFound();
    }
});

app.MapDelete("/todo/{id}", async (CosmosClient client, IConfiguration config, string id) =>
{
    var container = client.GetContainer(config["databaseId"], config["containerId"]);
    try
    {
        var response = await container.DeleteItemAsync<TodoDto>(id, PartitionKey.None);
        return Results.NoContent();
    }
    catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
     
        return Results.NotFound();
    }
});



app.Run();

