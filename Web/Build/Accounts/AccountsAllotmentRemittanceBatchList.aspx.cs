using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections;
using System.IO;
using System.Web;
using SouthNests.Phoenix.Common;
using System.Text;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceBatchList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsAllotmentRemittanceBatchList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvRemittenceBatch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsAllotmentRemittanceBatchFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsAllotmentRemittanceBatchList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','"+Session["sitepath"]+"/Accounts/AccountsAllotmentRemittanceBankProcess.aspx')", "Add", "<i class=\"fas fa-user-plus\"></i>", "ADDCOMPANY");
            // toolbargrid.AddImageLink("javascript:Openpopup('codehelp1','','AccountsAllotmentRemittanceBatchInstructionList.aspx')", "Add", "add.png", "ADDCOMPANY");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //MenuOrderForm.SetTrigger(pnlRemittance);
            //   ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAllotmentRemittanceBatchInstructionList.aspx";

            if (!IsPostBack)
            {
                PhoenixAccountsAllotmentRemittance.AllotmentRemittanceBatchListRefresh(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["Batchid"] = null;

                if (Request.QueryString["Batchid"] != null)
                {
                    ViewState["Batchid"] = Request.QueryString["Batchid"].ToString();
                    //      ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAllotmentRemittanceBatchInstructionList.aspx?Batchid=" + ViewState["Batchid"].ToString();
                }
                gvRemittenceBatch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRemittenceBatch_Sorting(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
        Rebind();
    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Batch No.", "Payment date", "Payment mode", "Account Code" };
        string[] alColumns = { "FLDBATCHNUMBER", "FLDPAYMENTDATE", "FLDPAYMENTMODENAME", "FLDBANKACCOUNT" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentAllotmentRemittenceBatchSelection;

        ds = PhoenixAccountsAllotmentRemittance.RemittanceBatchSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtRemittenceBatchNumberSearch")) : null
                                                            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlAccountCode")) : null
                                                            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlPaymentmode")) : null
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentFromdateSearch")) : string.Empty
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentTodateSearch")) : string.Empty
                                                            , null
                                                            , null
                                                            , (int)ViewState["PAGENUMBER"]
                                                            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                            );

        //ds = PhoenixAccountsAllotmentRemittance.RemittanceBatchSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, null, null, null, null, null, null, null, (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=AllotmentRemittanceBatch.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Remittance</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentAllotmentRemittenceBatchSelection = null;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRemittenceBatch.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentAllotmentRemittenceBatchSelection;

        ds = PhoenixAccountsAllotmentRemittance.RemittanceBatchSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtRemittenceBatchNumberSearch")) : null
                                                            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlAccountCode")) : null
                                                            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlPaymentmode")) : null
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentFromdateSearch")) : string.Empty
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentTodateSearch")) : string.Empty
                                                            , null
                                                            , null
                                                            , gvRemittenceBatch.CurrentPageIndex+1
                                                            , gvRemittenceBatch.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                            );
        string[] alCaptions = { "Batch No.", "Payment Date", "Payment mode", "Account Code" };
        string[] alColumns = { "FLDBATCHNUMBER", "FLDPAYMENTDATE", "FLDPAYMENTMODENAME", "FLDBANKACCOUNT" };

        General.SetPrintOptions("gvRemittenceBatch", "Remittance", alCaptions, alColumns, ds);
        gvRemittenceBatch.DataSource = ds;
        gvRemittenceBatch.VirtualItemCount = iRowCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["Batchid"] == null)
            {
                ViewState["Batchid"] = ds.Tables[0].Rows[0]["FLDBATCHID"].ToString();
               // gvRemittenceBatch.MasterTableView.Items[0].Selected = true;
            }

            SetRowSelection();
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvRemittenceBatch_RowCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;


        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            if (General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtPaymentDateEdit")).Text) == null)
                ucError.ErrorMessage = "Payment date is required.";

            if (ucError.IsError)
            {
                ucError.Visible = true;
                return;
            }
            try
            {
                PhoenixAccountsAllotmentRemittance.RemittanceInstructionBatchPaymentdateUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , new Guid(((RadLabel)e.Item.FindControl("lblBatchId")).Text)
                                                                                , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtPaymentDateEdit")).Text));

                Rebind();
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }

        if (e.CommandName.ToUpper().Equals("BANKUPLOAD"))
        {

            try
            {
                PhoenixAccountsAllotmentRemittance.RemittanceInstructionBatchStatusUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(((RadLabel)e.Item.FindControl("lblBatchId")).Text));
                Rebind();
              
                DataSet ds2 = PhoenixAccountsRemittanceBankDownload.GetBankDownloadMappedTemplates(
                    ((RadLabel)e.Item.FindControl("lblBatchId")).Text
                    , General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblCompanyId")).Text)
                    , null
                    , General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblAccountId")).Text)
                    );
                DataSet ds = PhoenixAccountsAllotmentRemittance.RemittanceBankInstructionSupplierPayableAmountCSV(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        string strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/ACCOUNTS/";
                        string filename = strpath + ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString() + ".csv";
                        Guid UBatchId = new Guid(ds.Tables[0].Rows[0]["FLDBATCHID"].ToString());
                        DataTable dt = PhoenixCommonFileAttachment.AttachmentList(UBatchId, "REMITTANCEBATCH");
                        int i = 0;
                        decimal sum = 0;

                        DataSet ds1 = new DataSet();
                        DataTable dt1 = new DataTable();

                        if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "ALT-CITI-MY")
                        {
                            ds1 = PhoenixAccountsAllotmentRemittance.RemittanceBankInstructionSupplierCitibankMY(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 1;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "ALT-CITI-SG")
                        {
                            ds1 = PhoenixAccountsAllotmentRemittance.RemittanceBankInstructionSupplierCitibankSG(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 2;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "ALT-DBS")
                        {
                            ds1 = PhoenixAccountsAllotmentRemittance.RemittanceBankInstructionSupplierDBS(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 3;
                            sum = Convert.ToDecimal(ds1.Tables[2].Rows[0]["SUMOFREMITTANCEAMOUNT"]);
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "ALT-ICICI-SG")
                        {
                            ds1 = PhoenixAccountsAllotmentRemittance.AllotmentRemittanceBankInstructionSupplierICICISG(new Guid(((RadLabel)e.Item.FindControl("lblBatchId")).Text));
                            dt1 = ds1.Tables[1];
                            i = 5;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "ALT-CITI-NRO-IN")
                        {
                            ds1 = PhoenixAccountsAllotmentRemittance.AllotmentRemittanceBankInstructionSupplierCITINRO(new Guid(((RadLabel)e.Item.FindControl("lblBatchId")).Text));
                            dt1 = ds1.Tables[1];
                            i = 6;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "ALT-STANDARD")
                        {
                            ds1 = PhoenixAccountsAllotmentRemittance.AllotmentRemittanceBankInstructionSupplierSCB(new Guid(((RadLabel)e.Item.FindControl("lblBatchId")).Text));
                            dt1 = ds1.Tables[1];
                            i = 7;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "ALT-ICICI-NRO")
                        {
                            ds1 = PhoenixAccountsAllotmentRemittance.AllotmentRemittanceBankInstructionSupplierICICINRO(new Guid(((RadLabel)e.Item.FindControl("lblBatchId")).Text));
                            dt1 = ds1.Tables[1];
                            i = 8;
                        }
                        else
                        {

                            ds1 = PhoenixAccountsAllotmentRemittance.RemittanceInstructionSupplierPayableAmountCSV(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 4;
                        }
                        PhoenixAccountsBankDownload2XL.DumpExcelAllotmentRemittance(ds1, filename, i, sum);

                        FileInfo f = new FileInfo(filename);
                        long s1 = f.Length;
                        if (dt.Rows.Count == 0)
                        { PhoenixCommonFileAttachment.InsertAttachment(UBatchId, ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString() + ".csv", "ACCOUNTS/" + ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString() + ".csv", s1, null, null, "REMITTANCEBATCH", new Guid(ds.Tables[0].Rows[0]["FLDBATCHID"].ToString())); }
                    }
                }
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }

        if (e.CommandName.ToUpper().Equals("APPROVE"))
        {
            if (General.GetNullableDateTime(((RadLabel)e.Item.FindControl("lblPaymentDate")).Text) == null)
                ucError.ErrorMessage = "Payment date is required.";

            if (ucError.IsError)
            {
                ucError.Visible = true;
                return;
            }
            try
            {
                PhoenixAccountsAllotmentRemittance.RemittancePostAsync(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ((RadLabel)e.Item.FindControl("lblBatchId")).Text, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            Rebind();
          //  _gridView.SelectedIndex = Int32.Parse(e.CommandArgument.ToString());
        }
        else if (e.CommandName.ToUpper().Equals("LINEITEM"))
        {
            Response.Redirect("AccountsAllotmentRemittanceBatchInstructionList.aspx?Batchid=" + ((RadLabel)e.Item.FindControl("lblBatchId")).Text);
        }
        else if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            Response.Redirect("AccountsAllotmentRemittanceBatchInstructionList.aspx?Batchid=" + ((RadLabel)e.Item.FindControl("lblBatchId")).Text);
        }
        else if (e.CommandName.ToUpper().Equals("HISTORY"))
        {
            Response.Redirect("AccountsAllotmentRemittanceBatchDownLoadHistory.aspx?Batchid=" + ((RadLabel)e.Item.FindControl("lblBatchId")).Text);
        }
        else
        {

            Rebind();
        }
    }
    protected void gvRemittenceBatch_RowDeleting(object sender, GridCommandEventArgs de)
    {
        Rebind();
    }

    protected void gvRemittenceBatch_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            
            {
                e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#bbddff'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
                RadLabel lblVoucherid = (RadLabel)e.Item.FindControl("lblVoucherid");
                RadLabel lblFileName = ((RadLabel)e.Item.FindControl("lblFileName"));
                RadLabel lblIsmodified = (RadLabel)e.Item.FindControl("lblIsmodified");

                LinkButton cmdPost = (LinkButton)e.Item.FindControl("cmdPost");
                LinkButton imgUpload = (LinkButton)e.Item.FindControl("imgUpload");


                if (cmdPost != null)
                    cmdPost.Attributes.Add("onclick", "javascript:this.onclick=function(){ return false;};");// this will fix the multiple click problem

                if (lblIsmodified != null && lblIsmodified.Text != "" && lblIsmodified.Text != "2")
                {
                    if (lblVoucherid.Text != "")
                        imgUpload.Visible = true;
                }
                else
                {
                    if (lblFileName != null)
                    {
                        if (lblFileName.Text != string.Empty)
                        {
                            RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                            HyperLink lnk = (HyperLink)e.Item.FindControl("lnkfilename");
                            lnk.NavigateUrl = "../accounts/download.aspx?filename=" + lblFileName.Text + "&filepath=" + PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + lblFilePath.Text;
                            //lnk.NavigateUrl = Session["sitepath"] + "/attachments/" + lblFilePath.Text;
                            if (lblVoucherid.Text == string.Empty)
                            {
                                lnk.Visible = false;
                            }
                            else
                            {
                                lnk.Visible = true;
                            }
                        }
                    }
                }
                RadLabel lblBankchargeVoucherId = (RadLabel)e.Item.FindControl("lblBankchargeVoucherId");
                RadLabel lblBankchargeYN = (RadLabel)e.Item.FindControl("lblBankchargeYN");
                RadLabel lblBatchId = (RadLabel)e.Item.FindControl("lblBatchId");
                LinkButton cmdBankCharges = (LinkButton)e.Item.FindControl("cmdBankCharges");
                if (lblVoucherid != null)
                {
                    if (lblVoucherid.Text != string.Empty)
                    {
                        if (cmdPost != null)
                            cmdPost.Visible = false;
                    }
                }
                if (lblBankchargeVoucherId != null && lblBankchargeYN != null)
                {
                    if (lblBankchargeYN.Text == "1" && lblBankchargeVoucherId.Text == string.Empty)
                    {
                        if (cmdPost != null)
                            cmdPost.Visible = false;
                    }
                    if (cmdBankCharges != null)
                    {
                        cmdBankCharges.Attributes.Add("onclick", "javascript:parent.openNewWindow('CrewPage','','"+Session["sitepath"]+"/Accounts/AccountsAllotmentRemittanceBatchTaxandChargesList.aspx?Batchid=" + lblBatchId.Text + "'); return false;");
                        if (lblBankchargeYN.Text == "1")
                            cmdBankCharges.Visible = true;
                        else
                            cmdBankCharges.Visible = false;
                    }
                }

            }
        }
    }


    private string ResolveImageType(string extn)
    {
        string imagepath = Session["images"] + "/";
        if (string.IsNullOrEmpty(extn)) extn = string.Empty;
        switch (extn.ToLower())
        {
            case ".jpg":
            case ".png":
            case ".gif":
            case ".bmp":
                imagepath += "imagefile.png";
                break;
            case ".doc":
                imagepath += "word.png";
                break;
            case ".xls":
                imagepath += "xls.png";
                break;
            case ".pdf":
                imagepath += "pdf.png";
                break;
            case ".rar":
            case ".zip":
                imagepath += "rar.png";
                break;
            case ".txt":
                imagepath += "notepad.png";
                break;
            default:
                imagepath += "anyfile.png";
                break;
        }
        return imagepath;
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    private void SetRowSelection()
    {
        gvRemittenceBatch.SelectedIndexes.Clear();
        for (int i = 0; i < gvRemittenceBatch.Items.Count; i++)
        {
            if (gvRemittenceBatch.MasterTableView.Items[i].GetDataKeyValue("FLDBATCHID").Equals(ViewState["Batchid"].ToString()))
            {
                gvRemittenceBatch.MasterTableView.Items[i].Selected = true;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void Rebind()
    {
        gvRemittenceBatch.SelectedIndexes.Clear();
        gvRemittenceBatch.EditIndexes.Clear();
        gvRemittenceBatch.DataSource = null;
        gvRemittenceBatch.Rebind();
    }
    protected void gvRemittenceBatch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
        SetRowSelection();
    }
}
