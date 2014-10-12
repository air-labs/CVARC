using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CVARC.Core;

namespace CVARC.Graphics
{
	public sealed partial class ScoreDisplayControl : UserControl
	{
		public ScoreDisplayControl()
		{
			InitializeComponent();
			_scoreBoxes.Add(ScoreLabel0);
			_scoreBoxes.Add(ScoreLabel1);
			BackColor = Color.Transparent;
			BringToFront();
			Invalidate(true);
			ParentChanged += (o, e) =>
				{
					SetAnchor();
					if(TopLevelControl != null)
						TopLevelControl.Resize += (obj, args) => SetAnchor();
				};
		}

		public void UpdateDisplayedScores()
		{
		    try
		    {
		        this.EasyInvoke(x => x.InternalUpdate());
		    }
		    catch (Exception e)
		    {
		    }
		}

		public ScoreCollection Scores
		{
			get { return _scores; }
			set
			{
				if(_scores != null)
					_scores.ScoresChanged -= UpdateDisplayedScores;
				_scores = value;
				_scores.ScoresChanged += UpdateDisplayedScores;
			}
		}

		private void SetAnchor()
		{
			if(TopLevelControl == null)
				return;
			Location = new Point(TopLevelControl.ClientSize.Width - ClientSize.Width, ClientSize.Height / 2);
		}

		private void InternalUpdate()
		{
			for(int i = 0; i < Scores.RobotCount; i++)
			{
				int sum = Scores.GetFullSumForRobot(i);
				_scoreBoxes[i].Text = sum.ToString(CultureInfo.InvariantCulture);
			}
		}

		/// <summary>
		/// Показывает окно с подробным перечислением очков
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DetailsLinkLabelClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if(Scores == null)
				return;
			var form = new Form
			           	{
			           		Size = new Size(400, 200),
			           		AutoSize = false,
			           		MinimizeBox = false,
			           		MaximizeBox = false,
			           		Padding = new Padding(10),
			           		FormBorderStyle = FormBorderStyle.FixedDialog
			           	};
			var outerPanel = new TableLayoutPanel {AutoSize = true, ColumnCount = 2, RowCount = 2};
			form.Controls.Add(outerPanel);
			for(int rNum = 0; rNum < 2; rNum++)
			{
				string penalties = string.Concat(Scores.GetPenalties(rNum).Select(x => x + "\r\n"));
				string text = string.Format(
					"Robot {0}:\r\n " +
					"Temporary scores: {1}\r\n" +
					"Penalties:\r\n{2}", rNum, Scores.GetTemp(rNum), penalties);
				var label = new Label
				            	{
				            		AutoSize = true,
				            		TabIndex = rNum,
				            		Anchor = (rNum == 0 ? AnchorStyles.Left : AnchorStyles.Right) | AnchorStyles.Top,
				            		Text = text
				            	};
				outerPanel.Controls.Add(label, rNum, 0);
				form.AutoScrollMinSize = label.Size;
			}
			form.ShowDialog();
		}

		private ScoreCollection _scores;

		private readonly List<Label> _scoreBoxes = new List<Label>();
	}
}