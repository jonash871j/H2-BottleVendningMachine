using System;
using System.Threading;

namespace H2_BottleVendningMachine.Lib
{
    public class Consumer
    {
        public Consumer(BufferTray<Drink> tray, PulledDrinkEvent pulledDrink)
        {
            if (tray.Length == 0)
            {
                Console.Beep(1000, 1000);
            }

            Tray = tray;
            PulledDrink = pulledDrink;
        }

        private static Random rng = new Random();

        public BufferTray<Drink> Tray { get; private set; }
        public PulledDrinkEvent PulledDrink { get; set; }

        public void Work()
        {
            while (true)
            {
                if (Monitor.TryEnter(Tray))
                {
                    int amount = rng.Next(1, 4);

                    for (int i = 0; i < amount; i++)
                    {
                        if (Tray.Position == 0)
                        {
                            break;
                        }

                        Drink drink = Tray.Pull();
                        Thread.Sleep(rng.Next(100, 250));
                        PulledDrink?.Invoke(drink);
                    }
                    Monitor.Pulse(Tray);
                    Monitor.Exit(Tray);
                }
                Thread.Sleep(3000);
            }
        }
    }
}
