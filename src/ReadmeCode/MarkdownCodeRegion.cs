namespace Knapcode.ReadmeCode
{
    public class MarkdownRegion
    {
        public MarkdownRegion(MarkdowRegionType type, string content)
        {
            Type = type;
            Content = content;
        }

        public MarkdowRegionType Type { get; }
        public string Content { get; }
    }

    public class MarkdownCommandOuput : MarkdownRegion
    {
        public MarkdownCommandOuput(string content, string parameters)
            : base(MarkdowRegionType.CommandOutput, content)
        {
            Parameters = parameters;
        }

        public string Parameters { get; }
    }

    public class MarkdownCodeRegion : MarkdownRegion
    {
        public MarkdownCodeRegion(string content, string filePath, string regionName)
            : base(MarkdowRegionType.CodeRegion, content)
        {
            FilePath = filePath;
            RegionName = regionName;
        }

        public string FilePath { get; }
        public string RegionName { get; }
    }
}
