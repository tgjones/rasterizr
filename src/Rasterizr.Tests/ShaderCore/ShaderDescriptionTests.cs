using System;
using NUnit.Framework;
using Nexus.Graphics.Colors;
using Rasterizr.ShaderCore;

namespace Rasterizr.Tests.ShaderCore
{
	[TestFixture]
	public class ShaderDescriptionTests
	{
		private struct TestPixelShaderInput
		{
			[Semantic(Semantics.TexCoord)]
			public ColorF TexCoord;

			[Semantic(Semantics.Color)]
			public ColorF Color;
		}

		private struct TestPixelShaderOutput
		{
			[Semantic(Semantics.Color)]
			public ColorF Color;
		}

		private class TestShader : IShader
		{
			public Type InputType
			{
				get { return typeof(TestPixelShaderInput); }
			}

			public Type OutputType
			{
				get { return typeof(TestPixelShaderOutput); }
			}

			public object BuildShaderInput()
			{
				throw new NotImplementedException();
			}

			public object Execute(object shaderInput)
			{
				throw new NotImplementedException();
			}
		}

		[Test]
		public void CanGetInputAndOutputParametersFromShader()
		{
			// Arrange.

			// Act.
			var shaderDescription = new ShaderDescription(new TestShader());

			// Assert.
			Assert.That(shaderDescription.InputParameters, Has.Length.EqualTo(2));
			Assert.That(shaderDescription.OutputParameters, Has.Length.EqualTo(1));
		}
	}
}