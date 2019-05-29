namespace PetriNets
{
	partial class EditLabelName
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tbName = new System.Windows.Forms.TextBox();
			this.btAccept = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbName
			// 
			this.tbName.Location = new System.Drawing.Point(13, 13);
			this.tbName.MaximumSize = new System.Drawing.Size(205, 20);
			this.tbName.MaxLength = 10;
			this.tbName.MinimumSize = new System.Drawing.Size(205, 20);
			this.tbName.Name = "tbName";
			this.tbName.Size = new System.Drawing.Size(205, 20);
			this.tbName.TabIndex = 0;
			// 
			// btAccept
			// 
			this.btAccept.Location = new System.Drawing.Point(13, 40);
			this.btAccept.Name = "btAccept";
			this.btAccept.Size = new System.Drawing.Size(205, 23);
			this.btAccept.TabIndex = 1;
			this.btAccept.Text = "Изменить";
			this.btAccept.UseVisualStyleBackColor = true;
			this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
			// 
			// EditLabelName
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(230, 88);
			this.Controls.Add(this.btAccept);
			this.Controls.Add(this.tbName);
			this.MaximumSize = new System.Drawing.Size(246, 127);
			this.MinimumSize = new System.Drawing.Size(246, 127);
			this.Name = "EditLabelName";
			this.Text = "EditLabelName";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Button btAccept;
	}
}