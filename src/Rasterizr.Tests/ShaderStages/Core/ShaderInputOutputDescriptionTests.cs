using System.Linq;
using NUnit.Framework;
using Nexus;
using Rasterizr.ShaderStages.Core;

namespace Rasterizr.Tests.ShaderStages.Core
{
	[TestFixture]
	public class ShaderInputOutputDescriptionTests
	{
		private struct TestPixelShaderInput
		{
			[Semantic(Semantics.Color)]
			public ColorF Color;
		}

		[Test]
		public void CanInstantiate()
		{
			Assert.DoesNotThrow(() => new ShaderInputOutputDescription(typeof(TestPixelShaderInput)));
		}

		[Test]
		public void CanGetProperties()
		{
			// Arrange.
			var description = new ShaderInputOutputDescription(typeof(TestPixelShaderInput));

			// Act.
			var properties = description.Properties.ToList();

			// Assert.
			Assert.That(properties, Has.Count.EqualTo(1));
			Assert.That(properties[0].Semantic.Index, Is.EqualTo(0));
			Assert.That(properties[0].Semantic.Name, Is.EqualTo(Semantics.Color));
		}

		[Test]
		public void CanGetValue()
		{
			// Arrange.
			var description = new ShaderInputOutputDescription(typeof(TestPixelShaderInput));
			var testPixelShaderInput = new TestPixelShaderInput { Color = ColorsF.Red };
			var semantic = new Semantic(Semantics.Color, 0);

			// Act.
			var value = description.GetValue(testPixelShaderInput, semantic);

			// Assert.
			Assert.That(value, Is.EqualTo(ColorsF.Red));
		}

		[Test]
		public void CanSetValue()
		{
			// Arrange.
			var description = new ShaderInputOutputDescription(typeof(TestPixelShaderInput));
			var testPixelShaderInput = (object) new TestPixelShaderInput { Color = ColorsF.Red };
			var semantic = new Semantic(Semantics.Color, 0);

			// Act.
			description.SetValue(ref testPixelShaderInput, semantic, ColorsF.Green);

			// Assert.
			Assert.That(testPixelShaderInput, Is.InstanceOf<TestPixelShaderInput>());
			Assert.That(((TestPixelShaderInput) testPixelShaderInput).Color, Is.EqualTo(ColorsF.Green));
		}
	}
}