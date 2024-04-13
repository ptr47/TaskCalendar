using Newtonsoft.Json;
using System.IO;

namespace Productivity
{
    internal class TaskManager
    {
        private Dictionary<DateTime, List<Task>> tasksByDate;
        private string filePath;

        public TaskManager(string filePath)
        {
            this.filePath = filePath;
            tasksByDate = LoadTasks();
        }

        public Dictionary<DateTime, List<Task>> TasksByDate => tasksByDate;

        public void AddTask(DateTime date, Task task)
        {
            if (!tasksByDate.ContainsKey(date))
            {
                tasksByDate[date] = new List<Task>();
            }
            tasksByDate[date].Add(task);
            SaveTasks();
        }

        private Dictionary<DateTime, List<Task>> LoadTasks()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Dictionary<DateTime, List<Task>>>(json);
            }
            return new Dictionary<DateTime, List<Task>>();
        }

        private void SaveTasks()
        {
            string json = JsonConvert.SerializeObject(tasksByDate, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }

}
