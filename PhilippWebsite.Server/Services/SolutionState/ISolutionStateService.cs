using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Server.Services.SolutionState
{
    public interface ISolutionStateService
    {
        public Task<SolutionStateModel> GetModel();
    }
}