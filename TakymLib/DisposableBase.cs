/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TakymLib.Properties;

namespace TakymLib
{
	/// <summary>
	///  破棄可能なオブジェクトの基底クラスです。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class DisposableBase : IDisposable, IAsyncDisposable
	{
		/// <summary>
		///  このオブジェクトの破棄処理を実行している場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsDisposing { get; private set; }

		/// <summary>
		///  このオブジェクトが破棄されている場合は<see langword="true"/>、有効な場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsDisposed { get; private set; }

		/// <summary>
		///  型'<see cref="TakymLib.DisposableBase"/>'の新しいインスタンスを生成します。
		/// </summary>
		protected DisposableBase()
		{
			this.IsDisposed = false;
		}

		/// <summary>
		///  型'<see cref="TakymLib.DisposableBase"/>'の現在のインスタンスを破棄します。
		/// </summary>
		~DisposableBase()
		{
			this.IsDisposing = true;
			this.Dispose(false);
		}

		/// <summary>
		///  現在のオブジェクトインスタンスと利用しているリソースを破棄します。
		/// </summary>
		public void Dispose()
		{
			this.IsDisposing = true;
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

#pragma warning disable CA1816 // Dispose メソッドは、SuppressFinalize を呼び出す必要があります
		/// <summary>
		///  現在のオブジェクトインスタンスと利用しているリソースを非同期で破棄します。
		/// </summary>
		public async ValueTask DisposeAsync()
		{
			this.IsDisposing = true;
			await this.DisposeAsyncCore();
			this.Dispose(false);
			GC.SuppressFinalize(this);
		}
#pragma warning restore CA1816 // Dispose メソッドは、SuppressFinalize を呼び出す必要があります

		/// <summary>
		///  現在のオブジェクトインスタンスと利用しているリソースを破棄します。
		///  この関数内で例外を発生させてはいけません。
		/// </summary>
		/// <param name="disposing">
		///  マネージドオブジェクトとアンマネージオブジェクト両方を破棄する場合は<see langword="true"/>、
		///  アンマネージオブジェクトのみを破棄する場合は<see langword="false"/>を設定します。
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			this.IsDisposed = true;
		}

		/// <summary>
		///  現在のオブジェクトインスタンスと利用しているリソースを非同期で破棄します。
		///  この関数内で例外を発生させてはいけません。
		/// </summary>
		protected virtual ValueTask DisposeAsyncCore()
		{
			return default;
		}

		/// <summary>
		///  現在のインスタンスが破棄されている場合に例外を発生させます。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException" />
		[DebuggerHidden()]
		[StackTraceHidden()]
		protected void EnsureNotDisposed()
		{
			if (this.IsDisposing) {
				throw new ObjectDisposedException(this.GetType().Name, Resources.DisposableBase_ObjectDisposedException_IsDisposing);
			}
			if (this.IsDisposed) {
				throw new ObjectDisposedException(this.GetType().Name);
			}
		}

		/// <summary>
		///  <see cref="TakymLib.DisposableBase.EnsureNotDisposed"/>を呼び出します。
		/// </summary>
		/// <remarks>
		///  デバッグログまたはスタックトレースへログ出力を行う場合に利用します。
		/// </remarks>
		/// <exception cref="System.ObjectDisposedException" />
		protected void ThrowIfDisposed()
		{
			this.LogThrowIfDisposed();
			this.EnsureNotDisposed();
		}

		[DebuggerHidden()]
		[StackTraceHidden()]
		[Conditional("DEBUG")]
		private void LogThrowIfDisposed()
		{
			Debug.WriteLineIf( this.IsDisposing, $"{this.GetType().Name}.{nameof(this.IsDisposing)} == {true}");
			Debug.WriteLineIf( this.IsDisposed,  $"{this.GetType().Name}.{nameof(this.IsDisposed)}  == {true}");
			Debug.Assert     (!this.IsDisposing, $"{this.GetType().Name} is disposing.");
			Debug.Assert     (!this.IsDisposed,  $"{this.GetType().Name} is disposed.");
		}
	}
}

namespace System.Diagnostics
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Struct, Inherited = false)]
	internal sealed class StackTraceHiddenAttribute : Attribute { }
}
