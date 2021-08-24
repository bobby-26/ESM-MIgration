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

public partial class CrewFamilyMedicalDocument : PhoenixBasePage
{
    string strFamilyId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            strFamilyId = Request.QueryString["familyid"];

            if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
            {
                Filter.CurrentCrewSelection = Request.QueryString["empid"].ToString();
            }
            SessionUtil.PageAccessRights(this.ViewState);
            int? vesselid = null;
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                SetEmployeePrimaryDetails();
                AutoCorrectMedicalStatus();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["FLM"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 95, "FLM");
                ViewState["HAVINGEXPIRY"] = null;
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Travel", "DOCUMENTS");
            toolbar.AddButton("Medical", "MEDICAL");
            toolbar.AddButton("Other", "OTHERDOCUMENT");
            ucsubtitle.MenuList = toolbar.Show();
            ucsubtitle.SelectedMenuIndex = 1;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Family NOK", "FAMILYNOK");
            toolbar.AddButton("Sign On/Off", "SIGNON");
            toolbar.AddButton("Documents", "DOCUMENTS");
            toolbar.AddButton("Travel", "TRAVEL");
            CrewFamilyTabs.AccessRights = this.ViewState;
            CrewFamilyTabs.MenuList = toolbar.Show();
            CrewFamilyTabs.SelectedMenuIndex = 2;

            SetMedicalRequest(ref vesselid, null);

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewFamilyMedicalDocument.aspx?familyid=" + strFamilyId, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "MedicalExcel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewMedical')", "Print Grid", "<i class=\"fas fa-print\"></i>", "MedicalPRINT");
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewMedicalArchived.aspx?empid=" + Filter.CurrentCrewSelection + "&type=1&familyid=" + strFamilyId + "'); return false;", "Show Archived", "<i class=\"fas fa-archive\"></i>", "ARCHIVED");
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewMedicalSlip.aspx?empid=" + Filter.CurrentCrewSelection + "&familyid=" + strFamilyId + "&vslid=" + vesselid + "'); return false;", "Initiate Medical Request", "<i class=\"fas fa-briefcase-medical\"></i>", "MEDICALREQUEST");
            MenuCrewMedical.AccessRights = this.ViewState;
            MenuCrewMedical.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewFamilyMedicalDocument.aspx?familyid=" + strFamilyId, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "MedicalTestExcel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMedicalTest')", "Print Grid", "<i class=\"fas fa-print\"></i>", "MedicalTestPRINT");
            CrewMedicalTest.AccessRights = this.ViewState;
            CrewMedicalTest.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewFamilyMedicalDocument.aspx?familyid=" + strFamilyId, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "VaccinationExcel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVaccination')", "Print Grid", "<i class=\"fas fa-print\"></i>", "VaccinationPRINT");
            CrewVaccination.AccessRights = this.ViewState;
            CrewVaccination.MenuList = toolbar.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewFamilyMedical_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("MEDICAL"))
            {
                Response.Redirect("CrewFamilyMedicalDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTS"))
            {
                Response.Redirect("CrewFamilyTravelDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("OTHERDOCUMENT"))
            {
                Response.Redirect("CrewFamilyOtherDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void AutoCorrectMedicalStatus()
    {
        PhoenixCrewMedicalDocuments.AutoCorrectCrewMedicalStatus(Int32.Parse(Filter.CurrentCrewSelection.ToString()), General.GetNullableInteger(Request.QueryString["familyid"]));
    }

    protected void SetMedicalRequest(ref int? vesselid, int? lftreqyn)
    {
        DataSet ds = PhoenixCrewMedicalDocuments.ListCrewCourseRequest(Int32.Parse(Filter.CurrentCrewSelection), lftreqyn);

        if (ds.Tables[0].Rows[0]["FLDVESSELID"].ToString() != "")
        {
            vesselid = Convert.ToInt32(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());

        }
    }

    protected void CrewFamilyTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FAMILYNOK"))
            {
                Response.Redirect("CrewFamilyNok.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("SIGNON"))
            {
                Response.Redirect("CrewFamilySignOn.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("TRAVEL"))
            {
                Response.Redirect("CrewFamilyTravel.aspx?familyid=" + Request.QueryString["familyid"] + "&from=familynok", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindMedicalData();
        BindMedicalTestData();
        BindMedicalVaccination();

    }
    protected void gvCrewMedical_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMedicalData();
    }

    private void BindMedicalData()
    {
        try
        {
            string[] alColumns = { "FLDMEDICALNAME", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDCOUNTRYNAME", "FLDSTATUSNAME" };
            string[] alCaptions = { "Medical", "Place of Issue", "Issued", "Expiry", "Flag", "Status" };
            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewFlagMedical(
                        Int32.Parse(Filter.CurrentCrewSelection), 1, General.GetNullableInteger(strFamilyId));

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
                        Int32.Parse(Filter.CurrentCrewSelection), 1, General.GetNullableInteger(strFamilyId));

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

    protected void gvCrewMedical_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "MEDICALADD")
            {

                string medicalid = ((UserControlHard)e.Item.FindControl("ucMedicalAdd")).SelectedHard;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueAdd")).Text;
                string dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd")).Text;
                string flagid = ((UserControlFlag)e.Item.FindControl("ucFlagAdd")).SelectedFlag;
                string status = ((UserControlHard)e.Item.FindControl("ddlStatusAdd")).SelectedHard;

                if (!IsValidMedical(medicalid, dateofissue, dateofexpiry, flagid, placeofissue, status))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewMedicalDocuments.InsertCrewFlagMedicalFamily(
                     Convert.ToInt32(Filter.CurrentCrewSelection)
                    , Convert.ToInt32(medicalid)
                    , placeofissue
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry)
                    , General.GetNullableInteger(flagid)
                    , int.Parse(strFamilyId)
                    , int.Parse(status)
                    );

                BindMedicalData();
                gvCrewMedical.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALARCHIVE")
            {
                string crewflagmedicalid = ((RadLabel)e.Item.FindControl("lblMedicalId")).Text;
                PhoenixCrewMedicalDocuments.ArchiveCrewMedical(int.Parse(crewflagmedicalid), 0);

                BindMedicalData();
                gvCrewMedical.Rebind();

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

            else if (e.CommandName.ToString().ToUpper() == "MEDICALCANCEL")
            {
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


    protected void gvCrewMedical_EditCommand(object sender, GridCommandEventArgs e)
    {
        //DisableFlag((UserControlHard)e.Item.FindControl("ucMedicalEdit"), null);
    }

    protected void gvCrewMedical_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string medicalid = ((UserControlHard)e.Item.FindControl("ucMedicalEdit")).SelectedHard;
            string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
            string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
            string dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit")).Text;
            string flagid = ((UserControlFlag)e.Item.FindControl("ucFlagEdit")).SelectedFlag;
            string flagmedicalid = ((RadLabel)e.Item.FindControl("lblMedicalIdEdit")).Text;
            string status = ((UserControlHard)e.Item.FindControl("ddlStatus")).SelectedHard;

            if (!IsValidMedical(medicalid, dateofissue, dateofexpiry, flagid, placeofissue, status))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }

            PhoenixCrewMedicalDocuments.UpdateCrewFlagMedicalFamily(Convert.ToInt32(flagmedicalid)
                     , Convert.ToInt32(Filter.CurrentCrewSelection)
                     , Convert.ToInt32(medicalid)
                     , placeofissue
                     , General.GetNullableDateTime(dateofissue)
                     , General.GetNullableDateTime(dateofexpiry)
                     , General.GetNullableInteger(flagid)
                     , int.Parse(status)
                     , int.Parse(strFamilyId)
                     );

            BindMedicalData();
            gvCrewMedical.Rebind();
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
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
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=FMEDICALUPLOAD'); return false;");
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
            if (ucHard != null)
            {
                ucHard.SelectedHard = drv["FLDMEDICALID"].ToString();

                if (drv["FLDMEDICALID"].ToString() == ViewState["FLM"].ToString())
                {
                    ucFlag.Enabled = true;
                }
                else
                {
                    ucFlag.Enabled = false;
                }
            }

            UserControlHard ucStatus = (UserControlHard)e.Item.FindControl("ddlStatus");
            if (ucStatus != null) ucStatus.SelectedHard = drv["FLDSTATUS"].ToString();

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

    protected void gvCrewMedical_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.SelectedIndex = e.NewSelectedIndex;
        _gridView.EditIndex = -1;
        BindMedicalData();
    }

    protected void gvCrewMedical_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    private bool IsValidMedical(string medicalid, string dateofissue, string dateofexpiry, string flagid, string placeofissue, string status)
    {
        Int16 resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        DataSet ds = PhoenixRegistersHard.EditHardCode(1, 95, "FLM");

        if (ds.Tables[0].Rows.Count > 0)
            if (medicalid == ds.Tables[0].Rows[0]["FLDHARDCODE"].ToString())
                if (General.GetNullableInteger(flagid) == null)
                    ucError.ErrorMessage = "Flag is required for Flag Medical.";

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
            ucError.ErrorMessage = "Issued should be greater than Last Signoff Date";
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
        if (!Int16.TryParse(status, out resultInt))
            ucError.ErrorMessage = "Status is required";
        return (!ucError.IsError);
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewFamilyNok.ListEmployeeFamily(Convert.ToInt32(Filter.CurrentCrewSelection), General.GetNullableInteger(strFamilyId));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                ViewState["LASTSIGNOFF"] = dt.Rows[0]["FLDLASTSIGNOFFDATE"].ToString();
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

    protected void CrewMedicalTest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("MEDICALTESTEXCEL"))
            {
                string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE" };
                string[] alCaptions = { "Medical Test", "Place of Issue", "Issued", "Expiry" };

                DataSet ds = PhoenixCrewMedicalDocuments.ListCrewMedicalTest(
                        Int32.Parse(Filter.CurrentCrewSelection), 1, General.GetNullableInteger(strFamilyId));

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


    protected void gvMedicalTest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMedicalTestData();
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
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=FMEDICALTESTUPLOAD'); return false;");
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

                if (!IsValidMedicalTest(medicaltestid, dateofissue, dateofexpiry))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewMedicalDocuments.InsertCrewMedicalTestFamily(Convert.ToInt32(Filter.CurrentCrewSelection)
                    , Convert.ToInt32(medicaltestid)
                    , placeofissue
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry)
                    , int.Parse(strFamilyId)
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
            else if (e.CommandName.ToString().ToUpper() == "MEDICALTESTDELETE")
            {
                string medicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestId")).Text;

                PhoenixCrewMedicalDocuments.DeleteCrewMedicalTest(
                    Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                    , Convert.ToInt32(medicaltestid));
                BindMedicalTestData();
                gvMedicalTest.Rebind();
            }

            //else if (e.CommandName.ToString().ToUpper() == "MEDICALTESTEDIT")
            //{
            //    _gridView.SelectedIndex = nCurrentRow;
            //    _gridView.EditIndex = nCurrentRow;
            //    BindMedicalTestData();
            //}

            //else if (e.CommandName.ToString().ToUpper() == "MEDICALTESTCANCEL")
            //{
            //    _gridView.EditIndex = -1;
            //    BindMedicalTestData();
            //}
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

            if (!IsValidMedicalTest(medicaltestid, dateofissue, dateofexpiry))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }

            PhoenixCrewMedicalDocuments.UpdateCrewMedicalTestFamily(
                 Convert.ToInt32(crewmedicaltestid)
                , Convert.ToInt32(Filter.CurrentCrewSelection)
                , Convert.ToInt32(medicaltestid)
                , placeofissue
                , General.GetNullableDateTime(dateofissue)
                , General.GetNullableDateTime(dateofexpiry)
                , int.Parse(strFamilyId)
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

    protected void gvMedicalTest_EditCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvMedicalTest_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }

    private void BindMedicalTestData()
    {
        try
        {
            string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE" };
            string[] alCaptions = { "Medical Test", "Place of Issue", "Issued", "Expiry" };

            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewMedicalTest(
                        Int32.Parse(Filter.CurrentCrewSelection), 1, General.GetNullableInteger(strFamilyId));

            General.SetPrintOptions("gvMedicalTest", "Medical Test", alCaptions, alColumns, ds);

            gvMedicalTest.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidMedicalTest(string medicaltestid, string dateofissue, string dateofexpiry)
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
                        Int32.Parse(Filter.CurrentCrewSelection), 1, General.GetNullableInteger(strFamilyId));

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


    protected void gvVaccination_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMedicalVaccination();
    }


    private void BindMedicalVaccination()
    {
        try
        {
            string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE" };
            string[] alCaptions = { "Vaccination", "Place of Issue", "Issued", "Expiry" };

            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewVaccination(
                        Int32.Parse(Filter.CurrentCrewSelection), 1, General.GetNullableInteger(strFamilyId));

            General.SetPrintOptions("gvVaccination", "Vaccination", alCaptions, alColumns, ds);

            gvVaccination.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
                
                if (!IsValidMedicalVaccination(vaccinationid, dateofissue, dateofexpiry))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewMedicalDocuments.InsertCrewVaccinationFamily(
                     Convert.ToInt32(Filter.CurrentCrewSelection)
                    , Convert.ToInt32(vaccinationid)
                    , placeofissue
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry)
                    , int.Parse(strFamilyId)
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


    protected void gvVaccination_UpdateCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            string vaccinationid = ((HiddenField)e.Item.FindControl("hdnVaccinationIdEdit")).Value;
            string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
            string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
            string dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit")).Text;
            string crewvaccinationid = ((RadLabel)e.Item.FindControl("lblCrewVaccinationIdEdit")).Text;            

            if (!IsValidMedicalVaccination(vaccinationid, dateofissue, dateofexpiry))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }

            PhoenixCrewMedicalDocuments.UpdateCrewVaccinationFamily(
                 Convert.ToInt32(crewvaccinationid)
                , Convert.ToInt32(Filter.CurrentCrewSelection)
                , Convert.ToInt32(vaccinationid)
                , placeofissue
                , General.GetNullableDateTime(dateofissue)
                , General.GetNullableDateTime(dateofexpiry)
                , int.Parse(strFamilyId)
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

    protected void gvVaccination_SortCommand(object sender, GridSortCommandEventArgs e)
    {

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
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=FVACCINATIONUPLOAD'); return false;");
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
                    ucexpiryEdit.CssClass = "";
                    ViewState["HAVINGEXPIRY"] = 0;
                }
                else
                {
                    ucexpiryEdit.CssClass = "input_mandatory";
                    ViewState["HAVINGEXPIRY"] = 1;
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

    protected void gvVaccination_EditCommand(object sender, GridCommandEventArgs e)
    {

    }

    private bool IsValidMedicalVaccination(string vaccinationid, string dateofissue, string dateofexpiry)
    {
        Int16 resultInt;
        DateTime resultDate;

        string HavingExpiry;
        HavingExpiry =  (ViewState["HAVINGEXPIRY"] == null) ? null : (ViewState["HAVINGEXPIRY"].ToString());

        ucError.HeaderMessage = "Please provide the following required information";

        if (!Int16.TryParse(vaccinationid, out resultInt))
            ucError.ErrorMessage = "Vaccination is required";

        if (string.IsNullOrEmpty(dateofissue))
            ucError.ErrorMessage = "Issued is required.";

        else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issued should be earlier than current date";
        }

        if (string.IsNullOrEmpty(dateofexpiry) && HavingExpiry == "1")
            ucError.ErrorMessage = "Expiry is required.";

        if (dateofissue != null && dateofexpiry != null && HavingExpiry == "1")
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


        if (h != null && h.ID == "ucMedicalAdd")
        {
            GridFooterItem row = (GridFooterItem)h.NamingContainer;
            flag = (UserControlFlag)row.FindControl("ucFlagAdd");
        }
        else
        {
            GridDataItem row = (GridDataItem)h.NamingContainer;

            flag = (UserControlFlag)row.FindControl("ucFlagEdit");
        }

        if (h.SelectedHard == ViewState["FLM"].ToString())
        {
            flag.Enabled = true;
        }
        else
        {
            flag.SelectedFlag = string.Empty;
            flag.Enabled = false;
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
        {
            txtExpiryDateAdd.CssClass = "";
            ViewState["HAVINGEXPIRY"] = 0;
        }
        else
        {
            txtExpiryDateAdd.CssClass = "input_mandatory";
            ViewState["HAVINGEXPIRY"] = 1;
        }
    }




}
