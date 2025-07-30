using Portal.Business.Utils;
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
        public MessageResponse ReadCfdi(Dictionary<string, Stream> files)
        {
            Guid id = Guid.NewGuid();
            PreviewReaderModel model = new PreviewReaderModel();
            MessageResponse response = new MessageResponse();
            response.ResponseType = ResponseType.OK;


            List<string> errors = new List<string>();
            foreach (var file in files)
            {
                XmlDocument document = new XmlDocument();
                document.Load(file.Value);

                string version = GetCfdiVersion(document);

                switch (version)
                {
                    //case CfdiVersion.VERSION_33:
                    //    var result33 = DeserializeCfdi33ToPreviewModel(file.Value, file.Key, parameters, isToValidate);
                    //    errors.AddRange((List<string>)result33.Data);
                    //    model.PreviewReaderEntries.Add((PreviewReaderEntries)result33.Entries);
                    //    break;
                    case CfdiVersion.VERSION_40:
                        var result40 = DeserializeCfdi40ToPreviewModel(file.Value, file.Key, parameters, isToValidate);
                        errors.AddRange((List<string>)result40.Data);
                        model.PreviewReaderEntries.Add((PreviewReaderEntries)result40.Entries);
                        break;
                    default:
                        break;
                }

                model.NumberOfFiles = model.NumberOfFiles + 1;
                model.ReaderId = id;

                file.Value.Close();
                file.Value.Dispose();
            }

            bool addEntries = false;
            bool groupEntries = false;
            using (var db = new ConfigurationHandler())
            {
                var company = db.GetCompany(parameters.CompanyId);
                addEntries = company?.AddEntries ?? false;
                //groupEntries = company?.GroupEntries ?? false;
                groupEntries = parameters.GroupEntries;

            }

            if (isToValidate)
            {

                decimal exchangeRate = model.PreviewReaderEntries.FirstOrDefault().ExchangeRate;
                var voucher = model.PreviewReaderEntries.FirstOrDefault();
                foreach (var c in model.PreviewReaderEntries.FirstOrDefault().ConceptEntries)
                {
                    if (string.IsNullOrWhiteSpace(c.SaeItem?.Id))
                    {
                        errors.Add($"El concepto con descripción {c.Description} no se encuentra mapeado en el proveedor {parameters.ProviderName}");
                    }
                    else
                    {
                        if (addEntries)
                        {
                            c.CompraPartida = new AlphaDevOps.Aspel.Sae.Negocio.Entidades.CompraPartidaBase();
                            c.CompraPartida.NUM_PAR = 0;
                            c.CompraPartida.CVE_ART = c.SaeItem.Id;
                            c.CompraPartida.CANT = c.Quantity;
                            c.CompraPartida.PXR = c.Quantity * c.SaeItem.ConversionFactor;


                            c.CompraPartida.IMPU1 = 0;
                            c.CompraPartida.IMPU2 = 0;
                            c.CompraPartida.IMPU3 = (c.ConceptEntriesTaxesWithheld?.FirstOrDefault()?.Tax ?? 0) * -100;
                            c.CompraPartida.IMPU4 = (c.ConceptEntriesTaxesTransferred.FirstOrDefault()?.Tax) * 100;
                            c.CompraPartida.TOTIMP4 = c.ConceptEntriesTaxesTransferred.Sum(s => s.TaxAmount) * exchangeRate;
                            c.CompraPartida.TOTIMP3 = ((c.ConceptEntriesTaxesWithheld?.Sum(s => s.TaxAmount) ?? 0) * -1) * exchangeRate;

                            c.CompraPartida.PREC = (c.RealCost + (c.Discount)) * exchangeRate; //entry.ValorUnitario;
                            c.CompraPartida.COST = (c.RealCost + (c.Discount)) * exchangeRate; //entry.ValorUnitario;
                            c.CompraPartida.COST_DEV = c.SaeItem.ConversionFactor != 0 ? (c.RealCost + (c.Discount)) / c.SaeItem.ConversionFactor * exchangeRate : 0;
                            c.CompraPartida.TOT_PARTIDA = ((c.RealCost + (c.Discount)) * c.Quantity) * exchangeRate;

                            if (voucher.Discount > 0)
                            {
                                decimal totalEntry = ((c.RealCost + (c.Discount)) * c.Quantity) * exchangeRate;
                                c.CompraPartida.DESCU = voucher.Discount > 0 ? ((c.Discount > 0 ? (((c.Discount * c.Quantity) * 100) / totalEntry) : 0) * exchangeRate) : 0;
                            }
                            else
                            {
                                c.CompraPartida.DESCU = 0;
                            }

                            c.CompraPartida.TIP_CAM = exchangeRate;
                            c.CompraPartida.TIPO_PROD = c.SaeItem.ElementType;
                            c.CompraPartida.TIPO_ELEM = "N";
                            c.CompraPartida.UNI_VENTA = c.SaeItem.SalesUnit;
                            c.CompraPartida.MINDIRECTO = 0;
                            c.CompraPartida.MAN_IEPS = "N";
                            c.CompraPartida.APL_MAN_IMP = 0;
                            c.CompraPartida.MTO_CUOTA = 0;
                            c.CompraPartida.MTO_PORC = 0;
                            c.CompraPartida.CUOTA_IEPS = 0;
                            c.CompraPartida.CVE_ESQ = c.SaeItem.TaxId;

                            //if (!c.NullValue)
                            //{
                            SaeDb.OpenConnection();
                            var impuestos = Impuestos.ObtenerEsquemaImpuestos(SaeDb, c.SaeItem.TaxId);
                            SaeDb.CloseConnection();
                            c.CompraPartida.IMP1APLA = (short?)impuestos.IMP1APLICA;
                            c.CompraPartida.IMP2APLA = (short?)impuestos.IMP2APLICA;
                            c.CompraPartida.IMP3APLA = (short?)impuestos.IMP3APLICA;
                            c.CompraPartida.IMP4APLA = (short?)impuestos.IMP4APLICA;
                            c.CompraPartida.FACTCONV = c.SaeItem.ConversionFactor;
                        }
                    }

                    //}
                }


            }

            if (groupEntries)
            {
                int entryOrder = 0;
                var entries = (from a in model.PreviewReaderEntries.FirstOrDefault().ConceptEntries
                               group a by new { a.SatItemDescription } into g
                               select g).Select(c => new ConceptEntries()
                               {
                                   EntryId = entryOrder++,
                                   SatItemId = c.FirstOrDefault().SatItemId,
                                   SatItemDescription = c.FirstOrDefault().SatItemDescription,
                                   SaeItem = c.FirstOrDefault().SaeItem,
                                   NullValue = c.FirstOrDefault().NullValue,
                                   Cost = c.FirstOrDefault().Cost,
                                   Base = c.FirstOrDefault().Base,
                                   Discount = c.FirstOrDefault().Discount,
                                   Quantity = c.Sum(x => x.Quantity),
                                   DiscountApportion = c.FirstOrDefault().DiscountApportion,
                                   Price = c.FirstOrDefault().Price,
                                   CurrencyPrice = c.FirstOrDefault().CurrencyPrice,
                                   LastDatePrice = c.FirstOrDefault().LastDatePrice,
                                   RealCost = c.FirstOrDefault().RealCost,
                                   TotalEntry = c.FirstOrDefault().TotalEntry,
                                   ItemType = c.FirstOrDefault().ItemType,
                                   IdentificationNumber = c.FirstOrDefault().IdentificationNumber,
                                   SatItemKey = c.FirstOrDefault().SatItemKey,
                                   SatUnitKey = c.FirstOrDefault().SatUnitKey,
                                   Description = c.FirstOrDefault().Description,
                                   ConceptEntriesTaxesTransferred = c.FirstOrDefault().ConceptEntriesTaxesTransferred,
                                   ConceptEntriesTaxesWithheld = c.FirstOrDefault().ConceptEntriesTaxesWithheld,
                                   ItemSeries = c.SelectMany(x => x.ItemSeries).ToList(),
                                   ItemCustoms = c.SelectMany(x => x.ItemCustoms).ToList(),
                                   InternalItemMappedId = c.FirstOrDefault().InternalItemMappedId
                               }).ToList();

                model.PreviewReaderEntries.FirstOrDefault().ConceptEntries = entries;
            }

            if (errors.Count == 0)
            {
                try
                {
                    using (var transaction = Db.Database.BeginTransaction())
                    {
                        foreach (var file in model.PreviewReaderEntries)
                        {
                            var result = SaveFile(file.Xml, id, file.Id);
                            if (result.ResponseType != ResponseType.OK)
                            {
                                throw new Exception(result.Message);
                            }

                        }

                        using (var commonHandler = new CommonHandler(parameters.CompanyId))
                        {
                            var parametersConfig = commonHandler.ObtenerParametrosInventario();
                            model.IsMultiWarehouse = parametersConfig.MULTIALMACEN == "T";
                        }
                        response.ResponseType = ResponseType.OK;
                        response.Data = model;

                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    response = new MessageResponse()
                    {
                        ResponseType = ResponseType.Error,
                        Message = ex.Message
                    };
                }
            }
            else
            {
                response = new MessageResponse()
                {
                    ResponseType = ResponseType.Error,
                    Message = string.Join(";", errors)
                };
            }

            return response;
        }

    }
}
