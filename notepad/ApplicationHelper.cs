using System.Diagnostics;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace notepad {
    public class ApplicationHelper {
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
        public int CheckExit(bool saved) {
            if (saved) {
                return 0;
            } else {

                var ok = MessageBox.Show("Would you like to save before exiting?", "Unsaved changes!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                if(ok == DialogResult.Yes) {
                    // save the note
                    return 1;
                } else if(ok == DialogResult.No) {
                    return 0;
                } else {
                    // do nothing
                    return 2;
                }
            }
        }

        /// <summary>
        /// Used by the PrintPreview and PrintDocument methods below. This is the code which will actually print the document.
        /// </summary>
        private void Print() {
            using(var print = new PrintDocument()) {
                print.Print();
            };
        }

        /// <summary>
        /// Used to display the print dialog window. Clicking OK then calls the Print() function above.
        /// </summary>
        public void PrintDocument() {
            var print = new PrintDialog();
            if (print.ShowDialog() == DialogResult.OK) {
                Print();
            }
        }

        /// <summary>
        /// Used to show the print preview dialog window. Clicking OK then calls the Print() function above.
        /// </summary>
        public void PrintPreview() {
            var dlg = new PrintPreviewDialog();
            if(dlg.ShowDialog() == DialogResult.OK) {
                Print();
            }
        }
    }
}
