using POS_PrintingServer_API.Enums;
using POS_PrintingServer_API.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tring.Fiscal.Driver;
using System.Web.Http.Cors;
using System.IO;
using POS_PrintingServer_API.Helper;
using POS_PrintingServer_API.API.Tring;
using POS_PrintingServer_API.API.Datecs;
using System.Threading;

namespace POS_PrintingServer_WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PrintingController : ApiController
    {
        [Route("api/InvoicePrint")]
        [HttpPost]
        public HttpResponseMessage InvoicePrint(HttpRequestMessage request, InvoiceViewModel model)
        {

            HttpResponseMessage response = null;
            ClientDetailsViewModel _details = ClientDetailsViewModel.GetClientDetails();
            if (_details == null)
            {
                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, ErrorMessage = "Missing configurations !" });
                return response;
            }

            if (_details.pos_type == E_PrinterType.datecsv1.ToString() || _details.pos_type == E_PrinterType.datecsv2.ToString() || _details.pos_type == E_PrinterType.datecsv3.ToString())
            {
                string fiscal_date = string.Empty;
                string fiscal_time = string.Empty;
                string fiscal_number = string.Empty;
                string fiscal_amount = string.Empty;
                // PrintInvoice
                SaveCommandToFile(ApplicationHelper.DatecsFolderPath, Datecs_GenerateCommands.PrintInvoice(_details.pos_type, _details.company.uin, _details.pos_username, _details.pos_password, model));
                Thread.Sleep(5000);
                fiscal_number = Datecs_GenerateCommands.ReadInvoiceResponse();
                string _errorMessage = string.Empty;
                try
                {
                    if (!string.IsNullOrEmpty(fiscal_number))
                    {
                        _errorMessage = GlobalCallBackViewModel.InvoicePrintCallBackURL(model, _details.uuid, model.user_id, model.destination, model.id.ToString(), fiscal_date, fiscal_time, fiscal_number, fiscal_amount);
                    }
                }
                catch (Exception ex)
                {
                    _errorMessage = ex.Message;
                }



                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, ErrorMessage = _errorMessage });
                return response;
            }
            else if (_details.pos_type == E_PrinterType.tring.ToString())
            {
                KasaOdgovor kasaOdgovor = Tring_Invoice.PrintInvoice(model);
                if (kasaOdgovor.VrstaOdgovora == VrsteOdgovora.OK)
                {
                    string _errorMessage = string.Empty;
                    try
                    {
                        DateTime _outTemp;
                        string fiscal_date = string.Empty;
                        string fiscal_time = string.Empty;
                        string fiscal_number = string.Empty;
                        string fiscal_amount = string.Empty;
                        foreach (Odgovor item in kasaOdgovor.Odgovori)
                        {
                            if (item.Naziv == "BrojFiskalnogRacuna")
                            {
                                fiscal_number = item.Vrijednost.ToString();
                            }
                            else if (item.Naziv == "DatumFiskalnogRacuna")
                            {
                                if (DateTime.TryParse(item.Vrijednost.ToString(), out _outTemp))
                                {
                                    fiscal_date = _outTemp.ToString("yyyy-MM-dd");
                                }
                            }
                            else if (item.Naziv == "VrijemeFiskalnogRacuna")
                            {
                                if (DateTime.TryParse(item.Vrijednost.ToString(), out _outTemp))
                                {
                                    fiscal_time = _outTemp.ToString("hh:mm[:ss]");
                                }
                            }
                            else if (item.Naziv == "IznosFiskalnogRacuna")
                            {
                                fiscal_amount = item.Vrijednost.ToString();
                            }

                        }
                        _errorMessage = GlobalCallBackViewModel.InvoicePrintCallBackURL(model, _details.uuid, model.user_id, model.destination, model.id.ToString(), fiscal_date, fiscal_time, fiscal_number, fiscal_amount);
                    }
                    catch (Exception ex)
                    {
                        _errorMessage = ex.Message;
                    }
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true, obj = kasaOdgovor, ErrorMessage = _errorMessage });
                    return response;
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, obj = new { error = kasaOdgovor.Odgovori[0].Naziv, code = kasaOdgovor.Odgovori[0].Vrijednost }, ErrorMessage = "" });
                    return response;
                }

            }
            response = request.CreateResponse(HttpStatusCode.OK, new { success = false, ErrorMessage = "Missing printer type !" });
            return response;
        }


        [Route("api/InvoicePrintDuplicate")]
        [HttpPost]
        public HttpResponseMessage InvoicePrintDuplicate(HttpRequestMessage request, InvoiceViewModel model)
        {
            HttpResponseMessage response = null;
            ClientDetailsViewModel _details = ClientDetailsViewModel.GetClientDetails();
            if (_details == null)
            {
                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, ErrorMessage = "Missing configurations !" });
                return response;
            }
            if (_details.pos_type == E_PrinterType.datecsv1.ToString() || _details.pos_type == E_PrinterType.datecsv2.ToString() || _details.pos_type == E_PrinterType.datecsv3.ToString())
            {
                // InvoicePrintDuplicate
                SaveCommandToFile(ApplicationHelper.DatecsFolderPath, Datecs_GenerateCommands.InvoicePrintDuplicate(_details.pos_type, model.fiscal_number));
                string _errorMessage = string.Empty;
                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, ErrorMessage = _errorMessage });
                return response;
            }
            else if (_details.pos_type == E_PrinterType.tring.ToString())
            {
                string _errorMessage = string.Empty;
                try
                {
                    KasaOdgovor kasaOdgovor = Tring_Invoice.PrintInvoiceDuplicate(model);
                    if (kasaOdgovor.VrstaOdgovora == VrsteOdgovora.OK)
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true, obj = kasaOdgovor, ErrorMessage = _errorMessage });
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, obj = new { error = kasaOdgovor.Odgovori[0].Naziv, code = kasaOdgovor.Odgovori[0].Vrijednost }, ErrorMessage = _errorMessage });
                    }

                }
                catch (Exception ex)
                {
                    _errorMessage = ex.Message;
                    response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, obj = false, ErrorMessage = _errorMessage });
                }
                return response;
            }
            response = request.CreateResponse(HttpStatusCode.OK, new { success = false, ErrorMessage = "Missing printer type !" });
            return response;
        }


        [Route("api/InvoiceReclaimDuplicate")]
        [HttpPost]
        public HttpResponseMessage PrintReclaimDuplicate(HttpRequestMessage request, InvoiceViewModel model)
        {
            HttpResponseMessage response = null;
            ClientDetailsViewModel _details = ClientDetailsViewModel.GetClientDetails();
            if (_details == null)
            {
                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, ErrorMessage = "Missing configurations !" });
                return response;
            }
            if (_details.pos_type == E_PrinterType.datecsv1.ToString() || _details.pos_type == E_PrinterType.datecsv2.ToString() || _details.pos_type == E_PrinterType.datecsv3.ToString())
            {

                // ReclaimPrintDuplicate
                SaveCommandToFile(ApplicationHelper.DatecsFolderPath, Datecs_GenerateCommands.ReclaimPrintDuplicate(_details.pos_type, model.stormed_number));
                string _errorMessage = string.Empty;
                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, ErrorMessage = _errorMessage });
                return response;
            }
            else if (_details.pos_type == E_PrinterType.tring.ToString())
            {
                string _errorMessage = string.Empty;
                try
                {
                    KasaOdgovor kasaOdgovor = Tring_Invoice.PrintReclaimDuplicate(model);
                    if (kasaOdgovor.VrstaOdgovora == VrsteOdgovora.OK)
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true, obj = kasaOdgovor, ErrorMessage = _errorMessage });
                    }else
                    {
                        response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, obj = new { error = kasaOdgovor.Odgovori[0].Naziv, code = kasaOdgovor.Odgovori[0].Vrijednost }, ErrorMessage = _errorMessage });

                    }

                }
                catch (Exception ex)
                {
                    _errorMessage = ex.Message;
                    response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, obj = false, ErrorMessage = _errorMessage });
                }
                return response;
            }
            response = request.CreateResponse(HttpStatusCode.OK, new { success = false, ErrorMessage = "Missing printer type !" });
            return response;
        }

        [Route("api/ReclaimInvoicePrint")]
        [HttpPost]
        public HttpResponseMessage ReclaimInvoicePrint(HttpRequestMessage request, InvoiceViewModel model)
        {
            HttpResponseMessage response = null;
            ClientDetailsViewModel _details = ClientDetailsViewModel.GetClientDetails();
            if (_details == null)
            {
                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, ErrorMessage = "Missing configurations !" });
                return response;
            }
            if (_details.pos_type == E_PrinterType.datecsv1.ToString() || _details.pos_type == E_PrinterType.datecsv2.ToString() || _details.pos_type == E_PrinterType.datecsv3.ToString())
            {

                string stormed_time = string.Empty;
                string stormed_date = string.Empty;
                string stormed_number = string.Empty;
                string stormed_amount = string.Empty;
                string _errorMessage = string.Empty;

                // InvoicePrintDuplicate
                SaveCommandToFile(ApplicationHelper.DatecsFolderPath, Datecs_GenerateCommands.PrintInvoiceReclaim(_details.pos_type, _details.company.uin, _details.pos_username, _details.pos_password, model));
                Thread.Sleep(5000);

                stormed_number = Datecs_GenerateCommands.ReadReclamResponse();
                _errorMessage = GlobalCallBackViewModel.ReclaimInvoicePrintCallBackURL(model, _details.uuid, model.user_id, model.destination, model.id.ToString(), stormed_date, stormed_time, stormed_number, stormed_amount);
                request.CreateResponse(HttpStatusCode.OK, new { success = true, ErrorMessage = _errorMessage });
                return response;
            }
            else if (_details.pos_type == E_PrinterType.tring.ToString())
            {

                 KasaOdgovor kasaOdgovor = Tring_Invoice.ReclaimInvoice(model);
                if (kasaOdgovor.VrstaOdgovora == VrsteOdgovora.OK)
                {
                    string _errorMessage = string.Empty;
                    try
                    {


                        DateTime _outTemp;


                        string stormed_time = string.Empty;
                        string stormed_date = string.Empty;
                        string stormed_number = string.Empty;
                        string stormed_amount = string.Empty;

                        foreach (Odgovor item in kasaOdgovor.Odgovori)
                        {
                            if (item.Naziv == "BrojFiskalnogRacuna")
                            {
                                stormed_number = item.Vrijednost.ToString();
                            }
                            else if (item.Naziv == "DatumFiskalnogRacuna")
                            {
                                if (DateTime.TryParse(item.Vrijednost.ToString(), out _outTemp))
                                {
                                    stormed_date = _outTemp.ToString("yyyy-MM-dd");
                                }
                            }
                            else if (item.Naziv == "VrijemeFiskalnogRacuna")
                            {
                                if (DateTime.TryParse(item.Vrijednost.ToString(), out _outTemp))
                                {
                                    stormed_time = _outTemp.ToString("hh:mm[:ss]");
                                }
                            }
                            else if (item.Naziv == "IznosFiskalnogRacuna")
                            {
                                stormed_amount = item.Vrijednost.ToString();
                            }

                        }
                        _errorMessage = GlobalCallBackViewModel.ReclaimInvoicePrintCallBackURL(model, _details.uuid, model.user_id, model.destination, model.id.ToString(), stormed_date, stormed_time, stormed_number, stormed_amount);
                    }
                    catch (Exception ex)
                    {
                        _errorMessage = ex.Message;
                    }
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true, obj = kasaOdgovor, ErrorMessage = _errorMessage });
                }else
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, obj = new { error = kasaOdgovor.Odgovori[0].Naziv, code = kasaOdgovor.Odgovori[0].Vrijednost }, ErrorMessage = "" });
                }
            }
            return response;
        }


        [Route("api/cashinout")]
        [HttpPost]
        public HttpResponseMessage cashinout(HttpRequestMessage request, CashInOutViewModel model)
        {

            HttpResponseMessage response = null;
            ClientDetailsViewModel _details = ClientDetailsViewModel.GetClientDetails();
            if (_details == null)
            {
                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, ErrorMessage = "Missing configurations !" });
                return response;
            }
            if (model == null || string.IsNullOrEmpty(model.type) || model.amount == 0)
            {
                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, ErrorMessage = "Invalid input !" });
                return response;
            }

            if (_details.pos_type == E_PrinterType.datecsv1.ToString() || _details.pos_type == E_PrinterType.datecsv2.ToString() || _details.pos_type == E_PrinterType.datecsv3.ToString())
            {
                // GetCashInOutCommand
                SaveCommandToFile(ApplicationHelper.DatecsFolderPath, Datecs_GenerateCommands.GetCashInOutCommand(_details.pos_type, model.type, model.amount));
                string _errorMessage = string.Empty;
                try
                {
                    _errorMessage = GlobalCallBackViewModel.CashInOutCallBackURL(_details.uuid, model.user_id, model.amount, model.type);
                }
                catch (Exception ex)
                {
                    _errorMessage = ex.Message;
                }
                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, ErrorMessage = _errorMessage });
                return response;
            }
            else if (_details.pos_type == E_PrinterType.tring.ToString())
            {
                KasaOdgovor kasaOdgovor = Tring_CashInOut.CashInOut(model.type, model.amount);
                if (kasaOdgovor.VrstaOdgovora == VrsteOdgovora.OK)
                {
                    string _errorMessage = string.Empty;
                    try
                    {
                        _errorMessage = GlobalCallBackViewModel.CashInOutCallBackURL(_details.uuid, model.user_id, model.amount, model.type);
                    }
                    catch (Exception ex)
                    {
                        _errorMessage = ex.Message;
                    }
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true, obj = kasaOdgovor, ErrorMessage = _errorMessage });
                    return response;
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, obj = new { error = kasaOdgovor.Odgovori[0].Naziv, code = kasaOdgovor.Odgovori[0].Vrijednost }, ErrorMessage = "" });
                    return response;
                }
            }
            response = request.CreateResponse(HttpStatusCode.OK, new { success = false, ErrorMessage = "Missing printer type !" });
            return response;
        }

        [Route("api/report")]
        [HttpPost]
        public HttpResponseMessage report(HttpRequestMessage request, ReportViewModel model)
        {

            HttpResponseMessage response = null;
            ClientDetailsViewModel _details = ClientDetailsViewModel.GetClientDetails();
            if (_details == null)
            {
                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, ErrorMessage = "Missing configurations !" });
                return response;
            }
            if (model == null || string.IsNullOrEmpty(model.reportType))
            {
                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, ErrorMessage = "Invalid input !" });
                return response;
            }
            E_ReportType _type = (E_ReportType)Enum.Parse(typeof(E_ReportType), model.reportType, true);

            if (_type == E_ReportType.Y)
            {
                if (model.date_from == null || model.date_to == null)
                {
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true, ErrorMessage = "[From Date] and [To Date] are required for Y Report" });
                    return response;
                }

            }
            if (_details.pos_type == E_PrinterType.datecsv1.ToString() || _details.pos_type == E_PrinterType.datecsv2.ToString() || _details.pos_type == E_PrinterType.datecsv3.ToString())
            {
                if (_type == E_ReportType.Z)
                {
                    LoadItems(_details.pos_type, _details.pos_destination_urls.pos_articles, _details.uuid);
                }

                // Print
                SaveCommandToFile(ApplicationHelper.DatecsFolderPath, Datecs_GenerateCommands.Print(_details.pos_type, _type, model.date_from, model.date_to));
                string _errorMessage = string.Empty;
                try
                {
                    _errorMessage = GlobalCallBackViewModel.PrintReportCallbackURL(_details.uuid, model.user_id, _type.ToString());
                }
                catch (Exception ex)
                {
                    _errorMessage = ex.Message;
                }
                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, ErrorMessage = _errorMessage });
                return response;
            }
            else if (_details.pos_type == E_PrinterType.tring.ToString())
            {
                KasaOdgovor kasaOdgovor = Tring_Report.Print(_type, model.date_from, model.date_to);
                if (kasaOdgovor.VrstaOdgovora == VrsteOdgovora.OK)
                {
                    string _errorMessage = string.Empty;
                    try
                    {
                        _errorMessage = GlobalCallBackViewModel.PrintReportCallbackURL(_details.uuid, model.user_id, _type.ToString());
                    }
                    catch (Exception ex)
                    {
                        _errorMessage = ex.Message;
                    }
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true, obj = kasaOdgovor, ErrorMessage = _errorMessage });
                }else
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, obj = new { error = kasaOdgovor.Odgovori[0].Naziv, code = kasaOdgovor.Odgovori[0].Vrijednost }, ErrorMessage = "" });
                }
                return response;
            }
            response = request.CreateResponse(HttpStatusCode.OK, new { success = false, ErrorMessage = "Missing printer type !" });
            return response;
        }

        [Route("api/test")]
        [HttpGet]
        public HttpResponseMessage test(HttpRequestMessage request)
        {

            HttpResponseMessage response = null;
            bool isOnline = Tring_Report.CheckIfPrinterOnline();
            response = request.CreateResponse(HttpStatusCode.OK, new { success = true, IsOnline = isOnline });
            return response;
        }


        public void LoadItems(string printerType, string url, string uuid)
        {
            // GenerateLoadItems
            SaveCommandToFile(ApplicationHelper.DatecsArticlesFolderPath, Datecs_GenerateCommands.GenerateLoadItems(url, uuid));
            // LoadItemCommand
            SaveCommandToFile(ApplicationHelper.DatecsFolderPath, Datecs_GenerateCommands.LoadItemCommand(printerType, ApplicationHelper.DatecsArticlesFolderPath));
        }


        public void SaveCommandToFile(string folderPath, string commandText)
        {
            if (File.Exists(folderPath))
            {
                File.Delete(folderPath);
            }
            using (StreamWriter writer = File.CreateText(folderPath))
            {
                writer.WriteLine(commandText);
            }
        }

    }
}
