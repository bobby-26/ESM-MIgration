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


public partial class Accounts_AccountsBankStatementReconExcelUploadLineitem : PhoenixBasePage
{
    public string strTransactionAmountTotal;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            SessionUtil.PageAccessRights(this.ViewState);
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //PhoenixToolbar toolbar = new PhoenixToolbar();
        //toolbar.AddButton("Upload", "UPLOAD");
        //MenutravelInvoice.AccessRights = this.ViewState;
        //MenutravelInvoice.MenuList = toolbar.Show();

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        //toolbar1.AddButton("Bank Statement", "BANKSTATEMENT");
        //toolbar1.AddButton("Bank Recon Status", "RECONSTATUS");
        //toolbar1.AddButton("Allocation/Bank Tag Report", "ALLOCATIONREPORT");
         Bankupload.AccessRights = this.ViewState;
         Bankupload.Title = "More Info";
         Bankupload.MenuList = toolbar1.Show();

            if (Request.QueryString["uploadid"] != null && Request.QueryString["uploadid"].ToString() != "")
                ViewState["uploadid"] = Request.QueryString["uploadid"].ToString();
            else
                ViewState["uploadid"] = "";

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsBankStatementReconExcelUploadLineitem.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbargrid.AddImageLink("javascript:CallPrint('gvAttachment')", "Print Grid", "icon_print.png", "PRINT");
            //toolbargrid.AddImageButton("javascript:Openpopup('codehelp1','','AccountsBankStatementReconExcelUploadFilter.aspx')", "Find", "search.png", "FIND");


            MenuAttachment.AccessRights = this.ViewState;
            MenuAttachment.MenuList = toolbargrid.Show();
         }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("BANKSTATEMENT"))
            {
                Response.Redirect("../Accounts/AccountsBankStatementExcelUpload.aspx", false);
            }

            if (CommandName.ToUpper().Equals("RECONSTATUS"))
            {
                Response.Redirect("../Accounts/AccountsBankStatementReconStatusList.aspx", false);
            }

            if (CommandName.ToUpper().Equals("ALLOCATIONREPORT"))
            {
                Response.Redirect("../Accounts/AccountsBankStatementReconExcelUploadAllocation.aspx", false);
            }
            else if (CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
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
               // BindData();
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
        decimal dTotalAmount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = {"FLDTAGNO", "FLDBANKVOUCHERNUMBER","FLDTTREFERENCE","FLDBANKREFERENCE","FLDCURRENCYCODE",
                                 "FLDAMOUNT", "FLDSTATUS"};
        string[] alCaptions = {"Bank Tagging Id", "Bank Voucher Number","Ledger TT Ref","Ledger Long description ","Currency code",
                                 "Amount","Status"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentBankStatementUploadSelection;

        ds = PhoenixAccountsBankStatementReconUpload.BankStatementReconLineItemAllocationList(General.GetNullableGuid(ViewState["uploadid"].ToString()),
                                                                                    gvAttachment.CurrentPageIndex+1, gvAttachment.PageSize, ref iRowCount, ref iTotalPageCount, ref dTotalAmount);

        General.SetPrintOptions("gvAttachment", "Bank Recon More Info", alCaptions, alColumns, ds);

        strTransactionAmountTotal = String.Format("{0:n}", dTotalAmount);
        strTransactionAmountTotal = "Amount (" + (strTransactionAmountTotal) + ")";
        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
            gvAttachment.DataSource = ds;
            gvAttachment.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;

        //}
        //else if (ds.Tables.Count > 0)
        //{
        //    ShowNoRecordsFound(ds.Tables[0], gvAttachment);
        //}
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
   

    }

     protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        decimal dTotalAmount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = {"FLDTAGNO", "FLDBANKVOUCHERNUMBER","FLDTTREFERENCE","FLDBANKREFERENCE","FLDCURRENCYCODE",
                                 "FLDAMOUNT", "FLDSTATUS"};
        string[] alCaptions = {"Bank Tagging Id", "Bank Voucher Number","Ledger TT Ref","Ledger Long description ","Currency code",
                                 "Amount","Status"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsBankStatementReconUpload.BankStatementReconLineItemList(General.GetNullableGuid(ViewState["uploadid"].ToString()),
                                                                                    1, iRowCount, ref iRowCount, ref iTotalPageCount, ref dTotalAmount);


        strTransactionAmountTotal = String.Format("{0:n}", dTotalAmount);
        strTransactionAmountTotal = "Amount (" + (strTransactionAmountTotal) + ")";


        Response.AddHeader("Content-Disposition", "attachment; filename=MoreInfo.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>MoreInfo</h3></td>");
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
}
