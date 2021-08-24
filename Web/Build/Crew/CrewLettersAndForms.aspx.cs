using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOperation;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class Crew_CrewLettersAndForms : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            ddlLicence.DocumentList = PhoenixRegistersDocumentLicence.ListDocumentLicence(null, null, 2, null, null);
        }

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Filter.CurrentJoiningPaperSelection = null;

        txtEmpId.Attributes.Add("style", "visibility:hidden");

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Next", "NEXT",ToolBarDirection.Right);

        MenuLettersAndForms.MenuList = toolbarmain.Show();

        if (Request.QueryString["empid"] != null)
        {
            txtEmpId.Text = Request.QueryString["empid"].ToString();
            divFind.Visible = false;
            SetEmployeeDetails();
        }
        if (Request.QueryString["rnkid"] != null)
            ViewState["rnkid"] = Request.QueryString["rnkid"].ToString();
        if (Request.QueryString["vslid"] != null)
        {
            ViewState["vesselid"] = Request.QueryString["vslid"].ToString();
            ucVessel.SelectedVessel = Request.QueryString["vslid"].ToString();
        }
        if (!IsPostBack)
        {
            ddlLicence.DocumentList = PhoenixRegistersDocumentLicence.ListDocumentLicence(null, null, 2, null, null);
        }
    }
    protected void ImgBtnValidFileno_Click(object sender, ImageClickEventArgs e)
    {
        if (IsValidFileNoCheck())
        {
            SetEmployeePrimaryDetails();
        }
        else
        {
            ucError.Visible = true;
            return;
        }
    }
    private bool IsValidFileNoCheck()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(txtFileNo.Text.Trim()))
            ucError.ErrorMessage = "File Number is required";

        return (!ucError.IsError);
    }
    private bool IsValidEmployeeDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(txtEmpId.Text.Trim()))
            ucError.ErrorMessage = "Employee is required";

        if (ViewState["vesselid"].ToString() == "" || ViewState["vesselid"].ToString().ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Vessel is required";

        if (ViewState["rnkid"].ToString() == "")
            ucError.ErrorMessage = "Rank is required";

        return (!ucError.IsError);
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeListByFileNo(txtFileNo.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtEmpId.Text = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                lblRankid.Text = dt.Rows[0]["FLDRANKPOSTED"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void SetEmployeeDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(txtEmpId.Text));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                lblRankid.Text = dt.Rows[0]["FLDRANKPOSTED"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuLettersAndForms_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("NEXT"))
        {
            ViewState["vesselid"] = ucVessel.SelectedVessel;
            ViewState["rnkid"] = lblRankid.Text;

            if (!IsValidEmployeeDetails())
            {
                ucError.Visible = true;
                return;
            }
            else
            {
                NameValueCollection nvc = new NameValueCollection();

                nvc.Clear();
                nvc.Add("employeeid", txtEmpId.Text);
                nvc.Add("vesselid", ucVessel.SelectedVessel);
                nvc.Add("rankid", ViewState["rnkid"].ToString());
                nvc.Add("dateofjoining", uDate.Text);
                nvc.Add("port", ucSeaport.SelectedSeaport);
                nvc.Add("flightschedule", txtFlightSchedule.Text);
                nvc.Add("agentaddress", txtAgentAddress.Text);
                nvc.Add("seatimeothercomments", txtSeaTimeComments.Text);
                nvc.Add("txtDceAddress", txtDceAddress.Text);
                nvc.Add("cargodetails", txtCargoDetails.Text);
                nvc.Add("requesteddce", ddlLicence.SelectedDocument);

                Filter.CurrentJoiningPaperSelection = nvc;

                Response.Redirect("../Crew/CrewLettersAndFormsReports.aspx", true);
            }
        }
    }
    protected void lnkJoiningPapers_Click(object sender, EventArgs e)
    {
        String scriptpopup = String.Format(
                    "javascript:Openpopup('codehelp1', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=JOININGLETTERS&empid=" + txtEmpId.Text + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rnkid"].ToString() + "&dateofjoining=" + uDate.Text + "&portid=" + ucSeaport.SelectedSeaport + "&flightschedule=" + txtFlightSchedule.Text + "&agentaddress=" + txtAgentAddress.Text + "&reports=" + ViewState["reports"].ToString() + "');");
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
    }
}
