using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class Accounts_AccountsCommitedCommitments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsCommitedCommitments.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvAdvancePayment')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsCommittedCommitmentsFilter.aspx", "Find", "search.png", "FIND");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
           // MenuOrderForm.SetTrigger(pnlOrderForm);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = null;

                if (Request.QueryString["COMMITMENTID"] != null)
                {
                    ViewState["COMMITMENTID"] = Request.QueryString["COMMITMENTID"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCommitedCommitmentsGeneral.aspx?COMMITMENTID=" + ViewState["COMMITMENTID"];
                }

                gvAdvancePayment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAdvancePayment_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
       // SetPageNavigator();
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDVESSELACCOUNT", "FLDNAME", "FLDPONUMBER", "FLDORDERDATE", "FLDCURRENCYNAME", "FLDPRIMEAMOUNT", "FLDAMOUNT", "FLDSUBACCOUNT", "FLDOWNERBUDGETGROUP", "FLDCOMMITTEDDATE", "FLDEXCLUDEDDATE", "FLDREVERSEDDATE", "FLDINVOICESTATUS", "FLDPURCHASEINVNO", "FLDPROJECTCODE", "FLDPOTYPE", "FLDPODESCRIPTION","FLDPOSTATUS" };
        string[] alCaptions = { "Vessel Name", "Vessel Account Code", "Supplier Name", "P.O. Number", "Ordered Date", "Prime Currency","Prime Amount","Amount(USD)", "Sub Ac", "Owner Budget Code", "Committed Date", "Excluded Date", "Reversed Date", "Invoice Status", "Purchase Invoice Number","Project Code", "Po Type", "PO Description","PO Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CommitedCommitmentsFilter;

        ds = PhoenixAccountsCommittedCosts.AccountsCommittedCostSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
           , nvc != null ? General.GetNullableString(nvc.Get("txtPoNumber")) : null
           , nvc != null ? General.GetNullableDateTime(nvc.Get("ucCommittedDate")) : null
           , nvc != null ? General.GetNullableInteger(nvc.Get("chkreversed")) : 1
            , (int)ViewState["PAGENUMBER"]
            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
            , ref iRowCount
            , ref iTotalPageCount
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselAccount")) : null);

        Response.AddHeader("Content-Disposition", "attachment; filename=AdditionalCommitments.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Additional Commitments</h3></td>");
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
               // SetPageNavigator();
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

        string[] alColumns = { "FLDVESSELNAME", "FLDVESSELACCOUNT", "FLDNAME", "FLDPONUMBER", "FLDORDERDATE", "FLDAMOUNT", "FLDSUBACCOUNT", "FLDOWNERBUDGETGROUP", "FLDCOMMITTEDDATE", "FLDEXCLUDEDDATE", "FLDREVERSEDDATE", "FLDINVOICESTATUS", "FLDPURCHASEINVNO", "FLDPROJECTCODE", "FLDPOTYPE", "FLDPODESCRIPTION", "FLDPOSTATUS" };
        string[] alCaptions = { "Vessel Name", "Vessel Account Code", "Supplier Name", "P.O. Number", "Ordered Date", "Amount(USD)", "Sub Ac", "Owner Budget Code", "Committed Date", "Excluded Date", "Reversed Date", "Invoice Status", "Purchase Invoice Number","Project Code", "Po Type", "PO Description","PO Status" };
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CommitedCommitmentsFilter;

        ds = PhoenixAccountsCommittedCosts.AccountsCommittedCostSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
           , nvc != null ? General.GetNullableString(nvc.Get("txtPoNumber")) : null
           , nvc != null ? General.GetNullableDateTime(nvc.Get("ucCommittedDate")) : null
           , nvc != null ? General.GetNullableInteger(nvc.Get("chkreversed")) : 1
            , (int)ViewState["PAGENUMBER"]
            , gvAdvancePayment.PageSize
            , ref iRowCount
            , ref iTotalPageCount
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselAccount")) : null);

        General.SetPrintOptions("gvAdvancePayment", "Committed Commitments", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAdvancePayment.DataSource = ds;
           

            if (ViewState["COMMITMENTID"] == null)
            {
                ViewState["COMMITMENTID"] = ds.Tables[0].Rows[0]["FLDCOMMITTEDCOSTBREAKUPID"].ToString();
               // gvAdvancePayment.SelectedIndex = 0;
            }

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCommitedCommitmentsGeneral.aspx?COMMITMENTID=" + ViewState["COMMITMENTID"];
            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCommitedCommitmentsGeneral.aspx";
            }
            DataTable dt = ds.Tables[0];
            gvAdvancePayment.DataSource = ds;
            //ShowNoRecordsFound(dt, gvAdvancePayment);
        }
        gvAdvancePayment.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void SetRowSelection()
    {
             for (int i = 0; i < gvAdvancePayment.Items.Count; i++)
        {
            if (gvAdvancePayment.MasterTableView.DataKeyValues[i].ToString().Equals(ViewState["COMMITMENTID"].ToString()))
            {
                gvAdvancePayment.MasterTableView.Items[i].Selected = true;
                break;
            }
        }
    }



  

   
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
       // SetPageNavigator();
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
       // SetPageNavigator();
        //gvAdvancePayment.SelectedIndex = 0;
       // BindPageURL(gvAdvancePayment.SelectedIndex);
        
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["COMMITMENTID"] = ((RadLabel)gvAdvancePayment.Items[rowindex].FindControl("lblPoNumber")).Text;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCommitedCommitmentsGeneral.aspx?COMMITMENTID=" + ViewState["COMMITMENTID"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAdvancePayment_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //gvAdvancePayment.SelectedIndex = e.NewSelectedIndex;
        BindPageURL(e.NewSelectedIndex);
    }

    protected void gvAdvancePayment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAdvancePayment.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAdvancePayment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
         

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            int iRowno;
            iRowno = int.Parse(e.CommandArgument.ToString());
            BindPageURL(iRowno);
            SetRowSelection();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }

   
}
