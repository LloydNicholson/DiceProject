using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceHandler : MonoBehaviour
{
    public List<GameObject> faces;

    public int? UpFace { get; private set; }

    public int CheckFaceUp()
    {
        var upFaceObj = GetFaceUpObject();
        if (int.TryParse(upFaceObj.name, out var res))
        {
            UpFace = res;
            return res;
        }
        else
        {
            throw new Exception("Up face cannot be determined");
        }
    }

    private GameObject GetFaceUpObject()
    {
        var yPos = float.MinValue;
        GameObject activeGameObject = null;

        foreach (var f in faces)
        {
            if (f.transform.position.y > yPos)
            {
                yPos = f.transform.position.y;
                activeGameObject = f;
            }
        }

        return activeGameObject;
    }
}
