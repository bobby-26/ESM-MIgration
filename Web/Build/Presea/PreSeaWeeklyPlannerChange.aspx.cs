using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaWeeklyPlannerChange : PhoenixBasePage
{

    string strBatch = "", strSemester ="";

    #region :   Page load and render Events :

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            MainMenuPreseaWeekPlanner.AccessRights = this.ViewState;
            MainMenuPreseaWeekPlanner.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["DAYID"] = "";
                ViewState["HOURID"] = "";

                ViewState["DAYID"] = Request.QueryString["Day"];
                ViewState["HOURID"] = Request.QueryString["Hour"];
 
                rdoActivity.DataSource = PhoenixPreSeaQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 102);
                rdoActivity.DataTextField = "FLDQUICKNAME";
                rdoActivity.DataValueField = "FLDQUICKCODE";
                rdoActivity.DataBind();

                SetPrimaryDayDetails();
                BindFaculty(ddlFaculty);
                SetPrimaryHourDetails();
                DisableControls();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

    #region :   Methods :

    protected void SetPrimaryDayDetails()
    {
        if (!String.IsNullOrEmpty(ViewState["DAYID"].ToString()))
        {
            DataTable dt = PhoenixPreSeaWeeklyPlanner.EditDayEntry(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["DAYID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ucBatch.SelectedBatch = dr["FLDBATCHID"].ToString();
                ucSemester.Batch = dr["FLDBATCHID"].ToString();
                ucSemester.SelectedSemester = dr["FLDSEMESTERID"].ToString();
                strBatch = dr["FLDBATCHID"].ToString();
                strSemester = dr["FLDSEMESTERID"].ToString();
                txtSection.Text = dr["FLDSECTIONNAME"].ToString();
                ucWeek.SelectedWeek = dr["FLDWEEKID"].ToString();
                DateTime dat = Convert.ToDateTime(dr["FLDDATE"].ToString());
                txtDate.Text = dat.ToShortDateString();
                BindPracticalDivisions(dr["FLDBATCHID"].ToString(), dr["FLDSECTIONID"].ToString());
            }
        }
    }

    protected void SetPrimaryHourDetails()
    {
        if (!String.IsNullOrEmpty(ViewState["HOURID"].ToString()))
        {
            DataTable dt = PhoenixPreSeaWeeklyPlanner.EditHourlyEntry(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["HOURID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                rdoActivity.SelectedValue = dr["FLDCLASSTYPE"].ToString();
                ddlDuration.SelectedValue = dr["FLDTOTALHOURS"].ToString();
                ucTimeSlotFrom.SelectedTime = dr["FLDHOURFROM"].ToString();
                ucTimeSlotTo.SelectedTime = dr["FLDHOURTO"].ToString();

                string divisions = dr["FLDPRACTICALDIVISIONS"].ToString();
                string subjects = dr["FLDPRACTICALSUBJECTS"].ToString();
                string instructors = dr["FLDINSTRUCTORS"].ToString();

                if (!String.IsNullOrEmpty(dr["FLDPRACTICALDIVISIONS"].ToString()))
                {
                    ucSubject.SelectedSubject = "";
                    ucSubject.Enabled = false;
                    ddlFaculty.SelectedValue = "DUMMY";
                    ddlFaculty.Enabled = false;
                    ClassDetails.Visible = false;
                    PracticalDetails.Visible = true;
                    FillPracticalDetails(divisions, subjects, instructors);
                }
                else if (!String.IsNullOrEmpty(dr["FLDSUBJECT"].ToString()))
                {
                    ucSubject.SelectedSubject = dr["FLDSUBJECT"].ToString();
                    ucSubject.Enabled = true;
                    ddlFaculty.SelectedValue = dr["FLDFACULTY"].ToString();
                    ddlFaculty.Enabled = true;
                    ClassDetails.Visible = true;
                    PracticalDetails.Visible = false;
                    grdPracticalTimetable.Enabled = false;
                }
                else
                {
                    ucSubject.Enabled = false;
                    ddlFaculty.Enabled = false;
                    PracticalDetails.Visible = false;
                    ClassDetails.Visible = true;
                    txtBreakDesc.Text = dr["FLDBREAKDETAILS"].ToString();
                }


            }
        }
    }

    private bool IsValidWeeklyPlan(bool notAllFilled)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucTimeSlotFrom.SelectedTime) == null)
            ucError.ErrorMessage = "Time from is required.";
        if (General.GetNullableInteger(ucTimeSlotTo.SelectedTime) == null)
            ucError.ErrorMessage = "Time to is required.";

        if (rdoActivity.SelectedItem.Text.ToUpper().Contains("PRACTICAL") && notAllFilled == true)
        {
            ucError.ErrorMessage = "All Practical Details has to be filled for Pracical Class";
        }
        else if (rdoActivity.SelectedItem.Text.Contains("Theory"))
        {
            if(General.GetNullableInteger(ucSubject.SelectedSubject) == null)
                ucError.ErrorMessage = "Subject is required for Theory Class.";
            if (General.GetNullableInteger(ddlFaculty.SelectedValue) == null)
                ucError.ErrorMessage = "Faculty is required for Theory Class";
        }

        if (General.GetNullableInteger(ucTimeSlotTo.SelectedTime).HasValue && General.GetNullableInteger(ucTimeSlotFrom.SelectedTime).HasValue)
        {
            if (ucTimeSlotFrom.SelectedTime == ucTimeSlotTo.SelectedTime)
                ucError.ErrorMessage = "Time from and Time to should not be Same.";
            else if (int.Parse(ucTimeSlotFrom.SelectedTime) > int.Parse(ucTimeSlotTo.SelectedTime))
                ucError.ErrorMessage = "Time to should be grater than From Time.";
        }

        return (!ucError.IsError);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        if (dt.Rows.Count <= 0)
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void BindFaculty(DropDownList ddl)
    {
        if (General.GetNullableInteger(strBatch).HasValue)
        {
            ddl.Items.Clear();
            ListItem li = new ListItem("--Select--", "DUMMY");
            ddl.Items.Add(li);

            DataSet ds = PhoenixPreSeaBatchAdmissionContact.ListPreSeaBatchAdmissionContact(Convert.ToInt32(strBatch));

            ddl.DataSource = ds.Tables[0];
            ddl.DataTextField = "FLDCONTACTNAME";
            ddl.DataValueField = "FLDUSERCODE";
            ddl.DataBind();
        }
    }

    protected void BindPracticalDivisions(string batch, string section)
    {
        DataSet ds = PhoenixPreSeaTrainee.ListPreSeaPractical(General.GetNullableInteger(batch), General.GetNullableInteger(section));
        if (ds.Tables.Count > 0)
        {
            grdPracticalTimetable.DataSource = ds.Tables[0];
            grdPracticalTimetable.DataBind();
        }
    }

    protected void FillPracticalDetails(string divisions, string subjects, string instructors)
    {
        
        string[] divisionValue = divisions.Split(',');
        string[] subjectValue = subjects.Split(',');
        string[] instructorValue = instructors.Split(',');
        int i = 0;
        foreach (GridViewRow row in grdPracticalTimetable.Rows)
        {

            UserControlPreSeaBatchSubject uc = (UserControlPreSeaBatchSubject)row.FindControl("ucPracSubject");
            DropDownList ddl = (DropDownList)row.FindControl("ddlInstructor");
            uc.SelectedSubject = subjectValue[i].ToString();
            ddl.SelectedValue = instructorValue[i].ToString();
            i++;
        }
    }

    protected void GetPracticalDetails(ref string divisions, ref string subjects, ref string instructors, ref bool notAllfilled)
    {
        foreach (GridViewRow row in grdPracticalTimetable.Rows)
        {
            Label lbl = (Label)row.FindControl("lblDivisionId");
            UserControlPreSeaBatchSubject uc = (UserControlPreSeaBatchSubject)row.FindControl("ucPracSubject");
            DropDownList ddl = (DropDownList)row.FindControl("ddlInstructor");
            if (General.GetNullableInteger(uc.SelectedSubject) == null || General.GetNullableInteger(ddl.SelectedValue) == null)
            {
                notAllfilled = true;
                return;
            }
            else
            {
                divisions += lbl.Text + ",";
                subjects += uc.SelectedSubject + ",";
                instructors += ddl.SelectedValue + ",";
            }
        }
    }

    protected void ClearPracticalDetails()
    {
        foreach (GridViewRow row in grdPracticalTimetable.Rows)
        {

            UserControlPreSeaBatchSubject uc = (UserControlPreSeaBatchSubject)row.FindControl("ucPracSubject");
            DropDownList ddl = (DropDownList)row.FindControl("ddlInstructor");
            uc.SelectedSubject = "";
            ddl.SelectedValue = "DUMMY";

        }
    }

    private void BindClassRoom()
    {
        ddlClassRoom.Items.Clear();
        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlClassRoom.Items.Add(li);

        DataTable dt = PhoenixPreSeaWeeklyPlanner.ListAvailableClassRoom(General.GetNullableDateTime(txtDate.Text.Trim()), ucTimeSlotFrom.SelectedTimeSlot, ucTimeSlotTo.SelectedTimeSlot);

        ddlClassRoom.DataSource = dt;
        ddlClassRoom.DataBind();
    }

    #endregion

    # region :  Radio Button and Drop down Selection Change events  :

    protected void rdoActivity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoActivity.SelectedItem.Text.ToUpper().Contains("PRACTICAL"))
        {
            ucSubject.Enabled = false;
            ddlFaculty.Enabled = false;
            ucSubject.CssClass = "readonlytextbox";
            ddlFaculty.CssClass = "readonlytextbox";
            grdPracticalTimetable.Enabled = true;
            txtBreakDesc.CssClass = "readonlytextbox";
            txtBreakDesc.Enabled = false;

            ClassDetails.Visible = false;
            PracticalDetails.Visible = true;
        }
        else if (rdoActivity.SelectedItem.Text.Contains("Theory"))
        {
            ucSubject.CssClass = "input_mandatory";
            ddlFaculty.CssClass = "input_mandatory";
            ucSubject.Enabled = true;
            ddlFaculty.Enabled = true;
            grdPracticalTimetable.Enabled = false;
            txtBreakDesc.CssClass = "readonlytextbox";
            txtBreakDesc.Enabled = false;

            PracticalDetails.Visible = false;
            ClassDetails.Visible = true;
        }
        else
        {
            ucSubject.CssClass = "readonlytextbox";
            ddlFaculty.CssClass = "readonlytextbox";
            ucSubject.Enabled = false;
            ddlFaculty.Enabled = false;
            txtBreakDesc.Enabled = true;
            txtBreakDesc.CssClass = "input_mandatory";

            ClassDetails.Visible = true;
            PracticalDetails.Visible = false;
        }

    }

    protected void TimeSlot_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableString(ucTimeSlotTo.SelectedTimeSlot) != null)
        {
            BindClassRoom();
        }
    }
    private void DisableControls()
    {
        ucBatch.Enabled = false;
        txtSection.ReadOnly = true;
        ucWeek.Enabled = false;
        ucSemester.Enabled = false;
        txtDate.ReadOnly = true;
    }
    # endregion

    # region :  Tabstrip Events :

    protected void MainMenuPreseaWeekPlanner_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
            String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                string sublist = "";
                string instructorlist = "";
                string divisionlist = "";
                bool notAllfilled = false;
                GetPracticalDetails(ref divisionlist, ref sublist, ref instructorlist, ref notAllfilled);

                if (!IsValidWeeklyPlan(notAllfilled))
                {
                    ucError.Visible = true;
                    return;
                }
                if (!String.IsNullOrEmpty(ViewState["HOURID"].ToString()))
                {
                    PhoenixPreSeaWeeklyPlanner.UpdateHourlyEntry(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                               , new Guid(ViewState["HOURID"].ToString())
                                                               , int.Parse(rdoActivity.SelectedValue)
                                                               , ucTimeSlotFrom.SelectedTimeSlot
                                                               , ucTimeSlotTo.SelectedTimeSlot
                                                               , General.GetNullableByte("0")
                                                               , General.GetNullableInteger(ucSubject.SelectedSubject)
                                                               , General.GetNullableInteger(ddlFaculty.SelectedValue)
                                                               , sublist
                                                               , divisionlist
                                                               , instructorlist
                                                               , decimal.Parse(ddlDuration.SelectedValue)
                                                               , txtBreakDesc.Text.Trim()
                                                               , General.GetNullableInteger(ddlClassRoom.SelectedValue), "");

                }

                ucStatus.Text = "Plan updated Successfully.";

            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupopen, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

    #region :   Grid Events :

    protected void grdPracticalTimetable_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            UserControlPreSeaBatchSubject ucSub = (UserControlPreSeaBatchSubject)e.Row.FindControl("ucPracSubject");
            if (ucSub != null)
            {
                ucSub.SubjectList = PhoenixPreSeaBatchManager.ListPreSeaBatchSubject(General.GetNullableInteger(strBatch), General.GetNullableInteger(strSemester));
            }
            DropDownList ddlInstuctor = (DropDownList)e.Row.FindControl("ddlInstructor");
            if (ddlInstuctor != null)
                BindFaculty(ddlInstuctor);
        }
    }

    #endregion


}
