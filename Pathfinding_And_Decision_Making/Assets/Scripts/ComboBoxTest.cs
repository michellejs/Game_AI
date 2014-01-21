using UnityEngine;
using System.Collections;

public class ComboBoxTest : MonoBehaviour
{
	GUIContent[] comboBoxList;
	private ComboBox comboBoxControl;// = new ComboBox();
	private GUIStyle listStyle = new GUIStyle();
	
	private void Start()
	{
		comboBoxList = new GUIContent[6];
		comboBoxList[0] = new GUIContent("Open");
		comboBoxList[1] = new GUIContent("Swamp");
		comboBoxList[2] = new GUIContent("Grass");
		comboBoxList[3] = new GUIContent("Obst.");
		comboBoxList[4] = new GUIContent("Goal");
		comboBoxList [5] = new GUIContent ("Start");

		listStyle.normal.textColor = Color.white; 
		listStyle.onHover.background =
			listStyle.hover.background = new Texture2D(2, 2);
		listStyle.padding.left =
			listStyle.padding.right =
				listStyle.padding.top =
				listStyle.padding.bottom = 4;
		
		comboBoxControl = new ComboBox(new Rect(600, 100, 50, 50), comboBoxList[0], comboBoxList, "button", "box", listStyle);
	}
	
	private void OnGUI () 
	{
		comboBoxControl.Show();
	}
}