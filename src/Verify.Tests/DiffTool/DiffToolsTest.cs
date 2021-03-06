﻿using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

public class DiffToolsTest :
    VerifyBase
{
    [Fact]
    public async Task WriteFoundTools()
    {
        var md = Path.Combine(SourceDirectory, "diffTools.include.md");
        File.Delete(md);
        using var writer = File.CreateText(md);
        foreach (var tool in DiffTools.Tools())
        {
            await writer.WriteLineAsync($@"
## [{tool.Name}]({tool.Url})");
            if (tool.WindowsExePaths.Any())
            {
                await writer.WriteLineAsync(@"
### Windows scanned paths:
");
                foreach (var path in tool.WindowsExePaths)
                {
                    await writer.WriteLineAsync($@" * `{path}`");
                }
            }

            if (tool.OsxExePaths.Any())
            {
                await writer.WriteLineAsync(@"
### OSX scanned paths:
");
                foreach (var path in tool.OsxExePaths)
                {
                    await writer.WriteLineAsync($@" * `{path}`");
                }
            }

            if (tool.LinuxExePaths.Any())
            {
                await writer.WriteLineAsync(@"
### Linux scanned paths:
");
                foreach (var path in tool.LinuxExePaths)
                {
                    await writer.WriteLineAsync($@" * `{path}`");
                }
            }
            if (!tool.BinaryExtensions.Any())
            {
                continue;
            }

            await writer.WriteLineAsync(@"
### Supported binary extensions:
");
            foreach (var extension in tool.BinaryExtensions)
            {
                await writer.WriteLineAsync($@" * {extension}");
            }
        }
    }

    //[Fact]
    //public void LaunchImageDiff()
    //{
    //    foreach (var tool in DiffTools.Tools().Where(x => x.Exists))
    //    {
    //        if (!tool.BinaryExtensions.Contains("png"))
    //        {
    //            continue;
    //        }

    //        DiffRunner.Launch(
    //            new ResolvedDiffTool(tool.Name, tool.ExePath!, tool.ArgumentPrefix),
    //            receivedPath: Path.Combine(SourceDirectory, "input_received.png"),
    //            verifiedPath: Path.Combine(SourceDirectory, "input_verified.png"));
    //    }
    //}

    //[Fact]
    //public void LaunchTextDiff()
    //{
    //    foreach (var tool in DiffTools.ResolvedDiffTools)
    //    {
    //        DiffRunner.Launch(
    //            tool,
    //            receivedPath: Path.Combine(SourceDirectory, "input_received.txt"),
    //            verifiedPath: Path.Combine(SourceDirectory, "input_verified.txt"));
    //    }
    //}

    [Fact]
    public void ExtensionLookup()
    {
        foreach (var tool in DiffTools.ExtensionLookup)
        {
            Debug.WriteLine($"{tool.Key}: {tool.Value.Name}");
        }
    }

    public DiffToolsTest(ITestOutputHelper output) :
        base(output)
    {
    }
}