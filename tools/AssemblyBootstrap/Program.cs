using AssemblyBootstrap;
using CommandLine;

Parser.Default.ParseArguments<Options>(args).WithParsed(new ConvertApp().Run);
