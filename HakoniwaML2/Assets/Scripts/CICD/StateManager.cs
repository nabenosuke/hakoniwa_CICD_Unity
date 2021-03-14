using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Core;

namespace CICD
{
  /*発生しても終了しないイベントのフラグ保持のため、このオブジェクトに情報を保持する
  各イベントスクリプトはイベントが起こったらこのeventStateを直接書き換える
  イベントが発生したらメモリにUnityに結果を書き込む
  robotが増えたらRobotごとに作る？(writerはRobotごと)
  RobotControllerに書き込みは任せる？
  フラグはenum作っておく？
  RobotやWorldを止めたり初期化するのもここ？
  rootじゃなくてここから各イベントを探索して大丈夫？
  */
  public class StateManager : MonoBehaviour, IState
  {
    public EventState eventState = EventState.EVENT_NONE;

    private GameObject root;
    private GameObject myObject;
    private IEvent[] events;
    private double rapTime = 0;

    IoWriter writer;
    void Start()
    {
      this.root = GameObject.Find("Robot");
      this.myObject = GameObject.Find("StateManager");
      events = GetComponentsInChildren<IEvent>();
      Debug.Log("StateManager has " + events.Length + " IEvents");
      Initialize();
    }

    public void Initialize()
    {
      eventState = EventState.EVENT_NONE;
      rapTime = 0;
      foreach (IEvent ievent in events)
      {
        ievent.Initialize();
      }
    }

    public void Goal(double rapTime)
    {
      eventState = EventState.GOAL;
      this.rapTime = rapTime;
    }


    public void HitWall()
    {
      eventState = EventState.HIT_WALL;
    }

    public void TimeOver()
    {
      eventState = EventState.TIME_OVER;
    }

    public EventState GetState()
    {
      return eventState;
    }

    public double GetRapTime()
    {
      return rapTime;
    }
  }
}
