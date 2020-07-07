using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    [Header("Item Stack Dictionary")]
    public ItemStackEntry[] itemStackEntry;

    [System.Serializable]
    public struct ItemStackEntry
    {
        public string name;
        public GameObject itemStack;
    }


}
