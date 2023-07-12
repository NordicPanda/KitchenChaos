using UnityEngine;

public class CutterCounterVisual : MonoBehaviour
{
    private const string CUT = "Cut";
    private Animator animator;
    [SerializeField] private CutterCounter cutterCounter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cutterCounter.OnCut += CutterCounter_OnCut;
    }

    private void CutterCounter_OnCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
}
