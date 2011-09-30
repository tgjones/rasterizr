namespace Rasterizr.Core.InputAssembler
{
	public class InputAssemblerOutput
	{
		public int PrimitiveID { get; private set; }
		public int VertexID { get; private set; }
		public object Vertex { get; private set; }

		public InputAssemblerOutput(int vertexID, int primitiveID, object vertex)
		{
			VertexID = vertexID;
			PrimitiveID = primitiveID;
			Vertex = vertex;
		}
	}
}