namespace PetriNets
{
	partial class PetriNet
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
			this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
			this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.circle = new System.Windows.Forms.ToolStripButton();
			this.rectangle = new System.Windows.Forms.ToolStripButton();
			this.line = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.Save = new System.Windows.Forms.ToolStripButton();
			this.Open = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.btPlay = new System.Windows.Forms.ToolStripButton();
			this.stop = new System.Windows.Forms.ToolStripButton();
			this.back = new System.Windows.Forms.ToolStripButton();
			this.tbEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.cmPlace = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.editNumberOfTokens = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmDeletePlace = new System.Windows.Forms.ToolStripMenuItem();
			this.cmArc = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmEditWeight = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmDeleteArc = new System.Windows.Forms.ToolStripMenuItem();
			this.cmTransition = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miEditTransition = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmDeleteTransition = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
			this.bindingNavigator1.SuspendLayout();
			this.cmPlace.SuspendLayout();
			this.cmArc.SuspendLayout();
			this.cmTransition.SuspendLayout();
			this.SuspendLayout();
			// 
			// bindingNavigator1
			// 
			this.bindingNavigator1.AddNewItem = null;
			this.bindingNavigator1.CountItem = null;
			this.bindingNavigator1.DeleteItem = null;
			this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorSeparator2,
            this.circle,
            this.rectangle,
            this.line,
            this.toolStripButton1,
            this.toolStripButton2,
            this.Save,
            this.Open,
            this.toolStripSeparator4,
            this.btPlay,
            this.stop,
            this.back});
			this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
			this.bindingNavigator1.MoveFirstItem = null;
			this.bindingNavigator1.MoveLastItem = null;
			this.bindingNavigator1.MoveNextItem = null;
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
			// Save
			// 
			this.Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.Save.Image = global::PetriNets.Properties.Resources.computer_icons_icon_design_iconfinder_save_icon;
			this.Save.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.Save.Name = "Save";
			this.Save.Size = new System.Drawing.Size(23, 22);
			this.Save.Text = "toolStripButton3";
			this.Save.Click += new System.EventHandler(this.Save_Click);
			// 
			// Open
			// 
			this.Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.Open.Image = global::PetriNets.Properties.Resources._36792_3_folders_file;
			this.Open.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.Open.Name = "Open";
			this.Open.Size = new System.Drawing.Size(23, 22);
			this.Open.Text = "toolStripButton3";
			this.Open.Click += new System.EventHandler(this.Open_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// btPlay
			// 
			this.btPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btPlay.Image = global::PetriNets.Properties.Resources.Animation_mode;
			this.btPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btPlay.Name = "btPlay";
			this.btPlay.Size = new System.Drawing.Size(23, 22);
			this.btPlay.Text = "toolStripButton3";
			this.btPlay.Click += new System.EventHandler(this.bindingNavigatorMoveNextItem_Click);
			// 
			// stop
			// 
			this.stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.stop.Image = global::PetriNets.Properties.Resources.stop;
			this.stop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.stop.Name = "stop";
			this.stop.Size = new System.Drawing.Size(23, 22);
			this.stop.Text = "toolStripButton2";
			this.stop.Click += new System.EventHandler(this.stop_Click);
			// 
			// back
			// 
			this.back.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.back.Enabled = false;
			this.back.Image = global::PetriNets.Properties.Resources.Back;
			this.back.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.back.Name = "back";
			this.back.Size = new System.Drawing.Size(23, 22);
			this.back.Text = "toolStripButton3";
			this.back.Click += new System.EventHandler(this.back_Click);
			// 
			// tbEdit
			// 
			this.tbEdit.Name = "tbEdit";
			this.tbEdit.Size = new System.Drawing.Size(242, 22);
			this.tbEdit.Text = "Изменить название";
			this.tbEdit.Click += new System.EventHandler(this.tbEdit_Click);
			// 
			// cmPlace
			// 
			this.cmPlace.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbEdit,
            this.editNumberOfTokens,
            this.toolStripSeparator3,
            this.tsmDeletePlace});
			this.cmPlace.Name = "cmPlace";
			this.cmPlace.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.cmPlace.Size = new System.Drawing.Size(243, 76);
			// 
			// editNumberOfTokens
			// 
			this.editNumberOfTokens.Name = "editNumberOfTokens";
			this.editNumberOfTokens.Size = new System.Drawing.Size(242, 22);
			this.editNumberOfTokens.Text = "Изменить количество токенов";
			this.editNumberOfTokens.Click += new System.EventHandler(this.editNumberOfTokens_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(239, 6);
			// 
			// tsmDeletePlace
			// 
			this.tsmDeletePlace.Name = "tsmDeletePlace";
			this.tsmDeletePlace.Size = new System.Drawing.Size(242, 22);
			this.tsmDeletePlace.Text = "Удалить";
			this.tsmDeletePlace.Click += new System.EventHandler(this.tsmDelete_Click);
			// 
			// cmArc
			// 
			this.cmArc.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmEditWeight,
            this.toolStripSeparator1,
            this.tsmDeleteArc});
			this.cmArc.Name = "cmArc";
			this.cmArc.Size = new System.Drawing.Size(150, 54);
			// 
			// tsmEditWeight
			// 
			this.tsmEditWeight.Name = "tsmEditWeight";
			this.tsmEditWeight.Size = new System.Drawing.Size(149, 22);
			this.tsmEditWeight.Text = "Изменить вес";
			this.tsmEditWeight.Click += new System.EventHandler(this.tsmEditWeight_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(146, 6);
			// 
			// tsmDeleteArc
			// 
			this.tsmDeleteArc.Name = "tsmDeleteArc";
			this.tsmDeleteArc.Size = new System.Drawing.Size(149, 22);
			this.tsmDeleteArc.Text = "Удалить";
			this.tsmDeleteArc.Click += new System.EventHandler(this.tsmDelete_Click);
			// 
			// cmTransition
			// 
			this.cmTransition.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEditTransition,
            this.toolStripSeparator2,
            this.tsmDeleteTransition});
			this.cmTransition.Name = "cmTransition";
			this.cmTransition.Size = new System.Drawing.Size(182, 54);
			// 
			// miEditTransition
			// 
			this.miEditTransition.Name = "miEditTransition";
			this.miEditTransition.Size = new System.Drawing.Size(181, 22);
			this.miEditTransition.Text = "Изменить название";
			this.miEditTransition.Click += new System.EventHandler(this.tbEdit_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(178, 6);
			// 
			// tsmDeleteTransition
			// 
			this.tsmDeleteTransition.Name = "tsmDeleteTransition";
			this.tsmDeleteTransition.Size = new System.Drawing.Size(181, 22);
			this.tsmDeleteTransition.Text = "Удалить";
			this.tsmDeleteTransition.Click += new System.EventHandler(this.tsmDelete_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// PetriNet
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.bindingNavigator1);
			this.DoubleBuffered = true;
			this.Name = "PetriNet";
			this.Text = "Сети Петри";
			this.Click += new System.EventHandler(this.CanvasClick);
			this.DoubleClick += new System.EventHandler(this.Form1_DoubleClick);
			((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
			this.bindingNavigator1.ResumeLayout(false);
			this.bindingNavigator1.PerformLayout();
			this.cmPlace.ResumeLayout(false);
			this.cmArc.ResumeLayout(false);
			this.cmTransition.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.BindingNavigator bindingNavigator1;
		private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
		private System.Windows.Forms.ToolStripButton circle;
		private System.Windows.Forms.ToolStripButton rectangle;
		private System.Windows.Forms.ToolStripButton line;
		private System.Windows.Forms.ToolStripButton stop;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
		private System.Windows.Forms.ToolStripMenuItem tbEdit;
		private System.Windows.Forms.ContextMenuStrip cmPlace;
		private System.Windows.Forms.ContextMenuStrip cmArc;
		private System.Windows.Forms.ContextMenuStrip cmTransition;
		private System.Windows.Forms.ToolStripMenuItem miEditTransition;
		private System.Windows.Forms.ToolStripMenuItem tsmDeleteTransition;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem tsmDeletePlace;
		private System.Windows.Forms.ToolStripMenuItem tsmEditWeight;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem tsmDeleteArc;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem editNumberOfTokens;
		private System.Windows.Forms.ToolStripButton Save;
		private System.Windows.Forms.ToolStripButton Open;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ToolStripButton btPlay;
		private System.Windows.Forms.ToolStripButton back;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
	}
}