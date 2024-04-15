using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace Productivity
{
    public enum UpdateMode
    {
        Add,
        Complete,
        Uncomplete,
        Delete
    }
    internal class TaskManager
    {
        private static string dirPath;
        public static int TotalTasks { get; set; }
        public static int CompletedTasks { get; set; }
        public static int IncompleteTasks { get; set; }

        public TaskManager()
        {
            dirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TasksData");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            LoadStatistics();
        }
        public static void UpdateStatistics(UpdateMode mode)
        {
            if (mode == UpdateMode.Add)
            {
                TotalTasks++;
                IncompleteTasks++;
            }
            else if (mode == UpdateMode.Complete)
            {
                IncompleteTasks--;
                CompletedTasks++;
            }
            else if (mode == UpdateMode.Uncomplete)
            {
                IncompleteTasks++;
                CompletedTasks--;
            }
            else if (mode == UpdateMode.Delete)
            {
                TotalTasks--;
                CompletedTasks--;
            }
            SaveStatistics();
        }
        private static bool LoadStatistics()
        {
            string filePath = Path.Combine(dirPath,"statistics.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var stats = JsonConvert.DeserializeObject<List<int>>(json);
                if (stats != null)
                {
                    TotalTasks = stats[0];
                    CompletedTasks = stats[1];
                    IncompleteTasks = stats[2];
                }
                else
                {
                    TotalTasks = 0;
                    CompletedTasks = 0;
                    IncompleteTasks = 0;
                }
                return true;
            }
            else
            {
                File.Create(filePath);
                return false;
            }
        }
        private static bool SaveStatistics()
        {
            string filePath = Path.Combine(dirPath, "statistics.json");
            try
            {
                List<int> stats = [TotalTasks, CompletedTasks, IncompleteTasks];
                string json = JsonConvert.SerializeObject(stats, Formatting.Indented);
                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date">The date of the task.</param>
        /// <param name="task">The task to be added.</param>
        /// <returns><see langword="true"/> if the task was successfully added; otherwise, <see langword="false"/>.</returns>
        public static bool AddTask(DateTime date, Task task)
        {
            string filePath = GetFilePath(date);
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                try
                {
                    Dictionary<DateTime, List<Task>> tasks = LoadTasks(filePath);
                    if (tasks.TryGetValue(date, out List<Task>? value))
                    {
                        value.Add(task);
                    }
                    else
                    {
                        tasks[date] = [task];
                    }
                    return SaveTasks(filePath, tasks);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
            return false; // if something fails return false
        }

        public static int? GetTaskNumber(DateTime date)
        {
            string filePath = GetFilePath(date);
            Dictionary<DateTime, List<Task>> tasksByDate = LoadTasks(filePath);
            if (tasksByDate != null && tasksByDate.TryGetValue(date, out List<Task>? value))
            {
                if (value.Count > 0)
                {
                    return value.Count;
                }
            }
            return null;
        }

        public static bool DeleteTask(DateTime time, Task task)
        {
            string filePath = GetFilePath(time);
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                try
                {
                    Dictionary<DateTime, List<Task>> tasks = LoadTasks(filePath);
                    if (tasks.TryGetValue(time, out List<Task>? value))
                    {
                        if (value.RemoveAll(t => t.Description == task.Description && t.Time == task.Time) > 0)
                        {
                            return SaveTasks(filePath, tasks);
                        }
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
        public static bool UpdateTask(DateTime time, Task taskToUpdate)
        {
            string filePath = GetFilePath(time);
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                try
                {
                    Dictionary<DateTime, List<Task>> tasks = LoadTasks(filePath);
                    if (tasks.TryGetValue(time, out List<Task>? taskList))
                    {
                        Task task = taskList.FirstOrDefault(t => t.Description == taskToUpdate.Description && t.Time == taskToUpdate.Time);

                        if (task != null)
                        {
                            // Toggle the completion status
                            task.IsCompleted = !task.IsCompleted;
                            return SaveTasks(filePath, tasks);
                        }
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
        public static List<Task> GetTasks(DateTime date)
        {
            string filePath = GetFilePath(date);
            Dictionary<DateTime, List<Task>> tasksByDate = LoadTasks(filePath);
            if (tasksByDate != null && tasksByDate.TryGetValue(date, out List<Task>? value))
            {
                return value;
            }
            return [];
        }
        private static string GetFilePath(DateTime date)
        {
            string fileName = $"tasks-{date:yyyy-MM}.json";
            return Path.Combine(dirPath, fileName);
        }
        private static Dictionary<DateTime, List<Task>> LoadTasks(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Dictionary<DateTime, List<Task>>>(json);
            }
            return [];
        }

        private static bool SaveTasks(string filePath, Dictionary<DateTime, List<Task>> tasks)
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
        public bool IsCompleted { get; set; }
        internal readonly uint id;
        public Task(string description, TimeSpan time)
        {
            Description = description;
            Time = time;
            IsCompleted = false;
            id = (uint)new Random().Next();
        }

        public Task()
        {
            Description = "Default";
            Time = TimeSpan.Zero;
            IsCompleted = false;
            id = (uint)new Random().Next();
        }
    }
}
