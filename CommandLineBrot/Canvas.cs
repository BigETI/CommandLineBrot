using System.Threading.Tasks;

namespace FastConsoleUI
{
    public class Canvas : AConsoleUIControl
    {
        private BufferCell[,] buffer;

        public BufferCell[,] Buffer
        {
            get
            {
                if (buffer == null)
                {
                    buffer = new BufferCell[Size.X, Size.Y];
                }
                else if ((buffer.GetLength(0) != Size.X) || (buffer.GetLength(1) != Size.Y))
                {
                    InitBuffer();
                }
                return buffer;
            }
        }

        public Canvas(IConsoleUI parent) : base(parent)
        {
            // ...
        }

        private void InitBuffer()
        {
            BufferCell cell = new BufferCell(BufferCell.empty.Character, ForegroundColor, BackgroundColor);
            buffer = new BufferCell[Size.X, Size.Y];
            Parallel.For(0, Size.Y, (y) =>
            {
                for (int x = 0; x < Size.X; x++)
                {
                    buffer[x, y] = cell;
                }
            });
        }

        public override void WriteToBuffer(BufferCell[,] buffer, RectInt rectangle)
        {
            Vector2Int buffer_size = new Vector2Int(buffer.GetLength(0), buffer.GetLength(1));
            Vector2Int start_index = Vector2Int.Max(-rectangle.Position, Vector2Int.zero);
            Vector2Int clamped_size = rectangle.Size - Vector2Int.Max(rectangle.Position + rectangle.Size - buffer_size, Vector2Int.zero);
            Parallel.For(start_index.Y, clamped_size.Y, (y) =>
            {
                for (int x = start_index.X; x < clamped_size.X; x++)
                {
                    buffer[rectangle.Position.X + x, rectangle.Position.Y + y] = this.buffer[x, y];
                }
            });
        }
    }
}
