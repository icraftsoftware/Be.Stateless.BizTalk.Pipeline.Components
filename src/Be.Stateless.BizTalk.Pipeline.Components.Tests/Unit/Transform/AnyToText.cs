﻿#region Copyright & License

// Copyright © 2012 - 2020 François Chabot
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.Unit.Transform
{
	/// <summary>
	/// Utility transform that derives from <see cref="TransformBase"/> and outputs text instead of XML.
	/// </summary>
	[SchemaReference("Be.Stateless.BizTalk.Schemas.Xml.Any", typeof(Schemas.Xml.Any))]
	public class AnyToText : TransformBase
	{
		static AnyToText()
		{
			_xmlContent = @"<?xml version='1.0' encoding='utf-8'?>
<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0'>
  <xsl:output omit-xml-declaration='yes' method='text' version='1.0' />
  <xsl:template match='/'>Any XML to Text</xsl:template>
</xsl:stylesheet>";
		}

		#region Base Class Member Overrides

		public override string[] SourceSchemas => new[] { typeof(Schemas.Xml.Any).FullName };

		public override string[] TargetSchemas => new[] { typeof(Schemas.Xml.Any).FullName };

		public override string XmlContent => _xmlContent;

		public override string XsltArgumentListContent => "<ExtensionObjects />";

		#endregion

		private static readonly string _xmlContent;
	}
}
