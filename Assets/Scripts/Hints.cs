using UnityEngine;
using UnityEngine.UI;

public class Hints : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject winPanel;

    public TravelBetweenWorlds travelBetweenWorlds;

    public GameObject pressTabToSelect, readyToTravel, currentlyShifting;
    int state = 100;
    int potentialState;




    void Update()
    {
        if (startPanel.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                startPanel.SetActive(false);
                travelBetweenWorlds.enabled = true;
            }
            return;
        }

        if (travelBetweenWorlds.mapControl.gameObject.activeInHierarchy)
        {
            potentialState = 4;
        }
        else
        {
            if (travelBetweenWorlds.isTransiting)
            {
                potentialState = 2;
            }
            else
            {
                if (travelBetweenWorlds.mapControl.currentMapIcon != travelBetweenWorlds.mapControl.closestIcon)
                {
                    potentialState = 1;
                }
                else potentialState = 0;
            }
        }

        if (potentialState != state)
        {
            state = potentialState;
            pressTabToSelect.SetActive(state == 0);
            readyToTravel.SetActive(state == 1);
            currentlyShifting.SetActive(state == 2);

        }


    }
}
