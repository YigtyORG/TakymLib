/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TakymLib.Aspect;

namespace TakymLib.Extensibility
{
	/// <summary>
	///  拡張機能を表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class FeatureModule : IPlugin
	{
		private readonly Assembly    _asm;
		private readonly VersionInfo _ver;

		/// <summary>
		///  この拡張機能のアセンブリ情報を取得します。
		/// </summary>
		public Assembly Assembly => _asm;

		/// <summary>
		///  この拡張機能の翻訳済みの表示名を取得します。
		/// </summary>
		/// <remarks>
		///  上書きしない場合、<see cref="TakymLib.VersionInfo.DisplayName"/>の値を返します。
		/// </remarks>
		public virtual string? DisplayName => this.Version.DisplayName;

		/// <summary>
		///  この拡張機能の翻訳済みの説明を取得します。
		/// </summary>
		/// <remarks>
		///  上書きしない場合、<see cref="TakymLib.VersionInfo.Description"/>の値を返します。
		/// </remarks>
		public virtual string? Description => this.Version.Description;

		/// <summary>
		///  この拡張機能のバージョン情報を取得します。
		/// </summary>
		public virtual VersionInfo Version => _ver;

		/// <summary>
		///  型'<see cref="TakymLib.Extensibility.FeatureModule"/>'の新しいインスタンスを生成します。
		/// </summary>
		protected FeatureModule()
		{
			_asm = this.GetType().Assembly;
			_ver = new VersionInfo(_asm);
		}

		protected private FeatureModule(Assembly asm)
		{
			_asm = asm;
			_ver = new VersionInfo(asm);
		}

		/// <summary>
		///  拡張機能を非同期的に初期化します。
		/// </summary>
		/// <param name="context"><see cref="TakymLib.Extensibility.ModuleInitializationContext"/>オブジェクトを指定します。</param>
		/// <param name="cancellationToken">処理の中断を通知するトークンを指定します。この引数は省略可能です。</param>
		/// <returns>この処理の非同期操作を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.OperationCanceledException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		public async LoggableTask InitializeAsync(ModuleInitializationContext context, CancellationToken cancellationToken = default)
		{
			context.EnsureNotNull();
			cancellationToken.ThrowIfCancellationRequested();
			await this.InitializeAsyncCore(context, cancellationToken);
		}

		/// <summary>
		///  この拡張機能に含まれる追加機能を列挙します。
		/// </summary>
		/// <returns><see cref="System.Collections.Generic.IEnumerable{T}"/>オブジェクトを返します。</returns>
		public IEnumerable<IPlugin> EnumerateChildren()
		{
			return this.EnumerateChildrenAsyncCore()?.ToEnumerable() ?? Enumerable.Empty<IPlugin>();
		}

		/// <summary>
		///  この拡張機能に含まれる追加機能を非同期的に列挙します。
		/// </summary>
		/// <returns><see cref="System.Collections.Generic.IAsyncEnumerable{T}"/>オブジェクトを返します。</returns>
		public IAsyncEnumerable<IPlugin> EnumerateChildrenAsync()
		{
			return this.EnumerateChildrenAsyncCore() ?? AsyncEnumerable.Empty<IPlugin>();
		}

		/// <summary>
		///  上書きされた場合、この拡張機能に含まれる追加機能を非同期的に列挙します。
		/// </summary>
		/// <returns><see cref="System.Collections.Generic.IAsyncEnumerable{T}"/>オブジェクトを返します。</returns>
		protected virtual IAsyncEnumerable<IPlugin>? EnumerateChildrenAsyncCore()
		{
			return null;
		}

		/// <summary>
		///  上書きされた場合、拡張機能を非同期的に初期化します。
		/// </summary>
		/// <param name="context"><see cref="TakymLib.Extensibility.ModuleInitializationContext"/>オブジェクトを指定します。</param>
		/// <param name="cancellationToken">処理の中断を通知するトークンを指定します。</param>
		/// <returns>この処理の非同期操作を返します。</returns>
		protected abstract ValueTask InitializeAsyncCore(ModuleInitializationContext context, CancellationToken cancellationToken);
	}
}
