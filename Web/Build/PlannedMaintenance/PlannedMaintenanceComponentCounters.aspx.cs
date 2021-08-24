using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceComponentCounters : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentCounters.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvComponentCounter')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuComponentCounter.AccessRights = this.ViewState;
            MenuComponentCounter.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"];
                BindComponentData();
                gvComponentCounter.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDHARDNAME", "FLDREADINGDATE", "FLDCURRENTVALUE", "FLDSTARTDATE", "FLDSTARTVALUE", "FLDZEROEDDATE", "FLDZEROEDVALUE", "FLDAVERAGE", "FLDDEPENDSONNAME" };
        string[] alCaptions = { "Counter Type", "Date Read", "Current Value", "Start Date", "Start Value", "Zeroed Date", "Zeroed Value", "Average", "Depends" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceComponentCounters.ComponentCountersSearch(General.GetNullableGuid(ViewState["COMPONENTID"].ToString())
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                        , sortexpression
                                                                                        , sortdirection
                                                                                        , gvComponentCounter.CurrentPageIndex + 1
                                                                                        , gvComponentCounter.PageSize
                                                                                        , ref iRowCount
                                                                                        , ref iTotalPageCount);
        General.SetPrintOptions("gvComponentCounter", "Component Counter", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvComponentCounter.DataSource = ds;
            gvComponentCounter.VirtualItemCount = iRowCount;
        }
        else
        {
            DataTable dt = ds.Tables[0];
            gvComponentCounter.DataSource = "";
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDHARDNAME", "FLDREADINGDATE", "FLDCURRENTVALUE", "FLDSTARTDATE", "FLDSTARTVALUE", "FLDZEROEDDATE", "FLDZEROEDVALUE", "FLDAVERAGE", "FLDDEPENDSONNAME" };
        string[] alCaptions = { "Counter Type", "Date Read", "Current Value", "Start Date", "Start Value", "Zeroed Date", "Zeroed Value", "Average", "Depends" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPlannedMaintenanceComponentCounters.ComponentCountersSearch(new Guid(ViewState["COMPONENTID"].ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , gvComponentCounter.CurrentPageIndex + 1
                                                                                , gvComponentCounter.PageSize
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount);
        General.ShowExcel("Component Counter", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void ComponentCounter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvComponentCounter.CurrentPageIndex = 0;
                BindData();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
 
    private bool IsValidComponentCounter(string countertype, string currentvalue, string ReadingDate, string startddate, string zeroeddate)
    {

        //string StartDate = General.GetNullableDateTime(startddate)

        Int16 resultint;
        ucError.HeaderMessage = "Please provide the following required information";

        if (ViewState["COMPONENTID"] == null || ViewState["COMPONENTID"].ToString() == "")
            ucError.ErrorMessage = "Components are not available";

        if (countertype.Trim().Equals("") || !Int16.TryParse(countertype, out resultint))
            ucError.ErrorMessage = "Counter type is required.";

        if(startddate != null)
        {
            if (!startddate.Trim().Equals("") && currentvalue.Trim().Equals(""))
                ucError.ErrorMessage = "Current value is required.";
        }
        

        if (ReadingDate != null && !(ReadingDate.Trim().Equals("")) && General.GetNullableDateTime(ReadingDate) == null)
            ucError.ErrorMessage = "Invalid Reading Date";

        if (startddate != null && !(startddate.Trim().Equals("")) && General.GetNullableDateTime(startddate) == null)
            ucError.ErrorMessage = "Invalid Start date";

        if (zeroeddate != null && !(zeroeddate.Trim().Equals("")) && General.GetNullableDateTime(zeroeddate) == null)
            ucError.ErrorMessage = "Invalid Zeroed date";
        if ((ReadingDate != null && !(ReadingDate.Trim().Equals(""))) && (startddate != null && !(startddate.Trim().Equals(""))))
        {
            if (General.GetNullableDateTime(ReadingDate) < General.GetNullableDateTime(startddate))
                ucError.ErrorMessage = "Reading Date Cannot be less than Start Date";
        }
        return (!ucError.IsError);
    }
    //protected void CalculateAverage(object sender, EventArgs e)
    //{
    //    DateTime? startdate = null;
    //    Decimal? startvalue = 0;
    //    DateTime? readingdate = null;
    //    Decimal? currentvalue = 0;
    //    Decimal? zeroedvalue = 0;
    //    try
    //    {
    //        UserControlMaskNumber number = sender as UserControlMaskNumber;
    //        GridViewRow row = number.NamingContainer as GridViewRow;
    //        string addedit = row.RowType == DataControlRowType.Footer ? "Add" : "Edit";

    //        startvalue = General.GetNullableDecimal(((UserControlMaskNumber)row.FindControl("txtStartValue" + addedit)).Text);
    //        currentvalue = General.GetNullableDecimal(((UserControlMaskNumber)row.FindControl("txtCurrentValue" + addedit)).Text);
    //        zeroedvalue = General.GetNullableDecimal(((UserControlMaskNumber)row.FindControl("txtZeroedValue" + addedit)).Text);
    //        startdate = General.GetNullableDateTime(((TextBox)row.FindControl("txtStartDate" + addedit)).Text);
    //        readingdate = General.GetNullableDateTime(((TextBox)row.FindControl("txtReadingDate" + addedit)).Text);

    //        DataTable dt = PhoenixPlannedMaintenanceComponentCounters.CalculateAvgRunningHours(readingdate, startdate, currentvalue, startvalue, zeroedvalue);
    //        ((UserControlMaskNumber)row.FindControl("txtAverage" + addedit)).Text = dt.Rows[0][0].ToString();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private void InsertComponentCounters(string componentid, string countertype, string ReadingDate, string currentvalue, string startdate, string startvalue, string zeroeddate, string zeroedvalue, string average, string calculateaverageyn, string dependson)
    {
        if (!IsValidComponentCounter(
                  countertype
                  , currentvalue
                  , ReadingDate
                  , startdate
                  , zeroeddate
                  ))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixPlannedMaintenanceComponentCounters.InsertComponentCounters(new Guid(componentid)
                        , int.Parse(countertype)
                        , General.GetNullableDateTime(ReadingDate)
                        , General.GetNullableDecimal(currentvalue)
                        , General.GetNullableDateTime(startdate)
                        , General.GetNullableDecimal(startvalue)
                        , General.GetNullableDateTime(zeroeddate)
                        , General.GetNullableDecimal(zeroedvalue)
                        , General.GetNullableDecimal(average)
                        , null
                        , General.GetNullableGuid(dependson)
                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        BindData();
    }
    private void UpdateComponentCounters(string counterid, string componentid, string countertype, string ReadingDate, string currentvalue, string startdate, string startvalue, string zeroeddate, string zeroedvalue, string calculateaverageyn, string dependson)
    {
        PhoenixPlannedMaintenanceComponentCounters.UpdateComponentCounters(new Guid(counterid), new Guid(componentid), int.Parse(countertype), General.GetNullableDateTime(ReadingDate), General.GetNullableDecimal(currentvalue), General.GetNullableDateTime(startdate), General.GetNullableDecimal(startvalue), General.GetNullableDateTime(zeroeddate), General.GetNullableDecimal(zeroedvalue), null, General.GetNullableGuid(dependson));
    }

    private void BindComponentData()
    {
        if ((Request.QueryString["COMPONENTID"] != null) && (Request.QueryString["COMPONENTID"] != ""))
        {
            DataSet ds = PhoenixInventoryComponent.ListComponent(new Guid(Request.QueryString["COMPONENTID"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            DataRow dr = ds.Tables[0].Rows[0];
            //txtTitle.Text += "    ( " + dr["FLDCOMPONENTNUMBER"].ToString() + " - " + dr["FLDCOMPONENTNAME"].ToString() + ")";
        }
    }

    protected void gvComponentCounter_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            gvComponentCounter.ShowFooter = true;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertComponentCounters(ViewState["COMPONENTID"].ToString(),
                    ((UserControlHard)e.Item.FindControl("ddlCounterTypeAdd")).SelectedHard,
                      ((UserControlDate)e.Item.FindControl("txtReadingDateAdd")).Text, ((UserControlDecimal)e.Item.FindControl("txtCurrentValueAdd")).Text
                      , ((UserControlDate)e.Item.FindControl("txtStartDateAdd")).Text, ((UserControlDecimal)e.Item.FindControl("txtStartValueAdd")).Text
                      , ((UserControlDate)e.Item.FindControl("txtZeroedDateAdd")).Text, ((UserControlDecimal)e.Item.FindControl("txtZeroedValueAdd")).Text
                      , ((UserControlDecimal)e.Item.FindControl("txtAverageAdd")).Text, string.Empty
                      , ((RadTextBox)e.Item.FindControl("txtDependsOnAdd")).Text != string.Empty ? ((RadTextBox)e.Item.FindControl("txtDependsOnComponentID")).Text : string.Empty
                      );

                gvComponentCounter.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidComponentCounter(
                 ((UserControlHard)e.Item.FindControl("ddlCounterTypeEdit")).SelectedHard
                  , ((UserControlDecimal)e.Item.FindControl("txtCurrentValueEdit")).Text
                  , ((UserControlDate)e.Item.FindControl("txtReadingDateEdit")).Text
                  , ((UserControlDate)e.Item.FindControl("txtStartDateEdit")).Text
                  , ((UserControlDate)e.Item.FindControl("txtZeroedDateEdit")).Text
                  ))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateComponentCounters
                    (((RadLabel)e.Item.FindControl("lblCounterIDEdit")).Text, ViewState["COMPONENTID"].ToString(),
                    ((UserControlHard)e.Item.FindControl("ddlCounterTypeEdit")).SelectedHard,
                    ((UserControlDate)e.Item.FindControl("txtReadingDateEdit")).Text, ((UserControlDecimal)e.Item.FindControl("txtCurrentValueEdit")).Text
                    , ((UserControlDate)e.Item.FindControl("txtStartDateEdit")).Text, ((UserControlDecimal)e.Item.FindControl("txtStartValueEdit")).Text
                    , ((UserControlDate)e.Item.FindControl("txtZeroedDateEdit")).Text, ((UserControlDecimal)e.Item.FindControl("txtZeroedValueEdit")).Text
                    , string.Empty
                    , ((RadTextBox)e.Item.FindControl("txtDependsOnEdit")).Text != string.Empty ? ((RadTextBox)e.Item.FindControl("txtDependsOnComponentIDEdit")).Text : string.Empty

                     );
                BindData();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPlannedMaintenanceComponentCounters.DeleteComponentCounters(new Guid(((RadLabel)e.Item.FindControl("lblCounterID")).Text));
                gvComponentCounter.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                gvComponentCounter.ShowFooter = false;
                if (e.Item is GridDataItem)
                {

                    LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                    if (ed != null)
                    {
                        ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponentCounter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvComponentCounter_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowComponentEdit");
            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtDepdendsOnComponentNameEdit");
            RadTextBox tb2 = (RadTextBox)e.Item.FindControl("txtDependsOnComponentIDEdit");
            RadTextBox tb3 = (RadTextBox)e.Item.FindControl("txtDependsOnEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            if (tb2 != null) tb2.Attributes.Add("style", "visibility:hidden");
            if (tb3 != null) tb3.Attributes.Add("readonly", "readonly");
            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListShowComponentEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?framename=ifMoreInfo', true); ");

            if (tb1 != null)
            {
                UserControlHard hrd = ((UserControlHard)e.Item.FindControl("ddlCounterTypeEdit"));
                hrd.Enabled = true;
                hrd.CssClass = "input";
                UserControlDate txt = ((UserControlDate)e.Item.FindControl("txtReadingDateEdit"));
                txt.ReadOnly = true;
                txt.CssClass = "readonlytextbox";
                UserControlDecimal number = ((UserControlDecimal)e.Item.FindControl("txtCurrentValueEdit"));
                number.Visible = true;
                number.CssClass = "readonlytextbox";
                txt = ((UserControlDate)e.Item.FindControl("txtStartDateEdit"));
                txt.ReadOnly = true;
                txt.CssClass = "readonlytextbox";
                number = ((UserControlDecimal)e.Item.FindControl("txtStartValueEdit"));
                number.Visible = true;
                number.CssClass = "readonlytextbox";
                txt = ((UserControlDate)e.Item.FindControl("txtZeroedDateEdit"));
                txt.ReadOnly = true;
                txt.CssClass = "readonlytextbox";
                number = ((UserControlDecimal)e.Item.FindControl("txtZeroedValueEdit"));
                number.Visible = true;
                number.CssClass = "readonlytextbox";
            }
        }

        if (e.Item is GridFooterItem)
        {
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowComponentAdd");
            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtDepdendsOnComponentName");
            RadTextBox tb2 = (RadTextBox)e.Item.FindControl("txtDependsOnComponentID");
            RadTextBox tb3 = (RadTextBox)e.Item.FindControl("txtDependsOnAdd");
            tb1.Attributes.Add("style", "visibility:hidden");
            tb2.Attributes.Add("style", "visibility:hidden");
            tb3.Attributes.Add("readonly", "readonly");
            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListShowComponentAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?framename=ifMoreInfo', true); ");
            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null)
            {
                add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);
            }
        }

        UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ddlCounterTypeEdit");
        if (ucHard != null) ucHard.SelectedHard = drv["FLDCOUNTERTYPE"].ToString();
    }
}
