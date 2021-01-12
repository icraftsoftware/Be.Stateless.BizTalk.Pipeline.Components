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
using AutoFixture;
using Be.Stateless.BizTalk.MicroComponent;
using Be.Stateless.BizTalk.Unit.Component;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace Be.Stateless.BizTalk.Component
{
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "XUnit fixture.")]
	public class MicroPipelineComponentFixture : PipelineComponentFixture<MicroPipelineComponent>
	{
		#region Nested Type: DummyMicroComponent

		[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
		public class DummyMicroComponent : IMicroComponent, IEquatable<DummyMicroComponent>
		{
			#region IEquatable<DummyMicroComponent> Members

			[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
			public bool Equals(DummyMicroComponent other)
			{
				return GetType() == other.GetType();
			}

			#endregion

			#region IMicroComponent Members

			public IBaseMessage Execute(IPipelineContext pipelineContext, IBaseMessage message)
			{
				throw new NotSupportedException();
			}

			#endregion

			#region Base Class Member Overrides

			public override bool Equals(object obj)
			{
				if (obj is null) return false;
				return ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((DummyMicroComponent) obj);
			}

			public override int GetHashCode()
			{
				return GetType().GetHashCode();
			}

			#endregion
		}

		#endregion

		static MicroPipelineComponentFixture()
		{
			// PipelineComponentFixture<MicroPipelineComponent> assumes and needs the following converter
			TypeDescriptor.AddAttributes(typeof(IEnumerable<IMicroComponent>), new TypeConverterAttribute(typeof(MicroComponentEnumerableConverter)));
		}

		#region Base Class Member Overrides

		protected override Fixture CreateAutoFixture()
		{
			var fixture = new Fixture();
			fixture.Register<IMicroComponent>(() => fixture.Create<DummyMicroComponent>());
			return fixture;
		}

		#endregion
	}
}
