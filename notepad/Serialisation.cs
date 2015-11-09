using System.Diagnostics;
using System.IO;

namespace notepad {
    public class Serialisation {

        /// <summary>
        /// Writes the current text in the textarea to the selected file.
        /// </summary>
        /// <param name="filename">String name of the file to write too</param>
        /// <param name="textToWrite">String to write to the specified file</param>
        /// <returns>Bool of success/failure</returns>
        public bool SaveCurrentFile(string filename, string textToWrite) {
            try {
                File.WriteAllText(filename, textToWrite);
                return true;
            } catch (IOException io) {
                Debug.WriteLine(io.Message);
                return false;
            }
        }

        /// <summary>
        /// Reads the text from the filename specified.
        /// </summary>
        /// <param name="file">The name of the file to open and read</param>
        /// <returns>String with the contents of the file.</returns>
        public string OpenFile(string file) {
            return File.ReadAllText(file);
        }
    }
}
