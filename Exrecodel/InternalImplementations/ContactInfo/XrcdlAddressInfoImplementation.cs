/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Exrecodel.ContactInfo;

namespace Exrecodel.InternalImplementations.ContactInfo
{
	internal sealed class XrcdlAddressInfoImplementation : XrcdlAddressInfo
	{
		private readonly XmlElement _address_elem;

		public override long? PostCode
		{
			get
			{
				if (long.TryParse(_address_elem.GetAttribute(Constants.PostCode), out long result)) {
					return result;
				} else {
					return null;
				}
			}
			set => _address_elem.SetAttribute(Constants.PostCode, value.ToString());
		}

		public override string Address
		{
			get => _address_elem.Value ?? string.Empty;
			set => _address_elem.Value = value;
		}

		internal XrcdlAddressInfoImplementation(XrcdlMetadataImplementation metadata, XmlElement addressElement) : base(metadata)
		{
			_address_elem = addressElement;
		}

		public override string GetContactType()
		{
			return Constants.AddressInfo;
		}

		public override IXrcdlConverter GetConverter()
		{
			return new XrcdlConverter(this);
		}

		private readonly struct XrcdlConverter : IXrcdlAsyncConverter
		{
			private readonly XrcdlAddressInfoImplementation _info;

			internal XrcdlConverter(XrcdlAddressInfoImplementation info)
			{
				_info = info;
			}

			public void ConvertToHtml(StringBuilder sb)
			{
				if (sb == null) {
					throw new ArgumentNullException(nameof(sb));
				}
				//
			}

			public async Task ConvertToHtmlAsync(StringBuilder sb)
			{
				if (sb == null) {
					throw new ArgumentNullException(nameof(sb));
				}
				//
			}

			public void Dispose()
			{
				// do nothing
			}

			public ValueTask DisposeAsync()
			{
				// do nothing
				return default;
			}
		}
	}
}
