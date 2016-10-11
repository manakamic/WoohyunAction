using UnityEngine;public class PlayerCamera : MonoBehaviour {    private const float CameraY = 1.5f;    private const float CameraZ = 2.5f;    private const float CameraMinY =  1.0f;    private const float CameraMaxY = 10.0f;    [SerializeField]    private Camera camera_ = null;    [SerializeField]    private Transform player_ = null;    private Transform camera_transform_ = null;    private Vector3 distance_ = Vector3.zero;    private bool on_rotate_left_  = false;    private bool on_rotate_right_ = false;    private bool on_zoom_up_   = false;    private bool on_zoom_down_ = false;    void Start() {        camera_transform_ = camera_.transform;        distance_ = new Vector3(0.0f, CameraY, CameraZ);    }    void LateUpdate() {        float v = Input.GetAxis("Vertical");        if (on_zoom_down_) {
            v -= 0.1f;
        }        if (on_zoom_up_) {
            v += 0.1f;
        }        if (v != 0.0f) {            Vector3 zoom = distance_ * (1.0f + v);            if (zoom.y >= CameraMinY && zoom.y <= CameraMaxY) {                distance_ = zoom;            }        }        float h = Input.GetAxis("Horizontal");        if (on_rotate_left_) {
            h -= 1.0f;
        }        if (on_rotate_right_) {
            h += 1.0f;
        }        if (h != 0.0f) {            distance_ = Quaternion.AngleAxis(h * -3.0f, Vector3.up) * distance_;        }        SetPotion();    }    private void SetPotion() {        camera_transform_.position = player_.position + distance_;        camera_transform_.LookAt(player_);    }    public void OnDownCameraRotateLeft() {
        on_rotate_left_ = true;
        on_rotate_right_ = false; // Cancel.
    }    public void OnUpCameraRotateLeft() {
        on_rotate_left_ = false;
    }    public void OnExitCameraRotateLeft() {
        on_rotate_left_ = false;
    }    public void OnDownCameraRotateRight() {
        on_rotate_right_ = true;
        on_rotate_left_ = false; // Cancel.
    }    public void OnUpCameraRotateRight() {
        on_rotate_right_ = false;
    }    public void OnExitCameraRotateRight() {
        on_rotate_right_ = false;
    }    public void OnDownCameraZoomUp() {
        on_zoom_up_ = true;
        on_zoom_down_ = false; // Cancel.
    }    public void OnUpCameraZoomUp() {
        on_zoom_up_ = false;
    }    public void OnExitCameraZoomUp() {
        on_zoom_up_ = false;
    }    public void OnDownCameraZoomDown() {
        on_zoom_down_ = true;
        on_zoom_up_ = false; // Cancel.
    }    public void OnUpCameraZoomDown() {
        on_zoom_down_ = false;
    }    public void OnExitCameraZoomDown() {
        on_zoom_down_ = false;
    }}