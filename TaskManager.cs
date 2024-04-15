using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Productivity
{
    public enum UpdateMode
    {
        Add,
        Complete,
        Delete
    }
    internal class TaskManager
    {
        private readonly string dirPath;
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int IncompleteTasks { get; set; }

        public TaskManager(string dirPath)
        {
            this.dirPath = dirPath;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            TotalTasks = 0;
            CompletedTasks = 0;
            IncompleteTasks = 0;
        }
        public void UpdateStatistics(UpdateMode mode)
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
            else if (mode == UpdateMode.Delete)
            {
                TotalTasks--;
                CompletedTasks--;
            }
        }
        private bool LoadStatistics()
        {
            string filePath = dirPath + "statistics.json";
            if(File.Exists(filePath))
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
                return false;
            }
        }
        private bool SaveStatistics()
        {
            string filePath = dirPath + "statistics.json";
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
        public bool AddTask(DateTime date, Task task)
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

        public bool DeleteTask(DateTime time, Task task)
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
        public bool UpdateTask(DateTime time, Task taskToUpdate)
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
        public List<Task> GetTasks(DateTime date)
        {
            string filePath = GetFilePath(date);
            Dictionary<DateTime, List<Task>> tasksByDate = LoadTasks(filePath);
            if (tasksByDate != null && tasksByDate.TryGetValue(date, out List<Task>? value))
            {
                return value;
            }
            return [];
        }
        private string GetFilePath(DateTime date)
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
        public Task(string description, TimeSpan time)
        {
            Description = description;
            Time = time;
            IsCompleted = false;
        }

        public Task()
        {
            Description = "Default";
            Time = TimeSpan.Zero;
            IsCompleted = false;
        }
    }
}
