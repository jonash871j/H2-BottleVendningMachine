using System;
using System.Threading;

namespace H2_BottleVendningMachine.Lib
{
    public class Producer
    {
        public Producer(BufferTray<Drink> mainTray, MessageEvent processInfo)
        {
            MainTray = mainTray;
            ProcessInfo = processInfo;
        }

        private static Random rng = new Random();

        public BufferTray<Drink> MainTray { get; private set; }
        public MessageEvent ProcessInfo { get; private set; }

        public void Work()
        {
            while (true)
            {
                if (MainTray.Position < MainTray.Length)
                {
                    if (Monitor.TryEnter(MainTray))
                    {
                        for (int i = MainTray.Position; i < MainTray.Length; i++)
                        {
                            DrinkType drinkType = (DrinkType)(rng.Next(0, 2));
                            MainTray.PushToFirst(new Drink(drinkType));
                            ProcessInfo?.Invoke($"Drink producer has produced a {drinkType}");
                            Thread.Sleep(rng.Next(200, 500));
                        }
                        Monitor.Pulse(MainTray);
                        Monitor.Exit(MainTray);
                    }
                }
            }
        }
    }
}
