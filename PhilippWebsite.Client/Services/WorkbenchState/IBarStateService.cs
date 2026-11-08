namespace PhilippWebsite.Client.Services.WorkbenchState
{
    public interface IBarStateService<TSection> where TSection : struct, Enum
    {
        public event Action? OnChanged;

        public TSection? ActiveSection { get; }

        public bool IsPinned { get; }
        public bool IsOpen { get; }

        public void Toggle(TSection section);
        public void Close();

        public void TogglePin();
    }
}
