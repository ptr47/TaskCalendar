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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date">The date of the task.</param>
        /// <param name="task">The task to be added.</param>
        /// <returns><see langword="true"/> if the task was successfully added; otherwise, <see langword="false"/>.</returns>

        public bool AddTask(DateTime date, Task task)
        {
            string filePath = GetFilePath(date);
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                try
                {
                    Dictionary<DateTime,Task> tasks = LoadTasks(filePath);
                    tasks.Add(date,task);
                    if (SaveTasks(filePath, tasks))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
            return false; // if something fails return false
        }

        private string GetFilePath(DateTime date)
        {
            string fileName = $"tasks-{date:yyyy-MM}.json";
            return Path.Combine(dirPath, fileName);
        }
        private static Dictionary<DateTime,Task> LoadTasks(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Dictionary<DateTime,Task>>(json);
            }
            return [];
        }

        private static bool SaveTasks(string filePath, Dictionary<DateTime,Task> tasks)
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
