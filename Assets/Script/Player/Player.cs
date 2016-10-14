using UnityEngine;public class Player : MonoBehaviour {    private const string Speed = "speed";    private const float MovePowerMax = 0.15f;    private const float MoveCoefficient = 20.0f;    private const float StopCoefficient = 2.0f;    private const float AnimWalkCoefficient = 0.08f;    private const float AnimWalkCoefficient_1_100 = AnimWalkCoefficient / 100.0f;    [SerializeField]    private Animator animator_ = null;    [SerializeField]    private CharacterController character_controller_ = null;    [SerializeField]    private ScreenDrag screen_drag_ = null;    [SerializeField]    private PlayerShot player_shot_ = null;    private Transform transform_ = null;    private Vector3 move_ = Vector3.zero;    private float motion_speed_ = 0.0f;    private bool motion_stop_ = false;    void Start() {        transform_ = gameObject.transform;        screen_drag_.on_world_dir_ = OnWorldDir;        screen_drag_.on_world_dir_end_ = OnWorldDirEnd;    }    void LateUpdate() {
        if (motion_stop_) {
            motion_speed_ -= StopCoefficient * Time.deltaTime;
            if (motion_speed_ < 0.0f) {
                motion_speed_ = 0.0f;
                motion_stop_ = false;
            }

            animator_.SetFloat(Speed, motion_speed_);
        }

        OnMove();    }    public void OnAttack() {
        player_shot_.OnShot();
    }    private void OnMove() {
        if (transform_.position.y < -0.5f/*仮*/) {
            // 動作させない.
            move_ = Vector3.zero;

            if (transform_.position.y < -20.0f/*仮*/) {
                // ワープ
                transform_.position = new Vector3(0.0f, 20.0f, 0.0f);
            }
        }

        //　重力を足す.
        move_ += Physics.gravity;

        character_controller_.Move(move_ * Time.deltaTime);
    }    private void OnWorldDir(Vector2 dir, float power) {        if (power > MovePowerMax) {            power = MovePowerMax;        }        SetMoveAnim(power);        SetDir(dir);        SetMove(dir, power);    }    private void OnWorldDirEnd() {
        move_ = Vector3.zero;        motion_stop_ = true;    }    /// <summary>    /// 移動のパワーによってアニメーションをセットする.    /// </summary>    /// <param name="power">移動のパワー</param>    private void SetMoveAnim(float power) {        if (motion_stop_) {
            motion_stop_ = false;
        }        if (power < AnimWalkCoefficient) {
            // 0.49 ~ 0.5 に変換.
            motion_speed_ = 0.49f + (power / AnimWalkCoefficient) * AnimWalkCoefficient_1_100;        }        else {
            // 0.5 ~ 1 に変換.
            motion_speed_ = 0.5f + ((power - AnimWalkCoefficient) / (MovePowerMax - AnimWalkCoefficient) * 0.5f);        }        animator_.SetFloat(Speed, motion_speed_);    }    private void SetDir(Vector2 dir) {        float angle_y = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;        transform_.rotation = Quaternion.AngleAxis(angle_y, Vector3.up);    }    private void SetMove(Vector2 dir, float power) {
        move_ = new Vector3(dir.x, 0.0f, dir.y);        move_ *= power * MoveCoefficient;    }}