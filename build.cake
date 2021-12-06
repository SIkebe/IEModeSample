using System.Diagnostics;

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
});

Task("Build")
    .IsDependentOn("Clean")
    .Does((ctx) =>
{
    DotNetBuild("./IEModeSample.sln", new DotNetBuildSettings { Configuration = configuration });
});

Task("Launch-Kestrel")
    .IsDependentOn("Build")
    .Does(async (ctx) =>
{
    // Run dotnet process background
    var process = new Process();
    process.StartInfo.FileName = "dotnet";
    process.StartInfo.Arguments = "run";
    process.StartInfo.WorkingDirectory = MakeAbsolute(Directory("./src/CookieSample")).FullPath;
    process.Start();

    await System.Threading.Tasks.Task.Delay(3000);
});

Task("Run-E2E-Tests")
    .IsDependentOn("Launch-Kestrel")
    .Does((ctx) =>
{
    DotNetTest("./src/CookieSample.E2ETests", new DotNetTestSettings { Configuration = configuration, NoBuild = true });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
