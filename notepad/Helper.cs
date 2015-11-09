using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace notepad {
    public class Helper {
        /// <summary>
        /// Format the window title for the form
        /// </summary>
        /// <param name="fileName">The file name to have as the form title</param>
        /// <returns>String with formatted text</returns>
        public string SetWindowTitle(string fileName) {
            return $"{fileName} - Text Editor";
        }

        /// <summary>
        /// Used to get the current datetime 
        /// </summary>
        /// <returns>String with UTC datetime</returns>
        public string ReturnTime() {
            return DateTime.UtcNow.ToString();
        }

        /// <summary>
        /// Starts a new process
        /// </summary>
        public void StartNew() {
            var info = new ProcessStartInfo(Application.ExecutablePath);
            Process.Start(info);
        }

        /// <summary>
        /// Counts the number of lines of text there are in the program
        /// </summary>
        /// <param name="textArea">Textbox area item</param>
        /// <returns>Integer with the count of the number of lines found</returns>
        public int LineCount(TextBox textArea) {
            string[] lines = Regex.Split(textArea.Text.Trim(), "\r\n");
            var lineCount = lines.Count();
            return lineCount;
        }

        /// <summary>
        /// Counts the number of words of text there are in the program
        /// </summary>
        /// <param name="textArea">Textbox area item</param>
        /// <returns>Integer with the count of the number of lines found</returns>
        public int WordCount(TextBox textArea) {
            string[] words = Regex.Split(textArea.Text.Trim(), "\\w+");
            var wordCounter = 0;
            wordCounter = words.Count();
            return wordCounter -= 1;
        }

        /// <summary>
        /// Creates a new FontDialog item with the initilaised parameters set
        /// </summary>
        /// <returns>FontDialog item</returns>
        public FontDialog SetUpFontDialog() {
            return new FontDialog {
                ShowColor = true,
                ShowApply = true,
                ShowEffects = true,
                ShowHelp = true
            };
        }

        /// <summary>
        /// Checks if the note has been saved before the user exits.
        /// </summary>
        /// <param name="saved">Boolean of if the note has been saved</param>
        public bool CheckExit(bool saved) {
            if (saved) {
                return false;
            } else {

                var ok = MessageBox.Show("Would you like to save before exiting?", "Warning", MessageBoxButtons.YesNoCancel);
                if(ok == DialogResult.Yes) {
                    // save the note
                    return true;
                } else if(ok == DialogResult.No) {
                    return false;
                } else {
                    // do nothing
                    return true;
                }
            }
        }
    }
}
