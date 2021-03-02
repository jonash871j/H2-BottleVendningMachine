using System;
using H2_BottleVendningMachine.Lib;
using Engine;
using System.Threading;
using System.Collections.Generic;

class Program
{
    static string ProcessInfo = "";

    static void Main(string[] args)
    {
        BottleVendingMachine machine = new BottleVendingMachine();
        machine.ProcessInfo += OnProcessInfo;
        machine.PulledDrinks += OnPulledDrinks;

        ConsoleEx.Create(64, 64);
        ConsoleEx.SetFont("Terminal", 16, 16);

        while(true)
        {
            DrawTray(machine.MainTray, "Main Tray", 0);
            DrawTray(machine.BeerTray, "Beer Tray", 1);
            DrawTray(machine.SodaTray, "Soda Tray", 2);
            ConsoleEx.SetPosition(1, 5);
            ConsoleEx.WriteLine(ProcessInfo);
            ConsoleEx.WriteLine(machine.MainTray.Position.ToString());
            ConsoleEx.Update();
            ConsoleEx.Clear();
        }
    }


    private static void OnPulledDrinks(Drink[] drinks)
    {
  
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

//class Program 
//{
//    static bool isInputMode = false;

//    static void Main(string[] args)
//    {
//        BottleVendingMachine machine = new BottleVendingMachine();
//        machine.ProcessInfo += OnProcessInfo;
//        machine.PulledDrinks += OnPulledDrinks;

//        while(true)
//        {
//            if (Input.KeyPressed(Key.ESCAPE))
//            {
//                isInputMode = true;
//            }

//            if (isInputMode)
//            {
//                Console.WriteLine("Enter amount of drinks to pull");
//                int drinkAmount = int.Parse(Console.ReadLine());
//                Console.WriteLine("Enter drink to pull");
//                string drinkType = Console.ReadLine();

//                if (Enum.IsDefined(typeof(DrinkType), drinkType))
//                {
//                    machine.RequestDrinkPulling((DrinkType)Enum.Parse(typeof(DrinkType), drinkType), drinkAmount);
//                    isInputMode = false;
//                }
//            }
//        }
//    }

//    private static void OnPulledDrinks(Drink[] drinks)
//    {
//        if (!isInputMode)
//        {
//            Console.WriteLine($"You pulled {drinks.Length} drinks");
//            foreach (Drink drink in drinks)
//            {
//                Console.WriteLine($"You pulled a {drink.Type}");
//            }
//        }
//    }

//    private static void OnProcessInfo(string msg)
//    {
//        if (!isInputMode)
//        {
//            Console.WriteLine(msg);
//        }
//    }
//}