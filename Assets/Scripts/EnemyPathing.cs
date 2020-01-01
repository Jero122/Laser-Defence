using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig = null;
    List<Transform> waypointsList = null;
    int waypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        waypointsList = waveConfig.GetWaypoints();
        transform.position = waypointsList[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void setWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void Move()
    {
        if (waypointIndex <= waypointsList.Count - 1)
        {
            var targetPosition = waypointsList[waypointIndex].transform.position;
            var moveMomentThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveMomentThisFrame);
            if (transform.position == targetPosition)
            {
                waypointIndex += 1;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
