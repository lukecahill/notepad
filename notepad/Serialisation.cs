using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace notepad {
    public class Serialisation {
        public bool SaveCurrentFile(string filename, string textToWrite) {
            try {
                File.WriteAllText(filename, textToWrite);
                return true;
            } catch (IOException io) {
                Debug.WriteLine(io.Message);
                return false;
            }
        }

        // Things below this line do not currently work. 
        //      Will need to be implmented
        public DialogResult ShowSaveDialog() {
            var dialog = new SaveFileDialog();
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK) {
                var file = dialog.FileName;
                var form = new MainWindow();
                form.OpenFile(file);
            }

            return result;
        }

        public OpenFileDialog OpenDialog(string filename, TextBox textArea) {
            var size = -1;
            var open = new OpenFileDialog();
            var help = new Helper();
            var Text = "";

            DialogResult result = open.ShowDialog();

            if (result == DialogResult.OK) {
                var file = open.FileName;
                filename = file;
                try {
                    textArea.Text = File.ReadAllText(file);
                    size = textArea.Text.Length;
                    Text = help.SetWindowTitle(Path.GetFileName(filename));
                } catch (IOException io) {
                    Debug.WriteLine($"IO exception occured: {io.Message}");
                }
            }
            return null;
        }

        public void SaveFile(string filename, TextBox textArea) {
            var help = new Helper();
            var form = new MainWindow();

            if (string.IsNullOrEmpty(filename)) {
                if (ShowSaveDialog() != DialogResult.OK) {
                    return;
                }
            }
            SaveCurrentFile(filename, textArea.Text);
            help.SetWindowTitle(Path.GetFileName(filename));
        }
    }
}
