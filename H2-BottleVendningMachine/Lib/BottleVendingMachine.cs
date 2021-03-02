using System;
using System.Collections.Generic;
using System.Threading;

namespace H2_BottleVendningMachine.Lib
{
    public delegate void MessageEvent(string msg);
    public delegate void PulledDrinksEvent(Drink[] drinks);

    public class BottleVendingMachine
    {
        public BottleVendingMachine()
        {
            MainTray = new BufferTray<Drink>(MAX_TRAY_ITEMS);
            BeerTray = new BufferTray<Drink>(MAX_TRAY_ITEMS);
            SodaTray = new BufferTray<Drink>(MAX_TRAY_ITEMS);

            Thread drinkProducerThread = new Thread(DrinkProducerProcess);
            Thread consumerSplitterThread = new Thread(ConsumerSplitterProcess);
            drinkProducerThread.Start();
            consumerSplitterThread.Start();
        }

        private const int MAX_TRAY_ITEMS = 10;
        private static Random rng = new Random();

        //private Queue<Drink> MainTray;
        //private Queue<Drink> BeerTray;
        //private Queue<Drink> SodaTray;


        public BufferTray<Drink> MainTray;
        public BufferTray<Drink> BeerTray;
        public BufferTray<Drink> SodaTray;


        public MessageEvent ProcessInfo { get; set; }
        public PulledDrinksEvent PulledDrinks { get; set; }

        public void RequestDrinkPulling(DrinkType type, int amount)
        {
            Thread drinkPullingThread = new Thread(() => DrinkPullingProcess(type, amount));
            drinkPullingThread.Start();
        }

        private void DrinkProducerProcess()
        {
            while(true)
            {
                if (MainTray.Position < MAX_TRAY_ITEMS)
                {
                    if (Monitor.TryEnter(MainTray))
                    {
                        for (int i = MainTray.Position; i < MAX_TRAY_ITEMS; i++)
                        {
                            DrinkType drinkType = (DrinkType)(rng.Next(0, 2));
                            MainTray.Push(new Drink(drinkType));
                            ProcessInfo.Invoke($"Drink producer has produced a {drinkType}");
                            //Thread.Sleep(rng.Next(200, 1000));
                        }
                        Monitor.Pulse(MainTray);
                        Monitor.Exit(MainTray);
                    }
                }
            }
        }
        private void ConsumerSplitterProcess()
        {
            while(true)
            {
                //while (MainTray.Position >= MAX_TRAY_ITEMS)
                //{
                //    ProcessInfo.Invoke("Main tray is empty");
                //    Thread.Sleep(1000);
                //    //Monitor.Wait(MainTray); // MainTray is empty
                //}

                if (Monitor.TryEnter(MainTray))
                {
                    if (MainTray.Position > 0)
                    {
                        Drink drink = MainTray.Pull();
                        BufferTray<Drink> tray = GetTray(drink.Type);

                        if (tray.Position < MAX_TRAY_ITEMS)
                        {
                            if (Monitor.TryEnter(tray))
                            {
                                tray.Push(drink);
                                ProcessInfo.Invoke($"{drink.Type} was splitted onto {drink.Type} tray");
                                Monitor.Pulse(tray);
                                Monitor.Exit(tray);
                                Thread.Sleep(rng.Next(100, 250));
                            }
                            else
                            {
                                Monitor.Wait(tray);
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
        private void DrinkPullingProcess(DrinkType type, int amount)
        {
            BufferTray<Drink> tray = GetTray(type);
            Queue<Drink> drinks = new Queue<Drink>();

            while(true)
            {
                if (Monitor.TryEnter(tray))
                {
                    for (int i = drinks.Count; i < amount; )
                    {
                        if (tray.Position == 0)
                        {
                            break;
                        }

                        drinks.Enqueue(tray.Pull());
                        Thread.Sleep(250);
                    }

                    if (drinks.Count == amount)
                    {
                        PulledDrinks.Invoke(drinks.ToArray());
                        Monitor.Pulse(tray);
                        Monitor.Exit(tray);
                        return;
                    }

                    Monitor.Pulse(tray);
                    Monitor.Exit(tray);
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