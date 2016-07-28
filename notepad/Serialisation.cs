using System.IO;

namespace notepad {
    /// <summary>
    /// Use to save the current file, and to open a new text document.
    /// </summary>
    public class Serialisation {

        /// <summary>
        /// Writes the current text in the textarea to the selected file.
        /// </summary>
        /// <param name="filename">String name of the file to write too</param>
        /// <param name="textToWrite">String to write to the specified file</param>
        /// <returns>Bool of success/failure</returns>
        public bool SaveCurrentFile(string filename, string textToWrite) {
            using(var stream = new StreamWriter(filename)) {
				stream.NewLine = "\n";
                stream.WriteLine(textToWrite);
                stream.Flush();
            };
            return true;
        }

        /// <summary>
        /// Reads the text from the filename specified.
        /// </summary>
        /// <param name="file">The name of the file to open and read</param>
        /// <returns>String with the contents of the file.</returns>
        public string OpenFile(string file) {
            var text = "";
            using(var stream = new StreamReader(file)) {
                text = stream.ReadToEnd();
            }
            return text;
        }
    }
}
