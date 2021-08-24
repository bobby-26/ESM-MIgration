using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class AccountsLeaveAllotment : PhoenixBasePage
{
    string empid;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Send for Allotment", "CONFIRM", ToolBarDirection.Right);
            LeaveAllotment.AccessRights = this.ViewState;
            LeaveAllotment.MenuList = toolbarmain.Show();

            empid = Request.QueryString["empid"];
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                txtPayTill.Text = LastDayOfMonthFromDateTime(DateTime.Now).ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void LeaveAllotment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("CONFIRM"))
        {
            if (!IsValidRequest(txtPayTill.Text))
            {
                ucError.Visible = true;
                return;
            }
            RadWindowManager1.RadConfirm("You will not be able to make any changes. Are you sure you want to send this for allotment?", "ConfirmSend", 320, 150, null, "Confirm");
            return;
            //ucConfirm.Visible = true;
            //ucConfirm.Text = "You will not be able to make any changes. Are you sure you want to send this for allotment?";
        }       
    }
    private void BindData()
    {
        try
        {
            //int iRowCount = 0;
            //int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 0; //DEFAULT DESC SORT
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixAccountsLeaveAllotment.ListLeaveAllotment(General.GetNullableDateTime(txtPayTill.Text), General.GetNullableInteger(empid));

            gvLVP.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidRequest(string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultDate;
        if (!General.GetNullableDateTime(date).HasValue)
            ucError.ErrorMessage = "Pay till is required";
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Pay till should be earlier than current date";
        }
        return (!ucError.IsError);
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void btnRequest_Click(object sender, EventArgs e)
    {
        
    }
    private DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
    {
        DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
    }

    protected void gvLVP_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancel = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
        }
    }

    protected void gvLVP_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLVP.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvLVP.SelectedIndexes.Clear();
        gvLVP.EditIndexes.Clear();
        gvLVP.DataSource = null;
        gvLVP.Rebind();
    }
    protected void ucConfirmSend_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixAccountsLeaveAllotment.InsertLeaveAllotmentRequest(DateTime.Parse(txtPayTill.Text), int.Parse(empid));
            Rebind();
            ucStatus.Text = "Allotment Sent.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
