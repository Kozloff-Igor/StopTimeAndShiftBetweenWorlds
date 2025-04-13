using UnityEngine;

public class MapIcon : MonoBehaviour
{
    public int WorldType;
    public Transform[] path;
    int currentPointId;
    public float speed;

    Transform currentTarget;


    void Start()
    {
        currentTarget = path[0];
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime * MapControl.mapTimeScale);
        if (Vector3.SqrMagnitude(transform.position - currentTarget.position) < 0.01f) 
        {
            currentPointId++;
            currentPointId %= path.Length;
            currentTarget = path[currentPointId];
        }
    }
}
