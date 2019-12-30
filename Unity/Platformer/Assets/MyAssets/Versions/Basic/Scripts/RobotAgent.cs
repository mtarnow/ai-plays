//using System.Collections.Generic;
//using UnityEngine;
//using MLAgents;
//using UnityStandardAssets.CrossPlatformInput;
//using UnityStandardAssets._2D;

//public class RobotAgent : Agent
//{
//    //int m_PlayerIndex;

//    public enum Team
//    {
//        Blue,
//        Red
//    }

//    public Team team;

//    PlayArea playArea;
//    RobotAgent enemyAgent;
//    Transform target;
//    //Transform target2;
//    Transform enemyTarget;

//    Rigidbody2D rBody;
//    Rigidbody2D enemyRBody;
//    PlatformerCharacter2D m_Character;
//    Weapon weapon;

//    public override void InitializeAgent()
//    {
//        base.InitializeAgent();
//        playArea = GetComponentInParent<PlayArea>();

//        if (team == Team.Blue)
//        {
//            //enemyAgent = playArea.redRobotAgent;
//            target = playArea.blueTarget;
//            //target2 = playArea.blueTarget2;
//            //enemyTarget = playArea.redTarget;
//        }
//        else
//        {
//            //enemyAgent = playArea.blueRobotAgent;
//            //target = playArea.redTarget;
//            //enemyTarget = playArea.blueTarget;
//        }

//        rBody = GetComponent<Rigidbody2D>();
//        //enemyRBody = enemyAgent.GetComponent<Rigidbody2D>();
//        m_Character = GetComponent<PlatformerCharacter2D>();
//        weapon = GetComponent<Weapon>();
//    }

//    public override void AgentReset()
//    {
//        //Debug.Log("reset " + team.ToString());
//        playArea.DestroyAllBullets();
//        ResetPos();
//        float[] crateSpawnCoords = playArea.GetRandomCrateSpawnCoords(playArea.blueCrate);
//        if (this.team == Team.Blue)
//        {
//            playArea.blueCrate.position = new Vector3(crateSpawnCoords[0], crateSpawnCoords[1], 0);
//            crateSpawnCoords = playArea.GetRandomCrateSpawnCoords(playArea.blueCrate);
//            //playArea.blueCrate2.position = new Vector3(crateSpawnCoords[0], crateSpawnCoords[1], 0);
//        }
//        else
//        {
//            //playArea.redCrate.position = new Vector3(crateSpawnCoords[0], crateSpawnCoords[1], 0);
//        }
//        weapon.bulletCount = 5;
//    }

//    private void ResetPos()
//    {
//        if (this.transform.position.y < -0.7f)
//        {
//            this.rBody.angularVelocity = 0;
//            this.rBody.velocity = Vector2.zero;
//        }
//        float[] spawnCoords = playArea.GetRandomCrateSpawnCoords(this);
//        this.transform.position = new Vector3(spawnCoords[0], spawnCoords[1], 0);
//    }

//    bool canJump = true;
//    public override void CollectObservations()
//    {
//        if (!canJump)
//            SetActionMask(1, 1);

//        // Target and Agent positions
//        //AddVectorObs(this.transform.position.x);
//        //AddVectorObs(this.transform.position.y);
//        AddVectorObs(this.transform.position.x - target.position.x);
//        AddVectorObs(this.transform.position.y - target.position.y);
//        //AddVectorObs(this.transform.position.x - target2.position.x);
//        //AddVectorObs(this.transform.position.y - target2.position.y);

//        //foreach (var pd in playArea.platformData)
//        //{
//        //    if (pd.platform.name != "Ground")
//        //    {
//        //        AddVectorObs(this.transform.position.x - pd.beg);
//        //        AddVectorObs(this.transform.position.x - pd.end);
//        //    }
//        //}

//        int layerMask;
//        if (this.team == Team.Blue)
//        {
//            layerMask = 5 << 9;
//        }
//        else
//        {
//            layerMask = 65 << 9;
//        }

//        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - this.transform.position, 50, layerMask);

//        if (hit.collider != null && (this.team == Team.Blue && hit.transform.gameObject.layer == 11 ||
//                                     this.team == Team.Red && hit.transform.gameObject.layer == 15))
//        {
//            Debug.DrawRay(transform.position, (target.position - this.transform.position) * 100, Color.green);
//            //Debug.Log("Did not Hit");
//        }
//        else
//        {
//            Debug.DrawRay(transform.position, (target.position - this.transform.position) * hit.distance, Color.red);
//            //Debug.Log("Did Hit");
//        }
//        AddVectorObs(hit.collider != null && (this.team == Team.Blue && hit.transform.gameObject.layer == 11 ||
//                                              this.team == Team.Red && hit.transform.gameObject.layer == 15) ? 1 : hit.distance / 50f);
//        //float xd = hit.collider != null && (this.team == Team.Blue && hit.transform.gameObject.layer == 11 ||
//        //                                    this.team == Team.Red && hit.transform.gameObject.layer == 15) ? 1 : hit.distance / 50f;
//        //Debug.Log("pierwszy: " + xd.ToString());

//        //hit = Physics2D.Raycast(transform.position, target2.position - this.transform.position, 50, layerMask);
//        //if (hit.collider != null && (this.team == Team.Blue && hit.transform.gameObject.layer == 11 || this.team == Team.Red && hit.transform.gameObject.layer == 15))
//        //{
//        //    Debug.DrawRay(transform.position, (target2.position - this.transform.position) * 100, Color.green);
//        //    //Debug.Log("Did not Hit");
//        //}
//        //else
//        //{
//        //    Debug.DrawRay(transform.position, (target2.position - this.transform.position) * hit.distance, Color.red);
//        //    //Debug.Log("Did Hit");
//        //}
//        //AddVectorObs(hit.collider != null && (this.team == Team.Blue && hit.transform.gameObject.layer == 11 || this.team == Team.Red && hit.transform.gameObject.layer == 15) ? 1 : hit.distance / 50f);
//        //xd = hit.collider != null && (this.team == Team.Blue && hit.transform.gameObject.layer == 11 || this.team == Team.Red && hit.transform.gameObject.layer == 15) ? 1 : hit.distance / 50f;
//        //Debug.Log("drugi: " + xd.ToString());
//    }

//    public float speed = 10;
//    public override void AgentAction(float[] vectorAction, string textAction)
//    {
//        //Utils.DebugAgent(this.name == "BlueRobotAgent", this, vectorAction);
//        int moveAct = (int)vectorAction[0];
//        float move = 0;
//        switch (moveAct)
//        {
//            case 0:
//                move = -1;
//                break;
//            case 1:
//                move = 0;
//                break;
//            case 2:
//                move = 1;
//                break;
//        }
//        int jumpAct = (int)vectorAction[1];
//        bool jump = vectorAction[1] == 1 ? true : false;

//        float lBoundX = playArea.transform.position.x - playArea.length / 2 + playArea.agentSpawnHOffset;
//        float rBoundX = playArea.transform.position.x + playArea.length / 2 - playArea.agentSpawnHOffset;
//        if (this.transform.position.x < lBoundX && move == -1 || this.transform.position.x > rBoundX && move == 1)
//        {
//            move = 0;
//        }
//        if (!canJump)
//        {
//            jump = false;
//        }
//        else if (jump)
//        {
//            canJump = false;
//        }

//        //if (m_Character.isGrounded() && jump)
//        //{

//        //}
//        m_Character.Move(move, false, jump);

//        // Rewards
//        float distanceToTarget = Vector3.Distance(this.transform.position, target.position);
//        //float distanceToTarget2 = Vector3.Distance(this.transform.position, target2.position);
//        // Reached target
//        if (distanceToTarget < 1.8f) //|| distanceToTarget2 < 1.8f)
//        {
//            //playArea.AssignRewards();
//            AddReward(1.0f);
//            playArea.Reset();
//        }
//        else
//        {
//            AddReward(-1.0f / this.agentParameters.maxStep);
//        }

//        // Fell off platform
//        if (this.transform.position.y < -0.7f)
//        {
//            ResetPos();
//        }

//    }

//    public override float[] Heuristic()
//    {
//        var action = new float[2];
//        switch (Input.GetAxisRaw("Horizontal"))
//        {
//            case -1:
//                action[0] = 0;
//                break;
//            case 0:
//                action[0] = 1;
//                break;
//            case 1:
//                action[0] = 2;
//                break;
//        }
//        action[1] = CrossPlatformInputManager.GetButtonDown("Jump") ? 1 : 2;
//        return action;
//    }

//    int i = 0;
//    void OnCollisionEnter2D(Collision2D col)
//    {
//        //Debug.Log("OnCollisionEnter2D");
//        if (col.gameObject.tag == "ground")
//        {
//            canJump = true;
//            //Debug.Log("can jump again " + i++.ToString() + "\n[" + Utils.pastStates.lastActions[19, 0].ToString() +
//            //    "] [" + Utils.pastStates.lastActions[19, 1].ToString() + "] " + Utils.actionCounter.ToString() + ", " + Utils.pastStates.lastHeights[19].ToString());
//        }
//    }

//    /*
//    void OnCollisionExit2D(Collision2D col)
//    {
//        Debug.Log("OnCollisionExit2D");
//        if (col.gameObject.tag == "ground")
//        {
//            canJump = false;
//            Debug.Log("can't jump " + i++.ToString() + "\n[" + Utils.pastStates.lastActions[19, 0].ToString() +
//                "] [" + Utils.pastStates.lastActions[19, 1].ToString() + "] " + Utils.actionCounter.ToString() + ", " + Utils.pastStates.lastHeights[19].ToString());
//        }
//    }
//    */
//}
