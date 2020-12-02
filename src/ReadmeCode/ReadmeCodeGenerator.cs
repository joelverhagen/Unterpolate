using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Knapcode.ReadmeCode
{
    [Generator]
    public class ReadmeCodeGenerator : ISourceGenerator
    {
        private const string DiagnosticCategory = "ReadmeCode";

        public void Execute(GeneratorExecutionContext context)
        {
            // System.Diagnostics.Debugger.Launch();

            foreach (var additionalFile in context.AdditionalFiles)
            {
                if (additionalFile.Path.EndsWith(".md", StringComparison.Ordinal))
                {
                    var additionalFileDir = Path.GetDirectoryName(additionalFile.Path);
                    var regions = MarkdownExtractor.ReadMarkdownCodeRegions(additionalFile.Path);
                    var filePathToRegions = new ConcurrentDictionary<string, IReadOnlyDictionary<string, string>>();

                    foreach (var region in regions)
                    {
                        var fullFilePath = Path.GetFullPath(Path.Combine(additionalFileDir, region.FilePath));
                        var codeRegions = filePathToRegions.GetOrAdd(fullFilePath, CodeExtractor.ReadCodeRegions);
                        if (!codeRegions.TryGetValue(region.RegionName, out var code))
                        {
                            context.ReportDiagnostic(Diagnostic.Create(
                                new DiagnosticDescriptor(
                                    id: "RMC0001",
                                    title: "Region does not exist",
                                    messageFormat: "The region {0} does not exist in {1}. The available regions are {2}.",
                                    DiagnosticCategory,
                                    DiagnosticSeverity.Error,
                                    isEnabledByDefault: true),
                                Location.None,
                                region.RegionName,
                                region.FilePath,
                                string.Join(", ", codeRegions.Keys.OrderBy(x => x))));
                        }

                        var trimmedRegion = StringUtility.TrimIndentation(region.Content);
                        var trimmedCode = StringUtility.TrimIndentation(code);

                        switch (region.Type)
                        {
                            case MarkdowRegionType.CodeRegion:
                                if (trimmedCode != trimmedRegion)
                                {
                                    context.ReportDiagnostic(Diagnostic.Create(
                                        new DiagnosticDescriptor(
                                            id: "RMC0002",
                                            title: "Code region does not match Markdown",
                                            messageFormat: "The code region {0} from {1} does not match the Markdown.",
                                            DiagnosticCategory,
                                            DiagnosticSeverity.Error,
                                            isEnabledByDefault: true),
                                        Location.None,
                                        region.RegionName,
                                        region.FilePath));
                                }
                                break;
                            case MarkdowRegionType.CommandOutput:
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                }

            }

            var sourceText = SourceText.From("// foo", Encoding.UTF8);
            context.AddSource(DiagnosticCategory, sourceText);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
