namespace H2_BottleVendningMachine.Lib
{
    public class Drink
    {
        public Drink(DrinkType type)
        {
            Type = type;
        }

        public DrinkType Type { get; private set; }
    }
}
