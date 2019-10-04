using System;
using System.Threading.Tasks;
using MvxAutomationApp.Core.Models;

namespace MvxAutomationApp.Core.Services
{
    public interface IPopupService
    {
        void Show(MessageType messageType, string message = null);
        Task<DateTimeOffset> PickDate();
    }
}
