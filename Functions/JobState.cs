namespace PyxeraConcurIntegrationConsole
{
    public class ProcessedJob
    {
        public string JobId { get; set; } = "";
        public string Month { get; set; } = "";
    }

    public class JobState
    {
        public DateTime LastRunDate { get; set; } = DateTime.MinValue;
        public List<ProcessedJob> ProcessedJobs { get; set; } = new();
    }

    public class StateManager
    {
        private readonly string _filePath;

        public StateManager(string fileName = "state.json")
        {
            // Check if running inside Azure App Service
            string home = Environment.GetEnvironmentVariable("HOME");

            string basePath;
            if (!string.IsNullOrEmpty(home))
            {
                // App Service -> use persistent D:\home\data
                basePath = Path.Combine(home, "data");
            }
            else
            {
                // Local dev -> put it in ./data
                basePath = Path.Combine(Directory.GetCurrentDirectory(), "data");
            }

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            _filePath = Path.Combine(basePath, fileName);
        }

        public JobState Load()
        {
            if (!File.Exists(_filePath))
            {
                var defaultState = new JobState
                {
                    LastRunDate = DateTime.Now,
                    ProcessedJobs = new List<ProcessedJob>()
                };

                Save(defaultState);
                return defaultState;
            }

            var json = File.ReadAllText(_filePath);
            return System.Text.Json.JsonSerializer.Deserialize<JobState>(json) ?? new JobState();
        }

        public void Save(JobState state)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(
                state,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true }
            );
            File.WriteAllText(_filePath, json);
        }
    }

}