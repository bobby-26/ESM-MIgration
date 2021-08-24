using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Generic;
using Telerik.Web.UI;

public partial class CrewOffshoreEmployeeSearch : PhoenixBasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
            toolbarsub.AddButton("Search", "SEARCH", ToolBarDirection.Right);

            CrewQuery.AccessRights = this.ViewState;
            CrewQuery.MenuList = toolbarsub.Show();

            toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Save Search", "SAVESEARCH", ToolBarDirection.Right);
            CrewQueryGeneral.AccessRights = this.ViewState;
            CrewQueryGeneral.MenuList = toolbarsub.Show();

            cblHasMissing.Attributes.Add("onclick", "radioMe(event);");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                btnSearchDelete.Attributes.Add("onclick", "return fnConfirmDelete(event, 'Are you sure you want delete this search?'); return false;");
                BindSavedSearch();

                foreach (RadComboBoxItem itm in ddlColumnlist.Items)
                {
                    itm.Checked = true;
                }

                gvDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindSavedSearch()
    {
        DataTable dt = PhoenixCrewOffshoreEmployee.CrewOffshoreEmployeeSearchList();
        ddlSavedSearch.Items.Clear();
        ddlSavedSearch.DataSource = dt;
        ddlSavedSearch.DataBind();
        ddlSavedSearch.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--", ""));
        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "resize", "resize();", true);
    }
    protected void ddlSavedSearch_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = PhoenixCrewOffshoreEmployee.CrewOffshoreEmployeeSearchEdit(General.GetNullableInteger(ddlSavedSearch.SelectedValue));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtFullName1.Text = dr["FLDFULLNAME1"].ToString();
            txtFullName2.Text = dr["FLDFULLNAME2"].ToString();
            txtSearchName.Text = dr["FLDSEARCHNAME"].ToString();
            txtPassportNo.Text = dr["FLDPASSPORTNO"].ToString();
            lstRank.selectedlist = dr["FLDCURRENTRANK"].ToString();

            if (dr["FLDZONE"].ToString() == string.Empty || dr["FLDZONE"].ToString() == "Dummy")
                lstZone.selectedlist = General.GetNullableString("-1");
            else
                lstZone.selectedlist = dr["FLDZONE"].ToString();

            ddlVessel.SelectedVessel = dr["FLDCURRENTVESSEL"].ToString();
            ddlVesselType.SelectedVesseltype = dr["FLDCURRENTVESSELTYPE"].ToString();
            ddlPlannedVessel.SelectedVessel = dr["FLDPLANNEDVESSEL"].ToString();
            ddlPlannedVesselType.SelectedVesseltype = dr["FLDPLANNEDVESSELTYPE"].ToString();
            ucCurrentStatus.SelectedHard = dr["FLDCURRENTSTATUS"].ToString();
            ddlPlannedShipManagedBy.SelectedAddress = dr["FLDMANAGEDBY"].ToString();
            ddlPlannedShipOwnedBy.SelectedAddress = dr["FLDOWNEDBY"].ToString();
            txtAvailableFrom.Text = dr["FLDAVAILABLEFROM"].ToString();
            ddlNationality.SelectedNationality = dr["FLDNATIONALITY"].ToString();
            ddlFlag.SelectedFlag = dr["FLDCURRENTSHIPFLAG"].ToString();
            lstVesselType.SelectedVesseltype = dr["FLDEXPERIENCESHIPTYPE"].ToString();
            lstRankList.selectedlist = dr["FLDEXPERIENCERANK"].ToString();
            ddlExperienceShip.SelectedVessel = dr["FLDEXPERIENCESHIP"].ToString();
            txtExperienceFrom.Text = dr["FLDEXPERIENCEFROM"].ToString();
            txtExperienceTo.Text = dr["FLDEXPERIENCETO"].ToString();
            ddlEngineType.SelectedEngineName = dr["FLDEXPERIENCEENGINE"].ToString();
            ddlSignedOff.SelectedVessel = dr["FLDSIGNEDOFFSHIP"].ToString();
            ddlSignedOffShipOwnedBy.SelectedAddress = dr["FLDSIGNEDOFFSHIPOWNEDBY"].ToString();
            ddlSignOffReason.SelectedSignOffReason = dr["FLDSIGNEDOFFREASON"].ToString();
            ddlSignedOffMonths.SelectedValue = dr["FLDSIGNEDOFFMONTHS"].ToString();
            ddlRank.SelectedRank = dr["FLDSTARTINGRANK"].ToString();
            ddlCourse.SelectedCourse = dr["FLDTRAININGCOURSE"].ToString();
            ucInstitution.SelectedAddress = dr["FLDTRAININGINSTITUTE"].ToString();
            ddlLicence.SelectedDocument = dr["FLDCOC"].ToString();
            ddlFlagcoc.SelectedCountry = dr["FLDFLAGOFCOC"].ToString();
            txtNewapplicantID.Text = dr["FLDNEWAPPLICANTID"].ToString();
            txtAddress.Text = dr["FLDADDRESS"].ToString();
            txtPreviousShip.Text = dr["FLDPREVIOUSVESSEL"].ToString();
            txtOwnerName.Text = dr["FLDOWNERNAME"].ToString();
            ucCountryVisaheld.SelectedCountry = dr["FLDVISAHELD"].ToString();
            ddleocnod.SelectedValue = dr["FLDEOCNOOFDAYS"].ToString();
            ucExpiryDocument.SelectedDocumentType = dr["FLDEXPIRYDOCUMENT"].ToString();
            txtMinimumAge.Text = dr["FLDMINIMUMAGE"].ToString();
            txtMaximumAge.Text = dr["FLDMAXIMUMAGE"].ToString();
            ddlTrainingNeeds.SelectedCourse = dr["FLDTRAININGNEEDS"].ToString();
            ddlMedicalTest.SelectedDocument = dr["FLDMEDICALTEST"].ToString();
            txtMedicalTestFrom.Text = General.GetDateTimeToString(dr["FLDMEDICALTESTFROM"].ToString());
            txtMedicalTestTo.Text = General.GetDateTimeToString(dr["FLDMEDICALTESTTO"].ToString());
            txtFileNo.Text = dr["FLDFILENO"].ToString();
            ucCurrentStage.SelectedHard = dr["FLDPLANSTATUS"].ToString();
            txtPhoneNumber.Text = dr["FLDPHONENUMBER"].ToString();
            txtMobileNumber.Text = dr["FLDMOBILENUMBER"].ToString();

            if (dr["FLDHASEMAILADDRESSYN"].ToString() == "1")
                chkHasEmailAddress.Checked = true;

            if (dr["FLDHASGENERALREMARKYN"].ToString() == "1")
                chkGeneralRemark.Checked = true;

            if (dr["FLDISNOTACTIVEYN"].ToString() == "1")
                chkIsNotActive.Checked = true;

            txtNotActiveFrom.Text = General.GetDateTimeToString(dr["FLDNOTACTIVEFROM"].ToString());
            ddlInactiveReason.SelectedHard = dr["FLDNOTACTIVEREASON"].ToString();

            if (dr["FLDISNTBEYN"].ToString() == "1")
                chkIsNTBE.Checked = true;

            txtShowRecord.Text = dr["FLDSHOWRECORDS"].ToString();
            ddlOrderby1.SelectedValue = dr["FLDORDERBYFIRST"].ToString();
            ddlOrderby2.SelectedValue = dr["FLDORDERBYSECOND"].ToString();

            ucTechFleet.SelectedFleet = dr["FLDTECHNICALGROUP"].ToString();
            ucSignOffTechGroup.SelectedFleet = dr["FLDSIGNEDOFFVESSELTECHGROUP"].ToString();
            ddlExpiryCOC.SelectedDocument = dr["FLDEXPIRYCOC"].ToString();
            ddlExpiryCourse.SelectedCourse = dr["FLDEXPIRYCOURSE"].ToString();
            if (dr["FLDHASTRAININGNEEDS"].ToString() == "1")
                chkTrainingNeeds.Checked = true;
            else
                chkTrainingNeeds.Checked = false;
            if (dr["FLDHASMISSING"].ToString() != string.Empty)
            {
                foreach (RadListBoxItem item in cblHasMissing.Items)
                {
                    item.Checked = dr["FLDHASMISSING"].ToString().Contains(item.Value) ? true : false;
                }
            }

            int s4 = cblHasMissing.CheckedItems.Count;

            if (dr["FLDDISPLAYCOLUMNS"].ToString() != string.Empty)
            {
                SetCsvValue(ddlColumnlist, dr["FLDDISPLAYCOLUMNS"].ToString());
            }


            if (dr["FLDCATEGORIESOFCOURSES"].ToString() != string.Empty || dr["FLDCATEGORIESOFLICENCE"].ToString() != string.Empty
                || dr["FLDCATEGORIESOFMEDICALTEST"].ToString() != string.Empty || dr["FLDCATEGORIESOFOHTERDOC"].ToString() != string.Empty)
            {
                List<string> str = new List<string>();
                foreach (string s in dr["FLDCATEGORIESOFCOURSES"].ToString().Trim(',').Split(','))
                    str.Add(s);
                foreach (string s in dr["FLDCATEGORIESOFLICENCE"].ToString().Trim(',').Split(','))
                    str.Add(s);
                foreach (string s in dr["FLDCATEGORIESOFMEDICALTEST"].ToString().Trim(',').Split(','))
                    str.Add(s);
                foreach (string s in dr["FLDCATEGORIESOFOHTERDOC"].ToString().Trim(',').Split(','))
                    str.Add(s);
                ViewState["DOCUMENT_CHECKED_ITEMS"] = str;
            }
            CheckSelectedAllDocument();
        }
        else
        {
            txtSearchName.Text = "";
            ClearAll();
        }
        //  ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "resize", "resize();", true);
    }
    private void Clear()
    {
        txtFullName1.Text = "";
        txtFullName2.Text = "";
        txtPassportNo.Text = "";
        lstRank.selectedlist = "";
        lstZone.selectedlist = "";
        ddlVessel.SelectedVessel = "";
        ddlVesselType.SelectedVesseltype = "";
        txtAvailableFrom.Text = "";
        ddlNationality.SelectedNationality = "";
        ddlFlag.SelectedFlag = "";
        lstVesselType.SelectedVesseltype = "";
        lstRankList.selectedlist = "";
        ddlExperienceShip.SelectedVessel = "";
        txtExperienceFrom.Text = "";
        txtExperienceTo.Text = "";
        ddlEngineType.SelectedEngineName = "";
        ddlPlannedVessel.SelectedVessel = "";
        ddlPlannedVesselType.SelectedVesseltype = "";
        ucCurrentStage.SelectedHard = "";
        cblHasMissing.ClearChecked();
        cblHasMissing.ClearSelection();
        cblHasMissing.SelectedIndex = -1;
    }
    private void ClearAll()
    {
        txtFullName1.Text = "";
        txtFullName2.Text = "";
        txtSearchName.Text = "";
        ddlSavedSearch.SelectedValue = "";
        txtPassportNo.Text = "";
        lstRank.selectedlist = "";
        lstZone.selectedlist = "";
        ucCurrentStatus.SelectedHard = "";
        ucCurrentStage.SelectedHard = "";
        ddlVessel.SelectedVessel = "";
        ddlVesselType.SelectedVesseltype = "";
        ucTechFleet.SelectedFleet = "";
        ddlPlannedShipOwnedBy.SelectedAddress = "";
        ddlPlannedShipManagedBy.SelectedAddress = "";
        ddlPlannedVessel.SelectedVessel = "";
        ddlPlannedVesselType.SelectedVesseltype = "";
        txtFileNo.Text = "";
        txtAvailableFrom.Text = "";
        ddlNationality.SelectedNationality = "";
        ddlFlag.SelectedFlag = "";
        lstVesselType.SelectedVesseltype = "";
        lstRankList.selectedlist = "";
        ddlExperienceShip.SelectedVessel = "";
        txtExperienceFrom.Text = "";
        txtExperienceTo.Text = "";
        ddlEngineType.SelectedEngineName = "";
        ddlRank.SelectedRank = "";
        ddlSignedOff.SelectedVessel = "";
        ddlSignedOffShipOwnedBy.SelectedAddress = "";
        ucSignOffTechGroup.SelectedFleet = "";
        ddlSignOffReason.SelectedSignOffReason = "";
        ddlSignedOffMonths.SelectedValue = "";
        ddlCourse.SelectedCourse = "";
        ucInstitution.SelectedAddress = "";
        ddlLicence.SelectedDocument = "";
        ddlFlagcoc.SelectedCountry = "";
        txtNewapplicantID.Text = "";
        txtAddress.Text = "";
        txtPreviousShip.Text = "";
        txtOwnerName.Text = "";
        ucCountryVisaheld.SelectedCountry = "";
        ddleocnod.SelectedValue = "";
        ucExpiryDocument.SelectedDocumentType = "";
        txtMinimumAge.Text = "";
        txtMaximumAge.Text = "";
        ddlTrainingNeeds.SelectedCourse = "";
        ddlMedicalTest.SelectedDocument = "";
        txtMedicalTestFrom.Text = "";
        txtMedicalTestTo.Text = "";
        chkHasEmailAddress.Checked = false;
        chkGeneralRemark.Checked = false;
        chkIsNotActive.Checked = false;
        txtNotActiveFrom.Text = "";
        ddlInactiveReason.SelectedHard = "";
        chkIsNTBE.Checked = false;
        txtShowRecord.Text = "";
        ddlOrderby1.SelectedValue = "";
        ddlOrderby2.SelectedValue = "";
        ViewState["DOCUMENT_CHECKED_ITEMS"] = null;
        ddlExpiryCOC.SelectedDocument = "";
        ddlExpiryCourse.SelectedCourse = "";
        chkTrainingNeeds.Checked = false;
        txtPhoneNumber.Text = "";
        txtMobileNumber.Text = "";
        lstZone.selectedlist = General.GetNullableString("-1");
      
        cblHasMissing.ClearChecked();
        cblHasMissing.ClearSelection();
        cblHasMissing.SelectedIndex = -1;
        gvDocument.Rebind();
    }
    protected DataSet CombineDataSet(DataSet ds1, DataTable dt)
    {
        ds1.Tables[0].Merge(dt);
        return ds1;
    }
    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            string strLicence = "";
            string strCourse = "";
            string strMedical = "";
            string strOtherdoc = "";
            if (ViewState["DOCUMENT_CHECKED_ITEMS"] != null)
            {
                List<string> SelectedDocument = new List<string>();
                SelectedDocument = (List<string>)ViewState["DOCUMENT_CHECKED_ITEMS"];
                if (SelectedDocument != null && SelectedDocument.Count > 0)
                {
                    List<string> licencelist = new List<string>();
                    List<string> courselist = new List<string>();
                    List<string> medicallist = new List<string>();
                    List<string> otherdoclist = new List<string>();
                    licencelist = SelectedDocument.FindAll(f => f.EndsWith("~1"));
                    courselist = SelectedDocument.FindAll(f => f.EndsWith("~2"));
                    medicallist = SelectedDocument.FindAll(f => f.EndsWith("~3"));
                    otherdoclist = SelectedDocument.FindAll(f => f.EndsWith("~4"));
                    strLicence = string.Join(",", licencelist.ToArray());
                    strCourse = string.Join(",", courselist.ToArray());
                    strMedical = string.Join(",", medicallist.ToArray());
                    strOtherdoc = string.Join(",", otherdoclist.ToArray());
                }
            }

            if (strLicence != "")
                strLicence = "," + strLicence.Replace("~1", "") + ",";
            if (strCourse != "")
                strCourse = "," + strCourse.Replace("~2", "") + ",";
            if (strMedical != "")
                strMedical = "," + strMedical.Replace("~3", "") + ",";
            if (strOtherdoc != "")
                strOtherdoc = "," + strOtherdoc.Replace("~4", "") + ",";

            string csvRank = lstRank.selectedlist.Replace("Dummy", "").Trim(',');
            string csvZone = lstZone.selectedlist.Replace("Dummy", "").Trim(',');
            string csvcolumnlist = GetCsvValue(ddlColumnlist);


            if (txtSearchName.Text.Trim().Equals(""))
            {
                PhoenixCrewOffshoreEmployee.CrewOffshoreEmployeeSearchInsert(General.GetNullableInteger(ddlSavedSearch.SelectedValue), txtSearchName.Text, txtFullName1.Text
                , txtFullName2.Text, txtPassportNo.Text, lstRank.selectedlist.Replace("Dummy", "").Trim(','), lstZone.selectedlist.Replace("Dummy", "").Trim(',')
                , General.GetNullableInteger(ddlVessel.SelectedVessel), General.GetNullableInteger(ddlVesselType.SelectedVesseltype)
                , General.GetNullableDateTime(txtAvailableFrom.Text), General.GetNullableInteger(ddlNationality.SelectedNationality)
                , General.GetNullableInteger(ddlFlag.SelectedFlag), lstVesselType.SelectedVesseltype.Replace("Dummy", "").Trim(',')
                , lstRankList.selectedlist.Replace("Dummy", "").Trim(','), General.GetNullableInteger(ddlExperienceShip.SelectedVessel)
                , General.GetNullableDateTime(txtExperienceFrom.Text), General.GetNullableDateTime(txtExperienceTo.Text)
                , General.GetNullableInteger(ddlEngineType.SelectedEngineName), General.GetNullableInteger(ddlSignedOff.SelectedVessel)
                , General.GetNullableInteger(ddlSignedOffShipOwnedBy.SelectedAddress), General.GetNullableInteger(ddlSignOffReason.SelectedSignOffReason)
                , General.GetNullableInteger(ddlSignedOffMonths.SelectedValue)
                , General.GetNullableInteger(ddlCourse.SelectedCourse), General.GetNullableInteger(ucInstitution.SelectedAddress), General.GetNullableInteger(ddlLicence.SelectedDocument), General.GetNullableInteger(ddlFlagcoc.SelectedCountry)
                , General.GetNullableString(txtNewapplicantID.Text), General.GetNullableString(txtAddress.Text), General.GetNullableString(txtPreviousShip.Text), General.GetNullableString(txtOwnerName.Text)
                , General.GetNullableInteger(ucCountryVisaheld.SelectedCountry), General.GetNullableInteger(ddleocnod.Text), General.GetNullableString(ucExpiryDocument.SelectedDocumentType)
                , General.GetNullableInteger(txtMinimumAge.Text), General.GetNullableInteger(txtMaximumAge.Text), General.GetNullableInteger(ddlTrainingNeeds.SelectedCourse), General.GetNullableInteger(ddlMedicalTest.SelectedDocument), General.GetNullableDateTime(txtMedicalTestFrom.Text)
                , General.GetNullableDateTime(txtMedicalTestTo.Text), General.GetNullableInteger(chkHasEmailAddress.Checked == true ? "1" : ""), General.GetNullableInteger(chkGeneralRemark.Checked == true ? "1" : ""), General.GetNullableInteger(chkIsNotActive.Checked == true ? "1" : ""), General.GetNullableDateTime(txtNotActiveFrom.Text)
                , General.GetNullableInteger(ddlInactiveReason.SelectedHard), General.GetNullableInteger(chkIsNTBE.Checked == true ? "1" : ""), General.GetNullableInteger(txtShowRecord.Text), General.GetNullableString(ddlOrderby1.SelectedValue), General.GetNullableString(ddlOrderby2.SelectedValue)
                , General.GetNullableString(strCourse), General.GetNullableInteger(ucTechFleet.SelectedFleet), General.GetNullableInteger(ucSignOffTechGroup.SelectedFleet)
                , General.GetNullableInteger(ddlPlannedVessel.SelectedVessel), General.GetNullableInteger(ddlPlannedVesselType.SelectedVesseltype)
                , General.GetNullableInteger(ddlPlannedShipOwnedBy.SelectedAddress), General.GetNullableInteger(ddlPlannedShipManagedBy.SelectedAddress)
                , General.GetNullableInteger(ucCurrentStatus.SelectedHard), General.GetNullableInteger(ddlRank.SelectedRank), General.GetNullableString(txtFileNo.Text)
                , General.GetNullableInteger(ucCurrentStage.SelectedHard), General.GetNullableInteger(ddlExpiryCourse.SelectedCourse), (byte?)General.GetNullableInteger((chkTrainingNeeds.Checked.Value ? "1" : ""))
                , General.GetNullableInteger(ddlExpiryCOC.SelectedDocument), (byte?)General.GetNullableInteger(cblHasMissing.SelectedValue)
                , General.GetNullableString(txtPhoneNumber.Text), General.GetNullableString(txtMobileNumber.Text)
                , General.GetNullableString(strLicence), General.GetNullableString(strMedical), General.GetNullableString(strOtherdoc),General.GetNullableString(csvcolumnlist));
            }
            Response.Redirect("CrewOffshoreEmployee.aspx?sid=" + (ddlSavedSearch.SelectedValue == string.Empty ? "0" : ddlSavedSearch.SelectedValue) + "&pl=6&launchedfrom=offshore", true);
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ClearAll();
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "resize", "resize();", true);
        }
    }
    protected void CrewQueryGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVESEARCH"))
        {
            if (!IsValidSearch())
            {
                ucError.Visible = true;
                return;
            }
            string strLicence = "";
            string strCourse = "";
            string strMedical = "";
            string strOtherdoc = "";

            if (ViewState["DOCUMENT_CHECKED_ITEMS"] != null)
            {
                List<string> SelectedDocument = new List<string>();
                SelectedDocument = (List<string>)ViewState["DOCUMENT_CHECKED_ITEMS"];
                if (SelectedDocument != null && SelectedDocument.Count > 0)
                {
                    List<string> licencelist = new List<string>();
                    List<string> courselist = new List<string>();
                    List<string> medicallist = new List<string>();
                    List<string> otherdoclist = new List<string>();
                    licencelist = SelectedDocument.FindAll(f => f.EndsWith("~1"));
                    courselist = SelectedDocument.FindAll(f => f.EndsWith("~2"));
                    medicallist = SelectedDocument.FindAll(f => f.EndsWith("~3"));
                    otherdoclist = SelectedDocument.FindAll(f => f.EndsWith("~4"));
                    strLicence = string.Join(",", licencelist.ToArray());
                    strCourse = string.Join(",", courselist.ToArray());
                    strMedical = string.Join(",", medicallist.ToArray());
                    strOtherdoc = string.Join(",", otherdoclist.ToArray());
                }
            }

            if (strLicence != "")
                strLicence = "," + strLicence.Replace("~1", "") + ",";
            if (strCourse != "")
                strCourse = "," + strCourse.Replace("~2", "") + ",";
            if (strMedical != "")
                strMedical = "," + strMedical.Replace("~3", "") + ",";
            if (strOtherdoc != "")
                strOtherdoc = "," + strOtherdoc.Replace("~4", "") + ",";

            string csvRank = lstRank.selectedlist.Replace("Dummy", "").Trim(',');
            string csvZone = lstZone.selectedlist.Replace("Dummy", "").Trim(',');
            string csvcolumnlist = GetCsvValue(ddlColumnlist);


            PhoenixCrewOffshoreEmployee.CrewOffshoreEmployeeSearchInsert(General.GetNullableInteger(ddlSavedSearch.SelectedValue), txtSearchName.Text, txtFullName1.Text
                , txtFullName2.Text, txtPassportNo.Text, lstRank.selectedlist.Replace("Dummy", "").Trim(','), lstZone.selectedlist.Replace("Dummy", "").Trim(',')
                , General.GetNullableInteger(ddlVessel.SelectedVessel), General.GetNullableInteger(ddlVesselType.SelectedVesseltype)
                , General.GetNullableDateTime(txtAvailableFrom.Text), General.GetNullableInteger(ddlNationality.SelectedNationality)
                , General.GetNullableInteger(ddlFlag.SelectedFlag), lstVesselType.SelectedVesseltype.Replace("Dummy", "").Trim(',')
                , lstRankList.selectedlist.Replace("Dummy", "").Trim(','), General.GetNullableInteger(ddlExperienceShip.SelectedVessel)
                , General.GetNullableDateTime(txtExperienceFrom.Text), General.GetNullableDateTime(txtExperienceTo.Text)
                , General.GetNullableInteger(ddlEngineType.SelectedEngineName), General.GetNullableInteger(ddlSignedOff.SelectedVessel)
                , General.GetNullableInteger(ddlSignedOffShipOwnedBy.SelectedAddress), General.GetNullableInteger(ddlSignOffReason.SelectedSignOffReason)
                , General.GetNullableInteger(ddlSignedOffMonths.SelectedValue)
                , General.GetNullableInteger(ddlCourse.SelectedCourse), General.GetNullableInteger(ucInstitution.SelectedAddress), General.GetNullableInteger(ddlLicence.SelectedDocument), General.GetNullableInteger(ddlFlagcoc.SelectedCountry)
                , General.GetNullableString(txtNewapplicantID.Text), General.GetNullableString(txtAddress.Text), General.GetNullableString(txtPreviousShip.Text), General.GetNullableString(txtOwnerName.Text)
                , General.GetNullableInteger(ucCountryVisaheld.SelectedCountry), General.GetNullableInteger(ddleocnod.Text), General.GetNullableString(ucExpiryDocument.SelectedDocumentType)
                , General.GetNullableInteger(txtMinimumAge.Text), General.GetNullableInteger(txtMaximumAge.Text), General.GetNullableInteger(ddlTrainingNeeds.SelectedCourse), General.GetNullableInteger(ddlMedicalTest.SelectedDocument), General.GetNullableDateTime(txtMedicalTestFrom.Text)
                , General.GetNullableDateTime(txtMedicalTestTo.Text), General.GetNullableInteger(chkHasEmailAddress.Checked == true ? "1" : ""), General.GetNullableInteger(chkGeneralRemark.Checked == true ? "1" : ""), General.GetNullableInteger(chkIsNotActive.Checked == true ? "1" : ""), General.GetNullableDateTime(txtNotActiveFrom.Text)
                , General.GetNullableInteger(ddlInactiveReason.SelectedHard), General.GetNullableInteger(chkIsNTBE.Checked == true ? "1" : ""), General.GetNullableInteger(txtShowRecord.Text), General.GetNullableString(ddlOrderby1.SelectedValue), General.GetNullableString(ddlOrderby2.SelectedValue)
                , General.GetNullableString(strCourse), General.GetNullableInteger(ucTechFleet.SelectedFleet), General.GetNullableInteger(ucSignOffTechGroup.SelectedFleet)
                , General.GetNullableInteger(ddlPlannedVessel.SelectedVessel), General.GetNullableInteger(ddlPlannedVesselType.SelectedVesseltype)
                , General.GetNullableInteger(ddlPlannedShipOwnedBy.SelectedAddress), General.GetNullableInteger(ddlPlannedShipManagedBy.SelectedAddress)
                , General.GetNullableInteger(ucCurrentStatus.SelectedHard), General.GetNullableInteger(ddlRank.SelectedRank), General.GetNullableString(txtFileNo.Text)
                , General.GetNullableInteger(ucCurrentStage.SelectedHard), General.GetNullableInteger(ddlExpiryCourse.SelectedCourse), (byte?)General.GetNullableInteger((chkTrainingNeeds.Checked.Value ? "1" : ""))
                , General.GetNullableInteger(ddlExpiryCOC.SelectedDocument), (byte?)General.GetNullableInteger(cblHasMissing.SelectedValue)
                , General.GetNullableString(txtPhoneNumber.Text), General.GetNullableString(txtMobileNumber.Text)
                , General.GetNullableString(strLicence), General.GetNullableString(strMedical), General.GetNullableString(strOtherdoc), General.GetNullableString(csvcolumnlist));
            BindSavedSearch();
        }
    }

    private string GetCsvValue(RadComboBox radComboBox)
    {
        var list = radComboBox.CheckedItems;
        string csv = string.Empty;
        if (list.Count != 0)
        {
            csv = ",";
            foreach (var item in list)
            {
                csv = csv + item.Value + ",";
            }
        }
        return csv;
    }
    private void SetCsvValue(RadComboBox radComboBox, string csvValue)
    {
        foreach (RadComboBoxItem item in radComboBox.Items)
        {
            item.Checked = false;
            if (csvValue.Contains("," + item.Value + ","))
            {
                item.Checked = true;
            }
        }
    }


    public bool IsValidSearch()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (txtSearchName.Text.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Search Name is required.";
        }
        return (!ucError.IsError);
    }
    public bool IsValidDelete()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableInteger(ddlSavedSearch.SelectedValue).HasValue)
        {
            ucError.ErrorMessage = "Select the Saved Search that you want to delete.";
        }
        return (!ucError.IsError);
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewOffshoreEmployee.OffshoreDocumentSearch((int)ViewState["PAGENUMBER"],
                                                                        gvDocument.PageSize, ref iRowCount, ref iTotalPageCount
                                                                        );

        gvDocument.DataSource = ds;
        gvDocument.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();

    }


    protected void SaveCheckedDocumentValues(Object sender, EventArgs e)
    {
        RadCheckBox chk = (RadCheckBox)sender;
        GridDataItem gr = ((GridDataItem)chk.Parent.Parent);
        List<string> SelectedDocument = new List<string>();
        string documentid;
        string doctype;
        string content;

        bool result = false;
        documentid = gr.GetDataKeyValue("FLDDOCUMENTID").ToString();// gvDocument.MasterTableView.datak DataKeys[gr.RowIndex].Value.ToString();
        doctype = ((RadLabel)gr.FindControl("lblDocumentType")).Text;
        content = documentid + "~" + doctype;

        if (chk.Checked == true)
        {
            result = true;// ((CheckBox)gvrow.FindControl("chkDocumentSelect")).Checked;
        }

        // Check in the ViewState
        if (ViewState["DOCUMENT_CHECKED_ITEMS"] != null)
            SelectedDocument = (List<string>)ViewState["DOCUMENT_CHECKED_ITEMS"];
        if (result)
        {
            if (!SelectedDocument.Contains(content))
                SelectedDocument.Add(content);
        }
        else
            SelectedDocument.Remove(content);

        if (SelectedDocument != null && SelectedDocument.Count > 0)
            ViewState["DOCUMENT_CHECKED_ITEMS"] = SelectedDocument;
    }

    private void CheckSelectedAllDocument()
    {
        if (ViewState["DOCUMENT_CHECKED_ITEMS"] != null)
        {
            string strdocumentid = "";
            string documentid;
            string doctype;
            string content;
            List<string> SelectedDocument = new List<string>();
            SelectedDocument = (List<string>)ViewState["DOCUMENT_CHECKED_ITEMS"];
            strdocumentid = "," + string.Join(",", SelectedDocument.ToArray()) + ",";
            for (int i = 0; i < gvDocument.Items.Count; i++)
            {
                if (SelectedDocument != null && SelectedDocument.Count > 0)
                {
                    documentid = gvDocument.MasterTableView.Items[i].GetDataKeyValue("FLDDOCUMENTID").ToString();
                    doctype = ((RadLabel)gvDocument.Items[i].FindControl("lblDocumentType")).Text;
                    content = documentid + "~" + doctype;
                    if (strdocumentid.Contains("," + content + ","))
                    {
                        RadCheckBox cbSelected = (RadCheckBox)gvDocument.Items[i].FindControl("chkDocumentSelect");
                        if (cbSelected != null)
                        {
                            cbSelected.Checked = true;
                        }
                    }
                }
            }
        }
    }
    protected void btnSearchDelete_Click(object sender, EventArgs e)
    {
        if (!IsValidDelete())
        {
            ucError.Visible = true;
            return;
        }
        PhoenixCrewOffshoreEmployee.DeleteCrewOffshoreEmployeeSearch(General.GetNullableInteger(ddlSavedSearch.SelectedValue).Value);
        BindSavedSearch();
        ClearAll();
    }

    protected void gvDocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocument.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDocument_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            string strdocumentid = "";
            string documentid;
            string doctype;
            string content;
            List<string> SelectedDocument = new List<string>();
            SelectedDocument = (List<string>)ViewState["DOCUMENT_CHECKED_ITEMS"];
            if (SelectedDocument != null && SelectedDocument.Count > 0)
            {
                strdocumentid = "," + string.Join(",", SelectedDocument.ToArray()) + ",";
                documentid = gvDocument.MasterTableView.DataKeyValues[e.Item.ItemIndex]["FLDDOCUMENTID"].ToString();
                doctype = ((RadLabel)e.Item.FindControl("lblDocumentType")).Text;
                content = documentid + "~" + doctype;
                if (strdocumentid.Contains("," + content + ","))
                {
                    RadCheckBox cbSelected = (RadCheckBox)e.Item.FindControl("chkDocumentSelect");
                    if (cbSelected != null)
                    {
                        cbSelected.Checked = true;
                    }
                }
            }
        }
    }

    protected void gvDocument_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
}
