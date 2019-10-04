using System;
using System.Collections.Generic;
using System.Text;
using MvxAutomationApp.Core.Models;

namespace MvxAutomationApp.Core.Services
{
    public interface IPopupService
    {
        void Show(MessageType messageType, string message = null);
    }
}
