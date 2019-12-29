using UnityEngine;

namespace Fungus
{
    [CommandInfo("Collection",
                 "Insert",
                     "Add at a specific location in the collection")]
    [AddComponentMenu("")]
    public class CollectionCommandInsert : CollectionBaseVarAndIntCommand
    {
        protected override void OnEnterInner()
        {
            collection.Value.Insert(integer.Value, variableToUse);
        }
    }
}