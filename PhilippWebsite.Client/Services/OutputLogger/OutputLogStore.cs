namespace PhilippWebsite.Client.Services.OutputLogger
{
    public class OutputLogStore
    {
        private const int LOGS_LIMIT = 50;

        public event Action? OnLogAdded;

        private readonly Queue<string> _logs;


        public OutputLogStore()
        {
            this._logs = new Queue<string>();
        }


        public IEnumerable<string> GetLogs() => this._logs;

        public void AddLog(string log)
        {
            this._logs.Enqueue(log);


            while (this._logs.Count > LOGS_LIMIT)
            {
                this._logs.Dequeue();
            }

            this.OnLogAdded?.Invoke();
        }
    }
}
