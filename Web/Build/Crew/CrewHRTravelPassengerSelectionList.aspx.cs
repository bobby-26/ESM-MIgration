using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewHRTravelPassengerSelectionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Employees", "EMPLOYEE");
            toolbarmain.AddButton("Family", "FAMILY");

            MenuHRTravelPassengerSelection.AccessRights = this.ViewState;
            MenuHRTravelPassengerSelection.MenuList = toolbarmain.Show();
            MenuHRTravelPassengerSelection.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewHRTravelPassengerSelectionList.aspx", "Find", "search.png", "FIND");

            MenuRegistersOfficeStaff.AccessRights = this.ViewState;
            MenuRegistersOfficeStaff.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (Request.QueryString["travelrequestid"] != null)
                    ViewState["travelrequestid"] = Request.QueryString["travelrequestid"].ToString();
                else
                    ViewState["travelrequestid"] = "";

                if (Request.QueryString["personalinfosn"] != null)
                    ViewState["personalinfosn"] = Request.QueryString["personalinfosn"].ToString();
                else
                    ViewState["personalinfosn"] = "";

            }
            BindData();
            SetPageNavigator();

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void HRTravelPassengerSelection_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EMPLOYEE"))
            {
                Response.Redirect("../Crew/CrewHRTravelPassengerSelectionList.aspx?travelrequestid=" + ViewState["travelrequestid"].ToString() + "&personalinfosn=" + ViewState["personalinfosn"].ToString(), false);
            }
            else if (dce.CommandName.ToUpper().Equals("FAMILY"))
            {
                Response.Redirect("../Crew/CrewHRTravelPassengerFamilySelectionList.aspx?travelrequestid=" + ViewState["travelrequestid"].ToString() + "&personalinfosn=" + ViewState["personalinfosn"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersOfficeStaff_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        //DataSet ds = PhoenixRegistersOfficeStaff.OfficeStaffSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
        //    General.GetNullableString(txtEmployeeNumber.Text),
        //    txtFirstName.Text,
        //     General.GetNullableInteger(ucDesignation.SelectedDesignation),
        //    sortexpression,
        //    sortdirection,
        //    (int)ViewState["PAGENUMBER"],
        //    General.ShowRecords(null),
        //    ref iRowCount,
        //    ref iTotalPageCount);
        DataSet ds = PhoenixRegistersOfficeStaff.OfficeStaffApprovalmappingSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    General.GetNullableString(txtEmployeeNumber.Text),
                                                                    txtFirstName.Text,
                                                                     General.GetNullableInteger(ucDesignation.SelectedDesignation),int.Parse("1"),                                                                    sortexpression,
                                                                    sortdirection,
                                                                    (int)ViewState["PAGENUMBER"],
                                                                    General.ShowRecords(null),
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount);
        if (ds.Tables[0].Rows.Count > 0)
        {

            gvOfficeStaff.DataSource = ds;
            gvOfficeStaff.DataBind();
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvOfficeStaff);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }

    protected void gvOfficeStaff_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper() == "ADDPASSENGER")
            {
                PhoenixCrewHRTravelRequest.HRTravelPassengerInsert(new Guid(ViewState["travelrequestid"].ToString())
                    , General.GetNullableInteger(ViewState["personalinfosn"].ToString())
                    , null
                    , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblOfficeStaffid")).Text)
                    , null
                    , General.GetNullableString(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFirstName")).Text)
                    , General.GetNullableString(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMiddleName")).Text)
                    , General.GetNullableString(((Label)_gridView.Rows[nCurrentRow].FindControl("lblLastName")).Text)
                    , null
                    , null
                    , null
                    , null
                    );

                ucstatus.Text = "Passenger has been added.";
            }
            String script = String.Format("javascript:fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvOfficeStaff_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }


    protected void gvOfficeStaff_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }


    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvOfficeStaff.SelectedIndex = -1;
        gvOfficeStaff.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }
    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }
    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }
    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
