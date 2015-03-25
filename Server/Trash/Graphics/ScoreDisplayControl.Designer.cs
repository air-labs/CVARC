namespace CVARC.Graphics
{
	sealed partial class ScoreDisplayControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.ScoreLabel0 = new System.Windows.Forms.Label();
			this.DetailsLinkLabel = new System.Windows.Forms.LinkLabel();
			this.ScoreLabel1 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Controls.Add(this.ScoreLabel0);
			this.panel1.Controls.Add(this.DetailsLinkLabel);
			this.panel1.Controls.Add(this.ScoreLabel1);
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(236, 100);
			this.panel1.TabIndex = 8;
			// 
			// ScoreLabel0
			// 
			this.ScoreLabel0.AutoSize = true;
			this.ScoreLabel0.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ScoreLabel0.ForeColor = System.Drawing.Color.Red;
			this.ScoreLabel0.Location = new System.Drawing.Point(23, 0);
			this.ScoreLabel0.Name = "ScoreLabel0";
			this.ScoreLabel0.Size = new System.Drawing.Size(51, 55);
			this.ScoreLabel0.TabIndex = 1;
			this.ScoreLabel0.Text = "0";
			// 
			// DetailsLinkLabel
			// 
			this.DetailsLinkLabel.AutoSize = true;
			this.DetailsLinkLabel.BackColor = System.Drawing.Color.Transparent;
			this.DetailsLinkLabel.Location = new System.Drawing.Point(64, 66);
			this.DetailsLinkLabel.Name = "DetailsLinkLabel";
			this.DetailsLinkLabel.Size = new System.Drawing.Size(79, 13);
			this.DetailsLinkLabel.TabIndex = 0;
			this.DetailsLinkLabel.TabStop = true;
			this.DetailsLinkLabel.Text = "Score Details...";
			this.DetailsLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.DetailsLinkLabelClicked);
			// 
			// ScoreLabel1
			// 
			this.ScoreLabel1.AutoSize = true;
			this.ScoreLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ScoreLabel1.ForeColor = System.Drawing.Color.Blue;
			this.ScoreLabel1.Location = new System.Drawing.Point(133, 0);
			this.ScoreLabel1.Name = "ScoreLabel1";
			this.ScoreLabel1.Size = new System.Drawing.Size(51, 55);
			this.ScoreLabel1.TabIndex = 2;
			this.ScoreLabel1.Text = "0";
			// 
			// ScoreDisplayControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Name = "ScoreDisplayControl";
			this.Size = new System.Drawing.Size(242, 106);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label ScoreLabel0;
		private System.Windows.Forms.LinkLabel DetailsLinkLabel;
		private System.Windows.Forms.Label ScoreLabel1;
	}
}
