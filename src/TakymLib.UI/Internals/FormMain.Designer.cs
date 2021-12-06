
namespace TakymLib.UI.Internals
{
	partial class FormMain
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.statusBar = new System.Windows.Forms.StatusStrip();
			this.btnSystemSettings = new System.Windows.Forms.ToolStripButton();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblCopyright = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusBar.SuspendLayout();
			this.SuspendLayout();
			//
			// mainMenu
			//
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(800, 24);
			this.mainMenu.TabIndex = 1;
			this.mainMenu.Text = "mainMenu";
			//
			// statusBar
			//
			this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSystemSettings,
            this.lblStatus,
            this.lblCopyright});
			this.statusBar.Location = new System.Drawing.Point(0, 428);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(800, 22);
			this.statusBar.TabIndex = 2;
			this.statusBar.Text = "statusBar";
			//
			// btnSystemSettings
			//
			this.btnSystemSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSystemSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnSystemSettings.Image")));
			this.btnSystemSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSystemSettings.Name = "btnSystemSettings";
			this.btnSystemSettings.Size = new System.Drawing.Size(16, 17);
			this.btnSystemSettings.Text = "btnSystemSettings";
			this.btnSystemSettings.Click += new System.EventHandler(this.btnSystemSettings_Click);
			//
			// lblStatus
			//
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(666, 17);
			this.lblStatus.Spring = true;
			this.lblStatus.Text = "lblStatus";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// lblCopyright
			//
			this.lblCopyright.Name = "lblCopyright";
			this.lblCopyright.Size = new System.Drawing.Size(72, 17);
			this.lblCopyright.Text = "lblCopyright";
			//
			// FormMain
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.mainMenu);
			this.IsMdiContainer = true;
			this.MainMenuStrip = this.mainMenu;
			this.Name = "FormMain";
			this.Text = "FormMain";
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.statusBar.ResumeLayout(false);
			this.statusBar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.StatusStrip statusBar;
		private System.Windows.Forms.ToolStripStatusLabel lblCopyright;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.ToolStripButton btnSystemSettings;
	}
}
