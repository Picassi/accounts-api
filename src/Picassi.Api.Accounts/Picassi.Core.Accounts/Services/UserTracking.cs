using System;
using System.Linq;
using Microsoft.Owin;
using Picassi.Core.Accounts.Bus;

namespace Picassi.Core.Accounts.Services
{
    public interface IUserTracking
    {
        void UserActive();
    }
    public class UserTracking : IUserTracking
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly IOwinContext _owinContext;

        public UserTracking(IMessagePublisher messagePublisher, IOwinContext owinContext)
        {
            _messagePublisher = messagePublisher;
            _owinContext = owinContext;
        }

        public void UserActive()
        {
            var principal = _owinContext.Authentication?.User;

            var identifier = principal?.Claims.FirstOrDefault(c => c.Type == "username")?.Value ?? "Developer";

            var userModel = new UserLogModel { Identifier = identifier, Time = DateTime.UtcNow };
            _messagePublisher.Publish("UserActive", userModel);
        }

        class UserLogModel
        {
            public string Identifier { get; set; }
            public DateTime Time { get; set; }
        }
    }
}
