using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionRescheduleTaskSuperintendentApproval : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Approve", "APPROVE",ToolBarDirection.Right);
        MenuSuperintendentComment.AccessRights = this.ViewState;
        MenuSuperintendentComment.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {           
            ViewState["RESCHEDULEREASON"] = "";
            ViewState["OLDTARGETDATE"] = "";
            ViewState["VESSELID"] = "";
            ViewState["RECENTRESCHEDULEDATE"] = "";

        }
        if (Request.QueryString["CORRECTIVEACTIONID"] != null && Request.QueryString["CORRECTIVEACTIONID"].ToString() != string.Empty)
        {
            ViewState["CORRECTIVEACTIONID"] = Request.QueryString["CORRECTIVEACTIONID"].ToString();
        }

        if (Request.QueryString["viewonly"] != null && Request.QueryString["viewonly"].ToString() != string.Empty)
        {
            ViewState["viewonly"] = Request.QueryString["viewonly"].ToString();
            lblSuperintendentComments.Text = "Secondary Approval Comments";
            BindSecondaryComments();
        }
        BindSuperintendentComments();
    }

    protected void MenuSuperintendentComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (General.GetNullableString(txtSuperintendentComments.Text) == null)
            {
                lblMessage.Text = "Superintendent Comments is required.";
                return;
            }

            if (General.GetNullableString(ViewState["RESCHEDULEREASON"].ToString()) != null && General.GetNullableDateTime(ucRescheduleDate.Text) == null)
            {
                lblMessage.Text = "Reschedule Date is required.";
                return;
            }

            string Script = "";
            if ((ViewState["CORRECTIVEACTIONID"] != null) && (ViewState["VESSELID"].ToString() != "0") && ViewState["viewonly"] != null)
            {
                lblMessage.Text = "";
                if (CommandName.ToUpper().Equals("APPROVE"))
                {
                    PhoenixInspectionLongTermAction.RescheduleTaskSecondaryApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                            , new Guid(ViewState["CORRECTIVEACTIONID"].ToString())
                                                                                            , General.GetNullableString(txtSuperintendentComments.Text));

                    if (General.GetNullableDateTime(ucRescheduleDate.Text) != null)
                    {
                        if (General.GetNullableDateTime(ViewState["RECENTRESCHEDULEDATE"].ToString()) != null)
                        {
                            if (DateTime.Compare(DateTime.Parse(ucRescheduleDate.Text), DateTime.Parse(ViewState["RECENTRESCHEDULEDATE"].ToString())) != 0)
                            {
                                PhoenixInspectionSchedule.ReScheduleHistoryInsert(
                                                                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , new Guid(ViewState["CORRECTIVEACTIONID"].ToString())
                                                                   , General.GetNullableString(ViewState["RESCHEDULEREASON"].ToString())
                                                                   , General.GetNullableDateTime(ViewState["OLDTARGETDATE"].ToString())
                                                                   , General.GetNullableDateTime(ucRescheduleDate.Text)
                                                                   , int.Parse(ViewState["VESSELID"].ToString())
                                                                   );
                            }
                            else
                            {
                                PhoenixInspectionSchedule.ReScheduleHistoryInsert(
                                                                         PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                       , new Guid(ViewState["CORRECTIVEACTIONID"].ToString())
                                                                       , General.GetNullableString(ViewState["RESCHEDULEREASON"].ToString())
                                                                       , General.GetNullableDateTime(ViewState["OLDTARGETDATE"].ToString())
                                                                       , General.GetNullableDateTime(ucRescheduleDate.Text)
                                                                       , int.Parse(ViewState["VESSELID"].ToString())
                                                                       );
                            }
                        }

                        BindSecondaryComments();
                        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                        Script += "fnReloadList('CI','ifMoreInfo');";
                        Script += "</script>" + "\n";
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                    }
                }
            }
            else if ((ViewState["CORRECTIVEACTIONID"] != null) && (ViewState["VESSELID"].ToString() != "0"))
            {
                lblMessage.Text = "";
                if (CommandName.ToUpper().Equals("APPROVE"))
                {
                    PhoenixInspectionLongTermAction.RescheduleTaskSuperintendentApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                            , new Guid(ViewState["CORRECTIVEACTIONID"].ToString())
                                                                                            , General.GetNullableString(txtSuperintendentComments.Text));

                    if (General.GetNullableDateTime(ucRescheduleDate.Text) != null)
                    {
                        if (General.GetNullableDateTime(ViewState["RECENTRESCHEDULEDATE"].ToString()) != null)
                        {
                            if (DateTime.Compare(DateTime.Parse(ucRescheduleDate.Text), DateTime.Parse(ViewState["RECENTRESCHEDULEDATE"].ToString())) != 0)
                            {
                                PhoenixInspectionSchedule.ReScheduleHistoryInsert(
                                                                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , new Guid(ViewState["CORRECTIVEACTIONID"].ToString())
                                                                   , General.GetNullableString(ViewState["RESCHEDULEREASON"].ToString())
                                                                   , General.GetNullableDateTime(ViewState["OLDTARGETDATE"].ToString())
                                                                   , General.GetNullableDateTime(ucRescheduleDate.Text)
                                                                   , int.Parse(ViewState["VESSELID"].ToString())
                                                                   );
                            }
                            else
                            {
                                PhoenixInspectionSchedule.ReScheduleHistoryInsert(
                                                                         PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                       , new Guid(ViewState["CORRECTIVEACTIONID"].ToString())
                                                                       , General.GetNullableString(ViewState["RESCHEDULEREASON"].ToString())
                                                                       , General.GetNullableDateTime(ViewState["OLDTARGETDATE"].ToString())
                                                                       , General.GetNullableDateTime(ucRescheduleDate.Text)
                                                                       , int.Parse(ViewState["VESSELID"].ToString())
                                                                       );
                            }
                        }

                        BindSuperintendentComments();
                        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                        Script += "fnReloadList('CI','ifMoreInfo');";
                        Script += "</script>" + "\n";
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                    }
                }
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    private void BindSuperintendentComments()
    {
        DataSet ds = PhoenixInspectionLongTermAction.ShipBoardTaskEdit(new Guid(ViewState["CORRECTIVEACTIONID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            if (dr["FLDSUPERINTENDENTAPPROVEDYN"].ToString() == "1")
            {
                txtSuperintendentComments.Text = dr["FLDSUPERINTENDENTCOMMENTS"].ToString();
            }
            else
            {
                txtSuperintendentComments.Text = dr["FLDRESCHEDULEREASON"].ToString();
            }
            //txtApprovedByName.Text = dr["FLDAPPROVEDBY"].ToString();
            //ucApprovedDate.Text = dr["FLDSUPERINTENDENTAPPROVEDDATE"].ToString();
            ucRescheduleDate.Text = dr["FLDRECENTRESCHEDULEDATE"].ToString();
            ViewState["RESCHEDULEREASON"] = dr["FLDRESCHEDULEREASON"].ToString();
            if (General.GetNullableDateTime(dr["FLDNEWTARGETDATE"].ToString()) == null)
            {
                ViewState["OLDTARGETDATE"] = dr["FLDTARGETDATE"].ToString();
            }
            else
            {
                ViewState["OLDTARGETDATE"] = dr["FLDNEWTARGETDATE"].ToString();
            }
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
            ViewState["RECENTRESCHEDULEDATE"] = dr["FLDRECENTRESCHEDULEDATE"].ToString();
        }
    }

    private void BindSecondaryComments()
    {
        DataSet ds = PhoenixInspectionLongTermAction.ShipBoardTaskEdit(new Guid(ViewState["CORRECTIVEACTIONID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            if (dr["FLDSECONDARYAPPROVEDYN"].ToString() == "1")
            {
                txtSuperintendentComments.Text = dr["FLDSECONDARYAPPROVEDCOMMENTS"].ToString();
            }
            else
            {
                txtSuperintendentComments.Text = dr["FLDSUPERINTENDENTCOMMENTS"].ToString();
            }
            //txtApprovedByName.Text = dr["FLDAPPROVEDBY"].ToString();
            //ucApprovedDate.Text = dr["FLDSUPERINTENDENTAPPROVEDDATE"].ToString();
            ucRescheduleDate.Text = dr["FLDRECENTRESCHEDULEDATE"].ToString();
            ViewState["RESCHEDULEREASON"] = dr["FLDRESCHEDULEREASON"].ToString();
            if (General.GetNullableDateTime(dr["FLDRECENTRESCHEDULEDATE"].ToString()) == null)
            {
                ViewState["OLDTARGETDATE"] = dr["FLDTARGETDATE"].ToString();
            }
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
            ViewState["RECENTRESCHEDULEDATE"] = dr["FLDRECENTRESCHEDULEDATE"].ToString();
        }
    }
}


