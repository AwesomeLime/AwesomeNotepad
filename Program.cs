using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace AwesomeNotepad
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 0)
            {
                string filePath = args[0];
                if (File.Exists(filePath))
                {
                   Properties.Settings.Default.openedFilePath = filePath;
                    Application.Run(new Form1());
                }
                else
                {
                    MessageBox.Show($"Файл не найден: {filePath}");
                }
            }
            else
            {
                Application.Run(new Form1());

            }
        }
    }
}
