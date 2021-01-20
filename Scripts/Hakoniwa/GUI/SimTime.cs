﻿using Hakoniwa.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hakoniwa.GUI
{
  public class SimTime : MonoBehaviour
  {
    WorldController root;
    GameObject myObject;
    Text simTimeText;
    // Start is called before the first frame update
    void Start()
    {
      this.root = GameObject.Find("Hakoniwa").GetComponent<WorldController>();
      this.myObject = GameObject.Find("GUI/Canvas/SimTime/Value");
      simTimeText = myObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
      double t = this.root.GetSimTime();
      if (t < 0.001)
      {
        t = 0.000;
        simTimeText.text = t.ToString();
      }
      else
      {
        long tl = (long)(t * 1000);
        t = (double)tl / 1000;
        simTimeText.text = t.ToString();
      }
    }
  }
}
