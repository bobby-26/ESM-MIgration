using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Inspection;
using System.Data;
using Telerik.Web.UI;
public partial class CrewDeBriefingCrewComplaints : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("SAVE", "SAVE",ToolBarDirection.Right);
            MenuRemarks.AccessRights = this.ViewState;
            MenuRemarks.MenuList = toolbarmain.Show();

            if (Request.QueryString["sigonoffid"] != null && Request.QueryString["sigonoffid"].ToString() != string.Empty)
            {
                ViewState["SIGONOFFID"] = Request.QueryString["sigonoffid"].ToString();

            }
            if (Request.QueryString["Vesselid"] != null && Request.QueryString["Vesselid"].ToString() != string.Empty)
            {
                ViewState["VESSELID"] = Request.QueryString["Vesselid"].ToString();
            }
            if (Request.QueryString["vesselname"] != null && Request.QueryString["vesselname"].ToString() != string.Empty)
            {
                txtVesselName.Text = Request.QueryString["vesselname"].ToString();

            }

            if (Request.QueryString["Rankid"] != null && Request.QueryString["Rankid"].ToString() != string.Empty)
            {
                ViewState["RANKID"] = Request.QueryString["Rankid"].ToString();
                txtRank.Text = ViewState["RANKID"].ToString();

            }
            if (Request.QueryString["Name"] != null && Request.QueryString["Name"].ToString() != string.Empty)
            {
                ViewState["NAME"] = Request.QueryString["Name"].ToString();
                txtName.Text = ViewState["NAME"].ToString();
            }
            if (Request.QueryString["Crewcomplaintid"] != null && Request.QueryString["Crewcomplaintid"].ToString() != string.Empty)
            {
                ViewState["CREWCOMPLAINTID"] = Request.QueryString["Crewcomplaintid"].ToString();
                BindInspectionIncident();
            }

        }
    }

    private void BindInspectionIncident()
    {

        DataSet ds;

        if (ViewState["CREWCOMPLAINTID"] != null && !string.IsNullOrEmpty(ViewState["CREWCOMPLAINTID"].ToString()))
        {
            ds = PhoenixInspectionIncident.CrewComplaintEdit(new Guid(ViewState["CREWCOMPLAINTID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtRank.Text = dr["FLDREPORTEDBYRANK"].ToString();
                txtName.Text = dr["FLDREPORTEDBYNAME"].ToString();
                txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
                txtRefNumber.Text = dr["FLDREFERENCENUMBER"].ToString();
                txtRemarks.Text = dr["FLDSUMMARY"].ToString();

                MenuRemarks.Visible = false;

            }
        }
    }
    protected void MenuRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (General.GetNullableString(txtRemarks.Text) == null)
            {
                lblMessage.Text = "Crew Complaint Remarks is required.";
                return;
            }

            string Script = "";

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixCrewDeBriefing.CrewComplaintsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , txtRemarks.Text.Trim(), int.Parse(ViewState["VESSELID"].ToString()),
                   General.GetNullableString(ViewState["RANKID"].ToString()), General.GetNullableString(ViewState["NAME"].ToString()),
                   General.GetNullableInteger(ViewState["SIGONOFFID"].ToString())
                   );

                //PhoenixCrewDeBriefing.updateCrewComplaints(General.GetNullableInteger(ViewState["SIGONOFFID"].ToString()),PhoenixSecurityContext.CurrentSecurityContext.UserCode);

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            BindInspectionIncident();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
}
