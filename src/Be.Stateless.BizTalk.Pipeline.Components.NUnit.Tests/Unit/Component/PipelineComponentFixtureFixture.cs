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
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Be.Stateless.BizTalk.Component;
using Be.Stateless.BizTalk.Component.Interop;
using Be.Stateless.BizTalk.MicroComponent;
using Be.Stateless.Extensions;
using Be.Stateless.Xml;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using NUnit.Framework;

namespace Be.Stateless.BizTalk.Unit.Component
{
	[TestFixture]
	public class PipelineComponentFixtureFixture : PipelineComponentFixture<PipelineComponentFixtureFixture.PipelineComponentDouble>
	{
		public class PipelineComponentDouble : PipelineComponent
		{
			#region Nested Type: MicroComponentDouble

			public class MicroComponentDouble : IMicroComponent
			{
				#region IMicroComponent Members

				public IBaseMessage Execute(IPipelineContext pipelineContext, IBaseMessage message)
				{
					return message;
				}

				#endregion

				public ExecutionModeEnum ExecutionMode { get; set; }

				[XmlElement(nameof(PluginType), typeof(RuntimeTypeXmlSerializer))]
				public Type PluginType { get; set; }
			}

			#endregion

			#region ExecutionModeEnum Enum

			[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "xUnit test class.")]
			public enum ExecutionModeEnum
			{
				Immediate = 0,
				Deferred = 1
			}

			#endregion

			public PipelineComponentDouble()
			{
				_microComponent = new MicroComponentDouble();
			}

			#region Base Class Member Overrides

			[Browsable(false)]
			public override string Description => "PipelineComponent test double.";

			protected override IBaseMessage ExecuteCore(IPipelineContext pipelineContext, IBaseMessage message)
			{
				return _microComponent.Execute(pipelineContext, message);
			}

			public override void GetClassID(out Guid classId)
			{
				classId = Guid.Empty;
			}

			protected override void Load(IPropertyBag propertyBag)
			{
				propertyBag.ReadProperty<ExecutionModeEnum>(nameof(ExecutionMode), value => ExecutionMode = value);
				propertyBag.ReadProperty(nameof(PluginType), value => PluginType = Type.GetType(value, true));
			}

			protected override void Save(IPropertyBag propertyBag)
			{
				propertyBag.WriteProperty(nameof(ExecutionMode), ExecutionMode);
				propertyBag.WriteProperty(nameof(PluginType), PluginType.IfNotNull(m => m.AssemblyQualifiedName));
			}

			#endregion

			[Browsable(true)]
			public ExecutionModeEnum ExecutionMode
			{
				get => _microComponent.ExecutionMode;
				set => _microComponent.ExecutionMode = value;
			}

			[Browsable(true)]
			[TypeConverter(typeof(TypeNameConverter))]
			public Type PluginType
			{
				get => _microComponent.PluginType;
				set => _microComponent.PluginType = value;
			}

			private readonly MicroComponentDouble _microComponent;
		}

		static PipelineComponentFixtureFixture()
		{
			// PipelineComponentFixture<PluginFactoryComponent> assumes and needs the following converters
			TypeDescriptor.AddAttributes(
				typeof(Type),
				new TypeConverterAttribute(typeof(TypeNameConverter)));
		}
	}
}
