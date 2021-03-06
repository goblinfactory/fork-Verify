﻿using System.Threading.Tasks;
using Verify;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

public class ExtensionSample :
    VerifyBase
{
    VerifySettings classLevelSettings;

    public ExtensionSample(ITestOutputHelper output) :
        base(output)
    {
        classLevelSettings = new VerifySettings();
        classLevelSettings.UseExtension("json");
    }

    [Fact]
    public Task AtMethod()
    {
        var settings = new VerifySettings(classLevelSettings);
        settings.UseExtension("xml");
        return Verify(
            target: @"<note>
<to>Joe</to>
<from>Kim</from>
<heading>Reminder</heading>
</note>",
            settings: settings);
    }

    [Fact]
    public Task SharedClassLevelSettings()
    {
        return Verify(
            target: @"{
    ""fruit"": ""Apple"",
    ""size"": ""Large"",
    ""color"": ""Red""
}",
            settings: classLevelSettings);
    }
}