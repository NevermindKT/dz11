using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowManager
{
    internal class Program
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SetWindowText(IntPtr hWnd, string lpString);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr GetShellWindow();

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        static List<string> windowNameList = new List<string>();

        const int SW_MINIMIZE = 6;
        const int WM_CLOSE = 0x0010;

        static void Main(string[] args)
        {
            showWindowList();

            windowChoose();
        }

        public static void windowChoose()
        {
            Console.Write("Введите номер окна: ");

            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= windowNameList.Count)
            {
                Console.Clear();
                Console.WriteLine($"Вы выбрали окно: {windowNameList[index - 1]}");

                mainMenu(windowNameList[index - 1]);
            }
            else
            {
                Console.WriteLine("Некорректный ввод.");
            }
        }

        public static void mainMenu(string currentWindow)
        {
            List<string> menuElements = new List<string>
            {
                "1. Переместить окно.",
                "2. Свернуть окно.",
                "3. Закрыть.",
                "4. Переименовать."
            };

            foreach (var elem in menuElements)
            {
                Console.WriteLine(elem);
            }
            Console.Write("Введите номер пункта: ");

            string input = Console.ReadLine();

            if (!int.TryParse(input, out int cursor))
            {
                Console.WriteLine("Неккоректный ввод.");
                return;
            }

            Console.Clear();

            switch (cursor)
            {
                case 1:
                    coordinatesInput(currentWindow);
                    return;
                case 2:
                    MinimizeWindow(FindWindow(null, currentWindow));
                    return;
                case 3:
                    CloseWindow(FindWindow(null, currentWindow));
                    return;
                case 4:
                    newNameInput(currentWindow);
                    return;

                default:
                    return;
            }
        }

        static void newNameInput(string currentWindow)
        {
            Console.Write("Введите новое имя: ");
            string input = Console.ReadLine();

            RenameWindow(FindWindow(null, currentWindow), input);
        }

        static void coordinatesInput(string currentWindow)
        {
            Console.WriteLine("Введите координаты.");
            Console.WriteLine("X: ");

            string input = Console.ReadLine();
            if (!int.TryParse(input, out int x))
            {
                Console.WriteLine("Неккоректный ввод.");
                return;
            }

            Console.Clear();

            Console.WriteLine("Введите координаты.");
            Console.WriteLine("Y: ");
            input = Console.ReadLine();
            if (!int.TryParse(input, out int y))
            {
                Console.WriteLine("Неккоректный ввод.");
                return;
            }

            MoveWindow(FindWindow(null, currentWindow), x, y, 500, 500);
        }

        static void MoveWindow(IntPtr hWnd, int x, int y, int width, int height)
        {
            MoveWindow(hWnd, x, y, width, height, true);
            Console.WriteLine("Окно перемещено.");
        }

        static void MinimizeWindow(IntPtr hWnd)
        {
            ShowWindow(hWnd, SW_MINIMIZE);
            Console.WriteLine("Окно свернуто.");
        }

        static void CloseWindow(IntPtr hWnd)
        {
            SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            Console.WriteLine("Окно закрыто.");
        }

        static void RenameWindow(IntPtr hWnd, string newName)
        {
            SetWindowText(hWnd, newName);
            Console.WriteLine("Окно переименовано.");
        }

        public static void showWindowList()
        {
            EnumWindows(EnumWindowCallBack, IntPtr.Zero);

            for (int i = 0; i < windowNameList.Count; i++)
            {
                Console.WriteLine($"{i + 1} {windowNameList[i]}");
            }
        }

        public static bool EnumWindowCallBack(IntPtr hWnd, IntPtr lParam)
        {
            if (!IsWindowVisible(hWnd) || hWnd == GetShellWindow())
                return true;

            int lenth = GetWindowTextLength(hWnd);
            if(lenth > 0)
            {
                StringBuilder windowTitle = new StringBuilder(lenth + 1);
                GetWindowText(hWnd, windowTitle, windowTitle.Capacity);
                windowNameList.Add(windowTitle.ToString());
            }
            return true;
        }
    }
}