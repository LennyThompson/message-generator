using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Cougar;
using Cougar.Utils;
using CougarMessage.Adapter;
using TemplateManager;
using StringTemplate4;

namespace CougarMessage.Generator
{
    public class GenerateCougarDart
    {
        public static bool Generate(MessageSchemaAdapter schemaAdapter, CougarGenerateConfig generateConfig)
        {
            string dartPath = Path.Combine(generateConfig.MessagesConfig.Destination.Root, "dart");

            if (!Directory.Exists(dartPath))
            {
                Directory.CreateDirectory(dartPath);
            }

            schemaAdapter.SetPackageName(generateConfig.MessagesConfig.PackageName);

            foreach (var enumeration in schemaAdapter.GetEnums())
            {
                try
                {
                    string filePath = Path.Combine(dartPath, $"{SnakeCaser.GetSnakeCase(enumeration.ShortName)}.enum.dart");
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        StringWriter stringWriter = new StringWriter();
                        AutoIndentWriter autoWriter = new AutoIndentWriter(stringWriter);
                        ST stringTemplate = TemplateManager.Instance.FindTemplate("generateCougarEnumDart", "");
                        stringTemplate.Add("schema", schemaAdapter);
                        stringTemplate.Add("enum", enumeration);
                        autoWriter.Write(stringTemplate.Render());
                        writer.Write(stringWriter.ToString());
                    }
                }
                catch (IOException exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }

            foreach (var message in schemaAdapter.GetMessages().Concat(schemaAdapter.GetNonMessages()))
            {
                try
                {
                    string filePath = Path.Combine(dartPath, message.DartClassFileName);
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        StringWriter stringWriter = new StringWriter();
                        AutoIndentWriter autoWriter = new AutoIndentWriter(stringWriter);
                        ST stringTemplate = TemplateManager.Instance.FindTemplate("generateCougarMessagesDart", "");
                        stringTemplate.Add("schema", schemaAdapter);
                        stringTemplate.Add("message", message);
                        autoWriter.Write(stringTemplate.Render());
                        writer.Write(stringWriter.ToString());
                    }
                }
                catch (IOException exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }

            return true;
        }
    }
}

