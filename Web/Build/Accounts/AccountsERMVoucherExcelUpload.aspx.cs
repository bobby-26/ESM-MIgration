using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Globalization;
using Telerik.Web.UI;

public partial class AccountsERMVoucherExcelUpload : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Upload", "UPLOAD",ToolBarDirection.Right);
        MenutravelInvoice.AccessRights = this.ViewState;
        MenutravelInvoice.MenuList = toolbar.Show();

      
    }

    protected void MenutravelInvoice_OnTabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToString().ToUpper() == "UPLOAD")
        {
            try
            {
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(FileUpload.FileName.ToString());

                if (extension.ToUpper() == ".XLSX")
                {
                    string strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + "Accounts/" + fileName + extension;
                    //string strpath = HttpContext.Current.Server.MapPath("~/Attachments/Accounts/") + fileName + extension;
                    FileUpload.PostedFile.SaveAs(strpath);
                    bool importstatus = false;
                    CheckImportfile(strpath, ref importstatus);

                    if (importstatus)
                    {
                        ERMVoucherExcelUpload(strpath, fileName);
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

    public void ERMVoucherExcelUpload(string filepath, string FileGuidName)
    {
        try
        {
            string patchid = Guid.NewGuid().ToString();

            List<string> cellvalues = new List<string>();
            var package = new ExcelPackage(new FileInfo(filepath));
            ExcelWorksheets worksheets = package.Workbook.Worksheets;

            if (worksheets.Count > 0)
            {
                foreach (ExcelWorksheet workSheet in worksheets)
                {
                    if (workSheet.Dimension != null)
                    {
                        int endRow = 1;

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            if (workSheet.Cells[i, 1].Value.ToString().Equals(""))
                            {
                                endRow = i;
                                break;
                            }
                            endRow = i;
                        }

                        string accountcode = workSheet.Cells[2, 1].Value.ToString();
                        //string accountdesc = workSheet.Cells[2, 2].Value.ToString();

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= endRow; i++)
                        {
                            for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                            {
                                if (!workSheet.Cells[i, 1].Value.ToString().Equals(""))
                                    cellvalues.Add(Convert.ToString(workSheet.Cells[i, j].Value));
                            }

                            DateTime? valuedate = General.GetNullableDateTime(workSheet.Cells[i, 2].Value.ToString());

                            if (valuedate == null)
                            {
                                ucError.ErrorMessage = "Value Date in row " + i.ToString() + " is not in date format";
                                ucError.Visible = true;
                                return;
                            }

                            if (!workSheet.Cells[i, 1].Value.ToString().Equals(accountcode))
                            {
                                ucError.ErrorMessage = "One upload file should have single account code only.";
                                ucError.Visible = true;
                                return;
                            }
                            //cellvalues.Add(Convert.ToString(workSheet.Cells[1, 1].Value));
                            CheckVoucherExists(cellvalues);
                            cellvalues.Clear();
                        }

                        Guid? uploadid = InsertERMVoucherExcel(accountcode);

                        HttpFileCollection postedFiles = Request.Files;
                        HttpPostedFile postedFile = postedFiles[0];

                        string filename = postedFile.FileName;
                        if (filename.LastIndexOfAny(new[] { '/', '\\' }) > 0)
                        {
                            filename = filename.Substring(filename.LastIndexOfAny(new[] { '/', '\\' }) + 1);
                        }
                        filepath = "ACCOUNTS/" + FileGuidName + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        PhoenixCommonFileAttachment.InsertAttachment(
                            new Guid(uploadid.ToString()), filename, filepath, postedFile.ContentLength
                                , 0
                                , 0, "ERMVOUCHER", new Guid(uploadid.ToString()));

                        if (uploadid == null)
                        {
                            ucError.ErrorMessage = "Document upload failed.";
                            ucError.Visible = true;
                            return;
                        }

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= endRow; i++)
                        {
                            for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                            {
                                if (!workSheet.Cells[i, 1].Value.ToString().Equals(""))
                                    cellvalues.Add(Convert.ToString(workSheet.Cells[i, j].Value));
                            }

                            //cellvalues.Add(Convert.ToString(workSheet.Cells[1, 1].Value));
                            InsertERMVoucher(cellvalues, new Guid(uploadid.ToString()));
                            cellvalues.Clear();
                        }

                        ucStatus.Text = "ERM Voucher Excel Uploaded Successfully.";
                        ucStatus.Visible = true;

                        String scriptKeepPopupOpen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'yes');");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookmarkScript", scriptKeepPopupOpen, true);

                        BindData();
                        gvAttachment.Rebind();
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

    private void CheckVoucherExists(List<string> li)
    {
        PhoenixAccountsERMVoucherDetail.ERMVoucherDetailVerify(
            li[0].ToString(), li[2].ToString(), General.GetNullableInteger(li[3].ToString()));
    }

    private Guid? InsertERMVoucherExcel(string accountcode)
    {
        Guid? uploadid = null;

        PhoenixAccountsERMVoucherDetail.ERMVoucherExcelUploadInsert(
            ref uploadid, accountcode);

        return uploadid;
    }

    private void InsertERMVoucher(List<string> li, Guid patchid)
    {
        try
        {
            PhoenixAccountsERMVoucherDetail.ERMVoucherDetailInsert(
                patchid,
                li[0].ToString(), // account code
                li[6].ToString(), // esm budgetcode
                General.GetNullableDateTime(li[1].ToString()), // voucher date
                li[2].ToString(), // erm voucher number
                General.GetNullableDecimal(li[4].ToString()), // amount
                li[5].ToString(), // description
                General.GetNullableInteger(li[3].ToString()), // xrow
                General.GetNullableString(li[7].ToString())); // phoenix voucher number
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        //string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        //int? sortdirection = null;

        //if (ViewState["SORTDIRECTION"] != null)
        //    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        
        ds = PhoenixAccountsERMVoucherDetail.ERMVoucherExcelUploadSearch(
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvAttachment.PageSize,
            ref iRowCount, ref iTotalPageCount);

        gvAttachment.DataSource = ds;
        gvAttachment.VirtualItemCount = iRowCount;
      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["ondblclick"] = "";
    }

 
    protected void gvAttachment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAttachment.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAttachment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvAttachment_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
                if (ed != null)
                {
                    DataRowView drv = (DataRowView)e.Item.DataItem;
                    //ed.Visible = false;
                    ed.Attributes["onclick"] = "javascript:openNewWindow('NATD','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey="
                       + drv["FLDEXCELUPLOADID"].ToString() + "&mod=ACCOUNTS&U=1'); return false;";
                    e.Item.Attributes["ondblclick"] = string.Empty;
                }

                ImageButton cmdLineItems = (ImageButton)e.Item.FindControl("cmdLineItems");
                if (cmdLineItems != null)
                {
                    DataRowView drv = (DataRowView)e.Item.DataItem;
                    //ed.Visible = false;
                    cmdLineItems.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Accounts/AccountsERMVoucherDetail.aspx?exceluploadid="
                       + drv["FLDEXCELUPLOADID"].ToString() + "'); return false;";
                }
            }
        }
        
    }
}
