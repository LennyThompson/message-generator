using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Net.Cougar;
using Net.CougarMessage;
using Net.CougarMessage.Adapter;
using Net.TemplateManager;
using StringTemplate;

public class GenerateCougarJS
{
    private const string JS_DIRECTORY = "js";

    public static bool Generate(MessageSchemaAdapter schemaAdapter, CougarMessagesConfig generateConfig)
    {
        try
        {
            string pathDestination = Path.Combine(generateConfig.Destination.RootPath(), JS_DIRECTORY);

            if (!Directory.Exists(pathDestination))
            {
                Directory.CreateDirectory(pathDestination);
            }

            foreach (var component in schemaAdapter.GetAllComponentAdapters())
            {
                try
                {
                    string pathComponentDest = Path.Combine(pathDestination, component.ComponentName);

                    if (!Directory.Exists(pathComponentDest))
                    {
                        Directory.CreateDirectory(pathComponentDest);
                    }

                    string strFileName = $"{component.ComponentName}-messages.js";
                    using (StreamWriter writerOutput = new StreamWriter(Path.Combine(pathComponentDest, strFileName)))
                    {
                        StringWriter writer = new StringWriter();
                        AutoIndentWriter autoWriter = new AutoIndentWriter(writer);
                        ST stringTemplate = TemplateManager.Instance.FindTemplate("buildComponentMessagesDefinition", "");
                        stringTemplate.Add("component", component);
                        autoWriter.Write(stringTemplate.Render());
                        writerOutput.Write(writer.ToString());
                    }

                    foreach (var message in component.Messages)
                    {
                        try
                        {
                            string strMsgFileName = $"message-{message.PlainName}.js";
                            using (StreamWriter writerMsgOutput = new StreamWriter(Path.Combine(pathComponentDest, strMsgFileName)))
                            {
                                StringWriter writerMsg = new StringWriter();
                                AutoIndentWriter autoMsgWriter = new AutoIndentWriter(writerMsg);
                                ST stringMsgTemplate = TemplateManager.Instance.FindTemplate("selectOutMessages", "");
                                stringMsgTemplate.Add("component", component);
                                stringMsgTemplate.Add("message", message);
                                autoMsgWriter.Write(stringMsgTemplate.Render());
                                writerMsgOutput.Write(writerMsg.ToString());
                            }

                            strMsgFileName = $"compare-{message.PlainName}.js";
                            using (StreamWriter writerMsgOutput = new StreamWriter(Path.Combine(pathComponentDest, strMsgFileName)))
                            {
                                StringWriter writerMsg = new StringWriter();
                                AutoIndentWriter autoMsgWriter = new AutoIndentWriter(writerMsg);
                                ST stringMsgTemplate = TemplateManager.Instance.FindTemplate("buildCompareTrace", "");
                                stringMsgTemplate.Add("component", component);
                                stringMsgTemplate.Add("message", message);
                                autoMsgWriter.Write(stringMsgTemplate.Render());
                                writerMsgOutput.Write(writerMsg.ToString());
                            }
                        }
                        catch (IOException exc)
                        {
                            Console.WriteLine($"Error generating js for {component.ComponentName} message {message.PlainName}");
                            Console.WriteLine(exc.Message);
                        }
                    }
                }
                catch (IOException exc)
                {
                    Console.WriteLine($"Error generating js for {component.ComponentName}");
                    Console.WriteLine(exc.Message);
                }
            }
        }
        catch (IOException exc)
        {
            Console.WriteLine(exc.Message);
            return false;
        }
        return true;
    }
}

