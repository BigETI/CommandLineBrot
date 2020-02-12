using FastConsoleUI;
using System;
using System.Threading.Tasks;

namespace CommandLineBrot
{
    class Program
    {
        private static Canvas canvas;

        private static bool keepRunning = true;

        private static int zPower = 1;

        private static int cPower = 1;

        private static double zoom = 1.0;

        private static double xOffset = -2.0;

        private static double yOffset = -1.0;

        private static readonly ConsoleColor[] colors = new ConsoleColor[]
        {
            ConsoleColor.DarkGray,
            ConsoleColor.Gray,
            ConsoleColor.DarkBlue,
            ConsoleColor.Blue
        };

        private static readonly ConsoleColor[] consoleColors = Enum.GetValues(typeof(ConsoleColor)) as ConsoleColor[];

        private static readonly int numSteps = 1024;

        static void Main(string[] args)
        {
            Window window = new Window();
            canvas = window.AddControl<Canvas>(Vector2Int.zero, Vector2Int.zero);
            window.OnKeyPressed += KeyPressedEvent;
            window.OnWindowResized += WindowResizedEvent;
            Console.CursorVisible = false;
            while (keepRunning)
            {
                window.Refresh();
                BuildBuffer();
            }
            Console.CursorVisible = true;
        }

        private static void KeyPressedEvent(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.OemPlus:
                    zoom *= 2.0;
                    BuildBuffer();
                    break;
                case ConsoleKey.OemMinus:
                    zoom *= 0.5;
                    BuildBuffer();
                    break;
                case ConsoleKey.LeftArrow:
                    xOffset -= zoom * 0.125;
                    BuildBuffer();
                    break;
                case ConsoleKey.RightArrow:
                    xOffset += zoom * 0.125;
                    BuildBuffer();
                    break;
                case ConsoleKey.UpArrow:
                    yOffset -= zoom * 0.125;
                    BuildBuffer();
                    break;
                case ConsoleKey.DownArrow:
                    yOffset += zoom * 0.125;
                    BuildBuffer();
                    break;
                case ConsoleKey.Escape:
                    keepRunning = false;
                    break;
                case ConsoleKey.Q:
                    if (cPower > 1)
                    {
                        --cPower;
                        BuildBuffer();
                    }
                    break;
                case ConsoleKey.W:
                    ++cPower;
                    BuildBuffer();
                    break;
                case ConsoleKey.A:
                    if (zPower > 1)
                    {
                        --zPower;
                        BuildBuffer();
                    }
                    break;
                case ConsoleKey.S:
                    ++zPower;
                    BuildBuffer();
                    break;
            }
        }

        private static void WindowResizedEvent(Vector2Int size)
        {
            canvas.Size = size;
            BuildBuffer();
        }

        private static void BuildBuffer()
        {
            BufferCell cell = new BufferCell(BufferCell.empty.Character, BufferCell.defaultForegroundColor, ConsoleColor.Black);
            BufferCell[,] buffer = canvas.Buffer;
            Vector2Int size = new Vector2Int(buffer.GetLength(0), buffer.GetLength(1));
            Parallel.For(0, size.Y, (y) =>
            {
                for (int x = 0, i, j; x < size.X; x++)
                {
                    Complex z = new Complex(0.0, 0.0);
                    Complex c = new Complex(((x * (3.0 * zoom)) / size.X) + (xOffset * zoom), ((y * (2.0 * zoom)) / size.Y) + (yOffset * zoom));
                    Complex cn = new Complex(1.0, 0.0);
                    for (i = 0; i < cPower; i++)
                    {
                        cn *= c;
                    }
                    for (i = 0; i < numSteps; i++)
                    {
                        Complex zn = new Complex(1.0, 0.0);
                        for (j = 0; j < zPower; j++)
                        {
                            zn *= z;
                        }
                        z = zn + cn;
                        if (Math.Abs(z.SquaredMagnitude) > 4.0)
                        {
                            break;
                        }
                    }
                    buffer[x, y] = ((i < numSteps) ? new BufferCell(BufferCell.empty.Character, BufferCell.defaultForegroundColor, colors[i % colors.Length]) : cell);
                }
            });
        }
    }
}
