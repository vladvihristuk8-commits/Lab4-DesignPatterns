using System;
using System.IO;
using Lab4.Patterns;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // ---------- Тест 1. Singleton ----------
        Console.WriteLine("=== Тест 1. Singleton - Менеджер логування ===");

        Logger logger1 = Logger.Instance;
        Logger logger2 = Logger.Instance;

        logger1.Info("Застосунок запущено");
        logger2.Warning(
            "Файл конфігурації не знайдено, використано типові налаштування");
        logger1.Error("Не вдалося підключитися до сервера оновлень");

        Console.WriteLine($"logger1 та logger2 - це один і той самий об'єкт: " +
                          $"{ReferenceEquals(logger1, logger2)}");

        // ---------- Тест 2. Adapter ----------
        Console.WriteLine("\n=== Тест 2. Adapter - Конвертер текстових форматів ===");

        ITextFormat adapter = new TextFormatAdapter();

        adapter.Write("note.txt", "Текст, збережений у форматі TXT");
        adapter.Write("note.json", "Текст, збережений у форматі JSON");
        adapter.Write("note.xml", "Текст, збережений у форматі XML");

        Console.WriteLine("TXT  -> " + adapter.Read("note.txt"));
        Console.WriteLine("JSON -> " + adapter.Read("note.json"));
        Console.WriteLine("XML  -> " + adapter.Read("note.xml"));

        ((TextFormatAdapter)adapter).Convert("note.json", "converted.xml");
        Console.WriteLine("Вміст converted.xml: " + adapter.Read("converted.xml"));

        try
        {
            adapter.Read("report.docx");
        }
        catch (NotSupportedException ex)
        {
            Logger.Instance.Error(ex.Message);
        }

        // ---------- Тест 3. Observer ----------
        Console.WriteLine("\n=== Тест 3. Observer - Сповіщення в чаті ===");

        ChatRoom devChat = new ChatRoom("Розробка");
        ChatRoom newsChat = new ChatRoom("Новини");

        ChatUser ivan = new ChatUser("Іван");
        ChatUser maria = new ChatUser("Марія");
        ChatUser oleh = new ChatUser("Олег");

        devChat.Subscribe(ivan);
        devChat.Subscribe(maria);
        devChat.Subscribe(oleh);

        newsChat.Subscribe(maria);

        devChat.SendMessage("Іван", "Завантажив нову версію проєкту на GitHub");
        newsChat.SendMessage("Редакція", "Опубліковано розклад сесії");

        devChat.Unsubscribe(oleh);
        devChat.SendMessage("Марія", "Перевірте, будь ласка, pull request");

        Console.WriteLine("\nУсі події збережено у файлі " +
                          Path.Combine(Directory.GetCurrentDirectory(), "app.log"));
        Console.ReadKey();
    }
}
