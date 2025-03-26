using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowManager
{
    internal class Program
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

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
                "1. Передвинуть окно.",
                "2. Свернуть окно.",
                "3. Закрыть.",
                "4. Переименовать."
            };

            foreach (var elem in menuElements)
            {
                Console.WriteLine(elem);
            }
            Console.Write("Введите номер пункта: ");

            int cursor;

            try
            {
                cursor = int.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"Некоректный ввод: {ex}");

                return;
            }

            switch (cursor)
            {
                case 1:

                    return;
                case 2:

                    return;
                case 3:

                    return;
                case 4:

                    return;

                default:
                    return;
            }
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