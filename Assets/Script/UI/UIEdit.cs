using UnityEngine;
using UnityEngine.UI;
using TouchScript;

public class UIEdit : MonoBehaviour {
    private const string AttackX = "atk_x";
    private const string AttackY = "atk_y";

    private const string CameraRotX = "cam_rot_x";
    private const string CameraRotY = "cam_rot_y";

    private const string CameraZoomX = "cam_zm_x";
    private const string CameraZoomY = "cam_zm_y";

    [SerializeField]
    private ScreenDrag screen_drag_ = null;

    [SerializeField]
    private Image full_screen_ = null;

    [SerializeField]
    private Text description_ = null;

    [SerializeField]
    private RectTransform attack_ = null;

    [SerializeField]
    private RectTransform camera_rotate_ = null;

    [SerializeField]
    private RectTransform camera_zoom_ = null;

    [SerializeField]
    private Image attack_button_ = null;

    [SerializeField]
    private Image camera_rotate_left_ = null;

    [SerializeField]
    private Image camera_rotate_right_ = null;

    [SerializeField]
    private Image camera_zoom_up_ = null;

    [SerializeField]
    private Image camera_zoom_down_ = null;

    private Vector2 move_ = Vector2.zero;

    private bool on_ui_edit_ = false;

    private bool on_attack_        = false;
    private bool on_camera_rotate_ = false;
    private bool on_camera_zoom_   = false;

    void LateUpdate() {
        if (!on_ui_edit_) {
            return;
        }

        if (on_attack_) {
            Vector2 now = attack_.anchoredPosition;

            attack_.anchoredPosition = now + move_;
            move_ = Vector2.zero;
        }
        else if (on_camera_rotate_) {
            Vector2 now = camera_rotate_.anchoredPosition;

            camera_rotate_.anchoredPosition = now + move_;
            move_ = Vector2.zero;
        }
        else if (on_camera_zoom_) {
            Vector2 now = camera_zoom_.anchoredPosition;

            camera_zoom_.anchoredPosition = now + move_;
            move_ = Vector2.zero;
        }
    }

    void OnEnable() {        if (TouchManager.Instance != null) {            TouchManager.Instance.TouchesMoved += TouchesMovedHandler;        }        LoadUIPosition();    }    void OnDisable() {        if (TouchManager.Instance != null) {            TouchManager.Instance.TouchesMoved -= TouchesMovedHandler;        }    }

    private void TouchesMovedHandler(object sender, TouchEventArgs e) {
        if (!on_ui_edit_) {
            return;
        }

        foreach (TouchPoint tp in e.Touches) {            move_ = tp.Position - tp.PreviousPosition;            break; // 最初の1つだけしか処理しない.        }
    }

    public void OnUIEdit() {
        on_ui_edit_ = !on_ui_edit_;

        SwitchEdit();
    }

    public void OnDownAttack() {
        on_attack_ = true;

        // 排他.
        on_camera_rotate_ = false;
        on_camera_zoom_   = false;
    }

    public void OnUpAttack() {
        on_attack_ = false;
    }

    public void OnDownCameraRotate() {
        on_camera_rotate_ = true;

        // 排他.
        on_attack_      = false;
        on_camera_zoom_ = false;
    }

    public void OnUpCameraRotate() {
        on_camera_rotate_ = false;
    }

    public void OnDownCameraZoom() {
        on_camera_zoom_ = true;

        // 排他.
        on_attack_        = false;
        on_camera_rotate_ = false;
    }

    public void OnUpCameraZoom() {
        on_camera_zoom_ = false;
    }

    private void SwitchEdit() {
        Time.timeScale = on_ui_edit_ ? 0.0f : 1.0f;

        screen_drag_.enable = !on_ui_edit_;

        full_screen_.enabled = on_ui_edit_;
        description_.enabled = on_ui_edit_;

        //attack_button_.raycastTarget       = !on_ui_edit_; // Dwon/Upを取れなくなるのでClickの方をOff.
        camera_rotate_left_.raycastTarget  = !on_ui_edit_;
        camera_rotate_right_.raycastTarget = !on_ui_edit_;
        camera_zoom_up_.raycastTarget      = !on_ui_edit_;
        camera_zoom_down_.raycastTarget    = !on_ui_edit_;

        if (!on_ui_edit_) {
            SaveUIPosition();
        }
    }

    private void SaveUIPosition() {
        Vector2 now = attack_.anchoredPosition;

        PlayerPrefs.SetFloat(AttackX, now.x);
        PlayerPrefs.SetFloat(AttackY, now.y);

        now = camera_rotate_.anchoredPosition;

        PlayerPrefs.SetFloat(CameraRotX, now.x);
        PlayerPrefs.SetFloat(CameraRotY, now.y);

        now = camera_zoom_.anchoredPosition;

        PlayerPrefs.SetFloat(CameraZoomX, now.x);
        PlayerPrefs.SetFloat(CameraZoomY, now.y);

        PlayerPrefs.Save();
    }

    private void LoadUIPosition() {
        Vector2 now = attack_.anchoredPosition;

        now.x = PlayerPrefs.GetFloat(AttackX, now.x);
        now.y = PlayerPrefs.GetFloat(AttackY, now.y);

        attack_.anchoredPosition = now;

        now = camera_rotate_.anchoredPosition;

        now.x = PlayerPrefs.GetFloat(CameraRotX, now.x);
        now.y = PlayerPrefs.GetFloat(CameraRotY, now.y);

        camera_rotate_.anchoredPosition = now;

        now = camera_zoom_.anchoredPosition;

        now.x = PlayerPrefs.GetFloat(CameraZoomX, now.x);
        now.y = PlayerPrefs.GetFloat(CameraZoomY, now.y);

        camera_zoom_.anchoredPosition = now;
    }
}
