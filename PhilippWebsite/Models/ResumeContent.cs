namespace PhilippWebsite.Models
{
    public sealed class ResumeContent
    {
        public HeroContent Hero { get; set; } = new();

        public AboutContent About { get; set; } = new();
        public StackContent Stack { get; set; } = new();
        public ProjectsContent Projects { get; set; } = new();
        public ContactsContent Contacts { get; set; } = new();
    }


    public sealed class HeroContent
    {
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }


    public sealed class AboutContent
    {
        public string Title { get; set; } = string.Empty;

        public string[] Paragraphs { get; set; } = [];
    }


    public sealed class StackContent
    {
        public string Title { get; set; } = string.Empty;

        public StackGroup[] Groups { get; set; } = [];
    }

    public sealed class StackGroup
    {
        public string Title { get; set; } = string.Empty;

        public string[] Items { get; set; } = [];
    }


    public sealed class ProjectsContent
    {
        public string Title { get; set; } = string.Empty;

        public ProjectContent[] Items { get; set; } = [];
    }

    public sealed class ProjectContent
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string[] Stack { get; set; } = [];
        public string[] Highlights { get; set; } = [];

        public string GithubUrl { get; set; } = string.Empty;
        public string DemoUrl { get; set; } = string.Empty;
    }


    public sealed class ContactsContent
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ContactContent[] Items { get; set; } = [];
    }

    public sealed class ContactContent
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
    }
}
