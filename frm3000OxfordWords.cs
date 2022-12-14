using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using SpeechLib;

namespace English___Vietnamese_Dictionary
{
    public partial class frm3000OxfordWords : Form
    {    
        public frm3000OxfordWords()
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

        int startID, endID;
        Dictionary<String, String> dictIndex = new Dictionary<string, string>();
        Thread loaddata, loadIDFile;

        private void frm3000OxfordWords_Load(object sender, EventArgs e)
        {
            if (dictIndex.Count == 0)
            {
                loadIDFile = new Thread(new ThreadStart(LoadIndexFile));
                loadIDFile.SetApartmentState(ApartmentState.STA);
                loadIDFile.Start();
            }
            if (listView1.Items.Count == 0)
            {
                toolStripProgressBar1.Visible = true;
                loaddata = new Thread(new ThreadStart(LoadData));
                loaddata.SetApartmentState(ApartmentState.MTA);
                loaddata.Start();
            }
        }

        public void LoadData()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            if (File.Exists(Application.StartupPath + "/Database/3000 words.txt"))
            {
               toolStripStatusLabel1.Text = "Loading data...";
                StreamReader reader = new StreamReader(Application.StartupPath + "/Database/3000 words.txt");
                int i = 0;
                int j = -1;
                while (reader.Peek() >=0)
                {
                    j += 1;
                    String s = reader.ReadLine();
                    if (s.Trim() !="")
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

        private void frm3000OxfordWords_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (loaddata !=null)
            {
                loaddata.Abort();
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

        public Point GetLineRange(int ID)
        {
            int i = 0;
            for (int k = 0; k < ID; k++)
            {
                i += richTextBox1.Lines[k].Length;
            }

            return new Point(i + ID + 1, richTextBox1.Lines[ID].Length);
        }

        public void HighLight()
        {
            string word="";
            string wordtype="";
            word = richTextBox1.Lines[0].Replace("/", "[").Substring(0, richTextBox1.Lines[0].Length - 1) + "]";
            if (word.Contains("(") && word.Contains(")"))
            {
                String word1 = word.Split('(')[0].Trim() + " " + word.Split(')')[1].Trim();

                wordtype = word.Split('(')[1].Trim().Split(')')[0].Trim();
                if (wordtype.Contains(","))
                {
                    List<String> lst = new List<string>();
                    foreach (string s in wordtype.Split(','))
                    {
                        switch (s.Trim())
                        {
                            case "n":
                                lst.Add("danh từ");
                                break;
                            case "v":
                                lst.Add("động từ");
                                break;
                            case "adj":
                                lst.Add( "tính từ");
                                break;
                            case "adv":
                                lst.Add( "trạng từ");
                                break;
                            case "prep":
                                lst.Add("giới từ");
                                break;
                            case "conj":
                                lst.Add("liên từ");
                                break;
                            case "pro":
                                lst.Add("đại từ");
                                break;
                            case "det":
                                lst.Add("mạo từ");
                                break;
                        }
                    }
                    wordtype = "";
                    foreach (string it in lst)
                    {
                        wordtype += ", " + it;
                    }
                    wordtype = wordtype.Substring(2, wordtype.Length - 2);
                }
                else
                {
                    switch (wordtype.Trim())
                    {
                        case "n":
                            wordtype = "danh từ";
                            break;
                        case "v":
                            wordtype = "động từ";
                            break;
                        case "adj":
                             wordtype = "tính từ";
                            break;
                        case "adv":
                             wordtype = "trạng từ";
                            break;
                        case "prep":
                             wordtype = "giới từ";
                            break;
                        case "conj":
                             wordtype = "liên từ";
                            break;
                        case "pro":
                             wordtype = "đại từ";
                            break;
                        case "det":
                             wordtype = "mạo từ";
                            break;
                    }
                }
                word = word1;
            }

            string temp = "";
            if (wordtype != "")
            {
                temp = Environment.NewLine +  "* " + wordtype;
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
            //
            //richTextBox1.SelectionStart = "";
            for (int k = 0; k < richTextBox1.Lines.Length; k++)
            {
                if (richTextBox1.Lines[k].StartsWith("•"))
                {
                    richTextBox1.SelectionStart = GetLineRange(k).X;
                    richTextBox1.SelectionLength = GetLineRange(k).Y;
                    richTextBox1.SelectionFont = new Font(richTextBox1.Font.Name, richTextBox1.Font.Size, FontStyle.Bold);

                }
                else if (richTextBox1.Lines[k].StartsWith("-"))
                {
                    richTextBox1.SelectionStart = GetLineRange(k).X;
                    richTextBox1.SelectionLength = GetLineRange(k).Y;
                    richTextBox1.SelectionColor = Properties.Settings.Default.TranslatedWordColor;

                }
                else if (richTextBox1.Lines[k].StartsWith("*"))
                {
                    richTextBox1.SelectionStart = GetLineRange(k).X;
                    richTextBox1.SelectionLength = GetLineRange(k).Y;
                    richTextBox1.SelectionColor = Properties.Settings.Default.WordTypeColor;

                }
            }
            richTextBox1.Refresh();

        }

        public void LoadIndexFile()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            if (File.Exists(Application.StartupPath + "/Database/3000 words_index.txt"))
            {
                StreamReader reader = new StreamReader(Application.StartupPath + "/Database/3000 words_index.txt");
                while (reader.Peek() >= 0)
                {
                    String s = reader.ReadLine();
                    if (s.Trim() != "")
                    {
                        try
                        {
                            //lst.Add(s.Split(':')[0].Trim());
                            dictIndex.Add(s.Trim().Split(':')[0].Trim(), s.Trim().Split(':')[1]);
                        }
                        catch
                        {
                        }
                    }
                }
                reader.Close();
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + "/Database/3000 words.txt"))
            {
                StreamReader reader = new StreamReader(Application.StartupPath + "/Database/3000 words.txt");
                int i = -1;
                startID = int.Parse(dictIndex[listView1.SelectedItems[0].Text].Split(',')[0].Trim());
                endID = startID + int.Parse(dictIndex[listView1.SelectedItems[0].Text].Split(',')[1].Trim());
                //MessageBox.Show(startID.ToString() + ", " + endID.ToString());
                richTextBox1.Text = "";
                while (reader.Peek() >= 0)
                {
                    i += 1;
                    string s = reader.ReadLine();
                    if ((i >= startID) && (i <= endID))
                    {
                        richTextBox1.AppendText(s + Environment.NewLine);
                    }

                }
                reader.Close();
                //Highlight
                richTextBox1.Text = richTextBox1.Text.Trim().Replace("*", "•").Replace("+", Environment.NewLine + "+").Replace("-", "- ").Replace("=", "*  ");
                richTextBox1.Text = richTextBox1.Text.Substring(1, richTextBox1.Text.Length - 1);
                HighLight();
            }
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
        
        private void button1_Click(object sender, EventArgs e)
        {
            //Create a TTS voice and speak.
            try
            {
               SpVoice  Voice = new SpVoice();
                Voice.Voice = Voice.GetVoices().Item(Properties.Settings.Default.VoiceType);
                Voice.Volume = Properties.Settings.Default.Volume;
                String word="";
                if (richTextBox1.Lines[0].Trim().Contains("["))
                {
                  word  = richTextBox1.Lines[0].Trim().Split('[')[0].Trim();
                }
                else
                {
                    word = richTextBox1.Lines[0].Trim();
                }
                Voice.Speak(word, SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault);
            }
            catch
            {
                MessageBox.Show("Speak error", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
