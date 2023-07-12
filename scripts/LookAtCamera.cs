using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        // set object's forward vector so it's opposite to camera view direction
        transform.forward = -Camera.main.transform.forward;

        // enabling this line instead of the one above makes objects rotate to always face the camera from any angle
        // transform.LookAt(2 * transform.position - Camera.main.transform.position);
    }
}
