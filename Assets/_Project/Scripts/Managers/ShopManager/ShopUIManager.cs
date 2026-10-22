using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Canvas")]
    [Tooltip("Add CanvasGroup ex. Frontdesk Canvas, Craft Canvas dll")]
    [SerializeField] private List<CanvasGroup> _allCanvas = new List<CanvasGroup>();

    private Dictionary<string, CanvasGroup> _canvasDict = new Dictionary<string, CanvasGroup>();

    #region Unity Lifecycle
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitializeCanvasDictionary();
    }
    #endregion

    #region Canvas Group Managament
    /// <summary>
    /// Toggle hide or show for canvas
    /// </summary>
    public void ToggleCanvas(string canvasName, bool isVisible)
    {
        if (_canvasDict.TryGetValue(canvasName, out CanvasGroup canvas))
        {
            SetCanvasState(canvas, isVisible);
        }
        else
        {
            Debug.LogWarning($"[UIManager] canvas {canvasName} didn't exist");
        }
    }
    
    /// <summary>
    /// Close or hide all canvas
    /// </summary>
    public void CloseAllCanvas()
    {
        foreach (var canvas in _allCanvas)
        {
            SetCanvasState(canvas, false);
        }
    }

    // Sign canvas by its name and store it inside _canvasDict dictionary
    private void InitializeCanvasDictionary()
    {
        foreach (var canvas in _allCanvas)
        {
            if (_canvasDict == null) continue;

            string canvasKey = canvas.gameObject.name;

            if (!_canvasDict.ContainsKey(canvasKey))
            {
                _canvasDict.Add(canvasKey, canvas);
            }
            else
            {
                Debug.LogWarning($"[UIManager] canvas already exist: {canvasKey}!", canvas.gameObject);
            }
        }
    }

    private void SetCanvasState(CanvasGroup canvas, bool isVisible)
    {
        canvas.alpha = isVisible ? 1 : 0;
        canvas.interactable = isVisible;
        canvas.blocksRaycasts = isVisible;
    }
    #endregion
}