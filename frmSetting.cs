using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace English___Vietnamese_Dictionary
{
    public partial class frmSetting : Form
    {
        Form1 formMain;
        public frmSetting(Form1 frm)
        {
            InitializeComponent();
            formMain = frm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Khôi phục cài đặt về trạng thái mặc định?", "Restore setting", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.OK)
            {
                radioButton1.Checked = true;
                comboBox1.SelectedItem = "Microsoft Sans Serif";
                comboBox2.SelectedItem = "12";
                pictureBox1.BackColor = Color.Blue;
                pictureBox2.BackColor = Color.FromKnownColor(KnownColor.Fuchsia);
                textBox1.Text = "50";
                checkBox1.Checked = true;
            }
        }

        SpeechLib.SpVoice Voice = new SpeechLib.SpVoice();

        private void frmSetting_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DictType == "EV")
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }
            //
            comboBox1.Items.Clear();
            foreach (FontFamily fnt in FontFamily.Families)
            {
                comboBox1.Items.Add(fnt.Name);
            }
            comboBox1.SelectedItem = Properties.Settings.Default.Font.Name;
            comboBox2.SelectedItem = Properties.Settings.Default.Font.Size.ToString();
            pictureBox1.BackColor = Properties.Settings.Default.TranslatedWordColor;
            pictureBox2.BackColor = Properties.Settings.Default.WordTypeColor;
            textBox1.Text = Properties.Settings.Default.SuggesttedWordMaximum.ToString();
            checkBox1.Checked = Properties.Settings.Default.DoubleClickTranslate;
            //
            textBox2.Font = Properties.Settings.Default.Font;
            //
            trackBar1.Value = Properties.Settings.Default.Volume;
            comboBox3.Items.Clear();
            for (int i = 0; i < formMain.Voice.GetVoices().Count;i++ )
            {
                comboBox3.Items.Add(formMain.Voice.GetVoices().Item(i).GetDescription());
            }
            comboBox3.SelectedIndex = Properties.Settings.Default.VoiceType;
        }

        private void frmSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.DoubleClickTranslate = checkBox1.Checked;
            if (radioButton1.Checked)
            {
                Properties.Settings.Default.DictType = "EV";
            }
            else
            {
                Properties.Settings.Default.DictType = "VE";
            }
            Properties.Settings.Default.Font = new Font(comboBox1.SelectedItem.ToString(), int.Parse(comboBox2.SelectedItem.ToString()), FontStyle.Regular);
            Properties.Settings.Default.SuggesttedWordMaximum = int.Parse(textBox1.Text);
            Properties.Settings.Default.TranslatedWordColor = pictureBox1.BackColor;
            Properties.Settings.Default.WordTypeColor = pictureBox2.BackColor;
            //
            if (formMain.richTextBox1.Text != "")
            {
                formMain.richTextBox1.Font = Properties.Settings.Default.Font;
                formMain.HighLight();
            }
            Properties.Settings.Default.VoiceType = comboBox3.SelectedIndex;
            Properties.Settings.Default.Volume = trackBar1.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ColorDialog color = new ColorDialog();
            if (color.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox1.BackColor = color.Color;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ColorDialog color = new ColorDialog();
            if (color.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox2.BackColor = color.Color;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                textBox2.Font = new Font(comboBox1.SelectedItem.ToString(), int.Parse(comboBox2.SelectedItem.ToString()), FontStyle.Regular);
            }
            catch { }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                textBox2.Font = new Font(comboBox1.SelectedItem.ToString(), int.Parse(comboBox2.SelectedItem.ToString()), FontStyle.Regular);
            }
            catch { }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Properties.Settings.Default.Volume = trackBar1.Value;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.VoiceType = comboBox3.SelectedIndex;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
