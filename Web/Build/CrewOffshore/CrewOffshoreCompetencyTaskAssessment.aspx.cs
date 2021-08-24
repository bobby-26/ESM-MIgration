using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreCompetencyTaskAssessment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["VESSELID"] = "";
            if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            {
                UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                UcVessel.Enabled = false;
            }
            else
            {
                if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                    UcVessel.SelectedVessel = ViewState["VESSELID"].ToString();
            }
            BindCrew();
            ViewState["EMPLOYEEID"] = "";
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Submit", "SUBMIT", ToolBarDirection.Right);
            CrewTab.AccessRights = this.ViewState;
            CrewTab.MenuList = toolbar1.Show();
        }
    }

    public void BindCrew()
    {
        DataTable dt = PhoenixCrewOffshoreTaskAssessment.OnboardCrewList(General.GetNullableInteger(ViewState["VESSELID"].ToString()));
        ddlcrew.Items.Clear();
        ddlcrew.Items.Insert(0, new RadComboBoxItem("Select", ""));
        ddlcrew.DataTextField = "FLDNAME";
        ddlcrew.DataValueField = "FLDEMPLOYEEID";

        ddlcrew.DataSource = dt;
        ddlcrew.DataBind();
    }
    protected void UcVessel_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = UcVessel.SelectedVessel;
        BindCrew();
    }

    protected void ddlcrew_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.TrainingneedsEmployeeList(General.GetNullableInteger(ddlcrew.SelectedValue.ToString()));

        lblfname.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
        txtfileno.Text = dt.Rows[0]["FLDFILENO"].ToString();
        lblrname.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
        ViewState["EMPLOYEEID"] = ddlcrew.SelectedValue.ToString();

        BindData();
        gvTask.Rebind();
    }

    public void BindData()
    {
        DataTable dt = PhoenixCrewOffshoreTaskAssessment.AssessmentTaskList(General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()));
        gvTask.DataSource = dt;
    }

    protected void gvTask_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvTask_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadComboBox ddlstatus = (RadComboBox)e.Item.FindControl("ddlstatus");
            if (ddlstatus != null)
            {
                DataTable dt = PhoenixCrewOffshoreTaskAssessment.TaskStatusList();
                ddlstatus.Items.Clear();
                ddlstatus.Items.Insert(0, new RadComboBoxItem("Select", ""));
                ddlstatus.DataTextField = "FLDQUICKNAME";
                ddlstatus.DataValueField = "FLDQUICKCODE";

                ddlstatus.DataSource = dt;
                ddlstatus.DataBind();
            }
        }
    }

    protected void CrewTab_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SUBMIT"))
            {
                string remarks = ",";
                string completedate = ",";
                string status = ",";
                string trainingid = ",";
              

                foreach (GridDataItem item in gvTask.Items)
                {
                    completedate += ((UserControlDate)item.FindControl("ucdate")).Text + ",";
                    remarks += (((RadTextBox)item.FindControl("txtremarks")).Text.Trim()) + ",";
                    status += (((RadComboBox)item.FindControl("ddlstatus")).SelectedValue) + ",";
                    trainingid += ((RadLabel)item.FindControl("lblTrainingNeedId")).Text + ",";
                    
                }

                PhoenixCrewOffshoreTaskAssessment.TaskUpdate(completedate,status,remarks,trainingid);
                BindData();
                gvTask.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}