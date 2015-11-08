using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace notepad {
    [Serializable]
    public partial class MainWindow : Form {
        public MainWindow() {
            InitializeComponent();
            wordWrapToolStripMenuItem.BackColor = Color.Red;
        }

        bool saved = false;
        private string filename;
        Helper help = new Helper();

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Written by Luke Cahill");
        }

        #region Copy, Paste & Cut
        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e) {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true) {
                textArea.Paste();
                Clipboard.Clear();
            }
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e) {
            if (textArea.SelectionLength > 0) {
                textArea.Copy();
            }
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e) {
            if (textArea.SelectionLength > 0) {
                textArea.Cut();
            }
        }

        #endregion

        public void searchToolStripMenuItem_Click(object sender, EventArgs e) {
            string search = Interaction.InputBox("What would you like to search for?", "Search", "");
            //string searchFor = Regex.Split(textBox1.Text.Trim(), search);

            int pos = textArea.Text.IndexOf(search);
            int length = search.Length;
            if (pos != -1) {
                textArea.SelectionStart = pos;
                textArea.SelectionLength = length;
            } else {
                MessageBox.Show($"\'{search}\'" + " was not found!", "Error");
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            textArea.SelectAll();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            help.StartNew();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            if (saved) {
                Environment.Exit(0);
            } else {
                MessageBox.Show("Would you like to save before exiting?", "Warning");
            }
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e) {
            if (textArea.WordWrap) {
                textArea.WordWrap = false;
                wordWrapToolStripMenuItem.Name = "Word wrap on";
                wordWrapToolStripMenuItem.BackColor = Color.Green;
            } else if (textArea.WordWrap == false) {
                textArea.WordWrap = true;
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
                textArea.Font = dlg.Font;
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e) {
            textArea.Undo();
        }

        private void textArea_TextChanged(object sender, EventArgs e) {
            saved = false;

            var lineCount = help.LineCount(textArea);
            toolStripStatusLabel1.Text = $"Lines: {lineCount}";
            statusStrip1.Refresh();

            var wordCounter = help.WordCount(textArea);
            toolStripStatusLabel2.Text = $"Words: {wordCounter}";
            statusStrip1.Refresh();
        }

        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e) {
            var time = help.ReturnTime();
            textArea.AppendText(time);

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

        #region Saving and Loading

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(filename)) {
                if (ShowSaveDialog() != DialogResult.OK) {
                    return;
                }
            }
            help.SaveCurrentFile(filename, textArea.Text);
            help.SetWindowTitle(Path.GetFileName(filename));
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            var size = -1;
            var open = new OpenFileDialog();
            DialogResult result = open.ShowDialog();

            if (result == DialogResult.OK) {
                var file = open.FileName;
                filename = file;
                try {
                    textArea.Text = File.ReadAllText(file);
                    size = textArea.Text.Length;
                    this.Text = help.SetWindowTitle(Path.GetFileName(filename));
                } catch (IOException io) {
                    System.Diagnostics.Debug.WriteLine($"IO exception occured: {io.Message}");
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            if (ShowSaveDialog() != DialogResult.OK) {
                return;
            }

            var result = help.SaveCurrentFile(filename, textArea.Text);
            help.SetWindowTitle(Path.GetFileName(filename));
        }

        public void OpenFile(string file) {
            this.filename = file;
        }

        DialogResult ShowSaveDialog() {
            var dialog = new SaveFileDialog();
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK) {
                filename = dialog.FileName;
            }

            return result;
        }
        #endregion
    } // end class
} // end namespace