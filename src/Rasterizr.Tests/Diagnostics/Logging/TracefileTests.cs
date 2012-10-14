using System.Collections;
using System.IO;
using NUnit.Framework;
using Nexus;
using Nexus.Graphics.Colors;
using Rasterizr.Core.Diagnostics;
using Rasterizr.Diagnostics.Logging;

namespace Rasterizr.Tests.Diagnostics.Logging
{
	[TestFixture]
	public class TracefileTests
	{
		[Test]
		public void CanSerializeAndDeserializeTracefileContainingVertices()
		{
			// Arrange.
			var tracefile = new Tracefile
			{
				Frames =
				{
					new TracefileFrame
					{
						Number = 1,
						Events =
						{
							new TracefileEvent
							{
								Number = 1,
								OperationType = OperationType.InputAssemblerSetVertices,
								Arguments =
								{
									new []
									{
										new VertexPositionColor(new Point3D(1, 2, 3), ColorsF.Red),
										new VertexPositionColor(new Point3D(4, 5, 6), ColorsF.Green)
									}
								}
							}
						}
					}
				}
			};

			// Act.
			var writer = new StringWriter();
			tracefile.Save(writer);
			var reader = new StringReader(writer.ToString());
			var loadedTracefile = Tracefile.FromTextReader(reader);

			// Assert.
			Assert.That(loadedTracefile.Frames, Has.Count.EqualTo(1));
			Assert.That(loadedTracefile.Frames[0].Number, Is.EqualTo(1));
			Assert.That(loadedTracefile.Frames[0].Events, Has.Count.EqualTo(1));
			Assert.That(loadedTracefile.Frames[0].Events[0].Number, Is.EqualTo(1));
			Assert.That(loadedTracefile.Frames[0].Events[0].OperationType, Is.EqualTo(OperationType.InputAssemblerSetVertices));
			Assert.That(loadedTracefile.Frames[0].Events[0].Arguments, Has.Count.EqualTo(1));
			Assert.That(loadedTracefile.Frames[0].Events[0].Arguments[0], Is.InstanceOf<IList>());
			var loadedVertices = (IList) loadedTracefile.Frames[0].Events[0].Arguments[0];
			Assert.That(loadedVertices[0], Is.EqualTo(new VertexPositionColor(new Point3D(1, 2, 3), ColorsF.Red)));
			Assert.That(loadedVertices[1], Is.EqualTo(new VertexPositionColor(new Point3D(4, 5, 6), ColorsF.Green)));
		}
	}
}