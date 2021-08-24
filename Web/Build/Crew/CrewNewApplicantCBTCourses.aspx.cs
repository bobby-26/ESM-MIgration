using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Text;
using Telerik.Web.UI;
public partial class CrewNewApplicantCBTCourses : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();            
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantCBTCourses.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCBTCourses')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbar.AddFontAwesomeButton("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewCourseandCertificateArchived.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&type=1&t=n'); return false;", "Find", "<i class=\"fas fa-archive\"></i>", "COURSESARCHIVED");
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantCBTCourses.aspx", "Email", " <i class=\"fa fa-envelope\"></i>", "EMAIL");
            CBTCourses.AccessRights = this.ViewState;
            CBTCourses.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            CrewCBTCourse.AccessRights = this.ViewState;
            CrewCBTCourse.MenuList = toolbar.Show();

            /////////////////////////////////////////string coursecbttype = PhoenixCommonRegisters.GetHardCode(1, 80, "CBT");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSIONCBT"] = null;
                ViewState["SORTDIRECTIONCBT"] = null;
                ViewState["CURRENTINDEXCBT"] = 1;
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                SetEmployeePrimaryDetails();
                gvCBTCourses.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }            
            BindDataCBT();
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

            DataSet ds = PhoenixNewApplicantCourse.NewApplicantCourseSearch(
                        Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), 1
                        , 1  // list courses other than cbt's
                        , sortexpression, sortdirection
                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvCBTCourses.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

            General.SetPrintOptions("gvCBTCourses", "Crew CBT Courses", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCBTCourses.DataSource = ds;
                gvCBTCourses.VirtualItemCount = iRowCount;
            }
            else
            {
                gvCBTCourses.DataSource = "";
            }

            //ViewState["ROWCOUNTCBT"] = iRowCount;
            //ViewState["TOTALPAGECOUNTCBT"] = iTotalPageCount;
            //RadComboBox institution = (RadComboBox)gvCBTCourses.FooterRow.FindControl("ucInstitutionAdd").FindControl("ddlAddressType");
            //institution.Attributes["style"] = "width:250px";
            //RadComboBox course = (RadComboBox)gvCBTCourses.FooterRow.FindControl("ddlCourseAdd").FindControl("ddlCourse");
            //course.Attributes["style"] = "width:250px";
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

                DataSet ds = PhoenixNewApplicantCourse.NewApplicantCourseSearch(
                        Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), 1
                        , 1  // list courses other than cbt's
                        , sortexpression, sortdirection
                        , (int)ViewState["PAGENUMBER"]
                        , General.ShowRecords(null)
                        , ref iRowCount
                        , ref iTotalPageCount);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Crew CBT Courses", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("EMAIL"))
            {
                StringBuilder stbcourseid = new StringBuilder();
                foreach (GridDataItem gvr in gvCBTCourses.Items)
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

                //string scriptRefreshDontClose = "";
                //scriptRefreshDontClose += "<script language='javaScript' id='CrewEmail'>" + "\n";
                //scriptRefreshDontClose += "parent.Openpopup('NAFA','','CrewEmail.aspx?csvcourseid=" + stbcourseid.ToString() + "&empid=" + Filter.CurrentNewApplicantSelection + "&course=1')";
                //scriptRefreshDontClose += "</script>" + "\n";
                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "CrewEmail", scriptRefreshDontClose, false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 
    //protected void gvCBTCourses_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //    try
    //    {
    //        if (e.CommandName.ToString().ToUpper() == "CBTADD")
    //        {

    //            _gridView.EditIndex = -1;
    //            string courseid = ((UserControlCourse)_gridView.FooterRow.FindControl("ddlCourseAdd")).SelectedCourse;
    //            string certificatenumber = ((TextBox)_gridView.FooterRow.FindControl("txtCourseNumberAdd")).Text;
    //            string dateofissue = ((UserControlDate)_gridView.FooterRow.FindControl("txtIssueDateAdd")).Text;
    //            string placeofissue = ((TextBox)_gridView.FooterRow.FindControl("txtPlaceOfIssueAdd")).Text;
    //            UserControlDate dateofexpiry = ((UserControlDate)_gridView.FooterRow.FindControl("txtExpiryDateAdd"));
    //            string institutionid = ((UserControlAddressType)_gridView.FooterRow.FindControl("ucInstitutionAdd")).SelectedAddress;
    //            if (!IsValidCourseCertificateCBT(courseid, certificatenumber, dateofissue, dateofexpiry))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            PhoenixNewApplicantCourse.InsertNewApplicantCourse(Convert.ToInt32(Filter.CurrentNewApplicantSelection)
    //                , Convert.ToInt32(courseid)
    //                , certificatenumber
    //                , General.GetNullableDateTime(dateofissue)
    //                , General.GetNullableDateTime(dateofexpiry.Text)
    //                , placeofissue, General.GetNullableInteger(institutionid)
    //                , null, null, null,null
    //                );

    //            BindDataCBT();
    //            SetPageNavigatorCBT();
    //        }
    //        else if (e.CommandName.ToString().ToUpper() == "CBTARCHIVE")
    //        {
    //            string id = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseId")).Text;
    //            PhoenixNewApplicantCourse.ArchiveCrewCourseCertificate(int.Parse(Filter.CurrentNewApplicantSelection), int.Parse(id), 0);
    //            _gridView.EditIndex = -1;
    //            _gridView.SelectedIndex = -1;
    //            BindDataCBT();
    //            SetPageNavigatorCBT();
    //        }
    //        else if (e.CommandName.ToString().ToUpper() == "CBTEDIT")
    //        {
    //            if (_gridView.EditIndex > -1)
    //            {
    //                if (!IsValidateGridCBT(_gridView, _gridView.EditIndex))
    //                {
    //                    ucError.Visible = true;
    //                    return;
    //                }
    //                _gridView.UpdateRow(_gridView.EditIndex, false);
    //            }

    //            _gridView.SelectedIndex = nCurrentRow;
    //            _gridView.EditIndex = nCurrentRow;
    //            BindDataCBT();
    //            RadComboBox institution = (RadComboBox)_gridView.Rows[nCurrentRow].FindControl("ucInstitutionEdit").FindControl("ddlAddressType");
    //            institution.Attributes["style"] = "width:250px";
    //            RadComboBox course = (RadComboBox)_gridView.Rows[nCurrentRow].FindControl("ddlCourseEdit").FindControl("ddlCourse");
    //            course.Attributes["style"] = "width:250px";
    //            //((TextBox)_gridView.Rows[e.NewEditIndex].FindControl("lblVISAIdEdit")).Focus();
    //            SetPageNavigatorCBT();
    //        }
    //        else if (e.CommandName.ToString().ToUpper() == "CBTDELETE")
    //        {
    //            string coursecertificateid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseId")).Text;

    //            PhoenixNewApplicantCourse.DeleteNewApplicantCourse(Convert.ToInt32(coursecertificateid));

    //            BindDataCBT();
    //            SetPageNavigatorCBT();
    //        }
    //        else if (e.CommandName.ToString().ToUpper() == "CBTUPDATE")
    //        {
    //            string courseid = ((UserControlCourse)_gridView.Rows[nCurrentRow].FindControl("ddlCourseEdit")).SelectedCourse;
    //            string certificatenumber = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCourseNumberEdit")).Text;
    //            string dateofissue = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtIssueDateEdit")).Text;
    //            string placeofissue = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPlaceOfIssueEdit")).Text;
    //            UserControlDate dateofexpiry = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtExpiryDateEdit"));
    //            string coursecertificateid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseIdEdit")).Text;
    //            string institutionid = ((UserControlAddressType)_gridView.Rows[nCurrentRow].FindControl("ucInstitutionEdit")).SelectedAddress;
    //            if (!IsValidateGridCBT(_gridView, nCurrentRow))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            PhoenixNewApplicantCourse.UpdateNewApplicantCourse(Convert.ToInt32(coursecertificateid)
    //                , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
    //                , Convert.ToInt32(courseid)
    //                , certificatenumber
    //                , General.GetNullableDateTime(dateofissue)
    //                , General.GetNullableDateTime(dateofexpiry.Text)
    //                , placeofissue, General.GetNullableInteger(institutionid), null, null,null
    //               );

    //            _gridView.EditIndex = -1;
    //            BindDataCBT();
    //            SetPageNavigatorCBT();
    //        }
    //        else if (e.CommandName.ToString().ToUpper() == "CBTCANCEL")
    //        {
    //            _gridView.EditIndex = -1;
    //            BindDataCBT();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.HeaderMessage = "Please make the required correction";
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;

    //    }
    //}

    
    //protected void gvCBTCourses_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
        
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
    //            && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
    //        {
    //            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //            db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
    //            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //            ImageButton db1 = (ImageButton)e.Row.FindControl("cmdArchive");
    //            db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
    //            Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
    //            ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
    //            if (att != null) att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
    //            Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");
    //            if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
    //            att.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
    //                + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&cmdname=CBTUPLOAD'); return false;");
    //            Label expdate = e.Row.FindControl("lblExpiryDate") as Label;
    //            System.Web.UI.WebControls.Image imgFlag = e.Row.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
    //            DateTime? d = General.GetNullableDateTime(expdate.Text);
    //            if (d.HasValue)
    //            {
    //                TimeSpan t = d.Value - DateTime.Now;
    //                if (t.Days >= 0 && t.Days < 120)
    //                {
    //                    //e.Row.CssClass = "rowyellow";
    //                    imgFlag.Visible = true;
    //                    imgFlag.ImageUrl = Session["images"] + "/yellow.png";
    //                }
    //                else if (t.Days < 0)
    //                {
    //                    //e.Row.CssClass = "rowred";
    //                    imgFlag.Visible = true;
    //                    imgFlag.ImageUrl = Session["images"] + "/red.png";
    //                }
    //            }
    //        }
    //        else
    //            e.Row.Attributes["onclick"] = "";
    //        UserControlCourse ucCourse = (UserControlCourse)e.Row.FindControl("ddlCourseEdit"); 
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        if (ucCourse != null)
    //        {
    //            ucCourse.CourseList = PhoenixRegistersDocumentCourse.ListDocumentCourse(General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 103, "4")));
    //            if (ucCourse.SelectedCourse == "")
    //                ucCourse.SelectedCourse = drv["FLDCOURSE"].ToString();
    //        }
               
    //        UserControlAddressType ucAddressType = (UserControlAddressType)e.Row.FindControl("ucInstitutionEdit");
    //        DataRowView drvInstitution = (DataRowView)e.Row.DataItem;
    //        if (ucAddressType != null) ucAddressType.SelectedAddress = drvInstitution["FLDINSTITUTIONID"].ToString();

    //    }
    //}
    
    private bool IsValidCourseCertificateCBT(string courseid, string certificatenumber, string dateofissue, UserControlDate dateofexpiry)
    {
        Int16 resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!Int16.TryParse(courseid, out resultInt))
            ucError.ErrorMessage = "Course is required";

        if (certificatenumber.Trim() == "")
            ucError.ErrorMessage = "Certificate Number is required";

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

    private bool IsValidateGridCBT(GridView _gridView, int nCurrentRow)
    {
        string courseid = ((UserControlCourse)_gridView.Rows[nCurrentRow].FindControl("ddlCourseEdit")).SelectedCourse;
        string certificatenumber = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCourseNumberEdit")).Text;
        string dateofissue = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtIssueDateEdit")).Text;
        string placeofissue = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPlaceOfIssueEdit")).Text;
        UserControlDate dateofexpiry = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtExpiryDateEdit"));
        string coursecertificateid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseIdEdit")).Text;

        return IsValidCourseCertificateCBT(courseid, certificatenumber, dateofissue, dateofexpiry);
    }
    private bool IsValidateGridCBT(GridEditableItem _gridView)
    {
        string courseid = ((UserControlCourse)_gridView.FindControl("ddlCourseEdit")).SelectedCourse;
        string certificatenumber = ((RadTextBox)_gridView.FindControl("txtCourseNumberEdit")).Text;
        string dateofissue = ((UserControlDate)_gridView.FindControl("txtIssueDateEdit")).Text;
        string placeofissue = ((RadTextBox)_gridView.FindControl("txtPlaceOfIssueEdit")).Text;
        UserControlDate dateofexpiry = ((UserControlDate)_gridView.FindControl("txtExpiryDateEdit"));
        string coursecertificateid = ((RadLabel)_gridView.FindControl("lblCourseIdEdit")).Text;

        return IsValidCourseCertificateCBT(courseid, certificatenumber, dateofissue, dateofexpiry);
    }

    protected void ddlDocumentCBT_TextChanged(object sender, EventArgs e)
    {
        UserControlCourse dc = (UserControlCourse)sender;
        DataSet ds = new DataSet();
        UserControlDate expirydate;
        //UserControlDate issuedate;
        if (dc.SelectedCourse != "Dummy")
        {
            ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(General.GetNullableInteger(dc.SelectedCourse).Value);
        }
        if (ds.Tables.Count > 0)
        {
            if (dc.ID == "ddlCourseAdd")
            {
                GridFooterItem Item = (GridFooterItem)dc.NamingContainer;
                expirydate = (UserControlDate)Item.FindControl("txtExpiryDateAdd");               
            }
            else
            {
                GridDataItem dataItem = (GridDataItem)dc.NamingContainer;
                expirydate = (UserControlDate)dataItem.FindControl("txtExpiryDateEdit");               
            }
            if (ds.Tables[0].Rows.Count>0 && ds.Tables[0].Rows[0]["FLDEXPIRY"].ToString() == "1")
            {
                expirydate.CssClass = "input_mandatory";
            }
            else
            {
                expirydate.CssClass = "input";
            }
        }
    }

    protected void gvCBTCourses_ItemCommand(object sender, GridCommandEventArgs e)
    {       
        try
        {
            if (e.CommandName.ToString().ToUpper() == "CBTADD")
            {

                GridFooterItem eeditedItem = e.Item as GridFooterItem;
                string courseid = ((UserControlCourse)eeditedItem.FindControl("ddlCourseAdd")).SelectedCourse;
                string certificatenumber = ((RadTextBox)eeditedItem.FindControl("txtCourseNumberAdd")).Text;
                string dateofissue = ((UserControlDate)eeditedItem.FindControl("txtIssueDateAdd")).Text;
                string placeofissue = ((RadTextBox)eeditedItem.FindControl("txtPlaceOfIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)eeditedItem.FindControl("txtExpiryDateAdd"));
                string institutionid = ((UserControlAddressType)eeditedItem.FindControl("ucInstitutionAdd")).SelectedAddress;
                if (!IsValidCourseCertificateCBT(courseid, certificatenumber, dateofissue, dateofexpiry))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixNewApplicantCourse.InsertNewApplicantCourse(Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                    , Convert.ToInt32(courseid)
                    , certificatenumber
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry.Text)
                    , placeofissue, General.GetNullableInteger(institutionid)
                    , null, null, null, null
                    );

                BindDataCBT();
                gvCBTCourses.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string id = ((RadLabel)eeditedItem.FindControl("lblCourseId")).Text;
                PhoenixNewApplicantCourse.ArchiveCrewCourseCertificate(int.Parse(Filter.CurrentNewApplicantSelection), int.Parse(id), 0);            
                BindDataCBT();
                gvCBTCourses.Rebind();
            }
           
            else if (e.CommandName.ToString().ToUpper() == "DELETE")
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string coursecertificateid = ((RadLabel)eeditedItem.FindControl("lblCourseId")).Text;
                PhoenixNewApplicantCourse.DeleteNewApplicantCourse(Convert.ToInt32(coursecertificateid));
                BindDataCBT();
                gvCBTCourses.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string courseid = ((UserControlCourse)eeditedItem.FindControl("ddlCourseEdit")).SelectedCourse;
                string certificatenumber = ((RadTextBox)eeditedItem.FindControl("txtCourseNumberEdit")).Text;
                string dateofissue = ((UserControlDate)eeditedItem.FindControl("txtIssueDateEdit")).Text;
                string placeofissue = ((RadTextBox)eeditedItem.FindControl("txtPlaceOfIssueEdit")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)eeditedItem.FindControl("txtExpiryDateEdit"));
                string coursecertificateid = ((RadLabel)eeditedItem.FindControl("lblCourseIdEdit")).Text;
                string institutionid = ((UserControlAddressType)eeditedItem.FindControl("ucInstitutionEdit")).SelectedAddress;
                if (!IsValidateGridCBT(eeditedItem))
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
                    , placeofissue, General.GetNullableInteger(institutionid), null, null, null
                   );                
                BindDataCBT();
                gvCBTCourses.Rebind();
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

    protected void gvCBTCourses_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCBTCourses.CurrentPageIndex + 1;
            BindDataCBT();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCBTCourses_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
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
                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
               + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&cmdname=CBTUPLOAD'); return false;");
            }
            RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            if (expdate!=null)
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
            UserControlCourse ucCourse = (UserControlCourse)e.Item.FindControl("ddlCourseEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucCourse != null)
            {
                ucCourse.CourseList = PhoenixRegistersDocumentCourse.ListDocumentCourse(General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 103, "4")));
                if (ucCourse.SelectedCourse == "")
                    ucCourse.SelectedCourse = drv["FLDCOURSE"].ToString();
            }
            UserControlAddressType ucAddressType = (UserControlAddressType)e.Item.FindControl("ucInstitutionEdit");
            DataRowView drvInstitution = (DataRowView)e.Item.DataItem;
            if (ucAddressType != null) ucAddressType.SelectedAddress = drvInstitution["FLDINSTITUTIONID"].ToString();
        }
    }
}
