using System;
using System.Collections.Generic;
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
			this.TopMost = true;
		}

		/// <summary>
		/// Iterate through the list - this goes forwards though the list and increments the current item by one
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void next_Click(object sender, EventArgs e) {
			if(current == (found.Count - 1)) {
				return;	// should never actually be hit.
			}
			
			current++;
			var item = found[current];
			SetTextBoxSelection(item.Postition, item.Length);
			SetButtons();
			mainWindow.BringToFront();
		}

		/// <summary>
		/// Iterate through the list - this goes backwards though the list and decrements the current item by one
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void previous_Click(object sender, EventArgs e) {
			if(current == 0) {
				return; // should never actually be hit.
			}
			
			current--;
			var item = found[current];
			SetTextBoxSelection(item.Postition, item.Length);
			SetButtons();
			mainWindow.BringToFront();
		}

		/// <summary>
		/// Runs the regex over the textbox with the text to find, then calls relevant functions to complete the task. Only works if there are more than 1 results found, and that the text is not empty/spaces.
		/// </summary>
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

		/// <summary>
		/// Selects the text inside the textbox
		/// </summary>
		/// <param name="start">The start index of the word found</param>
		/// <param name="length">The length of the word</param>
		private void SetTextBoxSelection(int start, int length) {
			textbox.SelectionStart = start;
			textbox.SelectionLength = length;
		}

		/// <summary>
		/// Sets the next & previous buttons to be enabled or disabled depending on where they are in the list.
		/// </summary>
		private void SetButtons() {
			if(current == (found.Count - 1) && (found.Count > 1)) {
				previous.Enabled = true;
				next.Enabled = false;
			} else if(current == 0 && (found.Count > 1)) {
				previous.Enabled = false;
				next.Enabled = true;
			}
		}

		/// <summary>
		/// Creates a new search result object with the correct position and length
		/// </summary>
		/// <param name="position">Position of the first character</param>
		/// <param name="length">Length of the word</param>
		/// <returns>SearchResult object with relevant position and length</returns>
		private SearchResult CreateResult(int position, int length) {
			return new SearchResult {
				Postition = position,
				Length = length
			};
		}

		/// <summary>
		/// Used to simulate a click of the search button if enter is pressed.
		/// </summary>
		/// <param name="sender">Object textbox</param>
		/// <param name="e">KeyPressEventArgs</param>
		private void textToFind_KeyPress(object sender, KeyPressEventArgs e) {
			if(e.KeyChar == (char)Keys.Enter) {
				searchButton.PerformClick();	// simulate a mouse click if return is pressed.
			}
		}

		/// <summary>
		/// Cycles through the matches of the Regex, adds the results to a list, then sets the default buttons, and selection of the textbox.
		/// </summary>
		/// <param name="match"></param>
		private void FoundResults(MatchCollection match) {
			var index = 0;
			foreach (Match item in match) {
				var result = CreateResult(item.Index, item.Value.Length);
				found.Add(result);
				index++;
			}

			SetButtons();
			SetTextBoxSelection(found[current].Postition, found[current].Length);
			mainWindow.BringToFront();
		}

		/// <summary>
		/// Change opactity to half when the mouse leaves the form
		/// </summary>
		private void Search_MouseLeave(object sender, EventArgs e) {
			this.Opacity = 0.5;
		}

		/// <summary>
		/// Change opactity to full when the mouse enters the form
		/// </summary>
		private void Search_MouseEnter(object sender, EventArgs e) {
			this.Opacity = 1;
		}
	}
}