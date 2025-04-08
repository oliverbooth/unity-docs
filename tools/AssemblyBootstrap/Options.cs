using CommandLine;

namespace AssemblyBootstrap;

/// <summary>
///     Represents the command line options for the AssemblyBootstrap application.
/// </summary>
internal sealed class Options
{
    /// <summary>
    ///     Gets or sets the input directory where the processed assemblies will be read.
    /// </summary>
    /// <value>The input directory.</value>
    [Value(0, HelpText = "Input directory containing assemblies to process.", Required = true)]
    public string InputDirectory { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the output directory where the processed documentation will be written.
    /// </summary>
    /// <value>The output directory.</value>
    [Value(1, HelpText = "Output directory for processed documentation.", Required = true)]
    public string OutputDirectory { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the Unity API version.
    /// </summary>
    /// <value>The Unity API version.</value>
    [Option('v', "version", HelpText = "The Unity API version.", Required = true)]
    public string Version { get; set; } = string.Empty;
}
