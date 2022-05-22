// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using System;
using System.Collections.Generic;
using UnityEngine;

public static class CameraUtilityHelper
{
    public static List<(Action, string, string)> OrthoActionLists = new List<(Action, string, string)>();
    public static LTDescr OrthoRotateInstance;
    public static Vector3 defCamPos;
}

