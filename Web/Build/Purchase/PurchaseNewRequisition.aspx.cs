using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;


public partial class PurchaseNewRequisition : PhoenixBasePage
{
    string vesselname;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Purchase/PurchaseNewRequisition.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Purchase/PurchaseNewRequisition.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbar.AddImageButton("../Purchase/PurchaseNewRequisition.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('rgvForm')", "Print Grid", "icon_print.png", "PRINT");
            MenuNewRequisition.AccessRights = this.ViewState;
            MenuNewRequisition.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                VesselConfiguration();
                MenuNewRequisition.SelectedMenuIndex = 0;
                if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
                    Response.Redirect("PhoenixLogout.aspx");
                if (Request.QueryString["pageno"] != null)
                {
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["pageno"].ToString());
                    rgvForm.CurrentPageIndex = int.Parse(Request.QueryString["pageno"].ToString()) - 1;
                }
                else
                    ViewState["PAGENUMBER"] = 1;

                rgvForm.PageSize = General.ShowRecords(null);

                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucVessel.Enabled = true;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    txtVessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName.ToString();
                    txtVessel.Visible = true;
                    ucVessel.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixCommonPurchase.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            rgvForm.Rebind();
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
        string[] alColumns = { "FLDSTOCKTYPE", "FLDFORMNO", "FLDTITLE", "FLDSUBACCOUNT", "FLDVENDORNAME", "FLDAMOUNTINUSD" };
        string[] alCaptions = { "Type", "Number", "Title", "Budget Code", "Vendor", "Committed(USD)" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (!string.IsNullOrEmpty(txtVessel.Text))
            vesselname = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        else
            vesselname = ucVessel.SelectedVessel.ToString();



        ds = PhoenixPurchaseSearch.NewRequisitionSearch(vesselname, txtFormNo.Text.Trim(),
              txtTitle.Text.Trim()
              , General.GetNullableDateTime(txtFromDate.Text), General.GetNullableDateTime(txtToDate.Text)
              , ddlStockType.SelectedValue,
              sortexpression, sortdirection, 1, rgvForm.VirtualItemCount>0? rgvForm.VirtualItemCount:10,
                  ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=NewRequisitions.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>New Requisitions</center></h3></td>");
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
                if (alCaptions[i].ToUpper().Equals("FLDCREATEDDATE") || alCaptions[i].ToUpper().Equals("FLDPURCHASEAPPROVEDATE") || alCaptions[i].ToUpper().Equals("FLDORDEREDDATE"))
                    Response.Write(General.GetDateTimeToString(dr[alColumns[i]]));
                else
                    Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void MenuNewRequisition_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                rgvForm.CurrentPageIndex = 0;
                rgvForm.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFormNo.Text = "";
                txtTitle.Text = "";
                ucVessel.SelectedVessel = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ddlStockType.SelectedValue = "";
                rgvForm.Rebind();
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

    protected void ddlStockType_TextChanged(object sender, EventArgs e)
    {
        rgvForm.Rebind();
    }

    protected void rgvForm_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSTOCKTYPE", "FLDFORMNO", "FLDTITLE", "FLDSUBACCOUNT", "FLDVENDORNAME", "FLDAMOUNTINUSD" };
        string[] alCaptions = { "Type", "Number", "Title", "Budget Code", "Vendor", "Committed(USD)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : ViewState["SORTEXPRESSION"].ToString();


        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (!string.IsNullOrEmpty(txtVessel.Text))
            vesselname = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        else
            vesselname = ucVessel.SelectedVessel.ToString();

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseSearch.NewRequisitionSearch(General.GetNullableString(vesselname), txtFormNo.Text.Trim(), txtTitle.Text.Trim()
                                                                , General.GetNullableDateTime(txtFromDate.Text)
                                                                , General.GetNullableDateTime(txtToDate.Text)
                                                                , General.GetNullableString(ddlStockType.SelectedValue)
                                                                , sortexpression
                                                                , sortdirection
                                                                , rgvForm.CurrentPageIndex+1
                                                                , rgvForm.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        rgvForm.DataSource = ds;
        rgvForm.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("rgvForm", "New Requisitions", alCaptions, alColumns, ds);
    }

    protected void rgvForm_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            RadLabel lblPriorityFlag = (RadLabel)item.FindControl("lblPriorityFlag");

            RadLabel lnkFormNumberName = (RadLabel)item.FindControl("lblFormNumber");
            Int64 result = 0;

            if (Int64.TryParse(lblPriorityFlag.Text, out result))
            {
                item.ForeColor = (result == 1) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                lnkFormNumberName.ForeColor = (result == 1) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            }
            DataRowView drv = (DataRowView)item.DataItem;
            ImageButton cmdLineItems = (ImageButton)item["ACTION"].FindControl("cmdLineItems");
            if (cmdLineItems != null)
                cmdLineItems.Attributes.Add("onclick", "openNewWindow('detail', '', 'Purchase/PurchaseLineItem.aspx?orderid=" + drv["FLDORDERID"] + "&VesselId=" + drv["FLDVESSELID"] + "&StockType=" + drv["FLDSTOCKTYPE"] + "&StockClass=" + drv["FLDSTOCKCLASSID"] + "'); return false;");
            ImageButton cmdQuotation = (ImageButton)item["ACTION"].FindControl("cmdQuotation");
            if (cmdQuotation != null)
                cmdQuotation.Attributes.Add("onclick", "openNewWindow('detail', '', 'Purchase/PurchaseQuotation.aspx?orderid=" + drv["FLDORDERID"] + "&VesselId=" + drv["FLDVESSELID"] + "&StockType=" + drv["FLDSTOCKTYPE"] + "&StockClass=" + drv["FLDSTOCKCLASSID"] + "&FormNo=" + drv["FLDFORMNO"] + "'); return false;");
            ImageButton cmdFormList = (ImageButton)item["ACTION"].FindControl("cmdFormList");
            if (cmdFormList != null)
                cmdFormList.Attributes.Add("onclick", "openNewWindow('detail', '', 'Purchase/PurchaseFormDetails.aspx?orderid=" + drv["FLDORDERID"] + "&VesselId=" + drv["FLDVESSELID"] + "'); return false;");
        }
    }

    protected void rgvForm_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void rgvForm_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        rgvForm.Rebind();
    }

    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        rgvForm.Rebind();
    }
}
