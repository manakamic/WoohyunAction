using UnityEngine;
using System.Collections.Generic;

public class PlayerShot : MonoBehaviour {
    private const float OffsetY = 1.0f;
    private const float OffsetZ = 0.5f;

    private const float Speed = 10.0f;
    private const float Life = 2.0f;

    private class Shot {
        public GameObject go_ = null;
        public float life_ = 0.0f;
    }

    [SerializeField]
    private GameObject shot_ = null;

    [SerializeField]
    private Transform player_transform_ = null;

    private List<Shot> shot_list_ = new List<Shot>();

    private Vector3 offset_ = new Vector3(0.0f, OffsetY, OffsetZ);

    void OnDestory() {
        foreach (Shot shot in shot_list_) {
            Destroy(shot.go_);
            shot.go_ = null;
        }
        shot_list_.Clear();
    }

    void LateUpdate() {
        int cnt = shot_list_.Count;

        if (cnt == 0) {
            return;
        }

        float delta = Speed * Time.deltaTime;
        Transform t = null;

        for (int i = 0; i < cnt; ++i) {
            Shot shot = shot_list_[i];

            t = shot.go_.transform;

            t.position += t.forward * delta;

            shot.life_ += Time.deltaTime;

            if (shot.life_ > Life) {
                shot.go_.SetActive(false);
            }
        }
    }

    public void OnShot() {
        // 再利用.
        foreach (Shot shot in shot_list_) {
            if (!shot.go_.activeSelf) {
                InitShotTransform(shot.go_.transform);
                shot.life_ = 0.0f;
                shot.go_.SetActive(true);

                return;
            }
        }

        // 新規追加.
        AddShot();
    }

    private void AddShot() {
        Shot shot = new Shot();

        shot.go_ = CreateShot();
        shot_list_.Add(shot);
    }

    private GameObject CreateShot() {
        GameObject clone = Instantiate<GameObject>(shot_);

        InitShotTransform(clone.transform);
        clone.SetActive(true);

        return clone;
    }

    private void InitShotTransform(Transform t) {
        Vector3 pos = player_transform_.position;

        Vector3 local = player_transform_.rotation * offset_;

        t.position = pos + local;
        t.rotation = player_transform_.rotation;
    }
}
