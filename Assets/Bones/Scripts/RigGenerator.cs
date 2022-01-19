using System;
using System.Collections.Generic;
using PGSauce.Core.SerializableDictionaries;
using UnityEngine;

namespace PGSauce.Games.BoneGenerator
{
    public class RigGenerator : MonoBehaviour
    {
        [SerializeField] private ConcreteSerializableDictionary<BodyPart, Mesh> bodyPartMeshes;
        [SerializeField] private BonesGenerator bonesGeneratorPrefab;
        [SerializeField] private Material material;
        [SerializeField] private BodyPart mainBodyPart;

        private ConcreteSerializableDictionary<BodyPart, BonesGenerator> _boneGenerators;
        private List<Mesh> _meshes;
        private BonesGenerator _mainBone;

        private void Start()
        {
            _boneGenerators = new ConcreteSerializableDictionary<BodyPart, BonesGenerator>();
            _meshes = new List<Mesh>();
            
            GenerateBones();
            Rig();
        }

        private void Rig()
        {
            ParentBones();
        }

        private void ParentBones()
        {
            foreach (var bodyPartMesh in bodyPartMeshes)
            {
                if (bodyPartMesh.Key.parent)
                {
                    _boneGenerators[bodyPartMesh.Key].transform
                        .SetParent(_boneGenerators[bodyPartMesh.Key.parent].transform);
                }
            }
        }

        private void GenerateBones()
        {
            foreach (var bodyPartMesh in bodyPartMeshes)
            {
                var bonesGenerator = Instantiate(bonesGeneratorPrefab);
                bonesGenerator.gameObject.name = bodyPartMesh.Key.name;
                _boneGenerators[bodyPartMesh.Key] = bonesGenerator;
                bonesGenerator.GenerateBone(bodyPartMesh.Value, material);
            }
            
            _mainBone = _boneGenerators[mainBodyPart];
        }
    }
}