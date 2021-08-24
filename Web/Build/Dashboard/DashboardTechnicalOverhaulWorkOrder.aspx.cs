using System;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;

public partial class DashboardTechnicalOverhaulWorkOrder : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Filter.CurrentWorkOrderReportLogFilter = null;
            ViewState["days"] = 15;
            ViewState["RESP"] = string.Empty;
            ViewState["ISCRITICAL"] = string.Empty;
            GetDateRange();
        }
    }

    protected void gvComponentCategpry_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (sender as RadGrid);
        DataSet ds = PhoenixDashboardTechnical.DashboardOverHaulWorkorder(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (ds.Tables.Count > 1)
        {
            DataTable dt = ds.Tables[1];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
               // grid.MasterTableView.ColumnGroups.FindGroupByName("cat" + (i + 1)).HeaderText = dt.Rows[i]["FLDHEADER"].ToString();
            }
        }
        grid.DataSource = ds;
    }

    protected void ddlDueDaysCat1_TextChanged(object sender, EventArgs e)
    {
        RadComboBox ddl = sender as RadComboBox;
        if (ddl.ID == "ddlResponsibility")
        {
            ViewState["RESP"] = ddl.SelectedValue;
        }
        else
        {
            ViewState["days"] = ddl.SelectedValue;
        }
        GetDateRange();
        gvComponentCategpry.DataSource = null;
        gvComponentCategpry.MasterTableView.Rebind();
    }

    protected void gvComponentCategpry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        LinkButton lnkMojorOverdue = (LinkButton)e.Item.FindControl("lnkMajorOverDue");
        if (lnkMojorOverdue != null)
        { 
            if(drv["FLDMAJOROVERDUE"].ToString() == "0")
                lnkMojorOverdue.Enabled = false;
            else
                lnkMojorOverdue.Attributes.Add("onclick", "javascript:top.openNewWindow('jnp','OverHaul','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalOverhaulWorkOrderList.aspx?componentId="+drv["FLDCOMPONENTID"]+"&d=0&OverDue=1&jobclass=MOH'); return false;");
        }
        LinkButton lnkMojor30 = (LinkButton)e.Item.FindControl("lnkMajorDue30");
        if (lnkMojor30 != null)
        {
            if (drv["FLDMAJORDUE30"].ToString() == "0")
                lnkMojor30.Enabled = false;
            else
                lnkMojor30.Attributes.Add("onclick", "javascript:top.openNewWindow('jnp','OverHaul','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalOverhaulWorkOrderList.aspx?componentId=" + drv["FLDCOMPONENTID"] + "&d=30&OverDue=0&jobclass=MOH'); return false;");
        }
        LinkButton lnkMajorDue60 = (LinkButton)e.Item.FindControl("lnkMajorDue60");
        if (lnkMajorDue60 != null)
        {
            if (drv["FLDMAJORDUE60"].ToString() == "0")
                lnkMajorDue60.Enabled = false;
            else
                lnkMajorDue60.Attributes.Add("onclick", "javascript:top.openNewWindow('jnp','OverHaul','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalOverhaulWorkOrderList.aspx?componentId=" + drv["FLDCOMPONENTID"] + "&d=60&OverDue=0&jobclass=MOH'); return false;");
        }
        LinkButton lnkOverHaulOverDue = (LinkButton)e.Item.FindControl("lnkOverHaulOverDue");
        if (lnkOverHaulOverDue != null)
        {
            if (drv["FLDOVERHAULOVERDUE"].ToString() == "0")
                lnkOverHaulOverDue.Enabled = false;
            else
                lnkOverHaulOverDue.Attributes.Add("onclick", "javascript:top.openNewWindow('jnp','OverHaul','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalOverhaulWorkOrderList.aspx?componentId=" + drv["FLDCOMPONENTID"] + "&d=0&OverDue=1&jobclass=OH'); return false;");
        }
        LinkButton lnkOverHaul30 = (LinkButton)e.Item.FindControl("lnkOverHaul30");
        if (lnkOverHaul30 != null)
        {
            if (drv["FLDOVERHAULDUE30"].ToString() == "0")
                lnkOverHaul30.Enabled = false;
            else
                lnkOverHaul30.Attributes.Add("onclick", "javascript:top.openNewWindow('jnp','OverHaul','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalOverhaulWorkOrderList.aspx?componentId=" + drv["FLDCOMPONENTID"] + "&d=30&OverDue=0&jobclass=OH'); return false;");
        }
        LinkButton lnkOverHaul60 = (LinkButton)e.Item.FindControl("lnkOverHaul60");
        if (lnkOverHaul60 != null)
        {
            if (drv["FLDOVERHAULDUE60"].ToString() == "0")
                lnkOverHaul60.Enabled = false;
            else
                lnkOverHaul60.Attributes.Add("onclick", "javascript:top.openNewWindow('jnp','OverHaul','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalOverhaulWorkOrderList.aspx?componentId=" + drv["FLDCOMPONENTID"] + "&d=60&OverDue=0&jobclass=OH'); return false;");
        }

    }
    private void GetDateRange()
    {
        if (ViewState["days"].ToString() == "15")
        {
            ViewState["FDATE"] = General.GetDateTimeToString(DateTime.Now);
            ViewState["TDATE"] = General.GetDateTimeToString(DateTime.Now.AddDays(15));
        }
        else if (ViewState["days"].ToString() == "30")
        {
            ViewState["FDATE"] = General.GetDateTimeToString(DateTime.Now);
            ViewState["TDATE"] = General.GetDateTimeToString(DateTime.Now.AddDays(30));
        }
        else if (ViewState["days"].ToString() == "90")
        {
            ViewState["FDATE"] = General.GetDateTimeToString(DateTime.Now);
            ViewState["TDATE"] = General.GetDateTimeToString(DateTime.Now.AddDays(90));
        }
    }

    protected void gvComponentCategpry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if(e.CommandName == RadGrid.FilterCommandName)
        {
            ViewState["RESP"] = grid.MasterTableView.GetColumn("FLDCOMPONENTCATEGORY").CurrentFilterValue;
            ViewState["days"] = grid.MasterTableView.GetColumn("FLDCAT1AND2NOTPLANNEDDUECOUNT").CurrentFilterValue;
            if(ViewState["days"].ToString().Equals(""))
            {
                ViewState["days"] = "15";
            }
            gvComponentCategpry.DataSource = null;
            gvComponentCategpry.MasterTableView.Rebind();
        }
    }

    protected void ddlResponsibility_DataBinding(object sender, EventArgs e)
    {
        RadComboBox ddlDiscipline = sender as RadComboBox;           
        ddlDiscipline.DataSource = PhoenixRegistersDiscipline.ListDiscipline();
        ddlDiscipline.DataTextField = "FLDDISCIPLINENAME";
        ddlDiscipline.DataValueField = "FLDDISCIPLINEID";        
        ddlDiscipline.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ddlDiscipline.SelectedValue = ViewState["RESP"].ToString();
    }

    protected void chkIsCritical_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox chk = (RadCheckBox)sender;
        ViewState["ISCRITICAL"] = chk.Checked.HasValue && chk.Checked.Value ? "1" : string.Empty;        
        GetDateRange();
        gvComponentCategpry.DataSource = null;
        gvComponentCategpry.MasterTableView.Rebind();
    }
}