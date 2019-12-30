using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    public RobotAgent blueRobotAgent;
    public RobotAgent redRobotAgent;
    public Transform blueCrate;
    //public Transform blueCrate2;
    //public Transform redCrate;

    //[HideInInspector]
    public Transform blueTarget;
    //public Transform blueTarget2;
    //[HideInInspector]
    //public Transform redTarget;
    List<RobotAgent> agents = new List<RobotAgent>();
    List<RobotAgent> unregistered = new List<RobotAgent>();
    List<Transform> crates = new List<Transform>();
    [HideInInspector]
    public List<PlatformData> platformData = new List<PlatformData>();
    float totalPlatformLength = 0;
    [HideInInspector]
    public float agentX;
    float agentSpawnY;
    float crateX;
    [HideInInspector]
    public float length;

    public class PlatformData
    {
        public Transform platform;
        public float beg;
        public float end;
        public float h;
        public float len;

        public PlatformData(Transform p)
        {
            platform = p;
            beg = p.position.x;
            end = beg + p.GetComponent<BoxCollider2D>().size.x * p.localScale.x;
            h = p.position.y + p.GetComponent<BoxCollider2D>().size.y * p.localScale.y;
            len = end - beg;
        }
    }


    public void Start()
    {
        //blueTarget = blueCrate.Find("BlueTarget");
        //redTarget = redCrate.Find("RedTarget");
        agents.Add(blueRobotAgent);
        agents.Add(redRobotAgent);
        unregistered = new List<RobotAgent>(agents);
        crates.Add(blueCrate);
        //crates.Add(blueCrate2);
        //crates.Add(redCrate);

        foreach (Transform t in this.transform)
        {
            if (t.tag == "ground")
            {
                platformData.Add(new PlatformData(t));
            }
        }

        foreach (PlatformData pd in platformData)
        {
            totalPlatformLength += pd.end - pd.beg;
        }

        InitializeParameters();
    }

    
    public void RegisterForReset(RobotAgent agent)
    {
        unregistered.Remove(agent);
        if (unregistered.Count == 0)
        {
            unregistered = new List<RobotAgent>(agents);
            Reset();
        }
    }

    public void Reset()
    {
        //foreach (var c in crates)
        //{
        //    // Move the target to a new spot
        //    float[] spawnCoords = GetRandomCrateSpawnCoords(c);
        //    c.position = new Vector3(spawnCoords[0], spawnCoords[1], 0);
        //}
        Debug.Log("playarea reseting");
        foreach (var a in agents)
        {
            a.Done();
        }
    }

    private void InitializeParameters()
    {
        agentX = 2 * blueRobotAgent.GetComponent<CircleCollider2D>().radius * blueRobotAgent.transform.localScale.x;
        agentSpawnY = 1.1f;
        crateX = blueCrate.GetComponent<BoxCollider2D>().size.x * blueCrate.localScale.x;
        foreach (PlatformData pd in platformData)
        {
            if (pd.platform.name == "Ground")
            {
                length = pd.len;
                break;
            }
        }
    }

    public float[] GetRandomCrateSpawnCoords(Component c)
    {
        if (c.tag == "ground")
        {
            foreach(PlatformData pd in platformData)
            {
                if (pd.platform.name == c.name)
                {
                    Debug.Log(c.name);
                }
            }
            float tplCrate = totalPlatformLength - platformData.Count * crateX;
            float totalX = Random.value * tplCrate;
            float len = platformData[0].len - crateX;
            int i;
            for (i = 1; len < totalX; i++)
            {
                len += platformData[i].len - crateX;
            }
            float x = platformData[i - 1].beg + totalX + platformData[i - 1].len - crateX - len;
            float y = platformData[i - 1].h;

            return new float[] { x, y };
        }
        else if (c.tag == "crate")
        {
            float tplCrate = totalPlatformLength - platformData.Count * crateX;
            float totalX = Random.value * tplCrate;
            float len = platformData[0].len - crateX;
            int i;
            for (i = 1; len < totalX; i++)
            {
                len += platformData[i].len - crateX;
            }
            float x = platformData[i - 1].beg + totalX + platformData[i - 1].len - crateX - len;
            float y = platformData[i - 1].h;

            return new float[] { x, y };
        }
        else
        {
            float tplAgent = totalPlatformLength - platformData.Count * agentX;
            float totalX = Random.value * tplAgent;
            float len = platformData[0].len - agentX;
            int i;
            for (i = 1; len < totalX; i++)
            {
                len += platformData[i].len - agentX;
            }
            float x = platformData[i - 1].beg + totalX + platformData[i - 1].len - agentX/2 - len;
            float y = platformData[i - 1].h + agentSpawnY;

            return new float[] { x, y };
        }
    }

    public void DestroyAllBullets()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("bullet");

        for (var i = 0; i < gameObjects.Length; i++)
            Destroy(gameObjects[i]);
    }
}