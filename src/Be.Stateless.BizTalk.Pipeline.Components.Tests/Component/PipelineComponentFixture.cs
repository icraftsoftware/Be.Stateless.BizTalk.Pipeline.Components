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
using System.IO;
using System.Text;
using Be.Stateless.BizTalk.Unit.Component;
using FluentAssertions;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using Moq;
using Xunit;
using static Be.Stateless.DelegateFactory;

namespace Be.Stateless.BizTalk.Component
{
	public class PipelineComponentFixture : PipelineComponentFixture<PipelineComponentFixture.PassThruPipelineComponent>
	{
		[Fact]
		public void DelegatesToExecuteCore()
		{
			using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("<root xmlns='urn:ns'></root>")))
			{
				MessageMock.Object.BodyPart.Data = inputStream;

				var sut = new Mock<PassThruPipelineComponent> { CallBase = true };

				sut.Object.Execute(PipelineContextMock.Object, MessageMock.Object);

				sut.Verify(pc => pc.ExecuteCore(PipelineContextMock.Object, MessageMock.Object), Times.Once());
			}
		}

		[Fact]
		public void RethrowAllExceptions()
		{
			var sut = new Mock<PipelineComponent> { CallBase = true };

			sut.Setup(pc => pc.ExecuteCore(PipelineContextMock.Object, MessageMock.Object))
				.Throws<InvalidOperationException>();

			Action(() => sut.Object.Execute(PipelineContextMock.Object, MessageMock.Object)).Should().Throw<InvalidOperationException>();
		}

		public class PassThruPipelineComponent : PipelineComponent
		{
			#region Base Class Member Overrides

			[Browsable(false)]
			public override string Description => string.Empty;

			protected internal override IBaseMessage ExecuteCore(IPipelineContext pipelineContext, IBaseMessage message)
			{
				return message;
			}

			public override void GetClassID(out Guid classId)
			{
				classId = Guid.NewGuid();
			}

			protected override void Load(IPropertyBag propertyBag) { }

			protected override void Save(IPropertyBag propertyBag) { }

			#endregion
		}
	}
}
