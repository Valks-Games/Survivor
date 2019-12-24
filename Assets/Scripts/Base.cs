public class Base : Structure
{
    public int Wood { get; private set; } = 0;
    public int Stone { get; private set; } = 0;

    public void DepositResource(string type, int amount) {
        switch (type) {
            case "Wood":
                Wood += amount;
                break;
            case "Stone":
                Stone += amount;
                break;
        }
    }
}
