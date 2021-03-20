using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;

namespace CICD
{
  public class Reset : MonoBehaviour
  {


    private GameObject[] robots;
    private GameObject[] envs;
    private GameObject stateManager;
    private Vector3[] robotsTransform;
    private Vector3[] envsTransform;
    private Quaternion[] robotsQuaternion;
    private Quaternion[] envsQuaternion;
    private Hakoniwa.GUI.SimStartCICD simStart;
    private int robotsNum;
    private int envsNum;
    // Start is called before the first frame update
    public void Initialize()
    {
      stateManager = GameObject.Find("env/StateManager");
      simStart = stateManager.GetComponent<Hakoniwa.GUI.SimStartCICD>();
      if (stateManager == null)
      {
        UnityEngine.Debug.Log("StateManagerが取得できませんでした");
      }
      RobotsInit();
      EnvsInit();

    }

    private void RobotsInit()
    {
      robots = GameObject.Find("Robot").GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
      robotsNum = robots.Length;
      robotsTransform = new Vector3[robotsNum];
      robotsQuaternion = new Quaternion[robotsNum];

      int index = 0;
      foreach (var parts in robots)
      {
        robotsTransform[index] = robots[index].transform.localPosition;
        robotsQuaternion[index] = robots[index].transform.localRotation;
        index++;
      }

    }

    private void EnvsInit()
    {
      envs = GameObject.Find("env").GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
      envsNum = envs.Length;
      envsTransform = new Vector3[envsNum];
      envsQuaternion = new Quaternion[envsNum];

      int index = 0;
      foreach (var parts in envs)
      {
        envsTransform[index] = envs[index].transform.localPosition;
        envsQuaternion[index] = envs[index].transform.localRotation;
        index++;
      }

    }

    private void RobotsReset()
    {
      int index = 0;
      foreach (var parts in robots)
      {
        robots[index].transform.localPosition = robotsTransform[index];
        robots[index].transform.localRotation = robotsQuaternion[index];
        index++;
      }
    }

    private void EnvsReset()
    {
      int index = 0;
      foreach (var parts in envs)
      {
        envs[index].transform.localPosition = envsTransform[index];
        envs[index].transform.localRotation = envsQuaternion[index];
        index++;
      }
    }

    public void AppReset()
    {
      RobotsReset();
      EnvsInit();
      stateManager.GetComponent<StateManager>().Initialize();
      simStart.CloseSim();
      //simStart.StartAthrill();
      //UnityEngine.Debug.Log("Reset");
      /* 再起動
          System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe"));
          Application.Quit();
          */
    }

    public void StartAthrill()
    {

      simStart.StartAthrill();
    }

  }
}
