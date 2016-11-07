namespace notepad {
	partial class Search {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.previous = new System.Windows.Forms.Button();
			this.next = new System.Windows.Forms.Button();
			this.searchButton = new System.Windows.Forms.Button();
			this.textToFind = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.replaceCheck = new System.Windows.Forms.CheckBox();
			this.replaceText = new System.Windows.Forms.TextBox();
			this.replaceOneBtn = new System.Windows.Forms.Button();
			this.replaceAllBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// previous
			// 
			this.previous.Enabled = false;
			this.previous.Location = new System.Drawing.Point(11, 53);
			this.previous.Name = "previous";
			this.previous.Size = new System.Drawing.Size(75, 23);
			this.previous.TabIndex = 3;
			this.previous.Text = "Previous";
			this.previous.UseVisualStyleBackColor = true;
			this.previous.Click += new System.EventHandler(this.previous_Click);
			// 
			// next
			// 
			this.next.Enabled = false;
			this.next.Location = new System.Drawing.Point(106, 53);
			this.next.Name = "next";
			this.next.Size = new System.Drawing.Size(75, 23);
			this.next.TabIndex = 4;
			this.next.Text = "Next";
			this.next.UseVisualStyleBackColor = true;
			this.next.Click += new System.EventHandler(this.next_Click);
			// 
			// searchButton
			// 
			this.searchButton.Location = new System.Drawing.Point(197, 27);
			this.searchButton.Name = "searchButton";
			this.searchButton.Size = new System.Drawing.Size(75, 23);
			this.searchButton.TabIndex = 2;
			this.searchButton.Text = "Search";
			this.searchButton.UseVisualStyleBackColor = true;
			this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
			// 
			// textToFind
			// 
			this.textToFind.Location = new System.Drawing.Point(12, 27);
			this.textToFind.Name = "textToFind";
			this.textToFind.Size = new System.Drawing.Size(179, 20);
			this.textToFind.TabIndex = 1;
			this.textToFind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textToFind_KeyPress);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(105, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Enter the text to find:";
			// 
			// replaceCheck
			// 
			this.replaceCheck.AutoSize = true;
			this.replaceCheck.Location = new System.Drawing.Point(192, 59);
			this.replaceCheck.Name = "replaceCheck";
			this.replaceCheck.Size = new System.Drawing.Size(66, 17);
			this.replaceCheck.TabIndex = 6;
			this.replaceCheck.Text = "Replace";
			this.replaceCheck.UseVisualStyleBackColor = false;
			this.replaceCheck.CheckedChanged += new System.EventHandler(this.replaceCheck_CheckedChanged);
			// 
			// replaceText
			// 
			this.replaceText.Location = new System.Drawing.Point(12, 91);
			this.replaceText.Name = "replaceText";
			this.replaceText.Size = new System.Drawing.Size(179, 20);
			this.replaceText.TabIndex = 7;
			this.replaceText.Visible = false;
			// 
			// replaceOneBtn
			// 
			this.replaceOneBtn.Location = new System.Drawing.Point(12, 117);
			this.replaceOneBtn.Name = "replaceOneBtn";
			this.replaceOneBtn.Size = new System.Drawing.Size(105, 23);
			this.replaceOneBtn.TabIndex = 8;
			this.replaceOneBtn.Text = "Replace This One";
			this.replaceOneBtn.UseVisualStyleBackColor = true;
			this.replaceOneBtn.Visible = false;
			this.replaceOneBtn.Click += new System.EventHandler(this.replaceOneBtn_Click);
			// 
			// replaceAllBtn
			// 
			this.replaceAllBtn.Location = new System.Drawing.Point(123, 117);
			this.replaceAllBtn.Name = "replaceAllBtn";
			this.replaceAllBtn.Size = new System.Drawing.Size(106, 23);
			this.replaceAllBtn.TabIndex = 9;
			this.replaceAllBtn.Text = "Replace All";
			this.replaceAllBtn.UseVisualStyleBackColor = true;
			this.replaceAllBtn.Visible = false;
			this.replaceAllBtn.Click += new System.EventHandler(this.replaceAllBtn_Click);
			// 
			// Search
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 144);
			this.Controls.Add(this.replaceAllBtn);
			this.Controls.Add(this.replaceOneBtn);
			this.Controls.Add(this.replaceText);
			this.Controls.Add(this.replaceCheck);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textToFind);
			this.Controls.Add(this.searchButton);
			this.Controls.Add(this.next);
			this.Controls.Add(this.previous);
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Search";
			this.Text = "Search";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button previous;
		private System.Windows.Forms.Button next;
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.TextBox textToFind;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox replaceCheck;
		private System.Windows.Forms.TextBox replaceText;
		private System.Windows.Forms.Button replaceOneBtn;
		private System.Windows.Forms.Button replaceAllBtn;
	}
}