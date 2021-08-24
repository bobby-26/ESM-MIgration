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


public partial class Accounts_AccountsBankStatementReconExcelUploadAllocation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Bank Recon Status", "RECONSTATUS",ToolBarDirection.Right);
        toolbar1.AddButton("Allocation/Bank Report", "ALLOCATIONREPORT",ToolBarDirection.Right);
        toolbar1.AddButton("Bank Statement", "BANKSTATEMENT",ToolBarDirection.Right);

        Bankupload.AccessRights = this.ViewState;
        Bankupload.Title = "Allocation/Bank Tag Report";
        Bankupload.MenuList = toolbar1.Show();

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsBankStatementReconExcelUploadAllocation.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
        toolbargrid.AddImageLink("javascript:CallPrint('gvAttachment')", "Print Grid", "icon_print.png", "PRINT");
        toolbargrid.AddImageButton("../Accounts/AccountsBankStatementReconExcelUploadAllocationFilter.aspx", "Find", "search.png", "FIND");


        MenuAttachment.AccessRights = this.ViewState;
        MenuAttachment.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
           // iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            SessionUtil.PageAccessRights(this.ViewState);

            //PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddButton("Upload", "UPLOAD");
            //MenutravelInvoice.AccessRights = this.ViewState;
            //MenutravelInvoice.MenuList = toolbar.Show();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            Bankupload.SelectedMenuIndex = 1;
        }
    }
    protected void Bankupload_OnTabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("BANKSTATEMENT"))
            {
                Response.Redirect("../Accounts/AccountsBankStatementReconExcelUpload.aspx", false);
            }

            if (CommandName.ToUpper().Equals("RECONSTATUS"))
            {
                Response.Redirect("../Accounts/AccountsBankStatementReconStatusList.aspx", false);
            }
            //if (dce.CommandName.ToUpper().Equals("REMITTANCE"))
            //{
            //    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceRequest.aspx?REMITTENCEID=" + ViewState["Remittenceid"];
            //}
            //if (dce.CommandName.ToUpper().Equals("LINEITEMS") && ViewState["Remittenceid"] != null && ViewState["Remittenceid"].ToString() != string.Empty)
            //{
            //    Response.Redirect("../Accounts/AccountsRemittanceRequestLineItem.aspx?REMITTENCEID=" + ViewState["Remittenceid"]);
            //}            
            //if (dce.CommandName.ToUpper().Equals("HISTORY") && ViewState["Remittenceid"] != null && ViewState["Remittenceid"].ToString() != string.Empty)
            //{
            //    Response.Redirect("../Accounts/AccountsRemittanceHistory.aspx?REMITTANCEID=" + ViewState["Remittenceid"]);
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuAttachment_OnTabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDBANKTAGNO", "FLDCURRENCYCODE", "FLDBANKSTATEMENTAMOUNT", "FLDALLOCATEDAMOUNT", "FLDREMAININGAMOUNT", "FLDALLOCATIONSTATUS" };
        string[] alCaptions = { "Bank Tagging Id", "Currency", "Bank Amount Net", "Allocated in Ledger", "Remaining amount", "Allocation Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentBankStatementUploadSelection;

        ds = PhoenixAccountsBankStatementReconUpload.BankStatementReconUploadAllocationSearch(
            General.GetNullableString(nvc != null ? nvc.Get("txtBankAccount") : null),
            General.GetNullableString(nvc != null ? nvc.Get("txtAccDesc") : null),
            General.GetNullableInteger(nvc != null ? nvc.Get("ucCurrency") : ""),
            //General.GetNullableString(nvc != null ? nvc.Get("ddlType") : null),
            General.GetNullableInteger(nvc != null ? nvc.Get("ucMonth") : ""),
            General.GetNullableInteger(nvc != null ? nvc.Get("chkExclPostedBankStmt") : "1"),
            sortdirection,
            sortexpression,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvAttachment.PageSize,
            ref iRowCount, ref iTotalPageCount,
            General.GetNullableInteger(nvc != null ? nvc.Get("chkExclArchivedBankStmt") : "1"), General.GetNullableString(nvc != null ? nvc.Get("txtBankTaggingId") : null),
            General.GetNullableInteger(nvc != null ? "0" : "1")
            );

        General.SetPrintOptions("gvAttachment", "Allocation/Bank Report", alCaptions, alColumns, ds);
            
            gvAttachment.DataSource = ds;
            gvAttachment.VirtualItemCount=iRowCount;
       ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDBANKTAGNO", "FLDCURRENCYCODE", "FLDBANKSTATEMENTAMOUNT", "FLDALLOCATEDAMOUNT", "FLDREMAININGAMOUNT", "FLDALLOCATIONSTATUS" };
            string[] alCaptions = { "Bank Tagging Id", "Currency", "Bank Amount Net", "Allocated in Ledger", "Remaining amount", "Allocation Status" };


            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentBankStatementUploadSelection;

            ds = PhoenixAccountsBankStatementReconUpload.BankStatementReconUploadSearch(
                General.GetNullableString(nvc != null ? nvc.Get("txtBankAccount") : null),
                General.GetNullableString(nvc != null ? nvc.Get("txtAccDesc") : null),
                General.GetNullableInteger(nvc != null ? nvc.Get("ucCurrency") : ""),
                //General.GetNullableString(nvc != null ? nvc.Get("ddlType") : null),
                General.GetNullableInteger(nvc != null ? nvc.Get("ucMonth") : ""),
                General.GetNullableInteger(nvc != null ? nvc.Get("chkExclPostedBankStmt") : "1"),
                sortdirection,
                sortexpression,
                1,
               PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                ref iRowCount, ref iTotalPageCount,
                General.GetNullableInteger(nvc != null ? nvc.Get("chkExclArchivedBankStmt") : "1"), General.GetNullableString(nvc != null ? nvc.Get("txtBankTaggingId") : null));


            Response.AddHeader("Content-Disposition", "attachment; filename=Allocation/BankTagReport.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Allocation/Bank Tag Report</h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



 

    protected void gvAttachment_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            RadTextBox txtuploadid = (RadTextBox)e.Item.FindControl("txtuploadid");
            ImageButton cmdMoreInfo = (ImageButton)e.Item.FindControl("cmdMoreInfo");
            if (cmdMoreInfo != null)
            {
                cmdMoreInfo.Attributes.Add("onclick", "Openpopup('codeHelp1', '', '../Accounts/AccountsBankStatementReconExcelUploadLineitem.aspx?uploadid=" + txtuploadid.Text + "');return false;");
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
    protected void Rebind()
    {
        gvAttachment.SelectedIndexes.Clear();
        gvAttachment.EditIndexes.Clear();
        gvAttachment.DataSource = null;
        gvAttachment.Rebind();
    }
    protected void gvAttachment_ItemCommand(object sender, GridCommandEventArgs e)
    {
         if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
}
