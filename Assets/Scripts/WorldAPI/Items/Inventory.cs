using System.Collections.Generic;

namespace WorldAPI.Items
{
    public class Inventory
    {
        public Dictionary<Item, int> Items = new Dictionary<Item, int>();

        public int MaxSize;

        public Inventory(int maxSize)
        {
            MaxSize = maxSize;
        }
    }
}