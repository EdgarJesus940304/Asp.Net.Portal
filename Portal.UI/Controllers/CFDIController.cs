using Newtonsoft.Json;
using Portal.Business.Handler;
using Portal.Business.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Portal.UI.Controllers
{
    public class CFDIController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        #region Lectura de XML
        [HttpPost]
        public ActionResult ReadCfidVoucher()
        {
            Dictionary<string, Stream> data = new Dictionary<string, Stream>();
            MessageResponse messageResponse = new MessageResponse();
            CfdiHandler handler = new CfdiHandler();
            bool isSavedSuccessfully = true;
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    if (file != null && file.ContentLength > 0)
                    {
                        data.Add(file.FileName, file.InputStream);
                    }

                }


                var response = handler.BuildPreviewModelXml(data, parameters, false);

                if (response.ResponseType == Bussiness.Models.Common.ResponseType.OK)
                {
                    messageResponse.Data = response.Data;
                    messageResponse.Status = HttpStatusCode.OK;
                    messageResponse.Message = "LECTURA TERMINADA SATISFACTORIAMENTE";
                    Response.StatusCode = (int)HttpStatusCode.OK;
                }
                else if (response.ResponseType == Bussiness.Models.Common.ResponseType.Warning)
                {
                    throw new BussinessException(response.Message);
                }
                else
                {
                    throw new Exception(response.Message);
                }

            }
            catch (BussinessException ex)
            {
                isSavedSuccessfully = false;
                messageResponse.Message = ex.Message;
                messageResponse.Data = data;
                messageResponse.Status = HttpStatusCode.NoContent;

                if (!isSavedSuccessfully)
                {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
                messageResponse.Message = string.Format("{0} {1}",
                    ex.Message, (ex?.InnerException?.Message ?? string.Empty));

                Response.ClearHeaders();
                Response.ClearContent();

                if (!isSavedSuccessfully)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    Response.StatusDescription = "Petición Incorrecta";
                    messageResponse.Status = HttpStatusCode.BadRequest;
                }
            }


            return Json(messageResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}