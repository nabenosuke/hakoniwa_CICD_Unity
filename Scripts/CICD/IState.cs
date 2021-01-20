using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CICD
{
  public enum EventState
  {
    EVENT_NONE = 0,
    GOAL = 1,
    TIME_OVER = 2,
    HIT_WALL = 3,
  }
  public interface IState
  {
    EventState GetState();
  }
}
