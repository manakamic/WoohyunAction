using UnityEngine;public class Player : MonoBehaviour {    private const string IdleWalk = "idle_walk";    private const string WalkRun  = "walk_run";    private const string RunIdle  = "run_idle";    private const string IdleRun  = "idle_run";    private const string RunWalk  = "run_walk";    private const string WalkIdle = "walk_idle";    private const float MovePowerMax = 0.15f;    private const float MoveCoefficient = 0.5f;    private const float AnimWalkCoefficient = 0.05f;    [SerializeField]    private Animator animator_ = null;    [SerializeField]    private CharacterController character_controller_ = null;    [SerializeField]    private ScreenDrag screen_drag_ = null;    private Transform transform_ = null;    private PlayerStateMachine state_machine_ = null;    private PlayerStateMachine.State anim_state_ = PlayerStateMachine.State.Idle;    private PlayerStateMachine.State schedule_anim_state_ = PlayerStateMachine.State.Unknown;    private bool on_anim_state_change_   = false;    private bool on_schedule_anim_state_ = false;    void Start() {        transform_ = gameObject.transform;        screen_drag_.on_world_dir_ = OnWorldDir;        screen_drag_.on_world_dir_end_ = OnWorldDirEnd;        state_machine_ = animator_.GetBehaviour<PlayerStateMachine>();

        if (state_machine_ != null) {
            state_machine_.on_state_enter_ = OnStateEnter;
        }
    }    void LateUpdate() {
        if (on_schedule_anim_state_) {
            if (!on_anim_state_change_) {
                on_schedule_anim_state_ = !SetAnimState(schedule_anim_state_);
            }
        }
    }    private void OnStateEnter(PlayerStateMachine.State state) {
        anim_state_ = state;
        on_anim_state_change_ = false;
    }    private void OnWorldDir(Vector2 dir, float power) {        if (power > MovePowerMax) {
            power = MovePowerMax;
        }        SetMoveAnim(power);        SetDir(dir);        SetMove(dir, power);    }    private void OnWorldDirEnd() {        if (on_anim_state_change_) {
            // ステート遷移を待ってIdleにする.
            SheduleAnimState(PlayerStateMachine.State.Idle);
        }        else {
            // Idleにする.
            SetAnimState(PlayerStateMachine.State.Idle);
        }    }    private void SheduleAnimState(PlayerStateMachine.State state) {
        schedule_anim_state_ = state;
        on_schedule_anim_state_ = true;
    }    private bool SetAnimState(PlayerStateMachine.State state) {
        if (on_anim_state_change_) {
            return false;
        }

        string trigger = null;

        switch (anim_state_) {
        case PlayerStateMachine.State.Idle:
            switch (state) {
            case PlayerStateMachine.State.Walk:
                trigger = IdleWalk;
                break;

            case PlayerStateMachine.State.Run:
                trigger = IdleRun;
                break;

            default:
                return false;
            }
            break;

        case PlayerStateMachine.State.Walk:
            switch (state) {
            case PlayerStateMachine.State.Idle:
                trigger = WalkIdle;
                break;

            case PlayerStateMachine.State.Run:
                trigger = WalkRun;
                break;

            default:
                return false;
            }
            break;

        case PlayerStateMachine.State.Run:
            switch (state) {
            case PlayerStateMachine.State.Idle:
                trigger = RunIdle;
                break;

            case PlayerStateMachine.State.Walk:
                trigger = RunWalk;
                break;

            default:
                return false;
            }
            break;

        default:
            return false;
        }

        animator_.SetTrigger(trigger);
        on_anim_state_change_ = true;

        return true;
    }    /// <summary>    /// 移動のパワーによってアニメーションをセットする.    /// </summary>    /// <param name="power">移動のパワー</param>    private void SetMoveAnim(float power) {        if (power < AnimWalkCoefficient) {            if (on_anim_state_change_) {
                SheduleAnimState(PlayerStateMachine.State.Walk);
            }            else {                SetAnimState(PlayerStateMachine.State.Walk);
            }        }        else {            if (on_anim_state_change_) {
                SheduleAnimState(PlayerStateMachine.State.Run);
            }            else {                SetAnimState(PlayerStateMachine.State.Run);
            }        }    }    private void SetDir(Vector2 dir) {        float angle_y = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;        transform_.rotation = Quaternion.AngleAxis(angle_y, Vector3.up);    }    private void SetMove(Vector2 dir, float power) {        Vector3 world = new Vector3(dir.x, 0.0f, dir.y);
        world *= power * MoveCoefficient;

        //　重力をy方向の速さに足していく.
        world.y += Physics.gravity.y * Time.deltaTime;        character_controller_.Move(world);    }}