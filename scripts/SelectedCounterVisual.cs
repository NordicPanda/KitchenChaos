using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter counter;                    // counter that has THIS selection visual
    [SerializeField] private GameObject[] selectedCounterModelArray; // selected counter visual (white) array

    private void Start()
    {        
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs args)
    {
        foreach (GameObject counterModelPart in selectedCounterModelArray)
        {
            if (args._selectedCounter == counter)
            {
                counterModelPart.SetActive(true);   // show
            }
            else
            {
                counterModelPart.SetActive(false);  // hide
            } 
        }
    }
}
