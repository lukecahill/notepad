using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace notepad {
    public static class Utilities {

        /// <summary>
        /// Counts the number of lines of text there are in the program
        /// </summary>
        /// <param name="textArea">Textbox area item</param>
        /// <returns>Integer with the count of the number of lines found</returns>
        public static int LineCount(TextBox textArea) {
            string[] lines = Regex.Split(textArea.Text.Trim(), "\r\n");
            var lineCount = lines.Count();
            return lineCount;
        }

        /// <summary>
        /// Counts the number of words of text there are in the program
        /// </summary>
        /// <param name="textArea">Textbox area item</param>
        /// <returns>Integer with the count of the number of lines found</returns>
        public static int WordCount(TextBox textArea) {
            string[] words = Regex.Split(textArea.Text.Trim(), "\\w+");
            var wordCounter = 0;
            wordCounter = words.Count();
            return wordCounter -= 1;
        }

        /// <summary>
        /// Creates a new FontDialog item with the initilaised parameters set
        /// </summary>
        /// <returns>FontDialog item</returns>
        public static FontDialog SetUpFontDialog() {
            return new FontDialog {
                ShowColor = true,
                ShowApply = true,
                ShowEffects = true,
                ShowHelp = true
            };
        }

        /// <summary>
        /// Used to get the current datetime 
        /// </summary>
        /// <returns>String with UTC datetime</returns>
        public static string ReturnTime() {
            return DateTime.UtcNow.ToString();
        }

        /// <summary>
        /// Searches the textbox for the specified text
        /// </summary>
        /// <param name="textArea">Textbox item which contains the text which is being searched for</param>
        /// <param name="search">String text which is being searched for</param>
        /// <returns>Int array containing the position at [0], and the length at [1] if the text is found. If it is not found returns null, which is later checked.</returns>
        public static int[] ReturnSearch(TextBox textArea, string search) {
            int[] array = new int[2];
            var postion = textArea.Text.IndexOf(search);
            var length = search.Length;

            if(postion != -1) {
                array[0] = postion;
                array[1] = length;
                return array;
            }
            return null;
        }
    }
}
