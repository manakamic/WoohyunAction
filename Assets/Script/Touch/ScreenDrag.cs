#define SWIPE

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Gesture;

public class ScreenDrag : MonoBehaviour {
    private const float LineNearZ = 1.0f;

    public UnityAction<Vector2, float> on_world_dir_ = null;
    public UnityAction on_world_dir_end_ = null;

    [SerializeField]
    private Camera camera_ = null;

    [SerializeField]
    private LineRenderer line_renderer_ = null;

    private Vector2 start_ = Vector2.zero;
    private Vector2 now_   = Vector2.zero;

    private bool on_drag_ = false;

    private bool enable_ = false;

    public bool enable {
        set { enable_ = value; }
    }

#if SWIPE
    private GestureSwipe gesture_swipe_;
#else
    private GesturePhase gesture_phase_;
#endif

    private GestureManager gesture_manager_;

    void OnDestory() {
        on_world_dir_ = null;
        on_world_dir_end_ = null;

#if SWIPE
#else
        if (gesture_phase_ != null) {
            gesture_phase_.Destroy();
            gesture_phase_ = null;
        }
#endif

        if (gesture_manager_ != null) {
            gesture_manager_.Destroy();
            gesture_manager_ = null;
        }
    }

    void Start() {
        gesture_manager_ = GestureManager.instance;

#if SWIPE
        gesture_swipe_ = new GestureSwipe(SwipeHandler);
#else
        gesture_phase_ = new GesturePhase(TouchesBeganHandler,
                                          TouchesMovedHandler,
                                          null,
                                          TouchesEndedHandler,
                                          TouchesCancelledHandler);
#endif

        enable_ = true;
    }

    void Update() {
        if (gesture_manager_ != null) {
            gesture_manager_.Update();
        }
    }

    void LateUpdate() {
        if (!enable_) {
            return;
        }

        if (line_renderer_.enabled && on_world_dir_ != null) {
            OnWorldDir();
        }
    }

#if SWIPE
    private void SwipeHandler() {
        if (!enable_) {
            return;
        }

        if (!gesture_swipe_.end) {
            if (!on_drag_) {
                on_drag_ = true;
                start_ = gesture_swipe_.startPos;
            }
            else {
                now_ = gesture_swipe_.position;
                line_renderer_.enabled = true;
            }
        }
        else {
            EndCancel();
        }
    }
#else
    private void TouchesBeganHandler() {
        if (!enable_ || on_drag_) {
            return;
        }

        on_drag_ = true;

        start_ = gesture_phase_.startPos;
    }

    private void TouchesMovedHandler() {
        now_ = gesture_phase_.position;
        line_renderer_.enabled = true;
    }

    private void TouchesEndedHandler() {
        EndCancel();
    }

    private void TouchesCancelledHandler() {
        EndCancel();
    }
#endif

    private void EndCancel() {
        on_drag_ = false;

        start_ = Vector2.zero;
        now_   = Vector2.zero;

        line_renderer_.enabled = false;

        if (on_world_dir_end_ != null) {
            on_world_dir_end_();
        }
    }

    private void OnWorldDir() {
        if (start_ == now_) {
            return;
        }

        Vector3 p0 = camera_.ScreenToWorldPoint(new Vector3(start_.x, start_.y, LineNearZ));
        Vector3 p1 = camera_.ScreenToWorldPoint(new Vector3(now_.x, now_.y, LineNearZ));

        line_renderer_.SetPosition(0, p0);
        line_renderer_.SetPosition(1, p1);

        if (on_world_dir_ == null) {
            return;
        }

        Transform t = camera_.transform;
        Ray ray0 = new Ray(p0, t.forward);
        Ray ray1 = new Ray(p1, t.forward);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float enter;

        if (plane.Raycast(ray0, out enter)) {
            Vector3 w0 = ray0.GetPoint(enter);

            if (plane.Raycast(ray1, out enter)) {
                Vector3 w1 = ray1.GetPoint(enter);
                Vector3 dir = w1 - w0;

                dir.y = 0.0f;
                dir.Normalize();

                Vector2 screen_size = new Vector2((float)Screen.width, (float)Screen.height);
                Vector2 line_size = now_ - start_;
                float power = line_size.magnitude / screen_size.magnitude;

                on_world_dir_(new Vector2(dir.x, dir.z), power);
            }
        }
    }
}
