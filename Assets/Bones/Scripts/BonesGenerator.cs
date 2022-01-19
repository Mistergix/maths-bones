using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PGSauce.Games.BoneGenerator
{
    public class BonesGenerator : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;

        private Vector3[] _vertices;
        private Vector3 _barycenter;
        private Matrix3x3 _covarianceMat;
        private Vector3 _eigenVector;
        private List<Vector3> _projectedPoints;
        private Vector3 _maxPoint;
        private Vector3 _minPoint;

        private const int EigenVectorIterations = 100;

        public void GenerateBone(Mesh mesh, Material material)
        {
            _projectedPoints = new List<Vector3>();
            
            meshFilter.mesh = mesh;
            meshRenderer.material = material;

            _vertices = mesh.vertices;
            
            ComputeBarycenter();
            OffsetVertices();
            ComputeCovarianceMatrix();
            ComputeEigenVector();
            ProjectPointsOntoEigenVector();
            OffsetBackVertices();
        }

        private void OffsetBackVertices()
        {
            for (var i = 0; i < _vertices.Length; i++)
            {
                _vertices[i] += _barycenter;
            }

            _minPoint += _barycenter;
            _maxPoint += _barycenter;
        }

        private void ProjectPointsOntoEigenVector()
        {
            var max = Vector3.zero;
            var min = Vector3.zero;
            foreach(var vertex in _vertices){
                var projected = Math.ProjectPointOnVector(vertex, _eigenVector);
                _projectedPoints.Add(projected);

                var projectedToMax = max - projected;
                var minToProjected = projected - min;
                var minToMax = min - max;
                var dot = Vector3.Dot(_eigenVector, projectedToMax);
                if (dot >= 0 && minToMax.magnitude < minToProjected.magnitude)
                {
                    max = projected;
                    minToMax = min - max;
                }

                dot = Vector3.Dot(_eigenVector, minToProjected);
                if (dot > 0 && minToMax.magnitude < projectedToMax.magnitude)
                {
                    min = projected;
                }
            }

            _minPoint = min;
            _maxPoint = max;
        }

        private void ComputeEigenVector()
        {
            _eigenVector = _covarianceMat.EigenVector(EigenVectorIterations);
        }

        private void ComputeCovarianceMatrix()
        {
            _covarianceMat = Math.GetCovarianceMatrix(_vertices);
        }

        private void OffsetVertices()
        {
            for (var i = 0; i < _vertices.Length; i++)
            {
                _vertices[i] -= _barycenter;
            }
        }

        private void ComputeBarycenter()
        {
            _barycenter = _vertices.Aggregate(Vector3.zero, (current, vertex) => current + vertex);
            _barycenter /= _vertices.Length;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_minPoint, _maxPoint);
        }
    }
}