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

using Be.Stateless.BizTalk.ContextProperties;
using Be.Stateless.BizTalk.Message.Extensions;
using Be.Stateless.BizTalk.Unit.MicroComponent;
using Moq;
using Xunit;

namespace Be.Stateless.BizTalk.MicroComponent
{
	public class FailedMessageRoutingEnablerFixture : MicroComponentFixture<FailedMessageRoutingEnabler>
	{
		[Fact]
		public void FailedMessageRoutingCanBeDisabled()
		{
			var sut = new FailedMessageRoutingEnabler {
				EnableFailedMessageRouting = false,
				SuppressRoutingFailureReport = false
			};

			sut.Execute(PipelineContextMock.Object, MessageMock.Object);

			MessageMock.Verify(
				m => m.SetProperty(BtsProperties.RouteMessageOnFailure, true),
				Times.Never());
			MessageMock.Verify(
				m => m.SetProperty(BtsProperties.SuppressRoutingFailureDiagnosticInfo, true),
				Times.Never());
		}

		[Fact]
		public void FailedMessageRoutingIsEnabledByDefault()
		{
			var sut = new FailedMessageRoutingEnabler();

			sut.Execute(PipelineContextMock.Object, MessageMock.Object);

			MessageMock.Verify(
				m => m.SetProperty(BtsProperties.RouteMessageOnFailure, true),
				Times.Once());
			MessageMock.Verify(
				m => m.SetProperty(BtsProperties.SuppressRoutingFailureDiagnosticInfo, true),
				Times.Once());
		}
	}
}
