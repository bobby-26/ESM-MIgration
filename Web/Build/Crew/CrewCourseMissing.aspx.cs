using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewCourseMissing : PhoenixBasePage
{
    string strEmployeeId = string.Empty, strRankId = string.Empty, strVesselId = string.Empty;
    string strJoinDate = string.Empty;
    string strrankname = string.Empty;
	string strcrewplanid = string.Empty;
    string strtype = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			SessionUtil.PageAccessRights(this.ViewState);
			strEmployeeId = Request.QueryString["empid"];
			strJoinDate = Request.QueryString["jdate"];
			strrankname = Request.QueryString["rankname"];
			strcrewplanid = Request.QueryString["crewplanid"];
            strtype = Request.QueryString["type"];
			if (Request.QueryString["t"] != null)
			{
				SetEmployeePrimaryDetails(Request.QueryString["t"]);
			}
			if (Request.QueryString["rnkid"] != null)
			{
				strRankId = Request.QueryString["rnkid"];
			}
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Request Course", "REQUEST",ToolBarDirection.Right);
            CrewMenu.MenuList = toolbar.Show();

            if (!IsPostBack)
			{
                confirm.Attributes.Add("style", "display:none");
                ucDocumentType.DataSource = PhoenixRegistersHard.ListHard(1, 103,0, "1,2,4,5,6");
                ucDocumentType.DataBind();
                ucDocumentType.SelectedValue = "449";
				if (Request.QueryString["vslid"] != null)
				{
					ucVessel.SelectedVessel = Request.QueryString["vslid"];
					ucVessel.Enabled = false;
				}
               
                txtRankName.Text = strrankname;
                ucTravelReason.SelectedReason = "12"; // Training
                ucTravelReason.Enabled = false;

             
            }
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../Crew/CrewCourseMissing.aspx?empid=" + Request.QueryString["empid"] + "&jdate=" + Request.QueryString["jdate"]
                + "&rankname=" + Request.QueryString["rankname"] + "&crewplanid=" + Request.QueryString["rankname"] + "&t=" + Request.QueryString["t"]
                + "&vslid=" + Request.QueryString["vslid"] + "&rnkid=" + Request.QueryString["rnkid"] + "&type=" + Request.QueryString["type"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            CourseRequestMenu.AccessRights = this.ViewState;
            CourseRequestMenu.MenuList = toolbarsub.Show();
            OnClickTravelRequest(null, null);
            BindData();
            BindReqData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("REQUEST"))
            {
                if (gvMissingCourse.Items.Count == 1 && gvMissingCourse.Items[0].Cells.Count == 1)
                {
                    ucError.ErrorMessage = "No Course found to Initiate Request";
                    ucError.Visible = true;
                    return;
                }
                
                RadWindowManager1.RadConfirm("You will not be able to make any changes. Are you sure you want to Initiate Course Request ?", "confirm", 320, 150, null, "Confirm");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
           
                string fromdate = txtFromDate.Text;
                string todate = txtToDate.Text;
                string remarks = txtRemarks.Text;
                string csvCourse = string.Empty;
                foreach (GridDataItem gv in gvMissingCourse.Items)
                {
                    RadCheckBox ck = (RadCheckBox)gv.FindControl("chkSelect");
                    if (ck.Checked==true && ck.Enabled==true)
                    {
                        csvCourse += ((RadLabel)gv.FindControl("lblDocumentId")).Text + ",";
                    }
                }
                if (!IsValidateRequest(fromdate, todate, remarks, csvCourse.TrimEnd(',')))
                {
                    ucError.Visible = true;
                    return;
                }
                
                PhoenixCrewCourseCertificate.InsertCourseRequest(int.Parse(strEmployeeId)
                    , int.Parse(strRankId)
                    , int.Parse(ucVessel.SelectedVessel)
                    , csvCourse.TrimEnd(',')
                    , DateTime.Parse(fromdate)
                    , DateTime.Parse(todate)
                    , byte.Parse(chkAccom.Checked.Value ? "1" : "0")
                    , remarks
                    , General.GetNullableGuid(strcrewplanid));                               

              
                ucStatus.Text   = "Course Request Initiated";
                gvMissingCourse.Rebind();
                gvReq.Rebind();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidateRequest(string fromdate, string todate, string remarks, string csvcourse)
    {
        DateTime resultDate;
		int result;
        ucError.HeaderMessage = "Please provide the following required information";

		if (General.GetNullableInteger(ucVessel.SelectedVessel)==null)
			ucError.ErrorMessage = "Vessel is required.";

        if (!DateTime.TryParse(fromdate, out resultDate))
            ucError.ErrorMessage = "From Date is required.";
        else if (DateTime.TryParse(fromdate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) < 0)
        {
            ucError.ErrorMessage = "From Date should be later than current date";
        }
        if (!DateTime.TryParse(todate, out resultDate))
            ucError.ErrorMessage = "To Date is required.";
        else if (DateTime.TryParse(todate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than current date";
        }
        if (string.IsNullOrEmpty(remarks))
            ucError.ErrorMessage = "Remarks is required.";
        if (string.IsNullOrEmpty(csvcourse))
            ucError.ErrorMessage = "select atleast one or more course.";
		if (chkTravel.Checked == true)
		{
			if (!DateTime.TryParse(ucTravelDate.Text, out resultDate))
				ucError.ErrorMessage = "Travel Date is required.";

            if (!DateTime.TryParse(ucArrivalDate.Text, out resultDate))
                ucError.ErrorMessage = "Arrival Date is required.";

            else if (DateTime.TryParse(ucTravelDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(ucArrivalDate.Text)) > 0)
			{
                ucError.ErrorMessage = "Arrival date should be greater or equal to Travel date";
			}            				
			if (string.IsNullOrEmpty(General.GetNullableString(ucOrigin.SelectedValue)))
				ucError.ErrorMessage = "Origin is required.";

            if (string.IsNullOrEmpty(General.GetNullableString(ucDestination.SelectedValue)))
				ucError.ErrorMessage = "Destination is required.";

			if (int.TryParse(ucTravelReason.SelectedReason, out result) == false)
				ucError.ErrorMessage = "Travel Reason is required.";
		}

        return (!ucError.IsError);
    }
    protected void BindData()
    {
        try
        {
			DataTable dt = PhoenixCrewCourseCertificate.ListCourseMissing(int.Parse(strEmployeeId), int.Parse(strRankId),
                General.GetNullableInteger(ucVessel.SelectedVessel) != null ? General.GetNullableInteger(ucVessel.SelectedVessel) : General.GetNullableInteger(Request.QueryString["vslid"]),
                            General.GetNullableDateTime(strJoinDate),
                            General.GetNullableInteger(ucDocumentType.SelectedValue), chkIncludeCoursesDone.Checked == true ? 0 : 1,General.GetNullableInteger(strtype));

            gvMissingCourse.DataSource = dt;
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMissingCourse_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
               && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                CheckBox ck = e.Row.FindControl("chkSelect") as CheckBox;
                //if (drv["FLDISREQ"].ToString() == "1") ck.Enabled = false;
            }            
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
    private void SetEmployeePrimaryDetails(string type)
    {
        try
        {
			DataTable dt = new DataTable();
			if (type == "p")
			{
				 dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(strEmployeeId));
				 if (dt.Rows.Count > 0)
				 {
					 txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
					 txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
					 txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
					 strRankId = dt.Rows[0]["FLDRANKPOSTED"].ToString();
					 dvTravel.Visible = false;
				 }
                if (string.IsNullOrEmpty(strRankId.ToString()))
                {
                    txtRankName.CssClass = "input_mandatory";
                    Span1.Visible = true;
                }
            }
			else
			{
				 dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
				 if (dt.Rows.Count > 0)
				 {
					 txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
					 txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
					 txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
					 strRankId = dt.Rows[0]["FLDRANK"].ToString();
					 dvTravel.Visible = false;
				 }
                if (string.IsNullOrEmpty(strRankId.ToString()))
                {
                    txtRankName.CssClass = "input_mandatory";
                    Span1.Visible = true;
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
		if (chkTravel.Checked == true)
		{
			dvTravel.Visible = true;
		}
		else
		{
			dvTravel.Visible = false;
		}
	}


    protected void BindReqData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 1; //default desc order
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {

            DataTable dt = PhoenixCrewCourseCertificate.SearchCourseRequest(General.GetNullableInteger(strEmployeeId)
                                                        ,  null
                                                        , null
                                                        , null
                                                        , null
                                                        , null
                                                        , null
                                                        , null
                                                        , null
                                                        , null
                                                        , sortexpression, sortdirection
                                                        , 1, General.ShowRecords(null)
                                                        , ref iRowCount, ref iTotalPageCount);

            gvReq.DataSource = dt;

                    

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }   

    protected void CourseRequestMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string csvRefNo = string.Empty;
                foreach (GridDataItem gv in gvReq.Items)
                {
                    RadCheckBox ck = (RadCheckBox)gv.FindControl("chkSelect");
                    if (ck.Checked==true && ck.Enabled ==true)
                    {
                        csvRefNo += ((RadLabel)gv.FindControl("lblRefNo")).Text + ",";
                    }
                }

                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDREFNO", "FLDFILENO", "FLDNAME", "FLDACCOMYN", "FLDRANKCODE", "FLDZONE", "FLDVESSELNAME", "FLDCOURSE", "FLDFROMDATE", "FLDTODATE", "FLDREMARKS", "FLDTRAVELNO", "FLDCREATEDDATE", "FLDCREATEDBY" };
                string[] alCaptions = { "Ref Number", "File No", "Name", "Accom. Req.", "Rank", "Zone", "VSL", "Courses to do", "Available From", "Available To", "Remarks", "Travel RefNo", "Created Date", "Created By" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;


                DataTable dt = PhoenixCrewCourseCertificate.SearchCourseRequest(General.GetNullableInteger(strEmployeeId)
                                                          , null
                                                          , null
                                                          , null
                                                          , null
                                                          , null
                                                          , null
                                                          , null
                                                          , null
                                                          , csvRefNo.ToString()
                                                          , sortexpression, sortdirection
                                                          , 1, General.ShowRecords(null)
                                                          , ref iRowCount, ref iTotalPageCount);

                General.ShowExcel("COURSE REQUEST", dt, alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    

    protected void gvMissingCourse_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindReqData();
    }

    protected void gvReq_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvReq_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblRemarks = (RadLabel)e.Item.FindControl("lblRemarks");
            UserControlToolTip ucToolTipAddress = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");

            ucToolTipAddress.Position = ToolTipPosition.TopCenter;
            ucToolTipAddress.TargetControlId = lblRemarks.ClientID;
        }
    }

    protected void ucDocumentType_TextChanged(object sender, EventArgs e)
    {
        gvMissingCourse.Rebind();
    }
}
