using Microsoft.Extensions.DependencyInjection.Extensions;


namespace PhilippWebsite.Client.Services.OutputLogger
{
    public static class OutputLoggerExtension
    {
        public static ILoggingBuilder AddOutpurLogger(this ILoggingBuilder builder)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, OutputLoggerProvider>());

            return builder;
        }
    }
}
