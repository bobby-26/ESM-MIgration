using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewLeavePayNextIncrement : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewLeavePayNextIncrement.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvLVP')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:Openpopup('Filter','','CrewLeavePayNextIncrementFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewLeavePayNextIncrement.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuIncrement.AccessRights = this.ViewState;
            MenuIncrement.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvLVP.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
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
    protected void MenuIncrement_TabStripCommand(object sender, EventArgs e)
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
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CrewIncrementDateTrackingFilter = null;
                Rebind();
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

        string[] alColumns = { "FLDRANKNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDYEAR", "FLDEFFECTIVEDATE", "FLDCAREERBREAKDAYS", "FLDINCREMENTDUEDATE" };
        string[] alCaptions = { "Rank", "Employee Code", "Employee Name", "Scale Year", "Effective Date", "Career Break Days", " Next Increment Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CrewIncrementDateTrackingFilter;
        DataTable dt = PhoenixCrewWageIncrement.SearchCrewNextIncrement(General.GetNullableInteger(ddlPool.SelectedPool), nvc != null ? nvc.Get("txtName") : string.Empty
                                        , nvc != null ? nvc.Get("txtFileNo") : string.Empty
                                        , General.GetNullableInteger(nvc != null ? nvc.Get("ddlRank") : string.Empty),
                                    sortexpression, sortdirection,
                                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                    iRowCount,
                                    ref iRowCount,
                                    ref iTotalPageCount);

        General.ShowExcel("Increment Date Tracking", dt, alColumns, alCaptions, sortdirection, sortexpression);

    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDRANKNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDYEAR", "FLDEFFECTIVEDATE", "FLDCAREERBREAKDAYS", "FLDINCREMENTDUEDATE" };
            string[] alCaptions = { "Rank", "Employee Code", "Employee Name", "Scale Year", "Effective Date", "Career Break Days", " Next Increment Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 0; //DEFAULT DESC SORT
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CrewIncrementDateTrackingFilter;
            DataTable dt = PhoenixCrewWageIncrement.SearchCrewNextIncrement(General.GetNullableInteger(ddlPool.SelectedPool), nvc != null ? nvc.Get("txtName") : string.Empty
                                        , nvc != null ? nvc.Get("txtFileNo") : string.Empty
                                        , General.GetNullableInteger(nvc != null ? nvc.Get("ddlRank") : string.Empty)
                                                        , sortexpression, sortdirection
                                                        , (int)ViewState["PAGENUMBER"], gvLVP.PageSize
                                                        , ref iRowCount, ref iTotalPageCount);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvLVP", "Increment Date Tracking", alCaptions, alColumns, ds);
            gvLVP.DataSource = dt;
            gvLVP.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLVP_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
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
            RadComboBox ddl = (RadComboBox)e.Item.FindControl("ddlYear");
            if (ddl != null)
            {
                ddl.SelectedValue = drv["FLDYEAR"].ToString();
            }
        }

    }

    private bool IsValidDate(string date, string year)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(date).HasValue)
            ucError.ErrorMessage = "Effective Date is required.";
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Effective Date should be earlier than current";
        }
        if (!General.GetNullableInteger(year).HasValue)
            ucError.ErrorMessage = "Year is required.";
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
    protected void gvLVP_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                int EmployeeId = int.Parse(((RadLabel)e.Item.FindControl("lblempid")).Text);
                string EffectiveDate = ((UserControlDate)e.Item.FindControl("txtEffectiveDate")).Text;
                string Year = ((RadComboBox)e.Item.FindControl("ddlYear")).SelectedValue;
                if (!IsValidDate(EffectiveDate, Year))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewWageIncrement.UpdateCrewNextIncrementDate(EmployeeId, DateTime.Parse(EffectiveDate), int.Parse(Year));
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
}
