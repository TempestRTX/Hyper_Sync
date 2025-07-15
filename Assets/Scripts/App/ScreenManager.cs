using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenManager : MonoBehaviour
{
    protected GameManager gameManager;
    protected EventManager eventManager;
    public GameState.AppState AppState;
    private void OnEnable()
    {
        Init();
    }
    
    //Get Common script Instances
    protected virtual void Init()
    {
        gameManager=GameManager.Instance;
        eventManager=EventManager.Instance;
    }
}
