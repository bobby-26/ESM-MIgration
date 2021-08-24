using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Integration;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;

public partial class InspectionPlan : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuInspectionPlan.AccessRights = this.ViewState;
            MenuInspectionPlan.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["SCHEDULEID"] = string.Empty;
                ViewState["INSPECTIONID"] = string.Empty;
                ViewState["InspectionPlanid"] = string.Empty;
                ViewState["PLANNERID"] = string.Empty;
                ViewState["VESSELID"] = string.Empty;

                if (Request.QueryString["SCHEDULEID"] != null && Request.QueryString["SCHEDULEID"].ToString() != string.Empty)
                    ViewState["SCHEDULEID"] = Request.QueryString["SCHEDULEID"].ToString();

                if (Request.QueryString["PLANNERID"] != null && Request.QueryString["PLANNERID"].ToString() != string.Empty)
                    ViewState["PLANNERID"] = Request.QueryString["PLANNERID"].ToString();

                if (Request.QueryString["INSPECTIONID"] != null && Request.QueryString["INSPECTIONID"].ToString() != string.Empty)
                    ViewState["INSPECTIONID"] = Request.QueryString["INSPECTIONID"].ToString();

                if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != string.Empty)
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();

               BindInspectionPlan();

                if (ViewState["InspectionPlanid"] == null || ViewState["InspectionPlanid"].ToString() == string.Empty)
                {
                    SaveAuditPlan();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void InspectionPlan_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                SaveAuditPlan();
               
                ucStatus.Text = "Audit Plan added successfully.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    
    private void BindInspectionPlan()
    {
        DataSet ds = PhoenixInspectionPlan.EditInspectionPlan(General.GetNullableGuid(ViewState["SCHEDULEID"].ToString())
            , General.GetNullableGuid(ViewState["PLANNERID"].ToString())
            , General.GetNullableInteger(ViewState["VESSELID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ViewState["InspectionPlanid"] = dr["FLDINSPECTIONPLANID"].ToString();
            uclastncobscount.Text = dr["FLDLASTINSPECTIONNCCOUNT"].ToString();
            ucClosedncobs.Text = dr["FLDCLOSEDINSPECTIONNCCOUNT"].ToString();
            ucnotclosedncobs.Text = dr["FLDNOTCLOSEDINSPECTIONNCCOUNT"].ToString();
            txtauditorcomments.Text = dr["FLDINSPECTORCOMMENTS"].ToString();
            txtSupervisorcomments.Text = dr["FLDSUPERVISORCOMMENTS"].ToString();
            txtsupervisorName.Text = dr["FLDSUPERVISORNAME"].ToString();
            txtSupervisorRank.Text = dr["FLDSUPERVISORDESIGNATION"].ToString();
            txtContact.Text = dr["FLDCONTACT"].ToString();
            lblassessmentstds.Text = dr["FLDASSESSMENTSTANDARDS"].ToString();

        }
    }
    
    protected void gvAuditteam_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet dss = new DataSet();
            if (ViewState["InspectionPlanid"] != null && ViewState["InspectionPlanid"].ToString() != string.Empty)
            {
                 dss = PhoenixInspectionPlan.listMeetingpersons(new Guid(ViewState["InspectionPlanid"].ToString()), 0);                
            }
            gvAuditteam.DataSource = dss;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvAuditteam_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {                
                string Name = ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text;
                string Rank = ((RadTextBox)e.Item.FindControl("txtRankAdd")).Text;
                if (!IsValidMeeting(Name, Rank))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionPlan.InsertInspectionPlanMeeting(General.GetNullableGuid(null), new Guid(ViewState["InspectionPlanid"].ToString()), Name, Rank, 0);
                
                gvAuditteam.Rebind();
                ucStatus.Text = "Audit team added successfully.";
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionPlan.DeleteMeetingperson(new Guid(((RadLabel)e.Item.FindControl("lblmeetingid")).Text));
                
                gvAuditteam.Rebind();
                ucStatus.Text = "Audit team deleted successfully.";
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string Name = ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text;
                string Rank = ((RadTextBox)e.Item.FindControl("txtRankEdit")).Text;
                if (!IsValidMeeting(Name, Rank))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionPlan.InsertInspectionPlanMeeting(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblmeetingidedit")).Text)
                    , new Guid(ViewState["InspectionPlanid"].ToString())
                    , Name
                    , Rank
                    , 0);
                                
                gvAuditteam.Rebind();
                ucStatus.Text = "Audit team updated successfully.";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOpeningMeeting_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string Name = ((RadTextBox)e.Item.FindControl("txtOpenNameAdd")).Text;
                string Rank = ((RadTextBox)e.Item.FindControl("txtOpenRankAdd")).Text;
                if (!IsValidMeeting(Name, Rank))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionPlan.InsertInspectionPlanMeeting(General.GetNullableGuid(null), new Guid(ViewState["InspectionPlanid"].ToString()), Name, Rank, 1);

                ucStatus.Text = "Updated successfully.";
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionPlan.DeleteMeetingperson(new Guid(((RadLabel)e.Item.FindControl("lblOpeningmeetingid")).Text));
                ucStatus.Text = "Updated successfully.";
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string Name = ((RadTextBox)e.Item.FindControl("txtOpenNameEdit")).Text;
                string Rank = ((RadTextBox)e.Item.FindControl("txtOpenRankEdit")).Text;
                if (!IsValidMeeting(Name, Rank))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionPlan.InsertInspectionPlanMeeting(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOpeningmeetingidedit")).Text)
                    , new Guid(ViewState["InspectionPlanid"].ToString())
                    , Name
                    , Rank
                    , 1);

                ucStatus.Text = "Audit team updated successfully.";
            }
            gvOpeningMeeting.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOpeningMeeting_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet dss = new DataSet();
            if (ViewState["InspectionPlanid"] != null && ViewState["InspectionPlanid"].ToString() != string.Empty)
            {
                 dss = PhoenixInspectionPlan.listMeetingpersons(new Guid(ViewState["InspectionPlanid"].ToString()), 1);                
            }
            gvOpeningMeeting.DataSource = dss;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvCloseMeeting_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string Name = ((RadTextBox)e.Item.FindControl("txtCloseNameAdd")).Text;
                string Rank = ((RadTextBox)e.Item.FindControl("txtCloseRankAdd")).Text;
                if (!IsValidMeeting(Name, Rank))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionPlan.InsertInspectionPlanMeeting(General.GetNullableGuid(null), new Guid(ViewState["InspectionPlanid"].ToString()), Name, Rank, 2);

                ucStatus.Text = "Updated successfully.";
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionPlan.DeleteMeetingperson(new Guid(((RadLabel)e.Item.FindControl("lblClosegmeetingid")).Text));
                ucStatus.Text = "Updated successfully.";
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string Name = ((RadTextBox)e.Item.FindControl("txtCloseNameEdit")).Text;
                string Rank = ((RadTextBox)e.Item.FindControl("txtCloseRankEdit")).Text;
                if (!IsValidMeeting(Name, Rank))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionPlan.InsertInspectionPlanMeeting(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblClosegmeetingidedit")).Text)
                    , new Guid(ViewState["InspectionPlanid"].ToString())
                    , Name
                    , Rank
                    , 2);

                ucStatus.Text = "Updated successfully.";
            }
            gvCloseMeeting.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCloseMeeting_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet dss = new DataSet();
            if (ViewState["InspectionPlanid"] != null && ViewState["InspectionPlanid"].ToString() != string.Empty)
            {
                dss = PhoenixInspectionPlan.listMeetingpersons(new Guid(ViewState["InspectionPlanid"].ToString()), 2);
                
            }
            gvCloseMeeting.DataSource = dss;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private bool IsValidMeeting(string Name, string Rank)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Name.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (Rank.Trim().Equals(""))
            ucError.ErrorMessage = "Rank is required.";

        return (!ucError.IsError);
    }
    private void SaveAuditPlan()
    {
        Guid Newid = new Guid();

        PhoenixInspectionPlan.InsertInspectionPlan(General.GetNullableGuid(ViewState["InspectionPlanid"].ToString())
            , new Guid(ViewState["INSPECTIONID"].ToString())
            , General.GetNullableInteger(ViewState["VESSELID"].ToString())
            , General.GetNullableGuid(ViewState["SCHEDULEID"].ToString())
            , General.GetNullableGuid(ViewState["PLANNERID"].ToString())
            , txtauditorcomments.Text
            , txtsupervisorName.Text
            , txtSupervisorRank.Text
            , txtSupervisorcomments.Text
            , General.GetNullableInteger(uclastncobscount.Text)
            , General.GetNullableInteger(ucClosedncobs.Text)
            , General.GetNullableInteger(ucnotclosedncobs.Text)
            , txtContact.Text
            , ref Newid);

        ViewState["InspectionPlanid"] = Newid;
    }
}