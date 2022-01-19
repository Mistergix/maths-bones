using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.PGDebugging;
using Sirenix.OdinInspector;

namespace PGSauce.Games.BoneGenerator
{
    public class MeshProvider : MonoBehaviour
    {
        #region Public And Serialized Fields

        [SerializeField] private Material material;
        [SerializeField] private SkinnedMeshRenderer meshRenderer;
        #endregion
        #region Private Fields
        #endregion
        #region Properties
        [ShowInInspector]
        public Mesh Mesh => meshRenderer.sharedMesh;
        public Material Material => material;
        #endregion
        #region Unity Functions
        public void Awake()
        {
        }
        
        public void Start()
        {
        }
        
        public void Update()
        {
        }
        
        public void OnEnable()
        {
        }
        
        public void OnDisable()
        {
        }
        
        public void OnDestroy()
        {
        }
        
        #endregion
        #region Public Methods
        #endregion
        #region Private Methods
        #endregion
    }
}
