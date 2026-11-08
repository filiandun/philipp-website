
namespace PhilippWebsite.Client.Models.BarSections
{
    public static class BarSectionExtensions
    {
        public static string GetTitle(this ActivityBarSection section) {

            return section switch
            {
                ActivityBarSection.SolutionExplorer => "Solution Explorer",

                _ => string.Empty
            };
        }

        public static string GetTitle(this BottomBarSection section)
        {

            return section switch
            {
                BottomBarSection.Output => "Output",

                _ => string.Empty
            };
        }
    }
}
