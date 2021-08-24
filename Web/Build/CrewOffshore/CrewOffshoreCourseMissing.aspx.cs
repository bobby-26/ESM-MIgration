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

public partial class CrewOffshoreCourseMissing : PhoenixBasePage
{

    string strEmployeeId = string.Empty, strRankId = string.Empty, strVesselId = string.Empty, strTrainingMatrixId = string.Empty;
    string strcrewplanid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            strEmployeeId = Request.QueryString["empid"];


            if (Request.QueryString["vesselid"] != null)
            {
                strVesselId = Request.QueryString["vesselid"].ToString();
            }
            if (Request.QueryString["rankid"] != null)
            {
                strRankId = Request.QueryString["rankid"].ToString();
            }
            if (Request.QueryString["trainingmatrixid"] != null)
            {
                strTrainingMatrixId = Request.QueryString["trainingmatrixid"].ToString();
            }
            if (Request.QueryString["crewplanid"] != null)
            {
                strcrewplanid = Request.QueryString["crewplanid"].ToString();
            }
            if (Request.QueryString["trainingmatrixid"] != null)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Suitability", "SUITABILITY");
                toolbar.AddButton("Course Req.", "COURSEREQUEST");

                CrewMenu.MenuList = toolbar.Show();
                CrewMenu.AccessRights = this.ViewState;

                CrewMenu.SelectedMenuIndex = 1;
            }
            if (!IsPostBack)
            {
                ViewState["Charterer"] = "";
                ViewState["vsltype"] = "";
                txtExpectedJoiningDate.Text = DateTime.Now.ToShortDateString();

                if (Request.QueryString["empid"] != null)
                {
                    SetEmployeePrimaryDetails(Request.QueryString["empid"].ToString());
                }

                if (Request.QueryString["vesselid"] != null)
                {
                    if (General.GetNullableInteger(Request.QueryString["vesselid"].ToString()) != null)
                    {
                        ucVessel.SelectedVessel = Request.QueryString["vesselid"].ToString();
                        DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(Request.QueryString["vesselid"].ToString()));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ucVesselType.SelectedVesseltype = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                            ViewState["vsltype"] = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                            ViewState["Charterer"] = ds.Tables[0].Rows[0]["FLDCHARTERER"].ToString();
                        }
                        else
                        {
                            ViewState["vsltype"] = "";
                            ViewState["Charterer"] = "";
                        }
                    }
                }
                if (Request.QueryString["rankid"] != null)
                {
                    if (General.GetNullableInteger(Request.QueryString["rankid"].ToString()) != null)
                        ddlRank.SelectedValue = Request.QueryString["rankid"].ToString();
                }
                if (Request.QueryString["trainingmatrixid"] != null)
                {
                    if (General.GetNullableInteger(Request.QueryString["trainingmatrixid"].ToString()) != null)
                        ddlTrainingMatrix.SelectedValue = Request.QueryString["trainingmatrixid"].ToString();
                }
                if (Request.QueryString["expectedjoiningdate"] != null)
                {
                    if (General.GetNullableDateTime(Request.QueryString["expectedjoiningdate"].ToString()) != null)
                        txtExpectedJoiningDate.Text = Request.QueryString["expectedjoiningdate"].ToString();
                }
                if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"].ToString() != "")
                {
                    SetTrainingMatrix(Request.QueryString["crewplanid"].ToString());
                }
                BindTrainingMatrix();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SUITABILITY"))
            {
                if (Request.QueryString["suitability"] != null)
                {
                    string querystring = "empid=" + Request.QueryString["empid"];
                    if (!string.IsNullOrEmpty(Request.QueryString["personalmaster"]))
                        querystring += "&personalmaster=1";
                    if (!string.IsNullOrEmpty(Request.QueryString["newapplicant"]))
                        querystring += "&newapplicant=1";
                    if (!string.IsNullOrEmpty(Request.QueryString["vesselid"]))
                        querystring += "&vesselid=" + Request.QueryString["vesselid"].ToString();
                    if (!string.IsNullOrEmpty(Request.QueryString["rankid"]))
                        querystring += "&rankid=" + Request.QueryString["rankid"].ToString();
                    if (!string.IsNullOrEmpty(Request.QueryString["trainingmatrixid"]))
                        querystring += "&trainingmatrixid=" + Request.QueryString["trainingmatrixid"].ToString();
                    if (!string.IsNullOrEmpty(Request.QueryString["expectedjoiningdate"]))
                        querystring += "&expectedjoiningdate=" + Request.QueryString["expectedjoiningdate"].ToString();
                    if (!string.IsNullOrEmpty(Request.QueryString["offsignerid"]))
                        querystring += "&offsignerid=" + Request.QueryString["offsignerid"].ToString();
                    if (!string.IsNullOrEmpty(Request.QueryString["crewplanid"]))
                        querystring += "&crewplanid=" + Request.QueryString["crewplanid"].ToString();
                    if (!string.IsNullOrEmpty(Request.QueryString["approval"]))
                        querystring += "&approval=" + Request.QueryString["approval"].ToString();

                    Response.Redirect("CrewOffshoreSuitabilityCheck.aspx?" + querystring, true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void InitiateCourseRequest(object sender, EventArgs e)
    {

    }

  
    protected void gvReq_RowCreated(object sender, GridViewRowEventArgs e)
    {
    }
   
    protected void gvReq_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
    }
   
    protected void gvReq_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }


    protected void gvMissingCourse_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
   
    
  

    private bool IsValidateRequest(string fromdate, string todate)
    {
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";
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
    protected void BindData()
    {
        try
        {
            DataTable dt = PhoenixCrewOffshoreEmployeeCourse.ListCourseMissing(General.GetNullableInteger(ucVessel.SelectedVessel) != null ? General.GetNullableInteger(ucVessel.SelectedVessel) : General.GetNullableInteger(Request.QueryString["vslid"]),
                General.GetNullableInteger(ddlRank.SelectedValue),
                int.Parse(strEmployeeId),
                General.GetNullableDateTime(txtExpectedJoiningDate.Text),
                General.GetNullableInteger(ddlTrainingMatrix.SelectedValue)
                );

            gvMissingCourse.DataSource = dt;

            BindRequest();
            BindCourseCompletion();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindRequest()
    {
        try
        {
            DataTable dt = PhoenixCrewOffshoreEmployeeCourse.ListCourseMissingRequest(General.GetNullableInteger(ucVessel.SelectedVessel) != null ? General.GetNullableInteger(ucVessel.SelectedVessel) : General.GetNullableInteger(Request.QueryString["vslid"]),
                General.GetNullableInteger(ddlRank.SelectedValue),
                int.Parse(strEmployeeId),
                General.GetNullableDateTime(txtExpectedJoiningDate.Text),
                General.GetNullableInteger(ddlTrainingMatrix.SelectedValue)
                );
            gvReq.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
    private void SetEmployeePrimaryDetails(string empid)
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(strEmployeeId));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRankName.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();

                ddlRank.DataSource = PhoenixCrewManagement.CrewRankDeptList(General.GetNullableInteger(dt.Rows[0]["FLDGROUP"].ToString()), null);
                ddlRank.DataBind();

                if (Request.QueryString["rankid"] == null)
                {
                    //if (General.GetNullableInteger(Request.QueryString["rankid"].ToString()) == null)
                    ddlRank.SelectedValue = dt.Rows[0]["FLDRANKPOSTED"].ToString();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void OnClickTravelRequest(object sender, EventArgs e)
    {
    }
    protected void BindTrainingMatrix()
    {
        ddlTrainingMatrix.Items.Clear();
        DataTable dt = new DataTable();
        dt = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixList(
                                            General.GetNullableInteger(ViewState["vsltype"].ToString()),
                                            General.GetNullableInteger(ddlRank.SelectedValue),
                                            General.GetNullableInteger(ViewState["Charterer"].ToString()));
        ddlTrainingMatrix.DataTextField = "FLDMATRIXNAME";
        ddlTrainingMatrix.DataValueField = "FLDMATRIXID";
        if (dt.Rows.Count > 0)
        {
            ddlTrainingMatrix.DataSource = dt;
        }
        ddlTrainingMatrix.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlTrainingMatrix.DataBind();

        if (ddlTrainingMatrix.Items.Count == 2)
            ddlTrainingMatrix.SelectedIndex = 1;
    }
    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            string vesselid = General.GetNullableInteger(ucVessel.SelectedVessel) == null ? lblVesselID.Text : ucVessel.SelectedVessel;
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(vesselid));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ucVesselType.SelectedVesseltype = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                ViewState["vsltype"] = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                ViewState["Charterer"] = ds.Tables[0].Rows[0]["FLDCHARTERER"].ToString();
            }
            BindTrainingMatrix();
            BindData();
        }
    }
    protected void ddlRank_Changed(object sender, EventArgs e)
    {
        BindTrainingMatrix();
        BindData();
    }

    protected void CourseRequestMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

 
   
    protected void ddlTrainingMatrix_Changed(object sender, EventArgs e)
    {
        BindData();
    }

    protected void MenuMissingCourse_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void txtExpectedJoiningDate_Changed(object sender, EventArgs e)
    {
        BindData();
    }
    private void SetTrainingMatrix(string crewplanid)
    {
        DataTable dt = PhoenixCrewPlanning.EditCrewPlan(new Guid(crewplanid));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            if (General.GetNullableInteger(dr["FLDVESSELID"].ToString()) != null)
            {
                DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(dr["FLDVESSELID"].ToString()));
                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ucVesselType.SelectedVesseltype = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                    ViewState["vsltype"] = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                    ViewState["Charterer"] = ds.Tables[0].Rows[0]["FLDCHARTERER"].ToString();
                }
                if (General.GetNullableInteger(dr["FLDRANKID"].ToString()) != null)
                    ddlRank.SelectedValue = dr["FLDRANKID"].ToString();
                if (General.GetNullableInteger(dr["FLDTRAININGMATRIXID"].ToString()) != null)
                {
                    BindTrainingMatrix();
                    if (ddlTrainingMatrix.Items.FindItemByValue(dr["FLDTRAININGMATRIXID"].ToString()) != null)
                        ddlTrainingMatrix.SelectedValue = dr["FLDTRAININGMATRIXID"].ToString();
                }
                if (General.GetNullableDateTime(dr["FLDEXPECTEDJOINDATE"].ToString()) != null)
                    txtExpectedJoiningDate.Text = dr["FLDEXPECTEDJOINDATE"].ToString();
            }
        }
    }
    /*Bind the CourseCompletion*/
    private void BindCourseCompletion()
    {
        try
        {

            string[] alColumns = { "FLDCOURSENAME", "FLDCOURSENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDNAME", "FLDNATIONALITY", "FLDAUTHORITY", "FLDREMARKS" };
            string[] alCaptions = { "Course", "Certificate Number", "Place Of Issue", "Issue Date", "Expiry Date", "Institution", "Nationality", "Issuing Authority", "Remarks" };
            DataTable dt = PhoenixCrewOffshoreEmployeeCourse.ListCourseMissingRequest(General.GetNullableInteger(ucVessel.SelectedVessel) != null ? General.GetNullableInteger(ucVessel.SelectedVessel) : General.GetNullableInteger(Request.QueryString["vslid"]),
               General.GetNullableInteger(ddlRank.SelectedValue),
               int.Parse(strEmployeeId),
               General.GetNullableDateTime(txtExpectedJoiningDate.Text),
               General.GetNullableInteger(ddlTrainingMatrix.SelectedValue));
            DataSet ds = new DataSet();
            //ds.Tables.Add(dt);
            //General.SetPrintOptions("gvCrewCourseCertificate", "Crew Course And Certificate", alCaptions, alColumns, ds);
            gvCrewCourseCertificate.DataSource = dt;

           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  

   
    

    

  

    //protected void gvCrewCourseCertificate_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    try
    //    {
    //        string courseid = ((UserControlCourse)_gridView.Rows[nCurrentRow].FindControl("ddlCourseEdit")).SelectedCourse;
    //        // string institutionid = ((UserControlAddressType)_gridView.Rows[nCurrentRow].FindControl("ucInstitutionEdit")).SelectedAddress;
    //        string certificatenumber = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCourseNumberEdit")).Text;
    //        string dateofissue = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtIssueDateEdit")).Text;
    //        string placeofissue = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPlaceOfIssueEdit")).Text;
    //        UserControlDate dateofexpiry = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtExpiryDateEdit"));
    //        string coursecertificateid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseIdEdit")).Text;
    //        string flagid = ((UserControlNationality)_gridView.Rows[nCurrentRow].FindControl("ddlFlagEdit")).SelectedNationality;
    //        string authority = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAuthorityEdit")).Text;

    //        if (!IsValidateGrid(gvCrewCourseCertificate, nCurrentRow))
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
    //            , null
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
        // string institutionid = ((UserControlAddressType)_gridView.Rows[nCurrentRow].FindControl("ucInstitutionEdit")).SelectedAddress;
        string flagid = ((UserControlNationality)_gridView.Items[nCurrentRow].FindControl("ddlFlagEdit")).SelectedNationality;
        return IsValidCourseCertificate(courseid, certificatenumber, dateofissue, dateofexpiry, placeofissue, "0", flagid);
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
            if (dc.ID == "ddlCourseAdd")
            {
                GridFooterItem footerItem = (GridFooterItem)gvCrewCourseCertificate.MasterTableView.GetItems(GridItemType.Footer)[0];
                expirydate = (UserControlDate)footerItem.FindControl("txtExpiryDateAdd");
                //issuedate = (UserControlDate)gvCrewCourseCertificate.FooterRow.FindControl("txtIssueDateAdd");
            }
            else
            {
                //GridViewRow row = ((GridViewRow)dc.Parent.Parent);
                //GridDataItem footerItem = (GridDataItem)gvCrewCourseCertificate.MasterTableView.Items;
                //expirydate = (UserControlDate)gvCrewCourseCertificate.Rows[row.RowIndex].FindControl("txtExpiryDateEdit");
                //issuedate = (UserControlDate)gvCrewCourseCertificate.Rows[row.RowIndex].FindControl("txtIssueDateEdit");
            }
            if (ds.Tables[0].Rows[0]["FLDEXPIRY"].ToString() == "1")
            {
               // expirydate.CssClass = "input_mandatory";
                //issuedate.CssClass = "input_mandatory";
            }
            else
            {
                //expirydate.CssClass = "input";
                //issuedate.CssClass = "input";
            }

        }
    }



    protected void gvMissingCourse_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvMissingCourse_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
        }
      
    }

    protected void gvMissingCourse_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
              

         
                string strEmployeeId = ((RadLabel)e.Item.FindControl("lblEmployeeID")).Text;
                string strRankId = ((RadLabel)e.Item.FindControl("lblRankID")).Text;
                string strCourse = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;

                UserControlDate txtFromDate = (UserControlDate)e.Item.FindControl("txtFromDate");
                UserControlDate txtToDate = (UserControlDate)e.Item.FindControl("txtToDate");

                if (!IsValidateRequest(txtFromDate.Text, txtToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixCrewOffshoreEmployeeCourse.CrewOffShoreMissingCourseUpdate(int.Parse(strEmployeeId)
                        , General.GetNullableInteger(strCourse)
                        , General.GetNullableDateTime(txtFromDate.Text)
                        , General.GetNullableDateTime(txtToDate.Text)
                        );

                    // gvMissingCourse.EditIndex = -1;
                    BindData();
                    gvMissingCourse.Rebind();
                }
            }
            if (e.CommandName.ToUpper().Equals("REQUEST"))
            {
                

                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                string strEmployeeId = ((RadLabel)e.Item.FindControl("lblEmployeeID")).Text;
                string strRankId = ((RadLabel)e.Item.FindControl("lblRankID")).Text;
                string strCourse = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;

                string fromdate = ((RadLabel)e.Item.FindControl("lblFromDate")).Text;
                string todate = ((RadLabel)e.Item.FindControl("lblToDate")).Text;

                if (!IsValidateRequest(fromdate, todate))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixCrewCourseCertificate.InsertCourseRequest(int.Parse(strEmployeeId)
                    , int.Parse(strRankId)
                    , int.Parse(ucVessel.SelectedVessel)
                    , (strCourse != "" && strCourse != null) ? "," + strCourse + "," : strCourse
                    , General.GetNullableDateTime(fromdate)
                    , General.GetNullableDateTime(todate)
                    , General.GetNullableByte("0")
                    , null
                    , General.GetNullableGuid(strcrewplanid)
                    , null
                    , null
                    , null
                    , null
                    , null
                    , null
                    , null
                    , General.GetNullableInteger(ddlTrainingMatrix.SelectedValue));

                    // gvMissingCourse.EditIndex = -1;
                    BindData();
                    gvMissingCourse.Rebind();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindRequest();
    }

    protected void gvReq_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("UPDATE"))

            {
                
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                string strRequestIdEdit = ((RadLabel)e.Item.FindControl("lblRequestIDEdit")).Text;
                string strEmployeeId = ((RadLabel)e.Item.FindControl("lblEmployeeID")).Text;
                string strRankId = ((RadLabel)e.Item.FindControl("lblRankID")).Text;
                string strCourse = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;

                string arrangedby = string.Empty;
                string instituteid = string.Empty;
                string wagespayable = string.Empty;
                string coursecostid = string.Empty;
                string airfarecost = string.Empty;
                string hotelcost = string.Empty;
                string othercost = string.Empty;

                UserControlDate txtFromDate = (UserControlDate)e.Item.FindControl("txtFromDate");
                UserControlDate txtToDate = (UserControlDate)e.Item.FindControl("txtToDate");


                RadioButtonList cblArrangedBy = (RadioButtonList)e.Item.FindControl("rblArrangedBy");
                if (General.GetNullableInteger(cblArrangedBy.SelectedValue) != null)
                    arrangedby = cblArrangedBy.SelectedValue;

                instituteid = ((UserControlAddressType)e.Item.FindControl("ucInstitution")).SelectedAddress;
                wagespayable = ((CheckBox)e.Item.FindControl("chkWagesPayable")).Checked == true ? "1" : "0";
                airfarecost = ((UserControlMaskNumber)e.Item.FindControl("txtAirfareCost")).Text;
                hotelcost = ((UserControlMaskNumber)e.Item.FindControl("txtHotelCost")).Text;
                othercost = ((UserControlMaskNumber)e.Item.FindControl("txtOtherCost")).Text;


                DataTable dt = PhoenixCrewOffshoreCourseCost.ListCourseCost(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null
                    , General.GetNullableInteger(strCourse)
                    , General.GetNullableInteger(instituteid));

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    coursecostid = dr["FLDCOURSECOSTID"].ToString();
                }

                if (!IsValidateRequest(txtFromDate.Text, txtToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixCrewOffshoreEmployeeCourse.CrewOffShoreCourseRequestUpdate(new Guid(strRequestIdEdit)
                    , General.GetNullableDateTime(txtFromDate.Text)
                    , General.GetNullableDateTime(txtToDate.Text)
                    , General.GetNullableInteger(arrangedby)
                    , General.GetNullableInteger(instituteid)
                    , General.GetNullableInteger(coursecostid)
                    , General.GetNullableInteger(wagespayable)
                    , General.GetNullableDecimal(airfarecost)
                    , General.GetNullableDecimal(hotelcost)
                    , General.GetNullableDecimal(othercost));

                    //gvReq.EditIndex = -1;
                    BindData();
                }
            }
            if (e.CommandName.ToUpper().Equals("UNPLAN"))
            {
             

                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                string strRequestId = ((RadLabel)e.Item.FindControl("lblRequestID")).Text;
                string strEmployeeId = ((RadLabel)e.Item.FindControl("lblEmployeeID")).Text;
                PhoenixCrewOffshoreEmployeeCourse.DeleteCourseMissingRequest(General.GetNullableGuid(strRequestId), int.Parse(strEmployeeId));
                BindData();
                BindCourseCompletion();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReq_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            UserControlAddressType uc = (UserControlAddressType)e.Item.FindControl("ucInstitution");
            RadioButtonList rblArrange = (RadioButtonList)e.Item.FindControl("rblArrangedBy");

            if (uc != null)
            {
                DataSet ds = PhoenixRegistersAddress.ListAddress("238");

                uc.AddressList = ds;
                uc.DataBind();

                if (General.GetNullableInteger(drv["FLDINSTITUTEID"].ToString()) != null)
                    uc.SelectedAddress = drv["FLDINSTITUTEID"].ToString();
            }
            if (rblArrange != null)
            {
                if (General.GetNullableInteger(drv["FLDARRANGEDBY"].ToString()) != null)
                {
                    rblArrange.SelectedValue = drv["FLDARRANGEDBY"].ToString();
                }
            }
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure to cancel course request?')");
            }
            else
            {
                e.Item.Attributes["onclick"] = "";
            }
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            //if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        }
    }

    protected void gvCrewCourseCertificate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCourseCompletion();
    }

    protected void gvCrewCourseCertificate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid _gridView = (RadGrid)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {
           if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {
                try
                {
                    string courseid = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;
                    // string institutionid = ((UserControlAddressType)e.Item.FindControl("ucInstitutionEdit")).SelectedAddress;
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
                        PhoenixCrewCourseCertificate.InsertCrewCourseCertificate(Convert.ToInt32(strEmployeeId)
                      , Convert.ToInt32(courseid)
                      , certificatenumber
                      , General.GetNullableDateTime(dateofissue)
                      , General.GetNullableDateTime(dateofexpiry.Text)
                      , placeofissue
                      , null
                      , null//General.GetNullableString(remarks)
                      , General.GetNullableInteger(flagid)
                      , General.GetNullableString(authority)
                      , null
                      );
                      
                        BindCourseCompletion();
                        gvCrewCourseCertificate.Rebind();
                    }
                    else
                    {

                        PhoenixCrewCourseCertificate.UpdateCrewCourseCertificate(Convert.ToInt32(coursecertificateid)
                            , Convert.ToInt32(strEmployeeId)
                            , Convert.ToInt32(courseid)
                            , certificatenumber
                            , General.GetNullableDateTime(dateofissue)
                            , General.GetNullableDateTime(dateofexpiry.Text)
                            , placeofissue
                            , null
                            , General.GetNullableInteger(flagid)
                            , General.GetNullableString(authority)
                            , null
                           );
                        
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
            if(e.CommandName.ToUpper()=="DELETE")
            {
                try
                {
                   
                    string coursecertificateid = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;

                    PhoenixCrewCourseCertificate.DeleteCrewCourseCertificate(Convert.ToInt32(coursecertificateid));
                    BindCourseCompletion();
                    gvCrewCourseCertificate.Rebind();

                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;

                }
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
        if (e.Item is GridDataItem)
        {
            //// Get the LinkButton control in the first cell
            //LinkButton _doubleClickButton = (LinkButton)e.Item.Cells[0].Controls[0];
            //// Get the javascript which is assigned to this LinkButton
            //string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            //// Add this javascript to the onclick Attribute of the row
            //e.Item.Attributes["onclick"] = _jsDouble;
        }

        if (e.Item is GridEditableItem)
        {
           
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (db != null) if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
                if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
                RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
                System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
                DateTime? d = null;

                if (expdate != null) d = General.GetNullableDateTime(expdate.Text);
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
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblCourseId");
                RadLabel lblCourseId = (RadLabel)e.Item.FindControl("lblCourseIdEdit");
                UserControlCourse uccourse = (UserControlCourse)e.Item.FindControl("ddlCourseEdit");
                if (lblCourseId != null && lblCourseId.Text != "")
                    uccourse.SelectedCourse = lblCourseId.Text;

           

            //UserControlAddressType ucAddressType = (UserControlAddressType)e.Item.FindControl("ucInstitutionEdit");
            //DataRowView drv = (DataRowView)e.Item.DataItem;
            //if (ucAddressType != null) ucAddressType.SelectedAddress = drv["FLDINSTITUTIONID"].ToString();

            UserControlCourse ucCourse = (UserControlCourse)e.Item.FindControl("ddlCourseEdit");
            DataRowView drvCourse = (DataRowView)e.Item.DataItem;
            if (ucCourse != null) ucCourse.SelectedCourse = drvCourse["FLDCOURSE"].ToString();

            UserControlNationality ucFlag = (UserControlNationality)e.Item.FindControl("ddlFlagEdit");
            DataRowView drvFlag = (DataRowView)e.Item.DataItem;
            if (ucFlag != null) ucFlag.SelectedNationality = drvFlag["FLDFLAGID"].ToString();

        }
    }
}
