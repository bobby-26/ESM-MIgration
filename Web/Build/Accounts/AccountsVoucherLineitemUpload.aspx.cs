using OfficeOpenXml;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;


public partial class AccountsVoucherLineitemUpload : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Upload", "UPLOAD",ToolBarDirection.Right);
            MenuVoucherUpload.AccessRights = this.ViewState;
            MenuVoucherUpload.Title = "Voucher Upload";
            MenuVoucherUpload.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
               

                if (Request.QueryString["VoucherId"] != "")
                    ViewState["VoucherId"] = Request.QueryString["VoucherId"];
                ViewState["PAGENUMBER"] = 1;
                gvVoucherUpload.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuVoucherUpload_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToString().ToUpper() == "UPLOAD")
        {
            try
            {
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(txtFileUpload.FileName.ToString());

                if (extension.ToUpper() == ".XLSX")
                {
                    string strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts/" + fileName + extension;
                    //string strpath = HttpContext.Current.Server.MapPath("~/Attachments/Accounts/") + fileName + extension;
                    txtFileUpload.PostedFile.SaveAs(strpath);
                    bool importstatus = false;
                    CheckImportfile(strpath, ref importstatus);

                    if (importstatus)
                    {
                        VoucherExcelUpload(strpath, fileName);
                    }
                    else
                    {
                        ucError.ErrorMessage = "Please upload correct file with data.";
                        ucError.Visible = true;
                        return;
                    }
                }
                else
                {
                    ucError.ErrorMessage = "Please upload .xlsx file only";
                    ucError.Visible = true;
                    return;
                }
                gvVoucherUpload.Rebind();
            }
            
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }

    private void CheckImportfile(string filepath, ref bool importstatus)
    {
        if (File.Exists(filepath))
            importstatus = true;
        else
            importstatus = false;
    }

    public void VoucherExcelUpload(string filepath, string FileGuidName)
    {
        try
        {
            string patchid = Guid.NewGuid().ToString();
            DateTime maxOBCModifiedDate = new DateTime();

            List<string> cellvalues = new List<string>();
            var package = new ExcelPackage(new FileInfo(filepath));
            ExcelWorksheets worksheets = package.Workbook.Worksheets;

            if (worksheets.Count > 0)
            {
                foreach (ExcelWorksheet workSheet in worksheets)
                {
                    if (workSheet.Dimension != null && workSheet.ToString() == "TBLOWNERBUDGETCODEMAP")
                    {
                        if (workSheet.Cells[1, 11].Value != null && workSheet.Cells[1, 11].Text.ToString() != string.Empty)
                            maxOBCModifiedDate = Convert.ToDateTime(workSheet.Cells[1, 11].Text.ToString());
                    }
                }
            }


            if (worksheets.Count > 0)
            {
                foreach (ExcelWorksheet workSheet in worksheets)
                {
                    if (workSheet.Dimension != null && workSheet.ToString() == "|Data")
                    {
                        int endRow = 1;

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            if (workSheet.Cells[i, 1].Value == null)
                            {
                                //endRow = i;
                                break;
                            }
                            endRow = i;
                        }

                        for (int j = workSheet.Dimension.Start.Column; j <= 14; j++)
                        {
                            if (!workSheet.Cells[1, j].Value.ToString().Equals(""))
                                cellvalues.Add(Convert.ToString(workSheet.Cells[1, j].Value));
                        }

                        if (!VerifyHeaders(cellvalues))
                        {
                            ucError.Visible = true;
                            return;
                        }

                        cellvalues.Clear();

                        decimal? baseExchangeRate = General.GetNullableDecimal(workSheet.Cells[2, 5].Value == null ? null : workSheet.Cells[2, 5].Value.ToString());
                        decimal? reportExchangeRate = General.GetNullableDecimal(workSheet.Cells[2, 6].Value == null ? null : workSheet.Cells[2, 6].Value.ToString());
                        decimal? amount = General.GetNullableDecimal(workSheet.Cells[2, 8].Value == null ? null : workSheet.Cells[2, 8].Value.ToString());

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= endRow; i++)
                        {
                            for (int j = workSheet.Dimension.Start.Column; j <= 14; j++)
                            {
                                if (!workSheet.Cells[i, 1].Value.ToString().Equals(""))
                                    cellvalues.Add(Convert.ToString(workSheet.Cells[i, j].Value));
                            }

                            baseExchangeRate = General.GetNullableDecimal(workSheet.Cells[i, 5].Value == null ? null : workSheet.Cells[i, 5].Value.ToString());
                            reportExchangeRate = General.GetNullableDecimal(workSheet.Cells[i, 6].Value == null ? null : workSheet.Cells[i, 6].Value.ToString());
                            amount = General.GetNullableDecimal(workSheet.Cells[i, 8].Value == null ? null : workSheet.Cells[i, 8].Value.ToString());

                            if (baseExchangeRate == null)
                            {
                                ucError.ErrorMessage = "Base Exchange Rate in row [" + i.ToString() + "] is not in correct format";
                                ucError.Visible = true;
                                return;
                            }
                            if (reportExchangeRate == null)
                            {
                                ucError.ErrorMessage = "Report Exchange Rate in row [" + i.ToString() + "] is not in correct format";
                                ucError.Visible = true;
                                return;
                            }
                            if (amount == null)
                            {
                                ucError.ErrorMessage = "Amount in row [" + i.ToString() + "] is not in correct format";
                                ucError.Visible = true;
                                return;
                            }
                            try
                            {
                                VerifyVoucherUpload(cellvalues, i.ToString(), int.Parse(ViewState["VoucherId"].ToString()));
                            }
                            catch (Exception EX)
                            {
                                ucError.ErrorMessage = EX.Message;
                                ucError.Visible = true;
                                return;
                            }
                            cellvalues.Clear();
                        }
                        
                        HttpFileCollection postedFiles = Request.Files;
                        HttpPostedFile postedFile = postedFiles[0];

                        string filename = postedFile.FileName;
                        if (filename.LastIndexOfAny(new[] { '/', '\\' }) > 0)
                        {
                            filename = filename.Substring(filename.LastIndexOfAny(new[] { '/', '\\' }) + 1);
                        }
                        filepath = "ACCOUNTS/" + FileGuidName + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));

                        Guid? voucherUploadId = InsertVoucherUpload(int.Parse(ViewState["VoucherId"].ToString()), filename, General.GetNullableDateTime(maxOBCModifiedDate.ToString()));

                        PhoenixCommonFileAttachment.InsertAttachment(
                            new Guid(voucherUploadId.ToString()), filename, filepath, postedFile.ContentLength
                                , 0
                                , 0, "VOUCHERUPLOAD", new Guid(voucherUploadId.ToString()));

                        if (voucherUploadId == null)
                        {
                            ucError.ErrorMessage = "Document upload failed.";
                            ucError.Visible = true;
                            return;
                        }

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= endRow; i++)
                        {
                            for (int j = workSheet.Dimension.Start.Column; j <= 14; j++)
                            {
                                if (!workSheet.Cells[i, 1].Value.ToString().Equals(""))
                                    cellvalues.Add(Convert.ToString(workSheet.Cells[i, j].Value));
                            }

                            //cellvalues.Add(Convert.ToString(workSheet.Cells[1, 1].Value));
                            InsertVoucherUploadLineItems(cellvalues, new Guid(voucherUploadId.ToString()), int.Parse(ViewState["VoucherId"].ToString()),i);
                            cellvalues.Clear();
                        }

                        ucStatus.Text = "Voucher Uploaded Successfully.";
                        ucStatus.Visible = true;
                        BindData();

                    }
                }
            }
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

    private bool VerifyHeaders(List<string> li)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!li[0].ToString().ToUpper().Equals("ACCOUNT CODE"))
            ucError.ErrorMessage = "Cannot found column header Account Code.";

        if (!li[1].ToString().ToUpper().Equals("ACCOUNT DESCRIPTION"))
            ucError.ErrorMessage = "Cannot found column header Account Description.";

        if (!li[2].ToString().ToUpper().Equals("ESM BUDGET CODE"))
            ucError.ErrorMessage = "Cannot found column header Budget Code.";

        if (!li[3].ToString().ToUpper().Equals("OWNER BUDGET CODE"))
            ucError.ErrorMessage = "Cannot found column header Owner Budget Code.";

        if (!li[4].ToString().ToUpper().Equals("BASE RATE"))
            ucError.ErrorMessage = "Cannot found column header Base Rate.";

        if (!li[5].ToString().ToUpper().Equals("REPORT RATE"))
            ucError.ErrorMessage = "Cannot found column header Report Rate.";

        if (!li[6].ToString().ToUpper().Equals("CURRENCY"))
            ucError.ErrorMessage = "Cannot found column header Currency.";

        if (!li[7].ToString().ToUpper().Equals("AMOUNT"))
            ucError.ErrorMessage = "Cannot found column header Amount.";

        if (!li[8].ToString().ToUpper().Equals("BASE AMOUNT"))
            ucError.ErrorMessage = "Cannot found column header Base Amount.";

        if (!li[9].ToString().ToUpper().Equals("REPORT AMOUNT"))
            ucError.ErrorMessage = "Cannot found column header Report Amount.";

        if (!li[10].ToString().ToUpper().Equals("LONG DESCRIPTION"))
            ucError.ErrorMessage = "Cannot found column header Long Description.";

        if (!li[11].ToString().ToUpper().Equals("HIDDEN FROM OWNER"))
            ucError.ErrorMessage = "Cannot found column header Hidden from Owner.";

        if (!li[12].ToString().ToUpper().Equals("OWNER ID"))
            ucError.ErrorMessage = "Cannot found column header Owner ID.";

        if (!li[13].ToString().ToUpper().Equals("OWNER BUDGET CODE MAP ID"))
            ucError.ErrorMessage = "Cannot found column header Owner Budget Code Map ID.";

        return (!ucError.IsError);
    }

    private void VerifyVoucherUpload(List<string> li, string rowno, int VoucherId)
    {
        PhoenixAccountsVoucherUpload.VerifyVoucherUpload(rowno, VoucherId, li[0].ToString(), li[2].ToString(), li[3].ToString(), Convert.ToDecimal(li[4].ToString()),
            Convert.ToDecimal(li[5].ToString()), li[6].ToString(), Convert.ToDecimal(li[7].ToString()), General.GetNullableInteger(li[11].ToString()),General.GetNullableGuid(li[13].ToString()));

    }

    private Guid? InsertVoucherUpload(int voucherId, string filename, DateTime? maxOBCModifiedDate)
    {
        Guid? voucherUploadId = null;

        PhoenixAccountsVoucherUpload.VoucherUploadInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, voucherId, filename, ref voucherUploadId, maxOBCModifiedDate);

        return voucherUploadId;
    }

    private void InsertVoucherUploadLineItems(List<string> li, Guid voucherUploadId, int voucherId, int rowno)
    {
        try
        {
            PhoenixAccountsVoucherUpload.VoucherUploadLineItemInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , voucherUploadId
                , voucherId
                , rowno
                , li[0].ToString()   //Account code
                , li[2].ToString()   //Budget code
                , li[3].ToString()   //Owner budget code
                , Convert.ToDecimal(li[4].ToString())    //Base exchange rate
                , Convert.ToDecimal(li[5].ToString())    //Report exchange rate
                , li[6].ToString()                       //Currency
                , Convert.ToDecimal(li[7].ToString())    //Amount
                , li[10].ToString()   //Long desc
                , int.Parse(li[11].ToString())            //Hidden From Owner
                , General.GetNullableGuid(li[13].ToString())    //Owner budget code id
                );
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();

            ds = PhoenixAccountsVoucherUpload.VoucherUploadList(int.Parse(ViewState["VoucherId"].ToString()),
                                                                int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                gvVoucherUpload.PageSize,
                                                                ref iRowCount, ref iTotalPageCount);

            //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
                gvVoucherUpload.DataSource = ds;
            gvVoucherUpload.VirtualItemCount = iRowCount;
                //gvVoucherUpload.DataBind();
           // }
            //else if (ds.Tables.Count > 0)
            //{
            //    ShowNoRecordsFound(ds.Tables[0], gvVoucherUpload);
            //}
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

 
    protected void gvVoucherUpload_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
        

            if (e.CommandName.ToUpper().Equals("POST"))
            {
                PhoenixAccountsVoucherUpload.VoucherUploadPost(new Guid(((RadLabel)e.Item.FindControl("lblVoucheruploadId")).Text)
                                                                , int.Parse(ViewState["VoucherId"].ToString())
                                                                , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                BindData();
                ucStatus.Text = "Voucher Posted.";

                string script = "javascript:fnReloadList('codehelp1');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsVoucherUpload.VoucherUploadDelete(new Guid(((RadLabel)e.Item.FindControl("lblVoucheruploadId")).Text)
                                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                BindData();
                gvVoucherUpload.Rebind();
                ucStatus.Text = "Voucher Upload Discared.";
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

    protected void gvVoucherUpload_RowDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem )
        {
        }
    }



    protected void gvVoucherUpload_RowDeleting(object sender, GridCommandEventArgs e)
    {
    }

    protected void lnkDownloadExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string file = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "\\Accounts\\VoucherLinesUpload.xlsx";// HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts") + @"\Accounts\VoucherLinesUpload.xlsx";
            string destinationpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "\\Accounts\\" + Guid.NewGuid().ToString() + ".xlsx";

            FileInfo fiTemplate = new FileInfo(file);
            fiTemplate.CopyTo(destinationpath);

            FileInfo fidestination = new FileInfo(destinationpath);

            using (ExcelPackage pck = new ExcelPackage(fidestination))
            {
                //PopulateAccountList(pck);
                //PopulateOwnerBudgetCode(pck);
                HttpContext.Current.Response.Clear();
                pck.SaveAs(HttpContext.Current.Response.OutputStream);
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12 .xltm";
                //HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=VoucherLinesUpload.xlsx");
                HttpContext.Current.Response.End();
            }
                
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private static void PopulateAccountList(ExcelPackage pck)
    //{
    //    int iRowCount = 0;
    //    int iTotalPageCount = 0;

    //    ExcelWorksheet ws = pck.Workbook.Worksheets["TBLACCOUNT"];

    //    DataSet ds = PhoenixRegistersAccount.AccountSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, null, null, null, null, null
    //            , 1, null, null, 1, 1000, ref iRowCount, ref iTotalPageCount);

    //    DataRow drReq = ds.Tables[0].Rows[0]; //dt.Rows[0];

    //    string[] alColumns = { "FLDMODIFIEDDATE", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDACCOUNTID", "FLDACCOUNTUSAGE" };

    //    for (int i = 1; i <= alColumns.Length; i++)
    //    {
    //        ws.Cells[1, i].Value = alColumns[i - 1].ToString();
    //    }

    //    int nRow = 2;

    //    foreach (DataRow dr in ds.Tables[0].Rows)
    //    {
    //        ws.Cells[nRow, 1].Value = ds.Tables[0].Rows[nRow - 2]["FLDMODIFIEDDATE"];
    //        ws.Cells[nRow, 2].Value = ds.Tables[0].Rows[nRow - 2]["FLDACCOUNTCODE"];
    //        ws.Cells[nRow, 3].Value = ds.Tables[0].Rows[nRow - 2]["FLDDESCRIPTION"];
    //        ws.Cells[nRow, 4].Value = ds.Tables[0].Rows[nRow - 2]["FLDACCOUNTID"];
    //        ws.Cells[nRow, 5].Value = ds.Tables[0].Rows[nRow - 2]["FLDACCOUNTUSAGE"];
    //        nRow = nRow + 1;
    //    }
    //}

    //private static void PopulateOwnerBudgetCode(ExcelPackage pck)
    //{
    //    int iRowCount = 0;
    //    int iTotalPageCount = 0;
    //    int iownerid = 0;

    //    ExcelWorksheet ws = pck.Workbook.Worksheets["TBLOWNERBUDGETCODEMAP"];
    //    DataSet ds = PhoenixCommonRegisters.OwnerBudgetCodeSearch("",null, null, null,null,null,null
    //            , 1, 10000, ref iRowCount, ref iTotalPageCount, ref iownerid);

    //    DataRow drReq = ds.Tables[0].Rows[0]; //dt.Rows[0];

    //    string[] alColumns = { "FLDOWNERBUDGETCODEMAPID", "FLDOWNERID", "Sub Account", "FLDNOTINCULDEINOWNERREPORTYN", "FLDOWNERBUDGETGROUPID" ,"Owner Code", };

    //    for (int i = 1; i <= alColumns.Length; i++)
    //    {
    //        ws.Cells[1, i].Value = alColumns[i - 1].ToString();
    //    }

    //    int nRow = 2;

    //    foreach (DataRow dr in ds.Tables[0].Rows)
    //    {
    //        ws.Cells[nRow, 1].Value = ds.Tables[0].Rows[nRow - 2]["FLDOWNERBUDGETCODEMAPID"];
    //        ws.Cells[nRow, 2].Value = ds.Tables[0].Rows[nRow - 2]["FLDOWNERID"];
    //        ws.Cells[nRow, 3].Value = ds.Tables[0].Rows[nRow - 2]["FLDSUBACCOUNT"];
    //        ws.Cells[nRow, 4].Value = ds.Tables[0].Rows[nRow - 2]["FLDNOTINCULDEINOWNERREPORTYN"];
    //        ws.Cells[nRow, 5].Value = ds.Tables[0].Rows[nRow - 2]["FLDOWNERBUDGETGROUPID"];
    //        ws.Cells[nRow, 6].Value = ds.Tables[0].Rows[nRow - 2]["FLDOWNERBUDGETCODE"] + " - " + ds.Tables[0].Rows[nRow - 2]["FLDOWNERCODEDESCRIPTION"];
    //        nRow = nRow + 1;
    //    }
    //}

    protected void gvVoucherUpload_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVoucherUpload.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
