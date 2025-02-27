using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Launcher
{
    public partial class FormMain : Form
    {
        string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        public FormMain()
        {
            InitializeComponent();
        }

        void button1_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Path.Combine(path, "Triggers")))
            {
                foreach (string item in Directory.EnumerateFiles(Path.Combine(path, "Triggers"), "*.scr"))
                {
                    List<string> list = new List<string>(File.ReadAllLines(item, Encoding.GetEncoding("Windows-1251")));
                    int count = list.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (string.IsNullOrEmpty(list[i]))
                        {
                            continue;
                        }
                        string text1 = list[i].Replace(" ", "").Replace("\t", "");
                        if (!text1.StartsWith("soldiers=") || !text1.EndsWith(";"))
                        {
                            continue;
                        }
                        string text2 = list[i + 1].Replace(" ", "").Replace("\t", "");
                        string text3 = list[i + 2].Replace(" ", "").Replace("\t", "");
                        if (text2.StartsWith("officers=") && text2.EndsWith(";") && text3.StartsWith("technics=") && text3.EndsWith(";"))
                        {
                            int num1 = stringToInt(text1.Replace("soldiers=", "").Replace(";", ""));
                            int num2 = stringToInt(text2.Replace("officers=", "").Replace(";", ""));
                            int num3 = stringToInt(text3.Replace("technics=", "").Replace(";", ""));
                            list[i] = list[i].Remove(list[i].IndexOf("=")) + "= " + (num1 + num2 + num3) + ";";
                            list[i + 1] = list[i + 1].Remove(list[i + 1].IndexOf("=")) + "= 0;";
                            list[i + 2] = list[i + 2].Remove(list[i + 2].IndexOf("=")) + "= 0;";
                        }
                    }
                    File.WriteAllLines(item, list, Encoding.GetEncoding("Windows-1251"));
                }
            }
            if (Directory.Exists(Path.Combine(path, "Missions")))
            {
                foreach (string item in Directory.EnumerateFiles(Path.Combine(path, "Missions"), "*.spg"))
                {
                    List<string> list = new List<string>(File.ReadAllLines(item, Encoding.GetEncoding("Windows-1251")));
                    int count = list.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (string.IsNullOrEmpty(list[i]) || (!list[i].Contains("controlID = SQSH_SOLDIER_ID") && !list[i].Contains("controlID = SQSH_SOLDIER_PLANT_ID")))
                        {
                            continue;
                        }
                        if (list[i + 7].Contains("SQSH_OFFICER_") && list[i + 14].Contains("SQSH_TECHNIC_"))
                        {
                            if (list[i + 1].Contains("false") && (list[i + 8].Contains("true") || list[i + 15].Contains("true")))
                            {
                                list[i + 1] = list[i + 1].Replace("false", "true");
                            }
                            if (list[i + 2].Contains("false") && (list[i + 9].Contains("true") || list[i + 16].Contains("true")))
                            {
                                list[i + 2] = list[i + 2].Replace("false", "true");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error in line: " + i);
                        }
                    }
                    File.WriteAllLines(item, list, Encoding.GetEncoding("Windows-1251"));
                }
            }
            if (File.Exists(Path.Combine(path, "AttributeLibrary")))
            {
                List<string> list = new List<string>(File.ReadAllLines(Path.Combine(path, "AttributeLibrary")));
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    if (list[i].IndexOf("damageMolecula", StringComparison.OrdinalIgnoreCase) != -1 && list[i + 1].IndexOf("elements", StringComparison.OrdinalIgnoreCase) != -1 && list[i + 2].IndexOf(";") != -1 && list[i + 3].IndexOf(",") != -1 && list[i + 4].IndexOf(",") != -1 && list[i + 5].IndexOf(",") != -1)
                    {
                        int num1 = stringToInt(list[i + 3].Replace(" ", "").Replace(",", ""));
                        int num2 = stringToInt(list[i + 4].Replace(" ", "").Replace(",", ""));
                        int num3 = stringToInt(list[i + 5].Replace(" ", "").Replace(",", ""));
                        list[i + 3] = "						" + (num1 + num2 + num3) + ",";
                        list[i + 4] = "						0,";
                        list[i + 5] = "						0,";
                    }
                }
                File.WriteAllLines(Path.Combine(path, "AttributeLibrary"), list);
            }
        }

        int stringToInt(string input)
        {
            int result = -1;
            if (!string.IsNullOrEmpty(input))
            {
                if (input.Contains("."))
                {
                    int.TryParse(input.Remove(input.IndexOf('.')), out result);
                }
                else if (input.Contains(","))
                {
                    int.TryParse(input.Remove(input.IndexOf(',')), out result);
                }
                else
                {
                    int.TryParse(input, out result);
                }
            }
            return result;
        }
    }
}
