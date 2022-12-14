using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace English___Vietnamese_Dictionary
{
    public partial class frmIrregularVerbs : Form
    {
        public frmIrregularVerbs()
        {
            InitializeComponent();
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private extern static int GetWindowLong(IntPtr hWnd, int index);

        public static bool VerticalScrollBarVisible(Control ctl)
        {
            int style = GetWindowLong(ctl.Handle, -16);
            return (style & 0x200000) != 0;
        }
        int startID, endID, lines;
        Dictionary<String, String> IrregularVerbs = new Dictionary<string, string>();
        Thread loaddata1, loadIDFile;

        private void IrregularVerbs_Load(object sender, EventArgs e)
        {
            if (IrregularVerbs.Count == 0)
            {
                loaddata1 = new Thread(new ThreadStart(LoadData1));
                loaddata1.SetApartmentState(ApartmentState.MTA);
                loaddata1.Start();
                //loadIDFile = new Thread(new ThreadStart(LoadIndexFile));
                //loadIDFile.SetApartmentState(ApartmentState.STA);
                //loadIDFile.Start();
            }
            if (listView1.Items.Count == 0)
            {
                toolStripProgressBar1.Visible = true;
                loaddata1 = new Thread(new ThreadStart(LoadData1));
                loaddata1.SetApartmentState(ApartmentState.MTA);
                loaddata1.Start();
            }
        }
        public void LoadData1()
        {  
            Control.CheckForIllegalCrossThreadCalls = false;
            if (File.Exists(Application.StartupPath + "/Database/IrregularVerbs.txt"))
            {
                toolStripStatusLabel1.Text = "Loading data...";
                StreamReader reader = new StreamReader(Application.StartupPath + "/Database/IrregularVerbs.txt");
                int i = 0;
                int j = -1;
                while (reader.Peek() >= 0)
                {
                    j += 1;
                    String s = reader.ReadLine();
                    if (s.Trim() != "")
                    {
                        if (s.Trim().StartsWith("@"))
                        {
                            i += 1;
                            if (s.Trim().Split('@')[1].Trim().Split('(')[0].Trim().Contains("/") == false)
                            {
                                ListViewItem it = new ListViewItem(s.Trim().Split('@')[1].Trim().Split('(')[0].Trim(), 0);
                                listView1.Items.Add(it);
                            }
                            else
                            {
                                ListViewItem it = new ListViewItem(s.Trim().Split('@')[1].Trim().Split('(')[0].Trim().Split('/')[0].Trim(), 0);
                                listView1.Items.Add(it);

                            }
                            toolStripProgressBar1.Value = (int)(i * 100 / 3509);

                        }
                    }
                }
                reader.Close();
                toolStripStatusLabel1.Text = "Ready";
                toolStripProgressBar1.Visible = false;
            }
        }
        private void Pharseverb_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (loaddata1 != null)
            {
                loaddata1.Abort();
            }
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        public Point GetLineRange2(int line)
        {
            int i = 0;
            for (int k = 0; k < line; k++)
            {
                i += richTextBox1.Lines[k].Length;
            }

            return new Point(i + line + 1, richTextBox1.Lines[line].Length);
        }

        public void HighLight()
        {
            string word = "";
            string wordtype = "";
            word = richTextBox1.Lines[0].Replace("/", "[").Substring(0, richTextBox1.Lines[0].Length - 1) + "]";
            if (word.Contains("(") && word.Contains(")"))
            {
                String word1 = word.Split('(')[0].Trim() + " " + word.Split(')')[1].Trim();

                wordtype = word.Split('(')[1].Trim().Split(')')[0].Trim();

                word = word1;
            }

            string temp = "";
            if (wordtype != "")
            {
                temp = Environment.NewLine + " Verb 2: " + wordtype;
            }

            for (int k = 1; k < richTextBox1.Lines.Length; k++)
            {
                if (richTextBox1.Lines[k].Trim() != "")
                {
                    temp += Environment.NewLine + richTextBox1.Lines[k].Trim();
                }
            }
            richTextBox1.Text = word + temp;

            //
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = listView1.SelectedItems[0].Text.Trim().Length;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.Name, richTextBox1.Font.Size, FontStyle.Bold);

            //richTextBox1.SelectionStart = "";
            for (int k = 0; k < richTextBox1.Lines.Length; k++)
            {
                if (richTextBox1.Lines[k].StartsWith("•"))
                {
                    richTextBox1.SelectionStart = GetLineRange2(k).X;
                    richTextBox1.SelectionLength = GetLineRange2(k).Y;
                    richTextBox1.SelectionFont = new Font(richTextBox1.Font.Name, richTextBox1.Font.Size, FontStyle.Bold);

                }
                else if (richTextBox1.Lines[k].StartsWith("-"))
                {
                    richTextBox1.SelectionStart = GetLineRange2(k).X;
                    richTextBox1.SelectionLength = GetLineRange2(k).Y;
                    richTextBox1.SelectionColor = Properties.Settings.Default.TranslatedWordColor;

                }
                else if (richTextBox1.Lines[k].StartsWith("*"))
                {
                    richTextBox1.SelectionStart = GetLineRange2(k).X;
                    richTextBox1.SelectionLength = GetLineRange2(k).Y;
                    richTextBox1.SelectionColor = Properties.Settings.Default.WordTypeColor;

                }
            }
            richTextBox1.Refresh();
        }
            private void richTextBox1_TextChanged(object sender, EventArgs e)
            {
                button1.Visible = richTextBox1.Text != "";

                if (VerticalScrollBarVisible(richTextBox1))
                {
                    button1.Left = this.Width - 81;
                }
                else
                {
                    button1.Left = this.Width - 65; ;

                }
            }

        }
     }

