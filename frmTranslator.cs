using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Web;
using System.Globalization;
using System.Diagnostics;
using System.Net;
using RavSoft.GoogleTranslator;
using System.Runtime.InteropServices;


namespace English___Vietnamese_Dictionary
{
    public partial class frmTranslator : Form
    {
        public frmTranslator()
        {
            InitializeComponent();
        }
        private void splitContainer1_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            label2.Left = e.SplitX + splitContainer1.Left;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Tiếng Anh")
            {
                label1.Text = "Tiếng Việt";
                label2.Text = "Tiếng Anh";

            }
            else
            {
                label2.Text = "Tiếng Việt";
                label1.Text = "Tiếng Anh";

            }
            String temp = _editSourceText.Text;
            _editSourceText.Text = _editTarget.Text;
            _editTarget.Text = temp;
        }

        private void frmTextTranslator_Load(object sender, EventArgs e)
        {
            _editSourceText.ReadOnly = false;
            _editTarget.ReadOnly = true;
            _editSourceText.Select();
        }

        private void frmTextTranslator_Resize(object sender, EventArgs e)
        {
            label2.Left = splitContainer1.Panel1.Width + splitContainer1.Left;

        }

        String _translationSpeakUrl;
        private void button1_Click(object sender, EventArgs e)
        {
            // Initialize the translator
            Translator t = new Translator();

            // Translate the text
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this._lblStatus.Text = "Translating...";
                this._lblStatus.Update();
                if (label1.Text == "Tiếng Anh")
                {
                    this._editTarget.Text = t.Translate(_editSourceText.Text, "English", "Vietnamese");
                    this._translationSpeakUrl = t.TranslationSpeechUrl;
                }
                else
                {
                    this._editTarget.Text = t.Translate(_editSourceText.Text, "Vietnamese", "English");
                    this._translationSpeakUrl = t.TranslationSpeechUrl;
                }
                if (t.Error == null)
                {
                    this._editTarget.Update();
                }
                else
                {
                    MessageBox.Show(t.Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                this._lblStatus.Text = string.Format("Translated in {0} mSec", (int)t.TranslationTime.TotalMilliseconds);
                this.Cursor = Cursors.Default;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this._translationSpeakUrl))
            {
                WebClient client = new WebClient();
                Uri ur = new Uri(this._translationSpeakUrl);
                Clipboard.SetText(_translationSpeakUrl);
                client.DownloadProgressChanged += WebClientDownloadProgressChanged;
                client.DownloadFileAsync(ur, Application.StartupPath + "/translate_tts.mp3");
            }

            if (_editTarget.Text != "")
            {

            }
        }

        void WebClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            if (e.ProgressPercentage == 100)
            {

            }
        }

        void SpeakText(String text)
        {
          


        }
    }
}
