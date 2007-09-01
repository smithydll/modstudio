using System;
using System.Collections.Generic;
using System.Text;

namespace ModStudio
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEditor
    {
        /// <summary>
        /// 
        /// </summary>
        void SaveFile();
        /// <summary>
        /// 
        /// </summary>
        void SaveFileAs();
        /// <summary>
        /// 
        /// </summary>
        void SetModified();
        /// <summary>
        /// 
        /// </summary>
        void SetUnmodified();

        /// <summary>
        /// 
        /// </summary>
        void DoCut();
        /// <summary>
        /// 
        /// </summary>
        void DoCopy();
        /// <summary>
        /// 
        /// </summary>
        void DoPaste();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsCopy();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsCut();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsCutCopyPaste();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsPaste();
    }
}
