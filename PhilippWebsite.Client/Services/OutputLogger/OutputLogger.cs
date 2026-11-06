namespace PhilippWebsite.Client.Services.OutputLogger
{
    public class OutputLogger : ILogger
    {
        private readonly OutputLogStore _outputLogStore;

        private readonly string _name;


        public OutputLogger(OutputLogStore outputLogStore, string name)
        {
            this._outputLogStore = outputLogStore;

            this._name = name;
        }


        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;


        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!this.IsEnabled(logLevel)) return;

            string message = formatter(state, exception);

            this._outputLogStore.AddLog($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{logLevel}] [{this._name}]: {message}");
        }
    }
}
