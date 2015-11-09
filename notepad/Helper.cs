using System;
using System.Diagnostics;
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
        /// Starts a new process
        /// </summary>
        public void StartNew() {
            var info = new ProcessStartInfo(Application.ExecutablePath);
            Process.Start(info);
        }

        /// <summary>
        /// Checks if the note has been saved before the user exits.
        /// </summary>
        /// <param name="saved">Boolean of if the note has been saved</param>
        public bool CheckExit(bool saved) {
            if (saved) {
                return false;
            } else {

                var ok = MessageBox.Show("Would you like to save before exiting?", "Unsaved changes!", MessageBoxButtons.YesNoCancel);
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
