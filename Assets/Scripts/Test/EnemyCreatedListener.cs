using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreatedListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MessagingSystem.Instance.AttachListener(typeof(EnemyCreatedMessage), HandleEnemyCreated);
    }

    private bool HandleEnemyCreated(Message msg)
	{
        EnemyCreatedMessage castMsg = msg as EnemyCreatedMessage;
        Debug.Log(string.Format("新敌人{0}已创建！", castMsg.enemyName));
        return true;
    }

    private void OnDestroy()
    {
        if (MessagingSystem.IsAlive)
        {
            MessagingSystem.Instance.DetachListener(typeof(EnemyCreatedMessage), HandleEnemyCreated);
        }
    }
}
