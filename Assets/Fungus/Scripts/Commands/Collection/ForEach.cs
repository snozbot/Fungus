﻿// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Loop over each element in the given collection.
    /// </summary>
    [CommandInfo("Collection",
                 "For Each",
                 "Loop over each element in the given collection.")]
    [AddComponentMenu("")]
    public class ForEach : Condition, ICollectionCompatible
    {
        [SerializeField]
        protected CollectionData collection;

        [SerializeField]
        [VariableProperty(compatibleVariableName = "collection")]
        protected Variable item;

        private int curIndex;

        #region Public members

        public override bool IsLooping { get { return true; } }

        protected override void PreEvaluate()
        {
            //if we came from the end then we are already looping, if not this is first loop so prep
            if (ParentBlock.PreviousActiveCommandIndex != endCommand.CommandIndex)
            {
                curIndex = -1;
            }
        }

        protected override bool EvaluateCondition()
        {
            var col = collection.Value;
            curIndex++;
            if(curIndex < col.Count())
            {
                col.Get(curIndex, ref item);
                return true;
            }

            return false;
        }

        protected override void OnFalse()
        {
            MoveToEnd();
        }

        protected override bool HasNeededProperties()
        {
            return collection.Value != null && item != null;
        }

        public override bool HasReference(Variable variable)
        {
            return collection. collectionRef == variable || item == variable ||
                base.HasReference(variable);
        }

        bool ICollectionCompatible.IsCompatible(Variable variable, string compatibleWith)
        {
            if (compatibleWith == "collection")
                return collection.Value == null ? false : collection.Value.IsCompatible(variable);
            else
                return true;
        }

        #endregion
    }
}