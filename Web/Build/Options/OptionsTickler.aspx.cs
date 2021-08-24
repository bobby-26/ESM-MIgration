using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
public partial class OptionsTickler : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddImageButton("../Options/OptionsTickler.aspx", "Find", "search.png", "FIND");
            toolbar1.AddImageButton("../Options/OptionsTickler.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuTickler.AccessRights = this.ViewState;
            MenuTickler.MenuList = toolbar1.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Send", "SAVE", ToolBarDirection.Right);            
            MenuPhoenixBroadcast.AccessRights = this.ViewState;
            MenuPhoenixBroadcast.MenuList = toolbar.Show();

            lblUserCode.Attributes.Add("style", "visibility:hidden");
            cmdShowUsers.Attributes.Add("onclick", "return showPickList('spnPickListUsers', 'codehelp1', '', '../Common/CommonPicklistMultipleUser.aspx?activeyn=172', true); ");

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
            ViewState["MODE"] = "0";
            gvTickler.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void MenuTickler_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ddlStatus.SelectedValue = "";
                ddlCompleted.SelectedValue = "";
                Rebind();
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidTickler(lblUserCode.Text, txtReminderDate.Text + " " + txtTime.SelectedTime.ToString(), txtMessage.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCommonTickler.Send(lblUserCode.Text, DateTime.Parse(txtReminderDate.Text + " " + txtTime.SelectedTime.ToString()).ToUniversalTime()
                    , null
                    , txtMessage.Text);
                Rebind();
                Reset();
            }
            if (CommandName.ToUpper().Equals("NEW"))
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
           , (int)ViewState["PAGENUMBER"], gvTickler.PageSize, ref iRowCount, ref iTotalPageCount);


        gvTickler.DataSource = dt;
        gvTickler.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
        //txtScheduleTime.Text = "";
        txtTime.SelectedTime = null;
        //txtScheduleOn.Text = "";
    }
    protected void gvTickler_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SNOOZE"))
            {
                //ViewState["MODE"] = "0";
                //Rebind();
                //RadTextBox txt = ((RadTextBox)e.Item.FindControl("txtDoneRemarks"));
                //if (txt != null)
                //{
                //    txt.Enabled = false;
                //    txt.CssClass = "readonlyTextBox";
                //    ((CheckBox)e.Item.FindControl("chkDoneYN")).Enabled = false;
                //}
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                RadGrid _gridView = (RadGrid)sender;
                _gridView.SelectedIndexes.Add(e.Item.ItemIndex);
                ViewState["MODE"] = "1";
                _gridView.EditIndexes.Add(e.Item.ItemIndex);
                Rebind();
                UserControlDate date = ((UserControlDate)e.Item.FindControl("txtRescheduleDate"));
                if (date != null)
                {
                    date.Enabled = false;
                    date.CssClass = "readonlyTextBox";
                }
                RadTextBox txt = ((RadTextBox)e.Item.FindControl("txtReScheduleTime"));
                if (txt != null)
                {
                    txt.Enabled = false;
                    txt.CssClass = "readonlytextbox";
                }
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTickler_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTickler.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTickler_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdEdit");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            ImageButton snz = (ImageButton)e.Item.FindControl("cmdSnooze");
            if (snz != null) snz.Visible = SessionUtil.CanAccess(this.ViewState, snz.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (db != null && General.GetNullableDateTime(drv["FLDCOMPLETEDDATE"].ToString()).HasValue)
            {
                db.Visible = false;
            }
            if (snz != null && drv["FLDSENTYN"].ToString() == "1")
            {
                snz.Visible = false;
            }
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucRemarksTT");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

            lbtn = (RadLabel)e.Item.FindControl("lblDoneRemarks");
            if (lbtn != null)
            {
                uct = (UserControlToolTip)e.Item.FindControl("ucDoneRemarksTT");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
            if (ViewState["MODE"].ToString() == "0")
            {
                RadTextBox txt = ((RadTextBox)e.Item.FindControl("txtDoneRemarks"));
                if (txt != null)
                {
                    txt.Enabled = false;
                    txt.CssClass = "reaonlyRadTextBox";
                    ((RadCheckBox)e.Item.FindControl("chkDoneYN")).Enabled = false;
                }
            }
            else
            {
                UserControlDate date = ((UserControlDate)e.Item.FindControl("txtRescheduleDate"));
                if (date != null)
                {
                    date.Enabled = false;
                    date.CssClass = "readonlyRadTextBox";
                    RadTimePicker txt = ((RadTimePicker)e.Item.FindControl("txtReScheduleTime"));
                    txt.Enabled = false;
                    txt.CssClass = "readonlyRadTextBox";
                }
            }
        }
    }
    protected void Rebind()
    {
        gvTickler.SelectedIndexes.Clear();
        gvTickler.EditIndexes.Clear();
        gvTickler.DataSource = null;
        gvTickler.Rebind();
    }

    protected void gvTickler_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string TicklerID = ((RadLabel)e.Item.FindControl("lblTicklerID")).Text;
            UserControlDate date = ((UserControlDate)e.Item.FindControl("txtRescheduleDate"));
            RadTimePicker time = ((RadTimePicker)e.Item.FindControl("txtReScheduleTime"));
            string doneremarks = ((RadTextBox)e.Item.FindControl("txtDoneRemarks")).Text;
            RadCheckBox chk = ((RadCheckBox)e.Item.FindControl("chkDoneYN"));
            if (ViewState["MODE"].ToString() == "1")
            {
                if (!IsDoneRemarksValid(chk.Checked.Equals(true), doneremarks))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCommonTickler.UpdateAsRead(new Guid(TicklerID), 1, doneremarks);
            }
            else
            {
                string reminderdate = ((RadLabel)e.Item.FindControl("lblReminderDate")).Text;
                if (!IsValidReschedule(reminderdate, date.Text + " " + time.SelectedTime.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCommonTickler.UpdateRescheduleDate(new Guid(TicklerID), DateTime.Parse(date.Text + " " + time.SelectedTime.ToString()).ToUniversalTime());
            }
            Rebind();
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvTickler_EditCommand(object sender, GridCommandEventArgs e)
    //{
    //    try
    //    {
    //        RadGrid _gridView = (RadGrid)sender;
    //        _gridView.SelectedIndexes.Add(e.Item.ItemIndex); 
    //        ViewState["MODE"] = "1";
    //        _gridView.EditIndexes.Add(e.Item.ItemIndex);
    //        Rebind();
    //        UserControlDate date = ((UserControlDate)e.Item.FindControl("txtRescheduleDate"));
    //        if (date != null)
    //        {
    //            date.Enabled = false;
    //            date.CssClass = "readonlytextbox";
    //        }
    //        TextBox txt = ((TextBox)e.Item.FindControl("txtReScheduleTime"));
    //        if (txt != null)
    //        {
    //            txt.Enabled = false;
    //            txt.CssClass = "readonlytextbox";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
}
