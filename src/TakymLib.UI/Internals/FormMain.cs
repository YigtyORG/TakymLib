/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TakymLib.UI.Internals
{
	internal sealed partial class FormMain : Form
	{
		private readonly AppHost          _app_host;
		private readonly IServiceProvider _provider;
		private readonly ILogger          _logger;

		internal FormMain(AppHost appHost, IServiceProvider provider)
		{
			_app_host = appHost;
			_provider = provider;
			_logger   = provider.GetRequiredService<ILogger<FormMain>>();

			this.InitializeComponent();

			_logger.LogInformation("The main window is initialized.");
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			var v = VersionInfo.Current;
			string  caption   = v.GetCaption();
			string? copyright = v.Copyright;

			_logger.LogDebug($"The {nameof(caption)}   is: \"{{{nameof(caption)}}}\".",   caption);
			_logger.LogDebug($"The {nameof(copyright)} is: \"{{{nameof(copyright)}}}\".", copyright);

			this.Text         = caption;
			lblCopyright.Text = copyright;
		}

		protected override void OnLoad(EventArgs e)
		{
			_logger.LogInformation("Loading...");

			base.OnLoad(e);

			using (var fss = new FormSystemSettings(_provider)) {
				btnSystemSettings.Image = fss.Icon.ToBitmap();
			}

			_logger.LogInformation("Loaded");
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_logger.LogInformation("Closing...");
			base.OnFormClosing(e);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			_logger.LogInformation("Closed");
			_app_host.Shutdown();
		}

		private void btnSystemSettings_Click(object sender, EventArgs e)
		{
			_logger.LogInformation("Showing the system settings window...");
			var fss = new FormSystemSettings(_provider);
			fss.MdiParent = this;
			fss.Show();
		}
	}
}
