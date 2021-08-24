using System;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;
public partial class CrewNewApplicantRegister : PhoenixBasePage
{
    protected string Code = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["Newapp"] != null && Request.QueryString["AddAsEmployee"]==null)
            toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

        MenuSecurityUser.AccessRights = this.ViewState;
        MenuSecurityUser.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["ADDASEMPLOYEE"] = "";

            if (Request.QueryString["AddAsEmployee"] != null)
            {
                ViewState["ADDASEMPLOYEE"] = "1";
            }

            ddlGender.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();

            DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            ViewState["POOLLIST"] = (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) ? ds.Tables[0].Rows[0]["FLDPOOLLIST"].ToString() : string.Empty;

            ucPool.CssClass = ViewState["POOLLIST"].ToString() == string.Empty ? "" : "dropdown_mandatory";

            cblVesselType.DataSource = PhoenixRegistersVesselType.ListVesselType(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            cblVesselType.DataTextField = "FLDTYPEDESCRIPTION";
            cblVesselType.DataValueField = "FLDVESSELTYPEID";
            cblVesselType.DataBind();

            ddlGender.SelectedHard = "157";                 // defaulted to Male
            ddlNationality.SelectedNationality = PhoenixGeneralSettings.CurrentGeneralSetting.Nationality.ToString();

            if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
                ucPool.SelectedPool = Request.QueryString["pl"].ToString();
        }
    }

    protected void SecurityUser_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string header = "", error = "";
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                StringBuilder strvesseltype = new StringBuilder();

                foreach (RadListBoxItem item in cblVesselType.Items)
                {
                    if (item.Checked == true)
                    {
                        strvesseltype.Append(item.Value.ToString());
                        strvesseltype.Append(",");
                    }
                }

                if (strvesseltype.Length > 1)
                {
                    strvesseltype.Remove(strvesseltype.Length - 1, 1);
                }

                if (IsValidEntry(strvesseltype.ToString(),ref header ,ref error))
                {
                    RadWindowManager1.RadAlert(error, 400, 175, header, null);
                    //ucError.Visible = true;
                    return;
                }
                Guid? dtkey = null;
                DataSet ds = new DataSet();

                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                {
                    ds = PhoenixNewApplicantManagement.OffshoreNewApplicantQuickInsert(txtFirstName.Text.ToUpper(), txtMiddleName.Text.ToUpper(), txtLastName.Text.ToUpper(), txtEMail.Text.ToLower(),
                                        General.GetNullableInteger(ddlNationality.SelectedNationality), General.GetNullableDateTime(ucDateofBirth.Text)
                                        , General.GetNullableInteger(ddlGender.SelectedHard), txtPassport.Text.ToUpper(), txtSeamanBookNumber.Text.ToUpper().Replace(" ", ""),
                                        General.GetNullableString(strvesseltype.ToString())
                                        , General.GetNullableInteger(ucRankApplied.SelectedRank).Value, ucTelephone.Text, ucMobile.Text, ref dtkey
                                        , General.GetNullableInteger(ucPool.SelectedPool)
                                        );
                }
                else
                {
                    if (ViewState["ADDASEMPLOYEE"]!= null && ViewState["ADDASEMPLOYEE"].ToString() =="1") { 
                    ds = PhoenixNewApplicantManagement.NewApplicantEmployeeInsert(txtFirstName.Text.ToUpper(), txtMiddleName.Text.ToUpper(), txtLastName.Text.ToUpper(), txtEMail.Text.ToLower(),
                                        General.GetNullableInteger(ddlNationality.SelectedNationality), General.GetNullableDateTime(ucDateofBirth.Text)
                                        , General.GetNullableInteger(ddlGender.SelectedHard), txtPassport.Text.ToUpper(), txtSeamanBookNumber.Text.ToUpper().Replace(" ", ""),
                                        General.GetNullableString(strvesseltype.ToString())
                                        , General.GetNullableInteger(ucRankApplied.SelectedRank).Value, ucTelephone.Text, ucMobile.Text, ref dtkey
                                        , General.GetNullableInteger(ucPool.SelectedPool)
                                        );

                        Filter.CurrentCrewSelection = ds.Tables[0].Rows[0][0].ToString();
                        if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                            Response.Redirect("../CrewOffshore/CrewOffshorePersonalGeneral.aspx?pl=" + Request.QueryString["pl"] + "&launchedfrom=" + Request.QueryString["launchedfrom"], false);
                        else
                            Response.Redirect("CrewPersonalGeneral.aspx", false);

                    }
                    else{
                        ds = PhoenixNewApplicantManagement.NewApplicantQuickInsert(txtFirstName.Text.ToUpper(), txtMiddleName.Text.ToUpper(), txtLastName.Text.ToUpper(), txtEMail.Text.ToLower(),
                                        General.GetNullableInteger(ddlNationality.SelectedNationality), General.GetNullableDateTime(ucDateofBirth.Text)
                                        , General.GetNullableInteger(ddlGender.SelectedHard), txtPassport.Text.ToUpper(), txtSeamanBookNumber.Text.ToUpper().Replace(" ", ""),
                                        General.GetNullableString(strvesseltype.ToString())
                                        , General.GetNullableInteger(ucRankApplied.SelectedRank).Value, ucTelephone.Text, ucMobile.Text, ref dtkey
                                        , General.GetNullableInteger(ucPool.SelectedPool)
                                        );

                        if (Filter.CurrentNewApplicantSelection != null)
                        {
                            Filter.CurrentNewApplicantSelection = ds.Tables[0].Rows[0][0].ToString();
                            if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                                Response.Redirect("../CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?pl=" + Request.QueryString["pl"] + "&launchedfrom=" + Request.QueryString["launchedfrom"], false);
                            else
                                Response.Redirect("CrewNewApplicantPersonal.aspx", false);
                        }
                        else
                        {
                            Filter.CurrentNewApplicantSelection = ds.Tables[0].Rows[0][0].ToString();
                            if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                                Response.Redirect("../CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?pl=" + Request.QueryString["pl"] + "&launchedfrom=" + Request.QueryString["launchedfrom"], false);
                            else
                                Response.Redirect("CrewNewApplicantPersonalGeneral.aspx", false);
                        }

                    }

                }
                ViewState["dtkey"] = dtkey;
                if (Request.Files["txtFileUpload"] !=null && Request.Files["txtFileUpload"].ContentLength > 0)
                {

                    PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.CREW, null, ".doc,.docx", string.Empty, "CREWRESUME");

                }
                

            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "New Applicant Registration";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        else
        {
            if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                Response.Redirect("../CrewOffshore/CrewOffshoreNewApplicantList.aspx?pl=" + Request.QueryString["pl"] + "&launchedfrom=" + Request.QueryString["launchedfrom"], false);
            else
                Response.Redirect("CrewNewApplicantQueryActivity.aspx", false);
        }
    }

    protected void DeselectVesseltype(object sender, EventArgs e)
    {
        if (chkSeaFarerExp.Checked == true)
        {
            cblVesselType.ClearChecked();
            cblVesselType.SelectedValue = "";
            cblVesselType.Enabled = false;
        }
        else
        {
            dvVesselType.Attributes["class"] = "input_mandatory";
            cblVesselType.Enabled = true;
        }

    }

    private bool IsValidEntry(string strVesselType ,ref string headermessage,ref string errormessage)
    {
         headermessage= "Please provide the following required information";
         errormessage="";
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtFirstName.Text.Trim().Length == 0)
        errormessage = errormessage + "First name is required.</br>";
        //ucError.ErrorMessage = "First name is required.";

        if (txtSeamanBookNumber.Text.Trim().Length == 0)
            errormessage = errormessage + "Seaman Book Number is required.</br>";
            //ucError.ErrorMessage = "Seaman Book Number is required.";
        if (!General.IsvalidEmail(txtEMail.Text) && txtEMail.Text.ToUpper() != "NA")
            errormessage = errormessage + "E-Mail is required.</br>";

        //ucError.ErrorMessage = "E-Mail is required.";
        if ((chkSeaFarerExp.Checked == false) && (strVesselType.Trim().Length == 0))
            ucError.ErrorMessage = "Select Atleast one vessel type.";

        if (General.GetNullableInteger(ucRankApplied.SelectedRank) == null)
            ucError.ErrorMessage = "Applied Rank is required";

        if (ViewState["POOLLIST"].ToString() != string.Empty && General.GetNullableInteger(ucPool.SelectedPool) == null)
        {
            ucError.ErrorMessage = "Pool is required";
        }
        if (!General.IsValidPhoneNumber(ucMobile.Text))
        {
            ucError.ErrorMessage = "Mobile Number is required";
        }
        if (!errormessage.Length.Equals(0))
        {
            return true;
        }
        else
            return false;
        //return (!errormessage.Length.Equals(0));
    }

}
