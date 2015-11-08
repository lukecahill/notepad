using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;

namespace notepad {
    public class Helper {
        public bool SaveCurrentFile(string filename, string textToWrite) {
            try {
                File.WriteAllText(filename, textToWrite);
                return true;
            } catch (IOException io) {
                Debug.WriteLine(io.Message);
                return false;
            }
        }

        public string SetWindowTitle(string fileName) {
            return $"{fileName} - Text Editor";
        }

        //public DialogResult ShowSaveDialog() {
        //    var dialog = new SaveFileDialog();
        //    var result = dialog.ShowDialog();

        //    if (result == DialogResult.OK) {
        //        var file = dialog.FileName;
        //        var form = new Form1();
        //        form.OpenFile(file);
        //    }

        //    return result;
        //}

        public string ReturnTime() {
            return System.DateTime.UtcNow.ToString();
        }

        public void StartNew() {
            var info = new ProcessStartInfo(Application.ExecutablePath);
            Process.Start(info);
        }

        public int LineCount(TextBox textArea) {
            string[] lines = Regex.Split(textArea.Text.Trim(), "\r\n");
            var lineCount = lines.Count();
            return lineCount;
        }

        public int WordCount(TextBox textArea) {
            string[] words = Regex.Split(textArea.Text.Trim(), "\\w+");
            var wordCounter = 0;
            wordCounter = words.Count();
            return wordCounter -= 1;
        }

    }
}
