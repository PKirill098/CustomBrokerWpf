
namespace KirillPolyanskiy.CustomBrokerWpf.Classes
{ //касса банковский счет
    class Coffer
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public int AccountId { set; get; }
        public bool isActual { set; get; }
    }
    public struct AgentName
    {
        public int Id { set; get; }
        public string Name { set; get; }
    }
    internal class DBEntity
    {
        internal bool Dirty{set;get;}
    }
}
