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
        public ActionResult ReadCfidVoucher(HttpPostedFileBase file)
        {
            MessageResponse<byte[]> response = new MessageResponse<byte[]>();
            CfdiHandler handler = new CfdiHandler();
            try
            {
                response = handler.GetPdf(file.InputStream);

                if (response.ResponseType == ResponseType.OK)
                {
                    return File(response.Data, "application/pdf", "archivo.pdf");
                }
                else
                {
                    throw new Exception(response.Message);
                }

            }
            catch (Exception ex)
            {
                Response.ClearHeaders();
                Response.ClearContent();
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return Json(new
                {
                    ResponseType = "Error",
                    Message = $"Error interno: {ex.Message} {(ex.InnerException?.Message ?? "")}"
                });
            }

        
        }

        #endregion

    }
}