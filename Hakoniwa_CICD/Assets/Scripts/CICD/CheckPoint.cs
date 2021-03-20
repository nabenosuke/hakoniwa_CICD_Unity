using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CICD
{
  public class CheckPoint : MonoBehaviour, IEvent
  {
    //それぞれのチェックポイントの通過状況を管理
    public bool isCheck = false;

    private CheckPointManager checkPointManager;
    // Start is called before the first frame update
    void Start()
    {
      checkPointManager = GameObject.Find("CheckPoints").GetComponent<CheckPointManager>();
      if (checkPointManager == null)
      {
        Debug.Log("CheckPointsが見つかりません");
      }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize()
    {
      isCheck = false;
    }
    void OnTriggerEnter(Collider collider)
    {
      if (!collider.isTrigger && collider.gameObject.transform.root.gameObject.name == "Robot")
      {
        isCheck = true;
        checkPointManager.UpdateCheckPoint();
        isCheck = false;
      }
    }
  }
}
