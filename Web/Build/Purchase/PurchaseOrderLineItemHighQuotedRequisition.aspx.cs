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
public partial class PurchaseOrderLineItemHighQuotedRequisition : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseOrderLineItemHighQuotedRequisition.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvHighestQuotedReq')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseOrderLineItemHighQuotedRequisition.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseOrderLineItemHighQuotedRequisition.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuList.AccessRights = this.ViewState;
            MenuList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvHighestQuotedReq.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if(PhoenixSecurityContext.CurrentSecurityContext.VesselID>0)
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

        string[] alColumns = { "FLDVESSELNAME", "FLDFORMNO", "FLDPRIORITY", "FLDNAME", "FLDFORMSTATUS", "FLDCREATEDDATE", "FLDPOCOMMITTEDAMOUNT", "FLDVENDORNAME", "FLDQUOTEDPRICE" };
        string[] alCaptions = { "Vessel Name", "Reqn No.", "Priority", "PR Description", "Status", "PR Creation Date", "PO Amount", "Vendor", "Unit Price" };

        if (ddlStockType.SelectedValue == "SPARE" || ddlStockType.SelectedValue == "SERVICE")
        {
            DataSet ds = PhoenixPurchaseOrderLineItemHighQuotedReq.LineItemHighQuotedSpareSearch
            (
                txtTitle.Text
                , txtFormNo.Text
                , General.GetNullableInteger(txtVendor.Text)
                , General.GetNullableDateTime(ucFromDate.Text)
                , General.GetNullableDateTime(ucToDate.Text)
                , General.GetNullableInteger(UcVesselSearch.SelectedVessel)
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvHighestQuotedReq.PageSize
                , ref iRowCount
                , ref iTotalPageCount
                , General.GetNullableString(ddlStockType.SelectedValue)
            );

            gvHighestQuotedReq.DataSource = ds;
            gvHighestQuotedReq.VirtualItemCount = iRowCount;

            General.SetPrintOptions("gvHighestQuotedReq", "Requisition Approved by Superintendent", alCaptions, alColumns, ds);
        }
        else
        {
            DataSet ds = PhoenixPurchaseOrderLineItemHighQuotedReq.LineItemHighQuotedStoreSearch
            (
                txtTitle.Text
                , txtFormNo.Text
                , General.GetNullableInteger(txtVendor.Text)
                , General.GetNullableDateTime(ucFromDate.Text)
                , General.GetNullableDateTime(ucToDate.Text)
                , General.GetNullableInteger(UcVesselSearch.SelectedVessel)
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvHighestQuotedReq.PageSize
                , ref iRowCount
                , ref iTotalPageCount
                , General.GetNullableString(ddlStockType.SelectedValue)
            );

            gvHighestQuotedReq.DataSource = ds;
            gvHighestQuotedReq.VirtualItemCount = iRowCount;

            General.SetPrintOptions("gvHighestQuotedReq", "Requisition Approved by Superintendent", alCaptions, alColumns, ds);
        }
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvHighestQuotedReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvHighestQuotedReq.CurrentPageIndex + 1;
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
        gvHighestQuotedReq.SelectedIndexes.Clear();
        gvHighestQuotedReq.EditIndexes.Clear();
        gvHighestQuotedReq.DataSource = null;
        gvHighestQuotedReq.Rebind();
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
                gvHighestQuotedReq.CurrentPageIndex = 0;
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

        string[] alColumns = { "FLDVESSELNAME", "FLDFORMNO", "FLDPRIORITY", "FLDNAME", "FLDFORMSTATUS", "FLDCREATEDDATE", "FLDPOCOMMITTEDAMOUNT", "FLDVENDORNAME", "FLDQUOTEDPRICE" };
        string[] alCaptions = { "Vessel Name", "Reqn No.", "Priority", "PR Description", "Status", "PR Creation Date", "PO Amount", "Vendor", "Unit Price" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (ddlStockType.SelectedValue == "SPARE" || ddlStockType.SelectedValue == "SERVICE")
        {
            ds = PhoenixPurchaseOrderLineItemHighQuotedReq.LineItemHighQuotedSpareSearch
            (
                 txtTitle.Text
                , txtFormNo.Text
                , General.GetNullableInteger(txtVendor.Text)
                , General.GetNullableDateTime(ucFromDate.Text)
                , General.GetNullableDateTime(ucToDate.Text)
                , General.GetNullableInteger(UcVesselSearch.SelectedVessel)
                , sortexpression
                , sortdirection
                , 1
                , iRowCount
                , ref iRowCount
                , ref iTotalPageCount
                , General.GetNullableString(ddlStockType.SelectedValue)
            );
        }
        else
        {
            ds = PhoenixPurchaseOrderLineItemHighQuotedReq.LineItemHighQuotedStoreSearch
            (
                 txtTitle.Text
                , txtFormNo.Text
                , General.GetNullableInteger(txtVendor.Text)
                , General.GetNullableDateTime(ucFromDate.Text)
                , General.GetNullableDateTime(ucToDate.Text)
                , General.GetNullableInteger(UcVesselSearch.SelectedVessel)
                , sortexpression
                , sortdirection
                , 1
                , iRowCount
                , ref iRowCount
                , ref iTotalPageCount
                , General.GetNullableString(ddlStockType.SelectedValue)
            );
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=Requisition Approved by Superintendent.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Requisition Approved by Superintendent</h3></td>");
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

    protected void gvHighestQuotedReq_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvHighestQuotedReq_SortCommand(object sender, GridSortCommandEventArgs e)
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