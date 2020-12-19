/****
 * TakymLib
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.Serialization;
using TakymLib.Properties;

namespace TakymLib
{
	/// <summary>
	///  型'<see cref="System.Runtime.Serialization.SerializationInfo"/>'クラスを拡張します。
	///  このクラスは静的です。
	/// </summary>
	public static class SerializationInfoExtensions
	{
		/// <summary>
		///  <see cref="System.Runtime.Serialization.SerializationInfo"/>から値を取得します。
		/// </summary>
		/// <typeparam name="T">取得する値の型です。</typeparam>
		/// <param name="info">値が保存されているストアです。</param>
		/// <param name="name">取得する値の名前です。</param>
		/// <returns>
		///  <paramref name="info"/>が保持する<paramref name="name"/>を名前に持つ<typeparamref name="T"/>に変換された値です。
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="info"/>または<paramref name="name"/>が<see langword="null"/>に設定されています。
		/// </exception>
		/// <exception cref="System.InvalidCastException">
		///  取得した値を型<typeparamref name="T"/>に変換する事ができません。
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException" />
		public static T GetValue<T>(this SerializationInfo info, string name)
		{
			info.EnsureNotNull(nameof(info));
			name.EnsureNotNull(nameof(name));
			try {
				object? result = info.GetValue(name, typeof(T));
				if (result is null) {
					return default!;
				} else {
					return ((T)(result));
				}
			} catch (InvalidCastException) {
				throw;
			} catch (Exception e) {
				throw new SerializationException(Resources.SerializationInfoExtensions_SerializationException, e);
			}
		}
	}
}
