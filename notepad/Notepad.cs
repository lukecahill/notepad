using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace notepad {
    public partial class MainWindow : Form {

        private bool saved = false;
        private string filename;
        private bool exitAfterSave = false;

        // Instantiate the helper class and the serialising class. Utilities is static and cannot be instantiated.
        ApplicationHelper help = new ApplicationHelper();
        Serialisation serialise = new Serialisation();

        /// <summary>
        /// Initialise the form which contains the text editor
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            wordWrapToolStripMenuItem.BackColor = Color.Red;
            saved = true;
        }

        /// <summary>
        /// Used to check whether to cancel the close event for saving, or to just exit without saving.
        /// </summary>
        /// <param name="code">Integer of the code returned from checking if the document should be saved or not.</param>
        /// <returns>Boolean of whether to exit the application or not.</returns>
        private bool ExitApplication(int code) {
            var close = true;
            switch (code) {
                case 0:
                    close = false;
                    break;
                case 1:
                    exitAfterSave = true;
                    saveAsToolStripMenuItem.PerformClick();
                    close = true;
                    break;
                case 2:
                    close = true;
                    break;
            }
            return close;
        }

        /// <summary>
        /// Overrides the defuault behaviour of OnFormClosing
        /// </summary>
        /// <param name="e">FormClosingEventArgs use to find the close reason, and to cancel the close if necessary</param>
        protected override void OnFormClosing(FormClosingEventArgs e) {
            base.OnFormClosing(e);

            if(e.CloseReason == CloseReason.UserClosing) {
                var isSaved = help.CheckExit(saved);
                var close = ExitApplication(isSaved);

                e.Cancel = close;
            } else {
                // only want to prompt to save if the user is the close reason. Otherwise
                //      return as seen here e.g. if the computer is shutting down.
                return;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Written by Luke Cahill");
        }

        #region Copy, Paste & Cut
        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e) {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text)) {
                textArea.Paste();
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

        private void searchToolStripMenuItem_Click(object sender, EventArgs e) {
			var search = new Search(textArea, this);
			
			search.Show();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            textArea.SelectAll();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            help.StartNew();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			if(!saved) {
				var closing = new FormClosingEventArgs(CloseReason.UserClosing, false);
				OnFormClosing(closing);
			} else {
				Environment.Exit(0);
			}
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!String.IsNullOrEmpty(filename)) {
                help.PrintDocument();
            } else {
                MessageBox.Show($"Please save your file before printing!", "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!String.IsNullOrEmpty(filename)) {
                help.PrintPreview();
            } else {
                MessageBox.Show($"Please save your file before printing!", "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
            var dlg = Utilities.SetUpFontDialog();

            if (dlg.ShowDialog() == DialogResult.OK) {
                textArea.Font = dlg.Font;
                textArea.ForeColor = dlg.Color;
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e) {
            textArea.Undo();
        }

        private void textArea_TextChanged(object sender, EventArgs e) {
            saved = false;
            toolStripStatusLabel1.Text = $"Lines: {Utilities.LineCount(textArea)}";
            statusStrip1.Refresh();

            toolStripStatusLabel2.Text = $"Words: {Utilities.WordCount(textArea)}";
            statusStrip1.Refresh();
        }

        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e) {
            textArea.AppendText(Utilities.ReturnTime());

        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e) {
            this.TopMost = alwaysOnTopToolStripMenuItem.Checked;
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            if (statusStrip1.Visible == false) {
                statusStrip1.Visible = true;
                statusStrip1.Refresh();
            } else if (statusStrip1.Visible) {
                statusStrip1.Visible = false;
                statusStrip1.Refresh();
            }
        }

        private void statusbarToolStripMenuItem_Click(object sender, EventArgs e) {
            if(statusStrip1.Visible) {
                statusStrip1.Visible = false;
            } else if(statusStrip1.Visible == false) {
                statusStrip1.Visible = true;
            }
        }

        #region Saving and Loading

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(filename)) {
                if (ShowSaveDialog() != DialogResult.OK) {
                    return;
                }
            }

            serialise.SaveCurrentFile(filename, textArea.Text);
            help.SetWindowTitle(Path.GetFileName(filename));
            saved = true;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            if (ShowSaveDialog() != DialogResult.OK) {
                return;
            }

            var result = serialise.SaveCurrentFile(filename, textArea.Text);
            help.SetWindowTitle(Path.GetFileName(filename));
            saved = true;

            if(exitAfterSave) {
                Environment.Exit(0);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            var size = -1;
            var open = new OpenFileDialog();
            DialogResult result = open.ShowDialog();

            if (result == DialogResult.OK) {
                var file = open.FileName;
                filename = file;

                try {
                    textArea.Text = serialise.OpenFile(file);
                    size = textArea.Text.Length;
                    this.Text = help.SetWindowTitle(Path.GetFileName(filename));
                    saved = true;
                } catch (IOException io) {
                    System.Diagnostics.Debug.WriteLine($"IO exception occured: {io.Message}");
                }
            }
        }

        DialogResult ShowSaveDialog() {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if(!String.IsNullOrEmpty(filename)) {
                var saveFile = Path.GetFileName(filename);
                dialog.FileName = saveFile;
            }
            
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK) {
                filename = dialog.FileName;
            }

            return result;
        }
        #endregion
    } // end class
} // end namespace