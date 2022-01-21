using System;
using UnityEngine;

namespace PGSauce.Games.BoneGenerator
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private RagdollCreator ragdoll;
        public float force;
        public ForceMode ForceMode;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ragdoll.TorsoRb.AddForce(Vector3.up * force, ForceMode);
            }
        }
    }
}