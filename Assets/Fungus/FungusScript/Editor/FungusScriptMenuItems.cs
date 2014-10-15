﻿using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Fungus
{

	public class FungusScriptMenuItems
	{
		[MenuItem("GameObject/Fungus/Fungus Script")]
		static void CreateFungusScript()
		{
			GameObject newFungusScriptGO = new GameObject();
			newFungusScriptGO.name = "FungusScript";
			FungusScript fungusScript = newFungusScriptGO.AddComponent<FungusScript>();
			Sequence sequence = Undo.AddComponent<Sequence>(newFungusScriptGO);
			sequence.nodeRect.x += 50;
			sequence.nodeRect.y += 50;
			sequence.nodeRect.width = 240;
			fungusScript.startSequence = sequence;
			fungusScript.selectedSequence = sequence;
			fungusScript.scrollPos = Vector2.zero;
			Undo.RegisterCreatedObjectUndo(newFungusScriptGO, "Create Fungus Script");
		}		
	}

}