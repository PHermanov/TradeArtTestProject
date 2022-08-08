using System.Collections.Concurrent;

namespace TradeArtTestProject.Communication
{
    public interface IMessageResultsStorage
    {
        ConcurrentBag<bool> Results { get; set; }
        public int Count => Results.Count;
        public void Add(bool value) => Results.Add(value);
    }

    public class MessageResultsStorage : IMessageResultsStorage
    {
        public ConcurrentBag<bool> Results { get; set; } = new();
    }
}
