using Engine.Contracts.Models;
using Graphics.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Engine.Framework
{
    public sealed class ModelLoader : IModelLoader
    {
        private string _folder;
        private ITextureLoader _textureLoader;
        private IBufferObjectFactory _bufferObjectFactory;

        public ModelLoader(string folder, 
            ITextureLoader textureLoader,
            IBufferObjectFactory bufferObjectFactory)
        {
            _folder = folder;
            _textureLoader = textureLoader;
            _bufferObjectFactory = bufferObjectFactory;
        }

        Model IModelLoader.Load(string fileName)
        {
            Model model = new Model { RenderUnits = new List<ModelRenderUnit>() };

            EditorModel editorModel = JsonConvert.DeserializeObject<EditorModel>(File.ReadAllText($"{_folder}\\{fileName}"));

            foreach(Submodel submodel in editorModel.Submodels)
            {
                ModelRenderUnit unit = new ModelRenderUnit();
                unit.Texture = _textureLoader.LoadTexture(submodel.Texture);
                unit.VertexBufferUnit = ConvertSubmodelToBufferUnit(submodel);

                model.RenderUnits.Add(unit);
            }

            model.CollisionModel = editorModel.CollisionModel;

            return model;
        }

        void IModelLoader.Delete(Model model)
        {
            foreach (ModelRenderUnit submodel in model.RenderUnits)
            {
                _textureLoader.DeleteTexture(submodel.Texture);
                _bufferObjectFactory.Delete(submodel.VertexBufferUnit.NormalBufferId.Value);
                _bufferObjectFactory.Delete(submodel.VertexBufferUnit.VertexBufferId);
                _bufferObjectFactory.Delete(submodel.VertexBufferUnit.TextureBufferId.Value);
            }
        }

        private VertexBufferUnit ConvertSubmodelToBufferUnit(Submodel submodel)
        {
            VertexBufferUnit vertexBufferUnit = new VertexBufferUnit();

            float[] vertices = new float[submodel.Polygons.Count * 9];
            float[] texcoords = new float[submodel.Polygons.Count * 6];
            float[] normals = new float[submodel.Polygons.Count * 9];

            int vertexIndex = 0, texcoordIndex = 0, normalIndex = 0;

            foreach (Polygon polygon in submodel.Polygons)
            {
                foreach(Vertex vertex in polygon.Vertices)
                {
                    vertices[vertexIndex++] = vertex.Position.X;
                    vertices[vertexIndex++] = vertex.Position.Y;
                    vertices[vertexIndex++] = vertex.Position.Z;

                    texcoords[texcoordIndex++] = vertex.TextureCoordinate.X;
                    texcoords[texcoordIndex++] = vertex.TextureCoordinate.Y;

                    normals[normalIndex++] = polygon.Normal.X;
                    normals[normalIndex++] = polygon.Normal.Y;
                    normals[normalIndex++] = polygon.Normal.Z;
                }
            }

            vertexBufferUnit.VertexBufferId = _bufferObjectFactory.GenerateVertexBuffer(vertices);
            vertexBufferUnit.TextureBufferId = _bufferObjectFactory.GenerateTextureCoordBuffer(texcoords);
            vertexBufferUnit.NormalBufferId = _bufferObjectFactory.GenerateNormalBuffer(normals);

            vertexBufferUnit.NumberOfTriangleCorners = vertices.Length / 3;

            return vertexBufferUnit;
        }
    }
}
