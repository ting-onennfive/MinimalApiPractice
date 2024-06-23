using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MinimalApiPractice.DB;

var builder = WebApplication.CreateBuilder(args);

#region �s�W�����U�A�Ȧܮe��

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

// swagger �]�w�A�]�w�����D�P��ܪ���
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Todo API",
        Description = ".net 8��minimal api �m�ߡ��ݿ�ƶ� CRUD �\���@",
        Version = "v1"
    });
});

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet("/to-do-task/get-by-id/{id}", (int id) =>
{
    var todoTask = DB.GetTodoTaskById(id);
    if (todoTask == null) return Results.NotFound();
    return Results.Ok(value: DB.GetTodoTaskById(id));
})
    .WithDescription("�ھګݿ�ƶ� id�A���o�浧�ݿ�ƶ�");

app.MapGet("/to-do-task/get-list", () => DB.GetTodoTasks())
    .WithDescription("���o�h���ݿ�ƶ�");

app.MapPost("/to-do-task", ([FromBody] TodoTask todoTask) => 
{
    var todoTasks = DB.GetTodoTasks();
    if (todoTasks.Any(t => t.id == todoTask.id)) return Results.BadRequest(new { message = $"�w�s�b���� id�G{todoTask.id}" });
    DB.CreateTodoTask(todoTask);
    return Results.Ok(value: todoTask);
});

app.MapPut("/to-do-task", ([FromBody] TodoTask todoTask) => DB.UpdateTodoTask(todoTask));
app.MapDelete("/to-do-task", ([FromBody] TodoTask todoTask) => DB.DeleteTodoTask(todoTask));

app.Run();
