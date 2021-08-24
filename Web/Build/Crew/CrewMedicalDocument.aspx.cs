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

public partial class CrewMedicalDocument : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmenu = new PhoenixToolbar();
            CrewMedical.AccessRights = this.ViewState;
            CrewMedical.MenuList = toolbarmenu.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            int? vesselid = null;
            toolbar.AddFontAwesomeButton("../Crew/CrewMedicalDocument.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "MedicalExcel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewMedical')", "Print Grid", "<i class=\"fas fa-print\"></i>", "MedicalPRINT");
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewMedicalArchived.aspx?empid=" + Filter.CurrentCrewSelection + "&type=1'); return false;", "Show Archived", "<i class=\"fas fa-archive\"></i>", "ARCHIVED");
            SetCourseRequest(ref vesselid, null);
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewMedicalAdd.aspx?empid=" + Filter.CurrentCrewSelection + "')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDMEDICAL");
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewMedicalSlip.aspx?empid=" + Filter.CurrentCrewSelection + "&vslid=" + vesselid + "&type=1'); return false;", "Medical Request", "<i class=\"fas fa-briefcase-medical\"></i>", "MEDICALREQUEST");
            MenuCrewMedical.AccessRights = this.ViewState;
            MenuCrewMedical.MenuList = toolbar.Show();
         
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewMedicalDocument.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "MedicalTestExcel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMedicalTest')", "Print Grid", "<i class=\"fas fa-print\"></i>", "MedicalTestPRINT");
            CrewMedicalTest.AccessRights = this.ViewState;
            CrewMedicalTest.MenuList = toolbar.Show();
            
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewMedicalDocument.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "VaccinationExcel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVaccination')", "Print Grid", "<i class=\"fas fa-print\"></i>", "VaccinationPRINT");
            CrewVaccination.AccessRights = this.ViewState;
            CrewVaccination.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                AutoArchive();
                AutoCorrectMedicalStatus();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["UNFIT"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "UFT");
                ViewState["TEMPFIT"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "TUF");
                ViewState["FLM"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 95, "FLM");

                SetEmployeePrimaryDetails();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void AutoArchive()
    {
        PhoenixCrewMedicalDocuments.AutoArchiveCewMedical(Int32.Parse(Filter.CurrentCrewSelection.ToString()));
    }

    private void AutoCorrectMedicalStatus()
    {
        PhoenixCrewMedicalDocuments.AutoCorrectCrewMedicalStatus(Int32.Parse(Filter.CurrentCrewSelection.ToString()), null);
    }

    protected void SetCourseRequest(ref int? vesselid, int? lftreqyn)
    {
        DataSet ds = PhoenixCrewMedicalDocuments.ListCrewCourseRequest(Int32.Parse(Filter.CurrentCrewSelection), lftreqyn);

        if (ds.Tables[0].Rows[0]["FLDVESSELID"].ToString() != "")
        {
            vesselid = Convert.ToInt32(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());

        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindMedicalData();
        BindMedicalTestData();
        BindMedicalVaccination();

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
                        Int32.Parse(Filter.CurrentCrewSelection.ToString()), 1, null);

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


    private void BindMedicalData()
    {
        try
        {
            string[] alColumns = { "FLDMEDICALNAME", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDCOUNTRYNAME", "FLDSTATUSNAME" };
            string[] alCaptions = { "Medical", "Place of Issue", "Issued", "Expiry", "Flag", "Status" };

            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewFlagMedical(
                        Int32.Parse(Filter.CurrentCrewSelection.ToString()), 1, null);

            General.SetPrintOptions("gvCrewMedical", "Medical", alCaptions, alColumns, ds);

            gvCrewMedical.DataSource = ds;            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrewMedical_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMedicalData();
    }

    protected void gvCrewMedical_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "MEDICALARCHIVE")
            {
                string crewflagmedicalid = ((RadLabel)e.Item.FindControl("lblMedicalId")).Text;
                PhoenixCrewMedicalDocuments.ArchiveCrewMedical(int.Parse(crewflagmedicalid), 0);
                BindMedicalData();
                gvCrewMedical.Rebind();

            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALEDIT")
            {
                BindMedicalData();
                DisableFlag((UserControlHard)e.Item.FindControl("ucMedicalEdit"), null);
            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALDELETE")
            {
                string medicalid = ((RadLabel)e.Item.FindControl("lblMedicalId")).Text;

                PhoenixCrewMedicalDocuments.DeleteCrewFlagMedical(
                    Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                    , Convert.ToInt32(medicalid));
                BindMedicalData();
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

    protected void gvCrewMedical_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            RadLabel l = (RadLabel)e.Item.FindControl("lblMedicalId");
            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
                cme.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Crew/CrewMedicalAdd.aspx?empid=" + Filter.CurrentCrewSelection + "&CREWFLAGMEDICALID=" + l.Text + "');return false;");
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=MEDICALUPLOAD'); return false;");
            }


            RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;

            if (expdate != null)
            {
                DateTime? d = General.GetNullableDateTime(expdate.Text);
                if (d.HasValue)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
            }


        }

        if (e.Item.IsInEditMode)
        {
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
                    ucStatus.CssClass = "";
                }
                else
                {
                    ucStatus.CssClass = "input_mandatory";
                }
            }
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }

    }
    protected void gvCrewMedical_EditCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            BindMedicalData();
            DisableFlag((UserControlHard)e.Item.FindControl("ucMedicalEdit"), null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewMedical_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string medicalid = ((RadLabel)e.Item.FindControl("lblMedicalId")).Text;

            PhoenixCrewMedicalDocuments.DeleteCrewFlagMedical(
                Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                , Convert.ToInt32(medicalid)
            );
            BindMedicalData();
            gvCrewMedical.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewMedical_SortCommand(object sender, GridSortCommandEventArgs e)
    {

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

        if (ViewState["LASTSIGNOFF"].ToString() != string.Empty && (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(ViewState["LASTSIGNOFF"].ToString())) < 0))
        {
            ucError.ErrorMessage = "Issued should be greater than Last SignOff Date";
        }
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
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                ViewState["LASTSIGNOFF"] = dt.Rows[0]["FLDLASTSIGNOFFDATE"].ToString();
            }
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
                        Int32.Parse(Filter.CurrentCrewSelection.ToString()), 1, null);

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


    private void BindMedicalTestData()
    {
        try
        {
            string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDSTATUSNAME", "FLDREMARKS" };
            string[] alCaptions = { "Medical Test", "Place of Issue", "Issued", "Expiry", "Status", "Remarks" };

            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewMedicalTest(
                        Int32.Parse(Filter.CurrentCrewSelection.ToString()), 1, null);

            General.SetPrintOptions("gvMedicalTest", "Medical Test", alCaptions, alColumns, ds);

            gvMedicalTest.DataSource = ds;            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvMedicalTest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMedicalTestData();
    }


    protected void gvMedicalTest_EditCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvMedicalTest_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToString().ToUpper() == "MEDICALTESTADD")
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
                    , Convert.ToInt32(Filter.CurrentCrewSelection)
                    , Convert.ToInt32(medicaltestid)
                    , placeofissue
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry)
                    , General.GetNullableInteger(status)
                    , General.GetNullableString(remarks)
                    );

                BindMedicalTestData();
                gvMedicalTest.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALTESTARCHIVE")
            {
                string medicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestId")).Text;
                PhoenixCrewMedicalDocuments.ArchiveCrewMedicalTest(int.Parse(medicaltestid), 0);

                BindMedicalTestData();
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

    protected void gvMedicalTest_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
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
                ucError.Visible = true;
                return;
            }

            PhoenixCrewMedicalDocuments.UpdateCrewMedicalTest(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , Convert.ToInt32(crewmedicaltestid)
                , Convert.ToInt32(Filter.CurrentCrewSelection)
                , Convert.ToInt32(medicaltestid)
                , placeofissue
                , General.GetNullableDateTime(dateofissue)
                , General.GetNullableDateTime(dateofexpiry)
                , General.GetNullableInteger(status)
                , General.GetNullableString(remarks)
                );

            BindMedicalTestData();
            gvMedicalTest.Rebind();
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMedicalTest_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string medicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestId")).Text;

            PhoenixCrewMedicalDocuments.DeleteCrewMedicalTest(
                Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                , Convert.ToInt32(medicaltestid)
                                                            );
            BindMedicalTestData();
            gvMedicalTest.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvMedicalTest_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=MEDICALTESTUPLOAD'); return false;");
            }

            RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;

            if (expdate != null)
            {
                DateTime? d = General.GetNullableDateTime(expdate.Text);
                if (d.HasValue)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
            }
        }

        if (e.Item.IsInEditMode)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;

            UserControlDocuments ucMedicaltest = (UserControlDocuments)e.Item.FindControl("ucMedicalTestEdit");
            if (ucMedicaltest != null) ucMedicaltest.SelectedDocument = drv["FLDMEDICALTESTID"].ToString();


            UserControlHard ucStatus = (UserControlHard)e.Item.FindControl("ddlStatusEdit");
            if (ucStatus != null)
            {
                ucStatus.HardTypeCode = "105";
                ucStatus.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105);
                if (ucStatus.SelectedHard == "")
                    ucStatus.SelectedHard = drv["FLDSTATUS"].ToString();
            }

        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
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
                        Int32.Parse(Filter.CurrentCrewSelection.ToString()), 1, null);

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

    private void BindMedicalVaccination()
    {
        try
        {
            string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE" };
            string[] alCaptions = { "Vaccination", "Place of Issue", "Issued", "Expiry" };

            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewVaccination(
                        Int32.Parse(Filter.CurrentCrewSelection.ToString()), 1, null);

            General.SetPrintOptions("gvVaccination", "Vaccination", alCaptions, alColumns, ds);

            gvVaccination.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvVaccination_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMedicalVaccination();
    }


    protected void gvVaccination_EditCommand(object sender, GridCommandEventArgs e)
    {

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
                    , Convert.ToInt32(Filter.CurrentCrewSelection)
                    , Convert.ToInt32(vaccinationid)
                    , placeofissue
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry)
                    );

                BindMedicalVaccination();
                gvVaccination.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "VACCINATIONARCHIVE")
            {
                string vaccinationid = ((RadLabel)e.Item.FindControl("lblCrewVaccinationId")).Text;
                PhoenixCrewMedicalDocuments.ArchiveCrewVaccination(int.Parse(vaccinationid), 0);

                BindMedicalVaccination();
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

    protected void gvVaccination_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string vaccinationid = ((HiddenField)e.Item.FindControl("hdnVaccinationIdEdit")).Value;
            string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
            string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
            string dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit")).Text;
            string crewvaccinationid = ((HiddenField)e.Item.FindControl("hdnCrewVaccinationId")).Value;
            string cssClass = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit")).CssClass;

            if (!IsValidMedicalVaccination(vaccinationid, dateofissue, dateofexpiry, cssClass))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewMedicalDocuments.UpdateCrewVaccination(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , Convert.ToInt32(crewvaccinationid)
                , Convert.ToInt32(Filter.CurrentCrewSelection)
                , Convert.ToInt32(vaccinationid)
                , placeofissue
                , General.GetNullableDateTime(dateofissue)
                , General.GetNullableDateTime(dateofexpiry)
                );

            BindMedicalVaccination();
            gvVaccination.Rebind();
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVaccination_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string crewvaccinationid = ((RadLabel)e.Item.FindControl("lblCrewVaccinationId")).Text;

            PhoenixCrewMedicalDocuments.DeleteCrewVaccination(
                Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                , Convert.ToInt32(crewvaccinationid)
            );
            BindMedicalVaccination();
            gvVaccination.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvVaccination_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=VACCINATIONUPLOAD'); return false;");
            }

            RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;

            if (expdate != null)
            {
                DateTime? d = General.GetNullableDateTime(expdate.Text);
                if (d.HasValue)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
            }

        }

        if (e.Item.IsInEditMode)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblDocumentVaccination = (RadLabel)e.Item.FindControl("lblVaccinationNameEdit");
            HiddenField lblDocumentVaccinationId = (HiddenField)e.Item.FindControl("hdnVaccinationIdEdit");

            if (lblDocumentVaccination != null)
                lblDocumentVaccination.Text = drv["FLDNAMEOFMEDICAL"].ToString();
            if (lblDocumentVaccinationId != null)
                lblDocumentVaccinationId.Value = drv["FLDVACCINATIONID"].ToString();

            UserControlDate ucexpiryEdit = (UserControlDate)e.Item.FindControl("txtExpiryDateEdit");
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
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

        }
    }

    protected void gvVaccination_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }


    private bool IsValidMedicalVaccination(string vaccinationid, string dateofissue, string dateofexpiry, string cssClass)
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
        if (string.IsNullOrEmpty(dateofexpiry) && cssClass != "input")
            ucError.ErrorMessage = "Expiry is required.";

        if (dateofissue != null && dateofexpiry != null && cssClass != "input")
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
        UserControlFlag flag;
        UserControlHard status;

        if (h != null)
        {
            if (h.ID == "ucFlagAdd")
            {
                GridFooterItem row = (GridFooterItem)h.NamingContainer;
                flag = (UserControlFlag)row.FindControl("ucFlagAdd");
                status = (UserControlHard)row.FindControl("ddlStatusAdd");
            }
            else
            {
                GridDataItem row = (GridDataItem)h.NamingContainer;
                flag = (UserControlFlag)row.FindControl("ucFlagEdit");
                status = (UserControlHard)row.FindControl("ddlStatus");
            }
            if (h.SelectedHard == ViewState["FLM"].ToString())
            {
                flag.Enabled = true;
            }
            else
            {
                flag.SelectedFlag = string.Empty;
                flag.Enabled = false;
                status.CssClass = "input_mandatory";
            }
        }
    }

    protected void Status_OnTextChangedEvent(object sender, EventArgs e)
    {
        UserControlHard h = sender as UserControlHard;
        RadTextBox txtremarks;
        UserControlHard status;
        UserControlDate txtdateofexpiry;

        if (h != null && (h.ID == "ddlStatusAdd"))
        {
            GridFooterItem row = (GridFooterItem)h.NamingContainer;
            txtremarks = (RadTextBox)row.FindControl("txtRemarksAdd");
            txtdateofexpiry = (UserControlDate)row.FindControl("txtExpiryDateAdd");
            status = (UserControlHard)row.FindControl("ddlStatusAdd");
        }
        else
        {
            GridDataItem row = (GridDataItem)h.NamingContainer;
            txtremarks = (RadTextBox)row.FindControl("txtRemarksEdit");
            txtdateofexpiry = (UserControlDate)row.FindControl("txtExpiryDateEdit");
            status = (UserControlHard)row.FindControl("ddlStatus");
        }

        if (h.SelectedHard == ViewState["UNFIT"].ToString()
            || h.SelectedHard == ViewState["TEMPFIT"].ToString())
        {
            txtremarks.CssClass = "gridinput_mandatory";
        }
        else
        {
            txtdateofexpiry.CssClass = "gridinput_mandatory";
        }
    }

    protected void ucVaccination_TextChangedEvent(object sender, EventArgs e)
    {
        UserControlDocumentVaccination uc = sender as UserControlDocumentVaccination;
        GridFooterItem row = (GridFooterItem)uc.NamingContainer;

        int documentId = ((UserControlDocumentVaccination)row.FindControl("ucVaccinationAdd")).SelectedValue;
        UserControlDate txtExpiryDateAdd = row.FindControl("txtExpiryDateAdd") as UserControlDate;

        DataSet ds = PhoenixRegistersDocumentMedical.ListDocumentVaccination(documentId);

        if (ds.Tables[0].Rows[0]["FLDEXPIRYYN"].ToString() == "0")
            txtExpiryDateAdd.CssClass = "";
        else
            txtExpiryDateAdd.CssClass = "input_mandatory";

    }


}

