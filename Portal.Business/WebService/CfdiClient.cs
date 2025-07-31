using Portal.Business.ServiceReference;
using Portal.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Business.WebService
{
    public class CfdiClient
    {
        public MessageResponse<byte[]> ConsumeServicePdf(string usuario, string password, string uuid)
        {
            try
            {
                MessageResponse<byte[]> messageResponse = new MessageResponse<byte[]>();

                WSClient objClient = new WSClient("WSPort");
                var responseClient = objClient.ObtenerPDF(usuario, password, uuid);


                if (!responseClient.Exitoso)
                {
                    messageResponse.ResponseType = ResponseType.Error;
                    messageResponse.Message = responseClient.MensajeError;
                    messageResponse.Number = responseClient.CodigoError;
                }
                else
                {
                    messageResponse.ResponseType = ResponseType.OK;
                    messageResponse.Data = responseClient.PDF;
                }


                return messageResponse;

            }
            catch (Exception ex)
            {
                return new MessageResponse<byte[]>()
                {
                    ResponseType = ResponseType.Error,
                    Message = $"{ex.Message} {ex?.InnerException}"
                };
            }
        }
    }
}
