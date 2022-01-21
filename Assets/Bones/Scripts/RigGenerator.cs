using System;
using System.Collections.Generic;
using PGSauce.Core.PGDebugging;
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
        [SerializeField] private float minDistanceCheckBones;
        [SerializeField] private float epsilonMerge;
        [SerializeField] private RagdollCreator ragdollCreator;

        private ConcreteSerializableDictionary<BodyPart, BonesGenerator> _boneGenerators;

        private void Start()
        {
            _boneGenerators = new ConcreteSerializableDictionary<BodyPart, BonesGenerator>();
            
            GenerateBones();
            Rig();
        }

        private void Rig()
        { 
            ParentBones(); 
            CreateJoints();
            RepositionRenderers();
            ragdollCreator.OnWizardCreate(_boneGenerators);
        }

        private void RepositionRenderers()
        {
            foreach (var generator in _boneGenerators)
            {
                generator.Value.RepositionRenderer();
            }
        }

        private void CreateJoints()
        {
            var childCount = mainBodyPart.Children.Count;
            var index = 0;
            var currentParent = mainBodyPart;
            var currentChild = mainBodyPart.Children[0];

            while (index < childCount)
            {
                var parentBone = _boneGenerators[currentParent];
                var childBone = _boneGenerators[currentChild];
                
                PGDebug.Message($"Parent is {parentBone.name}, child is {childBone.name}").Log();

                var distanceMinMax = Vector3.Distance(parentBone.MinPoint, childBone.MaxPoint);
                var distanceMinMin = Vector3.Distance(parentBone.MinPoint, childBone.MinPoint);
                var distanceMaxMin = Vector3.Distance(parentBone.MaxPoint, childBone.MinPoint);
                var distanceMaxMax = Vector3.Distance(parentBone.MaxPoint, childBone.MaxPoint);

                var distance = Mathf.Min( distanceMaxMin, distanceMinMax, distanceMaxMax, distanceMinMin);
                
                 if(distance < minDistanceCheckBones)
                {
                    if (IsDistance(distance, distanceMinMax))
                    {
                        if (distance < epsilonMerge)
                        {
                            childBone.RepositionMax(parentBone.MinPoint);
                        }
                        else
                        {
                            CreateJoint(childBone.MaxPoint, parentBone, childBone);
                        }
                    }
                    else if (IsDistance(distance, distanceMaxMin))
                    {
                        if (distance < epsilonMerge)
                        {
                            childBone.RepositionMin(parentBone.MaxPoint);
                        }
                        else
                        {
                            CreateJoint(childBone.MinPoint, parentBone, childBone);
                        }
                    }
                    else if (IsDistance(distance, distanceMaxMax))
                    {
                        if (distance < epsilonMerge)
                        {
                            childBone.RepositionMax(parentBone.MaxPoint);
                        }
                        else
                        {
                            CreateJoint(childBone.MaxPoint, parentBone, childBone);
                        }
                    }
                    else
                    {
                        if (distance < epsilonMerge)
                        {
                            childBone.RepositionMin(parentBone.MinPoint);
                        }
                        else
                        {
                            CreateJoint(childBone.MinPoint, parentBone, childBone);
                        }
                    }
                }
                 

                if(currentChild.Children.Count != 0)
                {
                    currentParent = currentChild;
                    currentChild = currentChild.Children[0];
                }
                else
                {
                    index += 1;
                    if (index < childCount)
                    {
                        currentParent = mainBodyPart;
                        currentChild = mainBodyPart.Children[index];
                    }
                }
            }
            
        }

        private static bool IsDistance(float distance, float distanceMinMax)
        {
            const float distanceTolerance = 0.001f;
            return System.Math.Abs(distance - distanceMinMax) < distanceTolerance;
        }

        private void CreateJoint(Vector3 childPoint, BonesGenerator parentBone, BonesGenerator childBone)
        {
            PGDebug.Message($"Create Joint : PARENT {parentBone.name}, CHILD {childBone.name}").Log();
            childBone.Reposition(childPoint);
        }

        private void ParentBones()
        {
            foreach (var bodyPartMesh in bodyPartMeshes)
            {
                if (bodyPartMesh.Key.parent)
                {
                    var childTransform = _boneGenerators[bodyPartMesh.Key].transform;
                    childTransform.SetParent(_boneGenerators[bodyPartMesh.Key.parent].transform);
                    childTransform.localPosition = bodyPartMesh.Key.boneLocalPosition;
                    
                    bodyPartMesh.Key.parent.Children.Add(bodyPartMesh.Key);
                }
            }

            foreach (var bodyPartMesh in bodyPartMeshes)
            {
                _boneGenerators[bodyPartMesh.Key].RecordRenderer();
            }
        }

        private void GenerateBones()
        {
            foreach (var bodyPartMesh in bodyPartMeshes)
            {
                var bonesGenerator = Instantiate(bonesGeneratorPrefab);
                bonesGenerator.gameObject.name = bodyPartMesh.Key.name;
                _boneGenerators[bodyPartMesh.Key] = bonesGenerator;
                bonesGenerator.GenerateBone(bodyPartMesh.Value, material, bodyPartMesh.Key);
                bodyPartMesh.Key.Children = new List<BodyPart>();
            }
        }
    }
}