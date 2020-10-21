using System;
using System.Collections.Generic;
using UnityEngine;

public class NpcMgr :Singleton<NpcMgr>
{
    //thisid Npc
    public Dictionary<int, Npc> allNpc = new Dictionary<int, Npc>();

    internal static void OncreateSceneNpc(Cmd cmd)
    {
        if (!Net.CheckCmd(cmd, typeof(CreateSceneNpc))) { return; }
        CreateSceneNpc createNpc = cmd as CreateSceneNpc;
        //
        var npcDatabase = NpcTable.instance[createNpc.modelId];
        var npcObj = ResourcesManager.instance.GetInstance(NpcTable.instance[createNpc.modelId].modelPath);

        var npc = npcObj.AddComponent<Npc>();
        npc.Init(createNpc, npcDatabase);

        NpcMgr.instance.allNpc[npc.ThisId] = npc;
    }

    internal void Reset()
    {
        foreach (var role in NpcMgr.instance.allNpc)
        {
            ResourcesManager.instance.Release(role.Value.gameObject);
        }
        NpcMgr.instance.allNpc.Clear();
    }
}

