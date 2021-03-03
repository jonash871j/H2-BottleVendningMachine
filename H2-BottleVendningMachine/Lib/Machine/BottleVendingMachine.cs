using System;
using System.Threading;

namespace H2_BottleVendningMachine.Lib
{
    public delegate void MessageEvent(string msg);
    public delegate void PulledDrinkEvent(Drink drink);

    public class BottleVendingMachine
    {
        public BottleVendingMachine()
        {
            MainTray = new BufferTray<Drink>(MAX_TRAY_ITEMS);
            BeerTray = new BufferTray<Drink>(MAX_TRAY_ITEMS);
            SodaTray = new BufferTray<Drink>(MAX_TRAY_ITEMS);

            Thread drinkProducerThread = new Thread(DrinkProducerProcess);
            Thread consumerSplitterThread = new Thread(ConsumerSplitterProcess);
            Thread beerConsumerThread = new Thread(() => ConsumerProcess(BeerTray));
            Thread sodaConsumerThread = new Thread(() => ConsumerProcess(SodaTray));

            drinkProducerThread.Start();
            consumerSplitterThread.Start();
            beerConsumerThread.Start();
            sodaConsumerThread.Start();
        }

        private const int MAX_TRAY_ITEMS = 10;

        public BufferTray<Drink> MainTray;
        public BufferTray<Drink> BeerTray;
        public BufferTray<Drink> SodaTray;

        public MessageEvent ProcessInfo { get; set; }
        public PulledDrinkEvent PulledDrink { get; set; }

        private void DrinkProducerProcess()
        {
            Producer producer = new Producer(MainTray, ProcessInfo);

            try
            {
                producer.Work();
            }
            catch (Exception ex)
            {
                ProcessInfo?.Invoke(ex.Message);
            }
        }
        private void ConsumerSplitterProcess()
        {
            ConsumerSplitter consumerSplitter = new ConsumerSplitter(MainTray, BeerTray, SodaTray, ProcessInfo);

            try
            {
                consumerSplitter.Work();
            }
            catch (Exception ex)
            {
                ProcessInfo?.Invoke(ex.Message);
            }
        }
        private void ConsumerProcess(BufferTray<Drink> tray)
        {
            Consumer consumer = new Consumer(tray, PulledDrink);

            try
            {
                consumer.Work();
            }
            catch (Exception ex)
            {
                ProcessInfo?.Invoke(ex.Message);
            } 
        }
    }
}