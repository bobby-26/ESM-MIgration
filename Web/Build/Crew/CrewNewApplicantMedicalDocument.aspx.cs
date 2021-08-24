using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CrewNewApplicantMedicalDocument : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvCrewMedical.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl00");
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl01");
    //        }
    //    }
    //    foreach (GridViewRow r in gvMedicalTest.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl00");
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl01");
    //        }
    //    }
    //    foreach (GridViewRow r in gvVaccination.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl00");
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl01");
    //        }
    //    }


    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {

                AutoArchive();
                AutoCorrectMedicalStatus();


                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["UNFIT"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "UFT");
                ViewState["TEMPFIT"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "TUF");

                SetEmployeePrimaryDetails();

            }
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            CrewMedical.Title = "Medical Document";
            CrewMedical.AccessRights = this.ViewState;
            CrewMedical.MenuList = toolbar1.Show();

            int? vesselid = null;
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantMedicalDocument.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "MEDICALExcel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewMedical')", "Print Grid", "<i class=\"fas fa-print\"></i>", "MEDICALPRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewMedicalAdd.aspx?empid=" + Filter.CurrentNewApplicantSelection + "')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDMEDICAL");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewMedicalArchived.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&type=1'); return false;", "Show Archived", "<i class=\"fas fa-archive\"></i>", "ARCHIVED");
            SetCourseRequest(ref vesselid);
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewMedicalSlip.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&vslid=" + vesselid + "&type=1&newappyn=1'); return false;", "Medical Request", "<i class=\"fas fa-briefcase-medical\"></i>", "MEDREQUEST");
            MenuCrewMedical.AccessRights = this.ViewState;
            MenuCrewMedical.MenuList = toolbar.Show();
            //MenuCrewMedical.SetTrigger(pnlCrewMedicalEntry);

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantMedicalDocument.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "MEDICALTESTExcel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMedicalTest')", "Print Grid", "<i class=\"fas fa-print\"></i>", "MEDICALTESTPRINT");
            CrewMedicalTest.AccessRights = this.ViewState;
            CrewMedicalTest.MenuList = toolbar.Show();
            //CrewMedicalTest.SetTrigger(pnlCrewMedicalEntry);

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantMedicalDocument.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "VACCINATIONExcel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVaccination')", "Print Grid", "<i class=\"fas fa-print\"></i>", "VACCINATIONPRINT");
            CrewVaccination.AccessRights = this.ViewState;
            CrewVaccination.MenuList = toolbar.Show();
            //CrewVaccination.SetTrigger(pnlCrewMedicalEntry);

            // BindMedicalData();
            // BindMedicalTestData();
           // BindMedicalVaccination();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvCrewMedical.Rebind();
        gvMedicalTest.Rebind();
        gvVaccination.Rebind();

    }
    private void AutoArchive()
    {
        PhoenixCrewMedicalDocuments.AutoArchiveCewMedical(Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()));
    }

    private void AutoCorrectMedicalStatus()
    {
        PhoenixCrewMedicalDocuments.AutoCorrectCrewMedicalStatus(Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), null);
    }
    protected void SetCourseRequest(ref int? vesselid)
    {
        DataSet ds = PhoenixCrewMedicalDocuments.ListCrewCourseRequest(Int32.Parse(Filter.CurrentNewApplicantSelection), null);

        if (ds.Tables[0].Rows[0]["FLDVESSELID"].ToString() != "")
        {
            vesselid = Convert.ToInt32(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());

        }
    }
    private void BindMedicalData()
    {
        try
        {
            string[] alColumns = { "FLDMEDICALNAME", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDCOUNTRYNAME", "FLDSTATUSNAME" };
            string[] alCaptions = { "Medical", "Place of Issue", "Issued", "Expiry", "Flag", "Status" };

            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewFlagMedical(
                        Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), 1, null);

            General.SetPrintOptions("gvCrewMedical", "Medical", alCaptions, alColumns, ds);
            gvCrewMedical.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewMedical_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("MEDICALEXCEL"))
            {
                string[] alColumns = { "FLDMEDICALNAME", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDCOUNTRYNAME", "FLDSTATUSNAME" };
                string[] alCaptions = { "Medical", "Place of Issue", "Issued", "Expiry", "Flag", "Status" };

                DataSet ds = PhoenixCrewMedicalDocuments.ListCrewFlagMedical(
                        Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), 1, null);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Medical", ds.Tables[0], alColumns, alCaptions, null, null);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   

   
    private bool IsValidMedical(string medicalid, string dateofissue, string dateofexpiry, string flagid, string placeofissue, string status)
    {
        Int16 resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        DataSet ds = PhoenixRegistersHard.EditHardCode(1, 95, "FLM");

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (medicalid == ds.Tables[0].Rows[0]["FLDHARDCODE"].ToString())
                if (General.GetNullableInteger(flagid) == null)
                    ucError.ErrorMessage = "Flag is required for Flag Medical.";

            if (medicalid != ds.Tables[0].Rows[0]["FLDHARDCODE"].ToString())
                if (!Int16.TryParse(status, out resultInt))
                    ucError.ErrorMessage = "Status is required";
        }
        if (!Int16.TryParse(medicalid, out resultInt))
            ucError.ErrorMessage = "Medical is required";

        if (string.IsNullOrEmpty(dateofissue))
            ucError.ErrorMessage = "Issued is required.";
        else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issued should be earlier than current date";
        }
        // Disable this validation Bug Id 7172
        //if (ViewState["LASTSIGNON"].ToString() != string.Empty && (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(ViewState["LASTSIGNON"].ToString())) < 0))
        //{
        //    ucError.ErrorMessage = "Issue Date should be greater than Last Signon Date";
        //}

        if (string.IsNullOrEmpty(placeofissue))
        {
            ucError.ErrorMessage = "Place of Issue is required";
        }
        if (string.IsNullOrEmpty(dateofexpiry))
            ucError.ErrorMessage = "Expiry is required.";

        if (dateofissue != null && dateofexpiry != null)
        {
            if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry, out resultDate)))
                if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry)))
                    ucError.ErrorMessage = "'Expiry' should be greater than 'Issued'";
        }

        return (!ucError.IsError);
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
                ViewState["LASTSIGNON"] = dt.Rows[0]["FLDLASTSIGNONDATE"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }


    private void BindMedicalTestData()
    {
        try
        {
            string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDSTATUSNAME", "FLDREMARKS" };
            string[] alCaptions = { "Medical Test", "Place of Issue", "Issued", "Expiry", "Status", "Remarks" };

            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewMedicalTest(
                        Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), 1, null);

            General.SetPrintOptions("gvMedicalTest", "Medical Test", alCaptions, alColumns, ds);
            gvMedicalTest.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewMedicalTest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("MEDICALTESTEXCEL"))
            {
                string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDSTATUSNAME", "FLDREMARKS" };
                string[] alCaptions = { "Medical Test", "Place of Issue", "Issued", "Expiry", "Status", "Remarks" };

                DataSet ds = PhoenixCrewMedicalDocuments.ListCrewMedicalTest(
                        Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), 1, null);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Medical Test", ds.Tables[0], alColumns, alCaptions, null, null);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMedicalTest_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindMedicalTestData();
    }


    protected void gvMedicalTest_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string medicaltestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblMedicalTestId")).Text;

            PhoenixCrewMedicalDocuments.DeleteCrewMedicalTest(
                Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                , Convert.ToInt32(medicaltestid)
                                                            );
            BindMedicalTestData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvMedicalTest_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.SelectedIndex = e.NewEditIndex;
            _gridView.EditIndex = e.NewEditIndex;
            BindMedicalTestData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


   
   

    private bool IsValidMedicalTest(string medicaltestid, string dateofissue, string dateofexpiry, string status, string remarks)
    {
        Int16 resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!Int16.TryParse(medicaltestid, out resultInt))
            ucError.ErrorMessage = "Medical Test is required";

        if (string.IsNullOrEmpty(dateofissue))
            ucError.ErrorMessage = "Issued is required.";

        else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issued should be earlier than current date";
        }
        if (status != ViewState["UNFIT"].ToString() && status != ViewState["TEMPFIT"].ToString())
        {
            if (string.IsNullOrEmpty(dateofexpiry))
                ucError.ErrorMessage = "Expiry is required.";
            if (dateofissue != null && dateofexpiry != null)
            {
                if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry, out resultDate)))
                    if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry)))
                        ucError.ErrorMessage = "'Expiry' should be greater than 'Issued'";
            }
        }
        else if (dateofissue != null && dateofexpiry != null)
        {
            if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry, out resultDate)))
                if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry)))
                    ucError.ErrorMessage = "'Expiry' should be greater than 'Issued'";

        }
        if (General.GetNullableInteger(status) == null)
        {
            ucError.ErrorMessage = "Status is required.";
        }
        if (status == ViewState["UNFIT"].ToString() || status == ViewState["TEMPFIT"].ToString())
        {

            if (remarks == "")
                ucError.ErrorMessage = "Remarks is required.";

        }

        return (!ucError.IsError);
    }


    private void BindMedicalVaccination()
    {
        try
        {
            string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE" };
            string[] alCaptions = { "Vaccination", "Place of Issue", "Issued", "Expiry" };

            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewVaccination(
                        Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), 1, null);

            General.SetPrintOptions("gvVaccination", "Vaccination", alCaptions, alColumns, ds);
            gvVaccination.DataSource = ds;
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewVaccination_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("VACCINATIONEXCEL"))
            {
                string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE" };
                string[] alCaptions = { "Vaccination", "Place of Issue", "Issued", "Expiry" };

                DataSet ds = PhoenixCrewMedicalDocuments.ListCrewVaccination(
                        Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), 1, null);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Vaccination", ds.Tables[0], alColumns, alCaptions, null, null);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVaccination_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindMedicalVaccination();
    }

  
    protected void gvVaccination_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string crewvaccinationid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewVaccinationId")).Text;

            PhoenixCrewMedicalDocuments.DeleteCrewVaccination(
                Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                , Convert.ToInt32(crewvaccinationid)
            );
            BindMedicalVaccination();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvVaccination_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.SelectedIndex = e.NewEditIndex;
            _gridView.EditIndex = e.NewEditIndex;
            BindMedicalVaccination();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  

    
    private bool IsValidMedicalVaccination(string vaccinationid, string dateofissue, string dateofexpiry, string cssclass)
    {
        Int16 resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!Int16.TryParse(vaccinationid, out resultInt))
            ucError.ErrorMessage = "Vaccination is required";

        if (string.IsNullOrEmpty(dateofissue))
            ucError.ErrorMessage = "Issued is required.";

        else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issued should be earlier than current date";
        }

        if (string.IsNullOrEmpty(dateofexpiry) && cssclass != "input")
            ucError.ErrorMessage = "Expiry is required.";

        if (dateofissue != null && dateofexpiry != null && cssclass != "input")
        {
            if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry, out resultDate)))
                if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry)))
                    ucError.ErrorMessage = "'Expiry' should be greater than 'Issued'";
        }

        return (!ucError.IsError);
    }
    protected void DisableFlag(object sender, EventArgs e)
    {
        UserControlHard h = sender as UserControlHard;
        GridViewRow row = (GridViewRow)h.Parent.Parent;
        UserControlFlag uc = row.FindControl(row.RowType == DataControlRowType.Footer ? "ucFlagAdd" : "ucFlagEdit") as UserControlFlag;
        UserControlHard status = row.FindControl(row.RowType == DataControlRowType.Footer ? "ddlStatusAdd" : "ddlStatus") as UserControlHard;
        if (h.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 95, "FLM"))
        {
            uc.Enabled = true;
            status.CssClass = "input";
        }
        else
        {
            uc.SelectedFlag = string.Empty;
            uc.Enabled = false;
            status.CssClass = "input_mandatory";
        }
    }
    protected void Status_OnTextChangedEvent(object sender, EventArgs e)
    {
        UserControlHard h = sender as UserControlHard;
           


        if (h.Parent.Parent.ToString().Contains("GridFooterItem"))
        {
            GridFooterItem footerItem = (GridFooterItem)h.Parent.Parent;
            
            RadTextBox txtremarks = footerItem.FindControl("txtRemarksAdd") as RadTextBox;//: "txtRemarksEdit") as TextBox;
            UserControlDate txtdateofexpiry = footerItem.FindControl("txtExpiryDateAdd") as UserControlDate;// : "txtExpiryDateEdit") as UserControlDate;
            UserControlHard status = footerItem.FindControl("ddlStatusAdd") as UserControlHard;// : "ddlStatus") as UserControlHard;

            if (h.SelectedHard == ViewState["UNFIT"].ToString() || h.SelectedHard == ViewState["TEMPFIT"].ToString())
            {
                txtdateofexpiry.CssClass = "gridinput";
                txtremarks.CssClass = "gridinput_mandatory";
            }
            else
            {
                txtdateofexpiry.CssClass = "gridinput_mandatory";
                txtremarks.CssClass = "gridinput";

            }
        }
        if (h.Parent.Parent.ToString().Contains("GridDataItem"))
        {
            GridDataItem footerItem = (GridDataItem)h.Parent.Parent;
            RadTextBox txtremarks = footerItem.FindControl("txtRemarksEdit") as RadTextBox;//: "txtRemarksEdit") as TextBox;
                UserControlDate txtdateofexpiry = footerItem.FindControl("txtExpiryDateEdit") as UserControlDate;// : "txtExpiryDateEdit") as UserControlDate;
                UserControlHard status = footerItem.FindControl("ddlStatus") as UserControlHard;// : "ddlStatus") as UserControlHard;

                if (h.SelectedHard == ViewState["UNFIT"].ToString() || h.SelectedHard == ViewState["TEMPFIT"].ToString())
                {
                    txtdateofexpiry.CssClass = "gridinput";
                    txtremarks.CssClass = "gridinput_mandatory";
                }
                else
                {
                    txtdateofexpiry.CssClass = "gridinput_mandatory";
                    txtremarks.CssClass = "gridinput";

                }
           
        }
    }

    protected void ucVaccination_TextChangedEvent(object sender, EventArgs e)
    {
        UserControlDocumentVaccination uc = sender as UserControlDocumentVaccination;

        GridFooterItem row = (GridFooterItem)uc.Parent.Parent;
        int documentId = ((UserControlDocumentVaccination)row.FindControl("ucVaccinationAdd")).SelectedValue;
        UserControlDate txtExpiryDateAdd = row.FindControl("txtExpiryDateAdd") as UserControlDate;
        DataSet ds = PhoenixRegistersDocumentMedical.ListDocumentVaccination(documentId);
        if (ds.Tables[0].Rows[0]["FLDEXPIRYYN"].ToString() == "0")
            txtExpiryDateAdd.CssClass = "input";
        else
            txtExpiryDateAdd.CssClass = "input_mandatory";

    }

    protected void gvCrewMedical_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMedicalData();
    }

    protected void gvCrewMedical_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
                && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                if (db1 != null) db1.Visible = SessionUtil.CanAccess(this.ViewState, db1.CommandName);
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
                if (att != null) att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

                HtmlGenericControl html2 = new HtmlGenericControl();
                if (lblIsAtt != null && lblIsAtt.Text == string.Empty)
                {
                    html2 = new HtmlGenericControl();
                    html2.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html2);
                }
                //if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=NMEDICALDOCUPLOAD'); return false;");

                RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
                System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
                DateTime? d = General.GetNullableDateTime(expdate.Text);

                if (d.HasValue && imgFlag != null)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        //e.Item.CssClass = "rowyellow";

                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        //e.Item.CssClass = "rowred";
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                        imgFlag.Visible = true;
                    }
                }
                RadLabel l = (RadLabel)e.Item.FindControl("lblMedicalId");
                LinkButton dbedit = (LinkButton)e.Item.FindControl("cmdEdit");
                dbedit.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Crew/CrewMedicalAdd.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&CREWFLAGMEDICALID=" + l.Text + "');return false;");

            }
            else
            {
                e.Item.Attributes["onclick"] = "";
            }
            DataRowView drv = (DataRowView)e.Item.DataItem;
            UserControlFlag ucFlag = (UserControlFlag)e.Item.FindControl("ucFlagEdit");
            if (ucFlag != null) ucFlag.SelectedFlag = drv["FLDFLAGID"].ToString();

            UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ucMedicalEdit");
            if (ucHard != null) ucHard.SelectedHard = drv["FLDMEDICALID"].ToString();

            UserControlHard ucStatus = (UserControlHard)e.Item.FindControl("ddlStatus");
            if (ucStatus != null)
            {
                ucStatus.SelectedHard = drv["FLDSTATUS"].ToString();
                DataSet ds = PhoenixRegistersHard.EditHardCode(1, 95, "FLM");
                if (drv["FLDMEDICALID"].ToString() == ds.Tables[0].Rows[0]["FLDHARDCODE"].ToString())
                {
                    ucStatus.CssClass = "input";
                }
                else
                {
                    ucStatus.CssClass = "input_mandatory";
                }

            }
        }
        if (e.Item is GridFooterItem)
        {
            ImageButton ad = (ImageButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
        }
    }

    protected void gvCrewMedical_ItemCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToString().ToUpper() == "MEDICALARCHIVE")
            {
                string crewflagmedicalid = ((RadLabel)e.Item.FindControl("lblMedicalId")).Text;
                PhoenixCrewMedicalDocuments.ArchiveCrewMedical(int.Parse(crewflagmedicalid), 0);

                gvCrewMedical.Rebind();

            }
            else if (e.CommandName.ToString().ToUpper() == "EDIT")
            {

                gvCrewMedical.Rebind();
                DisableFlag((UserControlHard)e.Item.FindControl("ucMedicalEdit"), null);
            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALDELETE")
            {
                string medicalid = ((RadLabel)e.Item.FindControl("lblMedicalId")).Text;

                PhoenixCrewMedicalDocuments.DeleteCrewFlagMedical(
                    Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                    , Convert.ToInt32(medicalid)
                );

                gvCrewMedical.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvMedicalTest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMedicalTestData();
    }

    protected void gvMedicalTest_ItemCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            if (e.Item is GridFooterItem)
            {
                string medicaltestid = ((UserControlDocuments)e.Item.FindControl("ucMedicalTestAdd")).SelectedDocument;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueAdd")).Text;
                string dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd")).Text;
                string status = ((UserControlHard)e.Item.FindControl("ddlStatusAdd")).SelectedHard;
                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text;
                if (!IsValidMedicalTest(medicaltestid, dateofissue, dateofexpiry, status, remarks))
                {

                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewMedicalDocuments.InsertCrewMedicalTest(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                    , Convert.ToInt32(medicaltestid)
                    , placeofissue
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry)
                    , General.GetNullableInteger(status)
                    , General.GetNullableString(remarks)
                    );

                gvMedicalTest.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALTESTARCHIVE")
            {
                string medicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestId")).Text;
                PhoenixCrewMedicalDocuments.ArchiveCrewMedicalTest(int.Parse(medicaltestid), 0);

                gvMedicalTest.Rebind();

            }

            else if (e.CommandName.ToString().ToUpper() == "MEDICALTESTDELETE")
            {
                string medicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestId")).Text;

                PhoenixCrewMedicalDocuments.DeleteCrewMedicalTest(
                    Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                    , Convert.ToInt32(medicaltestid)
                                                                );
                gvMedicalTest.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {
                string medicaltestid = ((UserControlDocuments)e.Item.FindControl("ucMedicalTestEdit")).SelectedDocument;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
                string dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit")).Text;
                string crewmedicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestIdEdit")).Text;
                string status = ((UserControlHard)e.Item.FindControl("ddlStatusEdit")).SelectedHard;
                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text;

                if (!IsValidMedicalTest(medicaltestid, dateofissue, dateofexpiry, status, remarks))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewMedicalDocuments.UpdateCrewMedicalTest(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(crewmedicaltestid)
                    , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                    , Convert.ToInt32(medicaltestid)
                    , placeofissue
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry)
                    , General.GetNullableInteger(status)
                    , General.GetNullableString(remarks)
                    );


                gvMedicalTest.Rebind();

            }

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    protected void gvMedicalTest_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
                && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
                if(db1!=null)db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                if (db1 != null) db1.Visible = SessionUtil.CanAccess(this.ViewState, db1.CommandName);
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
                if (att != null) att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
                
                if (lblIsAtt!=null && lblIsAtt.Text == string.Empty && att != null)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = " <span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                //att.ImageUrl = Session["images"] + "/no-attachment.png";

                if (att != null)
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                      + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=NMEDICALTESTUPLOAD'); return false;");
                }

                RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
                System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
                DateTime? d = null;
                if(expdate != null) d = General.GetNullableDateTime(expdate.Text);
               
                if (d.HasValue && imgFlag !=null)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        //e.Item.CssClass = "rowyellow";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        //e.Item.CssClass = "rowred";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
            }
            else
            {
                e.Item.Attributes["onclick"] = "";
            }
            UserControlDocuments ucMedicaltest = (UserControlDocuments)e.Item.FindControl("ucMedicalTestEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucMedicaltest != null) ucMedicaltest.SelectedDocument = drv["FLDMEDICALTESTID"].ToString();

            UserControlHard ucStatus = (UserControlHard)e.Item.FindControl("ddlStatusEdit");
            if (ucStatus != null) ucStatus.SelectedHard = drv["FLDSTATUS"].ToString();
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
        }
    }

    protected void gvVaccination_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMedicalVaccination();
    }

    protected void gvVaccination_ItemCommand(object sender, GridCommandEventArgs e)
    {
       
        try
        {
            if (e.CommandName.ToString().ToUpper() == "VACCINATIONADD")
            {
  

                string vaccinationid = ((UserControlDocumentVaccination)e.Item.FindControl("ucVaccinationAdd")).SelectedDocument;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueAdd")).Text;
                string dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd")).Text;
                string cssclass = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd")).CssClass;

                if (!IsValidMedicalVaccination(vaccinationid, dateofissue, dateofexpiry, cssclass))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewMedicalDocuments.InsertCrewVaccination(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                    , Convert.ToInt32(vaccinationid)
                    , placeofissue
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry)
                    );

                gvVaccination.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "VACCINATIONARCHIVE")
            {
                string vaccinationid = ((RadLabel)e.Item.FindControl("lblCrewVaccinationId")).Text;
                PhoenixCrewMedicalDocuments.ArchiveCrewVaccination(int.Parse(vaccinationid), 0);

                gvVaccination.Rebind();

            }
       
            else if (e.CommandName.ToString().ToUpper() == "VACCINATIONDELETE")
            {
                string crewvaccinationid = ((RadLabel)e.Item.FindControl("lblCrewVaccinationId")).Text;

                PhoenixCrewMedicalDocuments.DeleteCrewVaccination(
                    Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                    , Convert.ToInt32(crewvaccinationid)
                );
                gvVaccination.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {
                //string vaccinationid = ((UserControlDocumentVaccination)e.Item.FindControl("ucVaccinationEdit")).SelectedDocument;
                string vaccinationid = ((HiddenField)e.Item.FindControl("hdnVaccinationIdEdit")).Value;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
                string dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit")).Text;
                string crewvaccinationid = ((RadLabel)e.Item.FindControl("lblCrewVaccinationIdEdit")).Text;
                string cssClass = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit")).CssClass;

                if (!IsValidMedicalVaccination(vaccinationid, dateofissue, dateofexpiry, cssClass))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewMedicalDocuments.UpdateCrewVaccination(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(crewvaccinationid)
                    , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                    , Convert.ToInt32(vaccinationid)
                    , placeofissue
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry)
                    );


                gvVaccination.Rebind();
            }
           

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvVaccination_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
                && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
                if(db1!=null)db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                if (db1 != null) db1.Visible = SessionUtil.CanAccess(this.ViewState, db1.CommandName);
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
                if (att != null) att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
                HtmlGenericControl html = new HtmlGenericControl();
                if (lblIsAtt != null && lblIsAtt.Text == string.Empty && att != null)
                {               
                    html.InnerHtml = " <span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                if (att != null)
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=NVACCINATIONUPLOAD'); return false;");
                }

                RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
                System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
                DateTime? d = null;
                if(expdate != null) d = General.GetNullableDateTime(expdate.Text);
                if (d.HasValue && imgFlag !=null)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        //e.Item.CssClass = "rowyellow";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        //e.Item.CssClass = "rowred";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
            }
            else
            {
                e.Item.Attributes["onclick"] = "";
            }
            //UserControlDocumentVaccination ucDocumentVaccination = (UserControlDocumentVaccination)e.Item.FindControl("ucVaccinationEdit");
            RadLabel lblDocumentVaccination = (RadLabel)e.Item.FindControl("lblVaccinationNameEdit");
            HiddenField lblDocumentVaccinationId = (HiddenField)e.Item.FindControl("hdnVaccinationIdEdit");

            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (lblDocumentVaccination != null)
                lblDocumentVaccination.Text = drv["FLDNAMEOFMEDICAL"].ToString();
            if (lblDocumentVaccinationId != null)
                lblDocumentVaccinationId.Value = drv["FLDVACCINATIONID"].ToString();
            UserControlDate ucexpiryEdit = (UserControlDate)e.Item.FindControl("txtExpiryDateEdit");
            // HiddenField hdnexpiryyn = (HiddenField)e.Item.FindControl("hdnExpiryyn");
            if (ucexpiryEdit != null)
            {
                if (drv["FLDEXPIRYYN"].ToString() == "0")
                {
                    ucexpiryEdit.CssClass = "input";
                }
                else
                {
                    ucexpiryEdit.CssClass = "input_mandatory";
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
        }

    }
}

