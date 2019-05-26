namespace PetriNets
{
	partial class Form1
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
			this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
			this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
			this.pause = new System.Windows.Forms.ToolStripButton();
			this.stop = new System.Windows.Forms.ToolStripButton();
			this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
			this.circle = new System.Windows.Forms.ToolStripButton();
			this.rectangle = new System.Windows.Forms.ToolStripButton();
			this.line = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
			this.bindingNavigator1.SuspendLayout();
			this.SuspendLayout();
			// 
			// bindingNavigator1
			// 
			this.bindingNavigator1.AddNewItem = null;
			this.bindingNavigator1.CountItem = null;
			this.bindingNavigator1.DeleteItem = null;
			this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMoveNextItem,
            this.pause,
            this.stop,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.circle,
            this.rectangle,
            this.line,
            this.toolStripButton1,
            this.toolStripButton2});
			this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
			this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
			this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
			this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
			this.bindingNavigator1.MovePreviousItem = null;
			this.bindingNavigator1.Name = "bindingNavigator1";
			this.bindingNavigator1.PositionItem = null;
			this.bindingNavigator1.Size = new System.Drawing.Size(800, 25);
			this.bindingNavigator1.TabIndex = 0;
			this.bindingNavigator1.Text = "bindingNavigator1";
			// 
			// bindingNavigatorSeparator2
			// 
			this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
			this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// bindingNavigatorMoveFirstItem
			// 
			this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
			this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
			this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
			this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
			this.bindingNavigatorMoveFirstItem.Text = "Переместить в начало";
			// 
			// bindingNavigatorMoveNextItem
			// 
			this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
			this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
			this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
			this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
			this.bindingNavigatorMoveNextItem.Text = "Переместить вперед";
			// 
			// pause
			// 
			this.pause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.pause.Image = global::PetriNets.Properties.Resources.pause;
			this.pause.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.pause.Name = "pause";
			this.pause.Size = new System.Drawing.Size(23, 22);
			this.pause.Text = "toolStripButton1";
			// 
			// stop
			// 
			this.stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.stop.Image = global::PetriNets.Properties.Resources.stop;
			this.stop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.stop.Name = "stop";
			this.stop.Size = new System.Drawing.Size(23, 22);
			this.stop.Text = "toolStripButton2";
			// 
			// bindingNavigatorMoveLastItem
			// 
			this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
			this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
			this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
			this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
			this.bindingNavigatorMoveLastItem.Text = "Переместить в конец";
			// 
			// circle
			// 
			this.circle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.circle.Image = global::PetriNets.Properties.Resources.circle;
			this.circle.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.circle.Name = "circle";
			this.circle.Size = new System.Drawing.Size(23, 22);
			this.circle.Text = "toolStripButton1";
			this.circle.Click += new System.EventHandler(this.circle_Click);
			// 
			// rectangle
			// 
			this.rectangle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.rectangle.Image = global::PetriNets.Properties.Resources.rectangle;
			this.rectangle.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.rectangle.Name = "rectangle";
			this.rectangle.Size = new System.Drawing.Size(23, 22);
			this.rectangle.Text = "toolStripButton2";
			this.rectangle.Click += new System.EventHandler(this.rectangle_Click);
			// 
			// line
			// 
			this.line.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.line.Image = global::PetriNets.Properties.Resources.line;
			this.line.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.line.Name = "line";
			this.line.Size = new System.Drawing.Size(23, 22);
			this.line.Text = "toolStripButton3";
			this.line.Click += new System.EventHandler(this.line_Click);
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = global::PetriNets.Properties.Resources.cursour;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "toolStripButton1";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = global::PetriNets.Properties.Resources.cross;
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton2.Text = "toolStripButton2";
			this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.bindingNavigator1);
			this.Name = "Form1";
			this.Text = "Сети Петри";
			this.Click += new System.EventHandler(this.CanvasClick);
			((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
			this.bindingNavigator1.ResumeLayout(false);
			this.bindingNavigator1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.BindingNavigator bindingNavigator1;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
		private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
		private System.Windows.Forms.ToolStripButton circle;
		private System.Windows.Forms.ToolStripButton rectangle;
		private System.Windows.Forms.ToolStripButton line;
		private System.Windows.Forms.ToolStripButton pause;
		private System.Windows.Forms.ToolStripButton stop;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
	}
}