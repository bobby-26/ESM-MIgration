using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionShipboardtaskRescheduleReason : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuRescheduleReason.AccessRights = this.ViewState;
        MenuRescheduleReason.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {            
            ViewState["RECENTRESCHEDULEDATE"] = "";
            ViewState["VESSELID"] = "";
            if (Request.QueryString["CORRECTIVEACTIONID"] != null && Request.QueryString["CORRECTIVEACTIONID"].ToString() != string.Empty)
            {
                ViewState["CORRECTIVEACTIONID"] = Request.QueryString["CORRECTIVEACTIONID"].ToString();
            }
            BindRescheduleReason();
        }
    }

    protected void MenuRescheduleReason_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (General.GetNullableString(txtRescheduleReason.Text) == null)
            {
                lblMessage.Text = "Reschedule reason is required.";
                return;
            }

            //if (General.GetNullableString(txtRescheduleReason.Text) != null && General.GetNullableDateTime(ucRescheduleDate.Text) == null)
            //{
            //    lblMessage.Text = "Reschedule Date is required.";
            //    return;
            //}

            //string Script = "";

            if ((ViewState["CORRECTIVEACTIONID"] != null))
            {
                lblMessage.Text = "";
                if (CommandName.ToUpper().Equals("SAVE"))
                {

                    PhoenixInspectionLongTermAction.RescheduleTaskUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                         , new Guid(ViewState["CORRECTIVEACTIONID"].ToString())
                                                                         , General.GetNullableString(txtRescheduleReason.Text)
                                                                         , General.GetNullableDateTime(ucRescheduleDate.Text));

                    BindRescheduleReason();
                    String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                    //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    //Script += "fnReloadList('CI','ifMoreInfo');";
                    //Script += "</script>" + "\n";
                    //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }


    private void BindRescheduleReason()
    {
        DataSet ds = PhoenixInspectionLongTermAction.ShipBoardTaskEdit(new Guid(ViewState["CORRECTIVEACTIONID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            if (General.GetNullableDateTime(dr["FLDRECENTRESCHEDULEDATE"].ToString()) == null)
            {
                ViewState["OLDTARGETDATE"] = dr["FLDTARGETDATE"].ToString();
            }
            else
                ViewState["OLDTARGETDATE"] = dr["FLDRECENTRESCHEDULEDATE"].ToString();
            ucRescheduleDate.Text = dr["FLDRECENTRESCHEDULEDATE"].ToString();
            ViewState["RECENTRESCHEDULEDATE"] = dr["FLDRECENTRESCHEDULEDATE"].ToString();
            if (dr["FLDSUPERINTENDENTAPPROVEDYN"].ToString() == "1" && dr["FLDSECONDARYAPPROVEDYN"].ToString() == "1")
            {
                txtRescheduleReason.Text = dr["FLDSECONDARYAPPROVEDCOMMENTS"].ToString();
            }
            else if (dr["FLDSUPERINTENDENTAPPROVEDYN"].ToString() == "1")
            {
                txtRescheduleReason.Text = dr["FLDSUPERINTENDENTCOMMENTS"].ToString();
            }
            else
            {
                txtRescheduleReason.Text = dr["FLDRESCHEDULEREASON"].ToString();
            }
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
        }
    }
}

