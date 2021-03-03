namespace H2_BottleVendningMachine.Lib
{
    public class BufferTray<T>
    {
        public BufferTray(int length)
        {
            Length = length;
            Buffer = new T[length];
        }

        public T this[int i]
        {
            get => Buffer[i];
            set => Buffer[i] = value;
        }

        public int Length { get; private set; }
        public T[] Buffer { get; set; }
        public int Position { get; set; } = 0;

        public void Push(T type)
        {
            Buffer[Position] = type;
            Position++;
        }
        public void PushToFirst(T type)
        {
            for (int i = Position; i > 0; i--)
            {
                Buffer[i] = Buffer[i - 1];
            }

            Buffer[0] = type;
            Position++;


        }
        public T Pull()
        {
            Position--;
            return Buffer[Position];
        }
    }
}
