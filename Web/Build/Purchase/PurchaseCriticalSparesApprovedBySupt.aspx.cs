using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;

public partial class Purchase_PurchaseCriticalSparesApprovedBySupt : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseCriticalSparesApprovedBySupt.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCriticalSpares')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseCriticalSparesApprovedBySupt.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseCriticalSparesApprovedBySupt.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuList.AccessRights = this.ViewState;
            MenuList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCriticalSpares.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    UcVesselSearch.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVesselSearch.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDVESSELNAME", "FLDPRIORITY", "FLDFORMNO", "FLDCREATEDDATE", "FLDFORMSTATUS", "FLDNAME", "FLDPOCOMMITTEDAMOUNT", "FLDORDERREADINESSDATE", "FLDVENDORNAME" };
        string[] alCaptions = { "Vessel Name", "Priority", "Reqn No.", "Reqn Date", "Status", "Equipment", "Unit Price (USD)", "Readiness Date", "Vendor" };

        DataSet ds = PhoenixPurchaseOrderLineItemHighQuotedReq.CriticalSpareApprovedSearch
        (
             General.GetNullableString( txtTitle.Text)
            , General.GetNullableString( txtFormNo.Text)
            , General.GetNullableInteger(txtVendor.Text)
            , General.GetNullableDateTime(ucFromDate.Text)
            , General.GetNullableDateTime(ucToDate.Text)
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , gvCriticalSpares.PageSize
            , ref iRowCount
            , ref iTotalPageCount
            , General.GetNullableInteger(UcVesselSearch.SelectedVessel)
        );

        gvCriticalSpares.DataSource = ds;
        gvCriticalSpares.VirtualItemCount = iRowCount;

        General.SetPrintOptions("gvCriticalSpares", "Critical Spares Approved By Superintendent", alCaptions, alColumns, ds);
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvCriticalSpares_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCriticalSpares.CurrentPageIndex + 1;
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
        gvCriticalSpares.SelectedIndexes.Clear();
        gvCriticalSpares.EditIndexes.Clear();
        gvCriticalSpares.DataSource = null;
        gvCriticalSpares.Rebind();
    }
    protected void MenuList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvCriticalSpares.CurrentPageIndex = 0;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtTitle.Text = "";
                txtFormNo.Text = "";
                ucFromDate.Text = "";
                ucToDate.Text = "";
                txtVendor.Text = "";
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    UcVesselSearch.SelectedVessel = "";
                txtVendorNumber.Text = "";
                txtVenderName.Text = "";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDPRIORITY", "FLDFORMNO", "FLDCREATEDDATE", "FLDFORMSTATUS", "FLDNAME", "FLDPOCOMMITTEDAMOUNT", "FLDORDERREADINESSDATE", "FLDVENDORNAME" };
        string[] alCaptions = { "Vessel Name", "Priority", "Reqn No.", "Reqn Date", "Status", "Equipment", "Unit Price (USD)", "Readiness Date", "Vendor" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseOrderLineItemHighQuotedReq.CriticalSpareApprovedSearch
         (
              General.GetNullableString(txtTitle.Text)
             , General.GetNullableString(txtFormNo.Text)
             , General.GetNullableInteger(txtVendor.Text)
             , General.GetNullableDateTime(ucFromDate.Text)
             , General.GetNullableDateTime(ucToDate.Text)
             , sortexpression
             , sortdirection
             , (int)ViewState["PAGENUMBER"]
             , gvCriticalSpares.PageSize
             , ref iRowCount
             , ref iTotalPageCount
             , General.GetNullableInteger(UcVesselSearch.SelectedVessel)
         );
        Response.AddHeader("Content-Disposition", "attachment; filename=Requisition Approved by Superintendent.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Critical Spares Approved By Superintendent</h3></td>");
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

    protected void gvCriticalSpares_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
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
    protected void gvCriticalSpares_SortCommand(object sender, GridSortCommandEventArgs e)
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
        BindData();
    }
}