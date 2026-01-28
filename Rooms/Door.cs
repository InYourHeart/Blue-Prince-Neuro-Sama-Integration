namespace Blue_Prince_Neuro_Sama_Integration_Mod.Rooms
{
    public class Door
    {
        public bool? isLocked;
        public bool isOpen;

        public Door(bool? isLocked, bool isOpen) {
            this.isLocked = isLocked;
            this.isOpen = isOpen;
        }
    }
}
