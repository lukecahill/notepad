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
		MainWindow mainWindow;

		/// <summary>
		/// Initialise this class with the textbox, and MainWindow object.
		/// </summary>
		/// <param name="text">TextBox of the text editor writing area</param>
		/// <param name="main">MainWindow of the text editor writing area</param>
		public Search(TextBox text, MainWindow main) {
			InitializeComponent();
			textbox = text;
			mainWindow = main;
		}

		private void next_Click(object sender, EventArgs e) {
			if(current == (found.Count - 1)) {
				return;	// should never actually be hit.
			}
			
			current++;
			var item = found.Single(i => i.Index == current);
			SetTextBoxSelection(item.Postition, item.Length);
			SetButtons();
			mainWindow.BringToFront();
		}

		private void previous_Click(object sender, EventArgs e) {
			if(current == 0) {
				return; // should never actually be hit.
			}
			
			current--;
			var item = found.Single(i => i.Index == current);
			SetTextBoxSelection(item.Postition, item.Length);
			SetButtons();
			mainWindow.BringToFront();
		}

		private void searchButton_Click(object sender, EventArgs e) {
			var match = new Regex(textToFind.Text);
			var results = match.Matches(textbox.Text);
			

			if(textToFind.Equals(' ') || String.IsNullOrWhiteSpace(textToFind.Text)) {
				MessageBox.Show("Please enter some text to find.");
				return;	// should also handle this, with a message box or something.
			}

			found.Clear();
			current = 0;
			if(results.Count > 0) {
				FoundResults(results);
			} else {
				MessageBox.Show("Text not found!");
			}
		}

		private void SetTextBoxSelection(int start, int length) {
			textbox.SelectionStart = start;
			textbox.SelectionLength = length;
		}

		private void SetButtons() {
			if(current == (found.Count - 1) && (found.Count > 1)) {
				previous.Enabled = true;
				next.Enabled = false;
			} else if(current == 0 && (found.Count > 1)) {
				previous.Enabled = false;
				next.Enabled = true;
			}
		}

		private SearchResult CreateResult(int position, int length, int index) {
			return new SearchResult {
				Postition = position,
				Length = length,
				Index = index
			};
		}

		private void textToFind_KeyPress(object sender, KeyPressEventArgs e) {
			if(e.KeyChar == (char)Keys.Enter) {
				searchButton.PerformClick();	// simulate a mouse click if return is pressed.
			}
		}

		private void FoundResults(MatchCollection match) {
			var index = 0;
			foreach (Match item in match) {
				var result = CreateResult(item.Index, item.Value.Length, index);
				found.Add(result);
				index++;
			}

			SetButtons();
			SetTextBoxSelection(found[current].Postition, found[current].Length);
			mainWindow.BringToFront();
		}
	}
}