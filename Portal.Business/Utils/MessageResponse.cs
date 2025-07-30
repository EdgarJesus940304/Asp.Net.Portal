using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Business.Utils
{
    public enum ResponseType { OK, Warning, Error }

    public class MessageResponse
    {
        public MessageResponse()
        {
            Message = "";
            Number = 0;
        }

        public ResponseType ResponseType { get; set; }
        public string Message { get; set; }
        public int Number { get; set; }
    }


    public class MessageResponse<T> : MessageResponse
    {
        public T Data { get; set; }
    }
}
