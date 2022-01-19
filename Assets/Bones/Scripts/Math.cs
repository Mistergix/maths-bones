using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PGSauce.Games.BoneGenerator
{
    public static class Math
    {
        public static Matrix3x3 GetCovarianceMatrix(Vector3[] vertices)
        {
            var x = vertices.Select(vertex => vertex.x).ToList();
            var y = vertices.Select(vertex => vertex.y).ToList();
            var z = vertices.Select(vertex => vertex.z).ToList();

            var covXY = Covariance(x, y);
            var covXZ = Covariance(x, z);
            var covYZ = Covariance(y, z);

            var mat = new Matrix3x3();
            mat.m00 = Variance(x);
            mat.m11 = Variance(y);
            mat.m22 = Variance(z);

            mat.m01 = mat.m10 = covXY;
            mat.m02 = mat.m20 = covXZ;
            mat.m12 = mat.m21 = covYZ;

            return mat;
        }

        private static float Covariance(IReadOnlyCollection<float> x, IReadOnlyCollection<float> y)
        {
            var avgX = Average(x);
            var avgY = Average(y);
            return x.Zip(y, (xi, yi) => xi * yi).Sum() / x.Count - avgX * avgY;
        }

        private static float Variance(IReadOnlyCollection<float> values)
        {
            // E(X²) - E²(X)
            var avg = Average(values);
            return values.Sum(value => value * value) / values.Count - avg * avg;
        }

        private static float Average(IReadOnlyCollection<float> values)
        {
            return values.Sum() / values.Count;
        }
        
        public static float GreatestComponent(Vector3 v)
        {
            var absX = Mathf.Abs(v.x);
            var absY = Mathf.Abs(v.y);
            var absZ = Mathf.Abs(v.z);

            if (absX < absY)
            {
                return absY < absZ ? v.z : v.y;
            }
            
            return absX < absZ ? v.z : v.x;
        }

        public static Vector3 ProjectPointOnVector(Vector3 point, Vector3 vector)
        {
            return Vector3.Dot(point, vector) / vector.sqrMagnitude * vector;
        }
    }
}