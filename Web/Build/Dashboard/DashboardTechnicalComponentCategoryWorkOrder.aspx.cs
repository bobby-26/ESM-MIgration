using System;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;

public partial class Dashboard_DashboardTechnicalComponentCategoryWorkOrder : PhoenixBasePage
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
        DataSet ds = PhoenixDashboardTechnical.DashboardComponentCategoryWorkorder(PhoenixSecurityContext.CurrentSecurityContext.VesselID
            , int.Parse(ViewState["days"].ToString())
            , General.GetNullableInteger(ViewState["RESP"].ToString())
            , (byte?)General.GetNullableInteger(ViewState["ISCRITICAL"].ToString()));

        if (ds.Tables.Count > 1)
        {
            DataTable dt = ds.Tables[1];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                grid.MasterTableView.ColumnGroups.FindGroupByName("cat" + (i + 1)).HeaderText = dt.Rows[i]["FLDHEADER"].ToString();
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
        LinkButton lnkCat1and2NotPlannedOverDue = (LinkButton)e.Item.FindControl("lnkCat1and2NotPlannedOverDue");
        if (lnkCat1and2NotPlannedOverDue != null)
        {
            lnkCat1and2NotPlannedOverDue.Attributes.Add("onclick", "javascript:top.openNewWindow('jnp','Jobs Not Planned','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalJobCategoryNotPlanned.aspx?jc=cat1and2&cc="+drv["FLDCOMPCATEGORY"].ToString()+"&d=0 D&resp="+ViewState["RESP"].ToString()+ "&iscr=" + ViewState["ISCRITICAL"] + "&JobNotPlan=1'); return false;");
        }
        LinkButton lnkCat2and3NotPlannedOverdue = (LinkButton)e.Item.FindControl("lnkCat2and3NotPlannedOverdue");
        if (lnkCat2and3NotPlannedOverdue != null)
        {
            lnkCat2and3NotPlannedOverdue.Attributes.Add("onclick", "javascript:top.openNewWindow('jnp','Jobs Not Planned','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalJobCategoryNotPlanned.aspx?jc=cat3and4&cc=" + drv["FLDCOMPCATEGORY"].ToString()+ "&d=0 D&resp=" + ViewState["RESP"].ToString() + "&iscr=" + ViewState["ISCRITICAL"] + "&JobNotPlan=1'); return false;");
        }
        LinkButton lnkCat1and2NotPlanned = (LinkButton)e.Item.FindControl("lnkCat1and2NotPlanned");
        if (lnkCat1and2NotPlanned != null)
        {
            lnkCat1and2NotPlanned.Attributes.Add("onclick", "javascript:top.openNewWindow('jnp','Jobs Not Planned','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalJobCategoryNotPlanned.aspx?jc=cat1and2&cc=" + drv["FLDCOMPCATEGORY"].ToString() + "&d=" + ViewState["days"] + " D&resp=" + ViewState["RESP"].ToString() + "&iscr=" + ViewState["ISCRITICAL"] + "&JobNotPlan=1'); return false;");
        }
        LinkButton lnkCat2and3NotPlanned = (LinkButton)e.Item.FindControl("lnkCat2and3NotPlanned");
        if (lnkCat2and3NotPlanned != null)
        {
            lnkCat2and3NotPlanned.Attributes.Add("onclick", "javascript:top.openNewWindow('jnp','Jobs Not Planned','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalJobCategoryNotPlanned.aspx?jc=cat3and4&cc=" + drv["FLDCOMPCATEGORY"].ToString() + "&d=" + ViewState["days"] + " D&resp=" + ViewState["RESP"].ToString() + "&iscr=" + ViewState["ISCRITICAL"] + "&JobNotPlan=1'); return false;");
        }

        LinkButton lnkCat1and2IssueOverDue = (LinkButton)e.Item.FindControl("lnkCat1and2IssueOverDue");
        if(lnkCat1and2IssueOverDue != null)
        {
            lnkCat1and2IssueOverDue.Attributes.Add("onclick", "javascript:top.openNewWindow('workorder','Work Order','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx?jc=" + drv["FLDCSVCAT1AND2"].ToString() + "&cc=" + drv["FLDCOMPCATEGORY"].ToString() + "&tdate=" + General.GetDateTimeToString(DateTime.Now.AddDays(-1)) + "&status=" + drv["FLDCSVSTATUS"].ToString() + "&res=" + ViewState["RESP"].ToString() + "&cw=" + ViewState["ISCRITICAL"] + "'); return false;");
        }
        LinkButton lnkCat1and2Issue = (LinkButton)e.Item.FindControl("lnkCat1and2Issue");
        if(lnkCat1and2Issue != null)
        {
            lnkCat1and2Issue.Attributes.Add("onclick", "javascript:top.openNewWindow('workorder','Work Order','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx?jc=" + drv["FLDCSVCAT1AND2"].ToString() + "&cc=" + drv["FLDCOMPCATEGORY"].ToString() + "&fdate=" + ViewState["FDATE"] + "&tdate=" + ViewState["TDATE"] + "&status=" + drv["FLDCSVSTATUS"].ToString() + "&res=" + ViewState["RESP"].ToString() + "&cw=" + ViewState["ISCRITICAL"] + "'); return false;");
        }
        LinkButton lnkCat3and4IssueOverDue = (LinkButton)e.Item.FindControl("lnkCat3and4IssueOverDue");
        if (lnkCat3and4IssueOverDue != null)
        {
            lnkCat3and4IssueOverDue.Attributes.Add("onclick", "javascript:top.openNewWindow('workorder','Work Order','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx?jc=" + drv["FLDCSVCAT3AND4"].ToString() + "&cc=" + drv["FLDCOMPCATEGORY"].ToString() + "&tdate=" + General.GetDateTimeToString(DateTime.Now.AddDays(-1)) + "&status=" + drv["FLDCSVSTATUS"].ToString() + "&res=" + ViewState["RESP"].ToString() + "&cw=" + ViewState["ISCRITICAL"] + "'); return false;");
        }
        LinkButton lnkCat3and4Issue = (LinkButton)e.Item.FindControl("lnkCat3and4Issue");
        if (lnkCat3and4Issue != null)
        {
            lnkCat3and4Issue.Attributes.Add("onclick", "javascript:top.openNewWindow('workorder','Work Order','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx?jc=" + drv["FLDCSVCAT3AND4"].ToString() + "&cc=" + drv["FLDCOMPCATEGORY"].ToString() + "&fdate=" + ViewState["FDATE"] + "&tdate=" + ViewState["TDATE"] + "&status=" + drv["FLDCSVSTATUS"].ToString() + "&res=" + ViewState["RESP"].ToString() + "&cw=" + ViewState["ISCRITICAL"] + "'); return false;");
        }
        if (e.Item is GridFooterItem)
        {
            string status="", jc1and2 = "", jc3and4 = "";
            DataSet ds = (DataSet)(((RadGrid)sender).DataSource);
            if(ds.Tables.Count > 0 && ds.Tables[0].Rows.Count >0)
            {
                DataTable dt = ds.Tables[0];
                status = dt.Rows[0]["FLDCSVSTATUS"].ToString();
                jc1and2 = dt.Rows[0]["FLDCSVCAT1AND2"].ToString();
                jc3and4 = dt.Rows[0]["FLDCSVCAT3AND4"].ToString();
            }
            var valueFooterCell = (e.Item as GridFooterItem)["FLDCAT1AND2NOTPLANNEDOVERDUECOUNT"];
            valueFooterCell.Text = "<a href=\"javascript:top.openNewWindow('jnp','Jobs Not Planned','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalJobCategoryNotPlanned.aspx?jc=cat1and2&d=0 D&resp=" + ViewState["RESP"].ToString() + "&iscr=" + ViewState["ISCRITICAL"] + "&JobNotPlan=1');\">" + valueFooterCell.Text + "</a>";
            valueFooterCell = (e.Item as GridFooterItem)["FLDCAT1AND2NOTPLANNEDDUECOUNT"];
            valueFooterCell.Text = "<a href=\"javascript:top.openNewWindow('jnp','Jobs Not Planned','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalJobCategoryNotPlanned.aspx?jc=cat1and2&d=" + ViewState["days"] + " D&resp=" + ViewState["RESP"].ToString() + "&iscr=" + ViewState["ISCRITICAL"] + "&JobNotPlan=1');\">" + valueFooterCell.Text + "</a>";
            valueFooterCell = (e.Item as GridFooterItem)["FLDCAT2AND3NOTPLANNEDOVERDUECOUNT"];
            valueFooterCell.Text = "<a href=\"javascript:top.openNewWindow('jnp','Jobs Not Planned','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalJobCategoryNotPlanned.aspx?jc=cat3and4&d=0 D&resp=" + ViewState["RESP"].ToString() + "&iscr=" + ViewState["ISCRITICAL"] + "&JobNotPlan=1');\">" + valueFooterCell.Text + "</a>";
            valueFooterCell = (e.Item as GridFooterItem)["FLDCAT2AND3NOTPLANNEDDUECOUNT"];
            valueFooterCell.Text = "<a href=\"javascript:top.openNewWindow('jnp','Jobs Not Planned','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalJobCategoryNotPlanned.aspx?jc=cat3and4&d=" + ViewState["days"] + " D&resp=" + ViewState["RESP"].ToString() + "&iscr=" + ViewState["ISCRITICAL"] + "&JobNotPlan=1');\">" + valueFooterCell.Text + "</a>";

            valueFooterCell = (e.Item as GridFooterItem)["FLDCAT1AND2ISSUEDOVERDUECOUNT"];
            valueFooterCell.Text = "<a href=\"javascript:top.openNewWindow('jnp','Work Orders','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx?jc="+ jc1and2 + "&tdate=" + General.GetDateTimeToString(DateTime.Now.AddDays(-1)) + "&status=" + status + "&res=" + ViewState["RESP"].ToString() + "&cw=" + ViewState["ISCRITICAL"] + "');\">" + valueFooterCell.Text + "</a>";
            valueFooterCell = (e.Item as GridFooterItem)["FLDCAT1AND2ISSUEDCOUNT"];
            valueFooterCell.Text = "<a href=\"javascript:top.openNewWindow('jnp','Work Orders','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx?jc=" + jc1and2 + "&fdate=" + ViewState["FDATE"] + "&tdate=" + ViewState["TDATE"] + "&status=" + status + "&res=" + ViewState["RESP"].ToString() + "&cw=" + ViewState["ISCRITICAL"] + "');\">" + valueFooterCell.Text + "</a>";
            valueFooterCell = (e.Item as GridFooterItem)["FLDCAT2AND3ISSUEDOVERDUECOUNT"];
            valueFooterCell.Text = "<a href=\"javascript:top.openNewWindow('jnp','Work Orders','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx?jc=" + jc3and4 + "&tdate=" + General.GetDateTimeToString(DateTime.Now.AddDays(-1)) + "&status=" + status + "&res=" + ViewState["RESP"].ToString() + "&cw=" + ViewState["ISCRITICAL"] + "');\">" + valueFooterCell.Text + "</a>";
            valueFooterCell = (e.Item as GridFooterItem)["FLDCAT2AND3ISSUEDCOUNT"];
            valueFooterCell.Text = "<a href=\"javascript:top.openNewWindow('jnp','Work Orders','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx?jc=" + jc3and4 + "&fdate=" + ViewState["FDATE"] + "&tdate=" + ViewState["TDATE"] + "&status=" + status + "&res=" + ViewState["RESP"].ToString() + "&cw=" + ViewState["ISCRITICAL"] + "');\">" + valueFooterCell.Text + "</a>";

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