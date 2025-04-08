using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Xml.Serialization;
using SyntaxGenDotNet.CIL;
using SyntaxGenDotNet.CppCLI;
using SyntaxGenDotNet.CSharp;
using SyntaxGenDotNet.Syntax;
using SyntaxGenDotNet.VisualBasic;
using UnityDocumentation.Common.Data.Xml;

namespace AssemblyBootstrap;

internal sealed class ConvertApp
{
    private readonly AssemblyLoadContext _context = new("Unity");
    private readonly List<Assembly> _assemblies = [];

    public void Run(Options options)
    {
        if (!ValidateOptions(options))
        {
            return;
        }

        LoadAllAssemblies(options.InputDirectory);
        LoadAllAssemblies(Path.Combine(options.InputDirectory, ".."));

        foreach (Assembly assembly in _assemblies)
        {
            Console.WriteLine($"Loading types from {assembly.GetName().Name}...");
            HandleAssembly(assembly, options);
        }
    }

    private static void CreateNamespaceXmlFile(string outputDirectory, Type type)
    {
        if (string.IsNullOrWhiteSpace(type.Namespace))
        {
            return;
        }

        string namespaceFile = Path.Combine(outputDirectory, $"ns-{type.Namespace}.xml");
        if (File.Exists(namespaceFile))
        {
            return;
        }

        using var stream = File.Open(namespaceFile, FileMode.Create, FileAccess.Write);
        using var writer = new StreamWriter(stream);

        var serializer = new XmlSerializer(typeof(NamespaceDefinition));
        var namespaceDefinition = new NamespaceDefinition
        {
            Name = type.Namespace,
            Documentation = new DocumentationDefinition
            {
                Summary = string.Empty
            }
        };

        serializer.Serialize(writer, namespaceDefinition);
    }

    private static void CreateTypeXmlFile(Options options, Type type)
    {
        Console.WriteLine($"Creating XML file for {GetSanitizedTypeName(type)}");
        string safeFileName = type.Name.Replace('<', '_').Replace('>', '_');
        var outputDirectory = options.OutputDirectory;
        string namespaceDirectory = string.IsNullOrWhiteSpace(type.Namespace)
            ? outputDirectory
            : Path.Combine(outputDirectory, type.Namespace);
        string typeFile = Path.Combine(namespaceDirectory, $"{safeFileName}.xml");

        Directory.CreateDirectory(namespaceDirectory);

        using var stream = File.Open(typeFile, FileMode.Create, FileAccess.Write);
        using var writer = new StreamWriter(stream);

        var serializer = new XmlSerializer(typeof(TypeDefinition));
        serializer.Serialize(writer, CreateTypeDefinition(options, type));
    }

    private static TypeDefinition CreateTypeDefinition(Options options, Type type)
    {
        var assemblies = new[]
        {
            new AssemblyInfoDefinition
            {
                Name = type.Assembly.GetName().Name!,
                Versions = [options.Version]
            }
        };
        var interfaces = new List<InterfaceDefinition>();
        var members = new List<MemberDefinition>();

        foreach (Type interfaceType in type.GetInterfaces())
        {
            var definition = new InterfaceDefinition
            {
                Name = GetSanitizedTypeName(interfaceType)
            };
            interfaces.Add(definition);
        }

        GetConstructors(type, assemblies, members);
        GetMethods(type, assemblies, members);

        Type baseType = type.BaseType ?? (type.IsEnum ? typeof(Enum) : type.IsValueType ? typeof(ValueType) : typeof(object));

        return new TypeDefinition
        {
            Assemblies = assemblies,
            Base = new BaseDefinition { TypeName = GetSanitizedTypeName(baseType) },
            Interfaces = new InterfacesDefinition { Interfaces = interfaces.ToArray() },
            Documentation = new DocumentationDefinition { Summary = "To be added.", Remarks = "To be added." },
            Members = new MembersDefinition { Members = members.ToArray() },
            Name = GetSanitizedTypeName(type, false),
            FullName = GetSanitizedTypeName(type),
            Signatures = GetTypeSignatures(type)
        };
    }

    private static void GetConstructors(Type type, AssemblyInfoDefinition[] assemblies, List<MemberDefinition> members)
    {
        foreach (ConstructorInfo constructor in type.GetConstructors())
        {
            if (!constructor.IsPublic && (constructor.Attributes & MethodAttributes.FamANDAssem) == 0)
            {
                // ignore non-public constructors
                continue;
            }

            var parameters = new List<ParameterDefinition>();
            for (var index = 0; index < constructor.GetParameters().Length; index++)
            {
                var parameter = constructor.GetParameters()[index];
                if (string.IsNullOrWhiteSpace(parameter.Name))
                {
                    continue;
                }

                var parameterDefinition = new ParameterDefinition
                {
                    Name = parameter.Name,
                    Type = GetSanitizedTypeName(parameter.ParameterType),
                    Index = index,
                };
                parameters.Add(parameterDefinition);
            }

            var definition = new MemberDefinition
            {
                Name = ".ctor",
                Documentation = new DocumentationDefinition
                {
                    Parameters = parameters.Select(p => new ParameterDocumentationDefinition
                    {
                        Name = p.Name!,
                        Summary = "To be added.",
                    }).ToArray(),
                    Summary = "To be added.",
                    Remarks = "To be added."
                },
                Type = MemberTypes.Constructor,
                Assemblies = assemblies,
                Signatures = GetMemberSignatures(constructor),
                Parameters = parameters.Count == 0 ? null : new ParametersDefinition { Parameters = parameters.ToArray() }
            };

            members.Add(definition);
        }
    }

    private static void GetMethods(Type type, AssemblyInfoDefinition[] assemblies, List<MemberDefinition> members)
    {
        var properties = type.GetProperties();
        foreach (MethodInfo method in type.GetMethods())
        {
            if (method.IsConstructor ||
                ((method.Name.StartsWith("get_") || method.Name.StartsWith("set_")) &&
                 properties.Any(p => p.Name == method.Name[4..])))
            {
                // ignore ctor and property accessors, they are technically methods too!
                continue;
            }

            if (!method.IsPublic && (method.Attributes & MethodAttributes.FamANDAssem) == 0)
            {
                // ignore non-public constructors
                continue;
            }

            var definitions = new List<ParameterDefinition>();
            ParameterInfo[] parameters = method.GetParameters();

            for (var index = 0; index < parameters.Length; index++)
            {
                var parameter = parameters[index];
                if (string.IsNullOrWhiteSpace(parameter.Name))
                {
                    continue;
                }

                var parameterDefinition = new ParameterDefinition
                {
                    Name = parameter.Name,
                    Type = GetSanitizedTypeName(parameter.ParameterType),
                    Index = index,
                };
                definitions.Add(parameterDefinition);
            }

            var definition = new MemberDefinition
            {
                Name = method.Name,
                Documentation = new DocumentationDefinition
                {
                    Parameters = parameters.Select(p => new ParameterDocumentationDefinition
                    {
                        Name = p.Name!,
                        Summary = "To be added.",
                    }).ToArray(),
                    Summary = "To be added.",
                    Remarks = "To be added."
                },
                Type = MemberTypes.Method,
                Assemblies = assemblies,
                Signatures = GetMemberSignatures(method),
                Parameters = definitions.Count == 0 ? null : new ParametersDefinition { Parameters = definitions.ToArray() }
            };

            members.Add(definition);
        }
    }

    private static SignatureDefinition[] GetMemberSignatures(ConstructorInfo constructor)
    {
        return
        [
            GetTypeSignature("C#", new CSharpSyntaxGenerator().GenerateConstructorDeclaration(constructor)),
            GetTypeSignature("ILAsm", new CilSyntaxGenerator().GenerateConstructorDeclaration(constructor)),
            GetTypeSignature("DocId", $"T:{constructor.Name}"),
            GetTypeSignature("VB.NET", new VisualBasicSyntaxGenerator().GenerateConstructorDeclaration(constructor)),
            GetTypeSignature("C++ CLI", new CppCliSyntaxGenerator().GenerateConstructorDeclaration(constructor))
        ];
    }

    private static SignatureDefinition[] GetMemberSignatures(MethodInfo method)
    {
        return
        [
            GetTypeSignature("C#", new CSharpSyntaxGenerator().GenerateMethodDeclaration(method)),
            GetTypeSignature("ILAsm", new CilSyntaxGenerator().GenerateMethodDeclaration(method)),
            GetTypeSignature("DocId", $"T:{method.Name}"),
            GetTypeSignature("VB.NET", new VisualBasicSyntaxGenerator().GenerateMethodDeclaration(method)),
            GetTypeSignature("C++ CLI", new CppCliSyntaxGenerator().GenerateMethodDeclaration(method))
        ];
    }

    private static SignatureDefinition[] GetTypeSignatures(Type type)
    {
        return
        [
            GetTypeSignature("C#", new CSharpSyntaxGenerator().GenerateTypeDeclaration(type)),
            GetTypeSignature("ILAsm", new CilSyntaxGenerator().GenerateTypeDeclaration(type)),
            GetTypeSignature("DocId", $"T:{type.FullName}"),
            GetTypeSignature("VB.NET", new VisualBasicSyntaxGenerator().GenerateTypeDeclaration(type)),
            GetTypeSignature("C++ CLI", new CppCliSyntaxGenerator().GenerateTypeDeclaration(type))
        ];
    }

    private static SignatureDefinition GetTypeSignature(string language, SyntaxNode node)
    {
        return GetTypeSignature(language, GetSyntaxString(node));
    }

    private static SignatureDefinition GetTypeSignature(string language, string syntax)
    {
        return new SignatureDefinition { Language = language, Value = syntax };
    }

    private static string GetSyntaxString(SyntaxNode node)
    {
        var builder = new StringBuilder();
        builder.Append(node.LeadingWhitespace);

        for (var index = 0; index < node.Children.Count; index++)
        {
            SyntaxNode child = node.Children[index];
            if (child.Children.Count > 0)
            {
                builder.Append(GetSyntaxString(child));
                continue;
            }

            builder.Append(child.LeadingWhitespace);
            builder.Append(child.Text);

            if (index < node.Children.Count - 1 && !node.Children[index + 1].StripTrailingWhitespace)
            {
                builder.Append(child.TrailingWhitespace);
            }
        }

        if (node.Children.Count > 0 && node.Children[0].Children.Count > 0)
        {
            builder.Append(node.TrailingWhitespace);
        }

        return builder.ToString().Replace("\r\n", "\n");
    }

    private static bool ValidateOptions(Options options)
    {
        if (string.IsNullOrWhiteSpace(options.InputDirectory))
        {
            Console.WriteLine("No assemblies provided.");
            return false;
        }

        if (!Directory.Exists(options.InputDirectory))
        {
            Console.WriteLine($"Input directory '{options.InputDirectory}' does not exist.");
            return false;
        }

        if (!Directory.Exists(options.OutputDirectory))
        {
            Console.WriteLine($"Output directory '{options.OutputDirectory}' does not exist.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(options.Version))
        {
            Console.WriteLine("No Unity API version provided.");
            return false;
        }

        return true;
    }

    private static string GetSanitizedTypeName(Type type, bool fullName = true)
    {
        string name = fullName ? type.FullName ?? type.Name : type.Name;

        if (!type.IsGenericType)
        {
            return name;
        }

        int index = name.IndexOf('`');
        if (index > 0)
        {
            name = name[..index];
        }

        name += "<";
        Type[] genericArguments = type.GetGenericArguments();
        for (index = 0; index < genericArguments.Length; index++)
        {
            if (index > 0)
            {
                name += ",";
            }

            name += GetSanitizedTypeName(genericArguments[index]);
        }

        name += ">";
        return name;
    }

    private static void HandleAssembly(Assembly assembly, Options options)
    {
        try
        {
            var types = new List<Type>();

            try
            {
                types.AddRange(assembly.GetTypes());
            }
            catch (ReflectionTypeLoadException ex)
            {
                types.AddRange(ex.Types.Where(t => t != null)!);
            }

            foreach (Type type in types)
            {
                if (type is { IsPublic: false, IsVisible: false })
                {
                    continue;
                }

                try
                {
                    CreateNamespaceXmlFile(options.OutputDirectory, type);
                    CreateTypeXmlFile(options, type);
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error creating XML file for {type}: {exception}");
                    Console.ResetColor();
                }
            }
        }
        catch (Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error processing assembly {assembly.GetName().Name}: {exception}");
            Console.ResetColor();
        }
    }

    private void LoadAllAssemblies(string directory)
    {
        foreach (string dllFile in Directory.EnumerateFiles(directory, "*.dll"))
        {
            LoadAssembly(dllFile);
        }
    }

    private void LoadAssembly(string path)
    {
        try
        {
            Assembly assembly = _context.LoadFromAssemblyPath(path);
            Console.WriteLine($"Loaded {assembly.GetName().Name}");
            _assemblies.Add(assembly);
        }
        catch (FileLoadException exception)
        {
            if (!exception.Message.Contains("already loaded"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Could not load {Path.GetFileName(path)}: {exception.Message}");
                Console.ResetColor();
            }
        }
    }
}
