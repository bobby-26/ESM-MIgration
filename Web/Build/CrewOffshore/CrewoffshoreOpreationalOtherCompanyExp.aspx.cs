using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class CrewOffshore_CrewoffshoreOpreationalOtherCompanyExp : PhoenixBasePage
{
    string empid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        empid = Request.QueryString["empid"].ToString();

        if (!IsPostBack)
        {
            ViewState["TYPEOFANCORHANDLED"] = "";
            ViewState["ROVCLASS"] = "";

            SetEmployeePrimaryDetails();
            BindddType();
            Bindrovclass();
            if (Request.QueryString["empid"] != null)
            {
                EditCrewOperationalOtherExperience(int.Parse(empid));
            }
           
        }
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuCrewOtherExperienceList.AccessRights = this.ViewState;
        MenuCrewOtherExperienceList.MenuList = toolbarmain.Show();
    }
    protected void CrewAppraisalMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                System.Text.StringBuilder strROVClass = new System.Text.StringBuilder();
                foreach (ListItem item in cblRovType.Items)
                {
                    if (item.Selected == true)
                    {
                        if (item.Value.ToString() != strROVClass.ToString())
                        {
                            strROVClass.Append(item.Value.ToString());
                            strROVClass.Append(",");
                        }
                    }
                }
                ViewState["ROVCLASS"] = strROVClass.ToString();

                System.Text.StringBuilder strValue = new System.Text.StringBuilder();

                foreach (ListItem item in chkValue.Items)
                {
                    if (item.Selected == true)
                    {
                        if (item.Value.ToString() != strValue.ToString())
                        {
                            strValue.Append(item.Value.ToString());
                            strValue.Append(",");
                        }
                    }
                }
                ViewState["TYPEOFANCORHANDLED"] = strValue.ToString();

                PhoenixCrewOffshoreOtherExperience.SaveOperationalexp(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                         , Convert.ToInt32(empid)
                                                        
                                                         , General.GetNullableInteger(ddlDryDockingsattended.SelectedValue)
                                                         , General.GetNullableInteger(uctxtNumberofOVIDInspections.Text)
                                                         , General.GetNullableInteger(uctctNumberofOSVISInspections.Text)
                                                         , General.GetNullableString(ViewState["ROVCLASS"].ToString())
                                                         , General.GetNullableInteger(txtNumberofOceanTows.Text)
                                                         , General.GetNullableInteger(txtNumberofRigMoves.Text)
                                                         , General.GetNullableInteger(ddlFSIPSC.SelectedValue)
                                                         , General.GetNullableInteger(ddlFMEA.SelectedValue)
                                                         , General.GetNullableInteger(ddlDPAnnuals.SelectedValue)
                                                         , General.GetNullableInteger(ddlDeliveryOrTakeover.SelectedValue)
                                                         , General.GetNullableInteger(ddlHeavyLift.SelectedValue)
                                                         , General.GetNullableInteger(ddlDKDMud.SelectedValue)
                                                         , General.GetNullableInteger(ddlMethanol.SelectedValue)
                                                         , General.GetNullableInteger(ddlGlycol.SelectedValue)
                                                         , General.GetNullableString(ViewState["TYPEOFANCORHANDLED"].ToString())
                                                         , General.GetNullableInteger(ddlExperienceingranchors.SelectedValue)
                                                         , General.GetNullableInteger(ddlExperiencestowage.SelectedValue)
                                                         //, General.GetNullableInteger(txtSizelengthofchain.Text)
                                                         //, General.GetNullableDecimal(txtSizelengthofchainmeter.Text)
                                                         , General.GetNullableDecimal(ucAnchorhandlingdepth.Text)
                                                         , General.GetNullableInteger(ddlExperienceChainHandling.SelectedValue)
                                                         //, General.GetNullableInteger(txtAnchorHandling.Text)
                                                         //, General.GetNullableInteger(txtSupply.Text)
                                                         //, General.GetNullableInteger(txtDiveSupport.Text)
                                                         //, General.GetNullableInteger(txtROV.Text)
                                                         //, General.GetNullableInteger(txtFlotel.Text)
                                                         );
                ucStatus.Text = "Updated Successfully";
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(empid));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindddType()
    {
        chkValue.Visible = true;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = PhoenixRegistersDMRAnchorHandilingType.DMRAnchorHandilingTypeSearch("", 1, 100, ref iRowCount, ref iTotalPageCount);
        DataTable dt = ds.Tables[0];
        chkValue.DataSource = dt;
        chkValue.DataBind();
    }
    protected void Bindrovclass()
    {
        cblRovType.Visible = true;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = PhoenixRegistersDMRRovType.MDRRovTypeSearch("", 1, 100, ref iRowCount, ref iTotalPageCount);
        DataTable dt = ds.Tables[0];
        cblRovType.DataSource = dt;
        cblRovType.DataBind();
    }

    protected void EditCrewOperationalOtherExperience(int empid)
    {
        DataSet ds = PhoenixNewApplicantOtherExperience.ListEmployeeOperationalOtherExperienceds(Convert.ToInt32(empid));

        //DataTable dt = ds.Tables[0];
        DataTable dt1 = ds.Tables[0];

        if (dt1.Rows.Count > 0)
        {
            ddlDryDockingsattended.SelectedValue = dt1.Rows[0]["FLDNOOFDRYDOCKINGSATTENDED"].ToString();
            uctxtNumberofOVIDInspections.Text = dt1.Rows[0]["FLDNOOFOVIDINSPECTIONS"].ToString();
            uctctNumberofOSVISInspections.Text = dt1.Rows[0]["FLDNOOFOSVISINSPECTIONS"].ToString();
            txtNumberofOceanTows.Text = dt1.Rows[0]["FLDNOOFOCEANTOWS"].ToString();
            txtNumberofRigMoves.Text = dt1.Rows[0]["FLDNOOFRIGMOVES"].ToString();
            ddlFSIPSC.SelectedValue = dt1.Rows[0]["FLDFSIPSCYN"].ToString();
            ddlFMEA.SelectedValue = dt1.Rows[0]["FLDFMEAYN"].ToString();
            ddlDPAnnuals.SelectedValue = dt1.Rows[0]["FLDDPANNUALSYN"].ToString();
            ddlDeliveryOrTakeover.SelectedValue = dt1.Rows[0]["FLDNEWDELIVERYTAKEOVERYN"].ToString();
            ddlHeavyLift.SelectedValue = dt1.Rows[0]["FLDHEAVYLIFTPROJECTCARGOESYN"].ToString();
            ddlDKDMud.SelectedValue = dt1.Rows[0]["FLDDKDMUDYN"].ToString();
            ddlMethanol.SelectedValue = dt1.Rows[0]["FLDMETHANOLYN"].ToString();
            ddlGlycol.SelectedValue = dt1.Rows[0]["FLDGLYCOLYN"].ToString();
            ddlExperienceChainHandling.SelectedValue = dt1.Rows[0]["FLDEXPCHAINHANDLINGYN"].ToString();
            ucAnchorhandlingdepth.Text = dt1.Rows[0]["FLDMAXIMUMANCHORHANDLINGDEPTH"].ToString();
            ddlExperienceingranchors.SelectedValue = dt1.Rows[0]["FLDEXPERIENCEINGRAPPLING"].ToString();
            ddlExperiencestowage.SelectedValue= dt1.Rows[0]["FLDEXPERIENCESTOWAGEOFCHAINS"].ToString();
            if (dt1.Rows.Count > 0)
            {

                string Str = dt1.Rows[0]["FLDTYPESOFANCHORSHANDLED"].ToString();
                string[] str1 = Str.Split(',');

                foreach (ListItem li in chkValue.Items)
                {
                    foreach (string s in str1)
                    {
                        if (li.Value.Equals(s))
                        {
                            li.Selected = true;
                        }
                    }
                }
                string Strrov = dt1.Rows[0]["FLDCLASSOFROV"].ToString();
                string[] strrov1 = Strrov.Split(',');

                foreach (ListItem li1 in cblRovType.Items)
                {
                    foreach (string s1 in strrov1)
                    {
                        if (li1.Value.Equals(s1))
                        {
                            li1.Selected = true;
                        }
                    }
                }

            }
            else { ResetCrewOtherExperience(); }
        }
    }
    protected void ResetCrewOtherExperience()
    {
        ddlDryDockingsattended.SelectedValue = "";
        uctxtNumberofOVIDInspections.Text = "";
        uctctNumberofOSVISInspections.Text = "";
        txtNumberofOceanTows.Text = "";
        txtNumberofRigMoves.Text = "";
        ddlFSIPSC.SelectedValue = "";
        ddlFMEA.SelectedValue = "";
        ddlDPAnnuals.SelectedValue = "";
        ddlDeliveryOrTakeover.SelectedValue = "";
        ddlHeavyLift.SelectedValue = "";
        ddlDKDMud.SelectedValue = "";
        ddlMethanol.SelectedValue = "";
        ddlGlycol.SelectedValue = "";
        ddlExperienceingranchors.SelectedValue = "";
        ddlExperiencestowage.SelectedValue = "";
        ucAnchorhandlingdepth.Text = "";
        ddlExperienceingranchors.SelectedValue = "";
    }
}
