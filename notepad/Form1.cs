using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace notepad {
    [Serializable]
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            wordWrapToolStripMenuItem.BackColor = Color.Red;
        }

        bool saved = false;
        private string filename;

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Written by Luke Cahill");
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e) {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true) {
                textBox1.Paste();
                Clipboard.Clear();
            }
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e) {
            if (textBox1.SelectionLength > 0) {
                textBox1.Copy();
            }
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e) {
            if (textBox1.SelectionLength > 0) {
                textBox1.Cut();
            }
        }

        public void searchToolStripMenuItem_Click(object sender, EventArgs e) {
            string search = Interaction.InputBox("What would you like to search for?", "Search", "");
            //string searchFor = Regex.Split(textBox1.Text.Trim(), search);

            int pos = textBox1.Text.IndexOf(search);
            int length = search.Length;
            if (pos != -1) {
                textBox1.SelectionStart = pos;
                textBox1.SelectionLength = length;
            } else {
                MessageBox.Show("\'" + search + "\'" + " was not found!", "Error");
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            textBox1.SelectAll();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            var info = new System.Diagnostics.ProcessStartInfo(Application.ExecutablePath);
            System.Diagnostics.Process.Start(info);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            if (saved == true) {
                Environment.Exit(0);
            } else {
                MessageBox.Show("Would you like to save before exiting?", "Warning");
            }
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e) {
            if (textBox1.WordWrap == true) {
                textBox1.WordWrap = false;
                wordWrapToolStripMenuItem.Name = "Word wrap on";
                wordWrapToolStripMenuItem.BackColor = Color.Green;
            } else if (textBox1.WordWrap == false) {
                textBox1.WordWrap = true;
                wordWrapToolStripMenuItem.Name = "Word wrap off";
                wordWrapToolStripMenuItem.BackColor = Color.Red;
            }
        }

        private void customizeToolStripMenuItem_Click(object sender, EventArgs e) {
            FontDialog dlg = new FontDialog();
            dlg.ShowColor = true;
            dlg.ShowApply = true;
            dlg.ShowEffects = true;
            dlg.ShowHelp = true;

            if (dlg.ShowDialog() == DialogResult.OK) {
                textBox1.Font = dlg.Font;
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e) {
            textBox1.Undo();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            saved = false;

            //textBox1.SelectionStart = textBox1.Text.Length;
            //textBox1.SelectionLength = 0;
            //textBox1.ScrollToCaret();

            string[] lines = Regex.Split(textBox1.Text.Trim(), "\r\n");
            int lineCount = lines.Count();
            toolStripStatusLabel1.Text = string.Format("Lines: {0}", lineCount);
            statusStrip1.Refresh();

            string[] words = Regex.Split(textBox1.Text.Trim(), "\\w+");
            int wordCounter = 0;
            wordCounter = words.Count();
            wordCounter -= 1;
            toolStripStatusLabel2.Text = string.Format("Words: {0}", wordCounter);
            statusStrip1.Refresh();
        }

        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e) {
            string time = DateTime.Now.ToString();
            textBox1.AppendText(time);

        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e) {
            this.TopMost = alwaysOnTopToolStripMenuItem.Checked;
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            if (statusStrip1.Visible == false) {
                statusStrip1.Visible = true;
                statusStrip1.Refresh();
            } else if (statusStrip1.Visible == true) {
                statusStrip1.Visible = false;
                statusStrip1.Refresh();
            }
        }

        //=======================SAVE AND LOADING=======================================

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(filename)) {
                if (ShowSaveDialog() != DialogResult.OK) {
                    return;
                }
            }
            SaveCurrentFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            int size = -1;
            OpenFileDialog open = new OpenFileDialog();
            DialogResult result = open.ShowDialog();
            if (result == DialogResult.OK) {
                string file = open.FileName;
                try {
                    textBox1.Text = File.ReadAllText(file);
                    size = textBox1.Text.Length;
                    SetWindowTitle(file);
                } catch (IOException) {
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            if (ShowSaveDialog() != DialogResult.OK) {
                return;
            }

            SaveCurrentFile();
            //SaveFileDialog save = new SaveFileDialog();
            //DialogResult result = save.ShowDialog();
            //if (result == DialogResult.OK)
            //{
            //    string file = save.FileName;
            //    try
            //    {
            //        File.WriteAllText(file, textBox1.Text);
            //    }
            //    catch (IOException)
            //    {
            //    }
            //}
            //saved = true;
        }

        DialogResult ShowSaveDialog() {
            var dialog = new SaveFileDialog();
            // set your path, filter, title, whatever
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK) {
                filename = dialog.FileName;
            }

            return result;
        }

        void SaveCurrentFile() {
            //using (var writer = new StreamWriter(filename))
            //{
            // write your file
            try {
                File.WriteAllText(filename, textBox1.Text);
                SetWindowTitle(filename);
            } catch (IOException) {
            }
            //}
            saved = true;
        }

        void SetWindowTitle(string fileName) {
            this.Text = string.Format("{0} - Text Editor", Path.GetFileName(fileName));
        }
    } // end class
} // end namespace