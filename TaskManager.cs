using Newtonsoft.Json;
using System.IO;

namespace Productivity
{
    internal class TaskManager
    {
        private readonly string dirPath;

        public TaskManager(string dirPath)
        {
            this.dirPath = dirPath;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        public void AddTask(DateTime date, Task task)
        {
            string filePath = GetFilePath(date);
            List<Task> tasks = LoadTasks(filePath);
            tasks.Add(task);
            SaveTasks(filePath, tasks);
        }
        private string GetFilePath(DateTime date)
        {
            string fileName = $"tasks-{date:yyyy-MM}.json";
            return Path.Combine(dirPath, fileName);
        }
        private static List<Task> LoadTasks(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<Task>>(json);
            }
            return [];
        }

        private static void SaveTasks(string filePath, List<Task> tasks)
        {
            string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
    public class Task(string description, TimeSpan time)
    {
        public string Description { get; set; } = description;
        public TimeSpan Time { get; set; } = time;
    }
}
