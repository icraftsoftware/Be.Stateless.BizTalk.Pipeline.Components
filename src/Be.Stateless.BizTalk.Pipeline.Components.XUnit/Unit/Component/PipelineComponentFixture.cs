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
using Xunit;

namespace Be.Stateless.BizTalk.Unit.Component
{
	/// <summary>
	/// Base class that all <see cref="PipelineComponent"/> fixtures written against <c>xUnit</c> should inherit from.
	/// </summary>
	/// <typeparam name="T">
	/// The <see cref="PipelineComponent"/> derived class to test.
	/// </typeparam>
	/// <remarks>
	/// This is essentially an <c>xUnit</c> tailored version of the <see cref="PipelineComponentFixtureBase{T}"/> base class.
	/// </remarks>
	/// <seealso cref="PipelineComponentFixtureBase{T}"/>
	public abstract class PipelineComponentFixture<T> : PipelineComponentFixtureBase<T>
		where T : PipelineComponent, new()
	{
		[Fact]
		public void AllPropertiesAreLoadedFromPropertyBag()
		{
			VerifyAllPropertiesAreLoadedFromPropertyBag();
		}

		[Fact]
		public void AllPropertiesAreSavedToPropertyBag()
		{
			VerifyAllPropertiesAreSavedToPropertyBag();
		}

		[Fact]
		public void AllPropertiesSupportRoundTripStringConversion()
		{
			VerifyAllPropertiesSupportRoundTripStringConversion();
		}

		[Fact]
		public void ExecuteCoreIsSkippedWhenPipelineComponentIsNotEnabled()
		{
			VerifyExecuteCoreIsSkippedWhenPipelineComponentIsNotEnabled();
		}

		[Fact]
		public void ExecuteReturnsImmediatelyWhenMessageIsNull()
		{
			VerifyExecuteReturnsImmediatelyWhenMessageIsNull();
		}

		[Fact]
		public void ExecuteThrowsWhenPipelineContextIsNull()
		{
			VerifyExecuteThrowsWhenPipelineContextIsNull();
		}

		[Fact(Skip = "TODO")]
		public void Todo()
		{
			// TODO create ref pipeline, save its data to property bag and reload them from another pipeline and assert that both pipelines are equivalent, this
			// will test that these actual values are indeed written and read back

			//var sut = fixture.Create<T>();

			// i.e. ensures all the values converted to string are actually the values written and read to a property bag, in effect, verifying that all values
			// are actually converted to a string when written to property bag and vice versa
		}

		protected PipelineComponentFixture()
		{
			Initialize();
		}
	}
}
