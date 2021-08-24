using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Drawing;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaWeeklyPlannerCopyHours : PhoenixBasePage
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
                ucSemester.SelectedSemester = dr["FLDSEMESTERID"].ToString();
                strBatch = dr["FLDBATCHID"].ToString();
                strSemester = dr["FLDSEMESTERID"].ToString();
                txtSection.Text = dr["FLDSECTIONNAME"].ToString();
                lblSectionId.Text = dr["FLDSECTIONID"].ToString();
                ucWeek.SelectedWeek = dr["FLDWEEKID"].ToString();
                DateTime dat = Convert.ToDateTime(dr["FLDDATE"].ToString());
                txtDate.Text = dat.ToShortDateString();

                if (!String.IsNullOrEmpty(dr["FLDSECTIONNAME"].ToString()))
                    BindDates(int.Parse(dr["FLDWEEKID"].ToString()));
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
                }
                else if (!String.IsNullOrEmpty(dr["FLDSUBJECT"].ToString()))
                {
                    ucSubject.SelectedSubject = dr["FLDSUBJECT"].ToString();
                    ucSubject.Enabled = true;
                    ddlFaculty.SelectedValue = dr["FLDFACULTY"].ToString();
                    ddlFaculty.Enabled = true;
                    ClassDetails.Visible = true;
                    PracticalDetails.Visible = false;
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

    private bool IsValidWeeklyPlan()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ddlCopyDate.SelectedValue) == null)
            ucError.ErrorMessage = "Copy to date is required";

        if (General.GetNullableString(ddlCopyTimeSlots.SelectedValue) == null)
            ucError.ErrorMessage = "Copy to time slot is required";

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

            DataSet ds = PhoenixPreSeaBatchAdmissionContact.ListPreSeaBatchAdmissionContact(Convert.ToInt32(ucBatch.SelectedBatch));

            ddl.DataSource = ds.Tables[0];
            ddl.DataTextField = "FLDCONTACTNAME";
            ddl.DataValueField = "FLDUSERCODE";
            ddl.DataBind();
        }
    }

    private void BindDates(int week)
    {
        ddlCopyDate.Items.Clear();
        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlCopyDate.Items.Add(li);

        DataTable dt = PhoenixPreSeaWeeklyPlanner.ListDaysinWeek(week);

        ddlCopyDate.DataSource = dt;
        ddlCopyDate.DataBind();

    }

    #endregion

    # region :  Dropdown Change events  :

    protected void ddlCopyDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (General.GetNullableString(ddlCopyDate.SelectedValue) != null)
        {
            ddlCopyTimeSlots.Items.Clear();
            ListItem li = new ListItem("--Select--", "DUMMY");
            ddlCopyTimeSlots.Items.Add(li);

            int batch = int.Parse(ucBatch.SelectedBatch);
            int semester = int.Parse(ucSemester.SelectedSemester);
            int week = int.Parse(ucWeek.SelectedWeek);
            int section = int.Parse(lblSectionId.Text);
            DateTime date = DateTime.Parse(ddlCopyDate.SelectedValue);

            DataTable dt = PhoenixPreSeaWeeklyPlanner.ListPlannedSlotsForTheWeek(batch,semester,week,section,date);

            ddlCopyTimeSlots.DataSource = dt;
            ddlCopyTimeSlots.DataBind();
        }
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


                if (!IsValidWeeklyPlan())
                {
                    ucError.Visible = true;
                    return;
                }
                if (!String.IsNullOrEmpty(ViewState["HOURID"].ToString()))
                {
                    PhoenixPreSeaWeeklyPlannerMakeCopy.CopyWeeklyPlanSlottoSlot(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , new Guid(ViewState["HOURID"].ToString())
                                                                                , DateTime.Parse(ddlCopyDate.SelectedValue)
                                                                                , ddlCopyTimeSlots.SelectedValue);

                }

                ucStatus.Text = "Plan copied Successfully.";

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

}
