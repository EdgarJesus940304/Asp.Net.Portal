using Portal.Business.Utils;
using Portal.Business.WebService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Portal.Business.Handler
{
    public class CfdiHandler
    {
        public MessageResponse<byte[]> GetPdf(Stream file)
        {
            try
            {
                CfdiClient client = new CfdiClient();
                MessageResponse<string> result = GetUuidAtribute(file);

                if (result.ResponseType != ResponseType.OK)
                {
                    return new MessageResponse<byte[]>()
                    {
                        Message = result.Message,
                        ResponseType = result.ResponseType
                    };
                }

                MessageResponse<byte[]> responseClient = client.ConsumeServiceGetPdf(ServiceParameters.USER, ServiceParameters.PASSWORD, result.Data);


                return responseClient;


            }
            catch (Exception ex)
            {
                return new MessageResponse<byte[]>()
                {
                    Message = $"{ex.Message} {ex?.InnerException?.Message}",
                    ResponseType = ResponseType.Error
                };
            }
        }


        private MessageResponse<string> GetUuidAtribute(Stream file)
        {
            XmlDocument document = new XmlDocument();
            document.Load(file);
            XmlNamespaceManager ns = new XmlNamespaceManager(document.NameTable);
            ns.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4");
            ns.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            XmlNode timbreNode = document.SelectSingleNode("//cfdi:Complemento/tfd:TimbreFiscalDigital", ns);

            if (timbreNode != null)
            {
                string uuid = timbreNode.Attributes["UUID"]?.Value;
                return new MessageResponse<string>()
                {
                    ResponseType = ResponseType.OK,
                    Data = uuid
                };
            }
            else
            {
                return new MessageResponse<string>()
                {
                    ResponseType = ResponseType.Error,
                    Message = "El archivo CFDI no contiene un nodo de timbre fiscal"
                };
            }
        }

    }
}
