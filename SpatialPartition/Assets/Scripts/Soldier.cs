using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpatialPartialPattern
{
    public class Soldier
    {
        public MeshRenderer soldierMeshRenderer;
        public Transform soldierTrans;
        public float walkSpeed;
        public Soldier previousSoldier;
        public Soldier nextSoldier;
        public virtual void Move()
        {

        }
        public virtual void Move(Soldier soldier)
        {

        }
    }
}