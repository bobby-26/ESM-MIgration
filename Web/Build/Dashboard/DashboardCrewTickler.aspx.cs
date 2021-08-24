using System;
using System.Collections.Specialized;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;

public partial class DashboardCrewTickler : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SelectedOption();

                PhoenixToolbar toolbar1 = new PhoenixToolbar();
                toolbar1.AddImageButton("../Options/OptionsTickler.aspx", "Find", "search.png", "FIND");
                MenuTickler.AccessRights = this.ViewState;
                MenuTickler.MenuList = toolbar1.Show();

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Send", "SAVE");
                MenuPhoenixBroadcast.AccessRights = this.ViewState;
                MenuPhoenixBroadcast.MenuList = toolbar.Show();

                lblUserCode.Attributes.Add("style", "visibility:hidden");
                cmdShowUsers.Attributes.Add("onclick", "return showPickList('spnPickListUsers', 'codehelp1', '', '../Common/CommonPicklistMultipleUser.aspx?activeyn=172', true); ");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = 1;
                ViewState["MODE"] = "0";
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SelectedOption()
    {
        NameValueCollection nvc = new NameValueCollection();
        nvc.Clear();

        nvc.Add("Url", HttpContext.Current.Request.Url.AbsolutePath);
        nvc.Add("APP", "CREW");
        nvc.Add("Option", "TCK");

       

        //nvc.Add("Url", HttpContext.Current.Request.Url.AbsolutePath);
        //if (Request.QueryString["APP"] != null)
        //    nvc.Add("App", Request.QueryString["APP"].ToString());
        //if (Request.QueryString["OPT"] != null)
        //    nvc.Add("Option", Request.QueryString["OPT"].ToString());

        Filter.CurrentDashboardLastSelection = nvc;
        PhoenixDashboardOption.DashboardLastSelected(nvc);
    }

    protected void MenuTickler_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PhoenixBroadcast_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidTickler(lblUserCode.Text, txtReminderDate.Text + " " + txtTime.Text, txtMessage.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCommonTickler.Send(lblUserCode.Text, DateTime.Parse(txtReminderDate.Text + " " + txtTime.Text).ToUniversalTime()
                    , null
                    , txtMessage.Text);
                BindData();
                Reset();
            }
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
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
        string[] alColumns = { "NAME", "DESCRIPTION", "POSTEDDATE", "FLDDONEYN", "FLDDONEREMARKS", "ACTIONBY", "ACTIONDATE" };
        string[] alCaptions = { "Posted By", "Comments", "Date", "Completed", "Comments", "Action By", "Action Date" };

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCommonTickler.TicklerSearch((byte?)General.GetNullableInteger(ddlStatus.SelectedValue)
           , General.GetNullableDateTime(txtFromDate.Text), General.GetNullableDateTime(txtToDate.Text)
           , (byte?)General.GetNullableInteger(ddlCompleted.SelectedValue)
           , sortexpression, sortdirection
           , (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);


        if (dt.Rows.Count > 0)
        {
            gvTickler.DataSource = dt;
            gvTickler.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvTickler);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        SetPageNavigator();
    }
    protected void gvTickler_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("SNOOZE"))
            {
                _gridView.EditIndex = nCurrentRow;
                ViewState["MODE"] = "0";
                BindData();
                TextBox txt = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDoneRemarks"));
                txt.Enabled = false;
                txt.CssClass = "reaonlytextbox";
                ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkDoneYN")).Enabled = false;
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTickler_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            ImageButton db = (ImageButton)e.Row.FindControl("cmdEdit");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            ImageButton snz = (ImageButton)e.Row.FindControl("cmdSnooze");
            if (snz != null) snz.Visible = SessionUtil.CanAccess(this.ViewState, snz.CommandName);

            DataRowView drv = (DataRowView)e.Row.DataItem;

            if (db != null && General.GetNullableDateTime(drv["FLDCOMPLETEDDATE"].ToString()).HasValue)
            {
                db.Visible = false;
            }
            if (snz != null && drv["FLDSENTYN"].ToString() == "1")
            {
                snz.Visible = false;
            }
            Label lbtn = (Label)e.Row.FindControl("lblRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucRemarksTT");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

            lbtn = (Label)e.Row.FindControl("lblDoneRemarks");
            if (lbtn != null)
            {
                uct = (UserControlToolTip)e.Row.FindControl("ucDoneRemarksTT");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
            if (ViewState["MODE"].ToString() == "0")
            {
                TextBox txt = ((TextBox)e.Row.FindControl("txtDoneRemarks"));
                if (txt != null)
                {
                    txt.Enabled = false;
                    txt.CssClass = "reaonlytextbox";
                    ((CheckBox)e.Row.FindControl("chkDoneYN")).Enabled = false;
                }
            }
            else
            {
                UserControlDate date = ((UserControlDate)e.Row.FindControl("txtRescheduleDate"));
                if (date != null)
                {
                    date.Enabled = false;
                    date.CssClass = "readonlytextbox";
                    TextBox txt = ((TextBox)e.Row.FindControl("txtReScheduleTime"));
                    txt.Enabled = false;
                    txt.CssClass = "readonlytextbox";
                }
            }
        }
    }

    protected void gvTickler_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTickler_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.SelectedIndex = e.NewEditIndex;
            ViewState["MODE"] = "1";
            _gridView.EditIndex = e.NewEditIndex;
            BindData();
            UserControlDate date = ((UserControlDate)_gridView.Rows[e.NewEditIndex].FindControl("txtRescheduleDate"));
            date.Enabled = false;
            date.CssClass = "readonlytextbox";
            TextBox txt = ((TextBox)_gridView.Rows[e.NewEditIndex].FindControl("txtReScheduleTime"));
            txt.Enabled = false;
            txt.CssClass = "readonlytextbox";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTickler_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;

        try
        {
            Guid ticklerid = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            UserControlDate date = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtRescheduleDate"));
            TextBox time = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReScheduleTime"));
            string doneremarks = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDoneRemarks")).Text;
            CheckBox chk = ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkDoneYN"));
            if (ViewState["MODE"].ToString() == "1")
            {
                if (!IsDoneRemarksValid(chk.Checked, doneremarks))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCommonTickler.UpdateAsRead(ticklerid, 1, doneremarks);
            }
            else
            {
                string reminderdate = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblReminderDate")).Text;
                if (!IsValidReschedule(reminderdate, date.Text + " " + time.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCommonTickler.UpdateRescheduleDate(ticklerid, DateTime.Parse(date.Text + " " + time.Text).ToUniversalTime());
            }
            _gridView.EditIndex = -1;
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
        gv.Rows[0].Attributes["onclick"] = "";
    }
    private bool IsDoneRemarksValid(bool completedYN, string doneRemarks)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!completedYN)
            ucError.ErrorMessage = "Please tick the 'Completed.'";

        if (doneRemarks.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        return (!ucError.IsError);
    }
    private bool IsValidReschedule(string reminderdate, string rescheduledate)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;

        if (!DateTime.TryParse(rescheduledate, out resultDate))
        {
            ucError.ErrorMessage = "Reschedule Date and Time is required.";
        }
        else if (DateTime.TryParse(rescheduledate, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(reminderdate)) <= 0)
        {
            ucError.ErrorMessage = "Reschedule Date and Time should be later than Reminder Date";
        }

        return (!ucError.IsError);
    }
    private bool IsValidTickler(string usercode, string date, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (usercode.Trim().Equals(""))
            ucError.ErrorMessage = "User is required.";

        if (!General.GetNullableDateTime(date).HasValue)
            ucError.ErrorMessage = "Reminder On Date and Time is required.";

        if (remarks.Trim().Equals(""))
            ucError.ErrorMessage = "Message is required.";

        return (!ucError.IsError);
    }
    private void Reset()
    {
        txtUser.Text = "";
        lblUserCode.Text = "";
        txtMessage.Text = "";
        txtReminderDate.Text = "";
        txtTime.Text = "";
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
            return true;

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

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    protected void btnGo_Click(object sender, EventArgs e)
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
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();

    }
}
