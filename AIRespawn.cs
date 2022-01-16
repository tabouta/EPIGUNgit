using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class AIRespawn : MonoBehaviour
    {
        private int numberOfAI;

        [SerializeField]
        private int AIincrement;

        [SerializeField]
        private GameObject AI;

        [SerializeField]
        private string AITag = "AI";

        [SerializeField]
        private Vector3 spawnPoint;

        // Use this for initialization
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (GameObject.FindGameObjectWithTag(AITag) == null)
                NewWave();
        }

        private void NewWave()
        {
            GameObject t;
            numberOfAI += AIincrement;
            for(int i = 0; i < numberOfAI; i++)
            {
                t = Instantiate(AI);
                t.gameObject.transform.SetPositionAndRotation(spawnPoint,Quaternion.identity);
            }
        }
    }
}