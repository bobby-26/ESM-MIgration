using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;


public partial class PreSeaNewApplicantAcademicDetail : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            //if (Request.QueryString["ACADEMICID"] != null)
            //{
            //    EditPreSeaAcademics(Int16.Parse(Request.QueryString["ACADEMICID"].ToString()));
            //}
        }
        base.Render(writer);

    }
    string empid = string.Empty;
    string academictype = string.Empty;
    string academicid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        empid = Request.QueryString["empid"].ToString();
        academictype = Request.QueryString["ACADEMICTYPE"].ToString();
        academicid = Request.QueryString["ACADEMICID"].ToString();
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE");
        MenuPreSeaAcademic.AccessRights = this.ViewState;
        MenuPreSeaAcademic.MenuList = toolbar.Show();
        if (!IsPostBack)
        {

            ViewState["EMPLOYEEACADEMICID"] = Request.QueryString["ACADEMICID"];
            ViewState["ACADEMICTYPE"] = Request.QueryString["ACADEMICTYPE"];
            BindYearFrom();
            BindYearTo();
            BindYearPassYear();
            BindInstitute();            
            EditPreSeaAcademic(ViewState["EMPLOYEEACADEMICID"].ToString());
        }
        BindMarks();
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
    protected void BindInstitute()
    {
        DataSet ds = PhoenixPreSeaAddress.ListAddress(null);
        ddlInstitute.DataSource = ds;
        ddlInstitute.DataTextField = "FLDNAME";
        ddlInstitute.DataValueField = "FLDADDRESSCODE";
        ddlInstitute.DataBind();
        ddlInstitute.Items.Insert(0, "--Select--");
    }
    private void EditPreSeaAcademic(string academicid)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = PhoenixPreSeaNewApplicantAcademicQualification.EditPreSeaNewApplicantAcademic(Convert.ToInt32(empid), General.GetNullableInteger(academicid));
            if (dt.Rows.Count > 0)
            {
                ddlCertificate.SelectedQualification = dt.Rows[0]["FLDCERTIFICATE"].ToString();
                ucAcademicBoard.SelectedQuick = dt.Rows[0]["FLDBOARD"].ToString();
                ddlInstitute.SelectedValue = dt.Rows[0]["FLDINSTITUTIONID"].ToString();
                //ddl.Text = dt.Rows[0]["FLDINSTITUTION"].ToString();
                txtUniversity.Text = dt.Rows[0]["FLDUNIVERSITY"].ToString();
                txtExamRollno.Text = dt.Rows[0]["FLDROLLNUMBER"].ToString();
                ddlYearFrom.SelectedValue = dt.Rows[0]["FLDYEARSFROM"].ToString();
                ddlYearTo.SelectedValue = dt.Rows[0]["FLDYEARSTO"].ToString();
                ddlYearPass.SelectedValue = dt.Rows[0]["FLDYEAROFPASSING"].ToString();
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
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetInstituteAddress()
    {
        int instituteid = 0;
        if (General.GetNullableInteger(ddlInstitute.SelectedValue) != null)
            instituteid = General.GetNullableInteger(ddlInstitute.SelectedValue).Value;

        DataSet ds = PhoenixPreSeaAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                         instituteid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ucAddress.Address1   = ds.Tables[0].Rows[0]["FLDADDRESS1"].ToString();
            ucAddress.Address2   = ds.Tables[0].Rows[0]["FLDADDRESS2"].ToString();
            ucAddress.Address3   = ds.Tables[0].Rows[0]["FLDADDRESS3"].ToString();
            ucAddress.Address4   = ds.Tables[0].Rows[0]["FLDADDRESS4"].ToString();
            ucAddress.Country    = ds.Tables[0].Rows[0]["FLDCOUNTRYID"].ToString();
            ucAddress.State      = ds.Tables[0].Rows[0]["FLDSTATE"].ToString();
            ucAddress.City       = ds.Tables[0].Rows[0]["FLDCITY"].ToString();
            ucAddress.PostalCode = ds.Tables[0].Rows[0]["FLDPOSTALCODE"].ToString();
        }

    }
    protected void gvMarks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            ImageButton add = (ImageButton)e.Row.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);


            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlsubject = (DropDownList)e.Row.FindControl("ddlSubjectAdd");
                DataTable dt = new DataTable();
                ddlsubject.Items.Clear();
                dt = PhoenixPreSeaAcadamicSubjects.ListPreSeaCourseSubjects(General.GetNullableInteger(ViewState["ACADEMICTYPE"].ToString()));

                if (ddlsubject != null)
                {
                    ListItem li = new ListItem("-select-", "Dummy");
                    ddlsubject.Items.Add(li);
                    foreach (DataRow dr in dt.Rows)
                    {
                        li = new ListItem(dr["FLDSUBJECTNAME"].ToString(), dr["FLDSUBJECTID"].ToString());
                        ddlsubject.Items.Add(li);
                    }

                    ddlsubject.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindMarks()
    {
      int employeeid = int.Parse(empid);
        try
        {
            DataTable dt = new DataTable();
            dt = PhoenixPreSeaNewApplicantAcademicQualification.ListPreSeaNewApplicantAcademicMark(employeeid, Convert.ToInt32(academicid), General.GetNullableInteger(academictype));
            if (dt.Rows.Count > 0)
            {
                gvMarks.DataSource = dt;
                gvMarks.DataBind();
                txtTotal.Text = dt.Rows[0]["FLDTOTALMARKSOBTAINED"].ToString();
                txtPercentage.Text = dt.Rows[0]["FLDAVERAGE"].ToString();
                txtoutoff.Text = dt.Rows[0]["FLDTOTALMARKS"].ToString();
            }
            else
            {
                ShowNoRecordsFound(dt, gvMarks);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PreSeaAcademic_TabStripCommand(object sender, EventArgs e)
    {
        String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
        String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidAcademic(
                                      ddlInstitute.SelectedValue
                                    , ddlYearFrom.SelectedValue
                                    , ddlYearTo.SelectedValue
                                    , ddlYearPass.SelectedValue
                                    , ddlFirstAttemptYN.SelectedValue
                                    , ucAddress.Address1
                                    , ucAddress.Country
                                    , ucAddress.City))
                {
                    PhoenixPreSeaNewApplicantAcademicQualification.UpdatePreSeaNewApplicantAcademic(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                                  , Convert.ToInt32(empid)
                                                                                                  , General.GetNullableInteger(academicid)
                                                                                                  , General.GetNullableInteger(ddlInstitute.SelectedValue)
                                                                                                  , Convert.ToInt16(ddlCertificate.SelectedQualification)
                                                                                                  , General.GetNullableDecimal(txtPercentage.Text)
                                                                                                  , null
                                                                                                  , ucAddress.Address1
                                                                                                  , ucAddress.Address2
                                                                                                  , ucAddress.Address3
                                                                                                  , ucAddress.Address4
                                                                                                  , ucAddress.PostalCode
                                                                                                  , General.GetNullableInteger(ucAddress.Country)
                                                                                                  , General.GetNullableInteger(ucAddress.State)
                                                                                                  , General.GetNullableInteger(ucAddress.City)
                                                                                                  , General.GetNullableInteger(ddlCertificate.SelectedQualification)
                                                                                                  , General.GetNullableInteger(ucAcademicBoard.SelectedQuick)
                                                                                                  , General.GetNullableInteger(ddlFirstAttemptYN.SelectedValue)
                                                                                                  , General.GetNullableInteger(ddlYearFrom.SelectedValue)
                                                                                                  , General.GetNullableInteger(ddlYearTo.SelectedValue)
                                                                                                  , General.GetNullableInteger(ddlYearPass.SelectedValue)
                                                                                                  , General.GetNullableInteger(chkResultYN.Checked == true ? "1" : "0")
                                                                                                  , txtExamRollno.Text
                                                                                                  , txtUniversity.Text
                                                                                                  , General.GetNullableDecimal(txtTotal.Text)
                                                                                                  , ddlInstitute.Text
                                                                                                  );
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMarks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindMarks();
    }
    protected void gvMarks_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            GridView _gridView = (GridView)sender;

            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                DropDownList subject = (DropDownList)_gridView.FooterRow.FindControl("ddlSubjectAdd");
                string marks = ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtMarksAdd")).Text;
                string outoff = ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtOutOffAdd")).Text;

                if (!IsValidMarks(subject.SelectedValue, marks, outoff))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPreSeaNewApplicantAcademicQualification.InsertPreSeaNewApplicantAcademicMark(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , int.Parse(empid)
                                                                                    , int.Parse(academicid)
                                                                                    , General.GetNullableInteger(academictype)
                                                                                    , General.GetNullableInteger(subject.SelectedValue)
                                                                                    , General.GetNullableDecimal(marks)
                                                                                    , General.GetNullableDecimal(outoff)
                                                                                    , General.GetNullableDecimal(""));
                BindMarks();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMarks_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            Label marksid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblMarksId");
            if (!string.IsNullOrEmpty(marksid.Text))
            {
                PhoenixPreSeaNewApplicantAcademicQualification.DeletePreSeaNewApplicantAcademicMark(PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , int.Parse(empid)
               , int.Parse(academicid)
               , General.GetNullableGuid(marksid.Text));
            }
           
            BindMarks();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMarks_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindMarks();
    }
    protected void gvMarks_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            Label subject = (Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectName");
            Label subjectid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectId");
            Label marksid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblMarksId");
            string marks = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtMarksEdit")).Text;
            string outoff = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtOutOffEdit")).Text;

            if (!IsValidMarks(subject.Text, marks, outoff))
            {
                ucError.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(marksid.Text))
            {
                PhoenixPreSeaNewApplicantAcademicQualification.UpdatePreSeaNewApplicantAcademicMark(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                                   , int.Parse(empid)
                                                                                                   , int.Parse(academicid)
                                                                                                   , General.GetNullableGuid(marksid.Text)
                                                                                                   , General.GetNullableInteger(subjectid.Text)
                                                                                                   , General.GetNullableDecimal(marks)
                                                                                                   , General.GetNullableDecimal(outoff)
                                                                                                   , General.GetNullableDecimal(""));
            }
            else
            {
                PhoenixPreSeaNewApplicantAcademicQualification.InsertPreSeaNewApplicantAcademicMark(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                   , int.Parse(empid)
                                                                                   , int.Parse(academicid)
                                                                                   , General.GetNullableInteger(academictype)
                                                                                   , General.GetNullableInteger(subjectid.Text)
                                                                                   , General.GetNullableDecimal(marks)
                                                                                   , General.GetNullableDecimal(outoff)
                                                                                   , General.GetNullableDecimal(""));
            }
            _gridView.EditIndex = -1;
            BindMarks();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidMarks(string subject, string marks, string outoff)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        decimal resultDec, resultout;

        if (subject == null || subject == "")
            ucError.ErrorMessage = "Subject/Sem is required.";

        if (!decimal.TryParse(marks, out resultDec))
            ucError.ErrorMessage = "Mark obtain is required";

        if (!decimal.TryParse(outoff, out resultout))
            ucError.ErrorMessage = "Mark obtain out off is required";

        return (!ucError.IsError);
    }
    private bool IsValidAcademic(
               string institution
              , string yearfrom
              , string yearto
              , string yearpass
              , string attempt
              , string address1
              , string country
              , string city)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        Int32 result;

        if (ucAcademicBoard.SelectedQuick == "" || ucAcademicBoard.SelectedQuick.ToUpper().Equals("DUMMY"))
        {
            ucError.ErrorMessage = "Board required";
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

    protected void ddlInstitute_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetInstituteAddress();
    }
}
