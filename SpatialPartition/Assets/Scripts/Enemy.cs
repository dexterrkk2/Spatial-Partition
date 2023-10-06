using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpatialPartialPattern
{
    public class Enemy : Soldier
    {
        Vector3 currentTarget;
        Vector3 oldPos;
        float mapWidth;
        Grid grid;

        public Enemy(GameObject soldierObj, float mapWidth, Grid grid)
        {
            this.soldierTrans = soldierObj.transform;
            this.soldierMeshRenderer = soldierObj.GetComponent<MeshRenderer>();
            this.mapWidth = mapWidth;
            this.grid = grid;

            grid.Add(this);
            oldPos = soldierTrans.position;
            this.walkSpeed = 5f;
            GetNewTarget();
        }
        void GetNewTarget()
        {
            currentTarget = new Vector3(Random.Range(0, mapWidth), .5f, Random.Range(0, mapWidth));
            soldierTrans.rotation = Quaternion.LookRotation(currentTarget - soldierTrans.position);
        }
        public override void Move()
        {
            soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);

            grid.Move(this, oldPos);

            oldPos = soldierTrans.position;

            if((soldierTrans.position - currentTarget).magnitude < 1f)
            {
                GetNewTarget();
            }
        }
    }
}
