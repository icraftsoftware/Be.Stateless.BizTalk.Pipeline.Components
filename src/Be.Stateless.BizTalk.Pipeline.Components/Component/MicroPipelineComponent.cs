#region Copyright & License

// Copyright © 2012 - 2021 François Chabot
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
using System.Runtime.InteropServices;
using Be.Stateless.BizTalk.Component.Interop;
using Be.Stateless.BizTalk.MicroComponent;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace Be.Stateless.BizTalk.Component
{
	/// <summary>
	/// Runs a sequence of micro components, i.e. components implementing <see cref="IMicroComponent"/>, similarly to what a
	/// regular Microsoft BizTalk Server pipeline would do if the micro components were regular pipeline components.
	/// </summary>
	[ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
	[ComponentCategory(CategoryTypes.CATID_Any)]
	[Guid(CLASS_ID)]
	public class MicroPipelineComponent : PipelineComponent
	{
		public MicroPipelineComponent()
		{
			_microComponent = new MicroPipeline();
		}

		#region Base Class Member Overrides

		/// <summary>
		/// Description of the pipeline component.
		/// </summary>
		[Browsable(false)]
		[Description("Description of the pipeline component.")]
		public override string Description => "Runs a sequence of micro components as if they were regular pipeline components.";

		protected internal override IBaseMessage ExecuteCore(IPipelineContext pipelineContext, IBaseMessage message)
		{
			return _microComponent.Execute(pipelineContext, message);
		}

		/// <summary>
		/// Gets class ID of component for usage from unmanaged code.
		/// </summary>
		/// <param name="classId">
		/// Class ID of the component
		/// </param>
		[SuppressMessage("Design", "CA1021:Avoid out parameters")]
		public override void GetClassID(out Guid classId)
		{
			classId = _classID;
		}

		/// <summary>
		/// Loads configuration properties for the component from the <paramref name="propertyBag"/>.
		/// </summary>
		/// <param name="propertyBag">Configuration property bag</param>
		protected override void Load(IPropertyBag propertyBag)
		{
			propertyBag.ReadProperty(nameof(Components), value => _microComponent.LoadConfiguration(value));
		}

		/// <summary>
		/// Saves the current component configuration to the <paramref name="propertyBag"/>.
		/// </summary>
		/// <param name="propertyBag">Configuration property bag</param>
		protected override void Save(IPropertyBag propertyBag)
		{
			propertyBag.WriteProperty(nameof(Components), _microComponent.SaveConfiguration());
		}

		#endregion

		/// <summary>
		/// List of micro components that will be run in sequence by the micro pipeline.
		/// </summary>
		[Browsable(true)]
		[Description("List of micro components that will be run in sequence by the micro pipeline.")]
		[TypeConverter(typeof(MicroComponentEnumerableConverter))]
		public IEnumerable<IMicroComponent> Components
		{
			get => _microComponent.Components;
			set => _microComponent.Components = value;
		}

		private const string CLASS_ID = "02dd03e8-9509-4799-a196-a8c68e02d933";
		private static readonly Guid _classID = new(CLASS_ID);
		private readonly MicroPipeline _microComponent;
	}
}
