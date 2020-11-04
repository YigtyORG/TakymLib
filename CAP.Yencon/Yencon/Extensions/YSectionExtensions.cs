/****
 * Yencon - "Yencon Environment Configuration"
 *    「ヱンコン環境設定ファイル」
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;

namespace CAP.Yencon.Extensions
{
	/// <summary>
	///  型'<see cref="CAP.Yencon.YSection"/>'の機能を拡張します。
	/// </summary>
	public static class YSectionExtensions
	{
		/// <summary>
		///  指定されたセクションに新しい文字列値を作成し追加します。
		/// </summary>
		/// <param name="section">新しい文字列値の保存先のセクションです。</param>
		/// <param name="name">新しい文字列値の名前です。</param>
		/// <param name="value">文字列値です。</param>
		/// <returns>
		///  新しい文字列値を表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  またはサポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YString? CreateString(this YSection section, string name, string value)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			var result = section.CreateString(name);
			if (result != null) {
				result.Value = value;
			}
			return result;
		}

		/// <summary>
		///  指定されたセクションに新しい64ビット符号付き整数値を作成し追加します。
		/// </summary>
		/// <param name="section">新しい64ビット符号付き整数値の保存先のセクションです。</param>
		/// <param name="name">新しい64ビット符号付き整数値の名前です。</param>
		/// <param name="value">64ビット符号付き整数値です。</param>
		/// <returns>
		///  新しい64ビット符号付き整数値を表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  またはサポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YNumber? CreateNumber(this YSection section, string name, long value)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			var result = section.CreateNumber(name);
			if (result != null) {
				result.ValueS64= value;
			}
			return result;
		}

		/// <summary>
		///  指定されたセクションに新しい64ビット符号無し整数値を作成し追加します。
		/// </summary>
		/// <param name="section">新しい64ビット符号無し整数値の保存先のセクションです。</param>
		/// <param name="name">新しい64ビット符号無し整数値の名前です。</param>
		/// <param name="value">64ビット符号無し整数値です。</param>
		/// <returns>
		///  新しい64ビット符号無し整数値を表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  またはサポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YNumber? CreateNumber(this YSection section, string name, ulong value)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			var result = section.CreateNumber(name);
			if (result != null) {
				result.ValueU64 = value;
			}
			return result;
		}

		/// <summary>
		///  指定されたセクションに新しい倍精度浮動小数点数値を作成し追加します。
		/// </summary>
		/// <param name="section">新しい倍精度浮動小数点数値の保存先のセクションです。</param>
		/// <param name="name">新しい倍精度浮動小数点数値の名前です。</param>
		/// <param name="value">倍精度浮動小数点数値です。</param>
		/// <returns>
		///  新しい倍精度浮動小数点数値を表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  またはサポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YNumber? CreateNumber(this YSection section, string name, double value)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			var result = section.CreateNumber(name);
			if (result != null) {
				result.ValueDF = value;
			}
			return result;
		}

		/// <summary>
		///  指定されたセクションに新しい10進数値を作成し追加します。
		/// </summary>
		/// <param name="section">新しい10進数値の保存先のセクションです。</param>
		/// <param name="name">新しい10進数値の名前です。</param>
		/// <param name="value">10進数値です。</param>
		/// <returns>
		///  新しい10進数値を表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  またはサポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YNumber? CreateNumber(this YSection section, string name, decimal value)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			var result = section.CreateNumber(name);
			if (result != null) {
				result.ValueM = value;
			}
			return result;
		}

		/// <summary>
		///  指定されたセクションに新しい論理値を作成し追加します。
		/// </summary>
		/// <param name="section">新しい論理値の保存先のセクションです。</param>
		/// <param name="name">新しい論理値の名前です。</param>
		/// <param name="value">論理値です。</param>
		/// <returns>
		///  新しい論理値を表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  またはサポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YBoolean? CreateBoolean(this YSection section, string name, bool value)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			var result = section.CreateBoolean(name);
			if (result != null) {
				result.Value = value;
			}
			return result;
		}

		/// <summary>
		///  指定されたセクションに新しいリンク文字列を作成し追加します。
		/// </summary>
		/// <param name="section">新しいリンク文字列の保存先のセクションです。</param>
		/// <param name="name">新しいリンク文字列の名前です。</param>
		/// <param name="value">リンク文字列です。</param>
		/// <returns>
		///  新しいリンク文字列を表すオブジェクトです。
		///  <paramref name="name"/>が既に存在するノードと一致した場合、
		///  またはサポートされない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YLink? CreateLink(this YSection section, string name, string value)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			var result = section.CreateLink(name);
			if (result != null) {
				result.Value = value;
			}
			return result;
		}

		/// <summary>
		///  指定されたセクションから指定された名前の空値を取得します。
		/// </summary>
		/// <param name="section">取得元のセクションです。</param>
		/// <param name="name">取得する空値の名前です。</param>
		/// <returns>取得に成功した場合は空値を表すオブジェクト、失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YEmpty? GetEmpty(this YSection section, string name)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			section.TryGetNode<YEmpty>(name, out var result);
			return result;
		}

		/// <summary>
		///  指定されたセクションから指定された名前のセクションを取得します。
		/// </summary>
		/// <param name="section">取得元のセクションです。</param>
		/// <param name="name">取得するセクションの名前です。</param>
		/// <returns>取得に成功した場合はセクションを表すオブジェクト、失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YSection? GetSection(this YSection section, string name)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			section.TryGetNode<YSection>(name, out var result);
			return result;
		}

		/// <summary>
		///  指定されたセクションから指定された名前の配列を取得します。
		/// </summary>
		/// <param name="section">取得元のセクションです。</param>
		/// <param name="name">取得する配列の名前です。</param>
		/// <returns>取得に成功した場合は配列を表すオブジェクト、失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YArray? GetArray(this YSection section, string name)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			section.TryGetNode<YArray>(name, out var result);
			return result;
		}

		/// <summary>
		///  指定されたセクションから指定された名前の文字列値を取得します。
		/// </summary>
		/// <param name="section">取得元のセクションです。</param>
		/// <param name="name">取得する文字列値の名前です。</param>
		/// <returns>取得に成功した場合は文字列値を表すオブジェクト、失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YString? GetString(this YSection section, string name)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			section.TryGetNode<YString>(name, out var result);
			return result;
		}

		/// <summary>
		///  指定されたセクションから指定された名前の数値を取得します。
		/// </summary>
		/// <param name="section">取得元のセクションです。</param>
		/// <param name="name">取得する数値の名前です。</param>
		/// <returns>取得に成功した場合は数値を表すオブジェクト、失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YNumber? GetNumber(this YSection section, string name)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			section.TryGetNode<YNumber>(name, out var result);
			return result;
		}

		/// <summary>
		///  指定されたセクションから指定された名前の論理値を取得します。
		/// </summary>
		/// <param name="section">取得元のセクションです。</param>
		/// <param name="name">取得する論理値の名前です。</param>
		/// <returns>取得に成功した場合は論理値を表すオブジェクト、失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YBoolean? GetBoolean(this YSection section, string name)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			section.TryGetNode<YBoolean>(name, out var result);
			return result;
		}

		/// <summary>
		///  指定されたセクションから指定された名前のリンク文字列を取得します。
		/// </summary>
		/// <param name="section">取得元のセクションです。</param>
		/// <param name="name">取得するリンク文字列の名前です。</param>
		/// <returns>取得に成功した場合はリンク文字列を表すオブジェクト、失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YLink? GetLink(this YSection section, string name)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			section.TryGetNode<YLink>(name, out var result);
			return result;
		}

		/// <summary>
		///  指定されたセクションへ指定された名前の空値を設定します。
		/// </summary>
		/// <remarks>
		///  同名の値が既に存在する場合は削除されます。
		/// </remarks>
		/// <param name="section">設定先のセクションです。</param>
		/// <param name="name">設定する空値の名前です。</param>
		/// <returns>設定に成功した場合は空値を表すオブジェクト、失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YEmpty? SetEmpty(this YSection section, string name)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			section.RemoveNode(name);
			return section.CreateEmpty(name);
		}

		/// <summary>
		///  指定されたセクションへ指定された名前の文字列値を設定します。
		/// </summary>
		/// <remarks>
		///  同名の値が既に存在する場合は削除されます。
		/// </remarks>
		/// <param name="section">設定先のセクションです。</param>
		/// <param name="name">設定する文字列値の名前です。</param>
		/// <param name="value">文字列値です。</param>
		/// <returns>設定に成功した場合は文字列値を表すオブジェクト、失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YString? SetString(this YSection section, string name, string value)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			section.RemoveNode(name);
			return section.CreateString(name, value);
		}

		/// <summary>
		///  指定されたセクションへ指定された名前の64ビット符号付き整数値を設定します。
		/// </summary>
		/// <remarks>
		///  同名の値が既に存在する場合は削除されます。
		/// </remarks>
		/// <param name="section">設定先のセクションです。</param>
		/// <param name="name">設定する64ビット符号付き整数値の名前です。</param>
		/// <param name="value">64ビット符号付き整数値です。</param>
		/// <returns>設定に成功した場合は64ビット符号付き整数値を表すオブジェクト、失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YNumber? SetNumber(this YSection section, string name, long value)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			section.RemoveNode(name);
			return section.CreateNumber(name, value);
		}

		/// <summary>
		///  指定されたセクションへ指定された名前の64ビット符号無し整数値を設定します。
		/// </summary>
		/// <remarks>
		///  同名の値が既に存在する場合は削除されます。
		/// </remarks>
		/// <param name="section">設定先のセクションです。</param>
		/// <param name="name">設定する64ビット符号無し整数値の名前です。</param>
		/// <param name="value">64ビット符号無し整数値です。</param>
		/// <returns>設定に成功した場合は64ビット符号無し整数値を表すオブジェクト、失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YNumber? SetNumber(this YSection section, string name, ulong value)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			section.RemoveNode(name);
			return section.CreateNumber(name, value);
		}

		/// <summary>
		///  指定されたセクションへ指定された名前の倍精度浮動小数点数値を設定します。
		/// </summary>
		/// <remarks>
		///  同名の値が既に存在する場合は削除されます。
		/// </remarks>
		/// <param name="section">設定先のセクションです。</param>
		/// <param name="name">設定する倍精度浮動小数点数値の名前です。</param>
		/// <param name="value">倍精度浮動小数点数値です。</param>
		/// <returns>設定に成功した場合は倍精度浮動小数点数値を表すオブジェクト、失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YNumber? SetNumber(this YSection section, string name, double value)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			section.RemoveNode(name);
			return section.CreateNumber(name, value);
		}

		/// <summary>
		///  指定されたセクションへ指定された名前の10進数値を設定します。
		/// </summary>
		/// <remarks>
		///  同名の値が既に存在する場合は削除されます。
		/// </remarks>
		/// <param name="section">設定先のセクションです。</param>
		/// <param name="name">設定する10進数値の名前です。</param>
		/// <param name="value">10進数値です。</param>
		/// <returns>設定に成功した場合は10進数値を表すオブジェクト、失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YNumber? SetNumber(this YSection section, string name, decimal value)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			section.RemoveNode(name);
			return section.CreateNumber(name, value);
		}

		/// <summary>
		///  指定されたセクションへ指定された名前の論理値を設定します。
		/// </summary>
		/// <remarks>
		///  同名の値が既に存在する場合は削除されます。
		/// </remarks>
		/// <param name="section">設定先のセクションです。</param>
		/// <param name="name">設定する論理値の名前です。</param>
		/// <param name="value">論理値です。</param>
		/// <returns>設定に成功した場合は論理値を表すオブジェクト、失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YBoolean? SetBoolean(this YSection section, string name, bool value)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			section.RemoveNode(name);
			return section.CreateBoolean(name, value);
		}

		/// <summary>
		///  指定されたセクションへ指定された名前のリンク文字列を設定します。
		/// </summary>
		/// <remarks>
		///  同名の値が既に存在する場合は削除されます。
		/// </remarks>
		/// <param name="section">設定先のセクションです。</param>
		/// <param name="name">設定するリンク文字列の名前です。</param>
		/// <param name="value">リンク文字列です。</param>
		/// <returns>設定に成功した場合はリンク文字列を表すオブジェクト、失敗した場合は<see langword="null"/>を返します。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static YLink? SetLink(this YSection section, string name, string value)
		{
			if (section == null) {
				throw new ArgumentNullException(nameof(section));
			}
			section.RemoveNode(name);
			return section.CreateLink(name, value);
		}
	}
}
