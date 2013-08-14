using System.Linq;
using NUnit.Framework;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Resources;
using Rasterizr.Toolkit;
using SharpDX;
using SlimShader;
using SlimShader.Chunks.Common;
using SlimShader.Chunks.Xsgn;
using PrimitiveTopology = Rasterizr.Pipeline.InputAssembler.PrimitiveTopology;

namespace Rasterizr.Tests.Pipeline.InputAssembler
{
	[TestFixture]
	public class InputAssemblerStageTests
	{
		private static InputSignatureChunk GetTestVertexPositionNormalTextureShaderBytecode()
		{
			return new InputSignatureChunk
			{
				Parameters =
			    {
			        new SignatureParameterDescription("SV_Position", 0, Name.Position, RegisterComponentType.Float32,
			            0, ComponentMask.Xyz, ComponentMask.Xyz),
			        new SignatureParameterDescription("NORMAL", 0, Name.Undefined, RegisterComponentType.Float32,
			            1, ComponentMask.Xyz, ComponentMask.Xyz),
			        new SignatureParameterDescription("TEXCOORD", 0, Name.Undefined, RegisterComponentType.Float32,
			            2, ComponentMask.Xy, ComponentMask.Xy)
			    }
			};
		}

		private static InputSignatureChunk GetInstancedTestVertexPositionNormalShaderBytecode()
		{
			return new InputSignatureChunk
			{
				Parameters =
			    {
			        new SignatureParameterDescription("SV_Position", 0, Name.Position, RegisterComponentType.Float32,
			            0, ComponentMask.Xyz, ComponentMask.Xyz),
			        new SignatureParameterDescription("NORMAL", 0, Name.Undefined, RegisterComponentType.Float32,
			            1, ComponentMask.Xyz, ComponentMask.Xyz),
			        new SignatureParameterDescription("INSTANCEPOS", 0, Name.Undefined, RegisterComponentType.Float32,
			            2, ComponentMask.Xyz, ComponentMask.Xyz),
			        new SignatureParameterDescription("INSTANCESCALE", 0, Name.Undefined, RegisterComponentType.Float32,
			            3, ComponentMask.Xyz, ComponentMask.Xyz),
			        new SignatureParameterDescription("INSTANCECOLOR", 0, Name.Undefined, RegisterComponentType.Float32,
			            4, ComponentMask.Xyz, ComponentMask.Xyz)
			    }
			};
		}

		[Test]
		public void CanBindSingleVertexBuffer()
		{
			// Arrange.
			var device = new Device();
			var inputAssembler = new InputAssemblerStage(device);
			var vertices = new[]
			{
				new TestVertex.PositionNormalTexture(Vector3.Zero, Vector3.Zero, Vector2.Zero),
				new TestVertex.PositionNormalTexture(Vector3.Zero, Vector3.Zero, Vector2.Zero)
			};
			var vertexBuffer = device.CreateBuffer(new BufferDescription(BindFlags.VertexBuffer), vertices);

			// Act.
			inputAssembler.SetVertexBuffers(0, new[]
			{
				new VertexBufferBinding(vertexBuffer, 0, TestVertex.PositionNormalTexture.SizeInBytes)
			});

			// Assert.
			var vertexBufferBindings = new VertexBufferBinding[1];
			inputAssembler.GetVertexBuffers(0, 1, vertexBufferBindings);
			Assert.That(vertexBufferBindings[0].Buffer, Is.EqualTo(vertexBuffer));
		}

		[Test]
		public void CanBindMultipleVertexBuffers()
		{
			// Arrange.
			var device = new Device();
			var inputAssembler = new InputAssemblerStage(device);
			var vertices = new[]
			{
				new TestVertex.PositionNormalTexture(Vector3.Zero, Vector3.Zero, Vector2.Zero),
				new TestVertex.PositionNormalTexture(Vector3.Zero, Vector3.Zero, Vector2.Zero)
			};
			var vertexBuffer1 = device.CreateBuffer(new BufferDescription(BindFlags.VertexBuffer), vertices);
			var vertexBuffer2 = device.CreateBuffer(new BufferDescription(BindFlags.VertexBuffer), vertices);

			// Act.
			inputAssembler.SetVertexBuffers(0, new[]
			{
				new VertexBufferBinding(vertexBuffer1, 0, TestVertex.PositionNormalTexture.SizeInBytes),
				new VertexBufferBinding(vertexBuffer2, 0, TestVertex.PositionNormalTexture.SizeInBytes)
			});

			// Assert.
			var vertexBufferBindings = new VertexBufferBinding[2];
			inputAssembler.GetVertexBuffers(0, 2, vertexBufferBindings);
			Assert.That(vertexBufferBindings[0].Buffer, Is.EqualTo(vertexBuffer1));
			Assert.That(vertexBufferBindings[1].Buffer, Is.EqualTo(vertexBuffer2));
		}

		[Test]
		public void CanGetVertexStreamForLineList()
		{
			// Arrange.
			var device = new Device();
			var inputAssembler = new InputAssemblerStage(device);
			var inputSignature = GetTestVertexPositionNormalTextureShaderBytecode();
			inputAssembler.InputLayout = new InputLayout(device, TestVertex.PositionNormalTexture.InputElements, inputSignature);
			inputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
			var vertices = new[]
			{
				new TestVertex.PositionNormalTexture(new Vector3(1, 2, 3), new Vector3(3, 2, 1), new Vector2(3, 4)),
				new TestVertex.PositionNormalTexture(new Vector3(4, 5, 6), new Vector3(4, 6, 8), new Vector2(0.5f, 0.3f))
			};
			var vertexBuffer = device.CreateBuffer(new BufferDescription(BindFlags.VertexBuffer), vertices);
			inputAssembler.SetVertexBuffers(0, new[]
			{
				new VertexBufferBinding(vertexBuffer, 0, TestVertex.PositionNormalTexture.SizeInBytes)
			});

			// Act.
			var vertexStream = inputAssembler.GetVertexStream(inputSignature, 2, 0).ToList();

			// Assert.
			Assert.That(vertexStream, Has.Count.EqualTo(2));

			Assert.That(vertexStream[0].InstanceID, Is.EqualTo(0));
			Assert.That(vertexStream[0].VertexID, Is.EqualTo(0));
			Assert.That(vertexStream[0].Data, Is.EqualTo(new[]
				{
					new Number4(1.0f, 2.0f, 3.0f, 0.0f),
					new Number4(3.0f, 2.0f, 1.0f, 0.0f),
					new Number4(3.0f, 4.0f, 0.0f, 0.0f)
				}));

			Assert.That(vertexStream[1].InstanceID, Is.EqualTo(0));
			Assert.That(vertexStream[1].VertexID, Is.EqualTo(1));
			Assert.That(vertexStream[1].Data, Is.EqualTo(new[]
				{
					new Number4(4.0f, 5.0f, 6.0f, 0.0f),
					new Number4(4.0f, 6.0f, 8.0f, 0.0f),
					new Number4(0.5f, 0.3f, 0.0f, 0.0f)
				}));
		}

		[Test]
		public void CanGetVertexStreamForLineListWithMultipleVertexBuffers()
		{
			// Arrange.
			var device = new Device();
			var inputAssembler = new InputAssemblerStage(device);
			var inputSignature = GetTestVertexPositionNormalTextureShaderBytecode();
			var inputElements = TestVertex.PositionNormal.InputElements
				.Union(new[] { new InputElement("TEXCOORD", 0, Format.R32G32_Float, 1, 0) })
				.ToArray();
			inputAssembler.InputLayout = new InputLayout(device, inputElements, inputSignature);
			inputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
			var positionsAndNormals = new[]
			{
				new TestVertex.PositionNormal(new Vector3(1, 2, 3), new Vector3(3, 2, 1)),
				new TestVertex.PositionNormal(new Vector3(4, 5, 6), new Vector3(4, 6, 8))
			};
			var texCoords = new[]
			{
				new Vector2(3, 4),
				new Vector2(0.5f, 0.3f)
			};
			inputAssembler.SetVertexBuffers(0, new[]
			{
				new VertexBufferBinding(device.CreateBuffer(new BufferDescription(BindFlags.VertexBuffer), positionsAndNormals),
					0, TestVertex.PositionNormal.SizeInBytes),
				new VertexBufferBinding(device.CreateBuffer(new BufferDescription(BindFlags.VertexBuffer), texCoords),
					0, Vector2.SizeInBytes)
			});

			// Act.
			var vertexStream = inputAssembler.GetVertexStream(inputSignature, 2, 0).ToList();

			// Assert.
			Assert.That(vertexStream, Has.Count.EqualTo(2));

			Assert.That(vertexStream[0].InstanceID, Is.EqualTo(0));
			Assert.That(vertexStream[0].VertexID, Is.EqualTo(0));
			Assert.That(vertexStream[0].Data, Is.EqualTo(new[]
				{
					new Number4(1.0f, 2.0f, 3.0f, 0.0f),
					new Number4(3.0f, 2.0f, 1.0f, 0.0f),
					new Number4(3.0f, 4.0f, 0.0f, 0.0f)
				}));

			Assert.That(vertexStream[1].InstanceID, Is.EqualTo(0));
			Assert.That(vertexStream[1].VertexID, Is.EqualTo(1));
			Assert.That(vertexStream[1].Data, Is.EqualTo(new[]
				{
					new Number4(4.0f, 5.0f, 6.0f, 0.0f),
					new Number4(4.0f, 6.0f, 8.0f, 0.0f),
					new Number4(0.5f, 0.3f, 0.0f, 0.0f)
				}));
		}

		[Test]
		public void CanGetVertexStreamForIndexedLineList()
		{
			// Arrange.
			var device = new Device();
			var inputAssembler = new InputAssemblerStage(device);
			var inputSignature = GetTestVertexPositionNormalTextureShaderBytecode();
			inputAssembler.InputLayout = new InputLayout(device, TestVertex.PositionNormalTexture.InputElements, inputSignature);
			inputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
			var vertices = new[]
			{
				new TestVertex.PositionNormalTexture(),
				new TestVertex.PositionNormalTexture(),

				new TestVertex.PositionNormalTexture(new Vector3(1, 2, 3), new Vector3(3, 2, 1), new Vector2(3, 4)),
				new TestVertex.PositionNormalTexture(new Vector3(4, 5, 6), new Vector3(4, 6, 8), new Vector2(0.5f, 0.3f))
			};
			var vertexBuffer = device.CreateBuffer(new BufferDescription(BindFlags.VertexBuffer), vertices);
			inputAssembler.SetVertexBuffers(0, new[]
			{
				new VertexBufferBinding(vertexBuffer, 0, TestVertex.PositionNormalTexture.SizeInBytes)
			});
			var indexBuffer = device.CreateBuffer(new BufferDescription(BindFlags.IndexBuffer), new ushort[] { 2, 16, 1, 0 });
			inputAssembler.SetIndexBuffer(indexBuffer, Format.R16_UInt, 2);

			// Act.
			var vertexStream = inputAssembler.GetVertexStreamIndexed(inputSignature, 2, 1, 2).ToList();

			// Assert.
			Assert.That(vertexStream, Has.Count.EqualTo(2));

			Assert.That(vertexStream[0].InstanceID, Is.EqualTo(0));
			Assert.That(vertexStream[0].VertexID, Is.EqualTo(1));
			Assert.That(vertexStream[0].Data, Is.EqualTo(new[]
				{
					new Number4(4.0f, 5.0f, 6.0f, 0.0f),
					new Number4(4.0f, 6.0f, 8.0f, 0.0f),
					new Number4(0.5f, 0.3f, 0.0f, 0.0f)
				}));

			Assert.That(vertexStream[1].InstanceID, Is.EqualTo(0));
			Assert.That(vertexStream[1].VertexID, Is.EqualTo(2));
			Assert.That(vertexStream[1].Data, Is.EqualTo(new[]
				{
					new Number4(1.0f, 2.0f, 3.0f, 0.0f),
					new Number4(3.0f, 2.0f, 1.0f, 0.0f),
					new Number4(3.0f, 4.0f, 0.0f, 0.0f)
				}));
		}

		[Test]
		public void CanGetVertexStreamForInstancedLineList()
		{
			// Arrange.
			var device = new Device();
			var inputAssembler = new InputAssemblerStage(device);
			var inputSignature = GetInstancedTestVertexPositionNormalShaderBytecode();
			var inputElements = TestVertex.PositionNormal.InputElements
				.Union(new[]
				{
					new InputElement("INSTANCEPOS", 0, Format.R32G32B32_Float, 1, InputElement.AppendAligned,
						InputClassification.PerInstanceData, 1),
					new InputElement("INSTANCESCALE", 0, Format.R32G32B32_Float, 1, InputElement.AppendAligned,
						InputClassification.PerInstanceData, 1),
					new InputElement("INSTANCECOLOR", 0, Format.R32G32B32_Float, 2, InputElement.AppendAligned,
						InputClassification.PerInstanceData, 2)
				})
				.ToArray();
			inputAssembler.InputLayout = new InputLayout(device, inputElements, inputSignature);
			inputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
			var positionsAndNormals = new[]
			{
				new TestVertex.PositionNormal(new Vector3(1, 2, 3), new Vector3(3, 2, 1)),
				new TestVertex.PositionNormal(new Vector3(4, 5, 6), new Vector3(4, 6, 8))
			};
			var instancePositions = new[]
			{
				new Vector3(0, 0, 0),
				new Vector3(0, 0, 0),

				new Vector3(-2, 3, 16), // Position
				new Vector3(1, 1, 1), // Scale

				new Vector3(5, 3, 11),
				new Vector3(2, 2, 2),

				new Vector3(2, 5, 10),
				new Vector3(3, 3, 3),

				new Vector3(12, 15, 8),
				new Vector3(0.5f, 0.5f, 0.5f)
			};
			var instanceColors = new[]
			{
				new Color3F(0, 0, 0),

				new Color3F(1, 0, 0),
				new Color3F(0, 1, 0)
			};
			inputAssembler.SetVertexBuffers(0, new[]
			{
				new VertexBufferBinding(device.CreateBuffer(new BufferDescription(BindFlags.VertexBuffer), positionsAndNormals),
					0, TestVertex.PositionNormal.SizeInBytes),
				new VertexBufferBinding(device.CreateBuffer(new BufferDescription(BindFlags.VertexBuffer), instancePositions),
					0, Vector3.SizeInBytes * 2),
				new VertexBufferBinding(device.CreateBuffer(new BufferDescription(BindFlags.VertexBuffer), instanceColors),
					0, Color3F.SizeInBytes)
			});

			// Act.
			var vertexStream = inputAssembler.GetVertexStreamInstanced(inputSignature, 2, 4, 0, 1).ToList();

			// Assert.
			Assert.That(vertexStream, Has.Count.EqualTo(8));

			Assert.That(vertexStream[0].InstanceID, Is.EqualTo(0));
			Assert.That(vertexStream[0].VertexID, Is.EqualTo(0));
			Assert.That(vertexStream[0].Data, Is.EqualTo(new[]
				{
					new Number4(1.0f, 2.0f, 3.0f, 0.0f), // Position
					new Number4(3.0f, 2.0f, 1.0f, 0.0f), // Normal
					new Number4(-2.0f, 3.0f, 16.0f, 0.0f), // Instance Pos
					new Number4(1.0f, 1.0f, 1.0f, 0.0f), // Instance Scale
					new Number4(1.0f, 0.0f, 0.0f, 0.0f) // Instance Colour
				}));

			Assert.That(vertexStream[1].InstanceID, Is.EqualTo(0));
			Assert.That(vertexStream[1].VertexID, Is.EqualTo(1));
			Assert.That(vertexStream[1].Data, Is.EqualTo(new[]
				{
					new Number4(4.0f, 5.0f, 6.0f, 0.0f),
					new Number4(4.0f, 6.0f, 8.0f, 0.0f),
					new Number4(-2.0f, 3.0f, 16.0f, 0.0f),
					new Number4(1.0f, 1.0f, 1.0f, 0.0f),
					new Number4(1.0f, 0.0f, 0.0f, 0.0f)
				}));

			Assert.That(vertexStream[2].InstanceID, Is.EqualTo(1));
			Assert.That(vertexStream[2].VertexID, Is.EqualTo(0));
			Assert.That(vertexStream[2].Data, Is.EqualTo(new[]
				{
					new Number4(1.0f, 2.0f, 3.0f, 0.0f),
					new Number4(3.0f, 2.0f, 1.0f, 0.0f),
					new Number4(5.0f, 3.0f, 11.0f, 0.0f),
					new Number4(2.0f, 2.0f, 2.0f, 0.0f),
					new Number4(1.0f, 0.0f, 0.0f, 0.0f)
				}));

			Assert.That(vertexStream[3].InstanceID, Is.EqualTo(1));
			Assert.That(vertexStream[3].VertexID, Is.EqualTo(1));
			Assert.That(vertexStream[3].Data, Is.EqualTo(new[]
				{
					new Number4(4.0f, 5.0f, 6.0f, 0.0f),
					new Number4(4.0f, 6.0f, 8.0f, 0.0f),
					new Number4(5.0f, 3.0f, 11.0f, 0.0f),
					new Number4(2.0f, 2.0f, 2.0f, 0.0f),
					new Number4(1.0f, 0.0f, 0.0f, 0.0f)
				}));

			Assert.That(vertexStream[4].InstanceID, Is.EqualTo(2));
			Assert.That(vertexStream[4].VertexID, Is.EqualTo(0));
			Assert.That(vertexStream[4].Data, Is.EqualTo(new[]
				{
					new Number4(1.0f, 2.0f, 3.0f, 0.0f), // Position
					new Number4(3.0f, 2.0f, 1.0f, 0.0f), // Normal
					new Number4(2.0f, 5.0f, 10.0f, 0.0f), // Instance Pos
					new Number4(3.0f, 3.0f, 3.0f, 0.0f), // Instance Scale
					new Number4(0.0f, 1.0f, 0.0f, 0.0f) // Instance Colour
				}));

			Assert.That(vertexStream[5].InstanceID, Is.EqualTo(2));
			Assert.That(vertexStream[5].VertexID, Is.EqualTo(1));
			Assert.That(vertexStream[5].Data, Is.EqualTo(new[]
				{
					new Number4(4.0f, 5.0f, 6.0f, 0.0f),
					new Number4(4.0f, 6.0f, 8.0f, 0.0f),
					new Number4(2.0f, 5.0f, 10.0f, 0.0f),
					new Number4(3.0f, 3.0f, 3.0f, 0.0f),
					new Number4(0.0f, 1.0f, 0.0f, 0.0f)
				}));

			Assert.That(vertexStream[6].InstanceID, Is.EqualTo(3));
			Assert.That(vertexStream[6].VertexID, Is.EqualTo(0));
			Assert.That(vertexStream[6].Data, Is.EqualTo(new[]
				{
					new Number4(1.0f, 2.0f, 3.0f, 0.0f),
					new Number4(3.0f, 2.0f, 1.0f, 0.0f),
					new Number4(12.0f, 15.0f, 8.0f, 0.0f),
					new Number4(0.5f, 0.5f, 0.5f, 0.0f),
					new Number4(0.0f, 1.0f, 0.0f, 0.0f)
				}));

			Assert.That(vertexStream[7].InstanceID, Is.EqualTo(3));
			Assert.That(vertexStream[7].VertexID, Is.EqualTo(1));
			Assert.That(vertexStream[7].Data, Is.EqualTo(new[]
				{
					new Number4(4.0f, 5.0f, 6.0f, 0.0f),
					new Number4(4.0f, 6.0f, 8.0f, 0.0f),
					new Number4(12.0f, 15.0f, 8.0f, 0.0f),
					new Number4(0.5f, 0.5f, 0.5f, 0.0f),
					new Number4(0.0f, 1.0f, 0.0f, 0.0f)
				}));
		}

		[Test]
		public void CanGetVertexStreamForIndexedInstancedLineList()
		{
			// TODO
		}

		[Test]
		public void CanGetVertexStreamForTriangleListWithNoBuffersBound()
		{
			// Arrange.
			var device = new Device();
			var inputAssembler = new InputAssemblerStage(device);
			inputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
			var inputSignature = new InputSignatureChunk
			{
				Parameters =
			    {
			        new SignatureParameterDescription("SV_VertexID", 0, Name.VertexID, RegisterComponentType.UInt32,
			            0, ComponentMask.X, ComponentMask.X),
					new SignatureParameterDescription("SV_InstanceID", 0, Name.InstanceID, RegisterComponentType.UInt32,
			            1, ComponentMask.X, ComponentMask.X),
			    }
			};

			// Act.
			var vertexStream = inputAssembler.GetVertexStream(inputSignature, 3, 0).ToList();

			// Assert.
			Assert.That(vertexStream, Has.Count.EqualTo(3));

			Assert.That(vertexStream[0].InstanceID, Is.EqualTo(0));
			Assert.That(vertexStream[0].VertexID, Is.EqualTo(0));
			Assert.That(vertexStream[0].Data, Is.EqualTo(new[]
				{
					new Number4(0.0f, 0, 0, 0),
					new Number4(0.0f, 0, 0, 0)
				}));

			Assert.That(vertexStream[1].InstanceID, Is.EqualTo(0));
			Assert.That(vertexStream[1].VertexID, Is.EqualTo(1));
			Assert.That(vertexStream[1].Data, Is.EqualTo(new[]
				{
					new Number4(1.0f, 0, 0, 0),
					new Number4(0.0f, 0, 0, 0)
				}));

			Assert.That(vertexStream[2].InstanceID, Is.EqualTo(0));
			Assert.That(vertexStream[2].VertexID, Is.EqualTo(2));
			Assert.That(vertexStream[2].Data, Is.EqualTo(new[]
				{
					new Number4(2.0f, 0, 0, 0),
					new Number4(0.0f, 0, 0, 0)
				}));
		}
	}
}