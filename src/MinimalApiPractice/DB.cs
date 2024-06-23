public record TodoTask(int id, string description);

namespace MinimalApiPractice.DB
{
    public class DB
    {
        private static List<TodoTask> _todoTasks = new List<TodoTask>()
        {
            new TodoTask(1,"the description of task 1"),
            new TodoTask(2,"the description of task 2"),
            new TodoTask(3,"the description of task 3"),
            new TodoTask(4,"the description of task 4"),
            new TodoTask(5,"the description of task 5"),
        };

        public static List<TodoTask> GetTodoTasks()
        {
            return _todoTasks;
        }

        public static TodoTask? GetTodoTaskById(int id)
        {
            return _todoTasks.SingleOrDefault(t => t.id == id);
        }

        public static TodoTask? GetTodoTask(TodoTask todoTask)
        {
            return _todoTasks.SingleOrDefault(t => t == todoTask);
        }

        public static TodoTask CreateTodoTask(TodoTask todoTask)
        {
            _todoTasks.Add(todoTask);
            return todoTask;
        }

        public static TodoTask UpdateTodoTask(TodoTask todoTask)
        {
            _todoTasks = _todoTasks
                .Select(t =>
                {
                    if (t.id == todoTask.id) t = todoTask;
                    return t;
                })
                .ToList();
            return todoTask;
        }

        public static void DeleteTodoTask(TodoTask todoTask)
        {
            var targetTodoTask = GetTodoTask(todoTask);
            // record 為 Value Types，故值若相同則為相同
            _todoTasks = _todoTasks.FindAll(t => t != todoTask).ToList();
        }
    }
}
