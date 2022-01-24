using System;
using System.Collections.Generic;
using System.Drawing;
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
            var mode = "start";
            string folder = null;
            var tweaks = new bool[7];
            Size resolutionSize = new Size(0, 0);
            if (args.Length == 4)
            {
                mode = args[0];
                folder = args[1];
                var tweaksString = args[2];
                if (tweaksString.Count(c => c == '0') + tweaksString.Count(c => c == '1') == tweaksString.Length &&
                tweaksString.Length == tweaks.Length)
                {
                    for (int i = 0; i < tweaksString.Length; i++)
                        tweaks[i] = tweaksString[i] == '1';
                }
                var resolutionString = args[3];
                var resolutionStringParts = resolutionString.Split('x');
                if (resolutionStringParts.Length == 2 &&
                    int.TryParse(resolutionStringParts[0], out int resolutionWidth) &&
                    int.TryParse(resolutionStringParts[1], out int resolutionHeight))
                {
                    resolutionSize = new Size(resolutionWidth, resolutionHeight);
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(mode, folder, tweaks, resolutionSize));
        }
    }
}
