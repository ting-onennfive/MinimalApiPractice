using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MinimalApiPractice;
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
    if (todoTask == null) return new Result<TodoTask>(null, "��Ƥ��s�b", true);
    return new Result<TodoTask>(DB.GetTodoTaskById(id));
})
    .WithDescription("�ھګݿ�ƶ� id�A���o�浧�ݿ�ƶ�");

app.MapGet("/to-do-task/get-list", () => new Result<List<TodoTask>>(DB.GetTodoTasks()))
    .WithDescription("���o�h���ݿ�ƶ�");

app.MapPost("/to-do-task", ([FromBody] TodoTask todoTask) => 
{
    var todoTasks = DB.GetTodoTasks();
    if (todoTasks.Any(t => t.id == todoTask.id)) return new Result<TodoTask>(null, "��ƭ���", false);
    DB.CreateTodoTask(todoTask);
    return new Result<TodoTask>(todoTask, "���ʦ��\", true);
})
    .WithDescription("�s�W�ݿ�ƶ�");;

app.MapPut("/to-do-task", ([FromBody] TodoTask todoTask) =>
{
    var targetTodoTask = DB.GetTodoTaskById(todoTask.id);
    if (targetTodoTask == null) return new Result<TodoTask>(null, "��Ƥ��s�b", true);
    DB.UpdateTodoTask(todoTask);
    return new Result<TodoTask>(todoTask, "���ʦ��\", true);
})
    .WithDescription("�s��ݿ�ƶ�");

app.MapDelete("/to-do-task/{id}", (int id) =>
{
    var targetTodoTask = DB.GetTodoTaskById(id);
    if (targetTodoTask == null) return new Result<TodoTask>(null, "��Ƥ��s�b", true);
    DB.DeleteTodoTask(targetTodoTask);
    return new Result<TodoTask>(targetTodoTask, "���ʦ��\", true);
})
    .WithDescription("�R���ݿ�ƶ�");

app.Run();
