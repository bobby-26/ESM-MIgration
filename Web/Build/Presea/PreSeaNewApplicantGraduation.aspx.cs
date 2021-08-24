using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Registers;


public partial class PreSeaNewApplicantGraduation : PhoenixBasePage
{
    string NullableString = General.GetNullableString("");
    #region :   Page Events    :

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["INTERVIEWIDYN"] = "0";            

            PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;
            if (Request.QueryString["ACADEMICID"] != null)
                ViewState["EMPLOYEEACADEMICID"] = Request.QueryString["ACADEMICID"];
            if (Request.QueryString["ACADEMICTYPE"] != null)
                ViewState["ACADEMICTYPE"] = Request.QueryString["ACADEMICTYPE"];

            ddlQualification.DataSource = PhoenixRegistersDocumentQualification.ListQualifications(",BSC,BE/B.Tech,");
            ddlQualification.DataBind();

            ddlQualification.Enabled = true;

            ucState.Country = "97";
            LoadInstitues();

            BindYearFrom();
            BindYearTo();
            BindYearPassYear();
            ShowGraduationDetails();
            CheckInterviewedYN();

            ddlQualification.Focus();  

        }

        BindMarks();
    }

    #endregion

    #region :   Methods    :

    string academictype = string.Empty;
    string academicid = string.Empty;

    protected void CheckInterviewedYN()
    {
        if (!String.IsNullOrEmpty(Filter.CurrentPreSeaNewApplicantSelection))
        {
            int interviewedyn;
            interviewedyn = 0;
            PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantInterviewedCheckYN(General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection), ref interviewedyn);

            if (interviewedyn == 1)
            {
                ViewState["INTERVIEWIDYN"] = "1";
                HideSaveButtons();
            }
            else
            {
                ViewState["INTERVIEWIDYN"] = "0";
            }
        }
        else
        {
            ViewState["INTERVIEWIDYN"] = "0";
        }
    }

    protected void HideSaveButtons()
    {
        btnBasicSave.Visible = false;
    }

    protected void LoadInstitues()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        ucInstitution.DataSource = PhoenixPreSeaAddress.PreSeaAddressSearch(NullableString
                                                                            , NullableString
                                                                            , 97
                                                                            , null
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , null
                                                                            , null
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , null
                                                                            , 1
                                                                            , int.MaxValue
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount
                                                                            , null);
        ucInstitution.DataBind();
    }

    protected void LoadAcademicPanel()
    {
        BindYearFrom();
        BindYearTo();
        BindYearPassYear();
        if (ViewState["EMPLOYEEACADEMICID"] != null)
            EditPreSeaAcademic(ViewState["EMPLOYEEACADEMICID"].ToString());
    }

    protected void BindYearFrom()
    {
        ListItem li = new ListItem("-select-", "Dummy");
        ddlYearFrom.Items.Add(li);
        for (int i = (DateTime.Today.Year - 33); i <= (DateTime.Today.Year); i++)
        {
            li = new ListItem(i.ToString(), i.ToString());
            ddlYearFrom.Items.Add(li);
        }
        ddlYearFrom.DataBind();
    }

    protected void BindYearTo()
    {
        ListItem li = new ListItem("-select-", "Dummy");
        ddlYearTo.Items.Add(li);
        for (int i = (DateTime.Today.Year - 33); i <= (DateTime.Today.Year); i++)
        {
            li = new ListItem(i.ToString(), i.ToString());
            ddlYearTo.Items.Add(li);
        }
        ddlYearTo.DataBind();
    }

    protected void BindYearPassYear()
    {
        ListItem li = new ListItem("-select-", "Dummy");
        ddlYearPass.Items.Add(li);
        for (int i = (DateTime.Today.Year - 33); i <= (DateTime.Today.Year); i++)
        {
            li = new ListItem(i.ToString(), i.ToString());
            ddlYearPass.Items.Add(li);
        }
        ddlYearPass.DataBind();
    }

    protected void ShowGraduationDetails()
    {
        try
        {
            string qualify = "";
            DataTable dt = PhoenixPreSeaNewApplicantAcademicQualification.ListApplicantQualifications(int.Parse(Filter.CurrentPreSeaNewApplicantSelection), ",BSC,BE/B.Tech,");
            if (dt.Rows.Count > 0)
            {
                qualify = dt.Rows[0]["FLDQUALIFICATIONID"].ToString();
                ddlQualification.SelectedValue = dt.Rows[0]["FLDQUALIFICATIONID"].ToString();
            }
            if (General.GetNullableInteger(qualify).HasValue)
            {
                DataTable dt2 = PhoenixPreSeaNewApplicantAcademicQualification.ListCandidateQualification(int.Parse(Filter.CurrentPreSeaNewApplicantSelection)
                                                                                                        , General.GetNullableInteger(qualify));
                if (dt2.Rows.Count > 0)
                {
                    academicid = dt2.Rows[0]["FLDACADEMICID"].ToString();
                    academictype = dt2.Rows[0]["FLDACADEMICTYPE"].ToString();
                    ViewState["EMPLOYEEACADEMICID"] = dt2.Rows[0]["FLDACADEMICID"].ToString();
                    ViewState["ACADEMICTYPE"] = dt2.Rows[0]["FLDACADEMICTYPE"].ToString();
                    EditPreSeaAcademic(academicid);
                }
                else
                {
                    academicid = "";
                    academictype = "";
                    ViewState["EMPLOYEEACADEMICID"] = null;
                    ResetAcademicDeatils();
                }
            }
            else
            {
                academicid = "";
                academictype = "";
                ViewState["EMPLOYEEACADEMICID"] = null;
                ResetAcademicDeatils();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditPreSeaAcademic(string id)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = PhoenixPreSeaNewApplicantAcademicQualification.EditPreSeaNewApplicantAcademic(Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection), General.GetNullableInteger(id));
            if (dt.Rows.Count > 0)
            {
                ddlQualification.SelectedValue = dt.Rows[0]["FLDCERTIFICATE"].ToString();

                ucInstitution.SelectedValue = dt.Rows[0]["FLDINSTITUTIONID"].ToString();
                txtUniversity.Text = dt.Rows[0]["FLDUNIVERSITY"].ToString();
                txtExamRollno.Text = dt.Rows[0]["FLDROLLNUMBER"].ToString();
                ddlYearFrom.SelectedValue = dt.Rows[0]["FLDYEARSFROM"].ToString();
                ddlYearTo.SelectedValue = dt.Rows[0]["FLDYEARSTO"].ToString();
                ddlYearPass.SelectedValue = dt.Rows[0]["FLDYEAROFPASSING"].ToString();
                if (General.GetNullableInteger(dt.Rows[0]["FLDFIRSTATTEMPTYN"].ToString()) != null)
                    ddlFirstAttemptYN.SelectedValue = dt.Rows[0]["FLDFIRSTATTEMPTYN"].ToString();
                chkResultYN.Checked = (dt.Rows[0]["FLDRESULTYN"].ToString() == "1" ? true : false);
                ucAddress.Address1 = dt.Rows[0]["FLDADDRESS1"].ToString();
                ucAddress.Address2 = dt.Rows[0]["FLDADDRESS2"].ToString();
                ucAddress.Address3 = dt.Rows[0]["FLDADDRESS3"].ToString();
                ucAddress.Address4 = dt.Rows[0]["FLDADDRESS4"].ToString();
                ucAddress.Country = dt.Rows[0]["FLDCOUNTRYID"].ToString();
                ucAddress.State = dt.Rows[0]["FLDSTATEID"].ToString();
                ucAddress.City = dt.Rows[0]["FLDPLACEOFINSTITUTION"].ToString();
                ucAddress.PostalCode = dt.Rows[0]["FLDPOSTALCODE"].ToString();
                txtPercentage.Text = dt.Rows[0]["FLDPERCENTAGE"].ToString();
                txtTotal.Text = dt.Rows[0]["FLDTOTAL"].ToString();
                ucState.SelectedState = dt.Rows[0]["FLDSTATEID"].ToString();
                ddlPlace.SelectedCity = dt.Rows[0]["FLDPLACEOFINSTITUTION"].ToString();
                txtInstituteName.Text = dt.Rows[0]["FLDINSTITUTION"].ToString();
            }
            BindMarks();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    private void ResetAcademicDeatils()
    {
        ucInstitution.SelectedValue = "Dummy";
        txtUniversity.Text = "";
        txtExamRollno.Text = "";
        ddlYearFrom.SelectedValue = "Dummy";
        ddlYearTo.SelectedValue = "Dummy";
        ddlYearPass.SelectedValue = "Dummy";
        ddlFirstAttemptYN.SelectedValue = "Dummy";
        chkResultYN.Checked = false;
        ucAddress.Address1 = "";
        ucAddress.Address2 = "";
        ucAddress.Address3 = "";
        ucAddress.Address4 = "";
        ucAddress.Country = "";
        ucAddress.State = "";
        ucAddress.City = "";
        ucAddress.PostalCode = "";
        txtPercentage.Text = "";
        txtTotal.Text = "";
    }

    private void BindMarks()
    {
        int employeeid = int.Parse(Filter.CurrentPreSeaNewApplicantSelection);
        try
        {            
            academictype = ddlQualification.SelectedValue;
            if (ViewState["EMPLOYEEACADEMICID"] != null)
                academicid = ViewState["EMPLOYEEACADEMICID"].ToString();
            DataTable dt = new DataTable();
            dt = PhoenixPreSeaNewApplicantAcademicQualification.ListPreSeaNewApplicantAcademicMark(employeeid, General.GetNullableInteger(academicid), General.GetNullableInteger(academictype));

            if (dt.Rows.Count > 0)
            {
                gvMarks.DataSource = dt;
                gvMarks.DataBind();

            }
            else
            {
                ShowNoRecordsFound(dt, gvMarks);
            }
            rblMarksGrade_Changed(null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidMarks(string subject, string marks, string outoff, string grade)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        decimal resultDec, resultout;

        if (subject == null || subject == "")
            ucError.ErrorMessage = "Subject/Sem is required.";

        if (!decimal.TryParse(marks, out resultDec) && General.GetNullableInteger(grade) == null)
            ucError.ErrorMessage = "Either Mark obtain or Grade is required";

        if (decimal.TryParse(marks, out resultDec))
        {
            if (!decimal.TryParse(outoff, out resultout))
                ucError.ErrorMessage = "Max marks is required";
        }

        return (!ucError.IsError);
    }

    private bool IsValidAcademic(string qualification
              , string board
              , string institution
              , string yearfrom
              , string yearto
              , string yearpass
              , string attempt
              , string address1
              , string country
              , string city
              , string marksgrade
        )
    {
        ucError.HeaderMessage = "Please provide the following required information";
        Int32 result;

        if (!General.GetNullableInteger(qualification).HasValue)
            ucError.ErrorMessage = "Qualification is Required.";

        if (board == "" || board.ToUpper().Equals("DUMMY"))
        {
            ucError.ErrorMessage = "Board/University is required";
        }
        if (institution == "" || institution == "Dummy")
        {
            ucError.ErrorMessage = "Institution required";

        }
        if (yearfrom == "Dummy")
            ucError.ErrorMessage = "Year From required";
        if (yearto == "Dummy")
            ucError.ErrorMessage = "Year To required";
        if (yearpass == "Dummy")
            ucError.ErrorMessage = "Year Pass required";
        if (int.TryParse(country, out result) == false)
            ucError.ErrorMessage = "Country required";
        if (int.TryParse(city, out result) == false)
            ucError.ErrorMessage = "City required";
        if (yearfrom != "Dummy" && yearto != "Dummy")
        {
            int fromyear = int.Parse(yearfrom);
            int toyear = int.Parse(yearto);
            if (fromyear > toyear)
                ucError.ErrorMessage = "To year should be later than or equal to fromyear";
        }
        if (yearto != "Dummy" && yearpass != "Dummy")
        {
            int toyear = int.Parse(yearto);
            int passyear = int.Parse(yearpass);
            if (toyear > passyear)
                ucError.ErrorMessage = "Passed year should be later than or equal to To year";
        }
        if (General.GetNullableString(ucAddress.Address1) == null)
        {
            ucError.ErrorMessage = "Instution Address1 required";
        }
        if (General.GetNullableInteger(ucAddress.Country) == null)
        {
            ucError.ErrorMessage = "Instution Country required";
        }
        if (General.GetNullableInteger(marksgrade) == null)
        {
            ucError.ErrorMessage = "Marks or Garde required";
        }
        else
        {
            GridView _gridView = gvMarks;
            if (_gridView.Rows.Count > 0)
            {

                foreach (GridViewRow gvr in gvMarks.Rows)
                {
                    Label lblMarksId = (Label)gvr.FindControl("lblMarksId");
                    string marksoutoff = ((UserControlMaskNumber)gvr.FindControl("txtOutOffEdit")).Text;
                    string marksobtain = ((UserControlMaskNumber)gvr.FindControl("txtMarksEdit")).Text;
                    string subjectname = ((Label)gvr.FindControl("lblSubjectName")).Text;
                    string grade = ((DropDownList)gvr.FindControl("ddlGradeEdit")).SelectedValue;
                    string gradepoint = ((UserControlMaskNumber)gvr.FindControl("txtGradePointEdit")).Text;

                    if (marksgrade == "1")
                    {
                        if (General.GetNullableDecimal(marksobtain) == null)
                        {
                            ucError.ErrorMessage = subjectname + " Marks is required ";
                        }
                        if (General.GetNullableDecimal(marksoutoff) == null)
                        {
                            ucError.ErrorMessage = subjectname + " Max Marks is required ";
                        }
                        if (General.GetNullableDecimal(marksoutoff) != null && General.GetNullableDecimal(marksobtain) != null)
                        {
                            if (General.GetNullableDecimal(marksobtain) > General.GetNullableDecimal(marksoutoff))
                                ucError.ErrorMessage = subjectname + " marks is should not be greater than max marks";
                        }
                    }
                    if (marksgrade == "2")
                    {
                        if (General.GetNullableInteger(grade) == null)
                        {
                            ucError.ErrorMessage = subjectname + " grade is required ";
                        }
                        else if (General.GetNullableDecimal(gradepoint) == null)
                        {
                            ucError.ErrorMessage = subjectname + " grade points is required ";
                        }
                    }
                }
            }
        }
        if (!ucError.IsError)
            tblErrMsg.Visible = false;
        else
            tblErrMsg.Visible = true;

        return (!ucError.IsError);

    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SaveAcademicDetails()
    {
        if (IsValidAcademic(
                      ddlQualification.SelectedValue
                    , txtUniversity.Text.Trim()
                    , ucInstitution.SelectedValue
                    , ddlYearFrom.SelectedValue
                    , ddlYearTo.SelectedValue
                    , ddlYearPass.SelectedValue
                    , ddlFirstAttemptYN.SelectedValue
                    , ucAddress.Address1
                    , ucAddress.Country
                    , ucAddress.City
                    , rblMarksGrade.SelectedValue
                    ))
        {
            if (ViewState["EMPLOYEEACADEMICID"] != null)
            {
                PhoenixPreSeaNewApplicantAcademicQualification.UpdatePreSeaNewApplicantAcademic(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                              , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                                                                              , General.GetNullableInteger(ViewState["EMPLOYEEACADEMICID"].ToString())
                                                                              , General.GetNullableInteger(ucInstitution.SelectedValue)
                                                                              , Convert.ToInt16(ddlQualification.SelectedValue)
                                                                              , General.GetNullableDecimal(txtPercentage.Text)
                                                                              , General.GetNullableString("")
                                                                              , ucAddress.Address1
                                                                              , ucAddress.Address2
                                                                              , ucAddress.Address3
                                                                              , ucAddress.Address4
                                                                              , ucAddress.PostalCode
                                                                              , General.GetNullableInteger(ucAddress.Country)
                                                                              , General.GetNullableInteger(ucAddress.State)
                                                                              , General.GetNullableInteger(ucAddress.City)
                                                                              , General.GetNullableInteger(ddlQualification.SelectedValue)
                                                                              , General.GetNullableInteger("")
                                                                              , General.GetNullableInteger(ddlFirstAttemptYN.SelectedValue)
                                                                              , General.GetNullableInteger(ddlYearFrom.SelectedValue)
                                                                              , General.GetNullableInteger(ddlYearTo.SelectedValue)
                                                                              , General.GetNullableInteger(ddlYearPass.SelectedValue)
                                                                              , General.GetNullableInteger(chkResultYN.Checked == true ? "1" : "0")
                                                                              , txtExamRollno.Text
                                                                              , txtUniversity.Text
                                                                              , General.GetNullableDecimal(txtTotal.Text)
                                                                              , ucInstitution.SelectedValue.Equals("-1") ? txtInstituteName.Text.Trim() : ucInstitution.SelectedItem.Text.Replace("--Select--", "")
                                                                              );
                UpdateGraduationMarks(int.Parse(ViewState["EMPLOYEEACADEMICID"].ToString()));
            }
            else
            {
                int? outacademicid = 0;


                PhoenixPreSeaNewApplicantAcademicQualification.InsertPreSeaNewApplicantAcademic(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                              , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                                              , General.GetNullableInteger(ucInstitution.SelectedValue)
                                              , Convert.ToInt16(ddlQualification.SelectedValue)
                                              , General.GetNullableDecimal(txtPercentage.Text)
                                              , General.GetNullableString("")
                                              , ucAddress.Address1
                                              , ucAddress.Address2
                                              , ucAddress.Address3
                                              , ucAddress.Address4
                                              , ucAddress.PostalCode
                                              , General.GetNullableInteger(ucAddress.Country)
                                              , General.GetNullableInteger(ucAddress.State)
                                              , General.GetNullableInteger(ucAddress.City)
                                              , General.GetNullableInteger(ddlQualification.SelectedValue)
                                              , General.GetNullableInteger("")
                                              , General.GetNullableInteger(ddlFirstAttemptYN.SelectedValue)
                                              , General.GetNullableInteger(ddlYearFrom.SelectedValue)
                                              , General.GetNullableInteger(ddlYearTo.SelectedValue)
                                              , General.GetNullableInteger(ddlYearPass.SelectedValue)
                                              , General.GetNullableInteger(chkResultYN.Checked == true ? "1" : "0")
                                              , txtExamRollno.Text
                                              , txtUniversity.Text
                                              , ucInstitution.SelectedValue.Equals("-1") ? txtInstituteName.Text.Trim() : ucInstitution.SelectedItem.Text.Replace("--Select--", "")
                                              , ref outacademicid
                                              );
                InsertGraduationMarks(outacademicid);

                PhoenixPreSeaNewApplicantAcademicQualification.UpdatePreSeaNewApplicantAcademicResult(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                    , outacademicid);
            }
            ShowGraduationDetails();
        }
        else
        {
            ucError.Visible = true;
            return;
        }
    }

    private void InsertGraduationMarks(int? academicid)
    {
        GridView _gridView = gvMarks;
        if (_gridView.Rows.Count > 0)
        {
            foreach (GridViewRow gvr in gvMarks.Rows)
            {
                Label lblSubjectId = (Label)gvr.FindControl("lblSubjectId");
                Label lblAcademicId = (Label)gvr.FindControl("lblAcademicId");
                string academictype = ddlQualification.SelectedValue;
                UserControlMaskNumber marksoutoff = (UserControlMaskNumber)gvr.FindControl("txtOutOffEdit");
                UserControlMaskNumber marksobtain = (UserControlMaskNumber)gvr.FindControl("txtMarksEdit");
                string grade = ((DropDownList)gvr.FindControl("ddlGradeEdit")).SelectedValue;
                string gradepoint = ((UserControlMaskNumber)gvr.FindControl("txtGradePointEdit")).Text;

                if (grade.Contains("--Sel"))
                    grade = "";

                PhoenixPreSeaNewApplicantAcademicQualification.InsertPreSeaNewApplicantAcademicGradeMark(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , int.Parse(Filter.CurrentPreSeaNewApplicantSelection)
                     , academicid
                     , General.GetNullableInteger(academictype)
                     , General.GetNullableInteger(lblSubjectId.Text)
                     , General.GetNullableDecimal(marksobtain.Text)
                     , General.GetNullableDecimal(marksoutoff.Text)
                     , null
                     , General.GetNullableString(grade)
                     , General.GetNullableInteger(gradepoint)
                     );
            }
        }
    }

    private void UpdateGraduationMarks(int academicid)
    {
        GridView _gridView = gvMarks;

        if (_gridView.Rows.Count > 0)
        {
            foreach (GridViewRow gvr in gvMarks.Rows)
            {

                Label lblMarksId = (Label)gvr.FindControl("lblMarksId");
                Label lblAcademicId = (Label)gvr.FindControl("lblAcademicId");
                Label lblSubjectId = (Label)gvr.FindControl("lblSubjectId");
                UserControlMaskNumber marksoutoff = (UserControlMaskNumber)gvr.FindControl("txtOutOffEdit");
                UserControlMaskNumber marksobtain = (UserControlMaskNumber)gvr.FindControl("txtMarksEdit");
                string grade = ((DropDownList)gvr.FindControl("ddlGradeEdit")).SelectedValue;
                string gradepoint = ((UserControlMaskNumber)gvr.FindControl("txtGradePointEdit")).Text;

                if (grade.Contains("--Sel"))
                    grade = "";

                PhoenixPreSeaNewApplicantAcademicQualification.UpdatePreSeaNewApplicantAcademicGradeMark(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                                         , int.Parse(Filter.CurrentPreSeaNewApplicantSelection)
                                                                                                         , int.Parse(lblAcademicId.Text)
                                                                                                         , General.GetNullableGuid(lblMarksId.Text)
                                                                                                         , General.GetNullableInteger(lblSubjectId.Text)
                                                                                                         , General.GetNullableDecimal(marksobtain.Text)
                                                                                                         , General.GetNullableDecimal(marksoutoff.Text)
                                                                                                         , null
                                                                                                         , General.GetNullableString(grade)
                                                                                                         , General.GetNullableDecimal(gradepoint));

            }
        }
    }       

    #endregion

    #region :   Events    :

    protected void ddlState_TextChanged(object sender, EventArgs e)
    {
        ddlPlace.State = ucState.SelectedState;
        ddlPlace.CityList = PhoenixRegistersCity.ListCity(
            General.GetNullableInteger("97")
            , General.GetNullableInteger(ucState.SelectedState) == null ? 0 : General.GetNullableInteger(ucState.SelectedState));

        int iRowCount = 0;
        int iTotalPageCount = 0;
        ucInstitution.DataSource = PhoenixPreSeaAddress.PreSeaAddressSearch(NullableString
                                                                            , NullableString
                                                                            , 97
                                                                            , General.GetNullableInteger(ucState.SelectedState)
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , null
                                                                            , null
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , null
                                                                            , 1
                                                                            , int.MaxValue
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount
                                                                            , General.GetNullableInteger(ddlPlace.SelectedCity));
        ucInstitution.DataBind();
    }

    protected void ddlPlace_TextChanged(object sender, EventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        ucInstitution.DataSource = PhoenixPreSeaAddress.PreSeaAddressSearch(NullableString
                                                                            , NullableString
                                                                            , 97
                                                                            , General.GetNullableInteger(ucState.SelectedState)
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , null
                                                                            , null
                                                                            , NullableString
                                                                            , NullableString
                                                                            , NullableString
                                                                            , null
                                                                            , 1
                                                                            , int.MaxValue
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount
                                                                            , General.GetNullableInteger(ddlPlace.SelectedCity));
        ucInstitution.DataBind();
    }

    protected void ucInstitution_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ucInstitution.SelectedValue.ToUpper() != "DUMMY" && ucInstitution.SelectedValue.ToUpper() != "-1")
            {
                DataSet ds = PhoenixPreSeaAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , long.Parse(ucInstitution.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ucAddress.Address1 = dr["FLDADDRESS1"].ToString();
                    ucAddress.Address2 = dr["FLDADDRESS2"].ToString();
                    ucAddress.Address3 = dr["FLDADDRESS3"].ToString();
                    ucAddress.Address4 = dr["FLDADDRESS4"].ToString();

                    ucAddress.Country = dr["FLDCOUNTRYID"].ToString();
                    ucAddress.State = dr["FLDSTATE"].ToString();
                    ucAddress.City = dr["FLDCITY"].ToString();
                    ucAddress.PostalCode = dr["FLDPOSTALCODE"].ToString();
                }
            }
            txtInstituteName.Text = "";
            if (ucInstitution.SelectedItem.Text.ToUpper() != "OTHERS" && ucInstitution.SelectedItem.Text.ToUpper() != "--SELECT--")
            {
                txtInstituteName.Text = ucInstitution.SelectedItem.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlInstitution_DataBound(object sender, EventArgs e)
    {
        ucInstitution.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        ucInstitution.Items.Insert(1, new ListItem("Others", "-1"));
    }

    string total;
    string overallpercentage;

    protected void Qualification_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ddlQualification.SelectedValue).HasValue)
        {
            DataTable dt = PhoenixPreSeaNewApplicantAcademicQualification.ListCandidateQualification(int.Parse(Filter.CurrentPreSeaNewApplicantSelection)
                                                                                                    , General.GetNullableInteger(ddlQualification.SelectedValue));
            if (dt.Rows.Count > 0)
            {
                academicid = dt.Rows[0]["FLDACADEMICID"].ToString();
                academictype = dt.Rows[0]["FLDACADEMICTYPE"].ToString();
                ViewState["EMPLOYEEACADEMICID"] = dt.Rows[0]["FLDACADEMICID"].ToString();
                EditPreSeaAcademic(academicid);
            }
            else
            {
                academicid = "";
                academictype = "";
                ViewState["EMPLOYEEACADEMICID"] = null;
                ResetAcademicDeatils();
            }
        }
        else
        {
            academicid = "";
            academictype = "";
            ViewState["EMPLOYEEACADEMICID"] = null;
            ResetAcademicDeatils();
        }
        BindMarks();
    }

    protected void rblMarksGrade_Changed(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr in gvMarks.Rows)
        {
            UserControlMaskNumber marksobtained = (UserControlMaskNumber)gvr.FindControl("txtMarksEdit");
            UserControlMaskNumber maxmarks = (UserControlMaskNumber)gvr.FindControl("txtOutOffEdit");

            DropDownList grade = (DropDownList)gvr.FindControl("ddlGradeEdit");
            UserControlMaskNumber gradepoint = (UserControlMaskNumber)gvr.FindControl("txtGradePointEdit");

            if (rblMarksGrade.SelectedValue == "1") //marks
            {
                marksobtained.Enabled = true;
                maxmarks.Enabled = true;

                marksobtained.CssClass = "input_mandatory";
                maxmarks.CssClass = "input_mandatory";

                grade.Enabled = false;
                gradepoint.Enabled = false;
            }
            if (rblMarksGrade.SelectedValue == "2") //grade
            {
                marksobtained.Enabled = false;
                maxmarks.Enabled = false;

                marksobtained.CssClass = "readonlytextbox";
                maxmarks.CssClass = "readonlytextbox";

                if (General.GetNullableInteger(grade.SelectedValue) == null)
                {
                    grade.Enabled = true;
                    gradepoint.Enabled = true;

                }
                else
                {
                    grade.Enabled = false;
                    gradepoint.Enabled = false;

                }
                
            }
        }
    }
    protected void gvMarks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                overallpercentage = "0";
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                if (ViewState["INTERVIEWIDYN"].ToString() == "1" && edit != null)
                    edit.Visible = false;

                ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                if (ViewState["INTERVIEWIDYN"].ToString() == "1" && del != null)
                    del.Visible = false;

                ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                ImageButton add = (ImageButton)e.Row.FindControl("cmdAdd");
                if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);


                DropDownList ddlGrade = (DropDownList)e.Row.FindControl("ddlGradeEdit");
                UserControlMaskNumber umgpoint = (UserControlMaskNumber)e.Row.FindControl("txtGradePointEdit");
                UserControlMaskNumber Marks = (UserControlMaskNumber)e.Row.FindControl("txtMarksEdit");
                UserControlMaskNumber Maxmarks = (UserControlMaskNumber)e.Row.FindControl("txtOutOffEdit");
                
                if (Marks.Text != "")
                    Marks.ReadOnly = true;
                if (Maxmarks.Text != "")
                    Maxmarks.ReadOnly = true;

                if (ddlGrade != null)
                {
                    ddlGrade.DataSource = PhoenixPreSeaCommon.GetCGPAGradePoints();
                    ddlGrade.DataBind();
                    ListItem li = new ListItem("--Select--", "DUMMY");
                    ddlGrade.Items.Insert(0, li);

                    ddlGrade.SelectedValue = drv["FLDGRADEPOINT"].ToString();
                }

                Label lblTotal = (Label)e.Row.FindControl("lblTotalMarks");
                Label lblPercentage = (Label)e.Row.FindControl("lblAvg");

                if (lblTotal != null)
                    total = lblTotal.Text;

                if (lblPercentage != null)
                    overallpercentage = lblPercentage.Text;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                txtTotal.Text = total;
                txtPercentage.Text = overallpercentage;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void ddlGrade_TextChanged(object sender, EventArgs e)
    {
        DropDownList ddlGrade = (DropDownList)sender;
        GridViewRow gv = (GridViewRow)ddlGrade.Parent.Parent;
        if (gv != null)
        {
            UserControlMaskNumber umgpoint = (UserControlMaskNumber)gv.FindControl("txtGradePointEdit");

            if (General.GetNullableInteger(ddlGrade.SelectedValue).HasValue)
                umgpoint.Text = ddlGrade.SelectedValue;
            else
                umgpoint.Text = "";
        }
    }

    protected void Application_Save(object sender, EventArgs e)
    {
        try
        {
            SaveAcademicDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

}
