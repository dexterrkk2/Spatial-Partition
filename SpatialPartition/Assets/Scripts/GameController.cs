using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SpatialPartialPattern
{
    public class GameController : MonoBehaviour
    {
        public GameObject friendlyObj;
        public GameObject enemyObj;

        public Material enemyMaterial;
        public Material closestEnemyMaterial;

        public Transform enemyparent;
        public Transform friendlyParent;

        List<Soldier> enemySoldiers = new List<Soldier>();
        List<Soldier> friendlySoldiers = new List<Soldier>();

        List<Soldier> closestEnemies = new List<Soldier>();

        float mapWidth = 50f;
        int cellSize = 10;

        public int numberOfSoldiers = 100;

        public bool usePartition;
        Grid grid;

        public Text fps;
        private void Start()
        {
            grid = new Grid((int)mapWidth, cellSize);
            for(int i =0; i<numberOfSoldiers; i++)
            {
                Vector3 randomPos = new Vector3(Random.Range(0, mapWidth), .5f, Random.Range(0f, mapWidth));
                GameObject newEnemy = Instantiate(enemyObj, randomPos, Quaternion.identity) as GameObject;
                enemySoldiers.Add(new Enemy(newEnemy, mapWidth, grid));
                newEnemy.transform.parent = enemyparent;
                randomPos = new Vector3(Random.Range(0, mapWidth), .5f, Random.Range(0f, mapWidth));
                GameObject newFriendly = Instantiate(friendlyObj, randomPos, Quaternion.identity) as GameObject;
                friendlySoldiers.Add(new Friendly(newFriendly, mapWidth));
                newFriendly.transform.parent = friendlyParent;
            }
        }
        private void Update()
        {
            float startTime = Time.realtimeSinceStartup;
            for (int i =0; i<enemySoldiers.Count; i++)
            {
                enemySoldiers[i].Move();
            }

            for(int i =0; i<closestEnemies.Count; i++)
            {
                closestEnemies[i].soldierMeshRenderer.material = enemyMaterial;
            }
            closestEnemies.Clear();
            Soldier closestEnemy;
            for(int i =0; i<friendlySoldiers.Count; i++)
            {
                if (!usePartition)
                {
                    closestEnemy = FindClosestEnemySlow(friendlySoldiers[i]);
                }
                else
                {
                    closestEnemy = grid.FindClosestEnemy(friendlySoldiers[i]);
                }
                if(closestEnemy != null)
                {
                    closestEnemy.soldierMeshRenderer.material = closestEnemyMaterial;
                    closestEnemies.Add(closestEnemy);
                    friendlySoldiers[i].Move(closestEnemy);
                }
            }
            float elapsedTime = (Time.realtimeSinceStartup - startTime) * 1000f;
            fps.text = elapsedTime + " ms";
        }
        Soldier FindClosestEnemySlow(Soldier soldier)
        {
            Soldier closestEnemy = null;

            float bestDistSqr = Mathf.Infinity;

            //Loop thorugh all enemies
            for (int i = 0; i < enemySoldiers.Count; i++)
            {
                //The distance sqr between the soldier and this enemy
                float distSqr = (soldier.soldierTrans.position - enemySoldiers[i].soldierTrans.position).sqrMagnitude;

                //If this distance is better than the previous best distance, then we have found an enemy that's closer
                if (distSqr < bestDistSqr)
                {
                    bestDistSqr = distSqr;

                    closestEnemy = enemySoldiers[i];
                }
            }

            return closestEnemy;
        }
        public void UsePartician()
        {
            usePartition = !usePartition;
        }
    }
}