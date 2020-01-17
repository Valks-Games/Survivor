using WorldAPI.Items;

namespace WorldAPI.Resources
{
    public class Resource : Item
    {
        public Resource(string name) : base(name)
        {
        }

        public override string ToString()
        {
            return "A resource.";
        }
    }
}