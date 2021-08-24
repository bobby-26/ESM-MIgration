using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using SouthNests.Phoenix.Framework;
using System.Collections.Generic;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Globalization;
using System.Text;
using Telerik.Web.UI;

public partial class CrewTravelAgentInvoiceUpload : PhoenixBasePage
{
    DataSet ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("javascript:showDialog();", "Notes", "<i class=\"fas fa-info-circle\"></i>", "NOTES");
        toolbar.AddButton("Confirm", "CONFIRM", ToolBarDirection.Right);
        toolbar.AddButton("Refresh", "REFRESH", ToolBarDirection.Right);
        MenutravelInvoice.AccessRights = this.ViewState;
        MenutravelInvoice.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["INVOICENO"] = null;
            ViewState["PATHID"] = null;
            ViewState["FILEPATH"] = null;
            ViewState["FILENAME"] = null;
            ViewState["FILEEXTENTION"] = null;
            ViewState["INSERT"] = "0";
            ViewState["NORECORDYN"] = "0";

            Refresh();

            gvInvoiceUploadBulk.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }

    protected void FileUploadInvoice_FileUploaded(object sender, FileUploadedEventArgs e)
    {

        Refresh();

        string fileName = Guid.NewGuid().ToString();
        ViewState["FILENAME"] = fileName;

        UploadedFileCollection fileUpload = FileUploadInvoice.UploadedFiles;

        string extension = Path.GetExtension(e.File.GetName());

        if (extension.ToUpper() == ".XLSX")
        {
            //string strpath = HttpContext.Current.Server.MapPath("~/Attachments/Crew/") + fileName + extension;

            string attachmentpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/TEMP/";

            if (Directory.Exists(attachmentpath))
            {
                string strpath = attachmentpath + fileName + extension;

                ViewState["FILEPATH"] = strpath;

                for (int i = 0; i < fileUpload.Count; i++)
                {
                    UploadedFile postedFile = fileUpload[i];
                    if (postedFile.ContentLength > 0)
                    {
                        e.File.SaveAs(strpath);
                    }

                }

                bool importstatus = false;

                string extension1 = Path.GetExtension(e.File.GetName());
                ViewState["FILEEXTENTION"] = extension1;

                for (int i = 0; i < fileUpload.Count; i++)
                {
                    UploadedFile postedFile = fileUpload[i];
                    if (postedFile.ContentLength > 0)
                    {
                        CheckImportfile(strpath, ref importstatus);
                        if (importstatus)
                        {
                            InvoiceUpload(strpath);
                            BindData();
                            gvInvoiceUploadBulk.Rebind();
                        }
                        else
                        {
                            ucError.ErrorMessage = "Please upload correct file with data.";
                            ucError.Visible = true;
                            BindData();
                            gvInvoiceUploadBulk.Rebind();
                            return;
                        }
                    }
                }
            }
        }
        else
        {
            ucError.ErrorMessage = "Please upload .xlsx file only";
            ucError.Visible = true;
            BindData();
            return;
        }
    }

    protected void MenutravelInvoice_OnTabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToString().ToUpper() == "REFRESH")
            {
                try
                {
                    Refresh();
                    ucStatus.Text = "Refreshed";
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            if (CommandName.ToString().ToUpper() == "CONFIRM")
            {
                if (ViewState["NORECORDYN"].ToString() == "1")
                {
                    ucError.ErrorMessage = "Please upload valid invoice and confirm.";
                    ucError.Visible = true;
                    return;
                }
                else if (ViewState["NORECORDYN"].ToString() == "0")
                {
                    DataRow dr = ds.Tables[1].Rows[0];
                    int? invalidticketcount = General.GetNullableInteger(dr["FLDINAVLIDTICKETYNCNT"].ToString());
                    int? duplicateticketuploaded = General.GetNullableInteger(dr["FLDDUPLICATEINVOICEUPLOADYN"].ToString());
                    int? invdate3daysfromdepdate = General.GetNullableInteger(dr["FLD3DAYSFROMDEPDATE"].ToString());
                    int? invalidponumber = General.GetNullableInteger(dr["FLDINVLIDPONUMBERYN"].ToString());
                    int? zeroamount = General.GetNullableInteger(dr["FLDINVAMOUNTZERO"].ToString());
                    int? invoicepostedyn = General.GetNullableInteger(dr["FLDINVOICEPOSTEDYN"].ToString());
                    int? commitedyn = General.GetNullableInteger(dr["FLDCOMMITEDYN"].ToString());

                    if (invalidticketcount > 0 || duplicateticketuploaded > 0 || invdate3daysfromdepdate > 0 || invalidponumber > 0 || invoicepostedyn > 0 || commitedyn > 0)
                    {
                        if (invalidticketcount > 0)
                            ucError.ErrorMessage = "Invalid Ticket No. Occured";
                        if (duplicateticketuploaded > 0)
                            ucError.ErrorMessage = "Duplicate Ticket No. Occured";
                        if (invoicepostedyn > 0)
                            ucError.ErrorMessage = "Ticket No. Already Posted";
                        if (invalidponumber > 0)
                            ucError.ErrorMessage = "Invalid Request No. Occured";
                        if (invdate3daysfromdepdate > 0)
                            ucError.ErrorMessage = "Invoice Date should be 3 days later than Depature Date";
                        if (zeroamount > 0)
                            ucError.ErrorMessage = "Zero Amount Ticket Occured";
                        if (commitedyn > 0)
                            ucError.ErrorMessage = "Ticket Not Confirmed";

                        ucError.ErrorMessage = "       ";
                        ucError.ErrorMessage = "   Please correct and upload again.";
                        ucError.Visible = true;
                        return;
                    }
                    else
                    {
                        string fileName = Guid.NewGuid().ToString();

                        ViewState["INSERT"] = "1";
                        if (ViewState["FILEPATH"] != null & ViewState["FILENAME"] != null)
                        {
                            InvoiceUploadMain(ViewState["FILEPATH"].ToString(), ViewState["FILENAME"].ToString());
                            ucStatus.Text = "file Uploaded successfully";
                        }
                        Refresh();
                        BindData();
                        gvInvoiceUploadBulk.Rebind();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void lnkDownloadExcel_Click(object sender, EventArgs e)
    {
        try
        {
            int iRowCount = 0;
            string[] alColumns = { "INVOICENUMBER", "INVOICEDATE" , "REQUISITIONNO","PNR","DEPATUREDATE", "PASSENGERNAME", "VESSEL", "AIRLINENUMBER", "SECTOR1", "SECTOR2", "SECTOR3", "SECTOR4"
                                 ,"TICKETNO","BASIC","TOTALTAX","STXCOLLECTED","DISCOUNT","CANCELLATIONCHARGES","CREDITNOTEDATE","TOTAL","REMARKS"};
            string[] alCaptions = { "Invoice Number", "Invoice Date(DD/MM/YYYY)","Requisition No", "PNR", "Depature Date(DD/MM/YYYY)", "Passenger Name", "Vessel","Airline Number","Sector1","Sector2","Sector3","Sector4"
                                 ,"Ticket Number","Basic","Total Tax","STX Colllected","Discount","Cancellation Charges","Credit Note Date","Total","Remarks"};

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int sortdirection;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            else
                sortdirection = 0;
            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataTable dt = GetTable();
            
            ExportExcel("Travel Agent Invoice", dt, alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private DataTable GetTable()
    {
        DataTable table = new DataTable();

        table.Columns.Add("INVOICENUMBER", typeof(string));
        table.Columns.Add("INVOICEDATE", typeof(string));
        table.Columns.Add("REQUISITIONNO", typeof(string));
        table.Columns.Add("PNR", typeof(string));
        table.Columns.Add("DEPATUREDATE", typeof(string));
        table.Columns.Add("PASSENGERNAME", typeof(string));
        table.Columns.Add("VESSEL", typeof(string));
        table.Columns.Add("AIRLINENUMBER", typeof(string));
        table.Columns.Add("SECTOR1", typeof(string));
        table.Columns.Add("SECTOR2", typeof(string));
        table.Columns.Add("SECTOR3", typeof(string));
        table.Columns.Add("SECTOR4", typeof(string));
        table.Columns.Add("TICKETNO", typeof(string));
        table.Columns.Add("BASIC", typeof(string));
        table.Columns.Add("TOTALTAX", typeof(string));
        table.Columns.Add("STXCOLLECTED", typeof(string));
        table.Columns.Add("DISCOUNT", typeof(string));
        table.Columns.Add("CANCELLATIONCHARGES", typeof(string));
        table.Columns.Add("CREDITNOTEDATE", typeof(string));
        table.Columns.Add("TOTAL", typeof(string));
        table.Columns.Add("REMARKS", typeof(string));


        return table;
    }

    private void CheckImportfile(string filepath, ref bool importstatus)
    {
        if (File.Exists(filepath))
            importstatus = true;
        else
            importstatus = false;
    }
    private DateTime? GetNullableDateTimeFormat(string datetime)
    {
        StringBuilder str = new StringBuilder();
        DateTime dtout;
        string strdate = ConvertToDateTime(datetime);
        DateTime? dt = null;
        if (DateTime.TryParse(strdate, out dtout))
        {
            dt = dtout;
        }

        return dt;
    }
    public static string ConvertToDateTime(string strExcelDate)
    {
        double excelDate;
        try
        {
            excelDate = Convert.ToDouble(strExcelDate);
        }
        catch
        {
            return strExcelDate;
        }
        if (excelDate < 1)
        {
            throw new ArgumentException("Excel dates cannot be smaller than 0.");
        }
        DateTime dateOfReference = new DateTime(1900, 1, 1);
        if (excelDate > 60d)
        {
            excelDate = excelDate - 2;
        }
        else
        {
            excelDate = excelDate - 1;
        }
        return dateOfReference.AddDays(excelDate).ToShortDateString();
    }

    public void InvoiceUpload(string filepath)
    {
        try
        {
            string patchid = Guid.NewGuid().ToString();
            string extension = ViewState["FILEEXTENTION"].ToString();

            List<string> cellvalues = new List<string>();

            var package = new ExcelPackage(new FileInfo(filepath));

            ExcelWorksheets worksheets = package.Workbook.Worksheets;

            if (worksheets.Count > 0)
            {
                foreach (ExcelWorksheet workSheet in worksheets)
                {
                    if (workSheet.Dimension != null)
                    {
                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            if (workSheet.Dimension.End.Column != 21)
                            {
                                ucError.ErrorMessage = "Please upload correct file with data.";
                                ucError.Visible = true;
                                return;
                            }
                            for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                            {
                                if (j == 14 || j == 15 || j == 16 || j == 18 || j == 20)
                                {
                                    string StrVal = workSheet.Cells[i, j].Text.Replace("(", "");
                                    StrVal = StrVal.Replace(")", "");
                                    StrVal = StrVal.Replace(",", "");
                                    cellvalues.Add(StrVal);
                                }
                                else
                                    cellvalues.Add(Convert.ToString(workSheet.Cells[i, j].Text));
                            }
                            if (ViewState["INSERT"].ToString() == "1")
                            {
                                InsertInvoice(cellvalues, new Guid(patchid));

                            }
                            else
                                InsertInvoiceBulk(cellvalues, new Guid(patchid));
                            cellvalues.Clear();
                        }
                    }
                }
                ViewState["INSERT"] = "0";
            }
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

    public void InvoiceUploadMain(string filepath, string filename)
    {
        try
        {
            string patchid = filename;

            List<string> cellvalues = new List<string>();

            var package = new ExcelPackage(new FileInfo(filepath));

            ExcelWorksheets worksheets = package.Workbook.Worksheets;

            if (worksheets.Count > 0)
            {
                foreach (ExcelWorksheet workSheet in worksheets)
                {
                    if (workSheet.Dimension != null)
                    {
                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {

                            for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                            {
                                if (j == 14 || j == 15 || j == 16 || j == 18 || j == 20)
                                {
                                    string StrVal = workSheet.Cells[i, j].Text.Replace("(", "");
                                    StrVal = StrVal.Replace(")", "");
                                    StrVal = StrVal.Replace(",", "");
                                    cellvalues.Add(StrVal);
                                }
                                else
                                    cellvalues.Add(Convert.ToString(workSheet.Cells[i, j].Text));
                            }
                            InsertInvoice(cellvalues, new Guid(patchid));
                            cellvalues.Clear();
                        }
                    }
                }
                ViewState["INSERT"] = "0";
            }
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixCrewTravelInvoice.CrewTravelAgentInvoiceBulkSearch(
                    sortexpression, sortdirection,
                    (int)ViewState["PAGENUMBER"],
                    gvInvoiceUploadBulk.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

            gvInvoiceUploadBulk.DataSource = ds;
            gvInvoiceUploadBulk.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["PATHID"] = dr["FLDPATHID"].ToString();
                ViewState["NORECORDYN"] = "0";
            }
            else
            {
                ViewState["NORECORDYN"] = "1";
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void Refresh()
    {
        ViewState["FILEPATH"] = null;
        ViewState["FILEEXTENTION"] = null;
        ViewState["INSERT"] = "0";
        ViewState["NORECORDYN"] = "0";
        PhoenixCrewTravelInvoice.CrewTravelAgentInvoiceBulkDelete();

        ViewState["PAGENUMBER"] = 1;
        gvInvoiceUploadBulk.CurrentPageIndex = 0;
        BindData();
        gvInvoiceUploadBulk.Rebind();
    }

    private void InsertInvoice(List<string> li, Guid patchid)
    {
        try
        {
            PhoenixCrewTravelInvoice.InvoiceUpload
                                        (
                                       General.GetNullableString(li[0].ToString())
                                       , GetNullableDateTimeFormat(li[1].ToString().Trim())
                                       , li[2].ToString().Trim()
                                       , li[3].ToString().Trim()
                                        , GetNullableDateTimeFormat(li[4].ToString().Trim())
                                        , li[5].ToString().Trim()
                                        , li[6].ToString().Trim()
                                        , li[7].ToString().Trim()
                                        , li[8].ToString().Trim()
                                        , li[9].ToString().Trim()
                                        , li[10].ToString().Trim()
                                        , li[11].ToString().Trim()
                                        , li[12].ToString().Trim()
                                        , General.GetNullableDecimal(li[13].ToString().Trim())
                                        , General.GetNullableDecimal(li[14].ToString().Trim())
                                        , General.GetNullableDecimal(li[15].ToString().Trim())
                                        , General.GetNullableDecimal(li[16].ToString().Trim())
                                        , General.GetNullableDecimal(li[17].ToString().Trim())
                                        , General.GetNullableString(li[18].ToString().Trim())
                                        , General.GetNullableDecimal(li[19].ToString().Trim())
                                        , li[20].ToString().Trim()
                                        , patchid
                                        );
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }

    }
    private void InsertInvoiceBulk(List<string> li, Guid patchid)
    {
        try
        {
            if (General.GetNullableString(li[0].ToString()) != null)
            {
                PhoenixCrewTravelInvoice.InvoiceUploadBulk
                                            (
                                             General.GetNullableString(li[0].ToString().Trim())
                                            , GetNullableDateTimeFormat(li[1].ToString().Trim())
                                            , General.GetNullableString(li[2].ToString().Trim())
                                            , li[3].ToString().Trim()
                                            , GetNullableDateTimeFormat(li[4].ToString().Trim())
                                            , li[5].ToString().Trim()
                                            , li[6].ToString().Trim()
                                            , li[7].ToString().Trim()
                                            , li[8].ToString().Trim()
                                            , li[9].ToString().Trim()
                                            , li[10].ToString().Trim()
                                            , li[11].ToString().Trim()
                                            , li[12].ToString().Trim()
                                            , General.GetNullableDecimal(li[13].ToString().Trim())
                                            , General.GetNullableDecimal(li[14].ToString().Trim())
                                            , General.GetNullableDecimal(li[15].ToString().Trim())
                                            , General.GetNullableDecimal(li[16].ToString().Trim())
                                            , General.GetNullableDecimal(li[17].ToString().Trim())
                                            , General.GetNullableString(li[18].ToString().Trim())
                                            , General.GetNullableDecimal(li[19].ToString().Trim())
                                            , li[20].ToString().Trim()
                                            , patchid
                                            );
            }
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }

    }
    private void ExportExcel(string strHeading, DataTable dt, string[] alColumns, string[] alCaptions, int? strSortDirection, string strSortExpression)
    {
        try
        {

            int flag;

            List<string> FldColumns = new List<string>();

            foreach (DataColumn column in dt.Columns)
            {
                flag = 0;
                for (int i = 0; i < alColumns.Length; i++)
                {
                    if (alColumns[i] == column.ColumnName)
                    {
                        flag = 1;
                        column.Caption = alCaptions[i].ToString();
                    }
                }
                if (flag == 0)
                {
                    FldColumns.Add(column.ColumnName);
                }

            }

            for (int i = 0; i < FldColumns.Count; i++)
            {
                dt.Columns.Remove(FldColumns[i].ToString());
            }

            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                //ExcelWorksheet ws = pck.Workbook.Worksheets.Add("TravelAgentInvoice");

                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Travel Agent Invoice");

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(dt, true);

                //prepare the range for the column headers
                string cellRange = "A1:" + Convert.ToChar('A' + dt.Columns.Count - 1) + 1;

                //Format the header for columns
                using (ExcelRange rng = ws.Cells[cellRange])
                {
                    rng.Style.WrapText = false;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid
                    rng.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                    rng.Style.Font.Color.SetColor(Color.White);
                    rng.Worksheet.Cells.AutoFitColumns();
                    rng.Worksheet.Protection.AllowDeleteColumns = false;
                    rng.Worksheet.Protection.AllowInsertColumns = false;
                    rng.Worksheet.Protection.AllowSelectLockedCells = true;
                }

                //prepare the range for the rows
                string rowsCellRange = "A1:" + Convert.ToChar('A' + dt.Columns.Count - 1) + dt.Rows.Count * dt.Columns.Count;

                Byte[] fileBytes = pck.GetAsByteArray();

                //Clear the response
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Cookies.Clear();

                //Add the header & other information
                Response.Cache.SetCacheability(HttpCacheability.Private);
                Response.CacheControl = "private";
                Response.Charset = System.Text.UTF8Encoding.UTF8.WebName;
                Response.ContentEncoding = System.Text.UTF8Encoding.UTF8;
                Response.AppendHeader("Content-Length", fileBytes.Length.ToString());
                Response.AppendHeader("Pragma", "cache");
                Response.AppendHeader("Expires", "60");
                //Response.AppendHeader("Content-Disposition","attachment; " +"filename=\"" + strHeading + ".xlsx\"; " + "size=" + fileBytes.Length.ToString() + "; " +
                //"creation-date=" + DateTime.Now.ToString("R") + "; " +
                //"modification-date=" + DateTime.Now.ToString("R") + "; " +
                //"read-date=" + DateTime.Now.ToString("R"));

                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; " + "filename=\"" + strHeading + ".xlsx\"; " + "size=" + fileBytes.Length.ToString() );

                //  Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.ContentType = "application/vnd.msexcel";

                //Write it back to the client
                Response.BinaryWrite(fileBytes);

                Response.End();
               
            }
        }

        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
        finally
        {

        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvInvoiceUploadBulk.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvInvoiceUploadBulk_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInvoiceUploadBulk.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvInvoiceUploadBulk_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SELECT")
            {
                RadLabel lblAgentInvoiceBulkID = (RadLabel)e.Item.FindControl("lblAgentInvoiceBulkID");
                LinkButton lnkticket = (LinkButton)e.Item.FindControl("lnkTicket");

                if (lblAgentInvoiceBulkID != null)
                    lnkticket.Attributes.Add("onclick", "openNewWindow('Invoice','Invoice'," + "'" + Session["sitepath"] + "/Crew/CrewTravelAgentInvoiceGeneral.aspx?INVOICEID=" + lblAgentInvoiceBulkID.Text
                                                                                         + "&INVOICEBULK=1'); return false;");
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvInvoiceUploadBulk_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblInvalidTktNoYN = (RadLabel)e.Item.FindControl("lblInvalidTktNoYN");
            RadLabel lblInvalidPOYN = (RadLabel)e.Item.FindControl("lblInvalidPOYN");
            RadLabel lblDuplicateTicketNoYN = (RadLabel)e.Item.FindControl("lblDuplicateTicketNoYN");
            RadLabel lblInvoicePosted = (RadLabel)e.Item.FindControl("lblInvoicePosted");
            RadLabel lblDepDateMismatchYN = (RadLabel)e.Item.FindControl("lblDepDateMismatch");
            RadLabel lblAmountExceeded = (RadLabel)e.Item.FindControl("lblAmountExceeded");
            RadLabel lbl3DaysFromDepDate = (RadLabel)e.Item.FindControl("lbl3DaysFromDepDate");
            RadLabel lblCommitedYN = (RadLabel)e.Item.FindControl("lblCommitedYN");

            LinkButton imgIvalidTkt = (LinkButton)e.Item.FindControl("imgIvalidTkt");
            LinkButton imgInvalidPONumber = (LinkButton)e.Item.FindControl("imgInvalidPONumber");
            LinkButton imgDuplicateTicket = (LinkButton)e.Item.FindControl("imgDuplicateTicket");
            LinkButton imgInvDate3daysBefore = (LinkButton)e.Item.FindControl("imgInvDate3daysBefore");
            LinkButton ImgAmountExeeded = (LinkButton)e.Item.FindControl("ImgAmountExeeded");
            LinkButton ImgInvoiceAlreadyPosted = (LinkButton)e.Item.FindControl("ImgInvoiceAlreadyPosted");
            LinkButton ImgCommited = (LinkButton)e.Item.FindControl("ImgCommited");

            if (lblInvalidTktNoYN != null && lblInvalidTktNoYN.Text == "0")
            {
                if (imgIvalidTkt != null)
                    imgIvalidTkt.Visible = false;

                if (lblInvalidPOYN != null && lblInvalidPOYN.Text == "0")
                {
                    if (imgInvalidPONumber != null)
                        imgInvalidPONumber.Visible = false;

                    if (lblAmountExceeded != null && lblAmountExceeded.Text == "1")
                    {
                        if (ImgAmountExeeded != null)
                            ImgAmountExeeded.Visible = true;
                    }
                    if (lblDuplicateTicketNoYN != null && lblDuplicateTicketNoYN.Text == "1")
                    {
                        if (imgDuplicateTicket != null)
                            imgDuplicateTicket.Visible = true;
                    }
                    if (lbl3DaysFromDepDate != null && lbl3DaysFromDepDate.Text == "1")
                    {
                        if (imgInvDate3daysBefore != null)
                            imgInvDate3daysBefore.Visible = true;
                    }
                    if (lblInvoicePosted != null && lblInvoicePosted.Text == "1")
                    {
                        if (ImgInvoiceAlreadyPosted != null)
                            ImgInvoiceAlreadyPosted.Visible = true;
                    }
                }
                else if (lblInvalidPOYN != null && lblInvalidPOYN.Text == "1")
                {
                    if (imgInvalidPONumber != null)
                        imgInvalidPONumber.Visible = true;
                }


            }
            else if (lblInvalidTktNoYN != null && lblInvalidTktNoYN.Text == "1")
            {
                if (imgIvalidTkt != null)
                    imgIvalidTkt.Visible = true;
            }

            if (lblCommitedYN.Text == "" || (lblCommitedYN != null && lblCommitedYN.Text == "1"))
            {
                if (ImgCommited != null)
                    ImgCommited.Visible = true;
            }

            RadLabel lblAgentInvoiceBulkID = (RadLabel)e.Item.FindControl("lblAgentInvoiceBulkID");
            LinkButton lnkticket = (LinkButton)e.Item.FindControl("lnkTicket");

            if (lblAgentInvoiceBulkID != null)
            {

                lnkticket.Attributes.Add("onclick", "openNewWindow('Invoice','Invoice'," + "'" + Session["sitepath"] + "/Crew/CrewTravelAgentInvoiceGeneral.aspx?INVOICEID=" + lblAgentInvoiceBulkID.Text
                                                                                         + "&INVOICEBULK=1'); return false;");
            }

        }
    }

}
