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

public partial class CrewOffshorePendingCourseRequest : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePendingCourseRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePendingCourseRequest.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePendingCourseRequest.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            CourseRequestMenu.AccessRights = this.ViewState;
            CourseRequestMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["p"] != null && Request.QueryString["p"].ToString() != string.Empty)
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["p"]);
                else
                    ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["VESSELID"] = "";
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                }
                else
                {
                    if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                        UcVessel.SelectedVessel = ViewState["VESSELID"].ToString();
                }

                gvReq.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void InitiateCourseRequest(object sender, EventArgs e)
    {

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

        BindData();

    }

    protected void gvReq_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;


        BindData();
        ((UserControlMaskNumber)_gridView.Rows[de.NewEditIndex].FindControl("txtAirfareCost")).Focus();
    }

    protected void BindData()
    {

        string[] alColumns = { "FLDVESSELNAME","FLDFILENO","FLDEMPLOYEENAME", "FLDCOURSENAME","FLDARRANGEDBYNAME", "FLDFROMDATE", "FLDTODATE", "FLDINSTITUTENAME", "FLDPLACE", "FLDCOST","FLDWAGESPAYABLE",
                               "FLDAIRFARECOST", "FLDHOTELCOST" ,"FLDOTHERCOST"};
        string[] alCaptions = {"Vessel Name","File No", "Name", "Docuemnt  Name","Will be Arranged by","From Date", "To Date","Name of Institute" , "Place", "Cost", "Wages Payable",
                                "Airfare Cost", "Hotel Cost","Agency and Other Cost"  };
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = PhoenixCrewOffshoreEmployeeCourse.PendingCourseRequestSearch(General.GetNullableInteger(ViewState["VESSELID"].ToString()), txtEmployeeName.Text,
                 sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],gvReq.PageSize
                                                                  , ref iRowCount, ref iTotalPageCount);
            gvReq.DataSource = dt;
            gvReq.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

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
    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = UcVessel.SelectedVessel;
        BindData();
        gvReq.Rebind();

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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME","FLDFILENO","FLDEMPLOYEENAME", "FLDCOURSENAME","FLDARRANGEDBYNAME", "FLDFROMDATE", "FLDTODATE", "FLDINSTITUTENAME", "FLDPLACE", "FLDCOST","FLDWAGESPAYABLE",
                               "FLDAIRFARECOST", "FLDHOTELCOST" ,"FLDOTHERCOST"};
        string[] alCaptions = { "Vessel Name","File No","Name", "Docuemnt  Name","Will be Arranged by","From Date", "To Date","Name of Institute" , "Place", "Cost", "Wages Payable",
                                "Airfare Cost", "Hotel Cost","Agency and Other Cost"  };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreEmployeeCourse.PendingCourseRequestSearch(General.GetNullableInteger(ViewState["VESSELID"].ToString()), txtEmployeeName.Text,
                sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);

        General.ShowExcel("CourseRequest", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void CourseRequestMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtEmployeeName.Text = "";
                BindData();
                gvReq.Rebind();
            }
            else if (CommandName.ToUpper().Equals("SEARCH"))
            {
               
                BindData();
                gvReq.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReq_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;


    }


    protected void gvReq_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvReq.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReq_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

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


                    BindData();
                    gvReq.Rebind();
                }
            }
            if (e.CommandName.ToUpper().Equals("UNPLAN"))
            {
                GridView _gridView = (GridView)sender;

                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                string strRequestId = ((RadLabel)e.Item.FindControl("lblRequestID")).Text;
                string strEmployeeId = ((RadLabel)e.Item.FindControl("lblEmployeeID")).Text;
                PhoenixCrewOffshoreEmployeeCourse.DeleteCourseMissingRequest(General.GetNullableGuid(strRequestId), int.Parse(strEmployeeId));
                BindData();
                gvReq.Rebind();
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
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
        try
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
                RadLabel lblEmployeeID = (RadLabel)e.Item.FindControl("lblEmployeeID");
                RadLabel lblCourseId = (RadLabel)e.Item.FindControl("lblCourseId");
                RadLabel lblIsCancelRequestYN = (RadLabel)e.Item.FindControl("lblIsCancelRequestYN");

                LinkButton lnkEployeeName = (LinkButton)e.Item.FindControl("lnkEmployeeName");
                LinkButton cmdComplete = (LinkButton)e.Item.FindControl("cmdComplete");
                LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
                if (lblCourseId != null && lblCourseId.Text != "" && lblEmployeeID != null && lblEmployeeID.Text != "")
                {
                    if (cmdComplete != null)
                        cmdComplete.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','"+Session["sitepath"]+"/CrewOffshore/CrewOffshorePendingCourseComplete.aspx?empid=" + lblEmployeeID.Text + "&documentid=" + lblCourseId.Text + "'); return true;");
                }

                if (lnkEployeeName != null && lblEmployeeID != null && lblEmployeeID.Text != "")
                {
                    if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                        lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeID.Text + "&launchedfrom=offshore'); return false;");
                    else
                        lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeID.Text + "&launchedfrom=offshore'); return false;");
                    //lnkEployeeName.Attributes.Add("onclick", "Openpopup('PDForm', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + lblEmployeeid.Text + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
                }

                if (lblIsCancelRequestYN != null && lblIsCancelRequestYN.Text == "0" && cmdDelete != null)
                    cmdDelete.Visible = false;
                //if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
