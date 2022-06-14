using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Новая_попытка._Enums_;

namespace Новая_попытка.Messages
{
    public class ConnectionResponse
    {
        #region Properties

        public ResultCodes Result { get; set; }

        public string Reason { get; set; }

        #endregion Properties

        #region Methods

        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
            {
                Identifier = nameof(ConnectionResponse),
                Payload = this
            };

            return container;
        }

        #endregion Methods
    }
}
