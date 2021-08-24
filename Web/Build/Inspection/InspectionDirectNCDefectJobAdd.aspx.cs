using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionDirectNCDefectJobAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        txtComponentId.Attributes.Add("style", "visibility:hidden");
        try
        {
            PhoenixToolbar toolbarNoonReporttap = new PhoenixToolbar();
            toolbarNoonReporttap.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuSave.AccessRights = this.ViewState;
            MenuSave.MenuList = toolbarNoonReporttap.Show();

            if(!IsPostBack)
            {
                ViewState["REVIEWDNC"] = null;
                if (!string.IsNullOrEmpty(Request.QueryString["REVIEWDNC"]))
                {
                    ViewState["REVIEWDNC"] = Request.QueryString["REVIEWDNC"].ToString();
                }
                GetVesselId();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSave_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string componentid = txtComponentId.Text;
                string Details = txtdetailsofthedefect.Text;
                string Duedate = ucDueDate.Text;
                string Responsibility = ucDisciplineResponsibility.SelectedDiscipline;

                if (!IsValidDefectjob(Details, Duedate))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid defectJobId = Guid.Empty;
                PhoenixInspectionWorkOrder.DefectJobInsert(General.GetNullableGuid(componentid), Details,
                                                                   DateTime.Parse(Duedate), 
                                                                   General.GetNullableInteger(Responsibility),
                                                                   ref defectJobId,int.Parse(ViewState["VESSELID"].ToString()));

                PhoenixInspectionWorkOrder.InspectionDefectJobMap(defectJobId, new Guid(ViewState["REVIEWDNC"].ToString()),int.Parse(ViewState["DEFICIENCYTYPE"].ToString()), null, int.Parse(ViewState["VESSELID"].ToString()));

                gvDefectJob.Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private bool IsValidDefectjob(string Details, string Duedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (string.IsNullOrEmpty(Details))
            ucError.ErrorMessage = "Defect Details is required.";

        if (string.IsNullOrEmpty(Duedate))
            ucError.ErrorMessage = "Due Date is required.";

        return (!ucError.IsError);
    }

    protected void gvDefectJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    private void BindData()
    {
        try
        {
            if (ViewState["REVIEWDNC"] != null && ViewState["VESSELID"] != null)
            {
                DataTable dt = PhoenixInspectionWorkOrder.InspectionDefectJobList(new Guid(ViewState["REVIEWDNC"].ToString()), int.Parse(ViewState["VESSELID"].ToString()));
                gvDefectJob.DataSource = dt;
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void GetVesselId()
    {
        if (ViewState["REVIEWDNC"] != null && ViewState["REVIEWDNC"].ToString() != "")
        {
            DataSet ds = new DataSet();
            ViewState["DEFICIENCYTYPE"] = 0;
            ds = PhoenixInspectionAuditNonConformity.EditAuditNC(new Guid(ViewState["REVIEWDNC"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ViewState["DEFICIENCYTYPE"] = 1;
            }

            ds = PhoenixInspectionAuditDirectNonConformity.EditDirectObservation(new Guid(ViewState["REVIEWDNC"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ViewState["DEFICIENCYTYPE"] = 2;
            }

            imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?vesselid=" + ViewState["VESSELID"] + "&framename=ifMoreInfo', true); ");

            gvDefectJob.Rebind();
        }
    }
}