using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CrewOffshorePendingCourseComplete : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["EMPLOYEEID"] = "";
                ViewState["DOCUMENTID"] = "";
                if (Request.QueryString["empid"] != null && Request.QueryString["empid"].ToString() != string.Empty)
                    ViewState["EMPLOYEEID"] = Request.QueryString["empid"].ToString();
                if (Request.QueryString["documentid"] != null && Request.QueryString["documentid"].ToString() != string.Empty)
                    ViewState["DOCUMENTID"] = Request.QueryString["documentid"].ToString();

            }
            BindCourseCompletion();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void BindCourseCompletion()
    {

        string[] alColumns = { "FLDHARDNAME", "FLDCOURSENAME", "FLDCOURSENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDINSTITUTENAME", "FLDNATIONALITY", "FLDAUTHORITY" };
        string[] alCaptions = { "Type", "Course", "Certificate Number", "Place of Issue", "Issue Date", "Expiry Date", "Name of Institute", "Nationality", "Issuing Authority" };

        try
        {
            DataTable dt = PhoenixCrewOffshoreEmployeeCourse.ListCourseMissingComplete(General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()),
                                                                                  General.GetNullableInteger(ViewState["DOCUMENTID"].ToString()));
            gvCrewCourseCertificate.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewCourseCertificate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindCourseCompletion();
    }



    protected void gvCrewCourseCertificate_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string coursecertificateid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseId")).Text;

            PhoenixCrewCourseCertificate.DeleteCrewCourseCertificate(Convert.ToInt32(coursecertificateid));
            BindCourseCompletion();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    //protected void gvCrewCourseCertificate_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;

    //        if (_gridView.EditIndex > -1)
    //        {
    //            if (!IsValidateGrid(_gridView, _gridView.EditIndex))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }
    //            _gridView.UpdateRow(_gridView.EditIndex, false);
    //        }
    //        _gridView.SelectedIndex = e.NewEditIndex;
    //        _gridView.EditIndex = e.NewEditIndex;
    //        BindCourseCompletion();
    //        DropDownList institution = (DropDownList)_gridView.Rows[e.NewEditIndex].FindControl("ucInstitutionEdit").FindControl("ddlAddressType");
    //        institution.Attributes["style"] = "width:250px";
    //        DropDownList course = (DropDownList)_gridView.Rows[e.NewEditIndex].FindControl("ddlCourseEdit").FindControl("ddlCourse");
    //        course.Attributes["style"] = "width:250px";

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}



    //protected void gvCrewCourseCertificate_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    try
    //    {
    //        string courseid = ((UserControlCourse)_gridView.Rows[nCurrentRow].FindControl("ddlCourseEdit")).SelectedCourse;
    //        string institutionid = ((UserControlAddressType)_gridView.Rows[nCurrentRow].FindControl("ucInstitutionEdit")).SelectedAddress;
    //        string certificatenumber = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCourseNumberEdit")).Text;
    //        string dateofissue = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtIssueDateEdit")).Text;
    //        string placeofissue = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPlaceOfIssueEdit")).Text;
    //        UserControlDate dateofexpiry = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtExpiryDateEdit"));
    //        string coursecertificateid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseIdEdit")).Text;
    //        string flagid = ((UserControlNationality)_gridView.Rows[nCurrentRow].FindControl("ddlFlagEdit")).SelectedNationality;
    //        string authority = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAuthorityEdit")).Text;

    //        if (!IsValidateGrid(_gridView, nCurrentRow))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }

    //        PhoenixCrewCourseCertificate.UpdateCrewCourseCertificate(Convert.ToInt32(coursecertificateid)
    //            , Convert.ToInt32(Filter.CurrentCrewSelection)
    //            , Convert.ToInt32(courseid)
    //            , certificatenumber
    //            , General.GetNullableDateTime(dateofissue)
    //            , General.GetNullableDateTime(dateofexpiry.Text)
    //            , placeofissue
    //            , General.GetNullableInteger(institutionid)
    //            , General.GetNullableInteger(flagid)
    //            , General.GetNullableString(authority)
    //            , null
    //           );

    //        _gridView.EditIndex = -1;
    //        BindCourseCompletion();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.HeaderMessage = "Please make the required correction";
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;

    //    }
    //}

    //protected void gvCrewCourseCertificate_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.SelectedIndex = e.NewSelectedIndex;
    //    _gridView.EditIndex = -1;
    //}
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

    private bool IsValidateGrid(RadGrid _gridView, int nCurrentRow)
    {
        string courseid = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblCourseId")).Text;
        string certificatenumber = ((RadTextBox)_gridView.Items[nCurrentRow].FindControl("txtCourseNumberEdit")).Text;
        string dateofissue = ((UserControlDate)_gridView.Items[nCurrentRow].FindControl("txtIssueDateEdit")).Text;
        string placeofissue = ((RadTextBox)_gridView.Items[nCurrentRow].FindControl("txtPlaceOfIssueEdit")).Text;
        UserControlDate dateofexpiry = ((UserControlDate)_gridView.Items[nCurrentRow].FindControl("txtExpiryDateEdit"));
        string coursecertificateid = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblCourseIdEdit")).Text;
        string institutionid = ((UserControlAddressType)_gridView.Items[nCurrentRow].FindControl("ucInstitutionEdit")).SelectedAddress;
        string flagid = ((UserControlNationality)_gridView.Items[nCurrentRow].FindControl("ddlFlagEdit")).SelectedNationality;
        return IsValidCourseCertificate(courseid, certificatenumber, dateofissue, dateofexpiry, placeofissue, institutionid, flagid);
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
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidateRequest(string fromdate, string todate)
    {
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!DateTime.TryParse(fromdate, out resultDate))
            ucError.ErrorMessage = "From Date is required.";
        if (!DateTime.TryParse(todate, out resultDate))
            ucError.ErrorMessage = "To Date is required.";

        if (General.GetNullableDateTime(fromdate) != null && General.GetNullableDateTime(todate) != null)
        {
            if (DateTime.TryParse(fromdate, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(todate)) > 0)
                ucError.ErrorMessage = "To date should be greater or equal to from date";
        }

        return (!ucError.IsError);
    }
    protected void ddlDocument_TextChanged(object sender, EventArgs e)
    {
        UserControlCourse dc = (UserControlCourse)sender;
        DataSet ds = new DataSet();
        UserControlDate expirydate;
        //UserControlDate issuedate;
        GridViewRow gv = (GridViewRow)dc.NamingContainer;
        TextBox txtCourseNumberAdd = (TextBox)gv.FindControl("txtCourseNumberAdd");
        if (dc.SelectedCourse != "Dummy")
        {
            ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(General.GetNullableInteger(dc.SelectedCourse).Value);
        }
        if (ds.Tables.Count > 0)
        {

            GridDataItem row = ((GridDataItem)dc.Parent.Parent);
            expirydate = (UserControlDate)row.FindControl("txtExpiryDateEdit");
            //issuedate = (UserControlDate)gvCrewCourseCertificate.Rows[row.RowIndex].FindControl("txtIssueDateEdit");

            if (ds.Tables[0].Rows[0]["FLDEXPIRY"].ToString() == "1")
            {
                expirydate.CssClass = "input_mandatory";
                //issuedate.CssClass = "input_mandatory";
            }
            else
            {
                expirydate.CssClass = "input";
                //issuedate.CssClass = "input";
            }

        }
    }
    protected void ShowExcel()
    {

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDCOURSENAME","FLDARRANGEDBYNAME", "FLDFROMDATE", "FLDTODATE", "FLDINSTITUTENAME", "FLDPLACE", "FLDCOST","FLDWAGESPAYABLE",
                               "FLDAIRFARECOST", "FLDHOTELCOST" ,"FLDOTHERCOST"};
        string[] alCaptions = { "Name", "Docuemnt  Name","Will be Arranged by","From Date", "To Date","Name of Institute" , "Place", "Cost", "Wages Payable",
                                "Airfare Cost", "Hotel Cost","Agency and Other Cost"  };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataTable dt = PhoenixCrewOffshoreEmployeeCourse.ListCourseMissingComplete(General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()),
                                                                                    General.GetNullableInteger(ViewState["DOCUMENTID"].ToString()));
        General.ShowExcel("CourseRequest", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void gvCrewCourseCertificate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewCourseCertificate.CurrentPageIndex + 1;
            BindCourseCompletion();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewCourseCertificate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid _gridView = (RadGrid)sender;
        GridDataItem dataItem = e.Item as GridDataItem;

        int nCurrentRow =int.Parse(e.CommandArgument.ToString());
        //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {
           
            //if (e.CommandName.ToString().ToUpper() == "EDIT")
            //{
            //    //_gridView.SelectedIndex = nCurrentRow;
            //    //_gridView.EditIndex = nCurrentRow;
            //    BindCourseCompletion();
            //    RadComboBox institution = (RadComboBox)e.Item.FindControl("ucInstitutionEdit").FindControl("ddlAddressType");
            //    institution.Attributes["style"] = "width:250px";
            //    RadComboBox course = (RadComboBox)e.Item.FindControl("ddlCourseEdit").FindControl("ddlCourse");
            //    course.Attributes["style"] = "width:250px";
            //}
            //else
            if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {
                try
                {
                    string courseid = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;
                    string institutionid = ((UserControlAddressType)e.Item.FindControl("ucInstitutionEdit")).SelectedAddress;
                    string certificatenumber = ((RadTextBox)e.Item.FindControl("txtCourseNumberEdit")).Text;
                    string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
                    string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
                    UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit"));
                    string coursecertificateid = ((RadLabel)e.Item.FindControl("lblCourseIdEdit")).Text;
                    string flagid = ((UserControlNationality)e.Item.FindControl("ddlFlagEdit")).SelectedNationality;
                    string authority = ((RadTextBox)e.Item.FindControl("txtAuthorityEdit")).Text;

                    if (!IsValidateGrid(_gridView, nCurrentRow))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    if (coursecertificateid == null || coursecertificateid == "")
                    {
                        PhoenixCrewCourseCertificate.InsertCrewCourseCertificate(Convert.ToInt32(ViewState["EMPLOYEEID"].ToString())
                      , Convert.ToInt32(courseid)
                      , certificatenumber
                      , General.GetNullableDateTime(dateofissue)
                      , General.GetNullableDateTime(dateofexpiry.Text)
                      , placeofissue
                      , General.GetNullableInteger(institutionid)
                      , null//General.GetNullableString(remarks)
                      , General.GetNullableInteger(flagid)
                      , General.GetNullableString(authority)
                      , null
                      );
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                     "BookMarkScript", "fnReloadList('codehelp1', null, null);", true);
                    
                        BindCourseCompletion();
                        gvCrewCourseCertificate.Rebind();
                    }
                    else
                    {

                        PhoenixCrewCourseCertificate.UpdateCrewCourseCertificate(Convert.ToInt32(coursecertificateid)
                            , Convert.ToInt32(ViewState["EMPLOYEEID"].ToString())
                            , Convert.ToInt32(courseid)
                            , certificatenumber
                            , General.GetNullableDateTime(dateofissue)
                            , General.GetNullableDateTime(dateofexpiry.Text)
                            , placeofissue
                            , General.GetNullableInteger(institutionid)
                            , General.GetNullableInteger(flagid)
                            , General.GetNullableString(authority)
                            , null
                           );
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                     "BookMarkScript", "fnReloadList('codehelp1', null, null);", true);
                      
                        BindCourseCompletion();
                        gvCrewCourseCertificate.Rebind();
                    }


                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "Please make the required correction";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            else if (e.CommandName.ToString().ToUpper() == "CANCEL")
            {

            
                BindCourseCompletion();
                gvCrewCourseCertificate.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvCrewCourseCertificate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridDataItem)
        //{
        //    // Get the LinkButton control in the first cell
        //    LinkButton _doubleClickButton = (LinkButton)e.Item.Cells[0].Controls[0];
        //    // Get the javascript which is assigned to this LinkButton
        //    string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
        //    // Add this javascript to the onclick Attribute of the row
        //    e.Item.Attributes["onclick"] = _jsDouble;
        //}

        if (e.Item is GridDataItem)
        {

            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if(db !=null) db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            if (db != null) if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            ImageButton cme = (ImageButton)e.Item.FindControl("cmdEdit");
            if(cme !=null) if(!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
            RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            DateTime? d = null;
            if (expdate!=null) d = General.GetNullableDateTime(expdate.Text);
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
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblCourseId");
            RadLabel lblCourseId = (RadLabel)e.Item.FindControl("lblCourseIdEdit");
            UserControlCourse uccourse = (UserControlCourse)e.Item.FindControl("ddlCourseEdit");
            if (lblCourseId != null && lblCourseId.Text != "")
                uccourse.SelectedCourse = lblCourseId.Text;



            UserControlAddressType ucAddressType = (UserControlAddressType)e.Item.FindControl("ucInstitutionEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucAddressType != null) ucAddressType.SelectedAddress = drv["FLDINSTITUTIONID"].ToString();

            UserControlCourse ucCourse = (UserControlCourse)e.Item.FindControl("ddlCourseEdit");
            DataRowView drvCourse = (DataRowView)e.Item.DataItem;
            if (ucCourse != null) ucCourse.SelectedCourse = drvCourse["FLDCOURSE"].ToString();

            UserControlNationality ucFlag = (UserControlNationality)e.Item.FindControl("ddlFlagEdit");
            DataRowView drvFlag = (DataRowView)e.Item.DataItem;
            if (ucFlag != null) ucFlag.SelectedNationality = drvFlag["FLDFLAGID"].ToString();

        }
    }
}
