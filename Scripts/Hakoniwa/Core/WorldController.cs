﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakoniwa.Core
{
  public class WorldController : MonoBehaviour
  {
    private long unity_simtime = 1;
    public long maxDiffTime = 20000; /* usec */
    public int waitCount = 0;
    [SerializeField] private double dbgUnityStimeSec = 0.0;
    public long[] diff_time;
    public double[] dbgMiconStimeSec;
    public double[] dbgDiffTimeMsec;
    private List<IAssetController> assets = new List<IAssetController>();
    private GameObject root;
    private long delta_time = 0;

    //cicd
    private bool isStop = false;

    void Start()
    {
      this.root = GameObject.Find("Robot");
      int count = 0;

      HakoniwaConfig cfg = GameObject.Find("Hakoniwa").GetComponentInChildren<HakoniwaConfig>();
      cfg.Initialize();

      foreach (Transform child in this.root.transform)
      {
        count++;
        Debug.Log("child=" + child.name);
        GameObject obj = root.transform.Find(child.name).gameObject;
        IAssetController ctrl = obj.GetComponentInChildren<Hakoniwa.Core.IAssetController>();
        ctrl.Initialize();
        this.assets.Add(ctrl);
      }
      this.diff_time = new long[count];
      this.dbgMiconStimeSec = new double[count];
      this.dbgDiffTimeMsec = new double[count];
      this.delta_time = (long)(Time.fixedDeltaTime * 1000000f);
      Physics.autoSimulation = false;

    }

    public bool isConnected = true;
    public bool canStep = true;
    void FixedUpdate()
    {
      foreach (IAssetController asset in this.assets)
      {
        //Debug.Log(asset.CanStart());
        if (!asset.CanStart())
        {
          isStop = true;
          return;
        }
        else
        {
          isStop = false;
          asset.StartAthrill();
        }
      }

      int index = 0;
      isConnected = true;
      canStep = true;
      /*
       * Check diff time and connection.
       */
      foreach (IAssetController asset in this.assets)
      {
        if (this.diff_time[index] <= -this.maxDiffTime)
        {
          canStep = false;
        }
        if (!asset.IsConnected())
        {
          isConnected = false;
        }
        index++;
      }
      /*
       * judge simulation can step
       */
      if (isConnected && canStep)
      {
        this.unity_simtime += this.delta_time;
        foreach (IAssetController asset in this.assets)
        {
          asset.DoUpdate();
        }
        Physics.Simulate(Time.fixedDeltaTime);
      }
      else
      {
        waitCount++;
      }
      /*
       * publish hakoniwa time
       */
      foreach (IAssetController asset in this.assets)
      {
        asset.DoPublish(this.unity_simtime);
      }
    }
    void Update()
    {
      if (isStop)
      {
        return;
      }

      int index = 0;
      foreach (IAssetController asset in this.assets)
      {
        this.diff_time[index] = (long)asset.GetControllerTime() - (long)this.unity_simtime;
        this.dbgDiffTimeMsec[index] = ((double)this.diff_time[index]) / 1000;
        this.dbgMiconStimeSec[index] = ((double)asset.GetControllerTime()) / 1000000;
        index++;
      }
      this.dbgUnityStimeSec = ((double)this.unity_simtime) / 1000000;


    }
    public double GetSimTime()
    {
      return this.dbgUnityStimeSec;
    }


    public void Reset()
    {
      this.unity_simtime = 0;
      this.dbgUnityStimeSec = 0;
    }
  }
}
