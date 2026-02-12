namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Rooms
{
    public class Door
    {
        public bool exists;
        public bool? isLocked;
        public bool isOpen;
        public bool isBlocked;
        public bool isEntry;

        public Door(bool exists, bool? isLocked, bool isOpen, bool isEntry) {
            this.exists = exists;
            this.isLocked = isLocked;
            this.isOpen = isOpen;
            this.isEntry = isEntry;
        }
    }
}
