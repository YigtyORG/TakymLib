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
using Exrecodel.ContactInfo;

namespace Exrecodel.InternalImplementations.ContactInfo
{
	internal sealed class XrcdlLinkInfoImplementation : XrcdlLinkInfo
	{
		private const    string     _link_example = "https://example.com";
		private readonly XmlElement _link_elem;
		private          Uri?       _link;

		public override Uri Link
		{
			get
			{
				if (_link == null) {
					_link = new Uri(_link_elem.Value ?? _link_example);
				}
				return _link;
			}
			set
			{
				_link = value;
				_link_elem.Value = _link.OriginalString;
			}
		}

		public override string? Title
		{
			get => _link_elem.GetAttribute(Constants.Name);
			set => _link_elem.SetAttribute(Constants.Name, value);
		}

		internal XrcdlLinkInfoImplementation(XrcdlMetadataImplementation metadata, XmlElement linkElement) : base(metadata)
		{
			_link_elem = linkElement;
		}

		public override string GetContactType()
		{
			return Constants.LinkInfo;
		}
	}
}
