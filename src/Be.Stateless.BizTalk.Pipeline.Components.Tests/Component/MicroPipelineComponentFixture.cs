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
using AutoFixture.Kernel;
using Be.Stateless.BizTalk.MicroComponent;
using Be.Stateless.BizTalk.Unit.Component;
using FluentAssertions;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.PipelineEditor;
using Moq;
using Xunit;

namespace Be.Stateless.BizTalk.Component
{
	public class MicroPipelineComponentFixture : PipelineComponentFixture<MicroPipelineComponent>
	{
		[Fact]
		public void ExecuteMicroComponents()
		{
			var pipelineContextMock = new Mock<IPipelineContext>();
			var messageMock1 = new Unit.Message.Mock<IBaseMessage>();
			var messageMock2 = new Unit.Message.Mock<IBaseMessage>();
			var messageMock3 = new Unit.Message.Mock<IBaseMessage>();

			var microComponentMockOne = new Mock<IMicroComponent>();
			microComponentMockOne
				.Setup(mc => mc.Execute(pipelineContextMock.Object, messageMock1.Object)).Returns(messageMock2.Object)
				.Verifiable();
			var microComponentMockTwo = new Mock<IMicroComponent>();
			microComponentMockTwo
				.Setup(mc => mc.Execute(pipelineContextMock.Object, messageMock2.Object)).Returns(messageMock3.Object)
				.Verifiable();

			var sut = new MicroPipelineComponent {
				Components = new[] {
					microComponentMockOne.Object,
					microComponentMockTwo.Object
				}
			};

			sut.Execute(pipelineContextMock.Object, messageMock1.Object).Should().BeSameAs(messageMock3.Object);

			microComponentMockOne.VerifyAll();
			microComponentMockTwo.VerifyAll();
		}

		[Fact]
		public void ExecuteNoMicroComponents()
		{
			var messageMock = new Unit.Message.Mock<IBaseMessage>();

			var sut = new MicroPipelineComponent();

			sut.Execute(new Mock<IPipelineContext>().Object, messageMock.Object).Should().BeSameAs(messageMock.Object);
		}

		[Fact]
		[SuppressMessage("ReSharper", "CoVariantArrayConversion")]
		public void LoadConfiguration()
		{
			var specimenContext = new SpecimenContext(CreateAutoFixture());
			var microPipelineComponents = new[] {
				specimenContext.Create<IMicroComponent>(),
				specimenContext.Create<IMicroComponent>(),
				specimenContext.Create<IMicroComponent>()
			};

			var propertyBag = new PropertyBag();
			object enabled = true;
			propertyBag.Write("Enabled", ref enabled);
			object components = MicroPipelineComponentEnumerableConverter.Serialize(microPipelineComponents);
			propertyBag.Write("Components", ref components);

			var sut = new MicroPipelineComponent();
			sut.Load(propertyBag, 0);

			sut.Components.Should().BeEquivalentTo(microPipelineComponents);
		}

		[Fact]
		public void SaveConfiguration()
		{
			var specimenContext = new SpecimenContext(CreateAutoFixture());
			var microPipelineComponents = new[] {
				specimenContext.Create<IMicroComponent>(),
				specimenContext.Create<IMicroComponent>(),
				specimenContext.Create<IMicroComponent>()
			};

			var propertyBag = new PropertyBag();

			var sut = new MicroPipelineComponent { Components = microPipelineComponents };
			sut.Save(propertyBag, true, true);

			propertyBag.Read("Components", out var components, 0);
			components.Should().Be(MicroPipelineComponentEnumerableConverter.Serialize(microPipelineComponents));
		}

		static MicroPipelineComponentFixture()
		{
			// PipelineComponentFixture<MicroPipelineComponent> assumes and needs the following converter
			TypeDescriptor.AddAttributes(typeof(IEnumerable<IMicroComponent>), new TypeConverterAttribute(typeof(MicroPipelineComponentEnumerableConverter)));
		}

		protected override Fixture CreateAutoFixture()
		{
			var fixture = new Fixture();
			fixture.Register<IMicroComponent>(() => fixture.Create<MicroComponentDummyOne>());
			return fixture;
		}

		[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
		public class MicroComponentDummyOne : IMicroComponent, IEquatable<MicroComponentDummyOne>
		{
			#region IEquatable<MicroComponentDummyOne> Members

			[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
			public bool Equals(MicroComponentDummyOne other)
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
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != GetType()) return false;
				return Equals((MicroComponentDummyOne) obj);
			}

			public override int GetHashCode()
			{
				return GetType().GetHashCode();
			}

			#endregion
		}
	}
}
