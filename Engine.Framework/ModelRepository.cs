using Engine.Contracts.Models;
using Graphics.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using World.Model;
using System;

namespace Engine.Framework
{
    public sealed class ModelRepository : IModelRepository
    {
        private string _folder;
        private ITextureLoader _textureLoader;
        private IBufferObjectFactory _bufferObjectFactory;

        public ModelRepository(string folder, 
            ITextureLoader textureLoader,
            IBufferObjectFactory bufferObjectFactory)
        {
            _folder = folder;
            _textureLoader = textureLoader;
            _bufferObjectFactory = bufferObjectFactory;
        }

        Model IModelRepository.Load(ModelInstanceDescription modelInstance)
        {
            Model model = new Model { RenderUnits = new List<ModelRenderUnit>(), FileName = modelInstance.Filename, CollisionModel = new ComplexShape() };

            EditorModel editorModel = JsonConvert.DeserializeObject<EditorModel>(File.ReadAllText($"{_folder}\\{modelInstance.Filename}"));
            List<Face> faces = new List<Face>();

            foreach(Submodel submodel in editorModel.Submodels)
            {
                ModelRenderUnit unit = new ModelRenderUnit();
                unit.Texture = _textureLoader.LoadTexture(submodel.Texture);
                unit.VertexBufferUnit = ConvertSubmodelToBufferUnit(submodel);

                model.RenderUnits.Add(unit);

                AddFaces(submodel.Polygons, faces);
            }

            model.Position = modelInstance.Position;
            model.CollisionModel.Faces = faces.ToArray();

            return model;
        }

        void IModelRepository.Delete(Model model)
        {
            foreach (ModelRenderUnit submodel in model.RenderUnits)
            {
                _textureLoader.DeleteTexture(submodel.Texture);
                _bufferObjectFactory.Delete(submodel.VertexBufferUnit.NormalBufferId.Value);
                _bufferObjectFactory.Delete(submodel.VertexBufferUnit.VertexBufferId);
                _bufferObjectFactory.Delete(submodel.VertexBufferUnit.TextureBufferId.Value);
            }
        }

        private void AddFaces(List<Polygon> polygons, List<Face> faces)
        {
            Normal lastNormal = null;

            Face lastFace = null;

            List<Triangle> triangles = new List<Triangle>();

            foreach (Polygon polygon in polygons)
            {
                Triangle triangle = BuildTriangle(polygon.Vertices);
                if (lastNormal != null && lastNormal.X == polygon.Normal.X && lastNormal.Y == polygon.Normal.Y && lastNormal.Z == polygon.Normal.Z)
                {
                    triangles.Add(triangle);
                }
                else
                {
                    if (lastFace != null)
                    {
                        lastFace.Triangles = triangles.ToArray();
                        faces.Add(lastFace);
                    }
                    triangles.Clear();
                    triangles.Add(triangle);
                    lastNormal = polygon.Normal;
                    lastFace = new Face { Normal = new double[] { lastNormal.X, lastNormal.Y, lastNormal.Z } };
                }
            }

            lastFace.Triangles = triangles.ToArray();
            faces.Add(lastFace);
        }

        private Triangle BuildTriangle(IEnumerable<Vertex> vertices)
        {
            Triangle triangle = new Triangle();
            int corner = 1;
            foreach(Vertex vertex in vertices)
            {
                if (corner == 1)
                    triangle.Corner1 = new double[] { vertex.Position.X, vertex.Position.Y, vertex.Position.Z };
                if (corner == 2)
                    triangle.Corner2 = new double[] { vertex.Position.X, vertex.Position.Y, vertex.Position.Z };
                if (corner == 3)
                    triangle.Corner3 = new double[] { vertex.Position.X, vertex.Position.Y, vertex.Position.Z };

                corner++;
            }
            return triangle;
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
