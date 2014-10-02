namespace CVARC.Graphics
{
	partial class CameraSwitchControl
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
			this.CameraViewGroup = new System.Windows.Forms.GroupBox();
			this.DefaultViewRadioButton = new System.Windows.Forms.RadioButton();
			this.FollowRobotRadioButton = new System.Windows.Forms.RadioButton();
			this.TopViewRadioButton = new System.Windows.Forms.RadioButton();
			this.CameraViewGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// CameraViewGroup
			// 
			this.CameraViewGroup.Controls.Add(this.DefaultViewRadioButton);
			this.CameraViewGroup.Controls.Add(this.FollowRobotRadioButton);
			this.CameraViewGroup.Controls.Add(this.TopViewRadioButton);
			this.CameraViewGroup.Location = new System.Drawing.Point(25, 25);
			this.CameraViewGroup.Name = "CameraViewGroup";
			this.CameraViewGroup.Size = new System.Drawing.Size(100, 100);
			this.CameraViewGroup.TabIndex = 7;
			this.CameraViewGroup.TabStop = false;
			this.CameraViewGroup.Text = "Camera";
			// 
			// DefaultViewRadioButton
			// 
			this.DefaultViewRadioButton.AutoSize = true;
			this.DefaultViewRadioButton.Checked = true;
			this.DefaultViewRadioButton.Location = new System.Drawing.Point(6, 19);
			this.DefaultViewRadioButton.Name = "DefaultViewRadioButton";
			this.DefaultViewRadioButton.Size = new System.Drawing.Size(59, 17);
			this.DefaultViewRadioButton.TabIndex = 5;
			this.DefaultViewRadioButton.TabStop = true;
			this.DefaultViewRadioButton.Text = "Default";
			this.DefaultViewRadioButton.UseVisualStyleBackColor = true;
			this.DefaultViewRadioButton.CheckedChanged += new System.EventHandler(this.ButtonCheckedChanged);
			// 
			// FollowRobotRadioButton
			// 
			this.FollowRobotRadioButton.AutoSize = true;
			this.FollowRobotRadioButton.Location = new System.Drawing.Point(6, 65);
			this.FollowRobotRadioButton.Name = "FollowRobotRadioButton";
			this.FollowRobotRadioButton.Size = new System.Drawing.Size(87, 17);
			this.FollowRobotRadioButton.TabIndex = 4;
			this.FollowRobotRadioButton.Text = "Follow Robot";
			this.FollowRobotRadioButton.UseVisualStyleBackColor = true;
			this.FollowRobotRadioButton.CheckedChanged += new System.EventHandler(this.ButtonCheckedChanged);
			// 
			// TopViewRadioButton
			// 
			this.TopViewRadioButton.AutoSize = true;
			this.TopViewRadioButton.Location = new System.Drawing.Point(6, 42);
			this.TopViewRadioButton.Name = "TopViewRadioButton";
			this.TopViewRadioButton.Size = new System.Drawing.Size(70, 17);
			this.TopViewRadioButton.TabIndex = 3;
			this.TopViewRadioButton.Text = "Top View";
			this.TopViewRadioButton.UseVisualStyleBackColor = true;
			this.TopViewRadioButton.CheckedChanged += new System.EventHandler(this.ButtonCheckedChanged);
			// 
			// CameraSwitchControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.CameraViewGroup);
			this.Name = "CameraSwitchControl";
			this.CameraViewGroup.ResumeLayout(false);
			this.CameraViewGroup.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox CameraViewGroup;
		private System.Windows.Forms.RadioButton DefaultViewRadioButton;
		private System.Windows.Forms.RadioButton FollowRobotRadioButton;
		private System.Windows.Forms.RadioButton TopViewRadioButton;
	}
}
