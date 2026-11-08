
namespace PhilippWebsite.Client.Services.WorkbenchState
{
    public class BarStateService<TSection> : IBarStateService<TSection> where TSection : struct, Enum 
    {
        public event Action? OnChanged;

        private readonly ILogger<BarStateService<TSection>> _logger;

        public TSection? ActiveSection { get; private set; }

        public bool IsPinned { get; private set; }
        public bool IsOpen => this.ActiveSection is not null;


        public BarStateService(ILogger<BarStateService<TSection>> logger)
        {
            this.ActiveSection = null;

            this.IsPinned = false;

            this._logger = logger;
        }

        public BarStateService(ILogger<BarStateService<TSection>> logger, TSection activeSection, bool isPinned)
        {
            this.ActiveSection = activeSection;

            this.IsPinned = isPinned;

            this._logger = logger;

            this.OnChanged?.Invoke();
        }


        public void Toggle(TSection section)
        {
            this.ActiveSection = (this.ActiveSection is not null && this.ActiveSection.Equals(section)) ? null : section;

            this._logger.LogInformation("Toggle to '{ActiveSection}'", this.ActiveSection);

            this.OnChanged?.Invoke();
        }

        public void Close()
        {
            this._logger.LogInformation("Close '{ActiveSection}'", this.ActiveSection);

            this.ActiveSection = null;

            this.OnChanged?.Invoke();
        }

        public void TogglePin()
        {
            this.IsPinned = !this.IsPinned;

            this._logger.LogInformation("Toggle pin '{ActiveSection}' to '{IsPinned}'", this.ActiveSection, this.IsPinned);

            this.OnChanged?.Invoke();
        }
    }
}
