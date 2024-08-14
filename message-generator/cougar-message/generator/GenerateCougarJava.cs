using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text;
using StringTemplate;

namespace CougarMessage.Generator
{
    public class GenerateCougarCSharp
    {
        public static bool Generate(MessageSchemaAdapter schemaAdapter, CougarGenerateConfig generateConfig)
        {
            string csharpPath = Path.Combine(generateConfig.MessagesConfig.Destination.Root, "csharp");
            string csharpInterfacePath = Path.Combine(csharpPath, "Interfaces");
            string csharpEnumPath = Path.Combine(csharpPath, "Enums");
            string csharpDefinePath = Path.Combine(csharpPath, "Defines");

            Directory.CreateDirectory(csharpPath);
            Directory.CreateDirectory(csharpInterfacePath);
            Directory.CreateDirectory(csharpEnumPath);
            Directory.CreateDirectory(csharpDefinePath);

            schemaAdapter.SetPackageName(generateConfig.MessagesConfig.PackageName);

            foreach (var message in schemaAdapter.GetMessages())
            {
                try
                {
                    // Write the C# class
                    using (StreamWriter writer = new StreamWriter(Path.Combine(csharpPath, message.GetCSharpClassFileName())))
                    {
                        StringTemplate.Template stringTemplate = TemplateManager.Instance.FindTemplate("defineCougarMessagesCSharp", "");
                        stringTemplate.Add("schema", schemaAdapter);
                        stringTemplate.Add("message", message);
                        writer.Write(stringTemplate.Render());
                    }

                    // Write the C# interface
                    using (StreamWriter writer = new StreamWriter(Path.Combine(csharpInterfacePath, message.GetCSharpInterfaceFileName())))
                    {
                        StringTemplate.Template stringTemplate = TemplateManager.Instance.FindTemplate("defineCougarMessageInterface", "");
                        stringTemplate.Add("schema", schemaAdapter);
                        stringTemplate.Add("message", message);
                        writer.Write(stringTemplate.Render());
                    }
                }
                catch (IOException exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }

            foreach (var enumVal in schemaAdapter.GetEnums())
            {
                try
                {
                    // Write the C# enum
                    using (StreamWriter writer = new StreamWriter(Path.Combine(csharpEnumPath, enumVal.GetCSharpEnumFileName())))
                    {
                        StringTemplate.Template stringTemplate = TemplateManager.Instance.FindTemplate("defineCougarEnumCSharp", "");
                        stringTemplate.Add("schema", schemaAdapter);
                        stringTemplate.Add("enum", enumVal);
                        writer.Write(stringTemplate.Render());
                    }
                }
                catch (IOException exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }

            try
            {
                // Write the C# defines
                using (StreamWriter writer = new StreamWriter(Path.Combine(csharpDefinePath, "CougarDefines.cs")))
                {
                    StringTemplate.Template stringTemplate = TemplateManager.Instance.FindTemplate("defineCougarCSharpDefines", "");
                    stringTemplate.Add("schema", schemaAdapter);
                    writer.Write(stringTemplate.Render());
                }
            }
            catch (IOException exc)
            {
                Console.WriteLine(exc.Message);
            }

            return true;
        }
    }
}

