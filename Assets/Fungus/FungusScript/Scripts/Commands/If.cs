using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fungus
{
	public enum CompareOperator
	{
		Equals, 				// ==
		NotEquals, 				// !=
		LessThan,				// <
		GreaterThan,			// >
		LessThanOrEquals,		// <=
		GreaterThanOrEquals		// >=
	}

	[CommandInfo("Scripting", 
	             "If", 
	             "If the test expression is true, execute the following block of commands.")]
	[AddComponentMenu("")]
	public class If : Command
	{

		[Tooltip("Variable to use in expression")]
		[VariableProperty(typeof(BooleanVariable),
		                  typeof(IntegerVariable), 
		                  typeof(FloatVariable), 
		                  typeof(StringVariable))]
		public Variable variable;

		[Tooltip("The type of comparison to be performed")]
		public CompareOperator compareOperator;

		[Tooltip("Boolean value to compare against")]
		public BooleanData booleanData;

		[Tooltip("Integer value to compare against")]
		public IntegerData integerData;

		[Tooltip("Float value to compare against")]
		public FloatData floatData;

		[Tooltip("String value to compare against")]
		public StringData stringData;
		
		public override void OnEnter()
		{
			if (parentSequence == null)
			{
				return;
			}

			bool condition = false;

			if (variable == null)
			{
				Continue();
				return;
			}

			if (variable.GetType() == typeof(BooleanVariable))
			{
				bool lhs = (variable as BooleanVariable).value;
				bool rhs = booleanData.Value;

				switch (compareOperator)
				{
				case CompareOperator.Equals:
					condition = lhs == rhs;
					break;
				case CompareOperator.NotEquals:
				default:
					condition = lhs != rhs;
					break;
				}
			}
			else if (variable.GetType() == typeof(IntegerVariable))
			{
				int lhs = (variable as IntegerVariable).value;
				int rhs = integerData.Value;

				switch (compareOperator)
				{
				case CompareOperator.Equals:
					condition = lhs == rhs;
					break;
				case CompareOperator.NotEquals:
					condition = lhs != rhs;
					break;
				case CompareOperator.LessThan:
					condition = lhs < rhs;
					break;
				case CompareOperator.GreaterThan:
					condition = lhs > rhs;
					break;
				case CompareOperator.LessThanOrEquals:
					condition = lhs <= rhs;
					break;
				case CompareOperator.GreaterThanOrEquals:
					condition = lhs >= rhs;
					break;
				}
			}
			else if (variable.GetType() == typeof(FloatVariable))
			{
				float lhs = (variable as FloatVariable).value;
				float rhs = floatData.Value;

				switch (compareOperator)
				{
				case CompareOperator.Equals:
					condition = lhs == rhs;
					break;
				case CompareOperator.NotEquals:
					condition = lhs != rhs;
					break;
				case CompareOperator.LessThan:
					condition = lhs < rhs;
					break;
				case CompareOperator.GreaterThan:
					condition = lhs > rhs;
					break;
				case CompareOperator.LessThanOrEquals:
					condition = lhs <= rhs;
					break;
				case CompareOperator.GreaterThanOrEquals:
					condition = lhs >= rhs;
					break;
				}
			}
			else if (variable.GetType() == typeof(StringVariable))
			{
				string lhs = (variable as StringVariable).value;
				string rhs = stringData.Value;

				switch (compareOperator)
				{
				case CompareOperator.Equals:
					condition = lhs == rhs;
					break;
				case CompareOperator.NotEquals:
				default:
					condition = lhs != rhs;
					break;
				}
			}

			if (condition)
			{
				Continue();
			}
			else
			{
				// Find the next Else or EndIf command at the same indent level as this If command
				for (int i = commandIndex + 1; i < parentSequence.commandList.Count; ++i)
				{
					Command nextCommand = parentSequence.commandList[i];

					// Find next command at same indent level as this If command
					// Skip disabled commands & comments
					if (!nextCommand.enabled || 
					    nextCommand.GetType() == typeof(Comment) ||
					    nextCommand.indentLevel != indentLevel)
					{
						continue;
					}

					System.Type type = nextCommand.GetType();
					if (type == typeof(Else) || 
					    type == typeof(EndIf) || // Legacy support for old EndIf command
					    type == typeof(End))
					{
						if (i >= parentSequence.commandList.Count - 1)
						{
							// Last command in Sequence, so stop
							Stop();
						}
						else
						{
							// Execute command immediately after the Else or End command
							Continue(nextCommand);
							return;
						}
					}
				}

				// No matching End command found, so just stop the sequence
				Stop();
			}
		}

		public override string GetSummary()
		{
			if (variable == null)
			{
				return "Error: No variable selected";
			}

			string summary = variable.key;
			switch (compareOperator)
			{
			case CompareOperator.Equals:
				summary += " == ";
				break;
			case CompareOperator.NotEquals:
				summary += " != ";
				break;
			case CompareOperator.LessThan:
				summary += " < ";
				break;
			case CompareOperator.GreaterThan:
				summary += " > ";
				break;
			case CompareOperator.LessThanOrEquals:
				summary += " <= ";
				break;
			case CompareOperator.GreaterThanOrEquals:
				summary += " >= ";
				break;
			}

			if (variable.GetType() == typeof(BooleanVariable))
			{
				summary += booleanData.GetDescription();
			}
			else if (variable.GetType() == typeof(IntegerVariable))
			{
				summary += integerData.GetDescription();
			}
			else if (variable.GetType() == typeof(FloatVariable))
			{
				summary += floatData.GetDescription();
			}
			else if (variable.GetType() == typeof(StringVariable))
			{
				summary += stringData.GetDescription();
			}

			return summary;
		}

		public override bool HasReference(Variable variable)
		{
			return (variable == this.variable);
		}

		public override bool OpenBlock()
		{
			return true;
		}

		public override Color GetButtonColor()
		{
			return new Color32(253, 253, 150, 255);
		}
	}

}