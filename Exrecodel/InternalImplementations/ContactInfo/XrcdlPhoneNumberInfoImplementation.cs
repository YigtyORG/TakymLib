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
	internal sealed class XrcdlPhoneNumberInfoImplementation : XrcdlPhoneNumberInfo
	{
		private readonly XmlElement _num_elem;
		private readonly string     _type;

		public override string Number
		{
			get => _num_elem.Value ?? string.Empty;
			set => _num_elem.Value = value;
		}

		public override XrcdlPhoneNumberType Type
		{
			get => _type switch {
				Constants.PhoneNumberInfoTelephone           => XrcdlPhoneNumberType.Telephone,
				Constants.PhoneNumberInfoFacsimile           => XrcdlPhoneNumberType.Facsimile,
				Constants.PhoneNumberInfoShortMessageService => XrcdlPhoneNumberType.ShortMessageService,
				_                                            => XrcdlPhoneNumberType.Unknown
			};
		}

		public XrcdlPhoneNumberInfoImplementation(XrcdlMetadataImplementation metadata, XmlElement phoneNumberElement) : base(metadata)
		{
			_num_elem = phoneNumberElement;
			_type     = _num_elem.LocalName;
		}

		public override string GetContactType()
		{
			return _type;
		}
	}
}
