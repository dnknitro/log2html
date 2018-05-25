param($installPath, $toolsPath, $package, $project)

$log4netConfig = $project.ProjectItems.Item("log4net.config");
$log4netConfig.Properties.Item("CopyToOutputDirectory").Value = [int]2; # copy if newer
$log4netConfig.Properties.Item("BuildAction").Value = [int]2; # content Build Action


try
{
    # EnvDTE https://docs.microsoft.com/en-us/dotnet/api/envdte?view=visualstudiosdk-2017
    #
    # To test this script in nuget Package Manager Console:
    # $project = Get-Project
    #

    # AssemblyInfo.cs :
    $assemblyInfo = $project.ProjectItems.Item("Properties").ProjectItems.Item("AssemblyInfo.cs")
    $assemblyInfo.Open()
    $editDoc = $assemblyInfo.Document.Object("TextDocument")

    $strToAdd = "[assembly: log4net.Config.XmlConfigurator(ConfigFile = `"log4net.config`")]"

    if(!$editDoc.MarkText($strToAdd)) {
        $edit = $editDoc.EndPoint.CreateEditPoint()
        $edit.EndOfDocument()
        $edit.Insert([Environment]::NewLine)
        $edit.Insert("[assembly: log4net.Config.XmlConfigurator(ConfigFile = `"log4net.config`")]")
    }


    # GlobalTestSetup.cs
    # $assemblyInfo = $project.ProjectItems.Item("GlobalTestSetup.cs").ProjectItems.Item("GlobalTestSetup.cs")
    # $assemblyInfo.Open()
    # $editDoc = $assemblyInfo.Document.Object("TextDocument")

    # $strToAdd = "[assembly: log4net.Config.XmlConfigurator(ConfigFile = `"log4net.config`")]"

    # if(!$editDoc.MarkText($strToAdd)) {
    #     $edit = $editDoc.EndPoint.CreateEditPoint()
    #     $edit.EndOfDocument()
    #     $edit.Insert([Environment]::NewLine)
    #     $edit.Insert("[assembly: log4net.Config.XmlConfigurator(ConfigFile = `"log4net.config`")]")
    # }
}
catch
{
    Write-Host "Failed to update AssemblyInfo.cs"
}