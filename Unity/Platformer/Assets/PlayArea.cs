using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    public RobotAgent blueRobotAgent;
    public RobotAgent redRobotAgent;
    public Transform blueCrate;
    //public Transform blueCrate2;
    public Transform redCrate;

    //[HideInInspector]
    public Transform blueTarget;
    //public Transform blueTarget2;
    //[HideInInspector]
    public Transform redTarget;
    List<RobotAgent> agents = new List<RobotAgent>();
    List<Transform> crates = new List<Transform>();
    [HideInInspector]
    public List<PlatformData> platformData = new List<PlatformData>();
    float totalPlatformLength = 0;
    [HideInInspector]
    public float agentSpawnHOffset;
    float agentSpawnVOffset;
    float rCrateSpawnHOffset;
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
        crates.Add(blueCrate);
        //crates.Add(blueCrate2);
        crates.Add(redCrate);

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

    //public void AssignRewards()
    //{
    //    float distanceToTarget;
    //    foreach (var a in agents)
    //    {
    //        distanceToTarget = Vector3.Distance(a.transform.position, blueTarget.position);
    //        //if (a.team == RobotAgent.Team.Blue)
    //        //{
    //        //    distanceToTarget = Vector3.Distance(a.transform.position, blueTarget.position);
    //        //}
    //        //else
    //        //{
    //        //    distanceToTarget = Vector3.Distance(a.transform.position, redTarget.position);
    //        //}

    //        if (distanceToTarget < 1.8f)
    //        {
    //            a.AddReward(1.0f);
    //        }
    //        else
    //        {
    //            a.AddReward(-1.0f);
    //        }
    //    }
    //}

    public void Reset()
    {
        //foreach (var c in crates)
        //{
        //    // Move the target to a new spot
        //    float[] spawnCoords = GetRandomCrateSpawnCoords(c);
        //    c.position = new Vector3(spawnCoords[0], spawnCoords[1], 0);
        //}

        foreach (var a in agents)
        {
            a.Done();
        }
    }

    private void InitializeParameters()
    {
        agentSpawnHOffset = blueRobotAgent.GetComponent<CircleCollider2D>().radius * blueRobotAgent.transform.localScale.x;
        agentSpawnVOffset = 1.1f;
        rCrateSpawnHOffset = blueCrate.GetComponent<BoxCollider2D>().size.x * blueCrate.localScale.x;
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
        if (c.tag == "crate")
        {
            float tplCrate = totalPlatformLength - platformData.Count * rCrateSpawnHOffset;
            float totalX = Random.value * tplCrate;
            float len = platformData[0].len - rCrateSpawnHOffset;
            int i;
            for (i = 1; len < totalX; i++)
            {
                len += platformData[i].len - rCrateSpawnHOffset;
            }
            float x = platformData[i - 1].beg + totalX + platformData[i - 1].len - rCrateSpawnHOffset - len;
            float y = platformData[i - 1].h;

            return new float[] { x, y };
        }
        else
        {
            float tplAgent = totalPlatformLength - platformData.Count * agentSpawnHOffset * 2;
            float totalX = Random.value * tplAgent;
            float len = platformData[0].len - agentSpawnHOffset * 2;
            int i;
            for (i = 1; len < totalX; i++)
            {
                len += platformData[i].len - agentSpawnHOffset * 2;
            }
            float x = platformData[i - 1].beg + totalX + platformData[i - 1].len - agentSpawnHOffset - len;
            float y = platformData[i - 1].h + agentSpawnVOffset;

            return new float[] { x, y };
        }
    }
}