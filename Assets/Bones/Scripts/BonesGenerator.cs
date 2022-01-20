using System.Collections.Generic;
using System.Linq;
using PGSauce.Core;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;
using UnityEngine;

namespace PGSauce.Games.BoneGenerator
{
    public class BonesGenerator : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private GlobalFloat boneGizmosRadius;

        public Vector3 debugOffset;

        private Vector3[] _vertices;
        private Vector3 _barycenter;
        private Matrix3x3 _covarianceMat;
        private Vector3 _eigenVector;
        private List<Vector3> _projectedPoints;
        private Vector3 _maxPoint;
        private Vector3 _minPoint;
        private BodyPart _bodyPart;
        private Vector3 _rendererPos;

        private const int EigenVectorIterations = 100;

        public Vector3 MaxPoint => transform.GetWorldPosition(_maxPoint + debugOffset);

        public Vector3 MinPoint => transform.GetWorldPosition(_minPoint + debugOffset);

        public void GenerateBone(Mesh mesh, Material material, BodyPart bodyPart)
        {
            _projectedPoints = new List<Vector3>();
            _bodyPart = bodyPart;
            
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

        public void SetMinPointFromWorldPosition(Vector3 worldPos)
        {
            _minPoint = transform.GetLocalPosition(worldPos);
        }

        public void SetMaxPointFromWorldPosition(Vector3 worldPos)
        {
            _maxPoint = transform.GetLocalPosition(worldPos);
        }

        private void OffsetBackVertices()
        {
            for (var i = 0; i < _vertices.Length; i++)
            {
                _vertices[i] += _barycenter;
            }

            _minPoint = MinPoint + _barycenter;
            _maxPoint = MaxPoint + _barycenter;
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
            Gizmos.color = boneGizmosRadius ? Color.blue : Color.black;
            Gizmos.DrawLine(MinPoint, MaxPoint);
            if (boneGizmosRadius)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(MinPoint, boneGizmosRadius);
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(MaxPoint, boneGizmosRadius);
            }
        }
        
        public void RepositionMax(Vector3 point)
        {
            Reposition(point);
            SetMaxPointFromWorldPosition(point);
        }

        public void Reposition(Vector3 point)
        {
            var local = transform.parent.GetLocalPosition(point);
            var localOffset = (_bodyPart.boneLocalPosition - local);
            PGDebug.Message($"Reposition : LOCAL {local.ToString("F4")}, OFFSET {localOffset.ToString("F4")} for {name}").Log();
            //_minPoint += localOffset;
            //_maxPoint += localOffset;
            transform.position = point;

            var children = GetComponentsInChildren<BonesGenerator>();
            foreach (var child in children)
            {
                child._minPoint += localOffset;
                child._maxPoint += localOffset;
            }
        }

        public void RepositionMin(Vector3 point)
        {
            Reposition(point);
            SetMinPointFromWorldPosition(point);
        }

        public void RecordRenderer()
        {
            _rendererPos = meshFilter.transform.position;
        }

        public void RepositionRenderer()
        { 
            meshFilter.transform.position = _rendererPos;
        }
    }
}