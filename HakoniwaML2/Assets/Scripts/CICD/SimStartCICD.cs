using Hakoniwa.Core;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Hakoniwa.GUI
{
  //デバッグモードで実行できるように
  public class SimStartCICD : MonoBehaviour
  {
    private Process exProcess = new Process();
    private bool isInitialized = false;
    private string terminal = null;
    private string athrillPath = null;
    HakoniwaConfig hakoniwa_cfg = null;

    [SerializeField] private bool isDebug = false;

    public void Initialize()
    {
      GameObject hakoniwa = GameObject.Find("Hakoniwa");
      hakoniwa_cfg = hakoniwa.GetComponentInChildren<Hakoniwa.Core.HakoniwaConfig>();
      if (hakoniwa_cfg == null)
      {
        UnityEngine.Debug.LogError("Not found hakoniwa_cfg : " + hakoniwa_cfg);
        return;
      }
      HakoniwaConfigInfo cfg = hakoniwa_cfg.GetConfig();
      if (cfg == null)
      {
        UnityEngine.Debug.LogError("Not found cfg : " + cfg);
        return;
      }

      this.terminal = cfg.TerminalPath;
      this.athrillPath = cfg.AthrillPath;
    }

    private void activate_one(HakoniwaRobotConfigInfo robot_cfg)
    {
      string arg = null;
      if (isDebug == false)
      {
        if (this.hakoniwa_cfg.IsMmap(robot_cfg))
        {

          arg = string.Format("{0} -c1 -t -1 -m {1}/{2}/memory_mmap.txt  -d {1}/{2}/device_config_mmap.txt {1}/{3} ",
              athrillPath,
              robot_cfg.WorkspacePathUnix,
              robot_cfg.ApplicationName,
              robot_cfg.BinaryName);
        }
        else
        {
          arg = string.Format("{0} -c1 -t -1 -m {1}/{2}/memory.txt  -d {1}/{2}/device_config.txt {1}/{3} ",
              athrillPath,
              robot_cfg.WorkspacePathUnix,
              robot_cfg.ApplicationName,
              robot_cfg.BinaryName);
        }
      }
      else
      {
        UnityEngine.Debug.Log("Debug Mode");
        if (this.hakoniwa_cfg.IsMmap(robot_cfg))
        {

          arg = string.Format("{0} -c1 -i -t -1 -m {1}/{2}/memory_mmap.txt  -d {1}/{2}/device_config_mmap.txt {1}/{3} ",
              athrillPath,
              robot_cfg.WorkspacePathUnix,
              robot_cfg.ApplicationName,
              robot_cfg.BinaryName);
        }
        else
        {
          arg = string.Format("{0} -c1 -i -t -1 -m {1}/{2}/memory.txt  -d {1}/{2}/device_config.txt {1}/{3} ",
              athrillPath,
              robot_cfg.WorkspacePathUnix,
              robot_cfg.ApplicationName,
              robot_cfg.BinaryName);
        }
      }
      if (this.terminal.Contains("wsl"))
      {
        //UnityEngine.Debug.Log(terminal);
        exProcess.StartInfo.FileName = this.terminal;
        exProcess.StartInfo.Arguments = arg;
      }
      else if (this.terminal.Contains("open"))
      {
        string exec_cmd = hakoniwa_cfg.GetCurrentPath() + "/athrill.command";
        StreamWriter writer = new StreamWriter(exec_cmd);
        writer.Write(arg);
        writer.Close();
        exProcess.StartInfo.FileName = "/usr/bin/open";
        exProcess.StartInfo.Arguments = exec_cmd;
        //exProcess = Process.Start("/usr/bin/open", exec_cmd);
        UnityEngine.Debug.Log("exec= " + exec_cmd);
      }
      exProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
      exProcess.Start();
      isInitialized = true;
      //UnityEngine.Debug.Log("arg=" + arg);
      //UnityEngine.Debug.Log("exProcess=" + exProcess);
    }

    public void OnButtonClick()
    {
      //UnityEngine.Debug.Log("Button clicked");
      if (isInitialized == false)
      {
        this.Initialize();
        GameObject root = GameObject.Find("Robot");
        foreach (Transform child in root.transform)
        {
          HakoniwaRobotConfigInfo robot_cfg = hakoniwa_cfg.GetRobotConfig(child.name);
          if (robot_cfg != null)
          {
            this.activate_one(robot_cfg);
          }
        }
      }
    }


    //cicd
    //複数のプロセスを閉じれない？
    public void CloseSim()
    {
      if (isInitialized == true)
      {
        exProcess.CloseMainWindow();
        exProcess.Close();
      }
    }

    public void StartAthrill()
    {
      isInitialized = false;
      //UnityEngine.Debug.Log("Start Athrill");
      OnButtonClick();
    }
  }
}
