using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
public partial class Crew_CrewHAAssessReEmployment : PhoenixBasePage
{
    string strEmployeeId;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        try
        {
            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else
                strEmployeeId = Request.QueryString["empid"];

            if (string.IsNullOrEmpty(Request.QueryString["PendingReviewid"]))
                ViewState["PENDINGREVIEWID"] = string.Empty;
            else
                ViewState["PENDINGREVIEWID"] = Request.QueryString["PendingReviewid"];

            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuDO.AccessRights = this.ViewState;
            MenuDO.MenuList = toolbar.Show();

            Filter.CurrentCrewSelection = Request.QueryString["empid"];
            if (!IsPostBack)
            {
                ViewState["DOAID"] = string.Empty;
                ViewState["LAUNCHEDFROM"] = string.Empty;
                if (General.GetNullableInteger(strEmployeeId) == null)
                {
                    ucError.ErrorMessage = "Select a Employee from Query Activity";
                    ucError.Visible = true;
                    return;
                }
                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"] != "")
                    ViewState["LAUNCHEDFROM"] = Request.QueryString["launchedfrom"].ToString();
                SetEmployeePrimaryDetails();
                //DAO();
                cblAddressType.DataSource = PhoenixRegistersAddress.ListAddress("128");
                cblAddressType.DataTextField = "FLDNAME";
                cblAddressType.DataValueField = "FLDADDRESSCODE";
                cblAddressType.DataBind();
                ViewState["NTBRID"] = string.Empty;
                NTBREdit();
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void PrincipalManagerClick(object sender, EventArgs e)
    {
        if (rblPrincipalManager.SelectedValue == "2")
        {
            dvAddressType.Visible = true;

            dvAddressType.Attributes["class"] = "input_mandatory";
            cblAddressType.Enabled = true;
            ddlManager.Visible = false;
        }
        else
        {
            dvAddressType.Visible = false;
            ddlManager.Visible = true;
            cblAddressType.SelectedValue = null;
            cblAddressType.Enabled = false;
            dvAddressType.Attributes["class"] = "";
        }
    }
    protected void rblstblremep_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblstblremep.SelectedValue == "2")
        {
            //Response.Redirect("../Crew/CrewNTBRGeneral.aspx?empid=" + Request.QueryString["empid"]);
            rblPrincipalManager.Enabled = true;
            rblPrincipalManager.CssClass = "input_mandatory";
            ddlManager.Enabled = true;
            ddlManager.CssClass = "input_mandatory";
            cblAddressType.Enabled = true;
            cblAddressType.CssClass = "input_mandatory";
            ddlNTBRReason.Enabled = true;
            ddlNTBRReason.CssClass = "input_mandatory";
            txtNTBRDate.Enabled = true;
            txtNTBRDate.CssClass = "input_mandatory";
            txtNTBRRemarks.Enabled = true;
            txtNTBRRemarks.CssClass = "input_mandatory";

            txtHaAssessmentRemarks.Enabled = false;
            txtHaAssessmentRemarks.CssClass = "";
        }
        if (rblstblremep.SelectedValue == "1")
        {
            rblPrincipalManager.Enabled = false;
            rblPrincipalManager.CssClass = "";
            ddlManager.Enabled = false;
            ddlManager.CssClass = "";
            cblAddressType.Enabled = false;
            cblAddressType.CssClass = "";
            ddlNTBRReason.Enabled = false;
            ddlNTBRReason.CssClass = "";
            txtNTBRDate.Enabled = false;
            txtNTBRDate.CssClass = "";
            txtNTBRRemarks.Enabled = false;
            txtNTBRRemarks.CssClass = "";
            
            txtHaAssessmentRemarks.Enabled = true;
            txtHaAssessmentRemarks.CssClass = "input_mandatory";
        }
    }
    protected void DOA_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidstbl())
                {
                    ucError.Visible = true;
                    return;
                }

                if (rblstblremep.SelectedValue == "1")
                {
                    if (!IsValidstblrmrk())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixCrewDateOfAvailability.ReEmploymentAssessmentHA(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                            General.GetNullableInteger(strEmployeeId),
                                                                            General.GetNullableInteger(rblstblremep.SelectedValue.Trim()),
                                                                            General.GetNullableString(txtHaAssessmentRemarks.Text)
                                                                            , General.GetNullableGuid(ViewState["PENDINGREVIEWID"].ToString())
                                                                  );
                    string Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += " fnReloadList();";
                    Script += "</script>" + "\n";
                    //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
                }
                if (rblstblremep.SelectedValue == "2")
                {
                    if (!IsValidNTBRMgr())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    else
                    {
                        StringBuilder straddresstype = new StringBuilder();

                        foreach (ListItem item in cblAddressType.Items)
                        {
                            if (item.Selected == true)
                            {
                                straddresstype.Append(item.Value.ToString());
                                straddresstype.Append(",");
                            }
                        }

                        if (straddresstype.Length > 1)
                        {
                            straddresstype.Remove(straddresstype.Length - 1, 1);
                        }

                        if (string.IsNullOrEmpty(ViewState["NTBRID"].ToString()))
                        {
                            if (rblPrincipalManager.SelectedValue.ToString() == "1")
                            {
                                PhoenixCrewNTBR.CrewNTBRInsert(General.GetNullableInteger(strEmployeeId).Value
                                                                      , DateTime.Parse(txtNTBRDate.Text)
                                                                      , ddlManager.SelectedAddress
                                                                      , txtNTBRRemarks.Text
                                                                      , int.Parse(ddlNTBRReason.SelectedNTBRMgrReason)
                                                                      , null
                                                                      , null
                                                                      , Convert.ToInt32(rblPrincipalManager.SelectedValue)
                                                                      );
                            }
                            else if (rblPrincipalManager.SelectedValue.ToString() == "2")
                            {
                                PhoenixCrewNTBR.CrewNTBRPrincipalInsert(General.GetNullableInteger(strEmployeeId).Value
                                                                           , DateTime.Parse(txtNTBRDate.Text)
                                                                           , straddresstype.ToString()
                                                                           , txtNTBRRemarks.Text
                                                                           , int.Parse(ddlNTBRReason.SelectedNTBRMgrReason)
                                                                           , null
                                                                           , null
                                                                           , Convert.ToInt32(rblPrincipalManager.SelectedValue)
                                                                           );

                            }
                        }
                        else
                        {
                            PhoenixCrewNTBR.CrewNTBRUpdate(General.GetNullableInteger(strEmployeeId).Value
                                                    , General.GetNullableInteger(ViewState["NTBRID"].ToString())
                                                    , DateTime.Parse(txtNTBRDate.Text)
                                                    , rblPrincipalManager.SelectedValue.ToString() == "1" ? ddlManager.SelectedAddress : straddresstype.ToString()
                                                    , txtNTBRRemarks.Text
                                                    , int.Parse(ddlNTBRReason.SelectedNTBRMgrReason)
                                                    , null
                                                    , null
                                                    , Convert.ToInt32(rblPrincipalManager.SelectedValue)
                                                    );
                        }                      
                        PhoenixCrewDateOfAvailability.ReEmploymentAssessmentHA(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                            General.GetNullableInteger(strEmployeeId),
                                                                            General.GetNullableInteger(rblstblremep.SelectedValue.Trim()),
                                                                            null
                                                                            , General.GetNullableGuid(ViewState["PENDINGREVIEWID"].ToString())
                                                                  );
                        string Script = "";
                        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                        Script += " fnReloadList();";
                        Script += "</script>" + "\n";
                        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
                    }
                    NTBREdit();
                }
               
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void Reset()
    {
        ViewState["NTBRID"] = string.Empty;
        txtNTBRDate.Text = string.Empty;
        txtNTBRRemarks.Text = string.Empty;
        ddlNTBRReason.SelectedNTBRMgrReason = string.Empty;
        txtNTBRDate.ReadOnly = false;
        txtNTBRDate.CssClass = "input_mandatory";
        ddlNTBRReason.Readonly = true;
        ddlNTBRReason.CssClass = "input_mandatory";
        txtNTBRRemarks.ReadOnly = false;
        txtNTBRRemarks.CssClass = "input_mandatory";

        cblAddressType.SelectedValue = null;
        dvAddressType.Visible = false;
        rblPrincipalManager.Enabled = true;
        ddlManager.Enabled = true;
        rblPrincipalManager.SelectedValue = "1";
        ddlManager.Visible = true;
        ddlManager.SelectedAddress = "";
        
    }
    private void NTBREdit()
    {
        try
        {

            DataTable dt;
            dt = PhoenixCrewNTBR.CrewNTBRPrincipalMangerEdit(General.GetNullableInteger(strEmployeeId).Value);

            if (dt.Rows.Count > 0)
            {
                txtNTBRDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDNTBRDATE"].ToString());
                txtNTBRRemarks.Text = dt.Rows[0]["FLDNTBRREMARKS"].ToString();
                ddlNTBRReason.SelectedNTBRMgrReason = dt.Rows[0]["FLDREASONID"].ToString();
                ViewState["NTBRID"] = dt.Rows[0]["FLDNTBRID"].ToString();
                rblPrincipalManager.SelectedValue = dt.Rows[0]["FLDMANAGERYN"].ToString();
                if (dt.Rows[0]["FLDMANAGERYN"].ToString() == "1")
                {
                    dvAddressType.Visible = false;
                    ddlManager.Visible = true;
                    ddlManager.SelectedAddress = dt.Rows[0]["FLDADDRESSCODE"].ToString();
                }
                else
                {
                    dvAddressType.Attributes["class"] = "readonlytextbox";
                    dvAddressType.Visible = true;
                    ddlManager.Visible = false;
                    string[] addresstype = dt.Rows[0]["FLDADDRESSCODE"].ToString().Split(',');
                    foreach (string item in addresstype)
                    {
                        if (item.Trim() != "")
                        {
                            cblAddressType.Items.FindByValue(item).Selected = true;
                        }
                    }

                }
            }
            else
            {
                Reset();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidNTBRMgr()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(strEmployeeId) == null)
        {
            ucError.ErrorMessage = "Select a Employee from Query Activity";

        }
        //if (ViewState["NTBRID"].ToString() == "")
        //{
            if (rblPrincipalManager.SelectedValue == "1")
            {
                if (General.GetNullableInteger(ddlManager.SelectedAddress) == null)
                    ucError.ErrorMessage = "Please select a Manager";
            }
            else
            {
                if (cblAddressType.SelectedValue == "")
                    ucError.ErrorMessage = "Please select a Principal";
            }
        //}

        if (string.IsNullOrEmpty(txtNTBRDate.Text))
            ucError.ErrorMessage = "NTBR Date is required.";
        if (General.GetNullableDateTime(txtNTBRDate.Text) > DateTime.Now.Date)
            ucError.ErrorMessage = "NTBR Date cannot be grater than Todays Date";
        if (ddlNTBRReason.SelectedNTBRMgrReason.Trim().Equals("Dummy") || ddlNTBRReason.SelectedNTBRMgrReason.Trim().Equals(""))
            ucError.ErrorMessage = "NTBR Reason is required.";

        if (txtNTBRRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "NTBR Remarks is required.";

        return (!ucError.IsError);
    }

    public bool IsValidstbl()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (rblstblremep.SelectedValue != "1" && rblstblremep.SelectedValue != "2")
        {
            ucError.ErrorMessage = "Please provide the answer for 'Is candidate Suitable for re-employment:'" ;

        }
        return (!ucError.IsError);
    }
    public bool IsValidstblrmrk()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtHaAssessmentRemarks.Text == null || txtHaAssessmentRemarks.Text == "")
        {
            ucError.ErrorMessage = "Please provide ReEmployment Remark: ";

        }
        return (!ucError.IsError);
    }


    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(strEmployeeId));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeCode.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtPayRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtEmployeeName.Text = dt.Rows[0]["FLDLASTNAME"].ToString() + " " + dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtSignedOff.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDSIGNOFFDATE"].ToString()));
                txtLastVessel.Text = dt.Rows[0]["FLDLASTVESSELNAME"].ToString();
                //txtDOA.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDDOA"].ToString()));
                ViewState["STATUS"] = dt.Rows[0]["FLDSTATUSTYPE"].ToString();
                ViewState["ONBOARD"] = dt.Rows[0]["FLDPRESENTVESSEL"].ToString();
                //if (ViewState["STATUS"].ToString() == "0")
                //{
                //    txtDOA.CssClass = "input";
                //}
                //if (txtDOA != null && txtDOA.Text != "" && ViewState["LAUNCHEDFROM"] != null && ViewState["LAUNCHEDFROM"].ToString() != "")
                //{
                //    lblDateOfTeleconference.Text = "Last Contact Date";
                //    lblFollowUpDate.Text = "Next Contact Date";
                //}
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RemarksMandatory_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //CheckBox cbk = (CheckBox)sender;
            //if (cbk.Checked)
            //{
            //    txtRemarks.CssClass = "input_mandatory";
            //    txtRemarks.ReadOnly = false;
            //}
            //else
            //{
            //    txtRemarks.CssClass = "readonlytextbox";
            //    txtRemarks.ReadOnly = true;
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
