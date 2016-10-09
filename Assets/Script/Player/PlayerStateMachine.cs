using UnityEngine;
using UnityEngine.Events;

public class PlayerStateMachine : StateMachineBehaviour {
    private const int InvalidHash = -1;

    public enum State {
        Idle = 0,
        Walk,
        Run,
        Unknown
    }

    public UnityAction<State> on_state_enter_ = null;

    private int idle_hash_ = InvalidHash;
    private int walk_hash_ = InvalidHash;
    private int run_hash_  = InvalidHash;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (on_state_enter_ != null) {
            if (idle_hash_ == InvalidHash) {
                idle_hash_ = Animator.StringToHash("Base Layer.idle");
            }
            if (walk_hash_ == InvalidHash) {
                walk_hash_ = Animator.StringToHash("Base Layer.walk");
            }
            if (run_hash_ == InvalidHash) {
                run_hash_ = Animator.StringToHash("Base Layer.run");
            }

            int hash = stateInfo.fullPathHash;
            State state = State.Unknown;

            if (hash == idle_hash_) {
                state = State.Idle;
            }
            else if (hash == walk_hash_) {
                state = State.Walk;
            }
            else if (hash == run_hash_) {
                state = State.Run;
            }

            on_state_enter_(state);
        }
    }
}
