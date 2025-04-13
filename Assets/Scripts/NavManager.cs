using UnityEngine;

public class NavManager : MonoBehaviour
{
    public static NavManager Instance;
    public Transform player;

    private void Awake()
    {
        Instance = this;
    }


    public Vector3 RandomPointInsideBounds()
    {
        return new Vector3(Random.Range(10, 110), 10, Random.Range(10, 110));
    }

    public Vector3 PlayerPosition()
    {
        return player.position;
    }


}
