using System.Collections.Generic;
using UnityEngine;

namespace PGSauce.Games.BoneGenerator
{
    [CreateAssetMenu(menuName = "Bones/Body Part")]
    public class BodyPart : ScriptableObject
    {
        public BodyPart parent;
        public Vector3 boneLocalPosition;
        public List<BodyPart> Children { get; set; }
    }
}