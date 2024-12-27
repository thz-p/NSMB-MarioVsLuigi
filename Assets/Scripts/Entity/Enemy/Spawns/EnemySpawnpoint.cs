using UnityEngine;
using Photon.Pun;

public class EnemySpawnpoint : MonoBehaviour
{

    public string prefab;
    public GameObject currentEntity;

    public virtual bool AttemptSpawning()
    {
        // 如果 currentEntity 已经存在（不为空），表示已经有一个实体被生成，无法再次生成。
        // 返回 false 表示无法生成新的实体。
        if (currentEntity)
            return false;

        // 使用 Physics2D.OverlapCircleAll 检查在当前位置半径为 1.5f 的圆形范围内是否有任何物体。
        // 这个函数会返回所有与该圆形范围重叠的碰撞体列表。
        foreach (var hit in Physics2D.OverlapCircleAll(transform.position, 1.5f))
        {
            // 如果碰撞到的物体是标签为 "Player" 的游戏对象，说明当前位置不能生成新的实体。
            // 因为玩家不能在该位置生成。
            if (hit.gameObject.CompareTag("Player"))
                return false; // 返回 false，表示无法在该位置生成实体。
        }

        // 如果没有碰到玩家，可以在当前位置生成一个新的实体。
        // 使用 PhotonNetwork.InstantiateRoomObject 来在网络中生成实体。
        // 生成的实体会使用指定的 prefab（预制体）、当前位置和旋转角度。
        currentEntity = PhotonNetwork.InstantiateRoomObject(prefab, transform.position, transform.rotation);

        // 返回 true，表示成功生成实体。
        return true;
    }

    public void OnDrawGizmos()
    {
        string icon = prefab.Split("/")[^1];
        float offset = prefab switch
        {
            "Prefabs/Enemy/BlueKoopa" => 0.15f,
            "Prefabs/Enemy/RedKoopa" => 0.15f,
            "Prefabs/Enemy/Koopa" => 0.15f,
            "Prefabs/Enemy/Bobomb" => 0.22f,
            "Prefabs/Enemy/Goomba" => 0.22f,
            "Prefabs/Enemy/Spiny" => -0.03125f,
            _ => 0,
        };
        Gizmos.DrawIcon(transform.position + offset * Vector3.up, icon, true, new Color(1, 1, 1, 0.5f));
    }
}
