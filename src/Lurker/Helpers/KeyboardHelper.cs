﻿//-----------------------------------------------------------------------
// <copyright file="KeyboardHelper.cs" company="Wohs">
//     Missing Copyright information from a valid stylecop.json file.
// </copyright>
//-----------------------------------------------------------------------

namespace Lurker.Helpers
{
    using System;
    using WindowsInput;

    public class KeyboardHelper
    {
        #region Fields

        private readonly object CommandLock = new object();
        private static readonly string EnterKey = "{ENTER}";
        private IntPtr _windowHandle;
        private InputSimulator _simulator;
        

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardHelper"/> class.
        /// </summary>
        /// <param name="windowHandle">The window handle.</param>
        public KeyboardHelper(IntPtr windowHandle)
            : this()
        {
            this._windowHandle = windowHandle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardHelper"/> class.
        /// </summary>
        public KeyboardHelper()
        {
            this._simulator = new InputSimulator();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Writes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Write(string text)
        {
            this._simulator.Keyboard.TextEntry(text);
        }

        /// <summary>
        /// Simulates a search using Ctrl+F 
        /// </summary>
        /// <param name="searchTerm">The search term to use</param>
        public void Search(string searchTerm)
        {
            lock (CommandLock)
            {
                Native.SetForegroundWindow(this._windowHandle);
                System.Windows.Forms.SendKeys.SendWait("^F");

                // We are using the interop since SendWait block mouse input.
                this._simulator.Keyboard.TextEntry(searchTerm);
                System.Windows.Forms.SendKeys.SendWait(EnterKey);
            }
        }

        /// <summary>
        /// Sends the command.
        /// </summary>
        /// <param name="command">The command.</param>
        protected void SendCommand(string command)
        {            
            lock (CommandLock)
            {                
                Native.SetForegroundWindow(this._windowHandle);
                System.Windows.Forms.SendKeys.SendWait(EnterKey);

                // We are using the interop since SendWait block mouse input.
                this._simulator.Keyboard.TextEntry(command);
                System.Windows.Forms.SendKeys.SendWait(EnterKey);
            }
        }

        #endregion
    }
}
