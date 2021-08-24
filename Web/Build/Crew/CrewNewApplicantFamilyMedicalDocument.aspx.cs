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

public partial class CrewNewApplicantFamilyMedicalDocument : PhoenixBasePage
{
    string strFamilyId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            strFamilyId = Request.QueryString["familyid"];
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar maintoolbar = new PhoenixToolbar();
            maintoolbar.AddButton("Othr. Document", "OTHERDOCUMENT", ToolBarDirection.Right);
            maintoolbar.AddButton("Trvl Document", "DOCUMENTS", ToolBarDirection.Right);
            maintoolbar.AddButton("Medical", "MEDICAL", ToolBarDirection.Right);
            maintoolbar.AddButton("Family NOK", "FAMILYNOK", ToolBarDirection.Right);
            CrewFamilyMedical.AccessRights = this.ViewState;
            CrewFamilyMedical.MenuList = maintoolbar.Show();
            CrewFamilyMedical.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {
                SetEmployeePrimaryDetails();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["FLM"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 95, "FLM");

            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantFamilyMedicalDocument.aspx?familyid=" + strFamilyId, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "MEDICALEXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewMedical')", "Print Grid", "<i class=\"fas fa-print\"></i>", "MEDICALPRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewMedicalArchived.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&type=1&familyid=" + strFamilyId + "'); return false;", "Show Archived", "<i class=\"fas fa-archive\"></i>", "ARCHIVED");
            //if (ViewState["REQID"].ToString() != string.Empty)
            //{
            //    toolbar.AddImageLink("javascript:parent.Openpopup('NAFA','','../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=MEDICALSLIP&reqid=" + ViewState["REQID"].ToString() + "');return false;", "View Medical Request", "view-medical-request.png", "ARCHIVED");
            //}
            //else
            //{
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewMedicalSlip.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&familyid=" + strFamilyId + "'); return false;", "Initiate Medical Request", "<i class=\"fas fa-briefcase-medical\"></i>", "ARCHIVED");
            // }
            MenuCrewMedical.AccessRights = this.ViewState;
            MenuCrewMedical.MenuList = toolbar.Show();
            //MenuCrewMedical.SetTrigger(pnlCrewMedicalEntry);

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantFamilyMedicalDocument.aspx?familyid=" + strFamilyId, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "MEDICALTESTEXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMedicalTest')", "Print Grid", "<i class=\"fas fa-print\"></i>", "MEDICALTESTPRINT");
            CrewMedicalTest.AccessRights = this.ViewState;
            CrewMedicalTest.MenuList = toolbar.Show();
            //CrewMedicalTest.SetTrigger(pnlCrewMedicalEntry);

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantFamilyMedicalDocument.aspx?familyid=" + strFamilyId, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "VACCINATIONEXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVaccination')", "Print Grid", "<i class=\"fas fa-print\"></i>", "VACCINATIONPRINT");
            CrewVaccination.AccessRights = this.ViewState;
            CrewVaccination.MenuList = toolbar.Show();
            //CrewVaccination.SetTrigger(pnlCrewMedicalEntry);

            BindMedicalTestData();
            BindMedicalVaccination();
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

            if (CommandName.ToUpper().Equals("FAMILYNOK"))
            {
                Response.Redirect("CrewNewApplicantFamilyNok.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("OTHERDOCUMENT"))
            {
                Response.Redirect("CrewNewApplicantFamilyOtherDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTS"))
            {
                Response.Redirect("CrewNewApplicantFamilyTravelDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
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
    private void BindMedicalData()
    {
        try
        {
            string[] alColumns = { "FLDMEDICALNAME", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDCOUNTRYNAME" };
            string[] alCaptions = { "Medical", "Place Of Issue", "Issue Date", "Expiry Date", "Flag" };

            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewFlagMedical(
                        Int32.Parse(Filter.CurrentNewApplicantSelection), 1, General.GetNullableInteger(strFamilyId));

            General.SetPrintOptions("gvCrewMedical", "Crew Medical", alCaptions, alColumns, ds);


            gvCrewMedical.DataSource = ds;
            gvCrewMedical.VirtualItemCount = ds.Tables[0].Rows.Count;
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
                string[] alColumns = { "FLDMEDICALNAME", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDCOUNTRYNAME" };
                string[] alCaptions = { "Medical", "Place Of Issue", "Issue Date", "Expiry Date", "Flag" };

                DataSet ds = PhoenixCrewMedicalDocuments.ListCrewFlagMedical(
                       Int32.Parse(Filter.CurrentNewApplicantSelection), 1, General.GetNullableInteger(strFamilyId));

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Crew Medical", ds.Tables[0], alColumns, alCaptions, null, null);
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
            if (medicalid == ds.Tables[0].Rows[0]["FLDHARDCODE"].ToString())
                if (General.GetNullableInteger(flagid) == null)
                    ucError.ErrorMessage = "Flag is required for Flag Medical.";

        if (!Int16.TryParse(medicalid, out resultInt))
            ucError.ErrorMessage = "Medical is required";

        if (string.IsNullOrEmpty(dateofissue))
            ucError.ErrorMessage = "Issue Date is required.";
        else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }
        if (string.IsNullOrEmpty(placeofissue))
        {
            ucError.ErrorMessage = "Place of Issue is required";
        }
        if (string.IsNullOrEmpty(dateofexpiry))
            ucError.ErrorMessage = "Expiry Date is required.";

        if (dateofissue != null && dateofexpiry != null)
        {
            if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry, out resultDate)))
                if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry)))
                    ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
        }
        if (!Int16.TryParse(status, out resultInt))
            ucError.ErrorMessage = "Status is required";
        return (!ucError.IsError);
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                ViewState["LASTSIGNON"] = dt.Rows[0]["FLDLASTSIGNONDATE"].ToString();

            }
            dt = PhoenixNewApplicantFamilyNok.ListEmployeeFamily(Convert.ToInt32(Filter.CurrentNewApplicantSelection), General.GetNullableInteger(strFamilyId));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
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

    private void BindMedicalTestData()
    {
        try
        {
            string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE" };
            string[] alCaptions = { "Medical Test", "Place Of Issue", "Issue Date", "Expiry Date" };

            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewMedicalTest(
                        Int32.Parse(Filter.CurrentNewApplicantSelection), 1, General.GetNullableInteger(strFamilyId));

            General.SetPrintOptions("gvMedicalTest", "Medical Test", alCaptions, alColumns, ds);

            gvMedicalTest.DataSource = ds;
            gvMedicalTest.VirtualItemCount = ds.Tables[0].Rows.Count;


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
                string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE" };
                string[] alCaptions = { "Medical Test", "Place Of Issue", "Issue Date", "Expiry Date" };

                DataSet ds = PhoenixCrewMedicalDocuments.ListCrewMedicalTest(
                        Int32.Parse(Filter.CurrentNewApplicantSelection), 1, General.GetNullableInteger(strFamilyId));

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Crew Medical Test", ds.Tables[0], alColumns, alCaptions, null, null);
            }

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
            ucError.ErrorMessage = "Issue Date is required.";

        else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(dateofexpiry))
            ucError.ErrorMessage = "Expiry Date is required.";

        if (dateofissue != null && dateofexpiry != null)
        {
            if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry, out resultDate)))
                if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry)))
                    ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
        }

        return (!ucError.IsError);
    }
    private void BindMedicalVaccination()
    {
        try
        {
            string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE" };
            string[] alCaptions = { "Vaccination", "Place Of Issue", "Issue Date", "Expiry Date" };

            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewVaccination(
                        Int32.Parse(Filter.CurrentNewApplicantSelection), 1, General.GetNullableInteger(strFamilyId));

            General.SetPrintOptions("gvVaccination", "Crew Vaccination", alCaptions, alColumns, ds);
            gvVaccination.DataSource = ds;
            gvVaccination.VirtualItemCount = ds.Tables[0].Rows.Count;
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
                string[] alCaptions = { "Vaccination", "Place Of Issue", "Issue Date", "Expiry Date" };

                DataSet ds = PhoenixCrewMedicalDocuments.ListCrewVaccination(
                        Int32.Parse(Filter.CurrentNewApplicantSelection), 1, General.GetNullableInteger(strFamilyId));

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Crew Vaccination", ds.Tables[0], alColumns, alCaptions, null, null);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidMedicalVaccination(string vaccinationid, string dateofissue, string dateofexpiry)
    {
        Int16 resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!Int16.TryParse(vaccinationid, out resultInt))
            ucError.ErrorMessage = "Vaccination is required";

        if (string.IsNullOrEmpty(dateofissue))
            ucError.ErrorMessage = "Issue Date is required.";

        else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(dateofexpiry))
            ucError.ErrorMessage = "Expiry Date is required.";

        if (dateofissue != null && dateofexpiry != null)
        {
            if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry, out resultDate)))
                if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry)))
                    ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
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
            if (h.ID == "ucMedicalAdd")
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
    protected void gvCrewMedical_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMedicalData();
    }
    protected void gvMedicalTest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMedicalTestData();
    }
    protected void gvVaccination_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMedicalVaccination();
    }
    protected void gvCrewMedical_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            /*            GridEditableItem item = (GridEditableItem)e.Item*/

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Visible = SessionUtil.CanAccess(this.ViewState, db1.CommandName);
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
            }

            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
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
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=NFMEDICALUPLOAD'); return false;");
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
            GridDataItem item = (GridDataItem)e.Item;

            DataRowView drv = (DataRowView)item.DataItem;
            UserControlFlag ucFlag = (UserControlFlag)item.FindControl("ucFlagEdit");
            if (ucFlag != null) ucFlag.SelectedFlag = drv["FLDFLAGID"].ToString();


            UserControlHard ucMedicalEdit = (UserControlHard)item.FindControl("ucMedicalEdit");
            if (ucMedicalEdit != null)
            {
                ucMedicalEdit.HardList = PhoenixRegistersHard.ListHard(1, 95);
                if (ucMedicalEdit.SelectedHard == "")
                    ucMedicalEdit.SelectedHard = drv["FLDMEDICALID"].ToString();
            }

            UserControlHard ucStatus = (UserControlHard)item.FindControl("ddlStatus");
            if (ucStatus != null) ucStatus.SelectedHard = drv["FLDSTATUS"].ToString();
        }
        if (e.Item is GridFooterItem)
        {
            GridFooterItem footer = (GridFooterItem)e.Item;
            LinkButton ad = (LinkButton)footer.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
        }
    }

    protected void gvCrewMedical_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            if (e.CommandName.ToString().ToUpper() == "MEDICALADD")
            {
                GridFooterItem footer = (GridFooterItem)e.Item;

                _gridView.EditIndexes.Clear();

                string medicalid = ((UserControlHard)footer.FindControl("ucMedicalAdd")).SelectedHard;
                string dateofissue = ((UserControlDate)footer.FindControl("txtIssueDateAdd")).Text;
                string placeofissue = ((RadTextBox)footer.FindControl("txtPlaceOfIssueAdd")).Text;
                string dateofexpiry = ((UserControlDate)footer.FindControl("txtExpiryDateAdd")).Text;
                string flagid = ((UserControlFlag)footer.FindControl("ucFlagAdd")).SelectedFlag;
                string status = ((UserControlHard)footer.FindControl("ddlStatusAdd")).SelectedHard;
                if (!IsValidMedical(medicalid, dateofissue, dateofexpiry, flagid, placeofissue, status))
                {
                    ucError.Visible = true;
                    e.Canceled = true;
                    return;
                }

                PhoenixCrewMedicalDocuments.InsertCrewFlagMedicalFamily(
                     Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                    , Convert.ToInt32(medicalid)
                    , placeofissue
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry)
                    , General.GetNullableInteger(flagid)
                    , int.Parse(strFamilyId)
                    , int.Parse(status)
                    );

                BindMedicalData();
            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALARCHIVE")
            {
                GridDataItem item = (GridDataItem)e.Item;

                string crewflagmedicalid = ((RadLabel)item.FindControl("lblMedicalId")).Text;
                PhoenixCrewMedicalDocuments.ArchiveCrewMedical(int.Parse(crewflagmedicalid), 0);
                _gridView.EditIndexes.Clear();
                _gridView.SelectedIndexes.Clear();
                BindMedicalData();

            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALEDIT")
            {
                GridDataItem item = (GridDataItem)e.Item;

                gvMedicalTest.EditIndexes.Clear();
                gvVaccination.EditIndexes.Clear();
                gvMedicalTest.Rebind();
                gvVaccination.Rebind();
                BindMedicalData();
                DisableFlag((UserControlHard)item.FindControl("ucMedicalEdit"), null);
            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALDELETE")
            {
                GridDataItem item = (GridDataItem)e.Item;

                string medicalid = ((RadLabel)item.FindControl("lblMedicalId")).Text;

                PhoenixCrewMedicalDocuments.DeleteCrewFlagMedical(
                    Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                    , Convert.ToInt32(medicalid)
                );
                BindMedicalData();
            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALUPDATE")
            {
                GridDataItem item = (GridDataItem)e.Item;

                try
                {
                    string medicalid = ((UserControlHard)item.FindControl("ucMedicalEdit")).SelectedHard;
                    string dateofissue = ((UserControlDate)item.FindControl("txtIssueDateEdit")).Text;
                    string placeofissue = ((RadTextBox)item.FindControl("txtPlaceOfIssueEdit")).Text;
                    string dateofexpiry = ((UserControlDate)item.FindControl("txtExpiryDateEdit")).Text;
                    string flagid = ((UserControlFlag)item.FindControl("ucFlagEdit")).SelectedFlag;
                    string flagmedicalid = ((RadLabel)item.FindControl("lblMedicalIdEdit")).Text;
                    string status = ((UserControlHard)item.FindControl("ddlStatus")).SelectedHard;
                    if (!IsValidMedical(medicalid, dateofissue, dateofexpiry, flagid, placeofissue, status))
                    {
                        ucError.Visible = true;
                        e.Canceled = true;
                        return;
                    }

                    PhoenixCrewMedicalDocuments.UpdateCrewFlagMedicalFamily(Convert.ToInt32(flagmedicalid)
                        , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                        , Convert.ToInt32(medicalid)
                        , placeofissue
                        , General.GetNullableDateTime(dateofissue)
                        , General.GetNullableDateTime(dateofexpiry)
                        , General.GetNullableInteger(flagid)
                        , int.Parse(status)
                        , int.Parse(strFamilyId)
                        );

                    _gridView.EditIndexes.Clear();
                    BindMedicalData();
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "Please make the required correction";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALCANCEL")
            {
                _gridView.EditIndexes.Clear();
                BindMedicalData();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
        gvCrewMedical.Rebind();
    }

    protected void gvMedicalTest_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Visible = SessionUtil.CanAccess(this.ViewState, db1.CommandName);
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
            }
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
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
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=NFMEDICALTESTUPLOAD'); return false;");
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
        }
        if (e.Item.IsInEditMode)
        {
            UserControlDocuments ucMedicalTestEdit = (UserControlDocuments)e.Item.FindControl("ucMedicalTestEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucMedicalTestEdit != null)
            {
                //ucMedicalEdit.ShortNameFilter = "P&I,UKP,FLM";
                ucMedicalTestEdit.DocumentList = PhoenixRegistersDocumentMedical.ListDocumentMedical();
                if (ucMedicalTestEdit.SelectedDocument == "")
                    ucMedicalTestEdit.SelectedDocument = drv["FLDMEDICALTESTID"].ToString();
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
        }
    }
    protected void gvMedicalTest_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "MEDICALTESTADD")
            {
                GridFooterItem footer = (GridFooterItem)e.Item;

                gvMedicalTest.EditIndexes.Clear();

                string medicaltestid = ((UserControlDocuments)footer.FindControl("ucMedicalTestAdd")).SelectedDocument;
                string dateofissue = ((UserControlDate)footer.FindControl("txtIssueDateAdd")).Text;
                string placeofissue = ((RadTextBox)footer.FindControl("txtPlaceOfIssueAdd")).Text;
                string dateofexpiry = ((UserControlDate)footer.FindControl("txtExpiryDateAdd")).Text;

                if (!IsValidMedicalTest(medicaltestid, dateofissue, dateofexpiry))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewMedicalDocuments.InsertCrewMedicalTestFamily(Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                    , Convert.ToInt32(medicaltestid)
                    , placeofissue
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry)
                    , int.Parse(strFamilyId)
                    );

            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALTESTARCHIVE")
            {
                string medicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestId")).Text;
                PhoenixCrewMedicalDocuments.ArchiveCrewMedicalTest(int.Parse(medicaltestid), 0);
                gvMedicalTest.EditIndexes.Clear();
                gvMedicalTest.SelectedIndexes.Clear();

            }
            else if (e.CommandName.ToString().ToUpper() == "EDIT")
            {
                gvCrewMedical.EditIndexes.Clear();
                gvVaccination.EditIndexes.Clear();
                gvCrewMedical.Rebind();
                gvVaccination.Rebind();
                BindMedicalTestData();
            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALTESTDELETE")
            {
                string medicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestId")).Text;

                PhoenixCrewMedicalDocuments.DeleteCrewMedicalTest(
                    Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                    , Convert.ToInt32(medicaltestid)
                                                                );
            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALTESTUPDATE")
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
                        ucError.Visible = true;
                        e.Canceled = true;
                        return;
                    }

                    PhoenixCrewMedicalDocuments.UpdateCrewMedicalTestFamily(
                         Convert.ToInt32(crewmedicaltestid)
                        , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                        , Convert.ToInt32(medicaltestid)
                        , placeofissue
                        , General.GetNullableDateTime(dateofissue)
                        , General.GetNullableDateTime(dateofexpiry)
                        , int.Parse(strFamilyId)
                        );

                    gvMedicalTest.EditIndexes.Clear();
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "Please make the required correction";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALTESTCANCEL")
            {
                gvMedicalTest.EditIndexes.Clear();
            }
            BindMedicalTestData();

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
        gvMedicalTest.Rebind();
    }
    protected void gvVaccination_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Visible = SessionUtil.CanAccess(this.ViewState, db1.CommandName);
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
            }
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
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
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=NFVACCINATIONUPLOAD'); return false;");
            }
            RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
            Image imgFlag = e.Item.FindControl("imgFlag") as Image;
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
            UserControlDocumentVaccination ucDocumentVaccination = (UserControlDocumentVaccination)e.Item.FindControl("ucVaccinationEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucDocumentVaccination != null) ucDocumentVaccination.SelectedDocument = drv["FLDVACCINATIONID"].ToString();
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
        }

    }
    protected void gvVaccination_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "VACCINATIONADD")
            {
                GridFooterItem footer = (GridFooterItem)e.Item;

                gvVaccination.EditIndexes.Clear();

                string vaccinationid = ((UserControlDocumentVaccination)footer.FindControl("ucVaccinationAdd")).SelectedDocument;
                string dateofissue = ((UserControlDate)footer.FindControl("txtIssueDateAdd")).Text;
                string placeofissue = ((RadTextBox)footer.FindControl("txtPlaceOfIssueAdd")).Text;
                string dateofexpiry = ((UserControlDate)footer.FindControl("txtExpiryDateAdd")).Text;

                if (!IsValidMedicalVaccination(vaccinationid, dateofissue, dateofexpiry))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewMedicalDocuments.InsertCrewVaccinationFamily(
                     Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                    , Convert.ToInt32(vaccinationid)
                    , placeofissue
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry)
                    , int.Parse(strFamilyId)
                    );
            }
            else if (e.CommandName.ToString().ToUpper() == "EDIT")
            {
                gvCrewMedical.EditIndexes.Clear();
                gvMedicalTest.EditIndexes.Clear();
                gvCrewMedical.Rebind();
                gvMedicalTest.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "VACCINATIONARCHIVE")
            {
                string vaccinationid = ((RadLabel)e.Item.FindControl("lblCrewVaccinationId")).Text;
                PhoenixCrewMedicalDocuments.ArchiveCrewVaccination(int.Parse(vaccinationid), 0);
                gvVaccination.EditIndexes.Clear();
                gvVaccination.SelectedIndexes.Clear();
            }
            else if (e.CommandName.ToString().ToUpper() == "VACCINATIONDELETE")
            {
                string crewvaccinationid = ((RadLabel)e.Item.FindControl("lblCrewVaccinationId")).Text;

                PhoenixCrewMedicalDocuments.DeleteCrewVaccination(
                    Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                    , Convert.ToInt32(crewvaccinationid)
                );
            }
            else if (e.CommandName.ToString().ToUpper() == "VACCINATIONUPDATE")
            {
                string vaccinationid = ((UserControlDocumentVaccination)e.Item.FindControl("ucVaccinationEdit")).SelectedDocument;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
                string dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit")).Text;
                string crewvaccinationid = ((RadLabel)e.Item.FindControl("lblCrewVaccinationIdEdit")).Text;

                if (!IsValidMedicalVaccination(vaccinationid, dateofissue, dateofexpiry))
                {
                    ucError.Visible = true;
                    e.Canceled = true;
                    return;
                }

                PhoenixCrewMedicalDocuments.UpdateCrewVaccinationFamily(
                     Convert.ToInt32(crewvaccinationid)
                    , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                    , Convert.ToInt32(vaccinationid)
                    , placeofissue
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry)
                    , int.Parse(strFamilyId)
                    );
                gvVaccination.EditIndexes.Clear();
            }
            else if (e.CommandName.ToString().ToUpper() == "VACCINATIONCANCEL")
            {
                gvVaccination.EditIndexes.Clear();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
        BindMedicalVaccination();
        gvVaccination.Rebind();
    }
}
