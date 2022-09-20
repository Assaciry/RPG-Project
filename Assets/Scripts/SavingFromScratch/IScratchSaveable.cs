
namespace RPG.ScratchSaving
{
    interface IScratchSaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}
