using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using System.Data;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class Crew_CrewJoiningLetters : PhoenixBasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        //Filter.CurrentJoiningPaperSelection = null;

        txtEmpId.Attributes.Add("style", "visibility:hidden");
        //txtAgentId.Attributes.Add("style", "visibility:hidden");

        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("Next", "NEXT", ToolBarDirection.Right);
        toolbarmain.AddButton("Clear", "CLEAR", ToolBarDirection.Right);


        MenuLettersAndForms.MenuList = toolbarmain.Show();

        if (Request.QueryString["empid"] != null)
        {
            txtEmpId.Text = Request.QueryString["empid"].ToString();
            //divFind.Visible = false;
            SetEmployeeDetails();
            //ucTitle.ShowMenu = "false";
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
            try
            {
                ddlLicence.DocumentList = PhoenixRegistersDocumentLicence.ListDocumentLicence(null, null, 2, null, null);
                NameValueCollection nvc = Filter.CurrentJoiningPaperSelection;
                if (nvc != null)
                {
                    if (nvc.Get("flightschedule") != null)
                        txtFlightSchedule.Text = nvc.Get("flightschedule");
                    else
                        txtFlightSchedule.Text = "";

                    if (nvc.Get("agentaddress") != null)
                        txtAgentAddress.Text = nvc.Get("agentaddress");
                    else
                        txtAgentAddress.Text = "";

                    if (nvc.Get("seatimeothercomments") != null)
                        txtSeaTimeComments.Text = nvc.Get("seatimeothercomments");
                    else
                        txtSeaTimeComments.Text = "";

                    if (nvc.Get("txtDceAddress") != null)
                        txtDceAddress.Text = nvc.Get("txtDceAddress");
                    else
                        txtDceAddress.Text = "";

                    if (nvc.Get("cargodetails") != null)
                        txtCargoDetails.Text = nvc.Get("cargodetails");
                    else
                        txtCargoDetails.Text = "";
                    if (nvc.Get("DateofJoin") != null)
                        uDate.Text = nvc.Get("DateofJoin");
                    else
                        uDate.Text = "";
                    if (nvc.Get("PortofJoin") != null && nvc.Get("PortofJoin") != "Dummy")
                        ucSeaport.SelectedSeaport = nvc.Get("PortofJoin");
                    else
                        ucSeaport.SelectedSeaport = "";

                    if (nvc.Get("requesteddce") != null && nvc.Get("requesteddce") != "Dummy")
                        ddlLicence.SelectedDocument = nvc.Get("requesteddce");
                    else
                        ddlLicence.SelectedDocument = "";
                }
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }

    }

    protected void ImgBtnValidFileno_Click(object sender, EventArgs e)
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
                if (Filter.CurrentJoiningPaperSelection != null)
                    nvc = Filter.CurrentJoiningPaperSelection;
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
                nvc.Add("PortofJoin", ucSeaport.SelectedSeaport);
                nvc.Add("DateofJoin", uDate.Text);

                Filter.CurrentJoiningPaperSelection = nvc;

                Response.Redirect("../Crew/CrewJoiningLettersReports.aspx", true);
            }
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentJoiningPaperSelection = null;
            txtEmpId.Text = "";
            ucVessel.SelectedVessel = "";
            uDate.Text = "";
            ucSeaport.SelectedSeaport = "";
            txtFlightSchedule.Text = "";
            txtAgentAddress.Text = "";
            txtSeaTimeComments.Text = "";
            txtDceAddress.Text = "";
            txtCargoDetails.Text = "";
            ddlLicence.SelectedDocument = "";
            txtEmployeeNumber.Text = "";
            txtRank.Text = "";
            txtFirstName.Text = "";
            txtMiddleName.Text = "";
            txtLastName.Text = "";
            lblRankid.Text = "";
            txtFileNo.Text = "";
        }
    }

 

    protected void gvSeparateJoining_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        GridView _gridView = (GridView)sender;

        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToString().ToUpper() == "SELECT")
        {
            _gridView.SelectedIndex = nCurrentRow;

            if (ViewState["rnkid"].ToString().Equals("1") || ViewState["rnkid"].ToString().Equals("2"))
            {
                String scriptpopup = String.Format(
                            "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=MASTERBRIEFING&empid=" + txtEmpId.Text + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rnkid"].ToString() + "&dateofjoining=" + uDate.Text + "&portid=" + ucSeaport.SelectedSeaport + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            else if (ViewState["rnkid"].ToString().Equals("6") || ViewState["rnkid"].ToString().Equals("7"))
            {
                String scriptpopup = String.Format(
                           "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=CEBRIEFING&empid=" + txtEmpId.Text + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rnkid"].ToString() + "&dateofjoining=" + uDate.Text + "&portid=" + ucSeaport.SelectedSeaport + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }

    }

    protected void gvOtherReports_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        GridView _gridView = (GridView)sender;

        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToString().ToUpper() == "SELECT")
        {
            string name = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblShortName")).Text;

            if (name.ToString().ToUpper().Equals("DPT"))
            {
                String scriptpopup = String.Format(
                       "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=DEPARTURE&empid=" + txtEmpId.Text + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rnkid"].ToString() + "&dateofjoining=" + uDate.Text + "&portid=" + ucSeaport.SelectedSeaport + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }

            if (name.ToString().ToUpper().Equals("DLR"))
            {
                String scriptpopup = String.Format(
                        "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=DCELETTER&empid=" + txtEmpId.Text + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rnkid"].ToString() + "&cargodetails=" + txtCargoDetails.Text + "&selecteddcetype=" + ddlLicence.SelectedDocument + "&dceaddresse=" + txtDceAddress.Text + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }

            if (name.ToString().ToUpper().Equals("STL"))
            {
                String scriptpopup = String.Format(
                       "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=SEATIME&empid=" + txtEmpId.Text + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rnkid"].ToString() + "&othercomments=" + txtSeaTimeComments.Text + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }
    }
}
