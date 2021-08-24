using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Data;
using System;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Web.UI.WebControls;

public partial class PlannedMaintenanceCompJobMainSummaryInsert : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceGlobalComponentJob.aspx?JOBSUMMARYID=" + Request.QueryString["JOBSUMMARYID"] + "');", "Global Component", "<i class=\"fas fa-plus-circle\"></i>", "CREATE");

            menuGlobalComponent.AccessRights = this.ViewState;
            menuGlobalComponent.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuWorkOrder.AccessRights = this.ViewState;
            MenuWorkOrder.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvGlobalComponent.PageSize = General.ShowRecords(gvGlobalComponent.PageSize);

                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["JOBSUMMARYID"] = string.Empty;
                ViewState["JOBCOUNT"] = "0";

                BindVesselFleetList();
                BindVesselTypeList();
                BindVesselList();

                if (!string.IsNullOrEmpty(Request.QueryString["JOBSUMMARYID"]))
                {
                    ViewState["JOBSUMMARYID"] = Request.QueryString["JOBSUMMARYID"];
                    BindSummaryDetail();
                }
                rbBtnList.Enabled = false;

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
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDivWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvGlobalComponent.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void menuGlobalComponent_TabStripCommand(object sender, EventArgs e)
    {

    }
    protected void menuJob_TabStripCommand(object sender, EventArgs e)
    {

    }
    protected void gvGlobalComponent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGlobalComponent();
    }

    protected void BindGlobalComponent()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCODE", "FLDJOBTITLE" };
            string[] alCaptions = { "Comp. No.", "Comp. Name", "Job Code", " Job Title" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds;
            ds = PhoenixPlannedMaintenanceCompJobSummary.GlobalComponentJobSummarySearch(General.GetNullableGuid(ViewState["JOBSUMMARYID"].ToString()), null, null, sortexpression, sortdirection
                                                                            , gvGlobalComponent.CurrentPageIndex + 1
                                                                            , gvGlobalComponent.PageSize, ref iRowCount
                                                                            , ref iTotalPageCount);

            gvGlobalComponent.DataSource = ds;
            gvGlobalComponent.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                Save();
            }
            else if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceCompJobMainSummaryList.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Save()
    {
        if (string.IsNullOrEmpty(ViewState["JOBSUMMARYID"].ToString()))
        {
            return;
        }
        string vessellist = GetCsvValue(ddlVessel);
        if (!IsValidJobSummary(txtTitle.Text, rbBtnList.SelectedValue))
        {
            ucError.Visible = true;
            return;

        }
        PhoenixPlannedMaintenanceCompJobSummary.CompJobSummaryInsert(new Guid(ViewState["JOBSUMMARYID"].ToString()), txtTitle.Text, vessellist, General.GetNullableInteger(rbBtnList.SelectedValue));
        ucStatus.Text = "Saved Successfully...";
        ucStatus.Visible = true;
        Response.Redirect("../PlannedMaintenance/PlannedMaintenanceCompJobMainSummaryList.aspx", false);
    }
    private bool IsValidJobSummary(string title, string type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (title.Trim().Equals(""))
            ucError.ErrorMessage = "Title is required";

        if (type.Trim().Equals(""))
            ucError.ErrorMessage = "Type is required";

        if (gvGlobalComponent.VirtualItemCount < 1)
        {
            ucError.ErrorMessage = "Job is required";
        }
        if (ddlVessel.CheckedItems.Count < 1)
        {
            ucError.ErrorMessage = "Vessels Applicable is required";
        }

        return (!ucError.IsError);
    }

    protected void gvGlobalComponent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            gvGlobalComponent.SelectedIndexes.Clear();

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string gsummaryId = item.GetDataKeyValue("FLDGLOBALCOMPONENTSUMMARYID").ToString();
                PhoenixPlannedMaintenanceCompJobSummary.GlobalCompJobDelete(new Guid(gsummaryId));

                gvGlobalComponent.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindSummaryDetail()
    {
        DataTable dt;
        dt = PhoenixPlannedMaintenanceCompJobSummary.CompjobSummaryEdit(new Guid(ViewState["JOBSUMMARYID"].ToString()));

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            txtTitle.Text = dt.Rows[0]["FLDTITLE"].ToString();
            rbBtnList.SelectedValue = dt.Rows[0]["FLDAUTHORIZEDTYPE"].ToString();
            SetCsvValue(ddlVessel, dt.Rows[0]["FLDVESSELLIST"].ToString());
        }
    }
    protected void gvGlobalComponent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }
            LinkButton jdbtn = (LinkButton)e.Item.FindControl("cmdJobDesc");
            if (jdbtn != null)
            {
                jdbtn.Attributes.Add("onclick", "return openNewWindow('spnPickReason', 'codehelp1', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobDetail.aspx?framename=ifMoreInfo&JOBID=" + drv["FLDJOBID"].ToString() + "','false','1066','320'); return false;");
            }
        }
    }

    protected void ddlOwner_TextChangedEvent(object sender, EventArgs e)
    {
        UserControlMultiColumnAddress address = (UserControlMultiColumnAddress)sender;
        SetFilter(address.SelectedValue, null, null);
    }
    protected void ddlFleet_ItemChecked(object sender, RadComboBoxItemEventArgs e)
    {
        RadComboBox radComboBox = (RadComboBox)sender;
        string strfleetlist = GetCsvValue(radComboBox);
        SetFilter(null, strfleetlist, null);
        radComboBox.OpenDropDownOnLoad = true;
        ddlVeselType.OpenDropDownOnLoad = false;
    }
    protected void ddlVeselType_ItemChecked(object sender, RadComboBoxItemEventArgs e)
    {
        RadComboBox radComboBox = (RadComboBox)sender;
        string csvList = GetCsvValue(radComboBox);
        SetFilter(null, null, csvList);
        ddlFleet.OpenDropDownOnLoad = false;
        radComboBox.OpenDropDownOnLoad = true;
    }
    private string GetCsvValue(RadComboBox radComboBox)
    {
        var list = radComboBox.CheckedItems;
        string csv = string.Empty;
        if (list.Count != 0)
        {
            csv = ",";
            foreach (var item in list)
            {
                csv = csv + item.Value + ",";
            }
        }
        return csv;
    }
    private void SetCsvValue(RadComboBox radComboBox, string csvValue)
    {
        foreach (RadComboBoxItem item in radComboBox.Items)
        {
            item.Checked = false;
            if (csvValue.Contains("," + item.Value + ","))
            {
                item.Checked = true;
            }
        }
    }
    private void SetFilter(string owner, string fleet, string vesseltype)
    {
        string techfleet = string.Empty;
        techfleet = fleet;
        
        DataSet ds = PhoenixPlannedMaintenanceCompJobSummary.DashboarVesselList(techfleet, null, null, vesseltype, owner);

        foreach (RadComboBoxItem item in ddlVessel.Items)
            item.Checked = false;

        foreach (RadComboBoxItem item in ddlFleet.Items)
            item.Checked = false;

        foreach (RadComboBoxItem item in ddlVeselType.Items)
            item.Checked = false;

        if ((fleet != null && fleet.Trim().Equals("")) || (owner != null && owner.Trim().Equals(""))
            || (vesseltype != null && vesseltype.Trim().Equals(""))) return;
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                RadComboBoxItem item = ddlVessel.FindItemByValue(dr["FLDVESSELID"].ToString());
                if (item != null) item.Checked = true;
            }
            DataView view = new DataView(dt);
            DataTable vslType = view.ToTable(true, "FLDTYPE");
            foreach (DataRow dr in vslType.Rows)
            {
                RadComboBoxItem item = ddlVeselType.FindItemByValue(dr["FLDTYPE"].ToString());
                if (item != null) item.Checked = true;
            }
            view = new DataView(dt);
            string field = "FLDTECHFLEET";
            DataTable flt = view.ToTable(true, field);
            foreach (DataRow dr in flt.Rows)
            {
                RadComboBoxItem item = ddlFleet.FindItemByValue(dr[field].ToString());
                if (item != null) item.Checked = true;
            }
        }
    }
    private void BindVesselList()
    {
        DataSet ds = PhoenixPlannedMaintenanceCompJobSummary.DashboarVesselList();
        ddlVessel.DataSource = ds;
        ddlVessel.DataBind();
    }

    private void BindVesselFleetList()
    {
        DataSet ds = PhoenixRegistersFleet.ListFleet();
        ddlFleet.DataSource = ds;
        ddlFleet.DataBind();
    }
    private void BindVesselTypeList()
    {
        DataSet ds = PhoenixRegistersVesselType.ListVesselType(1);
        ddlVeselType.DataSource = ds;
        ddlVeselType.DataBind();
    }
}
