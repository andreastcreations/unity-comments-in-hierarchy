/************************************************************************************************************/
/*                                                                                                          */
/*          Some of the code here is from Unity3DCollege (Jason Weimann):                                   */
/*          Site:       https://unity3d.college/2017/09/04/customizing-hierarchy-bold-prefab-text/          */
/*          YouTube:    https://www.youtube.com/watch?v=pdDrY8Mc2lU                                         */
/*                                                                                                          */
/************************************************************************************************************/

#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ATMedia.CustomTools.Hierarchy
{
    [InitializeOnLoad]
    public class CommentsInHierarchy
    {
        static CommentsInHierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleComments_HierarchyWindowItemOnGUI;
        }

        private static void HandleComments_HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            // Declare variables for the colors
            Color32 selectionColor;
            Color32 hoverColor;
            Color32 normalColor;

            // Check if we are using the light or the dark skin
            if (EditorGUIUtility.isProSkin)
            {
                // Dark skin
                selectionColor = new Color32(44, 93, 134, 255);
                hoverColor = new Color32(68, 68, 68, 255);
                normalColor = new Color32(56, 56, 56, 255);
            }
            else
            {
                // Light skin
                selectionColor = new Color32(58, 114, 175, 255);
                hoverColor = new Color32(178, 178, 178, 255);
                normalColor = new Color32(194, 194, 194, 255);
            }

            // Get the Object
            Object obj = EditorUtility.InstanceIDToObject(instanceID);

            if (obj != null)
            {
                // Check for two comment styles
                bool firstCommentType = obj.name.StartsWith("//");
                bool secondCommentType = obj.name.StartsWith("/*") && obj.name.EndsWith("*/");

                // If one of them is used, change the GUIStyle
                if (firstCommentType || secondCommentType)
                {
                    Color32 backgroundColor;

                    // Set colors for every case (selected, hovered and regular)
                    if (Selection.instanceIDs.Contains(instanceID))
                    {
                        backgroundColor = selectionColor;
                    }
                    else
                    {
                        if (selectionRect.Contains(Event.current.mousePosition))
                        {
                            backgroundColor = hoverColor;
                        }
                        else
                        {
                            backgroundColor = normalColor;
                        }
                    }

                    // The original Rect has about 2 pixels offset, so take that into account
                    Rect offsetRect = new Rect(selectionRect.position + new Vector2(0f, 2f), selectionRect.size);
                    // Create the style we will use for our comments.
                    GUIStyle style = new GUIStyle()
                    {
                        normal = new GUIStyleState()
                        {
                            textColor = Color.green
                        },
                        fontStyle = FontStyle.Bold,
                        alignment = TextAnchor.MiddleLeft,
                        fixedHeight = 10
                    };

                    // Draw the rectangle and add the label on top of it.
                    EditorGUI.DrawRect(selectionRect, backgroundColor);
                    EditorGUI.LabelField(offsetRect, obj.name, style);

                    // Get the GameObject, set it to inactive and add an "EditorOnly" tag, in order to be exlcuded from any builds.
                    GameObject go = (GameObject)obj;
                    go.SetActive(false);
                    // Check if it is already tagged. If we don't check, Unity doesn't let us rename it for some reason.
                    if (go.tag != "EditorOnly")
                    {
                        go.tag = "EditorOnly";
                    }
                }
            }
        }
    }
}

#endif