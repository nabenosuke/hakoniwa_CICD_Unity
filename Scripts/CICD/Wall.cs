using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CICD
{
  public class Wall : MonoBehaviour
  {
    private StateManager stateMagager;
    // Start is called before the first frame update
    void Start()
    {
      stateMagager = GameObject.Find("StateManager").GetComponent<StateManager>();
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
      if (collision.collider.isTrigger == false && collision.gameObject.transform.root.gameObject.name == "Robot")
      {
        if (stateMagager.eventState == EventState.EVENT_NONE)
        {
          //stateMagager = collision.gameObject.GetComponentInParent<StateManager>();
          stateMagager.HitWall();
        }
      }
    }
  }
}
