using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TextReplacer;

public class FormMain : Form
{
	private string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

	private IContainer components;

	private Button button1;

	private FolderBrowserDialog folderBrowserDialog1;

	public FormMain()
	{
		InitializeComponent();
	}

	private int stringToInt(string input)
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

	private void button1_Click(object sender, EventArgs e)
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
					string text = list[i].Replace(" ", "").Replace("\t", "");
					if (!text.StartsWith("soldiers=") || !text.EndsWith(";"))
					{
						continue;
					}
					string text2 = list[i + 1].Replace(" ", "").Replace("\t", "");
					string text3 = list[i + 2].Replace(" ", "").Replace("\t", "");
					if (text2.StartsWith("officers=") && text2.EndsWith(";") && text3.StartsWith("technics=") && text3.EndsWith(";"))
					{
						int num = stringToInt(text.Replace("soldiers=", "").Replace(";", ""));
						int num2 = stringToInt(text2.Replace("officers=", "").Replace(";", ""));
						int num3 = stringToInt(text3.Replace("technics=", "").Replace(";", ""));
						if (num > 0 || num2 > 0 || num3 > 0)
						{
							list[i] = list[i].Remove(list[i].IndexOf("=")) + "= " + (num + num2 + num3) + ";";
							list[i + 1] = list[i + 1].Remove(list[i + 1].IndexOf("=")) + "= 0;";
							list[i + 2] = list[i + 2].Remove(list[i + 2].IndexOf("=")) + "= 0;";
						}
					}
				}
				File.WriteAllLines(item, list, Encoding.GetEncoding("Windows-1251"));
			}
		}
		if (!Directory.Exists(Path.Combine(path, "Missions")))
		{
			return;
		}
		foreach (string item2 in Directory.EnumerateFiles(Path.Combine(path, "Missions"), "*.spg"))
		{
			List<string> list2 = new List<string>(File.ReadAllLines(item2, Encoding.GetEncoding("Windows-1251")));
			int count2 = list2.Count;
			for (int j = 0; j < count2; j++)
			{
				if (string.IsNullOrEmpty(list2[j]) || (!list2[j].Contains("controlID = SQSH_SOLDIER_ID") && !list2[j].Contains("controlID = SQSH_SOLDIER_PLANT_ID")))
				{
					continue;
				}
				if (list2[j + 7].Contains("SQSH_OFFICER_") && list2[j + 14].Contains("SQSH_TECHNIC_"))
				{
					if (list2[j + 1].Contains("false") && (list2[j + 8].Contains("true") || list2[j + 15].Contains("true")))
					{
						list2[j + 1] = list2[j + 1].Replace("false", "true");
					}
					if (list2[j + 2].Contains("false") && (list2[j + 9].Contains("true") || list2[j + 16].Contains("true")))
					{
						list2[j + 2] = list2[j + 2].Replace("false", "true");
					}
				}
				else
				{
					MessageBox.Show("Error in line: " + j);
				}
			}
			File.WriteAllLines(item2, list2, Encoding.GetEncoding("Windows-1251"));
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextReplacer.FormMain));
		this.button1 = new System.Windows.Forms.Button();
		this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
		base.SuspendLayout();
		this.button1.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		this.button1.Location = new System.Drawing.Point(11, 12);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(182, 35);
		this.button1.TabIndex = 1;
		this.button1.Text = "Generate";
		this.button1.Click += new System.EventHandler(button1_Click);
		this.folderBrowserDialog1.ShowNewFolderButton = false;
		base.ClientSize = new System.Drawing.Size(204, 59);
		base.Controls.Add(this.button1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.Name = "FormMain";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "TextReplacer Creator";
		base.ResumeLayout(false);
	}
}
