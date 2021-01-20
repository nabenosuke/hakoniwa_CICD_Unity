using Hakoniwa.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CICD
{
  public class CheckPointManager : MonoBehaviour, IEvent
  {
    //全てのチェックポイントの通過状況を管理
    //自分より上に並んでいるチェックポイントが全て通過されている状況でのみ、通過を判定する
    private CheckPoint[] checkPoints;
    private StateManager stateMagager;
    private SimTime simTime;
    private int checkPointsNum;
    private int nowCheckPoint;
    [SerializeField] private int nextCheckPoint;
    // Start is called before the first frame update
    void Start()
    {
      stateMagager = GameObject.Find("StateManager").GetComponent<StateManager>();
      simTime = GameObject.Find("StateManager/SimTime").GetComponent<SimTime>();
      checkPoints = GetComponentsInChildren<CheckPoint>();
      checkPointsNum = checkPoints.Length;
      Debug.Log("The number of CheckPoint is " + checkPointsNum);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize()
    {
      nextCheckPoint = 0;
    }

    public void UpdateCheckPoint()
    {
      int count = 0;
      foreach (CheckPoint checkPoint in checkPoints)
      {
        if (checkPoint.isCheck)
        {
          nowCheckPoint = count;
        }
        count++;
      }

      if (nowCheckPoint == nextCheckPoint)
      {
        nextCheckPoint++;
        if (nextCheckPoint == checkPointsNum)
        {
          if (stateMagager.eventState == EventState.EVENT_NONE)
          {
            Debug.Log("goal");
            double rapTime = simTime.GetRapTime();
            stateMagager.Goal(rapTime);
          }
        }
      }
    }

  }
}
