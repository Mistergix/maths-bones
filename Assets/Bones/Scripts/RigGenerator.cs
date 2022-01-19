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

        private ConcreteSerializableDictionary<BodyPart, BonesGenerator> _boneGenerators;
        private List<Mesh> _meshes;

        private void Start()
        {
            _boneGenerators = new ConcreteSerializableDictionary<BodyPart, BonesGenerator>();
            _meshes = new List<Mesh>();
            foreach (var bodyPartMesh in bodyPartMeshes)
            {
                var bonesGenerator = Instantiate(bonesGeneratorPrefab);
                bonesGenerator.gameObject.name = bodyPartMesh.Key.name;
                _boneGenerators[bodyPartMesh.Key] = bonesGenerator;
                bonesGenerator.GenerateBone(bodyPartMesh.Value, material);
            }
        }
    }
}