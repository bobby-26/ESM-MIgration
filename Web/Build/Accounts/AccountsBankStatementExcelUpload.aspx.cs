using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class AccountsBankStatementExcelUpload : PhoenixBasePage
{
    public int iCompanyid;

    protected void Page_Load(object sender, EventArgs e)
    {
        iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Upload", "UPLOAD",ToolBarDirection.Right);
        MenutravelInvoice.AccessRights = this.ViewState;
        MenutravelInvoice.Title = "Bank Statement Upload";
        MenutravelInvoice.MenuList = toolbar.Show();

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsBankStatementExcelUploadFilter.aspx", "Find", "search.png", "FIND");
        MenuAttachment.AccessRights = this.ViewState;
        MenuAttachment.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyid, 0);

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
       }

    protected void ddlBankAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBankAccount.SelectedBankAccount.ToUpper() != "DUMMY")
        {
            DataSet ds = PhoenixRegistersAccount.ListBankAccount(
                Convert.ToInt32(ddlBankAccount.SelectedBankAccount.ToString()), null,
                PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                //txtCurrencyId.Text = dr["FLDBANKCURRENCYID"].ToString();
                txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
                txtDesc.Text = dr["FLDDESCRIPTION"].ToString();
                //txtAccountId.Text = dr["FLDACCOUNTID"].ToString();
                //txtSubAccountCode.Text = dr["FLDSUBACCOUNT"].ToString();
            }
        }
    }

    protected void MenuAttachment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvAttachment.SelectedIndexes.Clear();
        gvAttachment.EditIndexes.Clear();
        gvAttachment.DataSource = null;
        gvAttachment.Rebind();
    }
    protected void MenutravelInvoice_OnTabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToString().ToUpper() == "UPLOAD")
        {
            try
            {
                if (General.GetNullableInteger(ddlBankAccount.SelectedBankAccount) == null)
                {
                    ucError.ErrorMessage = "Please select a bank account.";
                    ucError.Visible = true;
                    return;
                }

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
                        BankStatementExcelUpload(strpath, fileName);
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

    public void BankStatementExcelUpload(string filepath, string FileGuidName)
    {
        try
        {
            if (General.GetNullableInteger(ucCompany.SelectedCompany) == null)
            {
                ucError.ErrorMessage = "Company is required.";
                ucError.Visible = true;
                return;
            }
            if (General.GetNullableString(ddlType.SelectedValue) == null)
            {
                ucError.ErrorMessage = "Type is required.";
                ucError.Visible = true;
                return;
            }

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
                            if (workSheet.Cells[i, 1].Value == null)
                            {
                                //endRow = i;
                                break;
                            }
                            endRow = i;
                        }

                        for (int j = workSheet.Dimension.Start.Column; j <= 5; j++)
                        {
                            if (!workSheet.Cells[1, j].Value.ToString().Equals(""))
                                cellvalues.Add(Convert.ToString(workSheet.Cells[1, j].Value));
                        }
                        if (!VerifyHeaders(cellvalues))
                        {
                            ucError.ErrorMessage = "File is of incorrect format";
                            ucError.Visible = true;
                            return;
                        }

                        cellvalues.Clear();

                        DateTime fromdate, todate;
                        DateTime? valuedate = General.GetNullableDateTime(workSheet.Cells[2, 4].Value.ToString());
                        decimal? amount = General.GetNullableDecimal(workSheet.Cells[2, 5].Value.ToString());
                        int month;

                        if (valuedate == null)
                        {
                            ucError.ErrorMessage = "Value Date in row 2 is not in date format";
                            ucError.Visible = true;
                            return;
                        }
                        else
                        {
                            month = valuedate.Value.Month;
                            fromdate = DateTime.Parse(valuedate.ToString());
                            todate = DateTime.Parse(valuedate.ToString());
                        }

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= endRow; i++)
                        {
                            for (int j = workSheet.Dimension.Start.Column; j <= 5; j++)
                            {
                                if (!workSheet.Cells[i, 1].Value.ToString().Equals(""))
                                    cellvalues.Add(Convert.ToString(workSheet.Cells[i, j].Value));
                            }
                            valuedate = General.GetNullableDateTime(workSheet.Cells[i, 4].Value.ToString());

                            if (valuedate == null)
                            {
                                ucError.ErrorMessage = "Value Date in row " + i.ToString() + " is not in date format";
                                ucError.Visible = true;
                                return;
                            }
                            else
                            {
                                if (valuedate.Value.Month != month)
                                {
                                    ucError.ErrorMessage = "Value Date in row " + i.ToString() + " is not in the same month";
                                    ucError.Visible = true;
                                    return;
                                }
                                if (valuedate < fromdate)
                                    fromdate = DateTime.Parse(valuedate.ToString());

                                if (valuedate > todate)
                                    todate = DateTime.Parse(valuedate.ToString());
                            }

                            amount = General.GetNullableDecimal(workSheet.Cells[i, 5].Value.ToString());

                            if (amount >= 0)
                            {
                                ucError.ErrorMessage = "Amount in row " + i.ToString() + " is not in correct format";
                                ucError.Visible = true;
                                return;
                            }

                            //cellvalues.Add(Convert.ToString(workSheet.Cells[1, 1].Value));
                            try
                            {
                                VerifyUploadExcel(cellvalues, i.ToString(), General.GetNullableInteger(ucCompany.SelectedCompany));

                            }
                            catch (Exception EX)
                            {
                                ucError.ErrorMessage = EX.Message;
                                ucError.Visible = true;
                                return;
                            }
                            cellvalues.Clear();
                        }

                        Guid? uploadid = InsertBankStatementExcel(
                            int.Parse(ddlBankAccount.SelectedBankAccount),
                            ddlType.SelectedValue,
                            fromdate,
                            todate,
                            month,
                            General.GetNullableInteger(ucCompany.SelectedCompany));

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
                                , 0, "BANKSTATEMENT", new Guid(uploadid.ToString()));

                        if (uploadid == null)
                        {
                            ucError.ErrorMessage = "Document upload failed.";
                            ucError.Visible = true;
                            return;
                        }

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= endRow; i++)
                        {
                            for (int j = workSheet.Dimension.Start.Column; j <= 5; j++)
                            {
                                if (!workSheet.Cells[i, 1].Value.ToString().Equals(""))
                                    cellvalues.Add(Convert.ToString(workSheet.Cells[i, j].Value));
                            }

                            //cellvalues.Add(Convert.ToString(workSheet.Cells[1, 1].Value));
                            InsertBankStatementLineItems(cellvalues, new Guid(uploadid.ToString()), int.Parse(ddlBankAccount.SelectedBankAccount));
                            cellvalues.Clear();
                        }

                        ucStatus.Text = "Bank Statement Excel Uploaded Successfully.";
                        ucStatus.Visible = true;
                        Rebind();
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
        if (!li[0].ToString().ToUpper().Equals("TT REFERENCE"))
            return false;
        else if (!li[1].ToString().ToUpper().Equals("BANK REFERENCE"))
            return false;
        else if (!li[2].ToString().ToUpper().Equals("CUSTOMER REFERENCE"))
            return false;
        else if (!li[3].ToString().ToUpper().Equals("VALUE DATE (DD/MM/YYYY)"))
            return false;
        else if (!li[4].ToString().ToUpper().Equals("DEBIT AMOUNT"))
            return false;
        else
            return true;
    }

    private void VerifyUploadExcel(List<string> li, string rowno, int? companyid)
    {
        PhoenixAccountsBankStatementUpload.VerifyBankStatementUpload(
            li[2].ToString().Trim(), rowno, ddlType.SelectedValue, General.GetNullableInteger(ddlBankAccount.SelectedBankAccount), li[0].ToString().Trim(), companyid);

    }

    private Guid? InsertBankStatementExcel(int accountid, string type, DateTime? fromdate, DateTime? todate, int month, int? companyid)
    {
        Guid? uploadid = null;

        PhoenixAccountsBankStatementUpload.BankStatementUploadInsert(
            ref uploadid, accountid, type, fromdate, todate, month, companyid);

        return uploadid;
    }

    private void InsertBankStatementLineItems(List<string> li, Guid uploadid, int accountid)
    {
        try
        {
            PhoenixAccountsBankStatementUpload.BankStatementLineItemInsert(
                uploadid,
                accountid,
                li[0].ToString(), // tt ref
                General.GetNullableDateTime(li[3].ToString()), // value date
                li[1].ToString(), // bank ref
                General.GetNullableDecimal(li[4].ToString()), // amount
                li[2].ToString()); // customer ref
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

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentBankStatementUploadSelection;

        ds = PhoenixAccountsBankStatementUpload.BankStatementUploadSearch(
            General.GetNullableString(nvc != null ? nvc.Get("txtBankAccount") : null),
            General.GetNullableString(nvc != null ? nvc.Get("txtAccDesc") : null),
            General.GetNullableInteger(nvc != null ? nvc.Get("ucCurrency") : ""),
            General.GetNullableString(nvc != null ? nvc.Get("ddlType") : null),
            General.GetNullableInteger(nvc != null ? nvc.Get("ucMonth") : ""),
            General.GetNullableInteger(nvc != null ? nvc.Get("chkExclPostedBankStmt") : "1"),
            sortdirection,
            sortexpression,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
           gvAttachment.PageSize,
            ref iRowCount, ref iTotalPageCount,
            General.GetNullableInteger(nvc != null ? nvc.Get("chkExclArchivedBankStmt") : "1"));
            
            gvAttachment.DataSource = ds;
             gvAttachment.VirtualItemCount = iRowCount;
      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

   
    protected void gvAttachment_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

        

            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                PhoenixAccountsRemittance.Bankvoucherpaymentdetailsupdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ((RadLabel)e.Item.FindControl("lblexceluploadId")).Text);
                Rebind();
                ucStatus.Text = "Bank Statement is Fully Posted.";
            }

            if (e.CommandName.ToUpper().Equals("ARCHIVE"))
            {
                PhoenixAccountsBankStatementUpload.ArchiveBankUPload(new Guid(((RadLabel)e.Item.FindControl("lblexceluploadId")).Text));
                Rebind();
                ucStatus.Text = "Bank Statement is Archived Successfully.";
            }
            else if (e.CommandName == "Page")
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

    protected void gvAttachment_RowDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
                ImageButton post = (ImageButton)e.Item.FindControl("cmdPost");
                ImageButton archive = (ImageButton)e.Item.FindControl("cmdArchive");

                if (ed != null)
                {

                    //ed.Visible = false;
                    ed.Attributes["onclick"] = "javascript:Openpopup('NATD','','../Accounts/AccountsVoucherRegisterFileAttachments.aspx?dtkey="
                       + drv["FLDUPLOADID"].ToString() + "&mod=ACCOUNTS&U=1'); return false;";
                    e.Item.Attributes["ondblclick"] = string.Empty;
                }

                RadLabel lblVoucherRefUpdatedYN = (RadLabel)e.Item.FindControl("lblVoucherRefUpdatedYN");


                if (post != null)
                {
                    if (lblVoucherRefUpdatedYN.Text == "1" || General.GetNullableInteger(drv["FLDISARCHIVED"].ToString()) == 1)
                    {
                        post.Visible = false;
                    }
                    else
                    {
                        post.Visible = true;
                    }
                }

                if (archive != null)
                {
                    if (General.GetNullableInteger(drv["FLDISARCHIVED"].ToString()) == 1)
                    {
                        archive.Visible = false;
                    }
                    else
                    {
                        archive.Visible = true;
                    }
                }

                //ImageButton cmdLineItems = (ImageButton)e.Row.FindControl("cmdLineItems");
                //if (cmdLineItems != null)
                //{
                //    DataRowView drv = (DataRowView)e.Row.DataItem;
                //    //ed.Visible = false;
                //    cmdLineItems.Attributes["onclick"] = "javascript:Openpopup('NATD','','../Accounts/AccountsERMVoucherDetail.aspx?exceluploadid="
                //       + drv["FLDUPLOADID"].ToString() + "'); return false;";
                //}
            }



        }
        if (e.Item is GridHeaderItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }

    
   
    protected void gvAttachment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
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
}
