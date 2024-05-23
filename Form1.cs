using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Text;
using System.ComponentModel.Design;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using AwesomeNotepad.Properties;

namespace AwesomeNotepad
{
    public partial class Form1 : Form
    {
        private string originalText;
        private string openedFileName;
        private string savedFileName;
        private int symbolCount;
        private int lineCount;
        private int columnCount;
        private int currentSearchIndex = 0;
        private int start = 0;
        private string encoding = "UTF-8";

        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            richTextBox1.AllowDrop = true;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.isNewFile == false)
            {
                if (Properties.Settings.Default.isAutoSave == false)
                {
                    string fileName = Path.GetFileName(openedFileName);
                    this.Text = $"Awesome Notepad - {fileName}*";
                    Properties.Settings.Default.isSaved = false;
                }
                else if (Properties.Settings.Default.isAutoSave == true)
                {

                }

            }

        }

        // Сохранить как...
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Lang == 1)
            {
                SaveFileDialog save = new SaveFileDialog();

                // Установка фильтра для диалогового окна
                save.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";
                if (Properties.Settings.Default.isOpened == true)
                {
                    save.FileName = $"{openedFileName}";
                }
                else
                {
                    save.FileName = "Безымянный";
                }


                if (save.ShowDialog() == DialogResult.OK)
                {
                    // Сохранение текста из TextBox в выбранный файл
                    File.WriteAllText(save.FileName, richTextBox1.Text);
                    savedFileName = save.FileName;
                    string sFileName = Path.GetFileName(savedFileName);
                    this.Text = $"Awesome Notepad - {sFileName}";
                    openedFileName = sFileName;
                    Properties.Settings.Default.isSaved = true;
                    Properties.Settings.Default.isOpened = true;
                    Properties.Settings.Default.isNewFile = false;
                    encoding = "UTF-8";
                }
            }
            else
            {
                SaveFileDialog save = new SaveFileDialog();

                // Setting the filter for the dialog
                save.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";
                if (Properties.Settings.Default.isOpened == true)
                {
                    save.FileName = $"{openedFileName}";
                }
                else
                {
                    save.FileName = "Untitled";
                }


                if (save.ShowDialog() == DialogResult.OK)
                {
                    // Save the text from the TextBox to the selected file
                    File.WriteAllText(save.FileName, richTextBox1.Text);
                    savedFileName = save.FileName;
                    string sFileName = Path.GetFileName(savedFileName);
                    this.Text = $"Awesome Notepad - {sFileName}";
                    openedFileName = sFileName;
                    Properties.Settings.Default.isSaved = true;
                    Properties.Settings.Default.isOpened = true;
                    Properties.Settings.Default.isNewFile = false;
                    encoding = "UTF-8";
                }

            }

        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // Открыть
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Lang == 1)
            {
                if (Properties.Settings.Default.isSaved == false)
                {
                    DialogResult open = MessageBox.Show("Текущий файл не сохранён. Вы хотите его сохранить?", "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == true)
                    {
                        SaveFileDialog saveD = new SaveFileDialog();

                        // Установка фильтра для диалогового окна
                        saveD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";
                        saveD.FileName = "Безымянный";

                        if (saveD.ShowDialog() == DialogResult.OK)
                        {
                            // Сохранение текста из TextBox в выбранный файл
                            File.WriteAllText(saveD.FileName, richTextBox1.Text);
                            savedFileName = saveD.FileName;
                            string sFileName = Path.GetFileName(savedFileName);
                            this.Text = $"Awesome Notepad - {sFileName}";
                            openedFileName = sFileName;
                            Properties.Settings.Default.isSaved = true;
                            encoding = "UTF-8";
                            OpenFileDialog openD = new OpenFileDialog();

                            // Установка фильтра для диалогового окна
                            openD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";

                            if (openD.ShowDialog() == DialogResult.OK)
                            {
                                // Чтение текста из выбранного файла и установка его в TextBox
                                richTextBox1.Text = File.ReadAllText(openD.FileName);
                                openedFileName = openD.FileName;
                                string fileName = Path.GetFileName(openedFileName);
                                this.Text = $"Awesome Notepad - {fileName}";
                                Properties.Settings.Default.isOpened = true;
                                Properties.Settings.Default.isSaved = true;
                                Properties.Settings.Default.isNewFile = false;
                                encoding = "UTF-8";
                            }
                        }
                        else
                        {

                        }
                    }
                    else if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == false)
                    {
                        // Сохранение текста из TextBox в выбранный файл
                        File.WriteAllText(openedFileName, richTextBox1.Text);
                        string oFileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {oFileName}";
                        Properties.Settings.Default.isSaved = true;
                        encoding = "UTF-8";
                        OpenFileDialog openD = new OpenFileDialog();

                        // Установка фильтра для диалогового окна
                        openD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";

                        if (openD.ShowDialog() == DialogResult.OK)
                        {
                            // Чтение текста из выбранного файла и установка его в TextBox
                            richTextBox1.Text = File.ReadAllText(openD.FileName);
                            openedFileName = openD.FileName;
                            string fileName = Path.GetFileName(openedFileName);
                            this.Text = $"Awesome Notepad - {fileName}";
                            Properties.Settings.Default.isOpened = true;
                            Properties.Settings.Default.isSaved = true;
                            Properties.Settings.Default.isNewFile = false;
                            encoding = "UTF-8";
                        }
                        else
                        {

                        }
                    }
                    else if (open == DialogResult.No)
                    {
                        OpenFileDialog openD = new OpenFileDialog();

                        // Установка фильтра для диалогового окна
                        openD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";

                        if (openD.ShowDialog() == DialogResult.OK)
                        {
                            // Чтение текста из выбранного файла и установка его в TextBox
                            richTextBox1.Text = File.ReadAllText(openD.FileName);
                            openedFileName = openD.FileName;
                            string fileName = Path.GetFileName(openedFileName);
                            this.Text = $"Awesome Notepad - {fileName}";
                            Properties.Settings.Default.isOpened = true;
                            Properties.Settings.Default.isSaved = true;
                            Properties.Settings.Default.isNewFile = false;
                            encoding = "UTF-8";
                        }
                        else
                        {

                        }
                    }
                    else if (open == DialogResult.Cancel)
                    {
                    }
                }
                else if (Properties.Settings.Default.isSaved == true)
                {
                    OpenFileDialog openD = new OpenFileDialog();

                    // Установка фильтра для диалогового окна
                    openD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";

                    if (openD.ShowDialog() == DialogResult.OK)
                    {
                        // Чтение текста из выбранного файла и установка его в TextBox
                        richTextBox1.Text = File.ReadAllText(openD.FileName);
                        openedFileName = openD.FileName;
                        string fileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {fileName}";
                        Properties.Settings.Default.isOpened = true;
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isNewFile = false;
                        encoding = "UTF-8";
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                if (Properties.Settings.Default.isSaved == false)
                {
                    DialogResult open = MessageBox.Show("The current file is unsaved. Do you want to save it?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == true)
                    {
                        SaveFileDialog saveD = new SaveFileDialog();

                        // Setting the filter for the dialog
                        saveD.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";
                        saveD.FileName = "Untitled";

                        if (saveD.ShowDialog() == DialogResult.OK)
                        {
                            // Save the text from the TextBox to the selected file
                            File.WriteAllText(saveD.FileName, richTextBox1.Text);
                            savedFileName = saveD.FileName;
                            string sFileName = Path.GetFileName(savedFileName);
                            this.Text = $"Awesome Notepad - {sFileName}";
                            openedFileName = sFileName;
                            Properties.Settings.Default.isSaved = true;
                            encoding = "UTF-8";

                            OpenFileDialog openD = new OpenFileDialog();

                            // Setting the filter for the dialog
                            openD.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";

                            if (openD.ShowDialog() == DialogResult.OK)
                            {
                                // Read the text from the selected file and set it to the TextBox
                                richTextBox1.Text = File.ReadAllText(openD.FileName);
                                openedFileName = openD.FileName;
                                string fileName = Path.GetFileName(openedFileName);
                                this.Text = $"Awesome Notepad - {fileName}";
                                Properties.Settings.Default.isOpened = true;
                                Properties.Settings.Default.isSaved = true;
                                Properties.Settings.Default.isNewFile = false;
                                encoding = "UTF-8";
                            }
                        }
                    }
                    else if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == false)
                    {
                        // Save the text from the TextBox to the selected file
                        File.WriteAllText(openedFileName, richTextBox1.Text);
                        string oFileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {oFileName}";
                        Properties.Settings.Default.isSaved = true;
                        encoding = "UTF-8";

                        OpenFileDialog openD = new OpenFileDialog();

                        // Setting the filter for the dialog
                        openD.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";

                        if (openD.ShowDialog() == DialogResult.OK)
                        {
                            // Read the text from the selected file and set it to the TextBox
                            richTextBox1.Text = File.ReadAllText(openD.FileName);
                            openedFileName = openD.FileName;
                            string fileName = Path.GetFileName(openedFileName);
                            this.Text = $"Awesome Notepad - {fileName}";
                            Properties.Settings.Default.isOpened = true;
                            Properties.Settings.Default.isSaved = true;
                            Properties.Settings.Default.isNewFile = false;
                            encoding = "UTF-8";
                        }
                    }
                    else if (open == DialogResult.No)
                    {
                        OpenFileDialog openD = new OpenFileDialog();

                        // Setting the filter for the dialog
                        openD.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";

                        if (openD.ShowDialog() == DialogResult.OK)
                        {
                            // Read the text from the selected file and set it to the TextBox
                            richTextBox1.Text = File.ReadAllText(openD.FileName);
                            openedFileName = openD.FileName;
                            string fileName = Path.GetFileName(openedFileName);
                            this.Text = $"Awesome Notepad - {fileName}";
                            Properties.Settings.Default.isOpened = true;
                            Properties.Settings.Default.isSaved = true;
                            Properties.Settings.Default.isNewFile = false;
                            encoding = "UTF-8";
                        }
                    }
                    else if (open == DialogResult.Cancel)
                    {
                    }
                }
                else if (Properties.Settings.Default.isSaved == true)
                {
                    OpenFileDialog openD = new OpenFileDialog();

                    // Setting the filter for the dialog
                    openD.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";

                    if (openD.ShowDialog() == DialogResult.OK)
                    {
                        // Read the text from the selected file and set it to the TextBox
                        richTextBox1.Text = File.ReadAllText(openD.FileName);
                        openedFileName = openD.FileName;
                        string fileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {fileName}";
                        Properties.Settings.Default.isOpened = true;
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isNewFile = false;
                        encoding = "UTF-8";
                    }
                }

            }

        }

        // Сохранить
        private void сохранитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Lang == 1)
            {
                try
                {
                    File.WriteAllText(openedFileName, richTextBox1.Text);
                    Properties.Settings.Default.isSaved = true;
                    string fileName = Path.GetFileName(openedFileName);
                    this.Text = $"Awesome Notepad - {fileName}";
                    encoding = "UTF-8";
                }
                catch
                {
                    SaveFileDialog save = new SaveFileDialog();

                    // Установка фильтра для диалогового окна
                    save.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";
                    save.FileName = "Безымянный";

                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        // Сохранение текста из TextBox в выбранный файл
                        File.WriteAllText(save.FileName, richTextBox1.Text);
                        savedFileName = save.FileName;
                        string sFileName = Path.GetFileName(savedFileName);
                        this.Text = $"Awesome Notepad - {sFileName}";
                        openedFileName = sFileName;
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isOpened = true;
                        Properties.Settings.Default.isNewFile = false;
                        encoding = "UTF-8";
                    }
                }
            }
            else
            {
                try
                {
                    File.WriteAllText(openedFileName, richTextBox1.Text);
                    Properties.Settings.Default.isSaved = true;
                    string fileName = Path.GetFileName(openedFileName);
                    this.Text = $"Awesome Notepad - {fileName}";
                    encoding = "UTF-8";
                }
                catch
                {
                    SaveFileDialog save = new SaveFileDialog();

                    // Setting the filter for the dialog
                    save.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";
                    save.FileName = "Untitled";

                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        // Save the text from the TextBox to the selected file
                        File.WriteAllText(save.FileName, richTextBox1.Text);
                        savedFileName = save.FileName;
                        string sFileName = Path.GetFileName(savedFileName);
                        this.Text = $"Awesome Notepad - {sFileName}";
                        openedFileName = sFileName;
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isOpened = true;
                        Properties.Settings.Default.isNewFile = false;
                        encoding = "UTF-8";
                    }
                }

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.isFirstRun == true)
            {
                CultureInfo currentCulture = CultureInfo.CurrentCulture;

                if (currentCulture.Name.StartsWith("en"))
                {
                    Properties.Settings.Default.Lang = 2;
                    Properties.Settings.Default.Save();
                }
                else if (currentCulture.Name.StartsWith("ru"))
                {
                    Properties.Settings.Default.Lang = 1;
                    Properties.Settings.Default.Save();
                }
                else if (currentCulture.Name.StartsWith("uz"))
                {
                    Properties.Settings.Default.Lang = 1;
                    Properties.Settings.Default.Save(); ;
                }
                else if (currentCulture.Name.StartsWith("kk"))
                {
                    Properties.Settings.Default.Lang = 1;
                    Properties.Settings.Default.Save();
                }
                else if (currentCulture.Name.StartsWith("be"))
                {
                    Properties.Settings.Default.Lang = 1;
                    Properties.Settings.Default.Save();
                }
                else if (currentCulture.Name.StartsWith("lv"))
                {
                    Properties.Settings.Default.Lang = 1;
                    Properties.Settings.Default.Save();
                }
                else if (currentCulture.Name.StartsWith("uk"))
                {
                    Properties.Settings.Default.Lang = 1;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.Lang = 2;
                    Properties.Settings.Default.Save();
                }
                richTextBox1.Font = new Font("Calibri", 12);
                Properties.Settings.Default.isFirstRun = false;
            }
            this.Location = Properties.Settings.Default.formLoc;
            this.Size = Properties.Settings.Default.formSize;
            try
            {
                if (Properties.Settings.Default.openedFilePath != null)
                {
                    openedFileName = Properties.Settings.Default.openedFilePath;
                    richTextBox1.Text = File.ReadAllText(openedFileName);
                    string fileName = Path.GetFileName(openedFileName);
                    this.Text = $"Awesome Notepad - {fileName}";
                    Properties.Settings.Default.isOpened = true;
                    Properties.Settings.Default.isSaved = true;
                    Properties.Settings.Default.isNewFile = false;
                    Properties.Settings.Default.openedFilePath = "";

                }
                else if (Properties.Settings.Default.openedFilePath == null)
                {
                    if (Properties.Settings.Default.Lang == 1)
                    {
                        this.Text = "Awesome Notepad - Безымянный.txt";
                    }
                    else
                    {
                        this.Text = "Awesome Notepad - Untitled.txt";
                    }
                }
            }
            catch
            {

            }
            if (Properties.Settings.Default.RememberFont == true)
            {
                richTextBox1.Font = Properties.Settings.Default.Font;
                checkBox8.Checked = true;
            }
            else
            {
                Properties.Settings.Default.Font = new Font("Calibri", 12);
                richTextBox1.Font = Properties.Settings.Default.Font;
                checkBox8.Checked = false;
            }
            if (Properties.Settings.Default.isNewLine == true)
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
            if (Properties.Settings.Default.isWarnings == true)
            {
                checkBox2.Checked = false;
            }
            else
            {
                checkBox2.Checked = true;
            }
            if (Properties.Settings.Default.isTopMost == true)
            {
                checkBox4.Checked = true;
            }
            else
            {
                checkBox4.Checked = false;
            }
            if (Properties.Settings.Default.AutoWordSelection == true)
            {
                checkBox3.Checked = true;
            }
            else
            {
                checkBox3.Checked = false;
            }
            if (Properties.Settings.Default.isPercentage == true)
            {
                checkBox6.Checked = true;
            }
            else
            {
                checkBox6.Checked = false;
            }
            if (Properties.Settings.Default.isBottomPanelHidden == true)
            {
                checkBox5.Checked = true;
            }
            else
            {
                checkBox5.Checked = false;
            }
            if (Properties.Settings.Default.isDev == true)
            {
                checkBox7.Checked = true;
            }
            else
            {
                checkBox7.Checked = false;
            }
            if (Properties.Settings.Default.Theme == 0)
            {
                comboBox1.SelectedIndex = 0;
            }
            if (Properties.Settings.Default.Theme == 1)
            {
                comboBox1.SelectedIndex = 1;
            }
            if (Properties.Settings.Default.Theme == 2)
            {
                comboBox1.SelectedIndex = 2;
            }
            if (Properties.Settings.Default.BottomPanelColor == 0)
            {
                comboBox2.SelectedIndex = 0;
            }
            else
            {
                comboBox2.SelectedIndex = 1;
            }
            if (Properties.Settings.Default.TopPanelColor == 0)
            {
                comboBox3.SelectedIndex = 0;
            }
            else
            {
                comboBox3.SelectedIndex = 1;
            }
            if (Properties.Settings.Default.ContextMenuColor == 0)
            {
                comboBox4.SelectedIndex = 0;
            }
            else
            {
                comboBox4.SelectedIndex = 1;
            }
            if (Properties.Settings.Default.SettingsColor == 0)
            {
                comboBox5.SelectedIndex = 0;
            }
            else
            {
                comboBox5.SelectedIndex = 1;
            }
            if (Properties.Settings.Default.Opacity == 0)
            {
                this.Opacity = 0.25;
                trackBar1.Value = 0;
            }
            else if (Properties.Settings.Default.Opacity == 1)
            {
                this.Opacity = 0.3;
                trackBar1.Value = 1;
            }
            else if (Properties.Settings.Default.Opacity == 2)
            {
                this.Opacity = 0.35;
                trackBar1.Value = 2;
            }
            else if (Properties.Settings.Default.Opacity == 3)
            {
                this.Opacity = 0.4;
                trackBar1.Value = 3;
            }
            else if (Properties.Settings.Default.Opacity == 4)
            {
                this.Opacity = 0.45;
                trackBar1.Value = 4;
            }
            else if (Properties.Settings.Default.Opacity == 5)
            {
                this.Opacity = 0.5;
                trackBar1.Value = 5;
            }
            else if (Properties.Settings.Default.Opacity == 6)
            {
                this.Opacity = 0.55;
                trackBar1.Value = 6;
            }
            else if (Properties.Settings.Default.Opacity == 7)
            {
                this.Opacity = 0.6;
                trackBar1.Value = 7;
            }
            else if (Properties.Settings.Default.Opacity == 8)
            {
                this.Opacity = 0.65;
                trackBar1.Value = 8;
            }
            else if (Properties.Settings.Default.Opacity == 9)
            {
                this.Opacity = 0.7;
                trackBar1.Value = 9;
            }
            else if (Properties.Settings.Default.Opacity == 10)
            {
                this.Opacity = 0.75;
                trackBar1.Value = 10;
            }
            else if (Properties.Settings.Default.Opacity == 11)
            {
                this.Opacity = 0.8;
                trackBar1.Value = 11;
            }
            else if (Properties.Settings.Default.Opacity == 12)
            {
                this.Opacity = 0.85;
                trackBar1.Value = 12;
            }
            else if (Properties.Settings.Default.Opacity == 13)
            {
                this.Opacity = 0.9;
                trackBar1.Value = 13;
            }
            else if (Properties.Settings.Default.Opacity == 14)
            {
                this.Opacity = 0.95;
                trackBar1.Value = 14;
            }
            else if (Properties.Settings.Default.Opacity == 15)
            {
                this.Opacity = 1;
                trackBar1.Value = 15;
            }
            if(Properties.Settings.Default.isCompact)
            {
                checkBox9.Checked = true;
                checkBox6.Enabled = false;
            }
            else
            {
                checkBox9.Checked = false;
                checkBox6.Enabled = true;
            }
            if(Properties.Settings.Default.Lang == 1)
            {
                comboBox6.SelectedIndex = 0;
            }
            else
            {
                comboBox6.SelectedIndex = 1;
            }
        }

        private void шрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Font = fontDialog1.Font;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.isSaved = true;
            Properties.Settings.Default.isNewFile = true;
            Properties.Settings.Default.isOpened = false;
            Properties.Settings.Default.Save();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Properties.Settings.Default.isWarnings == true)
            {
                if(Properties.Settings.Default.Lang == 1)
                {
                    if (Properties.Settings.Default.isSaved == false)
                    {
                        if (Properties.Settings.Default.isNewFile == true)
                        {
                            DialogResult save = MessageBox.Show("Ваш файл не сохранён. Желаете ли вы его сохранить?", "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (save == DialogResult.Yes)
                            {
                                SaveFileDialog saveD = new SaveFileDialog();

                                // Установка фильтра для диалогового окна
                                saveD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";
                                saveD.FileName = "Безымянный";

                                if (saveD.ShowDialog() == DialogResult.OK)
                                {
                                    // Сохранение текста из TextBox в выбранный файл
                                    File.WriteAllText(saveD.FileName, richTextBox1.Text);
                                }
                                else
                                {
                                    e.Cancel = true;
                                }
                            }
                            if (save == DialogResult.No)
                            {

                            }
                            if (save == DialogResult.Cancel)
                            {
                                e.Cancel = true;
                            }
                            Properties.Settings.Default.formLoc = this.Location;
                            Properties.Settings.Default.formSize = this.Size;
                            Properties.Settings.Default.Save();
                        }
                        else if (Properties.Settings.Default.isNewFile == false)
                        {
                            DialogResult save = MessageBox.Show("Ваш файл не сохранён. Желаете ли вы его сохранить?", "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (save == DialogResult.Yes)
                            {
                                SaveFileDialog saveD = new SaveFileDialog();

                                // Установка фильтра для диалогового окна
                                saveD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";
                                saveD.FileName = $"{openedFileName}";

                                if (saveD.ShowDialog() == DialogResult.OK)
                                {
                                    // Сохранение текста из TextBox в выбранный файл
                                    File.WriteAllText(saveD.FileName, richTextBox1.Text);
                                }
                                else
                                {
                                    e.Cancel = true;
                                }
                            }
                            if (save == DialogResult.No)
                            {

                            }
                            if (save == DialogResult.Cancel)
                            {
                                e.Cancel = true;
                            }
                            Properties.Settings.Default.formLoc = this.Location;
                            Properties.Settings.Default.formSize = this.Size;
                            Properties.Settings.Default.Save();
                        }
                    }
                }
                else
                {
                    if (Properties.Settings.Default.isSaved == false)
                    {
                        if (Properties.Settings.Default.isNewFile == true)
                        {
                            DialogResult save = MessageBox.Show("Your file is not saved. Do you want to save it?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (save == DialogResult.Yes)
                            {
                                SaveFileDialog saveD = new SaveFileDialog();

                                // Set the filter for the dialog box
                                saveD.Filter = "Text files (*.txt)|*.txt|Windows Batch file (*.bat)|*.bat|Windows Script (*.vbs)|*.vbs|Windows Registry file (*.reg)|*.reg|All files (*.*)|*.*";
                                saveD.FileName = "Untitled";

                                if (saveD.ShowDialog() == DialogResult.OK)
                                {
                                    // Save the text from the TextBox to the selected file
                                    File.WriteAllText(saveD.FileName, richTextBox1.Text);
                                }
                                else
                                {
                                    e.Cancel = true;
                                }
                            }
                            if (save == DialogResult.No)
                            {
                                // Do nothing
                            }
                            if (save == DialogResult.Cancel)
                            {
                                e.Cancel = true;
                            }
                            Properties.Settings.Default.formLoc = this.Location;
                            Properties.Settings.Default.formSize = this.Size;
                            Properties.Settings.Default.Save();
                        }
                        else if (Properties.Settings.Default.isNewFile == false)
                        {
                            DialogResult save = MessageBox.Show("Your file is not saved. Do you want to save it?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (save == DialogResult.Yes)
                            {
                                SaveFileDialog saveD = new SaveFileDialog();

                                // Set the filter for the dialog box
                                saveD.Filter = "Text files (*.txt)|*.txt|Windows Batch file (*.bat)|*.bat|Windows Script (*.vbs)|*.vbs|Windows Registry file (*.reg)|*.reg|All files (*.*)|*.*";
                                saveD.FileName = $"{openedFileName}";

                                if (saveD.ShowDialog() == DialogResult.OK)
                                {
                                    // Save the text from the TextBox to the selected file
                                    File.WriteAllText(saveD.FileName, richTextBox1.Text);
                                }
                                else
                                {
                                    e.Cancel = true;
                                }
                            }
                            if (save == DialogResult.No)
                            {
                                // Do nothing
                            }
                            if (save == DialogResult.Cancel)
                            {
                                e.Cancel = true;
                            }
                            Properties.Settings.Default.formLoc = this.Location;
                            Properties.Settings.Default.formSize = this.Size;
                            Properties.Settings.Default.Save();
                        }
                    }
                }

            }

            else
            {
                Properties.Settings.Default.formLoc = this.Location;
                Properties.Settings.Default.formSize = this.Size;
                Properties.Settings.Default.Save();
            }

            Properties.Settings.Default.formLoc = this.Location;
            Properties.Settings.Default.formSize = this.Size;
            Properties.Settings.Default.Save();

        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Lang == 1)
            {
                if (Properties.Settings.Default.isSaved == true)
                {
                    richTextBox1.Text = " ";
                    openedFileName = "";
                    savedFileName = "";
                    Properties.Settings.Default.isOpened = false;
                    Properties.Settings.Default.isSaved = true;
                    Properties.Settings.Default.isNewFile = true;
                    Properties.Settings.Default.isAutoSave = false;
                    this.Text = "Awesome Notepad - Безымянный.txt";
                    encoding = "UTF-8";
                }
                if (Properties.Settings.Default.isSaved == false)
                {
                    DialogResult open = MessageBox.Show("Текущий файл не сохранён. Вы хотите его сохранить?", "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == true)
                    {
                        SaveFileDialog saveD = new SaveFileDialog();

                        // Установка фильтра для диалогового окна
                        saveD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";
                        saveD.FileName = "Безымянный";

                        if (saveD.ShowDialog() == DialogResult.OK)
                        {
                            // Сохранение текста из TextBox в выбранный файл
                            File.WriteAllText(saveD.FileName, richTextBox1.Text);
                            savedFileName = saveD.FileName;
                            string sFileName = Path.GetFileName(savedFileName);
                            this.Text = $"Awesome Notepad - {sFileName}";
                            openedFileName = sFileName;
                            Properties.Settings.Default.isSaved = true;
                            encoding = "UTF-8";
                            richTextBox1.Text = " ";
                            openedFileName = "";
                            savedFileName = "";
                            Properties.Settings.Default.isOpened = false;
                            Properties.Settings.Default.isSaved = true;
                            Properties.Settings.Default.isNewFile = true;
                            Properties.Settings.Default.isAutoSave = false;
                            this.Text = "Awesome Notepad - Безымянный.txt";
                            encoding = "UTF-8";
                        }
                        else
                        {

                        }
                    }
                    else if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == false)
                    {
                        // Сохранение текста из TextBox в выбранный файл
                        File.WriteAllText(openedFileName, richTextBox1.Text);
                        string oFileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {oFileName}";
                        Properties.Settings.Default.isSaved = true;
                        encoding = "UTF-8";
                        richTextBox1.Text = " ";
                        openedFileName = "";
                        savedFileName = "";
                        Properties.Settings.Default.isOpened = false;
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isNewFile = true;
                        this.Text = "Awesome Notepad - Безымянный.txt";
                        encoding = "UTF-8";
                    }
                    else if (open == DialogResult.No)
                    {
                        richTextBox1.Text = " ";
                        openedFileName = "";
                        savedFileName = "";
                        Properties.Settings.Default.isOpened = false;
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isNewFile = true;
                        this.Text = "Awesome Notepad - Безымянный.txt";
                        encoding = "UTF-8";
                    }
                    else if (open == DialogResult.Cancel)
                    {

                    }
                }
            }
            else
            {
                if (Properties.Settings.Default.isSaved == true)
                {
                    richTextBox1.Text = " ";
                    openedFileName = "";
                    savedFileName = "";
                    Properties.Settings.Default.isOpened = false;
                    Properties.Settings.Default.isSaved = true;
                    Properties.Settings.Default.isNewFile = true;
                    Properties.Settings.Default.isAutoSave = false;
                    this.Text = "Awesome Notepad - Untitled.txt";
                    encoding = "UTF-8";
                }
                if (Properties.Settings.Default.isSaved == false)
                {
                    DialogResult open = MessageBox.Show("The current file is unsaved. Do you want to save it?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == true)
                    {
                        SaveFileDialog saveD = new SaveFileDialog();

                        // Setting the filter for the dialog
                        saveD.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";
                        saveD.FileName = "Untitled";

                        if (saveD.ShowDialog() == DialogResult.OK)
                        {
                            // Save the text from the TextBox to the selected file
                            File.WriteAllText(saveD.FileName, richTextBox1.Text);
                            savedFileName = saveD.FileName;
                            string sFileName = Path.GetFileName(savedFileName);
                            this.Text = $"Awesome Notepad - {sFileName}";
                            openedFileName = sFileName;
                            Properties.Settings.Default.isSaved = true;
                            encoding = "UTF-8";
                            richTextBox1.Text = " ";
                            openedFileName = "";
                            savedFileName = "";
                            Properties.Settings.Default.isOpened = false;
                            Properties.Settings.Default.isSaved = true;
                            Properties.Settings.Default.isNewFile = true;
                            Properties.Settings.Default.isAutoSave = false;
                            this.Text = "Awesome Notepad - Untitled.txt";
                            encoding = "UTF-8";
                        }
                        else
                        {

                        }
                    }
                    else if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == false)
                    {
                        // Save the text from the TextBox to the selected file
                        File.WriteAllText(openedFileName, richTextBox1.Text);
                        string oFileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {oFileName}";
                        Properties.Settings.Default.isSaved = true;
                        encoding = "UTF-8";
                        richTextBox1.Text = " ";
                        openedFileName = "";
                        savedFileName = "";
                        Properties.Settings.Default.isOpened = false;
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isNewFile = true;
                        this.Text = "Awesome Notepad - Untitled.txt";
                        encoding = "UTF-8";
                    }
                    else if (open == DialogResult.No)
                    {
                        richTextBox1.Text = " ";
                        openedFileName = "";
                        savedFileName = "";
                        Properties.Settings.Default.isOpened = false;
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isNewFile = true;
                        this.Text = "Awesome Notepad - Untitled.txt";
                        encoding = "UTF-8";
                    }
                    else if (open == DialogResult.Cancel)
                    {

                    }
                }

            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            devLabel.Text = $"isOpened = {Properties.Settings.Default.isOpened}, isSaved = {Properties.Settings.Default.isSaved}, isNewFile = {Properties.Settings.Default.isNewFile}, isAutoSave = {Properties.Settings.Default.isAutoSave}, isFirstRun = {Properties.Settings.Default.isFirstRun}, isWarnings = {Properties.Settings.Default.isWarnings}\nWidth = {this.Width}, Height = {this.Height}\nFont = {Properties.Settings.Default.Font}, RememberFont = {Properties.Settings.Default.RememberFont}\ncheckBox2.Checked = {checkBox2.Checked}";
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void devLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devLabel.Visible == true)
            {
                devTimer.Stop();
                devTimer.Enabled = false;
                devLabel.Visible = false;
            }
            else
            {
                devTimer.Enabled = true;
                devTimer.Start();
                devLabel.Visible = true;
            }

        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            // Проверяем, что данные, перетаскиваемые в окно, являются файлами
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Разрешаем копирование данных
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            // Получаем список файлов, перетащенных в окно
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Проверяем, что список файлов не пустой
            if (files != null && files.Length > 0)
            {

                // Получаем путь к первому файлу в списке
                string filePath = files[0];

                // Читаем содержимое файла
                string fileContent = File.ReadAllText(filePath);

                // Делаем что-то с содержимым файла, например, выводим его в текстовом поле
                richTextBox1.Text = fileContent;
                openedFileName = filePath;
                string fileName = Path.GetFileName(openedFileName);
                this.Text = $"Awesome Notepad - {fileName}";
                Properties.Settings.Default.isOpened = true;
                Properties.Settings.Default.isSaved = true;
                Properties.Settings.Default.isNewFile = false;
            }
        }

        private void richTextBox1_DragEnter(object sender, DragEventArgs e)
        {
            // Проверяем, что данные, перетаскиваемые в окно, являются файлами
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Разрешаем копирование данных
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void richTextBox1_DragDrop(object sender, DragEventArgs e)
        {
            // Получаем список файлов, перетащенных в окно
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Проверяем, что список файлов не пустой
            if (files != null && files.Length > 0)
            {

                // Получаем путь к первому файлу в списке
                string filePath = files[0];

                // Читаем содержимое файла
                string fileContent = File.ReadAllText(filePath);

                // Делаем что-то с содержимым файла, например, выводим его в текстовом поле
                richTextBox1.Text = fileContent;
                openedFileName = filePath;
                string fileName = Path.GetFileName(openedFileName);
                this.Text = $"Awesome Notepad - {fileName}";
                Properties.Settings.Default.isOpened = true;
                Properties.Settings.Default.isSaved = true;
                Properties.Settings.Default.isNewFile = false;
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void richTextBox1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void devToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (panel1.Visible == true)
            {
                richTextBox1.Width = richTextBox1.Width + 297;
                settingsTimer.Stop();
                panel1.Visible = false;
                this.MinimumSize = new Size(335, 240);
            }
            else
            {
                if (this.Width < 420)
                {
                    this.Size = new Size(420, this.Height);
                    richTextBox1.Width = richTextBox1.Width - 297;
                    settingsTimer.Start();
                    panel1.Visible = true;
                }
                else if (this.Height < 350)
                {
                    this.Size = new Size(this.Width, 350);
                    richTextBox1.Width = richTextBox1.Width - 297;
                    settingsTimer.Start();
                    panel1.Visible = true;
                }
                else if (this.Width < 420 && this.Height < 350)
                {
                    this.Size = new Size(420, 350);
                    richTextBox1.Width = richTextBox1.Width - 297;
                    settingsTimer.Start();
                    panel1.Visible = true;
                }
                else
                {
                    richTextBox1.Width = richTextBox1.Width - 297;
                    settingsTimer.Start();
                    panel1.Visible = true;
                }

            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void settingsTimer_Tick(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.isBottomPanelHidden == true)
            {
                panel1.Size = new Size(297, this.Height - 63);
            }
            else
            {
                panel1.Size = new Size(297, this.Height - 85);
            }
            this.MinimumSize = new Size(420, 350);
        }

        private void автосохранениеToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Lang == 1)
            {
                if (AutoSaveTSMC.Checked == false)
                {
                    AutoSaveTSMC.BackColor = Color.LightGray;
                    Properties.Settings.Default.isAutoSave = false;
                }
                else if (AutoSaveTSMC.Checked == true)
                {
                    if (Properties.Settings.Default.isOpened == true && Properties.Settings.Default.isNewFile == false)
                    {
                        AutoSaveTSMC.BackColor = Color.LightGreen;
                        Properties.Settings.Default.isAutoSave = true;
                    }
                    else if (Properties.Settings.Default.isOpened == false && Properties.Settings.Default.isNewFile == true)
                    {
                        DialogResult autosave = MessageBox.Show("Перед тем, как включить авто-сохранение, необходимо сохранить файл хотя-бы 1 раз. Хотите сохранить файл?", "ATXT - Сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                        if (autosave == DialogResult.Yes)
                        {
                            SaveFileDialog saveD = new SaveFileDialog();

                            // Установка фильтра для диалогового окна
                            saveD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";
                            saveD.FileName = "Безымянный";

                            if (saveD.ShowDialog() == DialogResult.OK)
                            {
                                // Сохранение текста из TextBox в выбранный файл
                                File.WriteAllText(saveD.FileName, richTextBox1.Text);
                                openedFileName = saveD.FileName;
                                Properties.Settings.Default.isAutoSave = true;
                                Properties.Settings.Default.isNewFile = false;
                                Properties.Settings.Default.isOpened = true;
                                AutoSaveTSMC.BackColor = Color.LightGreen;
                            }
                            else
                            {

                            }
                        }
                        if (autosave == DialogResult.No)
                        {
                            Properties.Settings.Default.isAutoSave = false;
                            AutoSaveTSMC.Checked = false;
                        }
                    }

                }
                else
                {
                    if (AutoSaveTSMC.Checked == false)
                    {
                        AutoSaveTSMC.BackColor = Color.LightGray;
                        Properties.Settings.Default.isAutoSave = false;
                    }
                    else if (AutoSaveTSMC.Checked == true)
                    {
                        if (Properties.Settings.Default.isOpened == true && Properties.Settings.Default.isNewFile == false)
                        {
                            AutoSaveTSMC.BackColor = Color.LightGreen;
                            Properties.Settings.Default.isAutoSave = true;
                        }
                        else if (Properties.Settings.Default.isOpened == false && Properties.Settings.Default.isNewFile == true)
                        {
                            DialogResult autosave = MessageBox.Show("Before enabling autosave, you need to save the file at least once. Do you want to save the file?", "ATXT - Save", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (autosave == DialogResult.Yes)
                            {
                                SaveFileDialog saveD = new SaveFileDialog();

                                // Setting the filter for the dialog
                                saveD.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";
                                saveD.FileName = "Untitled";

                                if (saveD.ShowDialog() == DialogResult.OK)
                                {
                                    // Save the text from the TextBox to the selected file
                                    File.WriteAllText(saveD.FileName, richTextBox1.Text);
                                    openedFileName = saveD.FileName;
                                    Properties.Settings.Default.isAutoSave = true;
                                    Properties.Settings.Default.isNewFile = false;
                                    Properties.Settings.Default.isOpened = true;
                                    AutoSaveTSMC.BackColor = Color.LightGreen;
                                }
                                else
                                {

                                }
                            }
                            else if (autosave == DialogResult.No)
                            {
                                Properties.Settings.Default.isAutoSave = false;
                                AutoSaveTSMC.Checked = false;
                            }
                        }

                    }

                }
            }
        }

        private void AutoSaveTSMC_Click(object sender, EventArgs e)
        {

        }

        private void saveTimer_Tick(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Lang == 1)
            {
                if (Properties.Settings.Default.isAutoSave == false)
                {

                }
                else
                {
                    try
                    {
                        AutoSaveTSMC.BackColor = Color.LightGreen;
                        File.WriteAllText(openedFileName, richTextBox1.Text);
                        Properties.Settings.Default.isAutoSave = true;
                        Properties.Settings.Default.isSaved = true;
                        string fileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {fileName}";
                    }
                    catch
                    {
                        saveTimer.Stop();
                        DialogResult autosave = MessageBox.Show("Для работы авто-сохранения, необходимо сохранить файл хотя-бы 1 раз. Хотите сохранить файл?", "ATXT - Сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                        if (autosave == DialogResult.Yes)
                        {
                            SaveFileDialog saveD = new SaveFileDialog();

                            // Установка фильтра для диалогового окна
                            saveD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";
                            saveD.FileName = "Безымянный";

                            if (saveD.ShowDialog() == DialogResult.OK)
                            {
                                // Сохранение текста из TextBox в выбранный файл
                                File.WriteAllText(saveD.FileName, richTextBox1.Text);
                                openedFileName = saveD.FileName;
                                Properties.Settings.Default.isAutoSave = true;
                                Properties.Settings.Default.isNewFile = false;
                                Properties.Settings.Default.isOpened = true;
                                AutoSaveTSMC.BackColor = Color.LightGreen;
                                saveTimer.Start();
                            }
                            else
                            {

                            }
                        }
                        if (autosave == DialogResult.No)
                        {
                            AutoSaveTSMC.BackColor = Color.LightGray;
                            Properties.Settings.Default.isAutoSave = false;
                            saveTimer.Start();
                        }
                    }

                }
            }
            else
            {
                if (Properties.Settings.Default.isAutoSave == false)
                {

                }
                else
                {
                    try
                    {
                        AutoSaveTSMC.BackColor = Color.LightGreen;
                        File.WriteAllText(openedFileName, richTextBox1.Text);
                        Properties.Settings.Default.isAutoSave = true;
                        Properties.Settings.Default.isSaved = true;
                        string fileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {fileName}";
                    }
                    catch
                    {
                        saveTimer.Stop();
                        DialogResult autosave = MessageBox.Show("Before enabling autosave, you need to save the file at least once. Do you want to save the file?", "ATXT - Save", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (autosave == DialogResult.Yes)
                        {
                            SaveFileDialog saveD = new SaveFileDialog();

                            // Setting the filter for the dialog
                            saveD.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";
                            saveD.FileName = "Untitled";

                            if (saveD.ShowDialog() == DialogResult.OK)
                            {
                                // Save the text from the TextBox to the selected file
                                File.WriteAllText(saveD.FileName, richTextBox1.Text);
                                openedFileName = saveD.FileName;
                                Properties.Settings.Default.isAutoSave = true;
                                Properties.Settings.Default.isNewFile = false;
                                Properties.Settings.Default.isOpened = true;
                                AutoSaveTSMC.BackColor = Color.LightGreen;
                                saveTimer.Start();
                            }
                            else
                            {

                            }
                        }
                        if (autosave == DialogResult.No)
                        {
                            AutoSaveTSMC.BackColor = Color.LightGray;
                            Properties.Settings.Default.isAutoSave = false;
                            saveTimer.Start();
                        }
                    }

                }

            }


        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void Count_Tick(object sender, EventArgs e)
        {
            symbolCount = richTextBox1.Text.Length;
            lineCount = richTextBox1.Lines.Length;
            int currentPosition = richTextBox1.SelectionStart;
            int currentLine = richTextBox1.GetLineFromCharIndex(currentPosition) + 1; // Учитываем, что строки в текстовых контролах обычно начинаются с 1
            int currentColumn = currentPosition - richTextBox1.GetFirstCharIndexFromLine(currentLine - 1) + 1; // Также учитываем, что столбцы могут начинаться с 1
            if(Properties.Settings.Default.Lang == 1)
            {
                if (Properties.Settings.Default.isCompact)
                {
                    toolStripStatusLabel1.Text = $"Количество символов: {symbolCount} | Символ: {currentPosition} | Кодировка: {encoding}";
                }
                else
                {
                    if (Properties.Settings.Default.isPercentage == false)
                    {
                        toolStripStatusLabel1.Text = $"Количество символов: {symbolCount} | Количество абзацев: {lineCount} | Строка: {currentLine} | Позиция: {currentColumn} | Символ: {currentPosition} | Кодировка: {encoding}";
                    }
                    else
                    {
                        double num3 = (symbolCount > 0) ? ((((double)currentPosition) / ((double)symbolCount)) * 100.0) : 0.0;
                        toolStripStatusLabel1.Text = $"Количество символов: {symbolCount} | Количество абзацев: {lineCount} | Строка: {currentLine} | Позиция: {currentColumn} | Символ: {currentPosition} | Кодировка: {encoding} | Процент пройденного текста: {num3:F1}%";
                    }
                }
                if (Properties.Settings.Default.isNewFile == true)
                {
                    if (symbolCount < 1)
                    {
                        Settings.Default.isSaved = true;
                        this.Text = $"Awesome Notepad - Безымянный.txt";
                    }
                    else
                    {
                        Settings.Default.isSaved = false;
                        this.Text = $"Awesome Notepad - Безымянный.txt*";
                    }
                }
            }
            else
            {
                if (Properties.Settings.Default.isCompact)
                {
                    toolStripStatusLabel1.Text = $"Characters: {symbolCount} | Character: {currentPosition} | Encoding: {encoding}";
                }
                else
                {
                    if (!Properties.Settings.Default.isPercentage)
                    {
                        toolStripStatusLabel1.Text = $"Characters: {symbolCount} | Paragraphs: {lineCount} | Line: {currentLine} | Position: {currentColumn} | Character: {currentPosition} | Encoding: {encoding}";
                    }
                    else
                    {
                        double percentage = (symbolCount > 0) ? ((((double)currentPosition) / ((double)symbolCount)) * 100.0) : 0.0;
                        toolStripStatusLabel1.Text = $"Characters: {symbolCount} | Paragraphs: {lineCount} | Line: {currentLine} | Position: {currentColumn} | Character: {currentPosition} | Encoding: {encoding} | Percentage of text completed: {percentage:F1}%";
                    }
                }
                if (Properties.Settings.Default.isNewFile)
                {
                    if (symbolCount < 1)
                    {
                        Properties.Settings.Default.isSaved = true;
                        this.Text = $"Awesome Notepad - Untitled.txt";
                    }
                    else
                    {
                        Properties.Settings.Default.isSaved = false;
                        this.Text = $"Awesome Notepad - Untitled.txt*";
                    }
                }

            }
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        private void сменитьКодировкуТекстаНаUTF8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Lang == 1)
            {
                if (Properties.Settings.Default.isSaved == false)
                {
                    DialogResult encode = MessageBox.Show("Перед сменой кодировки необходимо сохранить файл. Вы хотите его сохранить?", "ATXT - Предупреждение");
                }
                if (Properties.Settings.Default.isOpened == true && Properties.Settings.Default.isNewFile == false)
                {
                    Properties.Settings.Default.isAutoSave = false;
                    AutoSaveTSMC.BackColor = Color.LightGray;
                    try
                    {
                        using (StreamReader reader = new StreamReader(this.openedFileName, Encoding.UTF8))
                        {
                            string fileName = this.openedFileName;
                            string fileContent = reader.ReadToEnd();
                            this.richTextBox1.Text = fileContent;
                            encoding = "UTF-8";
                        }
                    }
                    catch
                    {

                    }

                }
                else
                {
                    DialogResult open = MessageBox.Show("Перед сменой кодировки необходимо сохранить файл. Вы хотите его сохранить?", "Сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == true)
                    {
                        SaveFileDialog saveD = new SaveFileDialog();

                        // Установка фильтра для диалогового окна
                        saveD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";
                        saveD.FileName = "Безымянный";

                        if (saveD.ShowDialog() == DialogResult.OK)
                        {
                            // Сохранение текста из TextBox в выбранный файл
                            File.WriteAllText(saveD.FileName, richTextBox1.Text);
                            savedFileName = saveD.FileName;
                            string sFileName = Path.GetFileName(savedFileName);
                            this.Text = $"Awesome Notepad - {sFileName}";
                            openedFileName = sFileName;
                            Properties.Settings.Default.isSaved = true;
                            Properties.Settings.Default.isOpened = true;
                            Properties.Settings.Default.isNewFile = false;
                            encoding = "UTF-8";
                            using (StreamReader reader = new StreamReader(this.savedFileName, Encoding.UTF8))
                            {
                                string fileName = this.savedFileName;
                                string fileContent = reader.ReadToEnd();
                                this.richTextBox1.Text = fileContent;
                                encoding = "UTF-8";
                            }
                        }
                        else
                        {

                        }
                    }
                    else if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == false)
                    {
                        // Сохранение текста из TextBox в выбранный файл
                        File.WriteAllText(openedFileName, richTextBox1.Text);
                        string oFileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {oFileName}";
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isOpened = true;
                        Properties.Settings.Default.isNewFile = false;
                        encoding = "UTF-8";
                        using (StreamReader reader = new StreamReader(this.openedFileName, Encoding.UTF8))
                        {
                            string fileName = this.openedFileName;
                            string fileContent = reader.ReadToEnd();
                            this.richTextBox1.Text = fileContent;
                            encoding = "UTF-8";
                        }
                    }
                    else if (open == DialogResult.No)
                    {

                    }
                }
            }
            else
            {
                if (Properties.Settings.Default.isSaved == false)
                {
                    DialogResult encode = MessageBox.Show("Before changing the encoding, you need to save the file. Do you want to save it?", "ATXT - Warning");
                }
                if (Properties.Settings.Default.isOpened == true && Properties.Settings.Default.isNewFile == false)
                {
                    Properties.Settings.Default.isAutoSave = false;
                    AutoSaveTSMC.BackColor = Color.LightGray;
                    try
                    {
                        using (StreamReader reader = new StreamReader(this.openedFileName, Encoding.UTF8))
                        {
                            string fileName = this.openedFileName;
                            string fileContent = reader.ReadToEnd();
                            this.richTextBox1.Text = fileContent;
                            encoding = "UTF-8";
                        }
                    }
                    catch
                    {

                    }

                }
                else
                {
                    DialogResult open = MessageBox.Show("Before changing the encoding, you need to save the file. Do you want to save it?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == true)
                    {
                        SaveFileDialog saveD = new SaveFileDialog();

                        // Setting the filter for the dialog
                        saveD.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";
                        saveD.FileName = "Untitled";

                        if (saveD.ShowDialog() == DialogResult.OK)
                        {
                            // Save the text from the TextBox to the selected file
                            File.WriteAllText(saveD.FileName, richTextBox1.Text);
                            savedFileName = saveD.FileName;
                            string sFileName = Path.GetFileName(savedFileName);
                            this.Text = $"Awesome Notepad - {sFileName}";
                            openedFileName = sFileName;
                            Properties.Settings.Default.isSaved = true;
                            Properties.Settings.Default.isOpened = true;
                            Properties.Settings.Default.isNewFile = false;
                            encoding = "UTF-8";
                            using (StreamReader reader = new StreamReader(this.savedFileName, Encoding.UTF8))
                            {
                                string fileName = this.savedFileName;
                                string fileContent = reader.ReadToEnd();
                                this.richTextBox1.Text = fileContent;
                                encoding = "UTF-8";
                            }
                        }
                        else
                        {

                        }
                    }
                    else if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == false)
                    {
                        // Save the text from the TextBox to the selected file
                        File.WriteAllText(openedFileName, richTextBox1.Text);
                        string oFileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {oFileName}";
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isOpened = true;
                        Properties.Settings.Default.isNewFile = false;
                        encoding = "UTF-8";
                        using (StreamReader reader = new StreamReader(this.openedFileName, Encoding.UTF8))
                        {
                            string fileName = this.openedFileName;
                            string fileContent = reader.ReadToEnd();
                            this.richTextBox1.Text = fileContent;
                            encoding = "UTF-8";
                        }
                    }
                    else if (open == DialogResult.No)
                    {

                    }
                }

            }
        }

        private void сменитьКодировкуТекстаНаANSIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Lang == 1)
            {
                Properties.Settings.Default.isAutoSave = false;
                AutoSaveTSMC.BackColor = Color.LightGray;
                if (Properties.Settings.Default.isOpened == true && Properties.Settings.Default.isNewFile == false)
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(this.openedFileName, Encoding.Default))
                        {
                            string fileName = this.openedFileName;
                            string fileContent = reader.ReadToEnd();
                            this.richTextBox1.Text = fileContent;
                        }
                        encoding = "ANSI";
                    }
                    catch
                    {

                    }
                }
                else
                {
                    DialogResult open = MessageBox.Show("Перед сменой кодировки необходимо сохранить файл. Вы хотите его сохранить?", "Сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == true)
                    {
                        SaveFileDialog saveD = new SaveFileDialog();

                        // Установка фильтра для диалогового окна
                        saveD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";
                        saveD.FileName = "Безымянный";

                        if (saveD.ShowDialog() == DialogResult.OK)
                        {
                            // Сохранение текста из TextBox в выбранный файл
                            File.WriteAllText(saveD.FileName, richTextBox1.Text);
                            savedFileName = saveD.FileName;
                            string sFileName = Path.GetFileName(savedFileName);
                            this.Text = $"Awesome Notepad - {sFileName}";
                            openedFileName = sFileName;
                            Properties.Settings.Default.isSaved = true;
                            Properties.Settings.Default.isOpened = true;
                            Properties.Settings.Default.isNewFile = false;
                            encoding = "UTF-8";
                            using (StreamReader reader = new StreamReader(this.savedFileName, Encoding.Default))
                            {
                                string fileName = this.savedFileName;
                                string fileContent = reader.ReadToEnd();
                                this.richTextBox1.Text = fileContent;
                            }
                            encoding = "ANSI";
                        }
                        else
                        {

                        }
                    }
                    else if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == false)
                    {
                        // Сохранение текста из TextBox в выбранный файл
                        File.WriteAllText(openedFileName, richTextBox1.Text);
                        string oFileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {oFileName}";
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isOpened = true;
                        Properties.Settings.Default.isNewFile = false;
                        encoding = "UTF-8";
                        using (StreamReader reader = new StreamReader(this.openedFileName, Encoding.Default))
                        {
                            string fileName = this.openedFileName;
                            string fileContent = reader.ReadToEnd();
                            this.richTextBox1.Text = fileContent;
                        }
                        encoding = "ANSI";
                    }
                    else if (open == DialogResult.No)
                    {

                    }
                }
            }
            else
            {
                Properties.Settings.Default.isAutoSave = false;
                AutoSaveTSMC.BackColor = Color.LightGray;
                if (Properties.Settings.Default.isOpened == true && Properties.Settings.Default.isNewFile == false)
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(this.openedFileName, Encoding.Default))
                        {
                            string fileName = this.openedFileName;
                            string fileContent = reader.ReadToEnd();
                            this.richTextBox1.Text = fileContent;
                        }
                        encoding = "ANSI";
                    }
                    catch
                    {

                    }
                }
                else
                {
                    DialogResult open = MessageBox.Show("Before changing the encoding, you need to save the file. Do you want to save it?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == true)
                    {
                        SaveFileDialog saveD = new SaveFileDialog();

                        // Setting the filter for the dialog
                        saveD.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";
                        saveD.FileName = "Untitled";

                        if (saveD.ShowDialog() == DialogResult.OK)
                        {
                            // Save the text from the TextBox to the selected file
                            File.WriteAllText(saveD.FileName, richTextBox1.Text);
                            savedFileName = saveD.FileName;
                            string sFileName = Path.GetFileName(savedFileName);
                            this.Text = $"Awesome Notepad - {sFileName}";
                            openedFileName = sFileName;
                            Properties.Settings.Default.isSaved = true;
                            Properties.Settings.Default.isOpened = true;
                            Properties.Settings.Default.isNewFile = false;
                            using (StreamReader reader = new StreamReader(this.savedFileName, Encoding.Default))
                            {
                                string fileName = this.savedFileName;
                                string fileContent = reader.ReadToEnd();
                                this.richTextBox1.Text = fileContent;
                            }
                            encoding = "ANSI";
                        }
                        else
                        {

                        }
                    }
                    else if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == false)
                    {
                        // Save the text from the TextBox to the selected file
                        File.WriteAllText(openedFileName, richTextBox1.Text);
                        string oFileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {oFileName}";
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isOpened = true;
                        Properties.Settings.Default.isNewFile = false;
                        using (StreamReader reader = new StreamReader(this.openedFileName, Encoding.Default))
                        {
                            string fileName = this.openedFileName;
                            string fileContent = reader.ReadToEnd();
                            this.richTextBox1.Text = fileContent;
                        }
                        encoding = "ANSI";
                    }
                    else if (open == DialogResult.No)
                    {

                    }
                }

            }

        }

        private void zToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void открытьВToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Lang == 1)
            {
                if (Properties.Settings.Default.isSaved == false)
                {
                    DialogResult open = MessageBox.Show("Текущий файл не сохранён. Вы хотите его сохранить?", "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == true)
                    {
                        SaveFileDialog saveD = new SaveFileDialog();

                        // Установка фильтра для диалогового окна
                        saveD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";
                        saveD.FileName = "Безымянный";

                        if (saveD.ShowDialog() == DialogResult.OK)
                        {
                            // Сохранение текста из TextBox в выбранный файл
                            File.WriteAllText(saveD.FileName, richTextBox1.Text);
                            savedFileName = saveD.FileName;
                            string sFileName = Path.GetFileName(savedFileName);
                            this.Text = $"Awesome Notepad - {sFileName}";
                            openedFileName = sFileName;
                            Properties.Settings.Default.isSaved = true;
                            encoding = "UTF-8";
                            OpenFileDialog openD = new OpenFileDialog();

                            // Установка фильтра для диалогового окна
                            openD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";

                            if (openD.ShowDialog() == DialogResult.OK)
                            {
                                using (StreamReader reader = new StreamReader(openD.FileName, Encoding.Default))
                                {
                                    string FileName = openD.FileName;
                                    string str3 = reader.ReadToEnd();
                                    this.richTextBox1.Text = str3;
                                }
                                openedFileName = openD.FileName;
                                string fileName = Path.GetFileName(openedFileName);
                                this.Text = $"Awesome Notepad - {fileName}";
                                Properties.Settings.Default.isOpened = true;
                                Properties.Settings.Default.isSaved = true;
                                Properties.Settings.Default.isNewFile = false;
                                encoding = "ANSI";
                            }
                        }
                        else
                        {

                        }
                    }
                    else if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == false)
                    {
                        // Сохранение текста из TextBox в выбранный файл
                        File.WriteAllText(openedFileName, richTextBox1.Text);
                        string oFileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {oFileName}";
                        Properties.Settings.Default.isSaved = true;
                        encoding = "UTF-8";
                        OpenFileDialog openD = new OpenFileDialog();

                        // Установка фильтра для диалогового окна
                        openD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";

                        if (openD.ShowDialog() == DialogResult.OK)
                        {
                            using (StreamReader reader = new StreamReader(openD.FileName, Encoding.Default))
                            {
                                string FileName = openD.FileName;
                                string str3 = reader.ReadToEnd();
                                this.richTextBox1.Text = str3;
                            }
                            openedFileName = openD.FileName;
                            string fileName = Path.GetFileName(openedFileName);
                            this.Text = $"Awesome Notepad - {fileName}";
                            Properties.Settings.Default.isOpened = true;
                            Properties.Settings.Default.isSaved = true;
                            Properties.Settings.Default.isNewFile = false;
                            encoding = "ANSI";
                        }
                        else
                        {

                        }
                    }
                    else if (open == DialogResult.No)
                    {
                        OpenFileDialog openD = new OpenFileDialog();

                        // Установка фильтра для диалогового окна
                        openD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";

                        if (openD.ShowDialog() == DialogResult.OK)
                        {
                            using (StreamReader reader = new StreamReader(openD.FileName, Encoding.Default))
                            {
                                string FileName = openD.FileName;
                                string str3 = reader.ReadToEnd();
                                this.richTextBox1.Text = str3;
                            }
                            openedFileName = openD.FileName;
                            string fileName = Path.GetFileName(openedFileName);
                            this.Text = $"Awesome Notepad - {fileName}";
                            Properties.Settings.Default.isOpened = true;
                            Properties.Settings.Default.isSaved = true;
                            Properties.Settings.Default.isNewFile = false;
                            encoding = "ANSI";
                        }
                        else
                        {

                        }
                    }
                    else if (open == DialogResult.Cancel)
                    {
                    }
                }
                else if (Properties.Settings.Default.isSaved == true)
                {
                    OpenFileDialog openD = new OpenFileDialog();

                    // Установка фильтра для диалогового окна
                    openD.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";

                    if (openD.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamReader reader = new StreamReader(openD.FileName, Encoding.Default))
                        {
                            string FileName = openD.FileName;
                            string str3 = reader.ReadToEnd();
                            this.richTextBox1.Text = str3;
                        }
                        openedFileName = openD.FileName;
                        string fileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {fileName}";
                        Properties.Settings.Default.isOpened = true;
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isNewFile = false;
                        encoding = "ANSI";
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                if (Properties.Settings.Default.isSaved == false)
                {
                    DialogResult open = MessageBox.Show("The current file is unsaved. Do you want to save it?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == true)
                    {
                        SaveFileDialog saveD = new SaveFileDialog();

                        // Setting the filter for the dialog
                        saveD.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";
                        saveD.FileName = "Untitled";

                        if (saveD.ShowDialog() == DialogResult.OK)
                        {
                            // Save the text from the TextBox to the selected file
                            File.WriteAllText(saveD.FileName, richTextBox1.Text);
                            savedFileName = saveD.FileName;
                            string sFileName = Path.GetFileName(savedFileName);
                            this.Text = $"Awesome Notepad - {sFileName}";
                            openedFileName = sFileName;
                            Properties.Settings.Default.isSaved = true;
                            encoding = "UTF-8";

                            OpenFileDialog openD = new OpenFileDialog();

                            // Setting the filter for the dialog
                            openD.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";

                            if (openD.ShowDialog() == DialogResult.OK)
                            {
                                using (StreamReader reader = new StreamReader(openD.FileName, Encoding.Default))
                                {
                                    string FileName = openD.FileName;
                                    string str3 = reader.ReadToEnd();
                                    this.richTextBox1.Text = str3;
                                }
                                openedFileName = openD.FileName;
                                string fileName = Path.GetFileName(openedFileName);
                                this.Text = $"Awesome Notepad - {fileName}";
                                Properties.Settings.Default.isOpened = true;
                                Properties.Settings.Default.isSaved = true;
                                Properties.Settings.Default.isNewFile = false;
                                encoding = "ANSI";
                            }
                        }
                        else
                        {

                        }
                    }
                    else if (open == DialogResult.Yes && Properties.Settings.Default.isNewFile == false)
                    {
                        // Save the text from the TextBox to the selected file
                        File.WriteAllText(openedFileName, richTextBox1.Text);
                        string oFileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {oFileName}";
                        Properties.Settings.Default.isSaved = true;
                        encoding = "UTF-8";
                        OpenFileDialog openD = new OpenFileDialog();

                        // Setting the filter for the dialog
                        openD.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";

                        if (openD.ShowDialog() == DialogResult.OK)
                        {
                            using (StreamReader reader = new StreamReader(openD.FileName, Encoding.Default))
                            {
                                string FileName = openD.FileName;
                                string str3 = reader.ReadToEnd();
                                this.richTextBox1.Text = str3;
                            }
                            openedFileName = openD.FileName;
                            string fileName = Path.GetFileName(openedFileName);
                            this.Text = $"Awesome Notepad - {fileName}";
                            Properties.Settings.Default.isOpened = true;
                            Properties.Settings.Default.isSaved = true;
                            Properties.Settings.Default.isNewFile = false;
                            encoding = "ANSI";
                        }
                        else
                        {

                        }
                    }
                    else if (open == DialogResult.No)
                    {
                        OpenFileDialog openD = new OpenFileDialog();

                        // Setting the filter for the dialog
                        openD.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";

                        if (openD.ShowDialog() == DialogResult.OK)
                        {
                            using (StreamReader reader = new StreamReader(openD.FileName, Encoding.Default))
                            {
                                string FileName = openD.FileName;
                                string str3 = reader.ReadToEnd();
                                this.richTextBox1.Text = str3;
                            }
                            openedFileName = openD.FileName;
                            string fileName = Path.GetFileName(openedFileName);
                            this.Text = $"Awesome Notepad - {fileName}";
                            Properties.Settings.Default.isOpened = true;
                            Properties.Settings.Default.isSaved = true;
                            Properties.Settings.Default.isNewFile = false;
                            encoding = "ANSI";
                        }
                        else
                        {

                        }
                    }
                    else if (open == DialogResult.Cancel)
                    {
                    }
                }
                else if (Properties.Settings.Default.isSaved == true)
                {
                    OpenFileDialog openD = new OpenFileDialog();

                    // Setting the filter for the dialog
                    openD.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";

                    if (openD.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamReader reader = new StreamReader(openD.FileName, Encoding.Default))
                        {
                            string FileName = openD.FileName;
                            string str3 = reader.ReadToEnd();
                            this.richTextBox1.Text = str3;
                        }
                        openedFileName = openD.FileName;
                        string fileName = Path.GetFileName(openedFileName);
                        this.Text = $"Awesome Notepad - {fileName}";
                        Properties.Settings.Default.isOpened = true;
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isNewFile = false;
                        encoding = "ANSI";
                    }
                    else
                    {

                    }
                }

            }
        }

        private void сохранитьКакВToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Lang == 1)
            {
                SaveFileDialog save = new SaveFileDialog();

                // Установка фильтра для диалогового окна
                save.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";
                if (Properties.Settings.Default.isOpened == true)
                {
                    save.FileName = $"{openedFileName}";
                }
                else
                {
                    save.FileName = "Безымянный";
                }


                if (save.ShowDialog() == DialogResult.OK)
                {
                    // Сохранение текста из TextBox в выбранный файл
                    string text = this.richTextBox1.Text;
                    using (StreamWriter writer = new StreamWriter(save.FileName, false, Encoding.Default))
                    {
                        writer.Write(text);
                    }
                    savedFileName = save.FileName;
                    string sFileName = Path.GetFileName(savedFileName);
                    this.Text = $"Awesome Notepad - {sFileName}";
                    openedFileName = sFileName;
                    Properties.Settings.Default.isSaved = true;
                    Properties.Settings.Default.isOpened = true;
                    Properties.Settings.Default.isNewFile = false;
                    encoding = "ANSI";
                }
            }
            else
            {
                SaveFileDialog save = new SaveFileDialog();

                // Setting the filter for the dialog
                save.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";
                if (Properties.Settings.Default.isOpened == true)
                {
                    save.FileName = $"{openedFileName}";
                }
                else
                {
                    save.FileName = "Untitled";
                }


                if (save.ShowDialog() == DialogResult.OK)
                {
                    // Save the text from the TextBox to the selected file
                    string text = this.richTextBox1.Text;
                    using (StreamWriter writer = new StreamWriter(save.FileName, false, Encoding.Default))
                    {
                        writer.Write(text);
                    }
                    savedFileName = save.FileName;
                    string sFileName = Path.GetFileName(savedFileName);
                    this.Text = $"Awesome Notepad - {sFileName}";
                    openedFileName = sFileName;
                    Properties.Settings.Default.isSaved = true;
                    Properties.Settings.Default.isOpened = true;
                    Properties.Settings.Default.isNewFile = false;
                    encoding = "ANSI";
                }

            }
        }

        private void сохранитьВANSIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Lang == 1)
            {
                try
                {
                    string text = this.richTextBox1.Text;
                    using (StreamWriter writer = new StreamWriter(openedFileName, false, Encoding.Default))
                    {
                        writer.Write(text);
                    }
                    Properties.Settings.Default.isSaved = true;
                    string fileName = Path.GetFileName(openedFileName);
                    this.Text = $"Awesome Notepad - {fileName}";
                    encoding = "ANSI";
                }
                catch
                {
                    SaveFileDialog save = new SaveFileDialog();

                    // Установка фильтра для диалогового окна
                    save.Filter = "Текстовые файлы (*.txt)|*.txt|Пакетный файл Windows (*.bat)|*.bat|Сценарий Windows (*.vbs)|*.vbs|Файл реестра Windows (*.reg)|*.reg|Все файлы (*.*)|*.*";
                    if (Properties.Settings.Default.isOpened == true)
                    {
                        save.FileName = $"{openedFileName}";
                    }
                    else
                    {
                        save.FileName = "Безымянный";
                    }


                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        // Сохранение текста из TextBox в выбранный файл
                        string text = this.richTextBox1.Text;
                        using (StreamWriter writer = new StreamWriter(save.FileName, false, Encoding.Default))
                        {
                            writer.Write(text);
                        }
                        savedFileName = save.FileName;
                        string sFileName = Path.GetFileName(savedFileName);
                        this.Text = $"Awesome Notepad - {sFileName}";
                        openedFileName = sFileName;
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isOpened = true;
                        Properties.Settings.Default.isNewFile = false;
                    }

                }
            }
            else
            {
                try
                {
                    string text = this.richTextBox1.Text;
                    using (StreamWriter writer = new StreamWriter(openedFileName, false, Encoding.Default))
                    {
                        writer.Write(text);
                    }
                    Properties.Settings.Default.isSaved = true;
                    string fileName = Path.GetFileName(openedFileName);
                    this.Text = $"Awesome Notepad - {fileName}";
                    encoding = "ANSI";
                }
                catch
                {
                    SaveFileDialog save = new SaveFileDialog();

                    // Setting the filter for the dialog
                    save.Filter = "Text files (*.txt)|*.txt|Batch files (*.bat)|*.bat|Windows script files (*.vbs)|*.vbs|Windows registry files (*.reg)|*.reg|All files (*.*)|*.*";
                    if (Properties.Settings.Default.isOpened == true)
                    {
                        save.FileName = $"{openedFileName}";
                    }
                    else
                    {
                        save.FileName = "Untitled";
                    }


                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        // Save the text from the TextBox to the selected file
                        string text = this.richTextBox1.Text;
                        using (StreamWriter writer = new StreamWriter(save.FileName, false, Encoding.Default))
                        {
                            writer.Write(text);
                        }
                        savedFileName = save.FileName;
                        string sFileName = Path.GetFileName(savedFileName);
                        this.Text = $"Awesome Notepad - {sFileName}";
                        openedFileName = sFileName;
                        Properties.Settings.Default.isSaved = true;
                        Properties.Settings.Default.isOpened = true;
                        Properties.Settings.Default.isNewFile = false;
                    }

                }

            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                richTextBox1.WordWrap = true;
                Properties.Settings.Default.isNewLine = true;
            }
            else
            {
                richTextBox1.WordWrap = false;
                Properties.Settings.Default.isNewLine = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                Properties.Settings.Default.isWarnings = false;
            }
            else if (checkBox2.Checked == false)
            {
                Properties.Settings.Default.isWarnings = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                richTextBox1.AutoWordSelection = true;
                Properties.Settings.Default.AutoWordSelection = true;
            }
            else
            {
                richTextBox1.AutoWordSelection = false;
                Properties.Settings.Default.AutoWordSelection = false;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool saved;
            if(Properties.Settings.Default.isSaved == true)
            {
                saved = true;
            }
            else
            {
                saved = false;
            }
            if (comboBox1.SelectedIndex == 0)
            {
                Properties.Settings.Default.Theme = 0;
                button1.Enabled = false;
                button2.Enabled = false;
                label8.Enabled = false;
                label9.Enabled = false;
                label10.Enabled = false;
                label11.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox5.Enabled = false;
                richTextBox1.SelectAll();
                richTextBox1.SelectionBackColor = Color.White;
                richTextBox1.Select();
                Properties.Settings.Default.isSaved = saved;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                Properties.Settings.Default.Theme = 1;
                button1.Enabled = false;
                button2.Enabled = false;
                label8.Enabled = false;
                label9.Enabled = false;
                label10.Enabled = false;
                label11.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox5.Enabled = false;
                richTextBox1.SelectAll();
                richTextBox1.SelectionBackColor = Color.Black;
                richTextBox1.Select();
                Properties.Settings.Default.isSaved = saved;
            }
            else
            {
                Properties.Settings.Default.Theme = 2;
                button1.Enabled = true;
                button2.Enabled = true;
                label8.Enabled = true;
                label9.Enabled = true;
                label10.Enabled = true;
                label11.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox5.Enabled = true;
                richTextBox1.BackColor = Properties.Settings.Default.BackColor;
                richTextBox1.ForeColor = Properties.Settings.Default.TextColor;
                panel1.BackColor = SystemColors.Control;
                panel1.ForeColor = SystemColors.ControlText;
                menuStrip1.BackColor = SystemColors.Control;
                menuStrip1.ForeColor = SystemColors.ControlText;
                файлToolStripMenuItem.BackColor = SystemColors.Control;
                файлToolStripMenuItem.ForeColor = SystemColors.ControlText;
                создатьToolStripMenuItem.BackColor = SystemColors.Control;
                создатьToolStripMenuItem.ForeColor = SystemColors.ControlText;
                сохранитьToolStripMenuItem.BackColor = SystemColors.Control;
                сохранитьToolStripMenuItem.ForeColor = SystemColors.ControlText;
                сохранитьToolStripMenuItem1.BackColor = SystemColors.Control;
                сохранитьToolStripMenuItem1.ForeColor = SystemColors.ControlText;
                открытьToolStripMenuItem.BackColor = SystemColors.Control;
                открытьToolStripMenuItem.ForeColor = SystemColors.ControlText;
                сохранитьВANSIToolStripMenuItem.BackColor = SystemColors.Control;
                сохранитьВANSIToolStripMenuItem.ForeColor = SystemColors.ControlText;
                сохранитьКакВToolStripMenuItem.BackColor = SystemColors.Control;
                сохранитьКакВToolStripMenuItem.ForeColor = SystemColors.ControlText;
                открытьВToolStripMenuItem.BackColor = SystemColors.Control;
                открытьВToolStripMenuItem.ForeColor = SystemColors.ControlText;
                кодировкаToolStripMenuItem.BackColor = SystemColors.Control;
                кодировкаToolStripMenuItem.ForeColor = SystemColors.ControlText;
                сменитьКодировкуТекстаНаUTF8ToolStripMenuItem.BackColor = SystemColors.Control;
                сменитьКодировкуТекстаНаUTF8ToolStripMenuItem.ForeColor = SystemColors.ControlText;
                сменитьКодировкуТекстаНаANSIToolStripMenuItem.BackColor = SystemColors.Control;
                сменитьКодировкуТекстаНаANSIToolStripMenuItem.ForeColor = SystemColors.ControlText;
                шрифтToolStripMenuItem.BackColor = SystemColors.Control;
                шрифтToolStripMenuItem.ForeColor = SystemColors.ControlText;
                toolStripSeparator1.Visible = true;
                toolStripSeparator2.Visible = true;
                toolStripSeparator3.Visible = true;
                видToolStripMenuItem.BackColor = SystemColors.Control;
                видToolStripMenuItem.ForeColor = SystemColors.ControlText;
                SettingsTSMI.BackColor = SystemColors.Control;
                SettingsTSMI.ForeColor = SystemColors.ControlText;
                оПрограммеToolStripMenuItem.BackColor = SystemColors.Control;
                оПрограммеToolStripMenuItem.ForeColor = SystemColors.ControlText;
                AutoSaveTSMC.ForeColor = SystemColors.ControlText;
                statusStrip1.ForeColor = SystemColors.ControlText;
                statusStrip1.BackColor = SystemColors.Control;
                devLabel.ForeColor = SystemColors.ControlText;
                devLabel.BackColor = Color.White;
                label3.ForeColor = SystemColors.ControlText;
                label4.ForeColor = SystemColors.ControlText;
                label5.ForeColor = SystemColors.ControlText;
                label6.ForeColor = SystemColors.ControlText;
                label7.ForeColor = SystemColors.ControlText;
                label8.ForeColor = SystemColors.ControlText;
                label9.ForeColor = SystemColors.ControlText;
                label10.ForeColor = SystemColors.ControlText;
                label11.ForeColor = SystemColors.ControlText;
                label12.ForeColor = SystemColors.ControlText;
                checkBox1.ForeColor = SystemColors.ControlText;
                checkBox2.ForeColor = SystemColors.ControlText;
                checkBox3.ForeColor = SystemColors.ControlText;
                checkBox4.ForeColor = SystemColors.ControlText;
                checkBox5.ForeColor = SystemColors.ControlText;
                checkBox6.ForeColor = SystemColors.ControlText;
                checkBox7.ForeColor = SystemColors.ControlText;
                checkBox8.ForeColor = SystemColors.ControlText;
                panel2.ForeColor = SystemColors.ControlText;
                panel2.BackColor = SystemColors.Control;
                panel3.BackColor = SystemColors.Control;
                panel4.BackColor = SystemColors.Control;
                panel3.ForeColor = SystemColors.ControlText;
                panel4.ForeColor = SystemColors.ControlText;
                найтиToolStripMenuItem.BackColor = SystemColors.Control;
                найтиToolStripMenuItem.ForeColor = SystemColors.ControlText;
                заменитьToolStripMenuItem.BackColor = SystemColors.Control;
                заменитьToolStripMenuItem.ForeColor = SystemColors.ControlText;
                перейтиToolStripMenuItem.BackColor = SystemColors.Control;
                перейтиToolStripMenuItem.ForeColor = SystemColors.ControlText;
                label2.ForeColor = SystemColors.ControlText;
                label2.BackColor = SystemColors.Control;
                label14.BackColor = SystemColors.Control;
                label15.BackColor = SystemColors.Control;
                label17.BackColor = SystemColors.Control;
                label14.ForeColor = SystemColors.ControlText;
                label15.ForeColor = SystemColors.ControlText;
                label17.ForeColor = SystemColors.ControlText;
                button1.ForeColor = SystemColors.ControlText;
                button2.ForeColor = SystemColors.ControlText;
                button3.ForeColor = SystemColors.ControlText;
                button4.ForeColor = SystemColors.ControlText;
                button5.ForeColor = SystemColors.ControlText;
                button6.ForeColor = SystemColors.ControlText;
                button7.ForeColor = SystemColors.ControlText;
                button8.ForeColor = SystemColors.ControlText;
                button9.ForeColor = SystemColors.ControlText;
                button10.ForeColor = SystemColors.ControlText;
                button11.ForeColor = SystemColors.ControlText;
                richTextBox1.SelectAll();
                richTextBox1.SelectionBackColor = Properties.Settings.Default.BackColor;
                richTextBox1.SelectionColor = Properties.Settings.Default.TextColor;
                richTextBox1.Select();
                Properties.Settings.Default.isSaved = saved;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult textColor = text.ShowDialog();
            if (textColor == DialogResult.OK)
            {
                Color textSelCol = text.Color;
                if (textSelCol == Properties.Settings.Default.BackColor)
                {
                    if(Properties.Settings.Default.Lang == 1)
                    {
                        MessageBox.Show("Нельзя выбирать одинаковый цвет для текста и фона. Выберите другой цвет.", "ATXT - Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("You cannot choose the same color for text and background. Please select a different color.", "ATXT - Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
                else
                {
                    Properties.Settings.Default.TextColor = textSelCol;
                    richTextBox1.ForeColor = textSelCol;
                }

            }
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                this.TopMost = true;
                Properties.Settings.Default.isTopMost = true;
            }
            else
            {
                this.TopMost = false;
                Properties.Settings.Default.isTopMost = false;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true)
            {
                Properties.Settings.Default.RememberFont = true;
            }
            else
            {
                Properties.Settings.Default.RememberFont = false;
            }
        }

        private void FontTimer_Tick(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.RememberFont == true)
            {
                Properties.Settings.Default.Font = richTextBox1.Font;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                Properties.Settings.Default.isPercentage = true;
            }
            else
            {
                Properties.Settings.Default.isPercentage = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                Properties.Settings.Default.isBottomPanelHidden = true;
                statusStrip1.Visible = false;
                richTextBox1.Height = richTextBox1.Height + 22;
            }
            else
            {
                Properties.Settings.Default.isBottomPanelHidden = false;
                statusStrip1.Visible = true;
                richTextBox1.Height = richTextBox1.Height - 22;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                devTimer.Enabled = true;
                devTimer.Start();
                devLabel.Visible = true;
                Properties.Settings.Default.isDev = true;
            }
            else
            {
                devTimer.Enabled = false;
                devTimer.Stop();
                devLabel.Visible = false;
                Properties.Settings.Default.isDev = false;
            }
        }

        private void Theme_Tick(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Theme == 0)
            {
                richTextBox1.BackColor = SystemColors.Window;
                richTextBox1.ForeColor = SystemColors.WindowText;
                panel1.BackColor = SystemColors.Control;
                panel1.ForeColor = SystemColors.ControlText;
                menuStrip1.BackColor = SystemColors.Control;
                menuStrip1.ForeColor = SystemColors.ControlText;
                файлToolStripMenuItem.BackColor = SystemColors.Control;
                файлToolStripMenuItem.ForeColor = SystemColors.ControlText;
                создатьToolStripMenuItem.BackColor = SystemColors.Control;
                создатьToolStripMenuItem.ForeColor = SystemColors.ControlText;
                сохранитьToolStripMenuItem.BackColor = SystemColors.Control;
                сохранитьToolStripMenuItem.ForeColor = SystemColors.ControlText;
                сохранитьToolStripMenuItem1.BackColor = SystemColors.Control;
                сохранитьToolStripMenuItem1.ForeColor = SystemColors.ControlText;
                открытьToolStripMenuItem.BackColor = SystemColors.Control;
                открытьToolStripMenuItem.ForeColor = SystemColors.ControlText;
                сохранитьВANSIToolStripMenuItem.BackColor = SystemColors.Control;
                сохранитьВANSIToolStripMenuItem.ForeColor = SystemColors.ControlText;
                сохранитьКакВToolStripMenuItem.BackColor = SystemColors.Control;
                сохранитьКакВToolStripMenuItem.ForeColor = SystemColors.ControlText;
                открытьВToolStripMenuItem.BackColor = SystemColors.Control;
                открытьВToolStripMenuItem.ForeColor = SystemColors.ControlText;
                кодировкаToolStripMenuItem.BackColor = SystemColors.Control;
                кодировкаToolStripMenuItem.ForeColor = SystemColors.ControlText;
                сменитьКодировкуТекстаНаUTF8ToolStripMenuItem.BackColor = SystemColors.Control;
                сменитьКодировкуТекстаНаUTF8ToolStripMenuItem.ForeColor = SystemColors.ControlText;
                сменитьКодировкуТекстаНаANSIToolStripMenuItem.BackColor = SystemColors.Control;
                сменитьКодировкуТекстаНаANSIToolStripMenuItem.ForeColor = SystemColors.ControlText;
                шрифтToolStripMenuItem.BackColor = SystemColors.Control;
                шрифтToolStripMenuItem.ForeColor = SystemColors.ControlText;
                toolStripSeparator1.Visible = true;
                toolStripSeparator2.Visible = true;
                toolStripSeparator3.Visible = true;
                toolStripSeparator4.Visible = true;
                видToolStripMenuItem.BackColor = SystemColors.Control;
                видToolStripMenuItem.ForeColor = SystemColors.ControlText;
                SettingsTSMI.BackColor = SystemColors.Control;
                SettingsTSMI.ForeColor = SystemColors.ControlText;
                оПрограммеToolStripMenuItem.BackColor = SystemColors.Control;
                оПрограммеToolStripMenuItem.ForeColor = SystemColors.ControlText;
                AutoSaveTSMC.ForeColor = SystemColors.ControlText;
                statusStrip1.ForeColor = SystemColors.ControlText;
                statusStrip1.BackColor = SystemColors.Control;
                devLabel.ForeColor = SystemColors.ControlText;
                devLabel.BackColor = Color.White;
                label3.ForeColor = SystemColors.ControlText;
                label4.ForeColor = SystemColors.ControlText;
                label5.ForeColor = SystemColors.ControlText;
                label6.ForeColor = SystemColors.ControlText;
                label7.ForeColor = SystemColors.ControlText;
                label8.ForeColor = SystemColors.ControlText;
                label9.ForeColor = SystemColors.ControlText;
                label10.ForeColor = SystemColors.ControlText;
                label11.ForeColor = SystemColors.ControlText;
                label12.ForeColor = SystemColors.ControlText;
                checkBox1.ForeColor = SystemColors.ControlText;
                checkBox2.ForeColor = SystemColors.ControlText;
                checkBox3.ForeColor = SystemColors.ControlText;
                checkBox4.ForeColor = SystemColors.ControlText;
                checkBox5.ForeColor = SystemColors.ControlText;
                checkBox6.ForeColor = SystemColors.ControlText;
                checkBox7.ForeColor = SystemColors.ControlText;
                checkBox8.ForeColor = SystemColors.ControlText;
                panel2.ForeColor = SystemColors.ControlText;
                panel2.BackColor = SystemColors.Control;
                panel3.BackColor = SystemColors.Control;
                panel4.BackColor = SystemColors.Control;
                panel3.ForeColor = SystemColors.ControlText;
                panel4.ForeColor = SystemColors.ControlText;
                найтиToolStripMenuItem.BackColor = SystemColors.Control;
                найтиToolStripMenuItem.ForeColor = SystemColors.ControlText;
                заменитьToolStripMenuItem.BackColor = SystemColors.Control;
                заменитьToolStripMenuItem.ForeColor = SystemColors.ControlText;
                перейтиToolStripMenuItem.BackColor = SystemColors.Control;
                перейтиToolStripMenuItem.ForeColor = SystemColors.ControlText;
                label2.ForeColor = SystemColors.ControlText;
                label2.BackColor = SystemColors.Control;
                label14.BackColor = SystemColors.Control;
                label15.BackColor = SystemColors.Control;
                label17.BackColor = SystemColors.Control;
                label14.ForeColor = SystemColors.ControlText;
                label15.ForeColor = SystemColors.ControlText;
                label17.ForeColor = SystemColors.ControlText;
                button1.ForeColor = SystemColors.ControlText;
                button2.ForeColor = SystemColors.ControlText;
                button3.ForeColor = SystemColors.ControlText;
                button4.ForeColor = SystemColors.ControlText;
                button5.ForeColor = SystemColors.ControlText;
                button6.ForeColor = SystemColors.ControlText;
                button7.ForeColor = SystemColors.ControlText;
                button8.ForeColor = SystemColors.ControlText;
                button9.ForeColor = SystemColors.ControlText;
                button10.ForeColor = SystemColors.ControlText;
                button11.ForeColor = SystemColors.ControlText;
                button12.ForeColor = SystemColors.ControlText;
            }
            else if (Properties.Settings.Default.Theme == 1)
            {
                richTextBox1.BackColor = Color.Black;
                richTextBox1.ForeColor = Color.White;
                panel1.BackColor = Color.FromArgb(20, 20, 20);
                panel1.ForeColor = Color.White;
                menuStrip1.BackColor = Color.Silver;
                menuStrip1.ForeColor = SystemColors.ControlText;
                файлToolStripMenuItem.BackColor = Color.Silver;
                файлToolStripMenuItem.ForeColor = SystemColors.ControlText;
                создатьToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                создатьToolStripMenuItem.ForeColor = Color.White;
                сохранитьToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                сохранитьToolStripMenuItem.ForeColor = Color.White;
                сохранитьToolStripMenuItem1.BackColor = Color.FromArgb(20, 20, 20);
                сохранитьToolStripMenuItem1.ForeColor = Color.White;
                открытьToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                открытьToolStripMenuItem.ForeColor = Color.White;
                сохранитьВANSIToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                сохранитьВANSIToolStripMenuItem.ForeColor = Color.White;
                сохранитьКакВToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                сохранитьКакВToolStripMenuItem.ForeColor = Color.White;
                открытьВToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                открытьВToolStripMenuItem.ForeColor = Color.White;
                кодировкаToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                кодировкаToolStripMenuItem.ForeColor = Color.White;
                сменитьКодировкуТекстаНаUTF8ToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                сменитьКодировкуТекстаНаUTF8ToolStripMenuItem.ForeColor = Color.White;
                сменитьКодировкуТекстаНаANSIToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                сменитьКодировкуТекстаНаANSIToolStripMenuItem.ForeColor = Color.White;
                шрифтToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                шрифтToolStripMenuItem.ForeColor = Color.White;
                toolStripSeparator1.Visible = false;
                toolStripSeparator2.Visible = false;
                toolStripSeparator3.Visible = false;
                toolStripSeparator4.Visible = false;
                видToolStripMenuItem.BackColor = Color.Silver;
                видToolStripMenuItem.ForeColor = SystemColors.ControlText;
                SettingsTSMI.BackColor = Color.Silver;
                SettingsTSMI.ForeColor = SystemColors.ControlText;
                оПрограммеToolStripMenuItem.BackColor = Color.Silver;
                оПрограммеToolStripMenuItem.ForeColor = SystemColors.ControlText;
                AutoSaveTSMC.ForeColor = SystemColors.ControlText;
                statusStrip1.ForeColor = Color.White;
                statusStrip1.BackColor = Color.FromArgb(20, 20, 20);
                devLabel.ForeColor = Color.White;
                devLabel.BackColor = Color.Black;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                label5.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label7.ForeColor = Color.White;
                label8.ForeColor = Color.White;
                label9.ForeColor = Color.White;
                label10.ForeColor = Color.White;
                label11.ForeColor = Color.White;
                label12.ForeColor = Color.White;
                checkBox1.ForeColor = Color.White;
                checkBox2.ForeColor = Color.White;
                checkBox3.ForeColor = Color.White;
                checkBox4.ForeColor = Color.White;
                checkBox5.ForeColor = Color.White;
                checkBox6.ForeColor = Color.White;
                checkBox7.ForeColor = Color.White;
                checkBox8.ForeColor = Color.White;
                panel2.ForeColor = Color.White;
                panel2.BackColor = Color.FromArgb(20, 20, 20);
                panel3.BackColor = Color.FromArgb(20, 20, 20);
                panel4.BackColor = Color.FromArgb(20, 20, 20);
                panel3.ForeColor = Color.White;
                panel4.ForeColor = Color.White;
                найтиToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                найтиToolStripMenuItem.ForeColor = Color.White;
                заменитьToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                заменитьToolStripMenuItem.ForeColor = Color.White;
                перейтиToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                перейтиToolStripMenuItem.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label2.BackColor = Color.FromArgb(20, 20, 20);
                label14.BackColor = Color.FromArgb(20, 20, 20);
                label15.BackColor = Color.FromArgb(20, 20, 20);
                label17.BackColor = Color.FromArgb(20, 20, 20);
                label14.ForeColor = Color.White;
                label15.ForeColor = Color.White;
                label17.ForeColor = Color.White;
                button1.ForeColor = SystemColors.ControlText;
                button2.ForeColor = SystemColors.ControlText;
                button3.ForeColor = SystemColors.ControlText;
                button4.ForeColor = SystemColors.ControlText;
                button5.ForeColor = SystemColors.ControlText;
                button6.ForeColor = SystemColors.ControlText;
                button7.ForeColor = SystemColors.ControlText;
                button8.ForeColor = SystemColors.ControlText;
                button9.ForeColor = SystemColors.ControlText;
                button10.ForeColor = SystemColors.ControlText;
                button11.ForeColor = SystemColors.ControlText;
                button12.ForeColor = SystemColors.ControlText;
            }
            else
            {
                richTextBox1.BackColor = Properties.Settings.Default.BackColor;
                richTextBox1.ForeColor = Properties.Settings.Default.TextColor;
                if (Properties.Settings.Default.BottomPanelColor == 0)
                {
                    statusStrip1.ForeColor = SystemColors.ControlText;
                    statusStrip1.BackColor = SystemColors.Control;
                    panel2.ForeColor = SystemColors.ControlText;
                    panel2.BackColor = SystemColors.Control;
                    panel3.BackColor = SystemColors.Control;
                    panel4.BackColor = SystemColors.Control;
                    panel3.ForeColor = SystemColors.ControlText;
                    panel4.ForeColor = SystemColors.ControlText;
                    найтиToolStripMenuItem.BackColor = SystemColors.Control;
                    найтиToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    заменитьToolStripMenuItem.BackColor = SystemColors.Control;
                    заменитьToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    перейтиToolStripMenuItem.BackColor = SystemColors.Control;
                    перейтиToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    label2.ForeColor = SystemColors.ControlText;
                    label2.BackColor = SystemColors.Control;
                    label14.BackColor = SystemColors.Control;
                    label15.BackColor = SystemColors.Control;
                    label17.BackColor = SystemColors.Control;
                    label14.ForeColor = SystemColors.ControlText;
                    label15.ForeColor = SystemColors.ControlText;
                    label17.ForeColor = SystemColors.ControlText;
                    button1.ForeColor = SystemColors.ControlText;
                    button2.ForeColor = SystemColors.ControlText;
                    button3.ForeColor = SystemColors.ControlText;
                    button4.ForeColor = SystemColors.ControlText;
                    button5.ForeColor = SystemColors.ControlText;
                    button6.ForeColor = SystemColors.ControlText;
                    button7.ForeColor = SystemColors.ControlText;
                    button8.ForeColor = SystemColors.ControlText;
                    button9.ForeColor = SystemColors.ControlText;
                    button10.ForeColor = SystemColors.ControlText;
                    button11.ForeColor = SystemColors.ControlText;
                    button12.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    statusStrip1.ForeColor = Color.White;
                    statusStrip1.BackColor = Color.FromArgb(20, 20, 20);
                    panel2.ForeColor = Color.White;
                    panel2.BackColor = Color.FromArgb(20, 20, 20);
                    panel3.BackColor = Color.FromArgb(20, 20, 20);
                    panel4.BackColor = Color.FromArgb(20, 20, 20);
                    panel3.ForeColor = Color.White;
                    panel4.ForeColor = Color.White;
                    найтиToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                    найтиToolStripMenuItem.ForeColor = Color.White;
                    заменитьToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                    заменитьToolStripMenuItem.ForeColor = Color.White;
                    перейтиToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                    перейтиToolStripMenuItem.ForeColor = Color.White;
                    label2.ForeColor = Color.White;
                    label2.BackColor = Color.FromArgb(20, 20, 20);
                    label14.BackColor = Color.FromArgb(20, 20, 20);
                    label15.BackColor = Color.FromArgb(20, 20, 20);
                    label17.BackColor = Color.FromArgb(20, 20, 20);
                    label14.ForeColor = Color.White;
                    label15.ForeColor = Color.White;
                    label17.ForeColor = Color.White;
                    button1.ForeColor = SystemColors.ControlText;
                    button2.ForeColor = SystemColors.ControlText;
                    button3.ForeColor = SystemColors.ControlText;
                    button4.ForeColor = SystemColors.ControlText;
                    button5.ForeColor = SystemColors.ControlText;
                    button6.ForeColor = SystemColors.ControlText;
                    button7.ForeColor = SystemColors.ControlText;
                    button8.ForeColor = SystemColors.ControlText;
                    button9.ForeColor = SystemColors.ControlText;
                    button10.ForeColor = SystemColors.ControlText;
                    button11.ForeColor = SystemColors.ControlText;
                    button12.ForeColor = SystemColors.ControlText;
                }
                if (Properties.Settings.Default.TopPanelColor == 0)
                {
                    menuStrip1.BackColor = SystemColors.Control;
                    menuStrip1.ForeColor = SystemColors.ControlText;
                    файлToolStripMenuItem.BackColor = SystemColors.Control;
                    файлToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    видToolStripMenuItem.BackColor = SystemColors.Control;
                    видToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    SettingsTSMI.BackColor = SystemColors.Control;
                    SettingsTSMI.ForeColor = SystemColors.ControlText;
                    оПрограммеToolStripMenuItem.BackColor = SystemColors.Control;
                    оПрограммеToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    AutoSaveTSMC.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    menuStrip1.BackColor = Color.Silver;
                    menuStrip1.ForeColor = SystemColors.ControlText;
                    файлToolStripMenuItem.BackColor = Color.Silver;
                    файлToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    видToolStripMenuItem.BackColor = Color.Silver;
                    видToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    SettingsTSMI.BackColor = Color.Silver;
                    SettingsTSMI.ForeColor = SystemColors.ControlText;
                    оПрограммеToolStripMenuItem.BackColor = Color.Silver;
                    оПрограммеToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    AutoSaveTSMC.ForeColor = SystemColors.ControlText;

                }
                if (Properties.Settings.Default.ContextMenuColor == 0)
                {
                    создатьToolStripMenuItem.BackColor = SystemColors.Control;
                    создатьToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    сохранитьToolStripMenuItem.BackColor = SystemColors.Control;
                    сохранитьToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    сохранитьToolStripMenuItem1.BackColor = SystemColors.Control;
                    сохранитьToolStripMenuItem1.ForeColor = SystemColors.ControlText;
                    открытьToolStripMenuItem.BackColor = SystemColors.Control;
                    открытьToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    сохранитьВANSIToolStripMenuItem.BackColor = SystemColors.Control;
                    сохранитьВANSIToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    сохранитьКакВToolStripMenuItem.BackColor = SystemColors.Control;
                    сохранитьКакВToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    открытьВToolStripMenuItem.BackColor = SystemColors.Control;
                    открытьВToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    кодировкаToolStripMenuItem.BackColor = SystemColors.Control;
                    кодировкаToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    сменитьКодировкуТекстаНаUTF8ToolStripMenuItem.BackColor = SystemColors.Control;
                    сменитьКодировкуТекстаНаUTF8ToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    сменитьКодировкуТекстаНаANSIToolStripMenuItem.BackColor = SystemColors.Control;
                    сменитьКодировкуТекстаНаANSIToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    шрифтToolStripMenuItem.BackColor = SystemColors.Control;
                    шрифтToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    toolStripSeparator1.Visible = true;
                    toolStripSeparator2.Visible = true;
                    toolStripSeparator3.Visible = true;
                    toolStripSeparator4.Visible = true;
                }
                else
                {
                    создатьToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                    создатьToolStripMenuItem.ForeColor = Color.White;
                    сохранитьToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                    сохранитьToolStripMenuItem.ForeColor = Color.White;
                    сохранитьToolStripMenuItem1.BackColor = Color.FromArgb(20, 20, 20);
                    сохранитьToolStripMenuItem1.ForeColor = Color.White;
                    открытьToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                    открытьToolStripMenuItem.ForeColor = Color.White;
                    сохранитьВANSIToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                    сохранитьВANSIToolStripMenuItem.ForeColor = Color.White;
                    сохранитьКакВToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                    сохранитьКакВToolStripMenuItem.ForeColor = Color.White;
                    открытьВToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                    открытьВToolStripMenuItem.ForeColor = Color.White;
                    кодировкаToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                    кодировкаToolStripMenuItem.ForeColor = Color.White;
                    сменитьКодировкуТекстаНаUTF8ToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                    сменитьКодировкуТекстаНаUTF8ToolStripMenuItem.ForeColor = Color.White;
                    сменитьКодировкуТекстаНаANSIToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                    сменитьКодировкуТекстаНаANSIToolStripMenuItem.ForeColor = Color.White;
                    шрифтToolStripMenuItem.BackColor = Color.FromArgb(20, 20, 20);
                    шрифтToolStripMenuItem.ForeColor = Color.White;
                    toolStripSeparator1.Visible = false;
                    toolStripSeparator2.Visible = false;
                    toolStripSeparator3.Visible = false;
                    toolStripSeparator4.Visible = false;
                }
                if (Properties.Settings.Default.SettingsColor == 0)
                {
                    panel1.BackColor = SystemColors.Control;
                    panel1.ForeColor = SystemColors.ControlText;
                    devLabel.ForeColor = SystemColors.ControlText;
                    devLabel.BackColor = Color.White;
                    label3.ForeColor = SystemColors.ControlText;
                    label4.ForeColor = SystemColors.ControlText;
                    label5.ForeColor = SystemColors.ControlText;
                    label6.ForeColor = SystemColors.ControlText;
                    label7.ForeColor = SystemColors.ControlText;
                    label8.ForeColor = SystemColors.ControlText;
                    label9.ForeColor = SystemColors.ControlText;
                    label10.ForeColor = SystemColors.ControlText;
                    label11.ForeColor = SystemColors.ControlText;
                    label12.ForeColor = SystemColors.ControlText;
                    checkBox1.ForeColor = SystemColors.ControlText;
                    checkBox2.ForeColor = SystemColors.ControlText;
                    checkBox3.ForeColor = SystemColors.ControlText;
                    checkBox4.ForeColor = SystemColors.ControlText;
                    checkBox5.ForeColor = SystemColors.ControlText;
                    checkBox6.ForeColor = SystemColors.ControlText;
                    checkBox7.ForeColor = SystemColors.ControlText;
                    checkBox8.ForeColor = SystemColors.ControlText;
                    button1.ForeColor = Color.Black;
                    button2.ForeColor = Color.Black;

                }
                else
                {
                    panel1.BackColor = Color.FromArgb(20, 20, 20);
                    panel1.ForeColor = Color.White;
                    devLabel.ForeColor = Color.White;
                    devLabel.BackColor = Color.Black;
                    label3.ForeColor = Color.White;
                    label4.ForeColor = Color.White;
                    label5.ForeColor = Color.White;
                    label6.ForeColor = Color.White;
                    label7.ForeColor = Color.White;
                    label8.ForeColor = Color.White;
                    label9.ForeColor = Color.White;
                    label10.ForeColor = Color.White;
                    label11.ForeColor = Color.White;
                    label12.ForeColor = Color.White;
                    checkBox1.ForeColor = Color.White;
                    checkBox2.ForeColor = Color.White;
                    checkBox3.ForeColor = Color.White;
                    checkBox4.ForeColor = Color.White;
                    checkBox5.ForeColor = Color.White;
                    checkBox6.ForeColor = Color.White;
                    checkBox7.ForeColor = Color.White;
                    checkBox8.ForeColor = Color.White;
                    button1.ForeColor = Color.Black;
                    button2.ForeColor = Color.Black;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult backColor = text.ShowDialog();
            if (backColor == DialogResult.OK)
            {
                Color backSelCol = text.Color;
                if (backSelCol == Properties.Settings.Default.TextColor)
                {
                    if (Properties.Settings.Default.Lang == 1)
                    {
                        MessageBox.Show("Нельзя выбирать одинаковый цвет для текста и фона. Выберите другой цвет.", "ATXT - Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("You cannot choose the same color for text and background. Please select a different color.", "ATXT - Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
                else
                {
                    Properties.Settings.Default.BackColor = backSelCol;
                    richTextBox1.BackColor = backSelCol;
                    richTextBox1.SelectAll();
                    richTextBox1.SelectionBackColor = backSelCol;
                    richTextBox1.Select();
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                Properties.Settings.Default.BottomPanelColor = 0;
            }
            else
            {
                Properties.Settings.Default.BottomPanelColor = 1;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                Properties.Settings.Default.TopPanelColor = 0;
            }
            else
            {
                Properties.Settings.Default.TopPanelColor = 1;
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == 0)
            {
                Properties.Settings.Default.ContextMenuColor = 0;
            }
            else
            {
                Properties.Settings.Default.ContextMenuColor = 1;
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex == 0)
            {
                Properties.Settings.Default.SettingsColor = 0;
            }
            else
            {
                Properties.Settings.Default.SettingsColor = 1;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (trackBar1.Value == 0)
            {
                this.Opacity = 0.25;
                Properties.Settings.Default.Opacity = 0;
            }
            else if (trackBar1.Value == 1)
            {
                this.Opacity = 0.3;
                Properties.Settings.Default.Opacity = 1;
            }
            else if (trackBar1.Value == 2)
            {
                this.Opacity = 0.35;
                Properties.Settings.Default.Opacity = 2;
            }
            else if (trackBar1.Value == 3)
            {
                this.Opacity = 0.4;
                Properties.Settings.Default.Opacity = 3;
            }
            else if (trackBar1.Value == 4)
            {
                this.Opacity = 0.45;
                Properties.Settings.Default.Opacity = 4;
            }
            else if (trackBar1.Value == 5)
            {
                this.Opacity = 0.5;
                Properties.Settings.Default.Opacity = 5;
            }
            else if (trackBar1.Value == 6)
            {
                this.Opacity = 0.55;
                Properties.Settings.Default.Opacity = 6;
            }
            else if (trackBar1.Value == 7)
            {
                this.Opacity = 0.6;
                Properties.Settings.Default.Opacity = 7;
            }
            else if (trackBar1.Value == 8)
            {
                this.Opacity = 0.65;
                Properties.Settings.Default.Opacity = 8;
            }
            else if (trackBar1.Value == 9)
            {
                this.Opacity = 0.7;
                Properties.Settings.Default.Opacity = 9;
            }
            else if (trackBar1.Value == 10)
            {
                this.Opacity = 0.75;
                Properties.Settings.Default.Opacity = 10;
            }
            else if (trackBar1.Value == 11)
            {
                this.Opacity = 0.8;
                Properties.Settings.Default.Opacity = 11;
            }
            else if (trackBar1.Value == 12)
            {
                this.Opacity = 0.85;
                Properties.Settings.Default.Opacity = 12;
            }
            else if (trackBar1.Value == 13)
            {
                this.Opacity = 0.9;
                Properties.Settings.Default.Opacity = 13;
            }
            else if (trackBar1.Value == 14)
            {
                this.Opacity = 0.95;
                Properties.Settings.Default.Opacity = 14;
            }
            else if (trackBar1.Value == 15)
            {
                this.Opacity = 1;
                Properties.Settings.Default.Opacity = 15;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string searchText = this.find.Text;
            int startPos = 0;
            int count = 0;
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = richTextBox1.BackColor;

            while (startPos < richTextBox1.TextLength)
            {
                int index = richTextBox1.Find(searchText, startPos, richTextBox1.TextLength, RichTextBoxFinds.None);

                if (index != -1)
                {
                    // Помещаем курсор в найденное место
                    richTextBox1.SelectionStart = index;
                    // Выделяем найденный текст
                    richTextBox1.SelectionBackColor = Color.DodgerBlue;
                    // Прокручиваем текст, чтобы он был виден
                    richTextBox1.ScrollToCaret();
                    // Увеличиваем счетчик вхождений
                    count++;
                    // Устанавливаем новую стартовую позицию для поиска
                    startPos = index + searchText.Length;
                }
                else
                {
                    // Если ничего не найдено, выходим из цикла
                    break;
                }
            }

            label13.Text = $"Найдено совпадений: {count}";
            if (count == 0)
            {
                label13.Text = "Не найдено ни одного совпадения.";
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void find_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            find.Text = "";
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = richTextBox1.BackColor;
        }

        private void найтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (panel2.Visible == false)
            {
                panel4.Visible = false;
                panel3.Visible = false;
                panel2.Visible = true;
                if(Properties.Settings.Default.Lang == 1)
                {
                    find.Text = "Учитывайте регистр и символы!";
                }
                else
                {
                    find.Text = "Consider case and symbols!";
                }
                int currentPosition = richTextBox1.SelectionStart;
                richTextBox1.SelectAll();
                richTextBox1.SelectionBackColor = richTextBox1.BackColor;
                richTextBox1.SelectionStart = currentPosition;
            }
            else
            {
                panel2.Visible = false;
                if (Properties.Settings.Default.Lang == 1)
                {
                    find.Text = "Учитывайте регистр и символы!";
                }
                else
                {
                    find.Text = "Consider case and symbols!";
                }
                int currentPosition = richTextBox1.SelectionStart;
                richTextBox1.SelectAll();
                richTextBox1.SelectionBackColor = richTextBox1.BackColor;
                richTextBox1.SelectionStart = currentPosition;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string searchText = find.Text;
            int startPos = currentSearchIndex; // Используем текущую позицию поиска как стартовую
            richTextBox1.SelectionBackColor = richTextBox1.BackColor;

            int index = richTextBox1.Find(searchText, startPos, richTextBox1.TextLength, RichTextBoxFinds.None);

            if (index != -1)
            {
                // Помещаем курсор в найденное место
                richTextBox1.SelectionStart = index;
                // Выделяем найденный текст
                richTextBox1.SelectionBackColor = Color.DodgerBlue;
                // Прокручиваем текст, чтобы он был виден
                richTextBox1.ScrollToCaret();
                // Обновляем текущую позицию поиска
                currentSearchIndex = index + searchText.Length;
                if (Properties.Settings.Default.Lang == 1)
                {
                    label13.Text = "Совпадение найдено!";
                }
                else
                {
                    label13.Text = "Match found!";
                }
            }
            else
            {
                if (Properties.Settings.Default.Lang == 1)
                {
                    label13.Text = "Совпадений не найдено.";
                }
                else
                {
                    label13.Text = "No matches found.";
                }
                currentSearchIndex = 0;
            }
        }

        private void видToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string searchText = what.Text;
            string replaceText = replace.Text;
            int startPos = 0;
            int count = 0;
            richTextBox1.SelectionBackColor = richTextBox1.BackColor;
            if (!string.IsNullOrEmpty(searchText))
            {
                while (startPos < richTextBox1.TextLength)
                {
                    int index = richTextBox1.Find(searchText, startPos, richTextBox1.TextLength, RichTextBoxFinds.None);

                    if (index != -1)
                    {
                        // Помещаем курсор в найденное место
                        richTextBox1.SelectionStart = index;
                        // Выделяем найденный текст
                        richTextBox1.SelectionBackColor = Color.LimeGreen;
                        // Заменяем выделенный текст на новый
                        richTextBox1.SelectedText = replaceText;
                        // Увеличиваем счетчик вхождений
                        count++;
                        // Устанавливаем новую стартовую позицию для поиска
                        startPos = index + replaceText.Length;
                    }
                    else
                    {
                        if (Properties.Settings.Default.Lang == 1)
                        {
                            label14.Text = "Не найдено ни одного совпадения.";
                        }
                        else
                        {
                            label14.Text = "No matches found.";
                        }
                        // Если ничего не найдено, выходим из цикла
                        break;
                    }
                }

                if (count != 0)
                {
                    if (Properties.Settings.Default.Lang == 1)
                    {
                        label14.Text = $"Заменено совпадений: {count}";
                    }
                    else
                    {
                        label14.Text = $"Matches replaced: {count}";
                    }
                    // Показываем количество вхождений
                }
                else
                {
                    if (Properties.Settings.Default.Lang == 1)
                    {
                        label14.Text = "Не найдено ни одного совпадения.";
                    }
                    else
                    {
                        label14.Text = "No matches found.";
                    }
                }
            }
            else
            {
                if (Properties.Settings.Default.Lang == 1)
                {
                    what.Text = "Укажите, что заменять!";
                }
                else
                {
                    what.Text = "Type what to replace!";
                }
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            what.Text = "";
            replace.Text = "";
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = richTextBox1.BackColor;
            if (Properties.Settings.Default.Lang == 1)
            {
                label13.Text = "Заменено совпадений: --";
            }
            else
            {
                label13.Text = "Matches replaced: --";
            }
        }

        private void заменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (panel3.Visible == false)
            {
                panel2.Visible = false;
                panel4.Visible = false;
                panel3.Visible = true;
                if (Properties.Settings.Default.Lang == 1)
                {
                    what.Text = "Что заменить...";
                    replace.Text = "На что заменить...";
                }
                else
                {
                    what.Text = "What replace...";
                    replace.Text = "Replace to...";
                }
                int currentPosition = richTextBox1.SelectionStart;
                richTextBox1.SelectAll();
                richTextBox1.SelectionBackColor = richTextBox1.BackColor;
                richTextBox1.SelectionStart = currentPosition;
            }
            else
            {
                panel3.Visible = false;
                if (Properties.Settings.Default.Lang == 1)
                {
                    what.Text = "Что заменить...";
                    replace.Text = "На что заменить...";
                }
                else
                {
                    what.Text = "What replace...";
                    replace.Text = "Replace to...";
                }
                int currentPosition = richTextBox1.SelectionStart;
                richTextBox1.SelectAll();
                richTextBox1.SelectionBackColor = richTextBox1.BackColor;
                richTextBox1.SelectionStart = currentPosition;
            }
        }
        private void ReplaceNext(string searchText, string replaceText)
        {
            string text = this.what.Text;
            int textLength = this.richTextBox1.TextLength;
            if (this.start < textLength)
            {
                this.start = this.richTextBox1.Find(text, this.start, this.richTextBox1.Text.Length, RichTextBoxFinds.MatchCase) + 1;
                this.richTextBox1.ScrollToCaret();
                richTextBox1.SelectionBackColor = Color.LimeGreen;
                this.richTextBox1.SelectedText = this.replace.Text;
                this.richTextBox1.Refresh();
                if (Properties.Settings.Default.Lang == 1)
                {
                    label14.Text = "Совпадение найдено и заменено!";
                }
                else
                {
                    label13.Text = "Match found and replaced!";
                }
            }
            if(this.start == 0)
            {
                richTextBox1.Undo();
                if (Properties.Settings.Default.Lang == 1)
                {
                    label14.Text = "Совпадений не найдено.";
                }
                else
                {
                    label13.Text = "No matches found.";
                }
            }
        }

            // Обработчик события нажатия кнопки "Заменить следующее"
            private void ReplaceNextButton_Click(object sender, EventArgs e)
            {
                string searchText = what.Text;
                string replaceText = replace.Text;
                richTextBox1.SelectionBackColor = richTextBox1.BackColor;

                // Если поле поиска не пустое
                if (!string.IsNullOrEmpty(searchText))
                {
                    ReplaceNext(searchText, replaceText);
                }
                else
                {
                if (Properties.Settings.Default.Lang == 1)
                {
                    what.Text = "Укажите, что заменять!";
                }
                else
                {
                    what.Text = "Type what to replace!";
                }
            }
            }

            private void panel3_Paint(object sender, PaintEventArgs e)
            {

            }

        private void button11_Click(object sender, EventArgs e)
        {
            int allSymbol = richTextBox1.Text.Length;
            string numberString = this.number.Text;
            int number;
            bool intparse = int.TryParse(numberString, out number);
            if (intparse)
            {
                if(number < 0)
                {
                    if (Properties.Settings.Default.Lang == 1)
                    {
                        this.number.Text = "Введите число не меньше 0!";
                    }
                    else
                    {
                        this.number.Text = "Enter a number no less than 0!";
                    }
                }
                else if (number <= allSymbol)
                {
                    richTextBox1.SelectionStart = number;
                    richTextBox1.Select();
                    this.richTextBox1.ScrollToCaret();
                }
                else
                {
                    richTextBox1.SelectionStart = allSymbol;
                    richTextBox1.Select();
                    this.richTextBox1.ScrollToCaret();
                    this.number.Text = $"{allSymbol}";
                }
            }
            else
            {
                if (Properties.Settings.Default.Lang == 1)
                {
                    this.number.Text = "Введите целое число!";
                }
                else
                {
                    this.number.Text = "Enter an integer!";
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int allSymbol = richTextBox1.Text.Length;
            int lineCount = richTextBox1.GetLineFromCharIndex(allSymbol);
            string numberString = this.number.Text;
            int number;
            bool intparse = int.TryParse(numberString, out number);
            if (intparse)
            {
                if (number <= 0)
                {
                    if (Properties.Settings.Default.Lang == 1)
                    {
                        this.number.Text = "Введите число не меньше 1!";
                    }
                    else
                    {
                        this.number.Text = "Enter a number no less than 1!";
                    }
                }
                else if (number <= lineCount)
                {
                    int line = richTextBox1.GetFirstCharIndexFromLine(number - 1);
                    richTextBox1.SelectionStart = line;
                    richTextBox1.Select();
                    this.richTextBox1.ScrollToCaret();
                }
                else
                {
                    int line = richTextBox1.GetFirstCharIndexFromLine(lineCount);
                    richTextBox1.SelectionStart = line;
                    richTextBox1.Select();
                    this.richTextBox1.ScrollToCaret();
                    this.number.Text = $"{lineCount + 1}";
                }
            }
            else
            {
                if (Properties.Settings.Default.Lang == 1)
                {
                    this.number.Text = "Введите целое число!";
                }
                else
                {
                    this.number.Text = "Enter an integer!";
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.number.Text = "";
        }

        private void перейтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(panel4.Visible == true)
            {
                panel4.Visible = false;
                if (Properties.Settings.Default.Lang == 1)
                {
                    number.Text = "Введите номер символа или строки...";
                }
                else
                {
                    number.Text = "Enter the symbol or line number...";
                }
            }
            else
            {
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = true;
                if (Properties.Settings.Default.Lang == 1)
                {
                    number.Text = "Введите номер символа или строки...";
                }
                else
                {
                    number.Text = "Enter the symbol or line number...";
                }
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox9.Checked == true)
            {
                Properties.Settings.Default.isCompact = true;
                Properties.Settings.Default.isPercentage = false;
                checkBox6.Checked = false;
                checkBox6.Enabled = false;
            }
            else
            {
                Properties.Settings.Default.isCompact = false;
                checkBox6.Enabled = true;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Lang == 1)
            {
                DialogResult isSaved = MessageBox.Show("Параметр isSaved отвечает за отображение напоминаний о несохранённом файле. При смене темы и других действиях, данный параметр может ошибочно показывать сохранённый файл за несохранённый. Эта кнопка сбрасывает этот параметр до состояния 'файл сохранён'. Данный параметр не работает в новых файлах.\n\nЖелаете продолжить?", "ATXT - Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (isSaved == DialogResult.Yes)
                {
                    Settings.Default.isSaved = true;
                    string fileName = Path.GetFileName(openedFileName);
                    this.Text = $"Awesome Notepad - {fileName}";
                }
                else
                {

                }
            }
            else
            {
                DialogResult isSaved = MessageBox.Show("The 'isSaved' parameter controls reminders about unsaved files. When changing themes and other actions, this parameter may mistakenly indicate an unsaved file as saved. This button resets this parameter to 'file saved' state. This parameter does not work on new files.\n\nDo you want to continue?", "ATXT - Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (isSaved == DialogResult.Yes)
                {
                    Properties.Settings.Default.isSaved = true;
                    string fileName = Path.GetFileName(openedFileName);
                    this.Text = $"Awesome Notepad - {fileName}";
                }
                else
                {
                    // Handle No case or leave empty if no action is needed
                }

            }
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox6.SelectedIndex == 0)
            {
                Properties.Settings.Default.Lang = 1;
                comboBox1.Items[0] = "Светлая";
                comboBox1.Items[1] = "Тёмная";
                comboBox1.Items[2] = "Другая";
                comboBox2.Items[0] = "Светлая";
                comboBox2.Items[1] = "Тёмная";
                comboBox3.Items[0] = "Светлая";
                comboBox3.Items[1] = "Тёмная";
                comboBox4.Items[0] = "Светлая";
                comboBox4.Items[1] = "Тёмная";
                comboBox5.Items[0] = "Светлая";
                comboBox5.Items[1] = "Тёмная";
            }
            else
            {
                Properties.Settings.Default.Lang = 2;
                comboBox1.Items[0] = "Light";
                comboBox1.Items[1] = "Dark";
                comboBox1.Items[2] = "Other";
                comboBox2.Items[0] = "Light";
                comboBox2.Items[1] = "Dark";
                comboBox3.Items[0] = "Light";
                comboBox3.Items[1] = "Dark";
                comboBox4.Items[0] = "Light";
                comboBox4.Items[1] = "Dark";
                comboBox5.Items[0] = "Light";
                comboBox5.Items[1] = "Dark";
            }
        }

        private void LangT_Tick(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Lang == 1)
            {
                label2.Text = "Найти...";
                label3.Text = "Настройки";
                label4.Text = "Основные";
                label5.Text = "Внешний вид";
                label6.Text = "Тема";
                label7.Text = "Предустановки";
                label8.Text = "Нижняя панель";
                label9.Text = "Верхняя панель";
                label10.Text = "Контекстное меню";
                label11.Text = "Настройки";
                label12.Text = "Прозрачность окна";
                label13.Text = "Найдено совпадений: --";
                label14.Text = "Заменено совпадений: --";
                label15.Text = "Заменить...";
                label16.Text = "Язык / Language";
                label17.Text = "Перейти...";
                файлToolStripMenuItem.Text = "Файл";
                создатьToolStripMenuItem.Text = "Создать";
                открытьToolStripMenuItem.Text = "Открыть в UTF-8...";
                сохранитьToolStripMenuItem1.Text = "Сохранить в UTF-8";
                сохранитьToolStripMenuItem.Text = "Сохранить как в UTF-8...";
                сохранитьВANSIToolStripMenuItem.Text = "Сохранить в ANSI";
                сохранитьКакВToolStripMenuItem.Text = "Сохранить как в ANSI...";
                открытьВToolStripMenuItem.Text = "Открыть в ANSI...";
                кодировкаToolStripMenuItem.Text = "Кодировка";
                сменитьКодировкуТекстаНаUTF8ToolStripMenuItem.Text = "Сменить кодировку текста на UTF-8";
                сменитьКодировкуТекстаНаANSIToolStripMenuItem.Text = "Сменить кодировку текста на ANSI";
                видToolStripMenuItem.Text = "Инструменты";
                шрифтToolStripMenuItem.Text = "Шрифт";
                найтиToolStripMenuItem.Text = "Найти...";
                заменитьToolStripMenuItem.Text = "Заменить...";
                перейтиToolStripMenuItem.Text = "Перейти...";
                SettingsTSMI.Text = "Настройки";
                оПрограммеToolStripMenuItem.Text = "О программе";
                AutoSaveTSMC.Text = " Авто-сохранение";
                button1.Text = "Выбрать цвет текста";
                button2.Text = "Выбрать цвет фона";
                button3.Text = "Всё";
                button4.Text = "С начала >";
                button7.Text = "С начала >";
                button8.Text = "Всё";
                button10.Text = "К строке";
                button11.Text = "К символу";
                button12.Text = "Сбросить параметр isSaved";
                checkBox1.Text = "Автоматически переходить на новую строку";
                checkBox2.Text = "Не показывать предупреждения при закрытии\nпрограммы";
                checkBox3.Text = "Выбирать целые слова при выделении";
                checkBox4.Text = "Отображать блокнот поверх всех окон";
                checkBox5.Text = "Скрыть нижнюю панель";
                checkBox6.Text = "Показывать процент пройденного текста";
                checkBox7.Text = "Показывать отладочную информацию";
                checkBox8.Text = "Запоминать выбранный шрифт";
                checkBox9.Text = "Компактная нижняя панель";

            }
            else if(Properties.Settings.Default.Lang == 2)
            {
                label2.Text = "Find...";
                label3.Text = "Settings";
                label4.Text = "General";
                label5.Text = "Appearance";
                label6.Text = "Theme";
                label7.Text = "Presets";
                label8.Text = "Bottom bar";
                label9.Text = "Top bar";
                label10.Text = "Context menu";
                label11.Text = "Settings";
                label12.Text = "Window transparency";
                label13.Text = "Matches found: --";
                label14.Text = "Replaced matches: --";
                label15.Text = "Replace...";
                label16.Text = "Язык / Language";
                label17.Text = "Jump to...";
                файлToolStripMenuItem.Text = "File";
                создатьToolStripMenuItem.Text = "Create";
                открытьToolStripMenuItem.Text = "Open in UTF-8...";
                сохранитьToolStripMenuItem1.Text = "Save in UTF-8";
                сохранитьToolStripMenuItem.Text = "Save as in UTF-8...";
                сохранитьВANSIToolStripMenuItem.Text = "Save in ANSI";
                сохранитьКакВToolStripMenuItem.Text = "Save as in ANSI...";
                открытьВToolStripMenuItem.Text = "Open in ANSI...";
                кодировкаToolStripMenuItem.Text = "Encoding";
                сменитьКодировкуТекстаНаUTF8ToolStripMenuItem.Text = "Change text encoding to UTF-8";
                сменитьКодировкуТекстаНаANSIToolStripMenuItem.Text = "Change text encoding to ANSI";
                видToolStripMenuItem.Text = "Tools";
                шрифтToolStripMenuItem.Text = "Font";
                найтиToolStripMenuItem.Text = "Find...";
                заменитьToolStripMenuItem.Text = "Replace...";
                перейтиToolStripMenuItem.Text = "Jump to...";
                SettingsTSMI.Text = "Settings";
                оПрограммеToolStripMenuItem.Text = "About";
                AutoSaveTSMC.Text = " Auto-save";
                button1.Text = "Select text color";
                button2.Text = "Select background color";
                button3.Text = "All";
                button4.Text = "From start";
                button7.Text = "From start";
                button8.Text = "All";
                button10.Text = "To line";
                button11.Text = "To symbol";
                button12.Text = "Reset isSaved setting";
                checkBox1.Text = "Automatically move to new line";
                checkBox2.Text = "Do not show warnings when closing the \nprogram";
                checkBox3.Text = "Select whole words";
                checkBox4.Text = "Show notepad on top of all windows";
                checkBox5.Text = "Hide bottom panel";
                checkBox6.Text = "Show percentage of text completed";
                checkBox7.Text = "Show debug information";
                checkBox8.Text = "Remember selected font";
                checkBox9.Text = "Compact bottom bar";
            }
        }

        private void what_TextChanged(object sender, EventArgs e)
        {

        }
    }
}


