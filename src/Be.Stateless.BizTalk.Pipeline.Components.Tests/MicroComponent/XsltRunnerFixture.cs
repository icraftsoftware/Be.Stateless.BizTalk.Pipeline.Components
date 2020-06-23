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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Be.Stateless.BizTalk.ContextProperties;
using Be.Stateless.BizTalk.Message.Extensions;
using Be.Stateless.BizTalk.Schema;
using Be.Stateless.BizTalk.Streaming.Extensions;
using Be.Stateless.BizTalk.Unit;
using Be.Stateless.BizTalk.Unit.MicroComponent;
using Be.Stateless.BizTalk.Unit.Transform;
using Be.Stateless.Reflection;
using FluentAssertions;
using Microsoft.BizTalk.Edi.BaseArtifacts;
using Microsoft.BizTalk.Streaming;
using Microsoft.XLANGs.BaseTypes;
using Moq;
using Xunit;

namespace Be.Stateless.BizTalk.MicroComponent
{
	public class XsltRunnerFixture : MicroComponentFixture
	{
		[Fact]
		public void DoesNothingWhenNoXslt()
		{
			using (var streamProbingScope = new ProbeStreamMockingScope())
			using (var streamTransformingScope = new TransformStreamMockingScope())
			{
				// CAUTION! does not call CreateXsltRunner() as this test concerns only XsltRunner and none of its derived types
				var sut = new XsltRunner();
				sut.MapType.Should().BeNull();
				sut.Execute(PipelineContextMock.Object, MessageMock.Object);

				streamProbingScope.Mock.VerifyGet(ps => ps.MessageType, Times.Never());
				streamTransformingScope.Mock.Verify(ps => ps.Apply(It.IsAny<Type>()), Times.Never());
			}
		}

		[Fact]
		public void EncodingDefaultsToUtf8WithoutSignature()
		{
			CreateXsltRunner().Encoding.Should().Be(new UTF8Encoding(false));
		}

		[Fact]
		[SuppressMessage("ReSharper", "PossibleInvalidCastException")]
		public virtual void ReplacesMessageOriginalDataStreamWithTransformResult()
		{
			PipelineContextMock
				.Setup(m => m.GetDocumentSpecByType("http://schemas.microsoft.com/Edi/EdifactServiceSchema#UNB"))
				.Returns(SchemaMetadata.For<EdifactServiceSchema.UNB>().DocumentSpec);

			var sut = CreateXsltRunner();
			sut.Encoding = Encoding.UTF8;
			sut.MapType = typeof(IdentityTransform);

			using (var dataStream = new MemoryStream(Encoding.UTF8.GetBytes("<UNB xmlns='http://schemas.microsoft.com/Edi/EdifactServiceSchema'></UNB>")))
			using (var transformedStream = dataStream.Transform().Apply(sut.MapType))
			using (var streamTransformingScope = new TransformStreamMockingScope())
			{
				MessageMock.Object.BodyPart.Data = dataStream;

				streamTransformingScope.Mock
					.Setup(ts => ts.ExtendWith(MessageMock.Object.Context))
					.Returns(streamTransformingScope.Mock.Object);
				streamTransformingScope.Mock
					.Setup(ts => ts.Apply(sut.MapType, sut.Encoding))
					.Returns(transformedStream)
					.Verifiable();

				sut.Execute(PipelineContextMock.Object, MessageMock.Object);

				streamTransformingScope.Mock.VerifyAll();

				MessageMock.Object.BodyPart.Data.Should().BeOfType<MarkableForwardOnlyEventingReadStream>();
				Reflector.GetField((MarkableForwardOnlyEventingReadStream) MessageMock.Object.BodyPart.Data, "m_data").Should().BeSameAs(transformedStream);
			}
		}

		[Fact]
		public virtual void XsltEntailsMessageTypeIsPromoted()
		{
			PipelineContextMock
				.Setup(m => m.GetDocumentSpecByType("http://schemas.microsoft.com/Edi/EdifactServiceSchema#UNB"))
				.Returns(SchemaMetadata.For<EdifactServiceSchema.UNB>().DocumentSpec);

			var sut = CreateXsltRunner();
			sut.Encoding = Encoding.UTF8;
			sut.MapType = typeof(IdentityTransform);

			using (var dataStream = new MemoryStream(Encoding.UTF8.GetBytes("<UNB xmlns='http://schemas.microsoft.com/Edi/EdifactServiceSchema'></UNB>")))
			{
				MessageMock.Object.BodyPart.Data = dataStream;
				sut.Execute(PipelineContextMock.Object, MessageMock.Object);
			}

			MessageMock.Verify(m => m.Promote(BtsProperties.MessageType, SchemaMetadata.For<EdifactServiceSchema.UNB>().MessageType), Times.Once());
			MessageMock.Verify(m => m.Promote(BtsProperties.SchemaStrongName, SchemaMetadata.For<EdifactServiceSchema.UNB>().DocumentSpec.DocSpecStrongName), Times.Once());
		}

		[Fact]
		public virtual void XsltEntailsMessageTypeIsPromotedOnlyIfOutputMethodIsXml()
		{
			PipelineContextMock
				.Setup(m => m.GetDocumentSpecByType("http://schemas.microsoft.com/Edi/EdifactServiceSchema#UNB"))
				.Returns(SchemaMetadata.For<EdifactServiceSchema.UNB>().DocumentSpec);

			var sut = CreateXsltRunner();
			sut.Encoding = Encoding.UTF8;
			sut.MapType = typeof(AnyToText);

			using (var dataStream = new MemoryStream(Encoding.UTF8.GetBytes("<UNB xmlns='http://schemas.microsoft.com/Edi/EdifactServiceSchema'></UNB>")))
			{
				MessageMock.Object.BodyPart.Data = dataStream;
				sut.Execute(PipelineContextMock.Object, MessageMock.Object);
			}

			MessageMock.Verify(m => m.Promote(BtsProperties.MessageType, SchemaMetadata.For<EdifactServiceSchema.UNB>().MessageType), Times.Never());
			MessageMock.Verify(m => m.Promote(BtsProperties.SchemaStrongName, SchemaMetadata.For<EdifactServiceSchema.UNB>().DocumentSpec.DocSpecStrongName), Times.Never());
		}

		[Fact]
		public virtual void XsltFromContextHasPrecedenceOverConfiguredOne()
		{
			PipelineContextMock
				.Setup(m => m.GetDocumentSpecByType("http://schemas.microsoft.com/Edi/EdifactServiceSchema#UNB"))
				.Returns(SchemaMetadata.For<EdifactServiceSchema.UNB>().DocumentSpec);

			var sut = CreateXsltRunner();
			sut.Encoding = Encoding.UTF8;
			sut.MapType = typeof(TransformBase);

			var mapType = typeof(IdentityTransform);
			using (var dataStream = new MemoryStream(Encoding.UTF8.GetBytes("<UNB xmlns='http://schemas.microsoft.com/Edi/EdifactServiceSchema'></UNB>")))
			using (var transformedStream = dataStream.Transform().Apply(mapType))
			using (var streamTransformingScope = new TransformStreamMockingScope())
			{
				MessageMock.Object.BodyPart.Data = dataStream;
				MessageMock
					.Setup(m => m.GetProperty(BizTalkFactoryProperties.MapTypeName))
					.Returns(typeof(IdentityTransform).AssemblyQualifiedName);

				var transformStreamMock = streamTransformingScope.Mock;
				transformStreamMock
					.Setup(ts => ts.ExtendWith(MessageMock.Object.Context))
					.Returns(transformStreamMock.Object);
				transformStreamMock
					.Setup(ts => ts.Apply(mapType, sut.Encoding))
					.Returns(transformedStream)
					.Verifiable();

				sut.Execute(PipelineContextMock.Object, MessageMock.Object);

				transformStreamMock.Verify(ts => ts.Apply(sut.MapType, sut.Encoding), Times.Never());
				transformStreamMock.VerifyAll();
			}
		}

		protected virtual XsltRunner CreateXsltRunner()
		{
			return new XsltRunner();
		}
	}
}
