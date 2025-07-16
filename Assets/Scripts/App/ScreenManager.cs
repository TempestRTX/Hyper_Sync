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
        InitScreen();
    }

    private void OnDisable()
    {
        throw new NotImplementedException();
    }

    //Get Common script Instances
    protected virtual void InitScreen()
    {
        gameManager=GameManager.Instance;
        eventManager=EventManager.Instance;
    }

    protected virtual void DeactivateScreen()
    {
        
    }
}
