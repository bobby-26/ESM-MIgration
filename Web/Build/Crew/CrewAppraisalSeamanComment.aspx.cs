using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
public partial class CrewAppraisalSeamanComment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["AppraisalStatus"] = 1;

            if (Request.QueryString["OCCASIONID"] != null)
                ViewState["OCCASIONID"] = Request.QueryString["occasionId"].ToString();


            EditAppraisal();
            SetEmployeePrimaryDetails();

            
            if(txtSeaman.Enabled)
                txtSeaman.Focus();
        }
        string canedit = "1";
        string canview = "1";
        string canpostseamancomment = "1";
        DataSet ds = PhoenixCrewAppraisal.AppraisalSecurityEdit(int.Parse(ViewState["Rankid"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));

        if (ds.Tables[0].Rows.Count > 0)
        {
            canedit = ds.Tables[0].Rows[0]["FLDCANEDIT"].ToString();
            canview = ds.Tables[0].Rows[0]["FLDCANVIEW"].ToString();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                canpostseamancomment = ds.Tables[0].Rows[0]["FLDCANPOSTCOMMENT"].ToString();
        }
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        if (canview.Equals("1") && !canpostseamancomment.Equals("0"))
        {
            toolbarmain.AddButton("Save & Finalize", "SAVE",ToolBarDirection.Right);
            MenuSeamanComment.AccessRights = this.ViewState;
            MenuSeamanComment.MenuList = toolbarmain.Show();
        }

        txtSeaman.Enabled = canpostseamancomment.Equals("1");

        toolbarmain = new PhoenixToolbar();

        if (Request.QueryString["cmd"] == null || Request.QueryString["cmd"].ToString() == "" && PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "PHOENIX")
        {
            toolbarmain.AddButton("List", "APPRAISAL");
        }
        toolbarmain.AddButton("Form", "FORM");
        toolbarmain.AddButton("Comment", "SEAMANCOMMENT");
        if (Filter.CurrentAppraisalSelection != null && ViewState["AppraisalStatus"].ToString() == "0")
        {
            toolbarmain.AddButton("Appraisal", "APPRAISALREPORT");
            toolbarmain.AddButton("Promotion", "PROMOTION");
        }

        AppraisalTabs.AccessRights = this.ViewState;
        AppraisalTabs.MenuList = toolbarmain.Show();
    }

    protected void SeamanComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidAppraisal())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["OCCASIONID"] == null || ViewState["OCCASIONID"].ToString() == string.Empty)
                {
                    PhoenixCrewAppraisal.UpdateAppraisalSeamanComment(
                        new Guid(Filter.CurrentAppraisalSelection)
                        , txtSeaman.Text);
                }
                else
                {
                    PhoenixCrewAppraisal.UpdateMidTenureAppraisalSeamanComment(
                        new Guid(Filter.CurrentAppraisalSelection)
                        , txtSeaman.Text);
                }
                ucStatus.Text = "Seafarer Comment is updated.";
                EditAppraisal();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AppraisalTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("APPRAISAL"))
            {
                Response.Redirect("CrewAppraisal.aspx", false);
            }
            if (CommandName.ToUpper().Equals("FORM"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                {
                    Response.Redirect("~/CrewOffshore/CrewOffshoreAppraisalActivity.aspx?&cmd=" + Request.QueryString["cmd"], false);
                }
                else if (ViewState["OCCASIONID"] == null || ViewState["OCCASIONID"].ToString() == string.Empty)
                {
                    Response.Redirect("CrewAppraisalActivity.aspx", false);
                }
                else
                {
                    Response.Redirect("CrewAppraisalMidtenureactivity.aspx", false);
                }

            }
            if (CommandName.ToUpper().Equals("APPRAISALREPORT"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=OFFSHOREAPPRAISAL&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0", false);
                else if (ViewState["OCCASIONID"] == null || ViewState["OCCASIONID"].ToString() == string.Empty)
                {
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=APPRAISAL&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0", false);
                }
                else
                {
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=APPRAISALMIDTENURE&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0", false);

                }
            }
            if (CommandName.ToUpper().Equals("PROMOTION"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=OFFSHOREPROMOTION&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0", false);
                else
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PROMOTION&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidAppraisal()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (txtSeaman.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Seafarer comment is required.";

        return (!ucError.IsError);
    }

    private void EditAppraisal()
    {
        DataSet ds = PhoenixCrewAppraisal.EditAppraisal(new Guid(Filter.CurrentAppraisalSelection));

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtSeaman.Text = HttpUtility.HtmlDecode(ds.Tables[0].Rows[0]["FLDSEAMANCOMMENT"].ToString());
            ViewState["AppraisalStatus"] = int.Parse(ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString());
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = new DataTable();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                dt = PhoenixVesselAccountsEmployee.EditVesselCrew(PhoenixSecurityContext.CurrentSecurityContext.InstallCode, Convert.ToInt32(Filter.CurrentCrewSelection));
            else
                dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
            }

            if (Filter.CurrentAppraisalSelection != null)
            {
                DataSet ds = PhoenixCrewAppraisal.EditAppraisal(new Guid(Filter.CurrentAppraisalSelection));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtvessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                    txtoccassion.Text = ds.Tables[0].Rows[0]["FLDOCCASSION"].ToString();
                    txtdate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDAPPRAISALDATE"].ToString());
                    ViewState["Rankid"] = ds.Tables[0].Rows[0]["FLDRANKID"].ToString();
                    ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
