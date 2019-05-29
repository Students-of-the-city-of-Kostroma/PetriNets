using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PetriNets
{
	public partial class EditLabelName : Form
	{
		internal protected string currentName = ""; 
		public EditLabelName()
		{
			InitializeComponent();
		}

		private void btAccept_Click(object sender, EventArgs e)
		{
			tbName.Text = Regex.Replace(tbName.Text, @"\s+", "");
			currentName = tbName.Text;
			this.Close();
		}
		
	}
}
