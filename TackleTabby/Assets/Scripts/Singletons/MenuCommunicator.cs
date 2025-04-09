using UnityEngine;

public class MenuCommunicator : GenericSingleton<MenuCommunicator>
{
    public bool HasMenuOpen => _hasMenuOpen;
    private bool _hasMenuOpen = false;


    public bool OpenMenu() => _hasMenuOpen = true;
    public bool CloseMenu() => _hasMenuOpen = false;
}