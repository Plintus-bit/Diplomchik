using Interfaces;
using Managers;

namespace Randomizable
{
    public class MessageType : IChance
    {
        public static MessageLoader messageLoader;
        
        public int chance;

        public string textId;
        public int Chance => chance;

        public MessageType() {}

        public MessageType(string textId, int chance)
        {
            Init(textId, chance);
        }
        
        public void Init(string textId, int chance)
        {
            this.textId = textId;
            this.chance = chance;
        }

        public string GetText()
        {
            messageLoader ??= MessageLoader.Instance;
            return messageLoader.GetDataById(textId);
        }
    }
}