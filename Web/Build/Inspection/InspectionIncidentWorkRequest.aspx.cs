using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class InspectionIncidentWorkRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarmain.AddButton("New", "NEW",ToolBarDirection.Right);
        if (Filter.CurrentSelectedIncidentMenu == null)
        {
            MenuWorkOrderRequestion.AccessRights = this.ViewState;
            MenuWorkOrderRequestion.MenuList = toolbarmain.Show();
        }
        txtComponentId.Attributes.Add("style", "visibility:hidden");
        txtJobId.Attributes.Add("style", "visibility:hidden");
        
        if (!IsPostBack)
        {
            ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;            
            ViewState["NEW"] = "false";
            BindData();   
            BindFields();
            gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        imgJob.Attributes.Add("onclick", "return showPickList('spnPickListJob', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJob.aspx', true); ");
        BindComponents();
    }

    private void BindComponents()
    {
        DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?vesselid=" + ViewState["VESSELID"] + "&framename=ifMoreInfo', true); ");
        }
    }

    private void BindFields()
    {
        if (ViewState["WORKORDERID"] != null)
        {
            DataSet ds = PhoenixInspectionWorkOrder.EditWorkOrder(new Guid(ViewState["WORKORDERID"].ToString()));
            DataRow dr = ds.Tables[0].Rows[0];
            lblWorkOrderID.Text = dr["FLDWORKORDERID"].ToString();
            txtWorkOrderNumber.Text = dr["FLDWORKORDERNUMBER"].ToString();
            txtTitle.Text = dr["FLDWORKORDERNAME"].ToString();
            txtJobDescription.Text = dr["FLDDETAILS"].ToString();
            txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
            txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
            txtComponentId.Text = dr["FLDCOMPONENTID"].ToString();
            txtCreatedDate.Text = General.GetDateTimeToString(dr["FLDCREATEDDATE"].ToString());
            if (dr["FLDWORKISUNEXPECTED"].ToString().Equals("1"))
                chkUnexpected.Checked = true;
            txtDuration.Text = dr["FLDPLANNINGESTIMETDURATION"].ToString();
            txtPlannedStartDate.Text = General.GetDateTimeToString(dr["FLDPLANNINGDUEDATE"].ToString());
            txtPriority.Text = dr["FLDPLANINGPRIORITY"].ToString();
            ucDiscipline.SelectedDiscipline = dr["FLDPLANNINGDISCIPLINE"].ToString();
            ucWTOApproval.SelectedHard = dr["FLDAPPROVALTYPE"].ToString();
            txtJobId.Text = dr["FLDJOBID"].ToString();
            txtJobName.Text = dr["FLDJOBTITLE"].ToString();
            txtJobCode.Text = dr["FLDJOBCODE"].ToString();
            ucMainType.SelectedQuick = dr["FLDWORKMAINTNANCETYPE"].ToString();
        }
        else
            ResetTextBox();
    }

    protected void MenuWorkOrderRequestion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["NEW"].ToString().ToUpper() != "TRUE")
                {
                    ucError.ErrorMessage = "Sorry,You can not make any more changes here.";
                    ucError.Visible = true;
                    return;
                }
                if (!IsValidRequisition(txtComponentId.Text, txtTitle.Text, txtPlannedStartDate.Text, txtJobId.Text, txtJobDescription.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string workorderid = null;

                byte? isDefect = null;

                if (chkIsDefect.Checked.Equals(true))
                    isDefect = byte.Parse("1");

                PhoenixInspectionWorkOrder.InsertWorkOrder(
                    int.Parse(ViewState["VESSELID"].ToString()), null,
                    General.GetNullableString(txtTitle.Text), General.GetNullableGuid(txtComponentId.Text), null,
                    General.GetNullableGuid(txtJobId.Text), null, null, null,
                    General.GetNullableInteger(ucWTOApproval.SelectedHard).HasValue ? 24 : 501, null,
                    General.GetNullableInteger(txtPriority.Text), General.GetNullableInteger(txtDuration.Text),
                    General.GetNullableDateTime(txtPlannedStartDate.Text),
                    General.GetNullableInteger(ucDiscipline.SelectedDiscipline), null, null, null, General.GetNullableInteger(ucMainType.SelectedQuick), null, null,
                    chkUnexpected.Checked == true ? "1" : "0", General.GetNullableInteger(ucWTOApproval.SelectedHard),
                    isDefect, ref workorderid, General.GetNullableGuid(Filter.CurrentIncidentID));
                if (txtJobDescription.Text != "")
                    PhoenixInspectionWorkOrder.UpdateDetailsWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(workorderid), General.GetNullableString(txtJobDescription.Text));
                ViewState["WORKORDERID"] = workorderid;

                PhoenixInspectionIncidentDamage.InsertIncidentWorkOrder
                                       (PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                       , new Guid(Filter.CurrentIncidentID)
                                       , new Guid(workorderid)
                                       , new Guid(txtComponentId.Text)
                                       , 1);

                ViewState["NEW"] = "false";
                Rebind();
                BindFields();
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                ResetTextBox();
                ViewState["WORKORDERID"] = null;
                ViewState["NEW"] = "true";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ResetTextBox()
    {
        txtWorkOrderNumber.Text = "";
        txtTitle.Text = "";
        txtComponentCode.Text = "";
        txtComponentName.Text = "";
        txtComponentId.Text = "";
        txtCreatedDate.Text = General.GetDateTimeToString(DateTime.Now.ToString());
        chkUnexpected.Checked = true;
        txtDuration.Text = "";
        txtPlannedStartDate.Text = "";
        txtPriority.Text = "1";
        ucDiscipline.SelectedDiscipline = "";
        txtJobDescription.Text = "";
        txtJobId.Text = "";
        txtJobName.Text = "";
        txtJobCode.Text = "";
        ucWTOApproval.SelectedHard = "";
        ucMainType.SelectedQuick = "";
    }
    private bool IsValidRequisition(string componentid, string title, string plannedstartdate, string jobid, string workdetails)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (componentid.Trim().Equals(""))
            ucError.ErrorMessage = "Component is required";

        if (title.Trim().Equals(""))
            ucError.ErrorMessage = "Title is required";

        if (!General.GetNullableDateTime(plannedstartdate).HasValue)
            ucError.ErrorMessage = "Planned Start Date is required";

        if (string.IsNullOrEmpty(jobid) && string.IsNullOrEmpty(workdetails))
            ucError.ErrorMessage = "Either Job Description or Work Details is required";

        return (!ucError.IsError);
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixInspectionIncidentDamage.InspectionIncidentWorkRequestSearch(
                             new Guid(Filter.CurrentIncidentID) ,                          
                             sortexpression, sortdirection,
                             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                             gvWorkOrder.PageSize,
                             ref iRowCount,
                             ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["OPERATIONMODE"] = "EDIT";
                gvWorkOrder.SelectedIndexes.Clear();

                if (ViewState["WORKORDERID"] == null)
                {
                    ViewState["WORKORDERID"] = ds.Tables[0].Rows[0]["FLDWORKORDERID"].ToString();
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ViewState["NEW"] = "true";
            }
            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvWorkOrder_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    //gvWorkOrder.SelectedIndex = se.NewSelectedIndex;
    //    ViewState["WORKORDERID"] = ((RadLabel)gvWorkOrder.Items[se.NewSelectedIndex].FindControl("lblWorkOrderId")).Text;
    //    BindFields();
    //}

    protected void gvWorkOrder_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkOrder.CurrentPageIndex + 1;
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
        gvWorkOrder.SelectedIndexes.Clear();
        gvWorkOrder.EditIndexes.Clear();
        gvWorkOrder.DataSource = null;
        gvWorkOrder.Rebind();
    }

    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["WORKORDERID"] = ((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text;
                e.Item.Selected = true;
                BindFields();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
