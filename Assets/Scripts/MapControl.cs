using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapControl : MonoBehaviour
{
    public static float mapTimeScale = 1f;
    //public TravelBetweenWorlds travelBetweenWorlds;
    public MapIcon[] mapIcons;
    public MapIcon currentMapIcon;
    public Color neutralColor, targetWorldColor, currentWorldColor;

    public MapIcon closestIcon;
    int travelDistance;
    int fuelWasted;
    public TMP_Text travelDistanceText;
    public TMP_Text fuelWastedText,fuelWastedText2;


    void Start()
    {
        mapTimeScale = 1f;
    }

    public void PerformTravel()
    {
        fuelWasted += travelDistance;
        fuelWastedText.text = fuelWasted.ToString();
        fuelWastedText2.text = fuelWasted.ToString();
        currentMapIcon.GetComponent<Image>().color = neutralColor;
        currentMapIcon = closestIcon;
        currentMapIcon.GetComponent<Image>().color = currentWorldColor;
    }

    void Update()
    {
        MapIcon newClosest = ClosestMapIcon();
        if (newClosest != closestIcon)
        {
            if (closestIcon != currentMapIcon) closestIcon.GetComponent<Image>().color = neutralColor;
            closestIcon = newClosest;
            closestIcon.GetComponent<Image>().color = targetWorldColor;
        }
        travelDistanceText.text = travelDistance.ToString();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (mapTimeScale == 1f) {BTN_StopTime();} else {BTN_StartTime();}
        }

    }

    MapIcon ClosestMapIcon()
    {
        MapIcon closestSoFar = mapIcons[0];
        float distSoFar = 999999999f;
        for (int q = 1; q < mapIcons.Length; q++)
        {
            if (currentMapIcon != mapIcons[q] && currentMapIcon.WorldType != mapIcons[q].WorldType)
            {
                float dist = Vector3.SqrMagnitude(currentMapIcon.transform.position - mapIcons[q].transform.position);
                if (dist < distSoFar)
                {
                    distSoFar = dist;
                    closestSoFar = mapIcons[q];
                }
            }
        }
        travelDistance = (int)Mathf.Sqrt(distSoFar);
        return closestSoFar;
    }

    public void BTN_StopTime()
    {
        mapTimeScale = 0f;
    }

    public void BTN_StartTime()
    {
        mapTimeScale = 1f;
    }


}
