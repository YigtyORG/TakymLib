/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Xml;

namespace Exrecodel.InternalImplementations
{
	internal sealed class XrcdlMetadataImplementation : XrcdlMetadata
	{
		private readonly XrcdlDocumentImplementation _doc;

		public override string Name
		{
			get => this.GetElement(Constants.Name).Value ?? string.Empty;
			set => this.GetElement(Constants.Name).Value = value;
		}

		public override XrcdlDocumentType Type
		{
			get
			{
				if (Enum.TryParse<XrcdlDocumentType>(this.GetElement(Constants.Type).Value, true, out var result)) {
					return result;
				} else {
					return XrcdlDocumentType.Unknown;
				}
			}
			set => this.GetElement(Constants.Type).Value = value.ToString().ToLower();
		}

		public override string? BaseDocumentPath
		{
			get => this.GetElement(Constants.BaseDocumentPath).Value;
			set => this.GetElement(Constants.BaseDocumentPath).Value = value;
		}

		public override string? Title
		{
			get => this.GetElement(Constants.Title).Value;
			set => this.GetElement(Constants.Title).Value = value;
		}

		public override string? Author
		{
			get => this.GetElement(Constants.Author).Value;
			set => this.GetElement(Constants.Author).Value = value;
		}

		public override string? Copyright
		{
			get => this.GetElement(Constants.Copyright).Value;
			set => this.GetElement(Constants.Copyright).Value = value;
		}

		public override string? VersionString
		{
			get => this.GetElement(Constants.VersionString).Value;
			set => this.GetElement(Constants.VersionString).Value = value;
		}

		public override DateTime? Creation
		{
			get
			{
				string s = this.GetElement(Constants.Creation).Value;
				if (string.IsNullOrEmpty(s)) {
					return null;
				} else {
					try {
						return XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.Unspecified);
					} catch {
						return null;
					}
				}
			}
			set
			{
				if (value.HasValue) {
					this.GetElement(Constants.Creation).Value = XmlConvert.ToString(value.Value, XmlDateTimeSerializationMode.Unspecified);
				} else {
					this.GetElement(Constants.Creation).Value = null;
				}
			}
		}

		public override DateTime? LastModified
		{
			get
			{
				string s = this.GetElement(Constants.LastModified).Value;
				if (string.IsNullOrEmpty(s)) {
					return null;
				} else {
					try {
						return XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.Unspecified);
					} catch {
						return null;
					}
				}
			}
			set
			{
				if (value.HasValue) {
					this.GetElement(Constants.LastModified).Value = XmlConvert.ToString(value.Value, XmlDateTimeSerializationMode.Unspecified);
				} else {
					this.GetElement(Constants.LastModified).Value = null;
				}
			}
		}

		public override IXrcdlContactInformationList Contact { get; }

		internal XrcdlMetadataImplementation(XrcdlDocumentImplementation document)
			: base(document)
		{
			_doc         = document;
			this.Contact = new XrcdlContactInformationList(this, this.GetElement(Constants.Contact));
		}

		private XmlElement GetElement(params string[] names)
		{
			string[] names2 = new string[names.Length + 1];
			names2[0] = Constants.Metadata;
			Array.Copy(names, 0, names2, 1, names.Length);
			return _doc.GetElement(names2);
		}

		internal XrcdlDocumentImplementation GetDocument()
		{
			return _doc;
		}
	}
}
