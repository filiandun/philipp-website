namespace PhilippWebsite.Client.Services.OutputLogger
{
    [ProviderAlias("Output")]
    public class OutputLoggerProvider : ILoggerProvider
    {
        private readonly OutputLogStore _outputLogStore;


        public OutputLoggerProvider(OutputLogStore outputLogStore)
        {
            this._outputLogStore = outputLogStore;
        }


        public ILogger CreateLogger(string categoryName)
        {
            return new OutputLogger(this._outputLogStore,categoryName);
        }

        public void Dispose() { }
    }
}
