using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreSuitabilityCheckWithDocument : PhoenixBasePage
{
    private string strEmployeeId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (Request.QueryString["personalmaster"] != null && Request.QueryString["personalmaster"].ToString() != "")
                strEmployeeId = Filter.CurrentCrewSelection;
            else if (Request.QueryString["newapplicant"] != null && Request.QueryString["newapplicant"].ToString() != "")
                strEmployeeId = Filter.CurrentNewApplicantSelection;

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
          

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["CVSL"] = -1;
                ViewState["CRNK"] = -1;
                ViewState["Charterer"] = "";
                ViewState["trainingmatrixid"] = "";
                ViewState["offsignerid"] = "";
                ViewState["reliefdate"] = "";
                ViewState["rankid"] = "";
                ViewState["vesselid"] = "";
                ViewState["vsltype"] = "";
                ViewState["crewplanid"] = "";
                ViewState["calledfrom"] = "";
                ViewState["signonid"] = "";
                ViewState["signondate"] = "";
                ViewState["port"] = "";
                ViewState["empid"] = strEmployeeId;
                ViewState["approval"] = "";
                ViewState["rowindex"] = "";
                ViewState["waivedyn"] = "";
                ViewState["edititem"] = "0";

                ViewState["rankexprowindex"] = "";
                ViewState["rankexpwaivedyn"] = "";
                ViewState["rankexpedititem"] = "0";

                ViewState["vtexprowindex"] = "";
                ViewState["vtexpwaivedyn"] = "";
                ViewState["vtexpedititem"] = "0";

                ViewState["planstatus"] = "";

                ucDate.Text = DateTime.Now.ToShortDateString();

                if (Request.QueryString["personalmaster"] != null && Request.QueryString["personalmaster"].ToString() != "")
                    SetEmployeePrimaryDetails();

                if (Request.QueryString["newapplicant"] != null && Request.QueryString["newapplicant"].ToString() != "")
                    SetNewApplicantPrimaryDetails();

                ucVessel.Enabled = true;
                 
                if (Request.QueryString["calledfrom"] != null && Request.QueryString["calledfrom"].ToString() != "")
                {
                    ViewState["calledfrom"] = Request.QueryString["calledfrom"].ToString();
                    chkShowAll.Checked = true;
                }
                if (Request.QueryString["signondate"] != null && Request.QueryString["signondate"].ToString() != "")
                {
                    ViewState["signondate"] = Request.QueryString["signondate"].ToString();
                }
                if (Request.QueryString["signonid"] != null && Request.QueryString["signonid"].ToString() != "")
                {
                    ViewState["signonid"] = Request.QueryString["signonid"].ToString();
                }
                if (Request.QueryString["port"] != null && Request.QueryString["port"].ToString() != "")
                {
                    ViewState["port"] = Request.QueryString["port"].ToString();
                }
                if (Request.QueryString["rankid"] != null && Request.QueryString["rankid"].ToString() != "")
                {
                    ddlRank.SelectedValue = Request.QueryString["rankid"];
                    ViewState["rankid"] = Request.QueryString["rankid"];
                }
                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                {
                    ucVessel.SelectedVessel = Request.QueryString["vesselid"].ToString();
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
                    ucVessel.bind();
                    ucVessel_TextChangedEvent(null, null);
                }

                if (Request.QueryString["reliefdate"] != null && Request.QueryString["reliefdate"].ToString() != "")
                {
                    ucDate.Text = Request.QueryString["reliefdate"].ToString();
                    ViewState["reliefdate"] = Request.QueryString["reliefdate"].ToString();
                }

                if (Request.QueryString["expectedjoiningdate"] != null && Request.QueryString["expectedjoiningdate"].ToString() != "")
                {
                    ucDate.Text = Request.QueryString["expectedjoiningdate"].ToString();
                    ViewState["reliefdate"] = Request.QueryString["expectedjoiningdate"].ToString();
                }

                if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"].ToString() != "")
                {
                    ViewState["crewplanid"] = Request.QueryString["crewplanid"].ToString();
                    BindPlan(ViewState["crewplanid"].ToString()); 
                }

                if (Request.QueryString["offsignerid"] != null && Request.QueryString["offsignerid"].ToString() != "")
                    ViewState["offsignerid"] = Request.QueryString["offsignerid"].ToString();

                if (Request.QueryString["trainingmatrixid"] != null && Request.QueryString["trainingmatrixid"].ToString() != "")
                    ViewState["trainingmatrixid"] = Request.QueryString["trainingmatrixid"].ToString();

                if (Request.QueryString["approval"] != null && Request.QueryString["approval"].ToString() != "")
                    ViewState["approval"] = Request.QueryString["approval"].ToString();

                BindTrainingMatrix(); 
                BindOffSigner();       
                BindCharter();      
            

                DataTable dt = PhoenixCrewOffshoreReliefRequest.CrewSanctionCheck(Convert.ToInt32(ViewState["empid"].ToString()));

                if (dt.Rows.Count > 0)
                    lblsanction.Visible = true;
                else
                    lblsanction.Visible = false;
            }
            //BindData();
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Suitability", "CHECK");
            if (ViewState["calledfrom"].ToString() == "")
            {
                toolbar.AddButton("Course Req.", "COURSEREQUEST");
            }
            CrewMenu.AccessRights = this.ViewState;
            CrewMenu.MenuList = toolbar.Show();
            CrewMenu.SelectedMenuIndex = 0;
            setToolBar();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
    {
        if (!IsValidCheck())
        {
            ucError.Visible = true;
            return;
        }
        BindData();
        gvSuitability.Rebind();
        gvRankExp.Rebind();
        gvVesselTypeExp.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvSuitability.Rebind();
    }
    protected void BindTrainingMatrix()
    {
        
        DataTable dt = new DataTable();
        dt = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixList(
                                            General.GetNullableInteger(ViewState["vsltype"].ToString()),
                                            General.GetNullableInteger(ddlRank.SelectedValue),
                                            General.GetNullableInteger(ViewState["Charterer"].ToString()));
        //ddlTrainingMatrix.DataTextField = "FLDMATRIXNAME";
        //ddlTrainingMatrix.DataValueField = "FLDMATRIXID";

      
        if (Request.QueryString["trainingmatrixid"] != null && Request.QueryString["trainingmatrixid"].ToString() != "")
        {
            ViewState["trainingmatrixid"] = Request.QueryString["trainingmatrixid"].ToString();
        }
        else
        {
            if (dt.Rows.Count > 0)
                ViewState["trainingmatrixid"] = dt.Rows[0]["FLDMATRIXID"].ToString();
            else
                ViewState["trainingmatrixid"] = 0;

        }
    }
    private bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(ddlRank.SelectedValue) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableDateTime(ucDate.Text) == null)
            ucError.ErrorMessage = "Expected Joining Date is required.";

      

        return (!ucError.IsError);
    }
    private bool IsValidSignOn(string date, string SeaPort)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Sign-On Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Sign-On Date should be earlier than current date";
        }
        if (!General.GetNullableInteger(SeaPort).HasValue)
        {
            ucError.ErrorMessage = "Sign-On Sea Port is required.";
        }

        return (!ucError.IsError);
    }
    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("PLANINSERT"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
                PhoenixCrewOffshoreReliefRequest.InsertPlan(int.Parse(ViewState["empid"].ToString()),
                    int.Parse(ucVessel.SelectedVessel), int.Parse(ddlRank.SelectedValue), General.GetNullableInteger(ddlOffSigner.SelectedValue),
                    DateTime.Parse(ucDate.Text), int.Parse(ViewState["trainingmatrixid"].ToString()));
                ucStatus.Text = "This candidate is proposed for the vessel successfully.";
            }
            else if (CommandName.ToUpper().Equals("PROPOSE"))
            {
                ViewState["rankid"] = General.GetNullableInteger(ddlRank.SelectedValue);
                ViewState["vesselid"] = General.GetNullableInteger(ucVessel.SelectedVessel);
                ViewState["reliefdate"] = General.GetNullableDateTime(ucDate.Text);
                ViewState["offsignerid"] = General.GetNullableInteger(ddlOffSigner.SelectedValue);
                ViewState["trainingmatrixid"] = General.GetNullableInteger(ViewState["trainingmatrixid"].ToString());

                int issuitable = 1;

                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixCrewOffshoreSuitabilityCheckAllDocuments.OffshoreValidateProposal(int.Parse(ViewState["empid"].ToString())
                        , int.Parse(ViewState["vesselid"].ToString()), int.Parse(ViewState["rankid"].ToString())
                        , General.GetNullableDateTime(ViewState["reliefdate"].ToString())
                        , General.GetNullableInteger(ViewState["trainingmatrixid"].ToString())
                        , ref issuitable);

                    if (issuitable == 1)
                    {

                        Response.Redirect("../CrewOffshore/CrewOffshoreProposal.aspx?empid=" + ViewState["empid"]
                            + "&rankid=" + ViewState["rankid"]
                            + "&vesselid=" + ViewState["vesselid"]
                            + "&reliefdate=" + ViewState["reliefdate"]
                            + "&offsignerid=" + ViewState["offsignerid"]
                            + "&trainingmatrixid=" + ViewState["trainingmatrixid"]
                            + "&personalmaster=" + Request.QueryString["personalmaster"]
                            + "&newapplicant=" + Request.QueryString["newapplicant"]
                            + "&popup=" + Request.QueryString["popup"]
                            + "&crewplanid=" + ViewState["crewplanid"]
                            + "&approval=" + ViewState["approval"], false);
                    }
                    else
                    {
                        ucError.ErrorMessage = "This employee is not suitable for proposal as per training matrix.";
                        ucError.Visible = true;
                        return;
                    }
                }
            }
            else if (CommandName.ToUpper().Equals("EDITPROPOSAL"))
            {
                ViewState["rankid"] = General.GetNullableInteger(ddlRank.SelectedValue);
                ViewState["vesselid"] = General.GetNullableInteger(ucVessel.SelectedVessel);
                ViewState["reliefdate"] = General.GetNullableDateTime(ucDate.Text);
                ViewState["offsignerid"] = General.GetNullableInteger(ddlOffSigner.SelectedValue);
                ViewState["trainingmatrixid"] = General.GetNullableInteger(ViewState["trainingmatrixid"].ToString());
                string iseditable = "0";

                DataTable dt = PhoenixCrewPlanning.EditCrewPlan(new Guid(ViewState["crewplanid"].ToString()));
                DataRow dr = dt.Rows[0];
                if (string.IsNullOrEmpty(dr["FLDAPPOINTMENTLETTERID"].ToString()) && dr["FLDPDSTATUS"].ToString() != PhoenixCommonRegisters.GetHardCode(1, 99, "AFS"))
                    iseditable = "1";

                int issuitable = 1;

                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixCrewOffshoreReliefRequest.OffshoreValidateProposal(int.Parse(ViewState["empid"].ToString())
                        , int.Parse(ViewState["vesselid"].ToString()), int.Parse(ViewState["rankid"].ToString())
                        , General.GetNullableDateTime(ViewState["reliefdate"].ToString())
                        , General.GetNullableInteger(ViewState["trainingmatrixid"].ToString())
                        , ref issuitable);

                    //if (issuitable == 1)
                    //{

                    Response.Redirect("../CrewOffshore/CrewOffshoreProposal.aspx?empid=" + ViewState["empid"]
                        + "&rankid=" + ViewState["rankid"]
                        + "&vesselid=" + ViewState["vesselid"]
                        + "&reliefdate=" + ViewState["reliefdate"]
                        + "&offsignerid=" + ViewState["offsignerid"]
                        + "&trainingmatrixid=" + ViewState["trainingmatrixid"]
                        + "&personalmaster=" + Request.QueryString["personalmaster"]
                        + "&newapplicant=" + Request.QueryString["newapplicant"]
                        + "&popup=" + Request.QueryString["popup"]
                        + "&crewplanid=" + ViewState["crewplanid"]
                        + "&approval=" + ViewState["approval"]
                        + "&editproposal=1"
                        + "&iseditable=" + iseditable, false);
                    //}
                }
            }
            else if (CommandName.ToUpper().Equals("CHECK"))
            {
                if (!IsValidCheck())
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
            }
            else if (CommandName.ToUpper().Equals("APPROVETRAVEL"))
            {
                PhoenixCrewOffshoreCrewChange.ApproveTravel(General.GetNullableGuid(ViewState["crewplanid"].ToString()));
                ucStatus.Text = "Travel is approved for this employee.";

                setToolBar();

                if (Request.QueryString["popup"] != null && Request.QueryString["popup"].ToString() != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                     "BookMarkScript", "fnReloadList('codehelp1', null, true);", true);
                }
            }
            else if (CommandName.ToUpper().Equals("APPROVETSIGNON"))
            {
                PhoenixCrewOffshoreCrewChange.ApproveSignOn(new Guid(ViewState["crewplanid"].ToString()),
                    General.GetNullableInteger(ucVessel.SelectedVessel), General.GetNullableInteger(ddlRank.SelectedValue),
                    General.GetNullableInteger(ViewState["empid"].ToString()));

                PhoenixCrewOffshoreCheckList.InsertDocumentCheckList(General.GetNullableGuid(ViewState["crewplanid"].ToString()));
                ucStatus.Text = "Sign on is approved for this employee.";
                setToolBar();

                if (Request.QueryString["popup"] != null && Request.QueryString["popup"].ToString() != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                     "BookMarkScript", "fnReloadList('codehelp1', null, true);", true);
                }
            }
            else if (CommandName.ToUpper().Equals("SIGNON"))
            {
                string signondate = ViewState["signondate"].ToString();
                string port = ViewState["port"].ToString();
                if (!IsValidSignOn(signondate, port))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewOffshoreCrewList.UpdateVesselSignOn(int.Parse(ViewState["signonid"].ToString())
                    , DateTime.Parse(signondate), General.GetNullableInteger(port), 1, General.GetNullableGuid(ViewState["crewplanid"].ToString()));

                ucStatus.Text = "This candidate is signed on for the vessel successfully.";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                  "BookMarkScript", "fnReloadList('suitability', null, true);", true);
            }
            else if (CommandName.ToUpper().Equals("APPROVEVESSEL"))
            {
                ViewState["rankid"] = General.GetNullableInteger(ddlRank.SelectedValue);
                ViewState["vesselid"] = General.GetNullableInteger(ucVessel.SelectedVessel);
                ViewState["reliefdate"] = General.GetNullableDateTime(ucDate.Text);
                ViewState["offsignerid"] = General.GetNullableInteger(ddlOffSigner.SelectedValue);
                ViewState["trainingmatrixid"] = General.GetNullableInteger(ViewState["trainingmatrixid"].ToString());

                int issuitable = 1;
                int? pendingwaiver = null;

                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixCrewOffshoreApproveProposal.OffshoreValidateApproval(int.Parse(ViewState["empid"].ToString())
                        , int.Parse(ViewState["vesselid"].ToString()), int.Parse(ViewState["rankid"].ToString())
                        , General.GetNullableDateTime(ViewState["reliefdate"].ToString())
                        , General.GetNullableInteger(ViewState["trainingmatrixid"].ToString())
                        , ref issuitable, ref pendingwaiver, PhoenixSecurityContext.CurrentSecurityContext.UserCode);



                    if (pendingwaiver == 1)
                    {
                        ucError.ErrorMessage = "There are some pending waivers. Please waive the documents to approve for vessel.";
                        ucError.Visible = true;
                        return;
                    }

                    if (issuitable == 0)
                    {
                        ucError.ErrorMessage = "This employee is not suitable for approval as per training matrix.";
                        ucError.Visible = true;
                        return;
                    }

                    Response.Redirect("../CrewOffshore/CrewOffshoreApproval.aspx?empid=" + ViewState["empid"]
                        + "&crewplanid=" + ViewState["crewplanid"].ToString()
                        + "&rankid=" + ViewState["rankid"]
                        + "&vesselid=" + ViewState["vesselid"]
                        + "&reliefdate=" + ViewState["reliefdate"]
                        + "&offsignerid=" + ViewState["offsignerid"]
                        + "&trainingmatrixid=" + ViewState["trainingmatrixid"]
                        + "&personalmaster=" + Request.QueryString["personalmaster"]
                        + "&newapplicant=" + Request.QueryString["newapplicant"]
                        + "&popup=" + Request.QueryString["popup"], false);
                }
            }
            else if (CommandName.ToUpper().Equals("APPOINTMENTLETTER"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreAppointmentLetter.aspx?employeeid=" + ViewState["empid"]
                            + "&personalmaster=" + Request.QueryString["personalmaster"]
                            + "&newapplicant=" + Request.QueryString["newapplicant"]
                            + "&popup=" + Request.QueryString["popup"]
                            + "&crewplanid=" + ViewState["crewplanid"]
                            + "&redirectedfrom=suitability", false);
            }
            else if (CommandName.ToUpper().Equals("PENDINGWAIVER"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshorePendingWaivers.aspx?empid=" + ViewState["empid"]
                            + "&personalmaster=" + Request.QueryString["personalmaster"]
                            + "&newapplicant=" + Request.QueryString["newapplicant"]
                            + "&popup=" + Request.QueryString["popup"]
                            + "&crewplanid=" + ViewState["crewplanid"]
                            + "&approval=1", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private bool IsValidCheck()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlRank.SelectedValue) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableDateTime(ucDate.Text) == null)
            ucError.ErrorMessage = "Expected Joining Date is required.";

      

        return (!ucError.IsError);
    }
    protected void ddlRank_TextChanged(object sender, EventArgs e)
    {

    }

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("COURSEREQUEST"))
            {
                string querystring = "empid=" + ViewState["empid"];
                if (!string.IsNullOrEmpty(Request.QueryString["personalmaster"]))
                    querystring += "&personalmaster=1";
                if (!string.IsNullOrEmpty(Request.QueryString["newapplicant"]))
                    querystring += "&newapplicant=1";
                querystring += "&rankid=" + General.GetNullableInteger(ddlRank.SelectedValue);
                querystring += "&vesselid=" + General.GetNullableInteger(ucVessel.SelectedVessel);
                querystring += "&trainingmatrixid=" + General.GetNullableInteger(ViewState["trainingmatrixid"].ToString());
                querystring += "&expectedjoiningdate=" + General.GetNullableDateTime(ucDate.Text);
                querystring += "&suitability=1";
                querystring += "&offsignerid=" + General.GetNullableInteger(ddlOffSigner.SelectedValue);
                querystring += "&crewplanid=" + ViewState["crewplanid"];
                querystring += "&approval=" + ViewState["approval"];

                Response.Redirect("CrewOffshoreCourseMissing.aspx?" + querystring, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        BindCharter();
        BindData();
    }

    protected void ddlchartername_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindData();
    }
    protected void setToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        //toolbar.AddButton("Check", "CHECK");
        if (string.IsNullOrEmpty(ViewState["crewplanid"].ToString()))
        {
            toolbar.AddButton("Propose", "PROPOSE", ToolBarDirection.Right);
            CrewMenuGeneral.AccessRights = this.ViewState;
            CrewMenuGeneral.MenuList = toolbar.Show();
        }
        else
        {
            DataTable dt = PhoenixCrewPlanning.EditCrewPlan(new Guid(ViewState["crewplanid"].ToString()));
            DataRow dr = dt.Rows[0];
            //if (string.IsNullOrEmpty(dr["FLDAPPOINTMENTLETTERID"].ToString()) && dr["FLDPDSTATUS"].ToString() != PhoenixCommonRegisters.GetHardCode(1, 99, "AFS"))
            toolbar.AddButton("Edit Proposal", "EDITPROPOSAL");
            if (dr["FLDPDSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 99, "AWA") || dr["FLDPDSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 99, "APR")) // Proposed, Approval rejected
            {
                if (!string.IsNullOrEmpty(ViewState["approval"].ToString()))
                {
                    toolbar.AddButton("Pending Waivers", "PENDINGWAIVER");
                    toolbar.AddButton("Approve for Vessel", "APPROVEVESSEL");
                }
            }
            else if (dr["FLDPDSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 99, "APV")) // Approve for vessel
            {
                toolbar.AddButton("Approve Travel", "APPROVETRAVEL");
            }
            else if (dr["FLDPDSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 99, "AFT")) // Approve for travel
            {
                if (string.IsNullOrEmpty(dr["FLDAPPOINTMENTLETTERID"].ToString()))
                    toolbar.AddButton("Appointment Letter", "APPOINTMENTLETTER");
                else
                    toolbar.AddButton("Approve Sign/On", "APPROVETSIGNON");
            }
            else if (dr["FLDPDSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 99, "AFS")) // Approve for sign on
            {
                toolbar.AddButton("Propose", "PROPOSE");
            }

            if (dr["FLDPDSTATUS"].ToString() != PhoenixCommonRegisters.GetHardCode(1, 99, "AFS"))
            {
                CrewMenuGeneral.AccessRights = this.ViewState;
                CrewMenuGeneral.MenuList = toolbar.Show();
            }
        }
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(ViewState["empid"].ToString()));

            //txtEmployeeNumber.Visible = true;

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                ddlRank.DataSource = PhoenixCrewManagement.CrewRankDeptList(General.GetNullableInteger(dt.Rows[0]["FLDGROUP"].ToString()), null);
                ddlRank.DataBind();
                ddlRank.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlRank.SelectedValue = dt.Rows[0]["FLDRANKPOSTED"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetNewApplicantPrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(ViewState["empid"].ToString()));


            tdempno.Visible = false;
            tdempnum.Visible = false;

            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                ddlRank.DataSource = PhoenixCrewManagement.CrewRankDeptList(General.GetNullableInteger(dt.Rows[0]["FLDGROUP"].ToString()), null);
                ddlRank.DataBind();
                ddlRank.SelectedValue = dt.Rows[0]["FLDRANK"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindOffSigner()
    {
        UserControlVessel vsl = ucVessel;
        RadComboBox rank = ddlRank;
        RadComboBox cob = ddlOffSigner;

        int? VesselId = General.GetNullableInteger(vsl.SelectedVessel);
        int? RankId = General.GetNullableInteger(rank.SelectedValue);

        cob.Items.Clear();

        if (VesselId.HasValue)
        {
            bool bind = false;
            if (int.Parse(ViewState["CVSL"].ToString()) != VesselId.Value)
            {
                ViewState["CVSL"] = VesselId.Value;
                bind = true;
            }
            if (RankId.HasValue && int.Parse(ViewState["CRNK"].ToString()) != RankId.Value)
            {
                ViewState["CRNK"] = RankId.Value;
                bind = true;
            }
            if (bind)
            {
                //cob.DataSource = PhoenixCrewOffshoreReliefRequest.ListRelivee(VesselId.Value, RankId);
                cob.DataSource = PhoenixCrewOffshoreReliefRequest.ListRelivee(VesselId.Value, RankId, General.GetNullableInteger(strEmployeeId));
            }
        }
        else
            //cob.DataSource = PhoenixCrewOffshoreReliefRequest.ListRelivee(General.GetNullableInteger(vsl.SelectedVessel), General.GetNullableInteger(rank.SelectedValue));
            cob.DataSource = PhoenixCrewOffshoreReliefRequest.ListRelivee(General.GetNullableInteger(vsl.SelectedVessel)
                                                                          , General.GetNullableInteger(rank.SelectedValue)
                                                                          , General.GetNullableInteger(strEmployeeId)
                                                                          );
        cob.DataTextField = "FLDEMPLOYEENAME";
        cob.DataValueField = "FLDEMPLOYEEID";
        cob.DataBind();
        cob.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

        if (Request.QueryString["offsignerid"] != null && Request.QueryString["offsignerid"].ToString() != "")
        {
            if (cob.Items.FindItemByValue(Request.QueryString["offsignerid"].ToString()) != null)
                cob.SelectedValue = Request.QueryString["offsignerid"].ToString();
        }
    }

    private void BindData()
    {
        try
        {
            int issuitable = 1;

            DataSet ds = PhoenixCrewOffshoreSuitabilityCheckAllDocuments.CrewSuitabilityForVesselRank(
                General.GetNullableInteger(ucVessel.SelectedVessel)
                , General.GetNullableInteger(ddlRank.SelectedValue) == null ? 0 : General.GetNullableInteger(ddlRank.SelectedValue)
                , int.Parse(ViewState["empid"].ToString())
                , General.GetNullableDateTime(ucDate.Text)              
                , ref issuitable
                , General.GetNullableInteger(chkShowAll.Checked ? "1" : "0")
                , General.GetNullableGuid(ViewState["crewplanid"].ToString())
                , General.GetNullableInteger(ddlchartername.SelectedValue)
                , General.GetNullableInteger(ViewState["trainingmatrixid"].ToString())
                );

            if(ds.Tables[0].Rows.Count>0)
            {
                txtchartername.Text = ds.Tables[0].Rows[0]["FLDCHARTER"].ToString();
                txtcompanyname.Text = ds.Tables[0].Rows[0]["FLDMANAGER"].ToString();
                txtvesseltype.Text= ds.Tables[0].Rows[0]["FLDVESSELTYPE"].ToString();
                txtflagname.Text= ds.Tables[0].Rows[0]["FLDFLAG"].ToString();
                txtmaxtourday.Text= ds.Tables[0].Rows[0]["FLDMAXTOURDAYS"].ToString();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                gvSuitability.DataSource = ds.Tables[1];


                string planstatus = "";
                foreach (GridItem item in gvSuitability.MasterTableView.Items)
                {
                    if (item is GridItem)
                    {
                        //  GridItem cmdItem = gvStudents.MasterTableView.Items;

                        GridDataItem dataItem = (GridDataItem)item;
                        RadLabel lblPlanStatus = (RadLabel)item.FindControl("lblPlanStatus");
                        planstatus = lblPlanStatus.Text;
                    }
                }

                if (!string.IsNullOrEmpty(planstatus))
                    ViewState["planstatus"] = planstatus;
                if (string.IsNullOrEmpty(planstatus))
                    gvSuitability.Columns[7].Visible = true;
                //GridDecorator.MergeRows(gvSuitability);
            }
            else
            {
              //  gvSuitability.DataSource = ds.Tables[0];
            }

            if (ds.Tables[2].Rows.Count > 0)
            {
                gvRankExp.DataSource = ds.Tables[2];

                //GridDecorator.MergeRowsExperience(gvRankExp);
                string planstatus = "";
                foreach (GridItem item in gvRankExp.MasterTableView.Items)
                {
                    if (item is GridItem)
                    {
                        //  GridItem cmdItem = gvStudents.MasterTableView.Items;

                        GridDataItem dataItem = (GridDataItem)item;
                        RadLabel lblPlanStatus = (RadLabel)item.FindControl("lblPlanStatus");
                        planstatus = lblPlanStatus.Text;
                    }
                }

                if (!string.IsNullOrEmpty(planstatus))
                    ViewState["planstatus"] = planstatus;
                if (string.IsNullOrEmpty(planstatus))
                    gvRankExp.Columns[3].Visible = true;
            }
            else
            {
                gvRankExp.DataSource = ds.Tables[2];
            }

            if (ds.Tables[3].Rows.Count > 0)
            {
                gvVesselTypeExp.DataSource = ds.Tables[3];

                //GridDecorator.MergeRowsExperience(gvVesselTypeExp);
                string planstatus = "";
                foreach (GridItem item in gvVesselTypeExp.MasterTableView.Items)
                {
                    if (item is GridItem)
                    {
                        //  GridItem cmdItem = gvStudents.MasterTableView.Items;

                        GridDataItem dataItem = (GridDataItem)item;
                        RadLabel lblPlanStatus = (RadLabel)item.FindControl("lblPlanStatus");
                        planstatus = lblPlanStatus.Text;
                    }
                }

                if (!string.IsNullOrEmpty(planstatus))
                    ViewState["planstatus"] = planstatus;
                if (string.IsNullOrEmpty(planstatus))
                    gvVesselTypeExp.Columns[3].Visible = true;
            }
            else
            {
                gvVesselTypeExp.DataSource = ds.Tables[3];
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSuitability_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvSuitability_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {


            RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
            RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");
            RadLabel lblDocumentName = (RadLabel)e.Item.FindControl("lblDocumentName");
            RadLabel lblExpiryDate = (RadLabel)e.Item.FindControl("lblExpiryDate");
            RadLabel lblNationality = (RadLabel)e.Item.FindControl("lblNationality");
            LinkButton lnkName = (LinkButton)e.Item.FindControl("lnkName");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblAttachmenttype = (RadLabel)e.Item.FindControl("lblAttachmenttype");
            RadLabel lblStage = (RadLabel)e.Item.FindControl("lblStage");
            RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
            CheckBox chkWaivedYN = (CheckBox)e.Item.FindControl("chkWaivedYN");
            RadTextBox txtReason = (RadTextBox)e.Item.FindControl("txtReason");
            RadLabel lblVerifiedYN = (RadLabel)e.Item.FindControl("lblVerifiedYN");
            RadLabel lblReqDocumentName = (RadLabel)e.Item.FindControl("lblReqDocumentName");
            RadLabel lblStageid = (RadLabel)e.Item.FindControl("lblStageid");
            RadLabel lblProposalStageid = (RadLabel)e.Item.FindControl("lblProposalStageid");
            CheckBox chkCanbeWaivedYN = (CheckBox)e.Item.FindControl("chkCanbeWaivedYN");
            RadLabel lblPlanStatus = (RadLabel)e.Item.FindControl("lblPlanStatus");
            RadLabel lblDocumentType = (RadLabel)e.Item.FindControl("lblDocumentType");
            RadLabel lblAuthenticatedYN = (RadLabel)e.Item.FindControl("lblAuthenticatedYN");


            UserControlToolTip uctDate = (UserControlToolTip)e.Item.FindControl("ucToolTipDate");
            uctDate.Position = ToolTipPosition.TopCenter;
            uctDate.TargetControlId = chkWaivedYN.ClientID;
            //chkWaivedYN.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctDate.ToolTip + "', 'visible');");
            //    chkWaivedYN.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctDate.ToolTip + "', 'hidden');");

            CheckBox chkWaivedconfirmYN = (CheckBox)e.Item.FindControl("chkWaivedconfirmYN");
            UserControlToolTip ucWaiveToolTipDate = (UserControlToolTip)e.Item.FindControl("ucWaiveToolTipDate");
            ucWaiveToolTipDate.Position = ToolTipPosition.TopCenter;
            ucWaiveToolTipDate.TargetControlId = chkWaivedconfirmYN.ClientID;
            LinkButton cmdApprove = (LinkButton)e.Item.FindControl("cmdApprove");

            if (chkWaivedYN != null)
            {

                if (chkWaivedYN.Checked == true)
                {
                    if (cmdApprove != null) cmdApprove.Visible = true;
                }
                else
                {
                    if (cmdApprove != null) cmdApprove.Visible = false;
                }
            }
            if (chkWaivedconfirmYN != null)
            {
                if (chkWaivedconfirmYN.Checked == true)
                {
                    if (cmdApprove != null) cmdApprove.Visible = false;
                    //if (imgReject != null) imgReject.Visible = true;
                }
            }

            if (cmdApprove != null)
            {
                //cmdApprove.Visible = SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName);
                cmdApprove.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Waive ?')");
            }
            if (General.GetNullableInteger(ViewState["rowindex"].ToString()) != null && General.GetNullableInteger(ViewState["rowindex"].ToString()) == e.Item.DataSetIndex)
            {
                if (chkWaivedYN != null)
                {
                    if (General.GetNullableInteger(ViewState["waivedyn"].ToString()) != null && General.GetNullableInteger(ViewState["waivedyn"].ToString()) == 1)
                    {
                        chkWaivedYN.Checked = true;
                        if (txtReason != null) txtReason.CssClass = "input_mandatory";
                    }
                    else
                    {
                        chkWaivedYN.Checked = false;
                        if (txtReason != null) txtReason.CssClass = "input";
                    }
                    ViewState["rowindex"] = "";
                    ViewState["waivedyn"] = "";
                    ViewState["edititem"] = "0";
                }
            }

            if (lblStageid != null && lblProposalStageid != null)
            {
                if (lblStageid.Text.Equals(lblProposalStageid.Text) && DataBinder.Eval(e.Item.DataItem, "FLDWAIVEDYN").ToString().Equals("1")) // enabled only for proposal stage and if the document can be waived.
                {
                    if (chkCanbeWaivedYN != null)
                    {
                        chkCanbeWaivedYN.Enabled = true;
                    }
                }
            }

            if (lblExpiredYN.Text.Trim() == "1" || lblMissingYN.Text.Trim() == "1" || lblVerifiedYN.Text.Trim() == "0")
            {
                //e.Item.BackColor= System.Drawing.Color.Red;
                //lblDocumentName.ForeColor = System.Drawing.Color.Red;
                //lblExpiryDate.ForeColor = System.Drawing.Color.Red;
                //lblNationality.ForeColor = System.Drawing.Color.Red;
                //lnkName.ForeColor = System.Drawing.Color.Red;
                //lblStage.ForeColor = System.Drawing.Color.Red;
                //lblStatus.ForeColor = System.Drawing.Color.Red;
                lblReqDocumentName.Attributes.Add("style", "color:red !important;");
                if (lblMissingYN.Text.Trim() == "1")
                {
                    //if (lblReqDocumentName != null) lblReqDocumentName.ForeColor = System.Drawing.Color.Red;
                    if (lblDocumentName != null) lblDocumentName.Visible = false;
                }
            }
            if (lblDTKey != null && !string.IsNullOrEmpty(lblDTKey.Text) && lblMissingYN.Text.Trim() == "0")
            {
                if (lblDocumentName != null) lblDocumentName.Visible = false;
                if (lnkName != null) lnkName.Visible = true;
                if (lblExpiredYN.Text.Trim() == "0" && lblMissingYN.Text.Trim() == "0" && lblVerifiedYN.Text.Trim() == "1")
                    lnkName.ForeColor = System.Drawing.Color.Black;
            }
            if (lnkName != null)
            {
                if (Request.QueryString["popup"] != null && Request.QueryString["popup"].ToString() != "")
                {
                    lnkName.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + lblAttachmenttype.Text + "&U=1'); return false;");
                }
                else
                {
                    lnkName.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/ Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + lblAttachmenttype.Text + "&U=1'); return false;");
                }
            }

            if (chkWaivedYN != null)
                chkWaivedYN.Enabled = DataBinder.Eval(e.Item.DataItem, "FLDWAIVEDYN").ToString().Equals("1") ? true : false;

            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            LinkButton cmdSave = (LinkButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);

        }
    }
    protected void gvSuitability_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;



            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadLabel lblDocumentType = (RadLabel)e.Item.FindControl("lblDocumentType");
                RadLabel lblDocumentId = (RadLabel)e.Item.FindControl("lblDocumentId");
                CheckBox cb = (CheckBox)e.Item.FindControl("chkWaivedYN");
                RadTextBox txtReason = (RadTextBox)e.Item.FindControl("txtReason");
                RadLabel lblWaivedDocumentId = (RadLabel)e.Item.FindControl("lblWaivedDocumentId");
                CheckBox chkCanbeWaivedYN = (CheckBox)e.Item.FindControl("chkCanbeWaivedYN");
                string doctid;

                if (lblDocumentType.Text == "1" || lblDocumentType.Text == "2" || lblDocumentType.Text == "3" || lblDocumentType.Text == "5" || lblDocumentType.Text == "6")
                    doctid = null;
                else
                    doctid = lblDocumentId.Text;

                if (!IsValidWaivedFilter((cb.Checked ? "2" : "0"), txtReason.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreReliefRequest.InsertWaivedDocument(int.Parse(ViewState["empid"].ToString()),
                    General.GetNullableInteger(ucVessel.SelectedVessel), int.Parse(ddlRank.SelectedValue),
                    General.GetNullableInteger(lblDocumentType.Text), General.GetNullableInteger(doctid),
                    General.GetNullableInteger(cb.Checked ? "2" : "0"),
                    null,//General.GetNullableInteger(ddlTrainingMatrix.SelectedValue),
                    General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                    General.GetNullableString(txtReason.Text),
                    General.GetNullableGuid(lblWaivedDocumentId.Text),
                    General.GetNullableInteger(chkCanbeWaivedYN.Checked ? "1" : "0"));

                BindData();
                gvSuitability.DataSource = null;
                gvSuitability.Rebind();

            }
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {

                RadLabel lblwaivedocid = (RadLabel)e.Item.FindControl("lblwaivedocid");
                PhoenixCrewOffshoreWaivingRequest.UpdateWaivingRequest(General.GetNullableGuid(lblwaivedocid.Text));
                BindData();
                gvSuitability.Rebind();
                ucStatus.Text = "Approved";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidWaivedFilter(string waiveyn, string reason)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required to waive the documents.";

        if (General.GetNullableInteger(ddlRank.SelectedValue) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableDateTime(ucDate.Text) == null)
            ucError.ErrorMessage = "Expected Joining Date is required.";


        if (General.GetNullableInteger(waiveyn) != null && General.GetNullableInteger(waiveyn) == 2)
        {
            if (General.GetNullableString(reason) == null)
                ucError.ErrorMessage = "Reason is required.";
        }

        return (!ucError.IsError);
    }
    public void chkCanbeWaivedYN_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridDataItem gvRow = (GridDataItem)cb.Parent.Parent;

            RadLabel lblDocumentType = (RadLabel)gvRow.FindControl("lblDocumentType");
            RadLabel lblDocumentId = (RadLabel)gvRow.FindControl("lblDocumentId");
            RadLabel lblWaivedDocumentId = (RadLabel)gvRow.FindControl("lblWaivedDocumentId");

            string doctid;

            if (lblDocumentType.Text == "1" || lblDocumentType.Text == "2" || lblDocumentType.Text == "3" || lblDocumentType.Text == "5" || lblDocumentType.Text == "6")
                doctid = null;
            else
                doctid = lblDocumentId.Text;

            if (string.IsNullOrEmpty(ViewState["planstatus"].ToString()))
            {
                PhoenixCrewOffshoreReliefRequest.InsertPendingWaivers(int.Parse(ViewState["empid"].ToString()),
                        General.GetNullableInteger(ucVessel.SelectedVessel), int.Parse(ddlRank.SelectedValue),
                        General.GetNullableInteger(lblDocumentType.Text), General.GetNullableInteger(doctid),
                        null,//General.GetNullableInteger(ddlTrainingMatrix.SelectedValue),
                        General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                        General.GetNullableGuid(lblWaivedDocumentId.Text), General.GetNullableInteger(cb.Checked ? "1" : "0"));

                BindData();
                gvSuitability.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
            gvSuitability.Rebind();
        }
    }
    protected void chkWaivedYN_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridDataItem gvRow = (GridDataItem)cb.Parent.Parent;

            RadTextBox txtReason = (RadTextBox)gvRow.FindControl("txtReason");

            ViewState["rowindex"] = gvRow.DataSetIndex;
            ViewState["waivedyn"] = cb.Checked ? 1 : 0;
            ViewState["edititem"] = 1;

           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();

        }
    }
    public void BindCharter()
    {
        try
        {
            ddlchartername.Items.Clear();

            DataTable dt = new DataTable();
            dt = PhoenixCrewOffshoreSuitabilityCheckAllDocuments.ListCharterName(General.GetNullableInteger(ucVessel.SelectedVessel));
            ddlchartername.DataTextField = "FLDCHARTERCOMMENCEDATE";
            ddlchartername.DataValueField = "FLDCHARTERID";

            if (dt.Rows.Count > 0)
            {
                ddlchartername.DataSource = dt;
            }

            ddlchartername.DataBind();
            ddlchartername.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            //ddlchartername.SelectedIndex=0;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void chkCanbeWaivedYNRankExp_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridDataItem gvRow = (GridDataItem)cb.Parent.Parent;

            RadLabel lblDocumentType = (RadLabel)gvRow.FindControl("lblDocumentType");
            RadLabel lblWaivedDocumentId = (RadLabel)gvRow.FindControl("lblWaivedDocumentId");

            if (string.IsNullOrEmpty(ViewState["planstatus"].ToString()))
            {
                PhoenixCrewOffshoreReliefRequest.InsertPendingWaivers(int.Parse(ViewState["empid"].ToString()),
                      General.GetNullableInteger(ucVessel.SelectedVessel), int.Parse(ddlRank.SelectedValue),
                      General.GetNullableInteger(lblDocumentType.Text), null,
                      General.GetNullableInteger(ViewState["trainingmatrixid"].ToString()), General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                      General.GetNullableGuid(lblWaivedDocumentId.Text), General.GetNullableInteger(cb.Checked ? "1" : "0"));

                BindData();
                gvRankExp.Rebind();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
            gvRankExp.Rebind();
        }
    }

    public void chkCanbeWaivedYNVTExp_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridDataItem gvRow = (GridDataItem)cb.Parent.Parent;

            RadLabel lblDocumentType = (RadLabel)gvRow.FindControl("lblDocumentType");
            RadLabel lblWaivedDocumentId = (RadLabel)gvRow.FindControl("lblWaivedDocumentId");

            if (string.IsNullOrEmpty(ViewState["planstatus"].ToString()))
            {
                PhoenixCrewOffshoreReliefRequest.InsertPendingWaivers(int.Parse(ViewState["empid"].ToString()),
                      General.GetNullableInteger(ucVessel.SelectedVessel), int.Parse(ddlRank.SelectedValue),
                      General.GetNullableInteger(lblDocumentType.Text), null,
                      General.GetNullableInteger(ViewState["trainingmatrixid"].ToString()), General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                      General.GetNullableGuid(lblWaivedDocumentId.Text), General.GetNullableInteger(cb.Checked ? "1" : "0"));

                BindData();
                gvVesselTypeExp.Rebind();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
            gvVesselTypeExp.Rebind();
        }
    }

    protected void chkWaivedYNRankExp_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridDataItem gvRow = (GridDataItem)cb.Parent.Parent;

            RadTextBox txtReason = (RadTextBox)gvRow.FindControl("txtReason");

            //gvRankExp.EditIndex = gvRow.DataItemIndex;
            //gvRankExp.SelectedIndex = gvRow.DataItemIndex;
            ViewState["rankexprowindex"] = gvRow.DataSetIndex;
            ViewState["rankexpwaivedyn"] = cb.Checked ? 1 : 0;
            ViewState["rankexpedititem"] = 1;

            //BindData();
            //gvSuitability.Rebind();
            //gvRankExp.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
            gvSuitability.Rebind();
            gvRankExp.Rebind();
        }
    }

    protected void chkWaivedYNVTExp_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridDataItem gvRow = (GridDataItem)cb.Parent.Parent;

            RadTextBox txtReason = (RadTextBox)gvRow.FindControl("txtReason");

            //gvVesselTypeExp.EditIndex = gvRow.DataItemIndex;
            //gvVesselTypeExp.SelectedIndex = gvRow.DataghItemIndex;
            ViewState["vtexprowindex"] = gvRow.DataSetIndex;
            ViewState["vtexpwaivedyn"] = cb.Checked ? 1 : 0;
            ViewState["vtexpedititem"] = 1;

            //BindData();
            //gvSuitability.Rebind();
            //gvRankExp.Rebind();
            //gvVesselTypeExp.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
            gvSuitability.Rebind();
            gvRankExp.Rebind();
            gvVesselTypeExp.Rebind();
        }
    }
    protected void gvRankExp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvRankExp_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblShortfall = (RadLabel)e.Item.FindControl("lblShortfall");
            RadLabel lblRank = (RadLabel)e.Item.FindControl("lblRank");
            RadLabel lblRankExp = (RadLabel)e.Item.FindControl("lblRankExp");
            RadLabel lblStage = (RadLabel)e.Item.FindControl("lblStage");
            CheckBox chkWaivedYN = (CheckBox)e.Item.FindControl("chkWaivedYN");
            RadTextBox txtReason = (RadTextBox)e.Item.FindControl("txtReason");
            RadLabel lblStageid = (RadLabel)e.Item.FindControl("lblStageid");
            RadLabel lblProposalStageid = (RadLabel)e.Item.FindControl("lblProposalStageid");
            CheckBox chkCanbeWaivedYN = (CheckBox)e.Item.FindControl("chkCanbeWaivedYN");
            RadLabel lblPlanStatus = (RadLabel)e.Item.FindControl("lblPlanStatus");

            UserControlToolTip uctDate = (UserControlToolTip)e.Item.FindControl("ucToolTipDate");
            uctDate.Position = ToolTipPosition.TopCenter;
            uctDate.TargetControlId = chkWaivedYN.ClientID;


            CheckBox chkWaivedconfirmYN = (CheckBox)e.Item.FindControl("chkWaivedconfirmYN");
            UserControlToolTip ucWaiveToolTipDate = (UserControlToolTip)e.Item.FindControl("ucWaiveToolTipDate");
            ucWaiveToolTipDate.Position = ToolTipPosition.TopCenter;
            ucWaiveToolTipDate.TargetControlId = chkWaivedconfirmYN.ClientID;


            LinkButton cmdApprove = (LinkButton)e.Item.FindControl("cmdApprove");

            if (chkWaivedYN != null)
            {

                if (chkWaivedYN.Checked == true)
                {
                    if (cmdApprove != null) cmdApprove.Visible = true;
                }
                else
                {
                    if (cmdApprove != null) cmdApprove.Visible = false;
                }
            }
            if (chkWaivedconfirmYN != null)
            {
                if (chkWaivedconfirmYN.Checked == true)
                {
                    if (cmdApprove != null) cmdApprove.Visible = false;
                    //if (imgReject != null) imgReject.Visible = true;
                }
            }

            if (cmdApprove != null)
            {
                //cmdApprove.Visible = SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName);
                cmdApprove.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Waive ?')");
            }


            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (lblShortfall.Text == "1")
            {
                lblRank.ForeColor = System.Drawing.Color.Red;
                lblRankExp.ForeColor = System.Drawing.Color.Red;
                lblStage.ForeColor = System.Drawing.Color.Red;
            }
            if (chkWaivedYN != null)
                chkWaivedYN.Enabled = drv["FLDWAIVEDYN"].ToString().Equals("1") ? true : false;

            if (General.GetNullableInteger(ViewState["rankexprowindex"].ToString()) != null && General.GetNullableInteger(ViewState["rankexprowindex"].ToString()) == e.Item.DataSetIndex)
            {
                if (chkWaivedYN != null)
                {
                    if (General.GetNullableInteger(ViewState["rankexpwaivedyn"].ToString()) != null && General.GetNullableInteger(ViewState["rankexpwaivedyn"].ToString()) == 1)
                    {
                        chkWaivedYN.Checked = true;
                        if (txtReason != null) txtReason.CssClass = "input_mandatory";
                    }
                    else
                    {
                        chkWaivedYN.Checked = false;
                        if (txtReason != null) txtReason.CssClass = "input";
                    }
                    ViewState["rankexprowindex"] = "";
                    ViewState["rankexpwaivedyn"] = "";
                    ViewState["rankexpedititem"] = "0";
                }
            }

            if (lblStageid != null && lblProposalStageid != null)
            {
                if (lblStageid.Text.Equals(lblProposalStageid.Text) && drv["FLDWAIVEDYN"].ToString().Equals("1")) // enabled only for proposal stage and if the document can be waived.
                {
                    if (chkCanbeWaivedYN != null)
                    {
                        chkCanbeWaivedYN.AutoPostBack = true;
                        chkCanbeWaivedYN.Enabled = true;
                    }
                }
            }

            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            LinkButton cmdSave = (LinkButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);
        }
    }

    protected void gvRankExp_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;



            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadLabel lblDocumentType = (RadLabel)e.Item.FindControl("lblDocumentType");
                CheckBox cb = (CheckBox)e.Item.FindControl("chkWaivedYN");
                RadTextBox txtReason = (RadTextBox)e.Item.FindControl("txtReason");
                RadLabel lblWaivedDocumentId = (RadLabel)e.Item.FindControl("lblWaivedDocumentId");
                CheckBox chkCanbeWaivedYN = (CheckBox)e.Item.FindControl("chkCanbeWaivedYN");

                if (!IsValidWaivedFilter((cb.Checked ? "2" : "0"), txtReason.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreReliefRequest.InsertWaivedDocument(int.Parse(ViewState["empid"].ToString()),
                    General.GetNullableInteger(ucVessel.SelectedVessel), int.Parse(ddlRank.SelectedValue),
                    General.GetNullableInteger(lblDocumentType.Text), null,
                    General.GetNullableInteger(cb.Checked ? "2" : "0"),
                    General.GetNullableInteger(ViewState["trainingmatrixid"].ToString()),
                    General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                    General.GetNullableString(txtReason.Text),
                    General.GetNullableGuid(lblWaivedDocumentId.Text),
                    General.GetNullableInteger(chkCanbeWaivedYN.Checked ? "1" : "0"));

                BindData();
                gvRankExp.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("EDITRANKEXP"))
            {
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("CANCELRANKEXP"))
            {
                ViewState["rankexprowindex"] = "";
                ViewState["rankexpwaivedyn"] = "";
                ViewState["rankexpedititem"] = "0";

                BindData();
                gvRankExp.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {

                RadLabel lblwaivedocid = (RadLabel)e.Item.FindControl("lblwaivedocid");
                PhoenixCrewOffshoreWaivingRequest.UpdateWaivingRequest(General.GetNullableGuid(lblwaivedocid.Text));
                BindData();
                gvRankExp.Rebind();
                ucStatus.Text = "Approved";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselTypeExp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvVesselTypeExp_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblShortfall = (RadLabel)e.Item.FindControl("lblShortfall");
            RadLabel lblVesselType = (RadLabel)e.Item.FindControl("lblVesselType");
            RadLabel lblVesselTypeExp = (RadLabel)e.Item.FindControl("lblVesselTypeExp");
            RadLabel lblStage = (RadLabel)e.Item.FindControl("lblStage");
            CheckBox chkWaivedYN = (CheckBox)e.Item.FindControl("chkWaivedYN");
            RadTextBox txtReason = (RadTextBox)e.Item.FindControl("txtReason");
            RadLabel lblStageid = (RadLabel)e.Item.FindControl("lblStageid");
            RadLabel lblProposalStageid = (RadLabel)e.Item.FindControl("lblProposalStageid");
            CheckBox chkCanbeWaivedYN = (CheckBox)e.Item.FindControl("chkCanbeWaivedYN");
            RadLabel lblPlanStatus = (RadLabel)e.Item.FindControl("lblPlanStatus");

            UserControlToolTip uctDate = (UserControlToolTip)e.Item.FindControl("ucToolTipDate");
            uctDate.Position = ToolTipPosition.TopCenter;
            uctDate.TargetControlId = chkWaivedYN.ClientID;


            CheckBox chkWaivedconfirmYN = (CheckBox)e.Item.FindControl("chkWaivedconfirmYN");
            UserControlToolTip ucWaiveToolTipDate = (UserControlToolTip)e.Item.FindControl("ucWaiveToolTipDate");
            ucWaiveToolTipDate.Position = ToolTipPosition.TopCenter;
            ucWaiveToolTipDate.TargetControlId = chkWaivedconfirmYN.ClientID;

            LinkButton cmdApprove = (LinkButton)e.Item.FindControl("cmdApprove");

            if (chkWaivedYN != null)
            {

                if (chkWaivedYN.Checked == true)
                {
                    if (cmdApprove != null) cmdApprove.Visible = true;
                }
                else
                {
                    if (cmdApprove != null) cmdApprove.Visible = false;
                }
            }
            if (chkWaivedconfirmYN != null)
            {
                if (chkWaivedconfirmYN.Checked == true)
                {
                    if (cmdApprove != null) cmdApprove.Visible = false;
                    //if (imgReject != null) imgReject.Visible = true;
                }
            }

            if (cmdApprove != null)
            {
                //cmdApprove.Visible = SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName);
                cmdApprove.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Waive ?')");
            }
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (lblShortfall.Text == "1")
            {
                lblVesselType.ForeColor = System.Drawing.Color.Red;
                lblVesselTypeExp.ForeColor = System.Drawing.Color.Red;
                lblStage.ForeColor = System.Drawing.Color.Red;
            }
            if (chkWaivedYN != null)
                chkWaivedYN.Enabled = drv["FLDWAIVEDYN"].ToString().Equals("1") ? true : false;

            if (General.GetNullableInteger(ViewState["vtexprowindex"].ToString()) != null && General.GetNullableInteger(ViewState["vtexprowindex"].ToString()) == e.Item.DataSetIndex)
            {
                if (chkWaivedYN != null)
                {
                    if (General.GetNullableInteger(ViewState["vtexpwaivedyn"].ToString()) != null && General.GetNullableInteger(ViewState["vtexpwaivedyn"].ToString()) == 1)
                    {
                        chkWaivedYN.Checked = true;
                        if (txtReason != null) txtReason.CssClass = "input_mandatory";
                    }
                    else
                    {
                        chkWaivedYN.Checked = false;
                        if (txtReason != null) txtReason.CssClass = "input";
                    }
                    ViewState["vtexprowindex"] = "";
                    ViewState["vtexpwaivedyn"] = "";
                    ViewState["vtexpedititem"] = "0";
                }
            }

            if (lblStageid != null && lblProposalStageid != null)
            {
                if (lblStageid.Text.Equals(lblProposalStageid.Text) && drv["FLDWAIVEDYN"].ToString().Equals("1")) // enabled only for proposal stage and if the document can be waived.
                {
                    if (chkCanbeWaivedYN != null)
                    {
                        chkCanbeWaivedYN.Enabled = true;
                    }
                }
            }

            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            LinkButton cmdSave = (LinkButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);
        }
    }

    protected void gvVesselTypeExp_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadLabel lblDocumentType = (RadLabel)e.Item.FindControl("lblDocumentType");
                CheckBox cb = (CheckBox)e.Item.FindControl("chkWaivedYN");
                RadTextBox txtReason = (RadTextBox)e.Item.FindControl("txtReason");
                RadLabel lblWaivedDocumentId = (RadLabel)e.Item.FindControl("lblWaivedDocumentId");
                CheckBox chkCanbeWaivedYN = (CheckBox)e.Item.FindControl("chkCanbeWaivedYN");

                if (!IsValidWaivedFilter((cb.Checked ? "2" : "0"), txtReason.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreReliefRequest.InsertWaivedDocument(int.Parse(ViewState["empid"].ToString()),
                    General.GetNullableInteger(ucVessel.SelectedVessel), int.Parse(ddlRank.SelectedValue),
                    General.GetNullableInteger(lblDocumentType.Text), null,
                    General.GetNullableInteger(cb.Checked ? "2" : "0"),
                    General.GetNullableInteger(ViewState["trainingmatrixid"].ToString()),
                    General.GetNullableGuid(ViewState["crewplanid"].ToString()),
                    General.GetNullableString(txtReason.Text),
                    General.GetNullableGuid(lblWaivedDocumentId.Text),
                    General.GetNullableInteger(chkCanbeWaivedYN.Checked ? "1" : "0"));

                BindData();
                gvVesselTypeExp.Rebind();
            }


            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {

                RadLabel lblwaivedocid = (RadLabel)e.Item.FindControl("lblwaivedocid");
                PhoenixCrewOffshoreWaivingRequest.UpdateWaivingRequest(General.GetNullableGuid(lblwaivedocid.Text));
                BindData();
                gvVesselTypeExp.Rebind();
                ucStatus.Text = "Approved";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPlannedVessel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindPlannedVessel();
    }
    private void BindPlannedVessel()
    {
        try
        {

            DataSet ds = PhoenixCrewOffshoreReliefRequest.ListPlannedVessel(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , int.Parse(ViewState["empid"].ToString()));


            gvPlannedVessel.DataSource = ds.Tables[0];


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindPlan(string crewplanid)
    {

        DataTable dt = PhoenixCrewPlanning.EditCrewPlan(new Guid(crewplanid));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            if (General.GetNullableInteger(dr["FLDVESSELID"].ToString()) != null)
            {
                DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(dr["FLDVESSELID"].ToString()));
                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ucVessel.bind();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ucVesselType.SelectedVesseltype = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                    ViewState["vsltype"] = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                    //ViewState["Charterer"] = ds.Tables[0].Rows[0]["FLDCHARTERER"].ToString();
                }
                DataSet ds1 = PhoenixCrewOffshoreCrewList.CrewOffshoreCurrentCharterer(General.GetNullableInteger(dr["FLDVESSELID"].ToString()), null);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    ViewState["Charterer"] = ds1.Tables[0].Rows[0]["FLDCHARTERERID"].ToString();
                }
                if (General.GetNullableInteger(dr["FLDRANKID"].ToString()) != null)
                    ddlRank.SelectedValue = dr["FLDRANKID"].ToString();
                if (General.GetNullableInteger(dr["FLDTRAININGMATRIXID"].ToString()) != null)
                {
                    BindTrainingMatrix();
                    //if (ddlTrainingMatrix.Items.FindItemByValue(dr["FLDTRAININGMATRIXID"].ToString()) != null)
                    //    ddlTrainingMatrix.SelectedValue = dr["FLDTRAININGMATRIXID"].ToString();
                }
                if (General.GetNullableDateTime(dr["FLDEXPECTEDJOINDATE"].ToString()) != null)
                    ucDate.Text = dr["FLDEXPECTEDJOINDATE"].ToString();
                if (General.GetNullableInteger(dr["FLDOFFSIGNERID"].ToString()) != null)
                {
                    BindOffSigner();
                    if (ddlOffSigner.Items.FindItemByValue(dr["FLDOFFSIGNERID"].ToString()) != null)
                        ddlOffSigner.SelectedValue = dr["FLDOFFSIGNERID"].ToString();
                }
            }
        }
    }

}