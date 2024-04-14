using Newtonsoft.Json;
using System.IO;
using System.Windows;

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

        public bool AddTask(DateTime date, Task task)
        {
            if (task == null)
            {
                return false; // Task cannot be null, return false
            }

            string filePath = GetFilePath(date);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return false; // Invalid file path, return false
            }

            try
            {
                List<Task> tasks = LoadTasks(filePath);
                tasks.Add(task);
                if (SaveTasks(filePath, tasks))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
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

        private static bool SaveTasks(string filePath, List<Task> tasks)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath) || tasks == null)
                {
                    return false;
                }

                string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false; // Error occurred, return false
            }
        }

    }
    public class Task
    {
        public string Description { get; set; }
        public TimeSpan Time { get; set; }

        public Task(string description, TimeSpan time)
        {
            Description = description;
            Time = time;
        }

        public Task()
        {
            Description = "Default";
            Time = TimeSpan.Zero;
        }
    }
}
