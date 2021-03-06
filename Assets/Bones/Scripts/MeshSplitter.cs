using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.PGDebugging;

namespace PGSauce.Games.BoneGenerator
{
    public class MeshSplitter : MonoBehaviour
    {
        #region Public And Serialized Fields
        [SerializeField] private MeshProvider meshProvider;
        #endregion
        #region Private Fields
        #endregion
        #region Properties
        #endregion
        #region Unity Functions
        public void Awake()
        {
            var go = new GameObject("Instantiated Mesh");
            var filter = go.AddComponent<MeshFilter>();
            var meshRenderer = go.AddComponent<MeshRenderer>();
            filter.mesh = meshProvider.Mesh;
            meshRenderer.sharedMaterial = meshRenderer.material;
            go.transform.SetParent(transform, true);
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
