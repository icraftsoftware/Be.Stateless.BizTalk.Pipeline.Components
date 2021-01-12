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
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Be.Stateless.BizTalk.Component.Interop;
using log4net;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using IComponent = Microsoft.BizTalk.Component.Interop.IComponent;

namespace Be.Stateless.BizTalk.Component
{
	/// <summary>
	/// Base class for BizTalk Server pipeline components.
	/// </summary>
	public abstract class PipelineComponent : IBaseComponent, IComponent, IComponentUI, IPersistPropertyBag
	{
		protected PipelineComponent()
		{
			Enabled = true;
		}

		#region IBaseComponent Members

		/// <summary>
		/// Description of the pipeline component.
		/// </summary>
		[Browsable(false)]
		[Description("Description of the pipeline component.")]
		public abstract string Description { get; }

		/// <summary>
		/// Name of the pipeline component.
		/// </summary>
		[Browsable(false)]
		[Description("Name of the pipeline component.")]
		public virtual string Name => GetType().Name;

		/// <summary>
		/// Version of the pipeline component.
		/// </summary>
		[Browsable(false)]
		[Description("Version of the pipeline component.")]
		public virtual string Version => "2.0";

		#endregion

		#region IComponent Members

		/// <summary>
		/// Executes a pipeline component to process the input message and get the resulting message.
		/// </summary>
		/// <param name="pipelineContext">
		/// The <see cref="IPipelineContext" /> that contains the current pipeline context.
		/// </param>
		/// <param name="message">
		/// The <see cref="IBaseMessage" /> that contains the message to process.
		/// </param>
		/// <returns>
		/// The <see cref="IBaseMessage" /> that contains the resulting message.
		/// </returns>
		[SuppressMessage("Naming", "CA1725:Parameter names should match base declaration")]
		public IBaseMessage Execute(IPipelineContext pipelineContext, IBaseMessage message)
		{
			try
			{
				if (message == null) return null;
				if (pipelineContext == null) throw new ArgumentNullException(nameof(pipelineContext));
				if (Enabled)
				{
					if (_logger.IsDebugEnabled) _logger.Debug($"Pipeline component {GetType()} is being executed.");
					return ExecuteCore(pipelineContext, message);
				}
				if (_logger.IsDebugEnabled) _logger.Debug($"Pipeline component {GetType()} has been disabled.");
				return message;
			}
			catch (Exception exception)
			{
				if (_logger.IsErrorEnabled) _logger.Error($"Pipeline component {GetType()} failed while being executed.", exception);
				throw;
			}
		}

		#endregion

		#region IComponentUI Members

		/// <summary>
		/// Component icon to use in BizTalk Editor.
		/// </summary>
		[Browsable(false)]
		[SuppressMessage("ReSharper", "ResourceItemNotResolved")]
		public virtual IntPtr Icon => IntPtr.Zero;

		/// <summary>
		/// The Validate method is called by the BizTalk Editor during the build of a BizTalk project.
		/// </summary>
		/// <param name="projectSystem">
		/// An object containing the configuration properties.
		/// </param>
		/// <returns>
		/// The IEnumerator enables the caller to enumerate through a collection of strings containing error messages. These
		/// error messages appear as compiler error messages. To report successful property validation, the method should return
		/// an empty enumerator.
		/// </returns>
		public virtual IEnumerator Validate(object projectSystem)
		{
			return null;
		}

		#endregion

		#region IPersistPropertyBag Members

		[SuppressMessage("Design", "CA1021:Avoid out parameters")]
		[SuppressMessage("Naming", "CA1725:Parameter names should match base declaration")]
		public abstract void GetClassID(out Guid classId);

		/// <summary>
		/// Not implemented
		/// </summary>
		void IPersistPropertyBag.InitNew() { }

		/// <summary>
		/// Loads the pipeline component configuration. This base class implementation must be called by derived classes if they
		/// override it.
		/// </summary>
		/// <param name="propertyBag"></param>
		/// <param name="errorLog"></param>
		public void Load(IPropertyBag propertyBag, int errorLog)
		{
			propertyBag.ReadProperty(nameof(Enabled), value => Enabled = value);
			if (Enabled) Load(propertyBag);
		}

		/// <summary>
		/// Saves the pipeline component configuration. This base class implementation must be called by derived classes if they
		/// override it.
		/// </summary>
		/// <param name="propertyBag"></param>
		/// <param name="clearDirty"></param>
		/// <param name="saveAllProperties"></param>
		public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
		{
			propertyBag.WriteProperty(nameof(Enabled), Enabled);
			Save(propertyBag);
		}

		#endregion

		/// <summary>
		/// Enables or disables the pipeline component.
		/// </summary>
		/// <remarks>
		/// Whether to let this pipeline component execute or not.
		/// </remarks>
		[Browsable(true)]
		[Description("Enables or disables the pipeline component.")]
		[SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
		public bool Enabled { get; set; }

		/// <summary>
		/// Loads configuration properties for the component
		/// </summary>
		/// <param name="propertyBag">Configuration property bag</param>
		protected abstract void Load(IPropertyBag propertyBag);

		/// <summary>
		/// Saves the pipeline component configuration. This base class implementation must be called by derived classes if they
		/// override it.
		/// </summary>
		/// <param name="propertyBag"></param>
		protected abstract void Save(IPropertyBag propertyBag);

		/// <summary>
		/// Executes a pipeline component to process the input message and get the resulting message.
		/// </summary>
		/// <param name="pipelineContext">
		/// The <see cref="IPipelineContext" /> that contains the current pipeline context.
		/// </param>
		/// <param name="message">
		/// The <see cref="IBaseMessage" /> that contains the message to process.
		/// </param>
		/// <returns>
		/// The <see cref="IBaseMessage" /> that contains the resulting message.
		/// </returns>
		[SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
		protected internal abstract IBaseMessage ExecuteCore(IPipelineContext pipelineContext, IBaseMessage message);

		private static readonly ILog _logger = LogManager.GetLogger(typeof(PipelineComponent));
	}
}
