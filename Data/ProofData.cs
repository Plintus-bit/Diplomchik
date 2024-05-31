namespace Data
{
    public class ProofData
    {
        public string id;
        public string title;
        public string description;
        public Author author;
        public int maxAmount;

        public ProofData(string id, string title, string description,
            Author author, int maxAmount)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.author = author;
            this.maxAmount = maxAmount;
        }
    }
}