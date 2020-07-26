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

using Be.Stateless.BizTalk.Component;
using NUnit.Framework;

namespace Be.Stateless.BizTalk.Unit.Component
{
	/// <summary>
	/// Base class that all <see cref="PipelineComponent"/> fixtures written against <c>NUnit</c> should inherit from.
	/// </summary>
	/// <typeparam name="T">
	/// The <see cref="PipelineComponent"/> derived class to test.
	/// </typeparam>
	/// <remarks>
	/// This is essentially a <c>NUnit</c> tailored version of the <see cref="PipelineComponentFixtureBase{T}"/> base class.
	/// </remarks>
	/// <seealso cref="PipelineComponentFixtureBase{T}"/>
	public abstract class PipelineComponentFixture<T> : PipelineComponentFixtureBase<T>
		where T : PipelineComponent, new()
	{
		#region Setup/Teardown

		[SetUp]
		public void PipelineComponentFixtureOfTSetUp()
		{
			Initialize();
		}

		#endregion

		[Test]
		public void AllPropertiesAreLoadedFromPropertyBag()
		{
			VerifyAllPropertiesAreLoadedFromPropertyBag();
		}

		[Test]
		public void AllPropertiesAreSavedToPropertyBag()
		{
			VerifyAllPropertiesAreSavedToPropertyBag();
		}

		[Test]
		public void AllPropertiesSupportRoundTripStringConversion()
		{
			VerifyAllPropertiesSupportRoundTripStringConversion();
		}

		[Test]
		public void ExecuteCoreIsSkippedWhenPipelineComponentIsNotEnabled()
		{
			VerifyExecuteCoreIsSkippedWhenPipelineComponentIsNotEnabled();
		}

		[Test]
		public void ExecuteReturnsImmediatelyWhenMessageIsNull()
		{
			VerifyExecuteReturnsImmediatelyWhenMessageIsNull();
		}

		[Test]
		public void ExecuteThrowsWhenPipelineContextIsNull()
		{
			VerifyExecuteThrowsWhenPipelineContextIsNull();
		}
	}
}
