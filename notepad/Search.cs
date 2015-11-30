using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace notepad {
	public partial class Search : Form {

		List<SearchResult> found = new List<SearchResult>();
		TextBox textbox;
		int current = 0;

		/// <summary>
		/// Initialise this class with the textbox.
		/// </summary>
		/// <param name="text">TextBox of the text editor writing area</param>
		public Search(TextBox text) {
			InitializeComponent();
			textbox = text;
		}

		private void next_Click(object sender, EventArgs e) {
			if(current == (found.Count - 1)) {
				return;	// should alert the user really.
			}

			previous.Enabled = true;
			current++;
			var item = found.Where(i => i.Index == current).Single();
			SetTextBoxSelection(item.Postition, item.Length);
		}

		private void previous_Click(object sender, EventArgs e) {
			if(current == 0) {
				return; // should alert the user really.
			}

			next.Enabled = true;
			current--;
			var item = found.Where(i => i.Index == current).Single();
			SetTextBoxSelection(item.Postition, item.Length);
		}

		private void searchButton_Click(object sender, EventArgs e) {
			var match = new Regex(textToFind.Text);
			var all = match.Matches(textbox.Text);

			if(textToFind.Equals(' ') || String.IsNullOrWhiteSpace(textToFind.Text)) {
				return;	// should also handle this, with a message box or something.
			}

			found.Clear();
			current = 0;
			if(all.Count > 0) {
				var index = 0;
				foreach(var item in all) {
					var word = item.ToString();
					var result = new SearchResult {
						Postition = textbox.Text.IndexOf(word, index),
						Length = word.Length,
						Index = index
					};
					found.Add(result);
					index++;
				}
				next.Enabled = true;
				SetTextBoxSelection(found[current].Postition, found[current].Length);
			} else {
				MessageBox.Show("Text not found!");
			}
		}

		private void SetTextBoxSelection(int start, int length) {
			textbox.SelectionStart = start;
			textbox.SelectionLength = length;
		}
	}
}
