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
	internal sealed class XrcdlSocialAccountInfoImplementation : XrcdlSocialAccountInfo
	{
		private readonly XmlElement _account_elem;
		private          Uri?       _uri_prefix;

		public override string AccountName
		{
			get => _account_elem.Value ?? string.Empty;
			set => _account_elem.Value = value;
		}

		public override Uri UriPrefix
		{
			get
			{
				if (_uri_prefix == null) {
					_uri_prefix = new Uri(_account_elem.GetAttribute(Constants.UriPrefix));
				}
				return _uri_prefix;
			}
			set
			{
				_uri_prefix = value;
				_account_elem.SetAttribute(Constants.UriPrefix, _uri_prefix.OriginalString);
			}
		}

		public override string? ServiceName
		{
			get => _account_elem.GetAttribute(Constants.Name);
			set => _account_elem.SetAttribute(Constants.Name, value);
		}

		internal XrcdlSocialAccountInfoImplementation(XrcdlMetadataImplementation metadata, XmlElement socialAccountElement) : base(metadata)
		{
			_account_elem = socialAccountElement;
		}

		public override string GetContactType()
		{
			return Constants.SocialAccountInfo;
		}
	}
}
