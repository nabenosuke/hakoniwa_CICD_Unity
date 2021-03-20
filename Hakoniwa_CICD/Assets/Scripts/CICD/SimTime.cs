using Hakoniwa.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CICD
{

  public class SimTime : MonoBehaviour, IEvent
  {
    public double deadline = 120;

    private WorldController root;
    private StateManager stateMagager;
    private double time = 0;
    [SerializeField] private double startTime = 0;
    // Start is called before the first frame update
    void Start()
    {
      this.root = GameObject.Find("Hakoniwa").GetComponent<WorldController>();
      this.stateMagager = GameObject.Find("StateManager").GetComponent<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
      double t = this.root.GetSimTime() - startTime;
      if (t < 0.001)
      {
        t = 0.000;
      }
      else
      {
        long tl = (long)(t * 1000);
        t = (double)tl / 1000;
      }
      time = t;

      if (time > deadline)
      {
        if (stateMagager.eventState == EventState.EVENT_NONE)
        {
          stateMagager.TimeOver();
        }
      }
    }

    public void Initialize()
    {
      if (root == null)
      {
        startTime = 0;
        time = 0;
      }
      else
      {
        startTime = this.root.GetSimTime();
        time = 0;
      }
    }

    public double GetRapTime()
    {
      return time * 1000000;
    }

  }

}
