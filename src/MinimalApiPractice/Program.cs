using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
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
    if (todoTask == null) return Results.NotFound();
    return Results.Ok(value: DB.GetTodoTaskById(id));
})
    .WithDescription("根據待辦事項 id，取得單筆待辦事項");

app.MapGet("/to-do-task/get-list", () => DB.GetTodoTasks())
    .WithDescription("取得多筆待辦事項");

app.MapPost("/to-do-task", ([FromBody] TodoTask todoTask) => 
{
    var todoTasks = DB.GetTodoTasks();
    if (todoTasks.Any(t => t.id == todoTask.id)) return Results.BadRequest(new { message = $"已存在重複 id：{todoTask.id}" });
    DB.CreateTodoTask(todoTask);
    return Results.Ok(value: todoTask);
});

app.MapPut("/to-do-task", ([FromBody] TodoTask todoTask) => DB.UpdateTodoTask(todoTask));
app.MapDelete("/to-do-task", ([FromBody] TodoTask todoTask) => DB.DeleteTodoTask(todoTask));

app.Run();
