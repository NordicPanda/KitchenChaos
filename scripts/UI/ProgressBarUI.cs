using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private CutterCounter cutterCounter;

    private void Start()
    {
        cutterCounter.OnProgressChanged += CutterCounter_OnProgressChanged;
        barImage.fillAmount = 1f;
        gameObject.SetActive(false);  // initially hidden
    }

    private void CutterCounter_OnProgressChanged(object sender, CutterCounter.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.hitPointsLeftNormalized;
        
        // only show progress bar when there's an object on the counter and the bar is not empty
        // hide the bar either when cutting is done or when it is not but the object is taken from the counter
        // HP == 0 means that cutting is done
        if (e.hitPointsLeftNormalized == 0)
        {            
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
