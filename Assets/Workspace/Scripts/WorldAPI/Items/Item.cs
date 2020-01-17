namespace WorldAPI.Items
{
    public class Item
    {
        public string Name;

        public Item(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return "A item.";
        }
    }
}