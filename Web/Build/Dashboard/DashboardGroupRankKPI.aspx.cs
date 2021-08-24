using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class DashboardGroupRankKPI : PhoenixBasePage
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

                gvKpi.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixDashboardOption.DashboardGroupRankKPISearch(
            new Guid(ViewState["MEASUREID"].ToString()),
            sortexpression,
            sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvKpi.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        gvKpi.DataSource = ds;
        gvKpi.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;


    }


    protected void gvKpi_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                string rankid = ((RadComboBox)e.Item.FindControl("ddlGroupRankAdd")).SelectedValue;
                string fromc = (((UserControlMaskNumber)e.Item.FindControl("txtFromCAdd")).Text);
                string toc = (((UserControlMaskNumber)e.Item.FindControl("txtToCAdd")).Text);
                string color = ((RadComboBox)e.Item.FindControl("ddlColorAdd")).SelectedValue;
                
                if (!IsValidKpi(fromc, toc))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDashboardOption.DashboardRankKPIInsert(new Guid(ViewState["MEASUREID"].ToString())
                    , General.GetNullableInteger(rankid), General.GetNullableInteger(fromc), General.GetNullableInteger(toc)
                    , color, null);

                gvKpi.Rebind();

                String script = "javascript:fnReloadList('codehelp1', 'true', 'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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

    protected void gvKpi_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvKpi.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvKpi_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
        }

        if (e.Item.IsInEditMode)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadComboBox gre = (RadComboBox)e.Item.FindControl("ddlGroupRankEdit");
            DataRowView drvGroupRank = (DataRowView)e.Item.DataItem;
            if (gre != null)
            {
                DataSet ds = new DataSet();
                ds = PhoenixRegistersGroupRank.ListGroupRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                gre.DataSource = ds;
                gre.DataTextField = "FLDGROUPRANK";
                gre.DataValueField = "FLDGROUPRANKID";
                gre.DataBind();
                gre.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                gre.SelectedValue = DataBinder.Eval(e.Item.DataItem, "FLDGROUPRANKID").ToString();
            }


            RadComboBox ddlColorEdit = (RadComboBox)e.Item.FindControl("ddlColorEdit");
            if (ddlColorEdit != null)
            {
                ddlColorEdit.DataSource = PhoenixCommonDashboard.DashboardCssColorList();
                ddlColorEdit.DataBind();

             
            }
        }

        if (e.Item is GridFooterItem)
        {
            RadComboBox gra = (RadComboBox)e.Item.FindControl("ddlGroupRankAdd");
            if (gra != null)
            {
                DataSet ds = new DataSet();
                ds = PhoenixRegistersGroupRank.ListGroupRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                gra.DataSource = ds;
                gra.DataTextField = "FLDGROUPRANK";
                gra.DataValueField = "FLDGROUPRANKID";
                gra.DataBind();
                gra.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }

            RadComboBox ddlColorAdd = (RadComboBox)e.Item.FindControl("ddlColorAdd");
            if (ddlColorAdd != null)
            {
                ddlColorAdd.DataSource = PhoenixCommonDashboard.DashboardCssColorList();
                ddlColorAdd.DataBind();
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }

    }

    protected void gvKpi_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        string kpiid = (((RadLabel)e.Item.FindControl("lblkpi")).Text);
        PhoenixDashboardOption.DashboardRankKPIDelete(new Guid(kpiid));

        String script = "javascript:fnReloadList('codehelp1', 'true', 'true');";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }


    protected void gvKpi_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string kpiid = (((RadLabel)e.Item.FindControl("lblkpiEdit")).Text);
            string rankid = ((RadComboBox)e.Item.FindControl("ddlGroupRankEdit")).SelectedValue;
            string fromc = (((UserControlMaskNumber)e.Item.FindControl("txtFromCEdit")).Text);
            string toc = (((UserControlMaskNumber)e.Item.FindControl("txtToCEdit")).Text);
            string color = ((RadComboBox)e.Item.FindControl("ddlColorEdit")).SelectedValue;

            PhoenixDashboardOption.DashboardRankKPIInsert(new Guid(ViewState["MEASUREID"].ToString())
                , General.GetNullableInteger(rankid), General.GetNullableInteger(fromc), General.GetNullableInteger(toc)
                , color, General.GetNullableGuid(kpiid));

            gvKpi.Rebind();

            String script = "javascript:fnReloadList('codehelp1', 'true', 'true');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}