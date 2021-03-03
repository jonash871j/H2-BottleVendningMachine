using H2_BottleVendningMachine.Lib;
using Engine;

class Program
{
    static string ProcessInfo = "";
    static int ConsumedBeers = 0;
    static int ConsumedSodas = 0;

    static void Main(string[] args)
    {
        BottleVendingMachine machine = new BottleVendingMachine();
        machine.ProcessInfo += OnProcessInfo;
        machine.PulledDrink += OnPulledDrinks;
        machine.Start();

        ConsoleEx.Create(48, 12);
        ConsoleEx.SetFont("Consolas", 16, 32);

        while(true)
        {
            DrawTray(machine.MainTray, $"Main Tray: {machine.MainTray.Position}", 0);
            DrawTray(machine.BeerTray, $"Beer Tray: {machine.BeerTray.Position}", 1);
            DrawTray(machine.SodaTray, $"Soda Tray: {machine.SodaTray.Position}", 2);

            ConsoleEx.SetPosition(1, 5);
            ConsoleEx.WriteLine();
            ConsoleEx.WriteLine(ProcessInfo);
            ConsoleEx.WriteLine($"Beers consumed: {ConsumedBeers}");
            ConsoleEx.WriteLine($"Sodas consumed: {ConsumedSodas}");

            ConsoleEx.Update();
            ConsoleEx.Clear();
        }
    }

    private static void OnPulledDrinks(Drink drink)
    {
        switch (drink.Type)
        {
            case DrinkType.Beer: ConsumedBeers++; break;
            case DrinkType.Soda: ConsumedSodas++; break;
        }
    }

    private static void OnProcessInfo(string msg)
    {
        ProcessInfo = msg;
    }

    static void DrawTray(BufferTray<Drink> tray, string name, int y)
    {
        for (int i = 0; i < tray.Position; i++)
        {
            Drink drink = tray.Buffer[i];

            if (drink.Type == DrinkType.Beer)
            {
                ConsoleEx.WriteCharacter(i, y, '■', Color.Yellow);
            }
            else
            {
                ConsoleEx.WriteCharacter(i, y, '■', Color.Green);
            }
        }
        ConsoleEx.WriteCoord(15, y, name);
    }
}