using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using System.Text;
using Telerik.Web.UI;
public partial class CrewNewApplicantCourseAndCertificate : PhoenixBasePage
{

    protected override void Render(HtmlTextWriter writer)
    {

        foreach (GridDataItem r in gvCrewCourseCertificate.Items)
        {
            if (r is GridDataItem)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantCourseAndCertificate.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewCourseCertificate')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewCourseandCertificateArchived.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&type=1&t=n'); return false;", "Show Archived", "<i class=\"fas fa-archive\"></i>", "COURSESARCHIVED");
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantCourseAndCertificate.aspx", "Email", " <i class=\"fa fa-envelope\"></i>", "COURSESEMAIL");

            if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCourseMissing.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&t=p'); return false;", "Initiate Course Request", "<i class=\"fas fa-book\"></i>", "COURSEREQUEST");
            else
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewCourseMissing.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&t=p'); return false;", "Initiate Course Request", "<i class=\"fas fa-book\"></i>", "COURSEREQUEST");
            MenuCrewCourseCertificate.AccessRights = this.ViewState;
            MenuCrewCourseCertificate.MenuList = toolbar.Show();

          
            toolbar = new PhoenixToolbar();
            CrewCourse.AccessRights = this.ViewState;
            CrewCourse.MenuList = toolbar.Show();
            //MenuCrewCourseCertificate.SetTrigger(pnlNewApplicantCourseCertificateEntry);

            /////////////////////////////////////////string coursecbttype = PhoenixCommonRegisters.GetHardCode(1, 80, "CBT");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["CURRENTINDEX"] = 1;
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                SetEmployeePrimaryDetails();
                gvCrewCourseCertificate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvCrewCourseCertificate.Rebind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCOURSENAME", "FLDCOURSENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDNAME", "FLDNATIONALITY", "FLDAUTHORITY", "FLDREMARKS" };
            string[] alCaptions = { "Course", "Certificate Number", "Place Of Issue", "Issue Date", "Expiry Date", "Institution", "	Nationality", "	Issuing Authority", "Remarks" };
            DataSet ds = PhoenixNewApplicantCourse.NewApplicantCourseSearch(
                        Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), 1
                        , 0  // list courses other than cbt's
                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvCrewCourseCertificate.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

            General.SetPrintOptions("gvCrewCourseCertificate", "Crew Course And Certificate", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrewCourseCertificate.DataSource = ds;
                gvCrewCourseCertificate.VirtualItemCount = iRowCount;
            }
            else
            {
                gvCrewCourseCertificate.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewCourseCertificate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDCOURSENAME", "FLDCOURSENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDNAME", "FLDNATIONALITY", "FLDAUTHORITY", "FLDREMARKS" };
                string[] alCaptions = { "Course", "Certificate Number", "Place Of Issue", "Issue Date", "Expiry Date", "Institution", "	Nationality", "	Issuing Authority", "Remarks" };

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixNewApplicantCourse.NewApplicantCourseSearch(
                                Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), 1
                                , 0  // list courses other than cbt's
                                , 1
                                , General.ShowRecords(null)
                                , ref iRowCount
                                , ref iTotalPageCount);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Crew Course And Certificate", ds.Tables[0], alColumns, alCaptions, null, "");
            }
            else if (CommandName.ToUpper().Equals("COURSESEMAIL"))
            {
                StringBuilder stbcourseid = new StringBuilder();
                foreach (GridDataItem gvr in gvCrewCourseCertificate.Items)
                {
                    if (((RadCheckBox)gvr.FindControl("chkChekedEmail")).Checked == true)
                    {

                        string courseid = ((RadLabel)gvr.FindControl("lbldocid")).Text;
                        stbcourseid.Append(courseid);
                        stbcourseid.Append(",");

                    }
                }
                if (stbcourseid.Length > 1)
                {
                    stbcourseid.Remove(stbcourseid.Length - 1, 1);
                }
                string sScript = "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewEmail.aspx?csvcourseid=" + stbcourseid.ToString() + "&empid=" + Filter.CurrentNewApplicantSelection + "&course=1');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCourseCertificate(string courseid, string certificatenumber, string dateofissue, UserControlDate dateofexpiry, string placeofissue, string institutionid, string flagid)
    {
        Int16 resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!Int16.TryParse(courseid, out resultInt))
            ucError.ErrorMessage = "Course is required";

        if (!Int16.TryParse(institutionid, out resultInt))
            ucError.ErrorMessage = "Institution is required";

        if (!Int16.TryParse(flagid, out resultInt))
            ucError.ErrorMessage = "Nationality is required";

        if (certificatenumber.Trim() == "")
            ucError.ErrorMessage = "Certificate Number is required";

        if (placeofissue.Trim() == "")
            ucError.ErrorMessage = "Place of issue is required";

        if (General.GetNullableDateTime(dateofissue) == null)
            ucError.ErrorMessage = "Issue Date is required.";

        if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }

        if ((General.GetNullableDateTime(dateofexpiry.Text) == null) && dateofexpiry.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Expiry Date is required.";

        if (dateofissue != null && dateofexpiry.Text != null)
        {
            if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry.Text, out resultDate)))
                if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry.Text)))
                    ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
        }

        return (!ucError.IsError);
    }

    private bool IsValidateGrid(GridView _gridView, int nCurrentRow)
    {
        string courseid = ((UserControlCourse)_gridView.Rows[nCurrentRow].FindControl("ddlCourseEdit")).SelectedCourse;
        string certificatenumber = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCourseNumberEdit")).Text;
        string dateofissue = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtIssueDateEdit")).Text;
        string placeofissue = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPlaceOfIssueEdit")).Text;
        UserControlDate dateofexpiry = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtExpiryDateEdit"));
        string coursecertificateid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseIdEdit")).Text;
        string institutionid = ((UserControlAddressType)_gridView.Rows[nCurrentRow].FindControl("ucInstitutionEdit")).SelectedAddress;
        string flagid = ((UserControlNationality)_gridView.Rows[nCurrentRow].FindControl("ddlFlagEdit")).SelectedNationality;
        return IsValidCourseCertificate(courseid, certificatenumber, dateofissue, dateofexpiry, placeofissue, institutionid, flagid);
    }

    private bool IsValidateGrid(GridEditableItem _gridView)
    {
        string courseid = ((UserControlCourse)_gridView.FindControl("ddlCourseEdit")).SelectedCourse;
        string certificatenumber = ((RadTextBox)_gridView.FindControl("txtCourseNumberEdit")).Text;
        string dateofissue = ((UserControlDate)_gridView.FindControl("txtIssueDateEdit")).Text;
        string placeofissue = ((RadTextBox)_gridView.FindControl("txtPlaceOfIssueEdit")).Text;
        UserControlDate dateofexpiry = ((UserControlDate)_gridView.FindControl("txtExpiryDateEdit"));
        string coursecertificateid = ((RadLabel)_gridView.FindControl("lblCourseIdEdit")).Text;
        string institutionid = ((UserControlAddressType)_gridView.FindControl("ucInstitutionEdit")).SelectedAddress;
        string flagid = ((UserControlNationality)_gridView.FindControl("ddlFlagEdit")).SelectedNationality;
        return IsValidCourseCertificate(courseid, certificatenumber, dateofissue, dateofexpiry, placeofissue, institutionid, flagid);
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
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

    protected void ddlDocument_TextChanged(object sender, EventArgs e)
    {
        UserControlCourse dc = (UserControlCourse)sender;
        DataSet ds = new DataSet();
        UserControlDate expirydate;
        //GridViewRow gv = (GridViewRow)dc.NamingContainer;

        if (dc.SelectedCourse != "Dummy" && General.GetNullableInteger(dc.SelectedCourse) != null)
        {
            ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(int.Parse(dc.SelectedCourse));
        }
        if (ds.Tables.Count > 0)
        {
            if (dc.ID == "ddlCourseAdd")
            {
                GridFooterItem Item = (GridFooterItem)dc.NamingContainer;
                expirydate = (UserControlDate)Item.FindControl("txtExpiryDateAdd");
                RadTextBox txtCourseNumberAdd = (RadTextBox)Item.FindControl("txtCourseNumberAdd");
                txtCourseNumberAdd.Focus();
            }
            else
            {
                GridDataItem dataItem = (GridDataItem)dc.NamingContainer;
                expirydate = (UserControlDate)dataItem.FindControl("txtExpiryDateEdit");
                RadTextBox txtCourseNumberEdit = (RadTextBox)dataItem.FindControl("txtCourseNumberEdit");
                txtCourseNumberEdit.Focus();
            }
            if (ds.Tables[0].Rows[0]["FLDEXPIRY"].ToString() == "1")
            {
                expirydate.CssClass = "input_mandatory"; ;
            }
            else
            {
                expirydate.CssClass = "input"; ;
            }
        }
    }

    protected void gvCrewCourseCertificate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "COURSESADD")
            {

                GridFooterItem eeditedItem = e.Item as GridFooterItem;
                string courseid = ((UserControlCourse)eeditedItem.FindControl("ddlCourseAdd")).SelectedCourse;
                string institutionid = ((UserControlAddressType)eeditedItem.FindControl("ucInstitutionAdd")).SelectedAddress;
                string certificatenumber = ((RadTextBox)eeditedItem.FindControl("txtCourseNumberAdd")).Text;
                string dateofissue = ((UserControlDate)eeditedItem.FindControl("txtIssueDateAdd")).Text;
                string placeofissue = ((RadTextBox)eeditedItem.FindControl("txtPlaceOfIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)eeditedItem.FindControl("txtExpiryDateAdd"));
                string remarks = ((RadTextBox)eeditedItem.FindControl("txtRemarksAdd")).Text;
                string flagid = ((UserControlNationality)eeditedItem.FindControl("ddlFlagAdd")).SelectedNationality;
                string authority = ((RadTextBox)eeditedItem.FindControl("txtAuthorityAdd")).Text;
                string verifiedyn = ((RadCheckBox)eeditedItem.FindControl("chkVerifiedAdd")).Checked == true ? "1" : "0";

                if (!IsValidCourseCertificate(courseid, certificatenumber, dateofissue, dateofexpiry, placeofissue, institutionid, flagid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixNewApplicantCourse.InsertNewApplicantCourse(Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                    , Convert.ToInt32(courseid)
                    , certificatenumber
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry.Text)
                    , placeofissue
                    , General.GetNullableInteger(institutionid)
                    , General.GetNullableString(remarks)
                    , General.GetNullableInteger(flagid)
                    , General.GetNullableString(authority)
                    , null
                   , byte.Parse(verifiedyn)
                    );

                BindData();
                gvCrewCourseCertificate.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "COURSESARCHIVE")
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string id = ((RadLabel)eeditedItem.FindControl("lblCourseId")).Text;
                PhoenixNewApplicantCourse.ArchiveCrewCourseCertificate(int.Parse(Filter.CurrentNewApplicantSelection), int.Parse(id), 0);
                BindData();
                gvCrewCourseCertificate.Rebind();
            }            
            else if (e.CommandName.ToString().ToUpper() == "COURSESDELETE")
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string coursecertificateid = ((RadLabel)eeditedItem.FindControl("lblCourseId")).Text;
                PhoenixNewApplicantCourse.DeleteNewApplicantCourse(Convert.ToInt32(coursecertificateid));

                BindData();
                gvCrewCourseCertificate.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string courseid = ((UserControlCourse)eeditedItem.FindControl("ddlCourseEdit")).SelectedCourse;
                string institutionid = ((UserControlAddressType)eeditedItem.FindControl("ucInstitutionEdit")).SelectedAddress;
                string certificatenumber = ((RadTextBox)eeditedItem.FindControl("txtCourseNumberEdit")).Text;
                string dateofissue = ((UserControlDate)eeditedItem.FindControl("txtIssueDateEdit")).Text;
                string placeofissue = ((RadTextBox)eeditedItem.FindControl("txtPlaceOfIssueEdit")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)eeditedItem.FindControl("txtExpiryDateEdit"));
                string coursecertificateid = ((RadLabel)eeditedItem.FindControl("lblCourseIdEdit")).Text;
                string authority = ((RadTextBox)eeditedItem.FindControl("txtAuthorityEdit")).Text;
                string flagid = ((UserControlNationality)eeditedItem.FindControl("ddlFlagEdit")).SelectedNationality;
                string verifiedyn = ((RadCheckBox)eeditedItem.FindControl("chkVerifiedEdit")).Checked == true ? "1" : "0";

                if (!IsValidateGrid(eeditedItem))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixNewApplicantCourse.UpdateNewApplicantCourse(Convert.ToInt32(coursecertificateid)
                    , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                    , Convert.ToInt32(courseid)
                    , certificatenumber
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry.Text)
                    , placeofissue
                    , General.GetNullableInteger(institutionid)
                    , General.GetNullableInteger(flagid)
                    , General.GetNullableString(authority)
                    , null
                    , byte.Parse(verifiedyn)
                  );
                BindData();
                gvCrewCourseCertificate.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewCourseCertificate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewCourseCertificate.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewCourseCertificate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
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
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                else
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&cmdname=COURSEUPLOAD'); return false;");
            }

            RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            if (db != null)
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
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblCourseId");
            LinkButton imgRemarks = (LinkButton)e.Item.FindControl("imgRemarks");
            RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
            if (imgRemarks != null)
            {
                if (lblR.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";
                    imgRemarks.Controls.Add(html);
                }
                imgRemarks.Attributes.Add("onclick", "javascript:openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.COURSE + "','xlarge');return false;");
            }            
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblVerified");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipCertificates");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
            UserControlAddressType ucAddressType = (UserControlAddressType)e.Item.FindControl("ucInstitutionEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucAddressType != null) ucAddressType.SelectedAddress = drv["FLDINSTITUTIONID"].ToString();

            UserControlCourse ucCourse = (UserControlCourse)e.Item.FindControl("ddlCourseEdit");
            DataRowView drvCourse = (DataRowView)e.Item.DataItem;
            if (ucCourse != null)
            {
                ucCourse.CourseList = PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse();
                if (ucCourse.SelectedCourse == "")
                    ucCourse.SelectedCourse = drvCourse["FLDCOURSE"].ToString();
            }

            UserControlNationality ucFlag = (UserControlNationality)e.Item.FindControl("ddlFlagEdit");
            DataRowView drvFlag = (DataRowView)e.Item.DataItem;
            if (ucFlag != null) ucFlag.SelectedNationality = drvFlag["FLDFLAGID"].ToString();
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
        }
    }
}