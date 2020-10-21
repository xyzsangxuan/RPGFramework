
using UnityEngine;

public class JumpMapPoint:MonoBehaviour
{
    //目标地图
    public int jumpToMapID = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != GameSetting.MainRoleLayer)
        {
            return;
        }
        //Debug.Log("MainRole Jump!");
        other.gameObject.GetComponent<MainRole>().OnJumpTo(jumpToMapID);
        
    }
}
