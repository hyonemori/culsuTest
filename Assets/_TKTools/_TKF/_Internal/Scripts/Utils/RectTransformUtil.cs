using UnityEngine;
using System.Collections;

namespace TKF
{
    public class RectTransformUtil
    {
        /// <summary>
        /// Worlds to local position from screen space camera.
        /// </summary>
        /// <returns>The to local position from screen space camera.</returns>
        /// <param name="worldPosition">World position.</param>
        /// <param name="camera">Camera.</param>
        /// <param name="canvas">Canvas.</param>
        static public Vector3 WorldToLocalPositionFromScreenSpaceCamera(Vector3 worldPosition,
                                                                        Camera camera,
                                                                        Canvas canvas)
        {
            return  WorldToLocalPositionFromScreenSpaceCamera(worldPosition, camera, canvas, canvas.GetComponent<RectTransform>());
        }

        /// <summary>
        /// Worlds to local position from screen space camera.
        /// </summary>
        /// <returns>The to local position from screen space camera.</returns>
        /// <param name="worldPosition">World position.</param>
        /// <param name="camera">Camera.</param>
        /// <param name="canvas">Canvas.</param>
        /// <param name="canvasRectTransform">Canvas rect transform.</param>
        static public Vector3 WorldToLocalPositionFromScreenSpaceCamera(Vector3 worldPosition,
                                                                        Camera camera,
                                                                        Canvas canvas,
                                                                        RectTransform canvasRectTransform)
        {
            var pos = Vector2.zero;
            var uiCamera = camera;
            var worldCamera = camera;
            var canvasRect = canvasRectTransform;

            var screenPos = RectTransformUtility.WorldToScreenPoint(worldCamera, worldPosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out pos);
            return pos;
        }

        /// <summary>
        /// Screens the point to view port in rectangle.
        /// </summary>
        /// <returns>The point to view port in rectangle.</returns>
        public static Vector2 ScreenPointToPivotPointRectangle(
            RectTransform rect,
            Vector2 screenPos,
            Canvas canvas
        )
        {
            float width = rect.sizeDelta.x;
            float height = rect.sizeDelta.y;
            Vector2 localPos = default(Vector2); 
            Vector2 currentPivot = rect.pivot;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rect,
                screenPos,
                canvas.worldCamera,
                out localPos
            );
            //pivot(0,0)とした時のpivotのローカル位置
            Vector2 pivot00LocalPos = new Vector2(currentPivot.x * width, currentPivot.y * height);
            //pivot(0,0)とした時の目標のローカル位置
            Vector2 pivot00targetPos = pivot00LocalPos + localPos;
            //viewPortに変換
            Vector2 viewPort = new Vector2(pivot00targetPos.x / width, pivot00targetPos.y / height);
            return viewPort;
        }

        /// <summary>
        /// Screens the point to anchor position.
        /// </summary>
        /// <returns>The point to anchor position.</returns>
        /// <param name="rect">Rect.</param>
        /// <param name="screenPos">Screen position.</param>
        /// <param name="canvas">Canvas.</param>
        public static Vector2 ScreenPointToAnchorPointInRectangle(
            RectTransform rect,
            Vector2 screenPos,
            Canvas canvas
        )
        {
            Vector3 worldPos = default(Vector3);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, screenPos, canvas.worldCamera, out worldPos);        
            Vector3 viewPort = canvas.worldCamera.WorldToViewportPoint(worldPos);
            return viewPort;
        }
    }
}