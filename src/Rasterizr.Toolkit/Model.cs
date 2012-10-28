using System.Collections.Generic;

namespace Rasterizr.Toolkit
{
	public class Model
	{
		public IList<ModelMesh> Meshes { get; private set; }

		public Model(IList<ModelMesh> meshes)
		{
			Meshes = meshes;
		}

		public void Draw()
		{
			foreach (ModelMesh mesh in Meshes)
				mesh.Draw();
		}
	}
}