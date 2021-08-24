using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using System.Web;
using Telerik.Web.UI;

public partial class VesselPositionSTSOperationUpdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarvoyagelist = new PhoenixToolbar();
            toolbarvoyagelist.AddFontAwesomeButton("../VesselPosition/VesselPositionSTSOperationUpdate.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbarvoyagelist.AddFontAwesomeButton("javascript:CallPrint('gvSTSOperation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarvoyagelist.AddFontAwesomeButton("../VesselPosition/VesselPositionSTSOperationUpdate.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbarvoyagelist.AddFontAwesomeButton("../VesselPosition/VesselPositionSTSOperationUpdate.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");

            MenuVoyageList.AccessRights = this.ViewState;
            MenuVoyageList.MenuList = toolbarvoyagelist.Show();

            if (!IsPostBack)
            {
                UcVessel.bind();
                UcVessel.DataBind();
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                }
                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvSTSOperation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
 
    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDDATE", "FLDREPORTTYPE", "FLDOPERATIONTYPE", "FLDCAGOQTY", "FLDSTSOPERATION" };
        string[] alCaptions = { "Vessel", "Date", "Report Type", "Operation Type", "Cargo Qty", "STS Y/N" };

        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixVesselPositionSTSOperationUpdate.STSOperationSearch(
        General.GetNullableInteger(UcVessel.SelectedVessel),
           General.GetNullableDateTime(txtReportFrom.Text),
           General.GetNullableDateTime(txtReportTo.Text),
           sortdirection,
           int.Parse(ViewState["PAGENUMBER"].ToString()),
           iRowCount, ref iRowCount, ref iTotalPageCount
           );

        Response.AddHeader("Content-Disposition", "attachment; filename=\"STSOperation.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>STS Operation</h3></td>");
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
   
    protected void VoyageList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }

        if (CommandName.ToUpper().Equals("RESET"))
        {
            ViewState["PAGENUMBER"] = 1;
            ClearFilter();
        }
    }

    private void ClearFilter()
    {
        ViewState["PAGENUMBER"] = 1;
        txtReportFrom.Text = "";
        txtReportTo.Text = "";
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
        {
            UcVessel.SelectedVessel = "";
        }
        Rebind();
    }
    protected void gvSTSOperation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSTSOperation.CurrentPageIndex + 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionSTSOperationUpdate.STSOperationSearch(
        General.GetNullableInteger(UcVessel.SelectedVessel),
           General.GetNullableDateTime(txtReportFrom.Text),
           General.GetNullableDateTime(txtReportTo.Text),
           sortdirection,
           int.Parse(ViewState["PAGENUMBER"].ToString()),
           gvSTSOperation.PageSize, ref iRowCount, ref iTotalPageCount
           );


        string[] alColumns = { "FLDVESSELNAME", "FLDDATE", "FLDREPORTTYPE", "FLDOPERATIONTYPE", "FLDCAGOQTY", "FLDSTSOPERATION" };
        string[] alCaptions = { "Vessel", "Date", "Report Type", "Operation Type", "Cargo Qty", "STS Y/N" };

        General.SetPrintOptions("gvSTSOperation", "STS Operation", alCaptions, alColumns, ds);

        gvSTSOperation.DataSource = ds;
        gvSTSOperation.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void Rebind()
    {
        gvSTSOperation.SelectedIndexes.Clear();
        gvSTSOperation.EditIndexes.Clear();
        gvSTSOperation.DataSource = null;
        gvSTSOperation.Rebind();
    }
    protected void gvSTSOperation_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvSTSOperation_ItemCommand(object sender, GridCommandEventArgs e)
    {

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            int? activeyn = ((RadCheckBox)e.Item.FindControl("chkSTS")).Checked == true ? 1 : 0;
            Guid? documentsyn = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOperationid")).Text);
            PhoenixVesselPositionSTSOperationUpdate.STSOperationUpdate(documentsyn, activeyn);
            Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }
}
