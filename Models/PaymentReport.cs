using Newtonsoft.Json;
namespace PyxeraConcurIntegrationConsole
{
    public class Definition
    {
        public string id { get; set; }
        public string name { get; set; }

        [JsonProperty("job-link")]
        public string joblink { get; set; }
    }

    public class Definitions
    {
        public List<Definition> definition { get; set; }
    }

    public class RootDefinition
    {
        public Definitions definitions { get; set; }
    }

    public class RootJob
    {
        public Jobs jobs { get; set; }
    }
    public class Jobs
    {
        public List<Job> job { get; set; }
    }
    public class Job
    {
        public string jobname { get; set; }
        public string jobid { get; set; }
        [JsonProperty("id")]
        [JsonConverter(typeof(NullReplacementConverterI))]
        public string id { get; set; }
        [JsonProperty("status-link")]
        [JsonConverter(typeof(NullReplacementConverterI))]
        public string statuslink { get; set; }
        [JsonProperty("start-time")]
        public DateTime starttime { get; set; }
        [JsonProperty("stop-time")]
        public DateTime stoptime { get; set; }
        [JsonConverter(typeof(NullReplacementConverterI))]
        public string status { get; set; }
        [JsonProperty("file-link")]
        [JsonConverter(typeof(NullReplacementConverterI))]
        public string filelink { get; set; }
    }
}