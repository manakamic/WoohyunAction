﻿using UnityEngine;

        OnMove();
        
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
    }
        move_ = Vector3.zero;
        move_ = new Vector3(dir.x, 0.0f, dir.y);