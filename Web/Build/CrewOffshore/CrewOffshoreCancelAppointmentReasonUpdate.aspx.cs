using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;

public partial class CrewOffshoreCancelAppointmentReasonUpdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
           
            ViewState["APPOINTMENTLETTERID"] = "";
            ViewState["CREWPLANID"] = "";
            ViewState["EMPLOYEEID"] = "";
            if (Request.QueryString["APPOINTMENTLETTERID"] != null && Request.QueryString["APPOINTMENTLETTERID"].ToString() != string.Empty)
            {
                ViewState["APPOINTMENTLETTERID"] = Request.QueryString["APPOINTMENTLETTERID"].ToString();
            }

            if (Request.QueryString["CREWPLANID"] != null && Request.QueryString["CREWPLANID"].ToString() != string.Empty)
            {
                ViewState["CREWPLANID"] = Request.QueryString["CREWPLANID"].ToString();
            }
            if (Request.QueryString["EMPLOYEEID"] != null && Request.QueryString["EMPLOYEEID"].ToString() != string.Empty)
                ViewState["EMPLOYEEID"] = Request.QueryString["EMPLOYEEID"].ToString();
            BindData();
        }
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuCancelReason.AccessRights = this.ViewState;
        MenuCancelReason.MenuList = toolbarmain.Show();

    }
    protected void MenuCancelReason_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string Script = "";
            if (!IsValidUpdate())
            {
                ucError.Visible = true;
                return;
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["APPOINTMENTLETTERID"].ToString() != "" && ViewState["CREWPLANID"].ToString() != "")
                {
                    int n = PhoenixCrewOffshoreContract.CancelAppointmentLetter(
                                General.GetNullableGuid(ViewState["APPOINTMENTLETTERID"].ToString())
                                , General.GetNullableGuid(ViewState["CREWPLANID"].ToString())
                                , General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString())
                                , General.GetNullableDateTime(ucDate.Text)
                                , txtCancelReason.Text);
                }
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('CI','ifMoreInfo');";
                    Script += "</script>" + "\n";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                    BindData();
                
            }
            
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
    protected void BindData()
    {
        DataTable dt = PhoenixCrewOffshoreContract.EditAppointmentLetter(General.GetNullableGuid(ViewState["APPOINTMENTLETTERID"].ToString())
                                                                       , General.GetNullableGuid(ViewState["CREWPLANID"].ToString()));

        if (dt.Rows.Count > 0)
        {
            ucDate.Text = dt.Rows[0]["FLDCANCELLATIONDATE"].ToString();
            txtCancelReason.Text = dt.Rows[0]["FLDCANCELLATIONREASON"].ToString();
        }
    }
    private bool IsValidUpdate()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableDateTime(ucDate.Text) == null)
            ucError.ErrorMessage = "Cancellation Date is required.";
        
        if (General.GetNullableString(txtCancelReason.Text) == null)
            ucError.ErrorMessage = "Cancel Reason is required.";                 
        
        return (!ucError.IsError);
    }
}

