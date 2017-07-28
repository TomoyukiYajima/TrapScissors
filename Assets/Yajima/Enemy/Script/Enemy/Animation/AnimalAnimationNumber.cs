using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimalAnimatorNumber
{
    ANIMATOR_NULL =             0,  // 空のアニメーション
    ANIMATOR_IDEL_NUMBER =      1,  // 待機アニメーション
    ANIMATOR_DISCOVER_NUMBER =  2,  // 発見アニメーション
    ANIMATOR_CHASE_NUMBER =     3,  // 追跡アニメーション
    ANIMATOR_ATTACK_NUMBER =    4,  // 攻撃アニメーション
    ANIMATOR_LOST_NUMBER =      5,  // 見失いアニメーション
    ANIMATOR_TRAP_HIT_NUMBER =  6,  // 罠ヒットアニメーション
    ANIMATOR_WALL_HIT_NUMBER =  7,  // 壁ヒットアニメーション
    ANIMATOR_FAINT_NUMBER =     8,  // 怯みアニメーション
    ANIMATOR_DEAD_NUMBER =      9,  // 死亡アニメーション
    ANIMATOR_EAT_NUMBER =       10, // 食事アニメーション
    ANIMATOR_SLEEP_NUMBER =     11  // 睡眠アニメーション
}
