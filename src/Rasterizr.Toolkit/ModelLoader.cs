namespace Rasterizr.Toolkit
{
	public static class ModelLoader
	{
		//public static Model FromScene(Device device, Scene scene)
		//{
		//	// Sort scene meshes by transparency.
		//	var sortedSceneMeshes = scene.Meshes;
		//	sortedSceneMeshes.Sort((l, r) => r.Material.Transparency.CompareTo(l.Material.Transparency));

		//	List<ModelMesh> modelMeshes = new List<ModelMesh>();
		//	foreach (Mesh mesh in sortedSceneMeshes)
		//	{
		//		ModelMesh modelMesh = new ModelMesh(device);
		//		modelMesh.Name = mesh.Name;
		//		modelMeshes.Add(modelMesh);

		//		modelMesh.Indices = mesh.Indices;

		//		modelMesh.Vertices = new List<VertexPositionNormalTexture>();
		//		for (int i = 0; i < mesh.Positions.Count; ++i)
		//		{
		//			Point2D texCoord = (mesh.TextureCoordinates.Count > i)
		//								? mesh.TextureCoordinates[i].Xy
		//								: Point2D.Zero;
		//			modelMesh.Vertices.Add(new VertexPositionNormalTexture(
		//				mesh.Positions[i], mesh.Normals[i], texCoord));
		//		}

		//		var effect = new BasicEffect(device);

		//		//if (!string.IsNullOrEmpty(mesh.Material.DiffuseTextureName))
		//		//{
		//		//	effect.Texture = Texture2D.FromFile(mesh.Material.DiffuseTextureName);
		//		//	effect.TextureEnabled = true;
		//		//}

		//		effect.DiffuseColor = mesh.Material.DiffuseColor;
		//		effect.SpecularColor = mesh.Material.SpecularColor;
		//		effect.SpecularPower = mesh.Material.Shininess;
		//		effect.Alpha = mesh.Material.Transparency;

		//		modelMesh.Effect = effect;
		//	}

		//	Model model = new Model(modelMeshes);
		//	return model;
		//}
	}
}