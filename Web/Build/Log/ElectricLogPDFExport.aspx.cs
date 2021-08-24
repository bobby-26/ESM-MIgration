using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogPDFExport : PhoenixBasePage
{
    string refreshWindowName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowToolBar();

        if (string.IsNullOrWhiteSpace(Request.QueryString["refreshWindowName"]) == false)
        {
            refreshWindowName = Request.QueryString["refreshWindowName"];
        }

        if (IsPostBack == false)
        {
            txtFromDate.MaxDate = DateTime.Now;
            txtToDate.MaxDate = DateTime.Now;
        }
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Export", "SAVE", ToolBarDirection.Right);
        MenugvCounterUpdate.MenuList = toolbar.Show();
        MenugvCounterUpdate.AccessRights = this.ViewState;
    }
    
    protected void chkCurrentPage_CheckedChanged(object sender, EventArgs e)
    {
        string selectedItem = radPdfExport.SelectedValue;
        if (selectedItem == "dateRange")
        {
            HtmlTableRow row = (HtmlTableRow)tbldaterange.FindControl("rowdaterange");
            row.Attributes.Add("style", "display:table-row;");
        } else
        {
            HtmlTableRow row = (HtmlTableRow)tbldaterange.FindControl("rowdaterange");
            row.Attributes.Add("style", "display:none;");
        }
    }

    private bool IsValidDate()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtFromDate.SelectedDate.HasValue == false || txtToDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Invalid Date or From or To Date Cannot be empty";
        }


        if (txtFromDate.SelectedDate.HasValue && txtToDate.SelectedDate.HasValue  &&
            DateTime.Compare(txtFromDate.SelectedDate.Value, txtToDate.SelectedDate.Value) > 0)
        {
            ucError.ErrorMessage = "From Date should not be greater than To Date.";
        }

        return (!ucError.IsError);

    }

    protected void MenugvCounterUpdate_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            bool isDateRange = radPdfExport.SelectedValue == "dateRange" ? true : false;
            if (isDateRange && IsValidDate() == false)
            {
                ucError.Visible = true;
                return;
            }

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("isDateRange", isDateRange.ToString());
            if (isDateRange)
            {
                nvc.Add("startDate", txtFromDate.SelectedDate.Value.ToString("dd-MM-yyyy"));
                nvc.Add("endDate", txtToDate.SelectedDate.Value.ToString("dd-MM-yyyy"));
            } else
            {
                nvc.Add("startDate", null);
                nvc.Add("endDate", null);
            }
            Filter.LogBookPdfExportCriteria = nvc;
            string closeWindow = "LogPdfExport";
            string refreshWindow = refreshWindowName == string.Empty ? "oilLog" : refreshWindowName;
            string script = string.Format("closeTelerikWindow('{0}', '{1}');", closeWindow, refreshWindow);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
        }
    }
}