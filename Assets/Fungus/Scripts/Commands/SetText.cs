// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
#if UNITY_LOCALIZATION
using UnityEngine.Localization;
#endif
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace Fungus
{
    /// <summary>
    /// Sets the text property on a UI Text object and/or an Input Field object.
    /// </summary>
    [CommandInfo("UI", 
                 "Set Text", 
                 "Sets the text property on a UI Text object and/or an Input Field object.")]
    [AddComponentMenu("")]
    public class SetText : Command, ILocalizable 
    {
        [Tooltip("Text object to set text on. Can be a UI Text, Text Field or Text Mesh object.")]
        [SerializeField] protected GameObject targetTextObject;
        
#if UNITY_LOCALIZATION
        [Tooltip("String value to assign to the text object. Ignored if textString is not empty.")]
#else
        [Tooltip("String value to assign to the text object")]
#endif
        [FormerlySerializedAs("stringData")]
        [SerializeField] protected StringDataMulti text;
        
#if UNITY_LOCALIZATION
        [Tooltip("Localization entry for text")]
        [SerializeField] protected LocalizedString localizedText;
#endif

        [Tooltip("Notes about this story text for other authors, localization, etc.")]
        [SerializeField] protected string description;

        #region Public members

        public override void OnEnter()
        {
            var flowchart = GetFlowchart();
#if UNITY_LOCALIZATION
            string newText = flowchart.SubstituteVariables(localizedText.IsEmpty ? text.Value : localizedText.GetLocalizedString());
#else
            string newText = flowchart.SubstituteVariables(text.Value);
#endif
            
            if (targetTextObject == null)
            {
                Continue();
                return;
            }

            TextAdapter textAdapter = new TextAdapter();
            textAdapter.InitFromGameObject(targetTextObject);

            if (textAdapter.HasTextObject())
            {
                textAdapter.Text = newText;
            }

            Continue();
        }
        
        public override string GetSummary()
        {
            if (targetTextObject != null)
            {
                return targetTextObject.name + " : " + text.Value;
            }
            
            return "Error: No text object selected";
        }
        
        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        public override bool HasReference(Variable variable)
        {
            return text.stringRef == variable || base.HasReference(variable);
        }

        #endregion


        #region Editor caches
#if UNITY_EDITOR
        protected override void RefreshVariableCache()
        {
            base.RefreshVariableCache();

            var f = GetFlowchart();

            f.DetermineSubstituteVariables(text, referencedVariables);
        }
#endif
        #endregion Editor caches

        #region ILocalizable implementation

        public virtual string GetStandardText()
        {
            return text;
        }

        public virtual void SetStandardText(string standardText)
        {
            text.Value = standardText;
        }

        public virtual string GetDescription()
        {
            return description;
        }
        
        public virtual string GetStringId()
        {
            // String id for Set Text commands is SETTEXT.<Localization Id>.<Command id>
            return "SETTEXT." + GetFlowchartLocalizationId() + "." + itemId;
        }
        
#if UNITY_LOCALIZATION

        public LocalizedString GetLocalizedStringComponent()
        {
            return localizedText;
        }
        
#endif

        #endregion

        #region Backwards compatibility

        // Backwards compatibility with Fungus v2.1.2
        [HideInInspector]
        [FormerlySerializedAs("textObject")]
        public Text _textObjectObsolete;
        protected virtual void OnEnable()
        {
            if (_textObjectObsolete != null)
            {
                targetTextObject = _textObjectObsolete.gameObject;
            }
        }

        #endregion
    }    
}