﻿using System;
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
		/// <param name="replace">Boolean of if the user wants to use the replace functionality</param>
		public Search(TextBox text, MainWindow main, bool replace = false) {
			InitializeComponent();
			textbox = text;
			mainWindow = main;
			this.TopMost = true;

			if (replace) {
				replaceAllBtn.Show();
				replaceOneBtn.Show();
				replaceText.Show();
				replaceCheck.Checked = true;
			}
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
			// textbox.Text.IndexOf // for non-regex based searching. allowing the user to search for regex
			
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
			} else if(current > 0) {
				previous.Enabled = true;
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
		/// <param name="match">MatchCollection containing the items which matched the Regex search</param>
		private void FoundResults(MatchCollection match) {
			foreach (Match item in match) {
				var result = CreateResult(item.Index, item.Value.Length);
				found.Add(result);
			}

			SetButtons();
			SetTextBoxSelection(found[current].Postition, found[current].Length);
			mainWindow.BringToFront();
		}

		/// <summary>
		/// Show or hide the replace options when checked/unchecked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void replaceCheck_CheckedChanged(object sender, EventArgs e) {
			if(replaceCheck.Checked) {
				replaceAllBtn.Show();
				replaceOneBtn.Show();
				replaceText.Show();
			} else {
				replaceAllBtn.Hide();
				replaceOneBtn.Hide();
				replaceText.Hide();
			}
		}

		/// <summary>
		/// Replaces the first instance of the word that is found
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void replaceOneBtn_Click(object sender, EventArgs e) {
			searchButton.PerformClick();
			textbox.SelectedText = replaceText.Text;
		}

		/// <summary>
		/// Performs a search and then iterates over the results replacing the found word with the text specified in the replace box. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void replaceAllBtn_Click(object sender, EventArgs e) {
			searchButton.PerformClick();
			var diff = 0;
			var over = false;
			var length = found[0].Length;
			
			if(length > replaceText.TextLength) {
				diff = length - replaceText.TextLength;
				over = false;
			} else if(length < replaceText.TextLength) {
				diff = replaceText.TextLength - length;
				over = true;
			}

			for(var i = 0; i < found.Count; i++) {
				if(over) {
					SetTextBoxSelection(found[i].Postition + (diff * i), length);
				} else {
					SetTextBoxSelection(found[i].Postition - (diff * i), length);
				}
				textbox.SelectedText = replaceText.Text;
			}
		}
	}
}