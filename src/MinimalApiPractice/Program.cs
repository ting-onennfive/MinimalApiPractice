using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MinimalApiPractice;
using MinimalApiPractice.DB;

var builder = WebApplication.CreateBuilder(args);

#region 新增欲註冊服務至容器

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

// swagger 設定，設定文件標題與顯示版本
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Todo API",
        Description = ".net 8／minimal api 練習／待辦事項 CRUD 功能實作",
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
    if (todoTask == null) return new Result<TodoTask>(null, "資料不存在", true);
    return new Result<TodoTask>(DB.GetTodoTaskById(id));
})
    .WithDescription("根據待辦事項 id，取得單筆待辦事項");

app.MapGet("/to-do-task/get-list", () => new Result<List<TodoTask>>(DB.GetTodoTasks()))
    .WithDescription("取得多筆待辦事項");

app.MapPost("/to-do-task", ([FromBody] TodoTask todoTask) => 
{
    var todoTasks = DB.GetTodoTasks();
    if (todoTasks.Any(t => t.id == todoTask.id)) return new Result<TodoTask>(null, "資料重複", false);
    DB.CreateTodoTask(todoTask);
    return new Result<TodoTask>(todoTask, "異動成功", true);
})
    .WithDescription("新增待辦事項");;

app.MapPut("/to-do-task", ([FromBody] TodoTask todoTask) =>
{
    var targetTodoTask = DB.GetTodoTaskById(todoTask.id);
    if (targetTodoTask == null) return new Result<TodoTask>(null, "資料不存在", true);
    DB.UpdateTodoTask(todoTask);
    return new Result<TodoTask>(todoTask, "異動成功", true);
})
    .WithDescription("編輯待辦事項");

app.MapDelete("/to-do-task/{id}", (int id) =>
{
    var targetTodoTask = DB.GetTodoTaskById(id);
    if (targetTodoTask == null) return new Result<TodoTask>(null, "資料不存在", true);
    DB.DeleteTodoTask(targetTodoTask);
    return new Result<TodoTask>(targetTodoTask, "異動成功", true);
})
    .WithDescription("刪除待辦事項");

app.Run();
