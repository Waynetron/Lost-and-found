using UnityEngine;

public class RotateToFollowMouse : MonoBehaviour {
    float strength = 1.35f;

    void Update() {
        // turns mouse position into a value between 0 and 1
        Vector2 mouseViewportPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        // adjust to be -0.5 to 0.5 instead of 0 to 1
        Vector2 normalized = mouseViewportPosition + new Vector2(-0.5f, -0.5f);

        transform.localEulerAngles = new Vector3(
            normalized.y * -strength,
            normalized.x * strength,
            0
        );
    }
}
