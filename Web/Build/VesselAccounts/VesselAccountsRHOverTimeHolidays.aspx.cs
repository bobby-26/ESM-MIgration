using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsRHOverTimeHolidays : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHOverTimeHolidays.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOvertimeHolidays')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHOverTimeHolidays.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuOverTimeHolidayList.AccessRights = this.ViewState;
        MenuOverTimeHolidayList.MenuList = toolbar.Show();

        if (!IsPostBack)
        {

            ddlUnion.AddressList = PhoenixRegistersAddress.ListAddress("134");
            ddlUnion.DataBind();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ddlYear.SelectedYear = DateTime.Today.Year;
            gvOvertimeHolidays.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDROWNUMBER", "FLDHOLIDAYNAME", "FLDHOLIDAYDATE" };
        string[] alCaptions = { "S.No.", "Holiday", "Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixVesselAccountsRHOverTimeHolidays.OverTimeHolidaysSearch(ddlYear.SelectedYear
            , int.Parse(ddlUnion.SelectedAddress == "" ? "0" : ddlUnion.SelectedAddress)
            , sortexpression
            , sortdirection
            , 1
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=UnionHolidayList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='1' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>&nbsp; Union Holiday List</h3></td>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();

    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDHOLIDAYNAME", "FLDHOLIDAYDATE" };
        string[] alCaptions = { "S.No.", "Holiday", "Date" };

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

        ds = PhoenixVesselAccountsRHOverTimeHolidays.OverTimeHolidaysSearch(ddlYear.SelectedYear
            , int.Parse(ddlUnion.SelectedAddress == "" ? "0" : ddlUnion.SelectedAddress)
            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvOvertimeHolidays.PageSize
            , ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvOvertimeHolidays", "Union Holiday List", alCaptions, alColumns, ds);

        gvOvertimeHolidays.DataSource = ds;
        gvOvertimeHolidays.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvOvertimeHolidays.SelectedIndexes.Clear();
        gvOvertimeHolidays.EditIndexes.Clear();
        gvOvertimeHolidays.DataSource = null;
        gvOvertimeHolidays.Rebind();
    }
    protected void OverTimeHolidayList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
               
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ddlUnion.AddressList = PhoenixRegistersAddress.ListAddress("134");
                ddlUnion.DataBind();

                ddlYear.SelectedYear = DateTime.Today.Year;
                Rebind();
               
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private void InsertOverTimeHolidays(int year, int addresscode, string holidayname, DateTime holidaydate)
    {
        PhoenixVesselAccountsRHOverTimeHolidays.OverTimeHolidayInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            year,addresscode,holidayname,holidaydate);
    }
    private void UpdateOverTimeHolidays(Guid overtimeholidaysid, int year, int addresscode, string holidayname, DateTime holidaydate)
    {
        PhoenixVesselAccountsRHOverTimeHolidays.OverTimeHolidayUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, year, addresscode
            , overtimeholidaysid, holidayname, holidaydate);
    }
    private void deleteOverTimeHolidays(Guid overtimeholidaysid)
    {
        PhoenixVesselAccountsRHOverTimeHolidays.OverTimeHolidayDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,overtimeholidaysid);
    }

    private bool IsValidHoliday(string holidayname, string holidaydate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (holidayname.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Holiday is required.";
        }

        if (General.GetNullableDateTime(holidaydate) == null)
        {
            ucError.ErrorMessage = "Date is required";
        }

        return (!ucError.IsError);
    }
    protected void gvOvertimeHolidays_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {                
                string holiday = ((RadTextBox)e.Item.FindControl("txtHolidayNameAdd")).Text;
                if (!IsValidHoliday(holiday, ((UserControlDate)e.Item.FindControl("txtHolidayDateAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertOverTimeHolidays(ddlYear.SelectedYear,int.Parse(ddlUnion.SelectedAddress),holiday.Trim(),
                    DateTime.Parse(((UserControlDate)e.Item.FindControl("txtHolidayDateAdd")).Text));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidHoliday(((RadTextBox)e.Item.FindControl("txtHolidayNameEdit")).Text
                    , ((UserControlDate)e.Item.FindControl("txtHolidayDateEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateOverTimeHolidays(new Guid(((RadLabel)e.Item.FindControl("lblHolidayIdEdit")).Text),ddlYear.SelectedYear,
                     int.Parse(ddlUnion.SelectedAddress),(((RadTextBox)e.Item.FindControl("txtHolidayNameEdit")).Text).Trim(),
                    DateTime.Parse(((UserControlDate)e.Item.FindControl("txtHolidayDateEdit")).Text));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                deleteOverTimeHolidays(new Guid(((RadLabel)e.Item.FindControl("lblHolidayId")).Text));
                Rebind();
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
    protected void gvOvertimeHolidays_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOvertimeHolidays.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOvertimeHolidays_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }           
        }
    }
}
