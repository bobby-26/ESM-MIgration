using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using System.Text;
using Telerik.Web.UI;
public partial class CrewCBTCourses : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmenu = new PhoenixToolbar();
            CrewCourse.AccessRights = this.ViewState;
            CrewCourse.MenuList = toolbarmenu.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewCBTCourses.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCBTCourses')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewCBTCourses.aspx", "Email", "<i class=\"fas fa-envelope\"></i>", "CBTEmail");
            CBTCourses.AccessRights = this.ViewState;
            CBTCourses.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["PAGENUMBERCBT"] = 1;
                ViewState["SORTEXPRESSIONCBT"] = null;
                ViewState["SORTDIRECTIONCBT"] = null;

                SetEmployeePrimaryDetails();
                gvCBTCourses.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        BindDataCBT();
        gvCBTCourses.Rebind();
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
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CBTCourses_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDCOURSENAME", "FLDCOURSENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDNAME" };
                string[] alCaptions = { "Course", "Certificate Number", "Place Of Issue", "Issue Date", "Expiry Date", "Institution" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSIONCBT"] == null) ? null : (ViewState["SORTEXPRESSIONCBT"].ToString());

                if (ViewState["SORTDIRECTIONCBT"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTIONCBT"].ToString());

                if (ViewState["ROWCOUNTCBT"] == null || Int32.Parse(ViewState["ROWCOUNTCBT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNTCBT"].ToString());

                DataSet ds = PhoenixCrewCourseCertificate.CrewCourseCertificateSearch(
                                Int32.Parse(Filter.CurrentCrewSelection.ToString()), 1
                                , 1  // list CBT courses only
                                , sortexpression, sortdirection
                                , 1
                                , iRowCount
                                , ref iRowCount
                                , ref iTotalPageCount);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Crew CBT Courses", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("CBTEMAIL"))
            {
                StringBuilder stbcourseid = new StringBuilder();
                foreach (GridDataItem gvr in gvCBTCourses.Items)
                {
                    if (((CheckBox)gvr.FindControl("chkChekedEmail")).Checked == true)
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

                string scriptRefreshDontClose = "";
                scriptRefreshDontClose += "<script language='javaScript' id='CrewEmail'>" + "\n";
                scriptRefreshDontClose += "parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewEmail.aspx?csvcourseid=" + stbcourseid.ToString() + "&empid=" + Filter.CurrentCrewSelection + "&course=1')";
                scriptRefreshDontClose += "</script>" + "\n";
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "CrewEmail", scriptRefreshDontClose, false);

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataCBT()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCOURSENAME", "FLDCOURSENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDNAME" };
            string[] alCaptions = { "Course", "Certificate Number", "Place Of Issue", "Issue Date", "Expiry Date", "Institution" };

            string sortexpression = (ViewState["SORTEXPRESSIONCBT"] == null) ? null : (ViewState["SORTEXPRESSIONCBT"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTIONCBT"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONCBT"].ToString());

            DataSet ds = PhoenixCrewCourseCertificate.CrewCourseCertificateSearch(
                        Int32.Parse(Filter.CurrentCrewSelection.ToString()), 1
                        , 1  // list CBT courses only
                        , sortexpression, sortdirection
                        , Int32.Parse(ViewState["PAGENUMBERCBT"].ToString())
                        , gvCBTCourses.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

            General.SetPrintOptions("gvCBTCourses", "Crew CBT Courses", alCaptions, alColumns, ds);

            gvCBTCourses.DataSource = ds;
            gvCBTCourses.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNTCBT"] = iRowCount;
            ViewState["TOTALPAGECOUNTCBT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCBTCourses_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBERCBT"] = ViewState["PAGENUMBERCBT"] != null ? ViewState["PAGENUMBERCBT"] : gvCBTCourses.CurrentPageIndex + 1;
        BindDataCBT();
    }



    protected void gvCBTCourses_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "CBTADD")
            {
                string courseid = ((UserControlCourse)e.Item.FindControl("ddlCourseAdd")).SelectedCourse;
                string certificatenumber = ((RadTextBox)e.Item.FindControl("txtCourseNumberAdd")).Text;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd"));
                string institutionid = ((UserControlAddressType)e.Item.FindControl("ucInstitutionAdd")).SelectedAddress;

                if (!IsValidCourseCertificateCBT(courseid, certificatenumber, dateofissue, dateofexpiry))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewCourseCertificate.InsertCrewCourseCertificate(Convert.ToInt32(Filter.CurrentCrewSelection)
                    , Convert.ToInt32(courseid)
                    , certificatenumber
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry.Text)
                    , placeofissue
                    , General.GetNullableInteger(institutionid)
                    , null, null, null, null
                    );

                BindDataCBT();
                gvCBTCourses.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "CBTARCHIVE")
            {
                string id = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;
                PhoenixCrewCourseCertificate.ArchiveCrewCourseCertificate(int.Parse(Filter.CurrentCrewSelection), int.Parse(id), 0);
                BindDataCBT();
                gvCBTCourses.Rebind();

            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERCBT"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvCBTCourses_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string courseid = ((UserControlCourse)e.Item.FindControl("ddlCourseEdit")).SelectedCourse;
            string certificatenumber = ((RadTextBox)e.Item.FindControl("txtCourseNumberEdit")).Text;
            string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
            string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
            UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit"));
            string coursecertificateid = ((RadLabel)e.Item.FindControl("lblCourseIdEdit")).Text;
            string institutionid = ((UserControlAddressType)e.Item.FindControl("ucInstitutionEdit")).SelectedAddress;

            if (!IsValidCourseCertificateCBT(courseid, certificatenumber, dateofissue, dateofexpiry))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }

            PhoenixCrewCourseCertificate.UpdateCrewCourseCertificate(Convert.ToInt32(coursecertificateid)
                , Convert.ToInt32(Filter.CurrentCrewSelection)
                , Convert.ToInt32(courseid)
                , certificatenumber
                , General.GetNullableDateTime(dateofissue)
                , General.GetNullableDateTime(dateofexpiry.Text)
                , placeofissue
                , General.GetNullableInteger(institutionid), null, null, null
               );

            BindDataCBT();
            gvCBTCourses.Rebind();
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCBTCourses_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string coursecertificateid = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;

            PhoenixCrewCourseCertificate.DeleteCrewCourseCertificate(Convert.ToInt32(coursecertificateid)
                                                            );
            BindDataCBT();
            gvCBTCourses.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvCBTCourses_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure want to delete?'); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton lnkCourseNumber = (LinkButton)e.Item.FindControl("lnkCourseNumber");
            if (cme != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
                lnkCourseNumber.Enabled = SessionUtil.CanAccess(this.ViewState, cme.CommandName);
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('Codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&cmdname=CBTUPLOAD'); return false;");
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
                        //e.Row.CssClass = "rowyellow";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        //e.Row.CssClass = "rowred";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
            }
        }

        if (e.Item.IsInEditMode)
        {
            DataRowView drvCourse = (DataRowView)e.Item.DataItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            UserControlCourse ucCourse = (UserControlCourse)e.Item.FindControl("ddlCourseEdit");
            if (ucCourse != null) ucCourse.SelectedCourse = drvCourse["FLDCOURSE"].ToString();

            UserControlAddressType ucAddressType = (UserControlAddressType)e.Item.FindControl("ucInstitutionEdit");
            if (ucAddressType != null) ucAddressType.SelectedAddress = drv["FLDINSTITUTIONID"].ToString();
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
    private bool IsValidCourseCertificateCBT(string courseid, string certificatenumber, string dateofissue, UserControlDate dateofexpiry)
    {
        Int16 resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!Int16.TryParse(courseid, out resultInt))
            ucError.ErrorMessage = "Course is required";

        if (certificatenumber.Trim() == "")
            ucError.ErrorMessage = "Certificate Number is required";

        if (string.IsNullOrEmpty(dateofissue))
            ucError.ErrorMessage = "Issue Date is required.";
        else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(dateofexpiry.Text) && dateofexpiry.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Expiry Date is required.";

        if (dateofissue != null && dateofexpiry.Text != null)
        {
            if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry.Text, out resultDate)))
                if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry.Text)))
                    ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
        }

        return (!ucError.IsError);
    }

    protected void ddlDocumentCBT_TextChanged(object sender, EventArgs e)
    {
        UserControlCourse dc = (UserControlCourse)sender;
        DataSet ds = new DataSet();
        UserControlDate expirydate;
        if (dc.SelectedCourse != "Dummy")
        {
            ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(General.GetNullableInteger(dc.SelectedCourse).Value);
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (dc != null && dc.ID == "ddlCourseAdd")
            {
                GridFooterItem row = (GridFooterItem)dc.NamingContainer;
                expirydate = (UserControlDate)row.FindControl("txtExpiryDateAdd");
            }
            else
            {
                GridDataItem row = (GridDataItem)dc.NamingContainer;

                expirydate = (UserControlDate)row.FindControl("txtExpiryDateEdit");
            }

            if (ds.Tables[0].Rows[0]["FLDEXPIRY"].ToString() == "1")
            {
                expirydate.CssClass = "input_mandatory";
            }
            else
            {
                expirydate.CssClass = "";
            }
        }
    }


    protected void gvCBTCourses_EditCommand(object sender, GridCommandEventArgs e)
    {

    }
}
