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

using System.Xml;
using System.Xml.Serialization;
using Be.Stateless.BizTalk.MicroComponent;

namespace Be.Stateless.BizTalk.Component.Extensions
{
	public static class MicroPipelineComponentExtensions
	{
		public static void Serialize(this IMicroComponent component, XmlWriter writer)
		{
			var overrides = new XmlAttributeOverrides();
			overrides.Add(component.GetType(), new XmlAttributes { XmlRoot = new XmlRootAttribute(Constants.MICRO_COMPONENT_ELEMENT_NAME) });
			var serializer = Be.Stateless.Xml.Serialization.XmlSerializerFactory.Create(component.GetType(), overrides);

			var microPipelineComponentDedicatedXmlWriter = new MicroPipelineComponentXmlWriter(writer, component);

			// http://stackoverflow.com/questions/625927/omitting-all-xsi-and-xsd-namespaces-when-serializing-an-object-in-net
			var ns = new XmlSerializerNamespaces();
			ns.Add(string.Empty, string.Empty);
			serializer.Serialize(microPipelineComponentDedicatedXmlWriter, component, ns);
		}
	}
}
