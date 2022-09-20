
namespace RPG.Controller
{
    public interface IRaycastable
    {
        bool HandleRaycast();
        CursorType GetCursorType();
    }
}