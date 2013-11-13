namespace FapChat.Wp8.Interfaces
{
    public interface IPhonePageExtender
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="showMask"></param>
        void SetProgress(string message, bool showMask = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hideMask"></param>
        void HideProgress(bool hideMask = false);
    }
}
