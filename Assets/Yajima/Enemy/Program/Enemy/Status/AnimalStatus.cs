using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 状態列挙クラス 
[System.Flags]
public enum AnimalState
{
    Idel =              1 << 0,     // 待機状態
    Search =            1 << 1,     // 捜索状態
    Discover =          1 << 2,     // 発見状態
    DiscoverAction =    1 << 3,     // 発見行動状態
    Attack =            1 << 4,     // 攻撃状態
    Faint =             1 << 5,     // 気絶状態
    Sleep =             1 << 6,     // 睡眠状態
    TrapHit =           1 << 7,     // トラバサミに挟まれている状態
    Meat =              1 << 8,     // お肉状態
    DeadIdel =          1 << 9,     // 死亡待機状態
    Runaway =           1 << 10,    // 逃亡状態
}

// 発見状態列挙クラス
[System.Flags]
public enum AnimalState_DiscoverState
{
    Discover_None =         1 << 0, // なにも見つけていない状態
    Discover_Player =       1 << 1, // プレイヤーを発見状態
    Discover_Animal =       1 << 2, // 動物発見状態
    Discover_Food =         1 << 3, // えさ発見状態
    Discover_Trap =         1 << 4, // トラバサミ発見状態
    Discover_Lost =         1 << 5, // 見失う状態
    Discover_Lost_Stop =    1 << 6  // 見失い停止状態
}

// えさ発見状態列挙クラス
[System.Flags]
public enum AnimalState_DiscoverFoodState
{
    DiscoverFood_Move =         1 << 0, // 発見移動
    DiscoverFood_AnimalMove =   1 << 1, // 動物発見移動
    DiscoverFood_Eat =          1 << 2, // えさ食べ状態
    DiscoverFood_Lift =         1 << 3, // 持ち上げ状態
    DiscoverFood_TakeAway =     1 << 4, // 持ち帰り状態
}

// トラバサミヒット状態列挙クラス
[System.Flags]
public enum AnimalState_TrapHitState
{
    TrapHit_Change =    1 << 0, // トラップ化状態
    TrapHit_TakeIn =    1 << 1, // トラバサミに飲み込まれた状態
    TrapHit_Runaway =   1 << 2  // トラバサミ逃げ状態
}
