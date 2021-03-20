using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace CICD
{

  public class PyStart : MonoBehaviour
  {
    public Process exProcess;
    private bool isInitialized = false;
    private string terminal = null;
    Hakoniwa.Core.HakoniwaConfig hakoniwa_cfg = null;
    [SerializeField] private string pythonPath = "/mnt/c/hakoniwa/ev3rt-athrill-v850e2m/sdk/workspace/line_trace_CICD";
    [SerializeField] private string pythonName = "test.py";
    public void Initialize()
    {
      GameObject hakoniwa = GameObject.Find("Hakoniwa");
      hakoniwa_cfg = hakoniwa.GetComponentInChildren<Hakoniwa.Core.HakoniwaConfig>();
      Hakoniwa.Core.HakoniwaConfigInfo cfg = hakoniwa_cfg.GetConfig();
      this.terminal = cfg.TerminalPath;
    }

    private void activate_one(Hakoniwa.Core.HakoniwaRobotConfigInfo robot_cfg)
    {
      string arg = null;
      arg = $"cd {pythonPath} && python3 {pythonName}";
      if (this.terminal.Contains("wsl"))
      {
        UnityEngine.Debug.Log(terminal);
        exProcess = Process.Start(this.terminal, arg);
      }
      /*else if (this.terminal.Contains("open"))
      {
        string exec_cmd = hakoniwa_cfg.GetCurrentPath() + "/athrill.command";
        StreamWriter writer = new StreamWriter(exec_cmd);
        writer.Write(arg);
        writer.Close();
        exProcess = Process.Start("/usr/bin/open", exec_cmd);
        UnityEngine.Debug.Log("exec= " + exec_cmd);
      }
      */
      UnityEngine.Debug.Log("PythonArg=" + arg);
      UnityEngine.Debug.Log(pythonPath);
      isInitialized = true;
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
          Hakoniwa.Core.HakoniwaRobotConfigInfo robot_cfg = hakoniwa_cfg.GetRobotConfig(child.name);
          if (robot_cfg != null)
          {
            this.activate_one(robot_cfg);
          }
        }
      }
    }

    public void CloseSim()
    {
      if (isInitialized == true)
      {
        exProcess.CloseMainWindow();
        exProcess.Close();
      }
    }
  }
}
