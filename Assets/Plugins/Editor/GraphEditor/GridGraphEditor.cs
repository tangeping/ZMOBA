using System.Collections;
using System.Collections.Generic;
using KBEngine;
using UnityEditor;
using UnityEngine;




namespace KBEngine
{
    public class GridGraphEditor : GraphicEditorBase
    {

        public override void DrawInspectorGUI()
        {
            if (targetGraph == null) return;
            GridNavGraph graphObj = targetGraph as GridNavGraph;

            if(graphObj == null)
            {
                Debug.LogError("GridGraphEditor::transform GridNavGraph fail!");
                return;
            }

            DrawBaseAttribute(graphObj);
        }

        /// <summary>
        /// 绘制基本属性
        /// </summary>
        /// <param name="graphObj"></param>
        public void DrawBaseAttribute(GridNavGraph graphObj)
        {
            bool changedFlag = false;
            //绘制width\depeh
            int newWidth = EditorGUILayout.IntField(new GUIContent("Width (nodes)", "Width of the graph in nodes"), graphObj.nodesInWidth);
            int newDepth = EditorGUILayout.IntField(new GUIContent("Depth (nodes)", "Depth (or height you might also call it) of the graph in nodes"), graphObj.nodesInDepth);

            if (newWidth != graphObj.nodesInWidth || newDepth != graphObj.nodesInDepth)
            {
                graphObj.gridDefineSize.x = newWidth * graphObj.nodeSize;
                graphObj.gridDefineSize.y = newDepth * graphObj.nodeSize;
                graphObj.gridCenter = graphObj.Matrix.MultiplyPoint3x4(new Vector3((newWidth / 2F), 0, (newDepth/2F)));
                graphObj.UpdateBaseArgs();
                changedFlag = true;
            }

            //绘制nodeSize
            float nodeSize = EditorGUILayout.FloatField(new GUIContent("Node Size", "The size of a single node"), graphObj.nodeSize);
            nodeSize = nodeSize < 0.01F ? 0.01F : nodeSize;
            if (nodeSize != graphObj.nodeSize)
            {
                graphObj.UpdateNodeSizeFromEditorChange(nodeSize);
                changedFlag = true;
            }

            //绘制center按钮
            graphObj.gridCenter = NearestIntVector3(graphObj.gridCenter);
            Vector3 centerVec3 = EditorGUILayout.Vector3Field("Center", graphObj.gridCenter);

            if (centerVec3 != graphObj.gridCenter)
            {
                graphObj.gridCenter = centerVec3;
                changedFlag = true;
            }

            //坡度值
            float maxSlope = EditorGUILayout.FloatField(new GUIContent("Max Slope", ""), graphObj.maxSlope);
            if (maxSlope != graphObj.maxSlope)
            {
                graphObj.maxSlope = maxSlope;
                changedFlag = true;
            } 

            //最大连接距离
            float maxDiffPosition = EditorGUILayout.FloatField(new GUIContent("Max Length", "Maximum connection length between two nodes!"), graphObj.maxDiffPosition);
            if (maxDiffPosition != graphObj.maxDiffPosition)
            {
                graphObj.maxDiffPosition = maxDiffPosition;
                changedFlag = true;
            } 

            //绘制冲突检查的属性, 像障碍物和高度检查
            DrawCollisionCheckAttribute(graphObj, ref changedFlag);


            if (changedFlag)
            {
                graphObj.ScanGraphicInternal();
            }

        }

        /// <summary>
        /// 绘制冲突检查的属性, 像障碍物和高度检查
        /// </summary>
        public void DrawCollisionCheckAttribute(GridNavGraph graphObj, ref bool changeFlag)
        {
            if(graphObj.graphicCollision == null) return;
            GraphicCollisionHandle graphicCollision = graphObj.graphicCollision;

            #region  碰撞检查

            bool checkFlag = graphicCollision.collisionCheck;
            graphicCollision.collisionCheck = GUILayout.Toggle(graphicCollision.collisionCheck,"Collision Check");
            if (checkFlag != graphicCollision.collisionCheck) changeFlag = true;

            EditorGUI.BeginDisabledGroup(!graphicCollision.collisionCheck);

            //射线高度
            float tmpData = graphicCollision.height;
            graphicCollision.height = EditorGUILayout.FloatField(new GUIContent("Ray Length", "Is the length of the ray"), graphicCollision.height);
            if (tmpData != graphicCollision.height) changeFlag = true;

            ////检测节点时的向上偏移的值
            //tmpData = graphicCollision.upwardOffsetOfNode;
            //graphicCollision.upwardOffsetOfNode = EditorGUILayout.FloatField(new GUIContent("UpwardOffset", "The value of the upward offset when the node is detected"), graphicCollision.upwardOffsetOfNode);
            //if (tmpData != graphicCollision.upwardOffsetOfNode) changeFlag = true;

            //检查的层
            int selectMask = graphicCollision.maskOfCollisionCheck.value;
            graphicCollision.maskOfCollisionCheck.value = EditorGUILayout.MaskField("Mask", selectMask, AStarPathEditor.GetLayerMaskField(true));
            if(selectMask != graphicCollision.maskOfCollisionCheck.value) changeFlag = true;

            EditorGUI.EndDisabledGroup();

            #endregion

            #region  高度检查

            checkFlag = graphicCollision.heightCheck;
            graphicCollision.heightCheck = GUILayout.Toggle(graphicCollision.heightCheck, "Height Check");
            if (checkFlag != graphicCollision.heightCheck) changeFlag = true;

            EditorGUI.BeginDisabledGroup(!graphicCollision.heightCheck);

            //射线高度
            tmpData = graphicCollision.rayOfLaunchHeight;
            graphicCollision.rayOfLaunchHeight = EditorGUILayout.FloatField(new GUIContent("Height", "Is the length of the ray"), graphicCollision.rayOfLaunchHeight);
            if (tmpData != graphicCollision.rayOfLaunchHeight) changeFlag = true;

            //检查的层
            selectMask = graphicCollision.maskOfHeightCheck.value;
            graphicCollision.maskOfHeightCheck.value = EditorGUILayout.MaskField("Mask", selectMask, AStarPathEditor.GetLayerMaskField());
            if (selectMask != graphicCollision.maskOfHeightCheck.value) changeFlag = true;

            EditorGUI.EndDisabledGroup();

            #endregion

        }

        /// <summary>
        /// 趋向于最近整数的Vector3
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 NearestIntVector3(Vector3 v)
        {
            if (Mathf.Abs(v.x - Mathf.Round(v.x)) < 0.001f) v.x = Mathf.Round(v.x);
            if (Mathf.Abs(v.y - Mathf.Round(v.y)) < 0.001f) v.y = Mathf.Round(v.y);
            if (Mathf.Abs(v.z - Mathf.Round(v.z)) < 0.001f) v.z = Mathf.Round(v.z);
            return v;
        }

    }

}
