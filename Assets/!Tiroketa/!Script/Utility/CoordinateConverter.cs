using UnityEngine;
using UnityEngine.EventSystems;

public static class CoordinateConverter
{
    /// <summary>
    /// Конвертирует мировые координаты в позицию на Canvas
    /// </summary>
    /// <param name="worldPosition">Мировая позиция</param>
    /// <param name="canvas">Целевой Canvas (если null, используется первый найденный)</param>
    /// <param name="camera">Камера для рендеринга (если Canvas в режиме Screen Space - Camera)</param>
    /// <returns>Позиция на Canvas</returns>
    public static Vector2 WorldToCanvasPosition(Vector3 worldPosition, Canvas canvas = null, Camera camera = null)
    {
        if (!canvas) canvas = Object.FindAnyObjectByType<Canvas>();
        if (!canvas) return Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            !camera ? Camera.main.WorldToScreenPoint(worldPosition) : camera.WorldToScreenPoint(worldPosition),
            !camera ? null : camera,
            out Vector2 canvasPosition);

        return canvasPosition;
    }

    /// <summary>
    /// Конвертирует позицию на Canvas в мировые координаты
    /// </summary>
    /// <param name="canvasPosition">Позиция на Canvas</param>
    /// <param name="canvas">Исходный Canvas (если null, используется первый найденный)</param>
    /// <param name="camera">Камера для рендеринга (если Canvas в режиме Screen Space - Camera)</param>
    /// <param name="zPosition">Z-координата в мировом пространстве</param>
    /// <returns>Мировая позиция</returns>
    public static Vector3 CanvasToWorldPosition(Vector2 canvasPosition, Canvas canvas = null, Camera camera = null, float zPosition = 0f)
    {
        if (!canvas) canvas = Object.FindAnyObjectByType<Canvas>();
        if (!canvas) return Vector3.zero;

        if (!camera && canvas.renderMode == RenderMode.ScreenSpaceCamera)
            camera = canvas.worldCamera;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            RectTransformUtility.WorldToScreenPoint(
                !camera ? Camera.main : camera,
                canvas.transform.TransformPoint(canvasPosition)),
            !camera ? Camera.main : camera,
            out Vector3 worldPosition);

        worldPosition.z = zPosition;
        return worldPosition;
    }

    /// <summary>
    /// Конвертирует экранные координаты в позицию на Canvas
    /// </summary>
    public static Vector2 ScreenToCanvasPosition(Vector2 screenPosition, Canvas canvas = null, Camera camera = null)
    {
        if (!canvas) canvas = Object.FindAnyObjectByType<Canvas>();
        if (!canvas) return Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPosition,
            !camera ? (canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main) : camera,
            out Vector2 canvasPosition);

        return canvasPosition;
    }

    /// <summary>
    /// Конвертирует позицию на Canvas в экранные координаты
    /// </summary>
    public static Vector2 CanvasToScreenPosition(Vector2 canvasPosition, Canvas canvas = null, Camera camera = null)
    {
        if (!canvas) canvas = Object.FindAnyObjectByType<Canvas>();
        if (!canvas) return Vector2.zero;

        if (!camera && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            camera = canvas.worldCamera ? canvas.worldCamera : Camera.main;

        return RectTransformUtility.WorldToScreenPoint(
            camera,
            canvas.transform.TransformPoint(canvasPosition));
    }
}