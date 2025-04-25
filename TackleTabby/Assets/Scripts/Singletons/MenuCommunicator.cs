public class MenuCommunicator : GenericSingleton<MenuCommunicator>
{
    public bool HasMenuOpen => _currentlyOpenMenu != null;
    public IOverlayMenu CurrentMenu => _currentlyOpenMenu;
    
    private IOverlayMenu _currentlyOpenMenu = null;


    public bool OpenedMenu(IOverlayMenu menu)
    {
        if (_currentlyOpenMenu != null)
            return false;
        
        _currentlyOpenMenu = menu;
        return true;
    }

    public bool ClosedMenu(IOverlayMenu menu)
    {
        if (_currentlyOpenMenu == null)
            return false;
        
        if (menu != _currentlyOpenMenu)
            return false;

        _currentlyOpenMenu = null;
        return true;
    }

    public bool ForceCloseCurrentMenu()
    {
        if (_currentlyOpenMenu == null)
            return false;
        
        _currentlyOpenMenu.CloseOverlay();
        return true;
    }
}