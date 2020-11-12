/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System.Xml;
using Exrecodel.ContactInfo;

namespace Exrecodel.InternalImplementations.ContactInfo
{
	internal sealed class XrcdlAddressInfoImplementation : XrcdlAddressInfo
	{
		private readonly XmlElement _address_elem;

		public override long? ZipCode
		{
			get
			{
				if (long.TryParse(_address_elem.GetAttribute(Constants.ZipCode), out long result)) {
					return result;
				} else {
					return null;
				}
			}
			set => _address_elem.SetAttribute(Constants.ZipCode, value.ToString());
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
	}
}
