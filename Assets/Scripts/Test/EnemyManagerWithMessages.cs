using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerWithMessages : MonoBehaviour
{
    private List<GameObject> _enemies = new List<GameObject>();
    [SerializeField]private GameObject _enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        MessagingSystem.Instance.AttachListener(typeof(CreateEnemyMessage), HandleCreateEnemy);
    }

    private bool HandleCreateEnemy(Message msg)
    {
        CreateEnemyMessage castMsg = msg as CreateEnemyMessage;
        string[] names = { "11", "22", "33" };
        GameObject enemy = GameObject.Instantiate(_enemyPrefab, 5.0f * Random.insideUnitSphere, Quaternion.identity, this.gameObject.transform);
        string enemyName = names[Random.Range(0, names.Length)];
        enemy.gameObject.name = enemyName;
        _enemies.Add(enemy);
        MessagingSystem.Instance.QueueMessage(new EnemyCreatedMessage(enemy, enemyName));
        return true;
    }

	private void OnDestroy()
	{
		if (MessagingSystem.IsAlive)
		{
            MessagingSystem.Instance.DetachListener(typeof(CreateEnemyMessage), HandleCreateEnemy);
		}
	}
}
