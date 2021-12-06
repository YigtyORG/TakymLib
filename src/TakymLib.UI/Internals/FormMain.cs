/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TakymLib.Extensibility;
using TakymLib.UI.Models;
using TakymLib.UI.Views;

namespace TakymLib.UI.Internals
{
	internal sealed partial class FormMain : Form
	{
		private readonly AppHost                                  _app_host;
		private readonly IServiceProvider                         _provider;
		private readonly ILogger                                  _logger;
		private readonly ValueTask<IReadOnlyList<PluginTreeNode>> _loader;
		private          FormSystemSettings?                      _fss;
		private          ElementWindow?                           _pmgr;
		private          PluginManagerView?                       _pmgr_view;
		private readonly PluginManagerModel                       _pmgr_model;

		internal FormMain(AppHost appHost, IServiceProvider provider)
		{
			Debug.Assert(appHost  is not null);
			Debug.Assert(provider is not null);

			_app_host   = appHost;
			_provider   = provider;
			_logger     = provider.GetRequiredService<ILogger<FormMain>>();
			_pmgr_model = new();

			this.InitializeComponent();

#pragma warning disable CA2012 // ValueTask を正しく使用する必要があります
			if (_provider.GetService<ModuleInitializationContext>() is ModuleInitializationContextInternal mici) {
				_loader = LoadModulesAsync(mici.Modules);
				if (_loader.IsCompleted) {
					_pmgr_model.PluginTree = _loader.Result;
				} else {
					_loader.GetAwaiter().OnCompleted(this.WhenCompletedToLoadModules);
				}
			}
#pragma warning restore CA2012 // ValueTask を正しく使用する必要があります

			_logger.LogInformation("The main window is initialized.");
		}

		private void WhenCompletedToLoadModules()
		{
			_pmgr_model.PluginTree = _loader.Result;
		}

		private static async ValueTask<IReadOnlyList<PluginTreeNode>> LoadModulesAsync(List<FeatureModule> modules)
		{
			int count = modules.Count;
			var nodes = new PluginTreeNode[count];
			for (int i = 0; i < count; ++i) {
				nodes[i] = await LoadPluginsAsync(modules[i]).ConfigureAwait(false);
			}
			return Array.AsReadOnly(nodes);

			static async ValueTask<PluginTreeNode> LoadPluginsAsync(IPlugin plugin)
			{
				var result = new List<PluginTreeNode>();
				await foreach (var item in plugin.EnumerateChildrenAsync().ConfigureAwait(false)) {
					result.Add(await LoadPluginsAsync(item).ConfigureAwait(false));
				}
				return new(plugin, result.AsReadOnly());
			}
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

			_fss = new(_provider);
			btnSystemSettings.Text        = _fss.Text;
			btnSystemSettings.ToolTipText = _fss.Text;
			btnSystemSettings.Image       = _fss.Icon.ToBitmap();

			_pmgr = new(this.GetPluginManagerView());
			btnPluginManager.Text        = _pmgr.Text;
			btnPluginManager.ToolTipText = _pmgr.Text;
			btnPluginManager.Image       = _pmgr.Icon.ToBitmap();

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
			this.ShowChildWindow(ref _fss, mwnd => new(mwnd._provider));
		}

		private void btnPluginManager_Click(object sender, EventArgs e)
		{
			_logger.LogInformation("Showing the plugin manager window...");
			this.ShowChildWindow(ref _pmgr, mwnd => new(mwnd.GetPluginManagerView()));
		}

		private void ShowChildWindow<TForm>(ref TForm? form, Func<FormMain, TForm> formFactory) where TForm: Form
		{
			if (form is null || form.IsDisposed) {
				form = formFactory(this);
				form.MdiParent = this;
				form.Show();
			} else {
				if (form.MdiParent != this) {
					form.Visible   = false;
					form.MdiParent = this;
					form.Visible   = true;
				} else if (form.Visible) {
					if (form.WindowState == FormWindowState.Minimized) {
						form.WindowState = FormWindowState.Normal;
					}
					form.Activate();
				} else {
					form.MdiParent = this;
					form.Visible   = true;
				}
			}
		}

		private PluginManagerView GetPluginManagerView()
		{
			if (_pmgr_view is null) {
				_pmgr_view = new PluginManagerView(_pmgr_model);
			}
			return _pmgr_view;
		}
	}
}
