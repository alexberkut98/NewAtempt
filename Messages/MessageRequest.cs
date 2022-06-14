using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Новая_попытка.Messages
{
    public class MessageRequest
    {
        #region Properties

        public string Message { get; set; }

        #endregion Properties

        #region Constructors

        public MessageRequest(string message)
        {
            Message = message;
        }

        #endregion Constructors

        #region Methods

        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
            {
                Identifier = nameof(MessageRequest),
                Payload = this
            };

            return container;
        }

        #endregion Methods
    }
}
