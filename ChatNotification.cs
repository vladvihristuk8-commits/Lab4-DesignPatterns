using System;
using System.Collections.Generic;

namespace Lab4.Patterns
{

    public interface IChatObserver
    {
        string Name { get; }
        void Update(string chatName, string author, string message);
    }

    public class ChatUser : IChatObserver
    {
        public string Name { get; }

        public ChatUser(string name) => Name = name;

        public void Update(string chatName, string author, string message)
        {
            Console.WriteLine(
                $"   -> {Name}: у чаті \"{chatName}\" {author} написав: {message}");
        }
    }
    public class ChatRoom
    {
        private readonly List<IChatObserver> _subscribers =
            new List<IChatObserver>();

        public string Name { get; }

        public ChatRoom(string name) => Name = name;

        public void Subscribe(IChatObserver observer)
        {
            if (!_subscribers.Contains(observer))
            {
                _subscribers.Add(observer);
                Logger.Instance.Info(
                    $"{observer.Name} підписався на чат \"{Name}\"");
            }
        }

        public void Unsubscribe(IChatObserver observer)
        {
            if (_subscribers.Remove(observer))
            {
                Logger.Instance.Info(
                    $"{observer.Name} відписався від чату \"{Name}\"");
            }
        }

        public void SendMessage(string author, string message)
        {
            Logger.Instance.Info($"Чат \"{Name}\": нове повідомлення від {author}");
            Notify(author, message);
        }

        private void Notify(string author, string message)
        {
            foreach (IChatObserver subscriber in _subscribers)
            {
      
                if (subscriber.Name != author)
                {
                    subscriber.Update(Name, author, message);
                }
            }
        }
    }
}
