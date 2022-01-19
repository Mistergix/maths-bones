using UnityEngine;

namespace PGSauce.Games.BoneGenerator
{
    public struct Matrix3x3
    {
        public float m00, m01, m02;
        
        public float m10, m11, m12;
        
        public float m20, m21, m22;

        /// <summary>
        /// An approximation of the eigen vector
        /// </summary>
        /// <param name="iterations"></param>
        /// <returns></returns>
        public Vector3 EigenVector(int iterations)
        {
            var v0 = Vector3.forward;
            var vK = v0;
            for (var k = 0; k < iterations; ++k)
            {
                var product = Product(vK);
                var lK = Math.GreatestComponent(product);
                vK = 1 / lK * product;
            }
            return vK;
        }

        private Vector3 Product(Vector3 v)
        {
            return new Vector3(
                m00 * v.x + m01 * v.y + m02 * v.z,
                m10 * v.x + m11 * v.y + m12 * v.z,
                m20 * v.x + m21 * v.y + m22 * v.z
            );
        }
    }
}