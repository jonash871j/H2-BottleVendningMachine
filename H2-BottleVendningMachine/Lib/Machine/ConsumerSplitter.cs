using System;
using System.Threading;

namespace H2_BottleVendningMachine.Lib
{
    public class ConsumerSplitter
    {
        public ConsumerSplitter(BufferTray<Drink> mainTray, BufferTray<Drink> beerTray, BufferTray<Drink> sodaTray, MessageEvent processInfo)
        {
            MainTray = mainTray;
            BeerTray = beerTray;
            SodaTray = sodaTray;
            ProcessInfo = processInfo;
        }

        private static Random rng = new Random();

        public BufferTray<Drink> MainTray { get; private set; }
        public BufferTray<Drink> BeerTray { get; private set; }
        public BufferTray<Drink> SodaTray { get; private set; }
        public MessageEvent ProcessInfo { get; private set; }

        public void Work()
        {
            while (true)
            {
                if (Monitor.TryEnter(MainTray))
                {
                    if (MainTray.Position > 0)
                    {
                        Drink drink = MainTray.Pull();
                        BufferTray<Drink> tray = GetTray(drink.Type);

                        if (tray.Position < tray.Length)
                        {
                            if (Monitor.TryEnter(tray))
                            {
                                tray.Push(drink);

                                ProcessInfo?.Invoke($"{drink.Type} was splitted onto {drink.Type} tray");

                                Monitor.Pulse(tray);
                                Monitor.Exit(tray);

                                Thread.Sleep(rng.Next(50, 250));
                            }
                        }
                        else
                        {
                            MainTray.Push(drink);
                        }
                    }

                    Monitor.Pulse(MainTray);
                    Monitor.Exit(MainTray);
                }
            }
        }

        private BufferTray<Drink> GetTray(DrinkType type)
        {
            if (type == DrinkType.Beer)
            {
                return BeerTray;
            }
            else
            {
                return SodaTray;
            }
        }
    }
}
