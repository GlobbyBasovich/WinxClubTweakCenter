using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinxClubTweakCenter
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string folder = null;
            var tweaks = new bool[7];
            var resolutionIndex = -2;

            if (args.Length == 3 &&
                args[1].Count(c => c == '0') + args[1].Count(c => c == '1') == args[1].Length &&
                args[1].Length == tweaks.Length &&
                int.TryParse(args[2], out resolutionIndex))
            {
                folder = args[0];
                for (int i = 0; i < args[1].Length; i++)
                    tweaks[i] = args[1][i] == '1';
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(folder, tweaks, resolutionIndex));
        }
    }
}
