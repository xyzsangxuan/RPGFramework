﻿using UnityEngine;
/// <summary>
/// 扩展原有类的一些函数，作用很大哦？！
/// </summary>
public static class UnityExtern
{
    public static T Find<T>(this GameObject parent,string path)//this GameObject 代表对这个gameObject类扩展了一个Find函数
    {
        return parent.transform.Find(path).GetComponent<T>();
    }
    //删除所有子物体节点
    public static void DestroyAllChildren(this GameObject parent)
    {
        for(int i = 0; i < parent.transform.childCount; ++i)
        {
            var child = parent.transform.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }
    }
}

