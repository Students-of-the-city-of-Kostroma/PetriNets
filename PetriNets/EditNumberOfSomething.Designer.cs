namespace PetriNets
{
	partial class EditNumberOfSomething
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
			this.tbTokens = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btOk = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbTokens
			// 
			this.tbTokens.Location = new System.Drawing.Point(13, 36);
			this.tbTokens.MaxLength = 3;
			this.tbTokens.Name = "tbTokens";
			this.tbTokens.Size = new System.Drawing.Size(204, 20);
			this.tbTokens.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(133, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Новое количество:";
			// 
			// btOk
			// 
			this.btOk.Location = new System.Drawing.Point(13, 63);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(204, 23);
			this.btOk.TabIndex = 2;
			this.btOk.Text = "OK";
			this.btOk.UseVisualStyleBackColor = true;
			this.btOk.Click += new System.EventHandler(this.btOk_Click);
			// 
			// EditNumberOfSomething
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(229, 90);
			this.Controls.Add(this.btOk);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbTokens);
			this.MaximumSize = new System.Drawing.Size(245, 129);
			this.MinimumSize = new System.Drawing.Size(245, 129);
			this.Name = "EditNumberOfSomething";
			this.Text = "EditNumberOfTokens";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbTokens;
		private System.Windows.Forms.Button btOk;
		public System.Windows.Forms.Label label1;
	}
}