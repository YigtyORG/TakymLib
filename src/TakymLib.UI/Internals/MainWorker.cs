/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TakymLib.Extensibility;

namespace TakymLib.UI.Internals
{
	internal sealed class MainWorker : IHostedService
	{
		private readonly ILogger                     _logger;
		private readonly ModuleLoader                _loader;
		private readonly ModuleInitializationContext _context;
		private readonly IConfiguration              _config;

		public MainWorker(
			ILogger<MainWorker>         logger,
			ModuleLoader                loader,
			ModuleInitializationContext context,
			IConfiguration              config,
			IHostApplicationLifetime    lifetime)
		{
			logger .EnsureNotNull();
			loader .EnsureNotNull();
			context.EnsureNotNull();
			config .EnsureNotNull();

			_logger  = logger;
			_loader  = loader;
			_context = context;
			_config  = config;

			if (logger.IsEnabled(LogLevel.Trace)) {
				lifetime.EnsureNotNull();
				lifetime.ApplicationStarted .Register(this.ApplicationStarted);
				lifetime.ApplicationStopping.Register(this.ApplicationStopping);
				lifetime.ApplicationStopped .Register(this.ApplicationStopped);
			}
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return Task.FromCanceled(cancellationToken);
			_logger.LogTrace($"{nameof(MainWorker)}.{nameof(StartAsync)}");

			bool disallowExtensions = _config.DisallowExtensions();
			if (_logger.IsEnabled(LogLevel.Debug)) {
				bool showSplash = _config.ShowSplash();
				_logger.LogDebug("{Key} = {Value}", SystemSettings.Keys.DisallowExtensions, disallowExtensions);
				_logger.LogDebug("{Key} = {Value}", SystemSettings.Keys.ShowSplash, showSplash);
			}

			return disallowExtensions
				? Task.CompletedTask
				: Task.Run(() => WaitForLoadModules(_logger, _loader, _context, cancellationToken), cancellationToken);

			static async Task WaitForLoadModules(
				ILogger                     logger,
				ModuleLoader                loader,
				ModuleInitializationContext context,
				CancellationToken           cancellationToken)
			{
				var loadModules = LoadModules(logger, loader, context, cancellationToken).ConfigureAwait(false);
				if (await loadModules is not null and var oce) ExceptionDispatchInfo.Throw(oce);
			}

			static async ValueTask<OperationCanceledException?> LoadModules(
				ILogger                     logger,
				ModuleLoader                loader,
				ModuleInitializationContext context,
				CancellationToken           cancellationToken)
			{
				if (context is ModuleInitializationContextInternal mici) {
					var result  = mici.Modules;
					var modules = loader.LoadFromDirectoryAsync(context, AppContext.BaseDirectory, cancellationToken: cancellationToken);
					try {
						await foreach (var module in modules.ConfigureAwait(false)) {
							result.Add(module);
							cancellationToken.ThrowIfCancellationRequested();
						}
						return null;
					} catch (OperationCanceledException oce) when (oce.CancellationToken == cancellationToken) {
						return oce;
					}
				} else {
					logger.LogWarning("The specified module initialization context does not support loading modules.");
					return null;
				}
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return Task.FromCanceled(cancellationToken);
			_logger.LogTrace($"{nameof(MainWorker)}.{nameof(StopAsync)}");
			return Task.CompletedTask;
		}

		private void ApplicationStarted()
		{
			_logger.LogTrace($"{nameof(MainWorker)}.{nameof(ApplicationStarted)}");
		}

		private void ApplicationStopping()
		{
			_logger.LogTrace($"{nameof(MainWorker)}.{nameof(ApplicationStopping)}");
		}

		private void ApplicationStopped()
		{
			_logger.LogTrace($"{nameof(MainWorker)}.{nameof(ApplicationStopped)}");
		}
	}
}
