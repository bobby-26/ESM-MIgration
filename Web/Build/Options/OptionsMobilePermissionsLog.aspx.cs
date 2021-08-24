using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class OptionsMobilePermissionsLog : PhoenixBasePage
{

    private string application = string.Empty;
    private string path = string.Empty;
    private string type = string.Empty;
    private string contentType = string.Empty;
    private string status = string.Empty;
    private string description = string.Empty;
    private string exception = string.Empty;

    private DateTime? requestedOn;
    private DateTime? requestedTill;


    private string[] alColumns = { "FLDAPPLICATION", "FLDPATH", "FLDTYPE", "FLDCONTENTTYPE", "FLDPARAMETERS", "FLDSTATUS", "FLDRESPONSE", "FLDEXCEPTION", "FLDCREATEDDATE" };
    private string[] alCaptions = { "Application", "Path", "Request Type", "Content Type", "Parameters", "Status", "Response", "Exception", "Requested On" };
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Options/OptionsMobilePermissionsLog.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMobilePermissionsLog')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Options/OptionsMobilePermissionsLog.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Options/OptionsMobilePermissionsLog.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            toolbar.AddButton("User Identity", "IDENTITY", ToolBarDirection.Right);
            //toolbar.AddButton("Log", "LOG", ToolBarDirection.Right);
            //toolbar.AddButton("Login Audit", "BUDGET", ToolBarDirection.Right);

            MenuOptionsMobilePermissionsLog.AccessRights = this.ViewState;
            MenuOptionsMobilePermissionsLog.MenuList = toolbar.Show();
            MenuOptionsMobilePermissionsLog.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvMobilePermissionsLog.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        //int active = 0;
        DataSet ds = new DataSet();

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        setFilterText();
        ds = PhoenixGeneralSettings.RequestLogMetaDataSearch(requestedOn, requestedTill, application, type, contentType, path, status, description, exception, sortexpression, sortdirection,
        1,
        iRowCount,
        ref iRowCount,
        ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=RequestLog.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Request Log</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");

                object colvalue = dr[alColumns[i]];
                if (alColumns[i] == "FLDRESPONSE")
                {
                    string value = dr[alColumns[i]].ToString();
                    string result = string.Empty;
                    if (string.IsNullOrEmpty(value) || value == "NA")
                    {
                        result = "Not Registered";
                    }
                    else
                        result = "Registered";
                    Response.Write(result);
                }
                else
                    Response.Write(colvalue);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void RegistersMobilePermissionsLog_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                //txtSearch.Text = string.Empty;
                txtApplication.Text = "";
                txtPath.Text = "";
                txtType.Text = "";
                txtContentType.Text = "";
                txtStatus.Text = "";
                //txtDesc.Text = "";
                txtException.Text = "";
                txtRequestedOn.Text = "";
                txtRequestedTill.Text = "";
                //txtSearch.Text = string.Empty;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("IDENTITY"))
            {
                Response.Redirect("~/Options/OptionsMobilePermissions.aspx");
            }
            //if (CommandName.ToUpper().Equals("LOG"))
            //{
            //    Response.Redirect("~/Options/OptionsMobilePermissionsLog.aspx");
            //}
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvMobilePermissionsLog.SelectedIndexes.Clear();
        gvMobilePermissionsLog.EditIndexes.Clear();
        gvMobilePermissionsLog.DataSource = null;
        gvMobilePermissionsLog.Rebind();
    }
    protected void setFilterText()
    {
        application = null;
        path = null;
        type = null;
        contentType = null;
        status = null;
        description = null;
        exception = null;


        if (!txtApplication.Text.Trim().Equals(""))
            application = txtApplication.Text;
        if (!txtPath.Text.Trim().Equals(""))
            path = txtPath.Text;
        if (!txtType.Text.Trim().Equals(""))
            type = txtType.Text;
        if (!txtContentType.Text.Trim().Equals(""))
            contentType = txtContentType.Text;
        if (!txtStatus.Text.Trim().Equals(""))
            status = txtStatus.Text;
        //if (!txtDesc.Text.Trim().Equals(""))
        //    description = txtDesc.Text;
        if (!txtException.Text.Trim().Equals(""))
            exception = txtException.Text;
        requestedOn = General.GetNullableDateTime(txtRequestedOn.Text);
        requestedTill = General.GetNullableDateTime(txtRequestedTill.Text);
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        //int active = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        setFilterText();
        DataSet ds = PhoenixGeneralSettings.RequestLogMetaDataSearch(requestedOn, requestedTill, application, type, contentType, path, status, description, exception, sortexpression, sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvMobilePermissionsLog.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        General.SetPrintOptions("gvMobilePermissionsLog", "Request Log", alCaptions, alColumns, ds);

        gvMobilePermissionsLog.DataSource = ds;
        gvMobilePermissionsLog.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvMobilePermissionsLog_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["logId"] = ((RadLabel)e.Item.FindControl("lblLogId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName == "Page")
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
    protected void gvMobilePermissionsLog_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;

        if (e.Item is GridDataItem)
        {
            string lblCheck = ((RadLabel)e.Item.FindControl("lblStatus")).Text;
            if (lblCheck.Contains("Failed"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                TableCell cell = (TableCell)item["gvStatus"];
                cell.ForeColor = System.Drawing.Color.White;
                cell.CssClass = "bgOR";
            }
            //cell.BackColor = System.Drawing.Color.Red;
        }

        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
    }
    private void DeleteRequestLog(Guid logId)
    {
        PhoenixGeneralSettings.DeleteDevice(logId);
    }

    protected void gvMobilePermissionsLog_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvMobilePermissionsLog_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMobilePermissionsLog.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteRequestLog(new Guid(ViewState["logId"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
