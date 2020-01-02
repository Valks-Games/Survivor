using System;
using System.Collections.Generic;
using System.Text;

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
