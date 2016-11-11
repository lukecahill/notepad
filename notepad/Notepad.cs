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
				if(textArea.Text.Equals("")) { return; }
                var isSaved = help.CheckExit(saved);
                var close = ExitApplication(isSaved);

                e.Cancel = close;
            } else {
                // only want to prompt to save if the user is the close reason. Otherwise
                //      return as seen here e.g. if the computer is shutting down.
                return;
            }
        }

		/// <summary>
		/// Show an about message when clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Written by Luke Cahill");
        }

        #region Copy, Paste & Cut
		/// <summary>
		/// Paste any text that is found in the users clipboard.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e) {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text)) {
                textArea.Paste();
            }
        }

		/// <summary>
		/// Copy any highlighted text
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void copyToolStripMenuItem1_Click(object sender, EventArgs e) {
            if (textArea.SelectionLength > 0) {
                textArea.Copy();
            }
        }

		/// <summary>
		/// Cut any highlighted text
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void cutToolStripMenuItem1_Click(object sender, EventArgs e) {
            if (textArea.SelectionLength > 0) {
                textArea.Cut();
            }
        }

        #endregion

		/// <summary>
		/// Shows the search form, allowing the user to user the search and replace functionality
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void searchToolStripMenuItem_Click(object sender, EventArgs e) {
			var search = new Search(textArea, this);
			
			search.Show();
        }

		/// <summary>
		/// Select all of the text in the text area
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            textArea.SelectAll();
        }

		/// <summary>
		/// Start a new instance of the notepad
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            help.StartNew();
        }

		/// <summary>
		/// Exit the current instance. Checks that the current document is saved if any changes have been made.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			if(!saved) {
				var closing = new FormClosingEventArgs(CloseReason.UserClosing, false);
				OnFormClosing(closing);
			} else {
				Environment.Exit(0);
			}
        }

		/// <summary>
		/// Prints the current documents
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void printToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!String.IsNullOrEmpty(filename)) {
                help.PrintDocument();
            } else {
                MessageBox.Show($"Please save your file before printing!", "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

		/// <summary>
		/// Shows the print preview page to the user. Can then choose to print from this screen
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!String.IsNullOrEmpty(filename)) {
                help.PrintPreview();
            } else {
                MessageBox.Show($"Please save your file before printing!", "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

		/// <summary>
		/// Turn word wrap on and off. Changes the text and colors to show what is selected. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Shows a dialog page where the user can edit the current font.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void customizeToolStripMenuItem_Click(object sender, EventArgs e) {
            var dlg = Utilities.SetUpFontDialog();

            if (dlg.ShowDialog() == DialogResult.OK) {
                textArea.Font = dlg.Font;
                textArea.ForeColor = dlg.Color;
            }
        }

		/// <summary>
		/// Undo any changes in the textarea
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void undoToolStripMenuItem_Click(object sender, EventArgs e) {
            textArea.Undo();
        }

		/// <summary>
		/// Update the word and line count as the user is typing. 
		/// Changes the saved boolean to false as changes have been made.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void textArea_TextChanged(object sender, EventArgs e) {
            saved = false;
            toolStripStatusLabel1.Text = $"Lines: {Utilities.LineCount(textArea)}";
            statusStrip1.Refresh();

            toolStripStatusLabel2.Text = $"Words: {Utilities.WordCount(textArea)}";
            statusStrip1.Refresh();
        }

		/// <summary>
		/// Prints the current date and time when clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e) {
            textArea.AppendText(Utilities.ReturnTime());

        }

		/// <summary>
		/// Keeps this instance of Notepad+++ on top of all other windows. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e) {
            this.TopMost = alwaysOnTopToolStripMenuItem.Checked;
        }

		/// <summary>
		/// Show or hide the status strip.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Save the current document when clicked. If it is a new file with no file name then a prompt is shown asking the user to enter a filename. 
		/// Sets the window title to the title of the saved document.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Save the current document when clicked. If it is a new file with no file name then a prompt is shown asking the user to enter a filename. 
		/// Sets the window title to the title of the saved document.
		/// If this was shown after the user clicked the exit button then it will exit the program, after saving.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Shows the dialog allowing the user to choose a file to open.
		/// When a file is chosen then that file is opened and it's content written to the text area.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// The save dialog that is shown.
		/// Filter so that the only documents which are shows are text files. 
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// Open the search form, but show the replace functionality straight away
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void searchReplaceToolStripMenuItem_Click(object sender, EventArgs e) {
			var search = new Search(textArea, this, true);
			search.Show();
		}
	} // end class
} // end namespace