using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace test_model;

public class JsonInputData<InputType> : IEnumerable<InputType>
{
    protected virtual string SourcePath => "";
    protected virtual string SourceFile => "";

    private string FilePath
    {
        get 
        { 
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (directoryName == null)
            {
                throw new Exception("Couldn't get assembly directory");
            }
            
            return Path.Combine(directoryName, SourcePath, $"{SourceFile}.json");
        }
    }

    public IEnumerator<InputType> GetEnumerator()
    {
        using (TextReader reader = File.OpenText(FilePath))
        {
            List<InputType>? listInputs = JsonSerializer.Deserialize<List<InputType>>(reader.ReadToEnd());

            if (listInputs != null)
            {
                foreach (var input in listInputs)
                {
                    yield return input;
                }
            }
        }
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}