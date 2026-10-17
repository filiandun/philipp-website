using PhilippWebsite.Shared.SolutionState;


namespace PhilippWebsite.Client.Clients.SolutionState
{
    public interface ISolutionStateClient
    {
        public Task<SolutionStateModel?> GetModel();
    }
}
