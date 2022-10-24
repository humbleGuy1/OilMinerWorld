using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public interface IBlockable
{
    public Blockable Blockable { get; }
    public Transform transform { get; }
}
