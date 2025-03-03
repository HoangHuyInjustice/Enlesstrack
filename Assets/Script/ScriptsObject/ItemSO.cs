using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 0)]
public class ItemSO : ScriptableObject
{
    public float BonusSpeed;
    public int BonusHP;
    public int BonusDef;
}
