using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;
using Telerik.Web.UI;
public partial class DashboardKPI : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["measureid"] != null)
                    ViewState["MEASUREID"] = Request.QueryString["measureid"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidKpi(string fromc, string toc)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((toc == null) || (toc == ""))
            ucError.ErrorMessage = "Range From is required.";

        if ((fromc == null) || (fromc == ""))
            ucError.ErrorMessage = "Range To is required.";

        return (!ucError.IsError);
    }
    protected void Rebind()
    {
        gvKpi.SelectedIndexes.Clear();
        gvKpi.EditIndexes.Clear();
        gvKpi.DataSource = null;
        gvKpi.Rebind();
    }
    protected void gvKpi_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;



            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string vesselid = ((RadComboBox)e.Item.FindControl("ddlvessellistAdd")).SelectedValue;
                string fromc = (((UserControlMaskNumber)e.Item.FindControl("txtFromCAdd")).Text);
                string toc = (((UserControlMaskNumber)e.Item.FindControl("txtToCAdd")).Text);
                string color = ((RadComboBox)e.Item.FindControl("ddlColorAdd")).SelectedValue;


                PhoenixDashboardOption.DashboardKPIInsert(new Guid(ViewState["MEASUREID"].ToString())
                    , General.GetNullableInteger(vesselid), General.GetNullableInteger(fromc), General.GetNullableInteger(toc)
                    , color, null);


                Rebind();

                String script = "javascript:fnReloadList('codehelp1', 'true', 'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string kpiid = (((RadLabel)e.Item.FindControl("lblkpi")).Text);
                PhoenixDashboardOption.DashboardKPIDelete(new Guid(kpiid));

                String script = "javascript:fnReloadList('codehelp1', 'true', 'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string kpiid = (((RadLabel)e.Item.FindControl("lblkpiEdit")).Text);
                string vesselid = ((RadComboBox)e.Item.FindControl("ddlvessellist")).SelectedValue;
                string fromc = (((UserControlMaskNumber)e.Item.FindControl("txtFromCEdit")).Text);
                string toc = (((UserControlMaskNumber)e.Item.FindControl("txtToCEdit")).Text);
                string color = ((RadComboBox)e.Item.FindControl("ddlColorEdit")).SelectedValue;


                PhoenixDashboardOption.DashboardKPIInsert(new Guid(ViewState["MEASUREID"].ToString())
                    , General.GetNullableInteger(vesselid), General.GetNullableInteger(fromc), General.GetNullableInteger(toc)
                    , color, General.GetNullableGuid(kpiid));

                String script = "javascript:fnReloadList('codehelp1', 'true', 'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            Rebind();

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvKpi_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {

                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadComboBox ddlColorEdit = (RadComboBox)e.Item.FindControl("ddlColorEdit");
                if (ddlColorEdit != null)
                {
                    ddlColorEdit.DataSource = PhoenixCommonDashboard.DashboardCssColorList();
                    ddlColorEdit.DataBind();
                    ddlColorEdit.SelectedValue = drv["FLDCOLOR"].ToString();
                }
                RadComboBox ddlvessellist = (RadComboBox)e.Item.FindControl("ddlvessellist");
                if (ddlvessellist != null)
                {
                    ddlvessellist.DataSource = PhoenixDashboardOption.DashboarVesselList();
                    ddlvessellist.DataBind();
                    ddlvessellist.SelectedValue = drv["FLDVESSELID"].ToString();
                    ddlvessellist.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                }

            }

            if (e.Item is GridFooterItem)
            {
                RadComboBox ddlColorAdd = (RadComboBox)e.Item.FindControl("ddlColorAdd");
                if (ddlColorAdd != null)
                {
                    ddlColorAdd.DataSource = PhoenixCommonDashboard.DashboardCssColorList();
                    ddlColorAdd.DataBind();
                }

                RadComboBox ddlvesselAdd = (RadComboBox)e.Item.FindControl("ddlvessellistAdd");
                if (ddlvesselAdd != null)
                {
                    ddlvesselAdd.DataSource = PhoenixDashboardOption.DashboarVesselList();
                    ddlvesselAdd.DataBind();
                    ddlvesselAdd.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                }

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void gvKpi_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvKpi.CurrentPageIndex + 1;

        BindData();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixDashboardOption.DashboardKPISearch(
            new Guid(ViewState["MEASUREID"].ToString()),
            sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        gvKpi.DataSource = ds;
        gvKpi.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvKpi_SortCommand(object sender, GridSortCommandEventArgs e)
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
    }
}
