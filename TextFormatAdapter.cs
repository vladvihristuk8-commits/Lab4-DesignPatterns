using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml.Linq;

namespace Lab4.Patterns
{
  
    public interface ITextFormat
    {
        string Read(string path);
        void Write(string path, string content);
    }

    public class TxtFileHandler
    {
        public string LoadText(string path) => File.ReadAllText(path);

        public void SaveText(string path, string text) =>
            File.WriteAllText(path, text);
    }

    public class JsonService
    {
        public string DeserializeFromFile(string path)
        {
            using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));
            return document.RootElement.GetProperty("content").GetString();
        }

        public void SerializeToFile(string path, string value)
        {
            var data = new Dictionary<string, string> { ["content"] = value };
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            File.WriteAllText(path, JsonSerializer.Serialize(data, options));
        }
    }

    public class XmlService
    {
        public string ExtractNodeValue(string path)
        {
            XDocument document = XDocument.Load(path);
            return document.Root.Element("content").Value;
        }

        public void BuildDocument(string path, string value)
        {
            new XDocument(new XElement("document",
                new XElement("content", value))).Save(path);
        }
    }

    public class TextFormatAdapter : ITextFormat
    {
        private readonly TxtFileHandler _txtHandler = new TxtFileHandler();
        private readonly JsonService _jsonService = new JsonService();
        private readonly XmlService _xmlService = new XmlService();

        public string Read(string path)
        {
            string extension = Path.GetExtension(path).ToLowerInvariant();
            switch (extension)
            {
                case ".txt":
                    return _txtHandler.LoadText(path);
                case ".json":
                    return _jsonService.DeserializeFromFile(path);
                case ".xml":
                    return _xmlService.ExtractNodeValue(path);
                default:
                    throw new NotSupportedException(
                        $"Формат {extension} не підтримується адаптером");
            }
        }

        public void Write(string path, string content)
        {
            string extension = Path.GetExtension(path).ToLowerInvariant();
            switch (extension)
            {
                case ".txt":
                    _txtHandler.SaveText(path, content);
                    break;
                case ".json":
                    _jsonService.SerializeToFile(path, content);
                    break;
                case ".xml":
                    _xmlService.BuildDocument(path, content);
                    break;
                default:
                    throw new NotSupportedException(
                        $"Формат {extension} не підтримується адаптером");
            }
            Logger.Instance.Info($"Створено файл {Path.GetFileName(path)}");
        }

        public void Convert(string sourcePath, string targetPath)
        {
            string content = Read(sourcePath);
            Write(targetPath, content);
            Logger.Instance.Info(
                $"Конвертація {Path.GetFileName(sourcePath)} -> " +
                $"{Path.GetFileName(targetPath)} виконана успішно");
        }
    }
}
