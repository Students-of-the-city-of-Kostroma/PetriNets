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
	public partial class EditNumberOfSomething : Form
	{
		protected internal int numberOfToken = -1;
		public EditNumberOfSomething()
		{
			InitializeComponent();
		}

		private void btOk_Click(object sender, EventArgs e)
		{
			tbTokens.Text = Regex.Replace(tbTokens.Text, @"[^0-9]", "");
			tbTokens.Text = Regex.Replace(tbTokens.Text, @"\s+", "");
			if(tbTokens.Text != "")
				numberOfToken = Math.Abs(int.Parse(tbTokens.Text));
			this.Close();
		}
	}
}
