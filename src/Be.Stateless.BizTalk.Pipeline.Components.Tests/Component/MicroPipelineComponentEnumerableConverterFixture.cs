#region Copyright & License

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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Be.Stateless.BizTalk.MicroComponent;
using Be.Stateless.BizTalk.Schema.Annotation;
using Be.Stateless.BizTalk.Stream;
using Be.Stateless.Xml;
using FluentAssertions;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using Xunit;

namespace Be.Stateless.BizTalk.Component
{
	public class MicroPipelineComponentEnumerableConverterFixture
	{
		[Fact]
		public void CanConvertFrom()
		{
			var sut = new MicroPipelineComponentEnumerableConverter();
			sut.CanConvertFrom(typeof(string)).Should().BeTrue();
		}

		[Fact]
		public void CanConvertTo()
		{
			var sut = new MicroPipelineComponentEnumerableConverter();
			sut.CanConvertTo(typeof(string)).Should().BeTrue();
		}

		[Fact]
		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		public void ConvertFrom()
		{
			var xml = $@"<mComponents>
  <mComponent name='{typeof(MicroPipelineComponentDummyOne).AssemblyQualifiedName}'>
    <Property-One>1</Property-One>
    <Property-Two>2</Property-Two>
  </mComponent>
  <mComponent name='{typeof(MicroPipelineComponentDummyTwo).AssemblyQualifiedName}' >
    <Property-Six>6</Property-Six>
    <Property-Ten>9</Property-Ten>
  </mComponent>
  <mComponent name='{typeof(MicroPipelineComponentDummyTen).AssemblyQualifiedName}'>
    <Encoding>utf-8</Encoding>
    <Index>10</Index>
    <Requirements>Default</Requirements>
    <Name>DummyTen</Name>
    <Plugin>{typeof(DummyXmlTranslatorComponent).AssemblyQualifiedName}</Plugin>
  </mComponent>
</mComponents>";

			var sut = new MicroPipelineComponentEnumerableConverter();

			var result = sut.ConvertFrom(xml) as IEnumerable<IMicroComponent>;

			result.Should().NotBeNull().And.BeEquivalentTo(
				new MicroPipelineComponentDummyOne { One = "1", Two = "2" },
				new MicroPipelineComponentDummyTwo { Six = "6", Ten = "9" },
				new MicroPipelineComponentDummyTen {
					Encoding = new UTF8Encoding(false),
					Index = 10,
					Requirements = XmlTranslationRequirements.Default,
					Name = "DummyTen",
					Plugin = typeof(DummyXmlTranslatorComponent)
				});
		}

		[Fact]
		public void ConvertFromEmpty()
		{
			var sut = new MicroPipelineComponentEnumerableConverter();

			sut.ConvertFrom(string.Empty).Should().BeSameAs(Enumerable.Empty<IMicroComponent>());
		}

		[Fact]
		public void ConvertFromNull()
		{
			var sut = new MicroPipelineComponentEnumerableConverter();

			sut.ConvertFrom(null).Should().BeSameAs(Enumerable.Empty<IMicroComponent>());
		}

		[Fact]
		public void ConvertTo()
		{
			var list = new List<IMicroComponent> {
				new MicroPipelineComponentDummyOne(),
				new MicroPipelineComponentDummyTwo(),
				new MicroPipelineComponentDummyTen()
			};

			var sut = new MicroPipelineComponentEnumerableConverter();

			sut.ConvertTo(list, typeof(string)).Should().Be(
				"<mComponents>"
				+ $"<mComponent name='{typeof(MicroPipelineComponentDummyOne).AssemblyQualifiedName}'>"
				+ "<Property-One>one</Property-One>"
				+ "<Property-Two>two</Property-Two>"
				+ "</mComponent>"
				+ $"<mComponent name='{typeof(MicroPipelineComponentDummyTwo).AssemblyQualifiedName}'>"
				+ "<Property-Six>six</Property-Six>"
				+ "<Property-Ten>ten</Property-Ten>"
				+ "</mComponent>"
				+ $"<mComponent name='{typeof(MicroPipelineComponentDummyTen).AssemblyQualifiedName}'>"
				+ "<Encoding>utf-8 with signature</Encoding>"
				+ "<Index>10</Index>"
				+ "<Name>DummyTen</Name>"
				+ $"<Plugin>{typeof(DummyContextPropertyExtractorComponent).AssemblyQualifiedName}</Plugin>"
				+ "<Requirements>AbsorbXmlDeclaration TranslateAttributeNamespace</Requirements>"
				+ "</mComponent>"
				+ "</mComponents>");
		}

		[Fact]
		public void ConvertToNull()
		{
			var sut = new MicroPipelineComponentEnumerableConverter();

			sut.ConvertTo(Enumerable.Empty<IMicroComponent>(), typeof(string)).Should().BeNull();
		}

		[Fact]
		public void DeserializeComplexTypeWithCustomXmlSerialization()
		{
			var xml = $@"<mComponents>
  <mComponent name='{typeof(DummyContextPropertyExtractorComponent).AssemblyQualifiedName}'>
    <Enabled>true</Enabled>
    <Extractors>
      <s0:Properties xmlns:s0='urn:schemas.stateless.be:biztalk:annotations:2013:01' xmlns:s1='urn'>
        <s1:Property1 xpath='*/some-node' />
        <s1:Property2 promoted='true' xpath='*/other-node' />
      </s0:Properties>
    </Extractors>
  </mComponent>
  <mComponent name='{typeof(MicroPipelineComponentDummyOne).AssemblyQualifiedName}'>
    <Property-One>1</Property-One>
    <Property-Two>2</Property-Two>
  </mComponent>
</mComponents>";

			var sut = new MicroPipelineComponentEnumerableConverter();

			var deserialized = sut.ConvertFrom(xml) as IMicroComponent[];

			deserialized.Should().BeEquivalentTo(
				new DummyContextPropertyExtractorComponent {
					Enabled = true,
					Extractors = new[] {
						new XPathExtractor(new XmlQualifiedName("Property1", "urn"), "*/some-node"),
						new XPathExtractor(new XmlQualifiedName("Property2", "urn"), "*/other-node", ExtractionMode.Promote)
					}
				},
				new MicroPipelineComponentDummyOne { One = "1", Two = "2" }
			);
		}

		[Fact]
		public void DeserializeComplexTypeWithDefaultXmlSerialization()
		{
			var xml = $@"<mComponents>
  <mComponent name='{typeof(DummyXmlTranslatorComponent).AssemblyQualifiedName}'>
    <Enabled>true</Enabled>
    <xt:Translations override='false' xmlns:xt='urn:schemas.stateless.be:biztalk:translations:2013:07'>
      <xt:NamespaceTranslation matchingPattern='sourceUrn1' replacementPattern='urn:test1' />
      <xt:NamespaceTranslation matchingPattern='sourceUrn5' replacementPattern='urn:test5' />
    </xt:Translations>
  </mComponent>
</mComponents>";

			var sut = new MicroPipelineComponentEnumerableConverter();

			var deserialized = sut.ConvertFrom(xml) as IMicroComponent[];

			deserialized.Should().BeEquivalentTo(
				new DummyXmlTranslatorComponent {
					Enabled = true,
					Translations = new XmlTranslationSet {
						Override = false,
						Items = new[] {
							new XmlNamespaceTranslation("sourceUrn1", "urn:test1"),
							new XmlNamespaceTranslation("sourceUrn5", "urn:test5")
						}
					}
				});
		}

		[Fact]
		public void SerializeComplexTypeWithCustomXmlSerialization()
		{
			var component = new DummyContextPropertyExtractorComponent {
				Enabled = true,
				Extractors = new[] {
					new XPathExtractor(new XmlQualifiedName("Property1", "urn"), "*/some-node"),
					new XPathExtractor(new XmlQualifiedName("Property2", "urn"), "*/other-node", ExtractionMode.Promote)
				}
			};

			var sut = new MicroPipelineComponentEnumerableConverter();

			sut.ConvertTo(new IMicroComponent[] { component }, typeof(string)).Should().Be(
				"<mComponents>"
				+ $"<mComponent name='{typeof(DummyContextPropertyExtractorComponent).AssemblyQualifiedName}'>"
				+ "<Enabled>true</Enabled>"
				+ "<Extractors>"
				+ "<s0:Properties xmlns:s0='urn:schemas.stateless.be:biztalk:annotations:2013:01' xmlns:s1='urn'>"
				+ "<s1:Property1 xpath='*/some-node' />"
				+ "<s1:Property2 mode='promote' xpath='*/other-node' />"
				+ "</s0:Properties>"
				+ "</Extractors>"
				+ "</mComponent>"
				+ "</mComponents>");
		}

		[Fact]
		public void SerializeComplexTypeWithDefaultXmlSerialization()
		{
			var component = new DummyXmlTranslatorComponent {
				Enabled = true,
				Translations = new XmlTranslationSet {
					Override = false,
					Items = new[] {
						new XmlNamespaceTranslation("sourceUrn1", "urn:test1"),
						new XmlNamespaceTranslation("sourceUrn5", "urn:test5")
					}
				}
			};

			var sut = new MicroPipelineComponentEnumerableConverter();

			sut.ConvertTo(new IMicroComponent[] { component }, typeof(string)).Should().Be(
				"<mComponents>"
				+ $"<mComponent name='{typeof(DummyXmlTranslatorComponent).AssemblyQualifiedName}'>"
				+ "<Enabled>true</Enabled>"
				+ "<Translations override='false' xmlns='urn:schemas.stateless.be:biztalk:translations:2013:07'>"
				+ "<NamespaceTranslation matchingPattern='sourceUrn1' replacementPattern='urn:test1' />"
				+ "<NamespaceTranslation matchingPattern='sourceUrn5' replacementPattern='urn:test5' />"
				+ "</Translations>"
				+ "</mComponent>"
				+ "</mComponents>");
		}

		public class DummyContextPropertyExtractorComponent : IMicroComponent
		{
			#region IMicroComponent Members

			public IBaseMessage Execute(IPipelineContext pipelineContext, IBaseMessage message)
			{
				throw new NotSupportedException();
			}

			#endregion

			[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
			public bool Enabled { get; set; }

			[XmlIgnore]
			public PropertyExtractorCollection Extractors { get; set; }

			[Browsable(false)]
			[EditorBrowsable(EditorBrowsableState.Never)]
			[XmlElement("Extractors")]
			public PropertyExtractorCollectionSerializerSurrogate ExtractorSerializerSurrogate
			{
				get => new PropertyExtractorCollectionSerializerSurrogate(Extractors);
				set => Extractors = value;
			}
		}

		public class DummyXmlTranslatorComponent : IMicroComponent
		{
			#region IMicroComponent Members

			public IBaseMessage Execute(IPipelineContext pipelineContext, IBaseMessage message)
			{
				throw new NotSupportedException();
			}

			#endregion

			[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
			public bool Enabled { get; set; }

			[XmlElement("Translations", Namespace = XmlTranslationSet.NAMESPACE)]
			public XmlTranslationSet Translations { get; set; }
		}

		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Required by XML serialization")]
		[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Required by XML serialization")]
		public class MicroPipelineComponentDummyOne : IMicroComponent, IXmlSerializable
		{
			#region IMicroComponent Members

			public IBaseMessage Execute(IPipelineContext pipelineContext, IBaseMessage message)
			{
				throw new NotSupportedException();
			}

			#endregion

			#region IXmlSerializable Members

			public XmlSchema GetSchema()
			{
				return null;
			}

			public void ReadXml(XmlReader reader)
			{
				reader.ReadStartElement("Property-One");
				One = reader.ReadContentAsString();
				reader.ReadEndElement();
				reader.ReadStartElement("Property-Two");
				Two = reader.ReadContentAsString();
				reader.ReadEndElement();
			}

			public void WriteXml(XmlWriter writer)
			{
				writer.WriteElementString("Property-One", "one");
				writer.WriteElementString("Property-Two", "two");
			}

			#endregion

			public string One { get; set; }

			public string Two { get; set; }
		}

		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Required by XML serialization")]
		[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Required by XML serialization")]
		public class MicroPipelineComponentDummyTwo : IMicroComponent, IXmlSerializable
		{
			#region IMicroComponent Members

			public IBaseMessage Execute(IPipelineContext pipelineContext, IBaseMessage message)
			{
				throw new NotSupportedException();
			}

			#endregion

			#region IXmlSerializable Members

			public XmlSchema GetSchema()
			{
				return null;
			}

			public void ReadXml(XmlReader reader)
			{
				Six = reader.ReadElementContentAsString("Property-Six", string.Empty);
				Ten = reader.ReadElementContentAsString("Property-Ten", string.Empty);
			}

			public void WriteXml(XmlWriter writer)
			{
				writer.WriteElementString("Property-Six", "six");
				writer.WriteElementString("Property-Ten", "ten");
			}

			#endregion

			public string Six { get; set; }

			public string Ten { get; set; }
		}

		[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global", Justification = "Required by XML serialization")]
		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Required by XML serialization")]
		[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Required by XML serialization")]
		public class MicroPipelineComponentDummyTen : IMicroComponent
		{
			public MicroPipelineComponentDummyTen()
			{
				Encoding = new UTF8Encoding(true);
				Index = 10;
				Requirements = XmlTranslationRequirements.AbsorbXmlDeclaration | XmlTranslationRequirements.TranslateAttributeNamespace;
				Name = "DummyTen";
				Plugin = typeof(DummyContextPropertyExtractorComponent);
			}

			#region IMicroComponent Members

			public IBaseMessage Execute(IPipelineContext pipelineContext, IBaseMessage message)
			{
				throw new NotSupportedException();
			}

			#endregion

			[XmlElement(typeof(EncodingXmlSerializer))]
			public Encoding Encoding { get; set; }

			public int Index { get; set; }

			public string Name { get; set; }

			[XmlElement(typeof(RuntimeTypeXmlSerializer))]
			public Type Plugin { get; set; }

			public XmlTranslationRequirements Requirements { get; set; }
		}
	}
}
