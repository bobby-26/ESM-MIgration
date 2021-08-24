using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Web.UI;
using Telerik.Web.UI;

public partial class VesselAccountsWRHOTDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Lock", "LOCK");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbarmain.AddButton("Unlock", "UNLOCK", ToolBarDirection.Right);
            }
            toolbarmain.AddButton("Summary", "SUMMARY", ToolBarDirection.Right);
            MenuRHGeneral.AccessRights = this.ViewState;
            MenuRHGeneral.MenuList = toolbarmain.Show();

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ddlVessel.Enabled = false;
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../VesselAccounts/VesselAccountsWRHOTDetail.aspx" + (Request.QueryString["access"] != null ? "?access=1" : string.Empty), "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvOPA')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../VesselAccounts/VesselAccountsWRHOTDetail.aspx", "Find", "search.png", "FIND");
            //toolbar.AddImageButton("../VesselAccounts/VesselAccountsWRHOT.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbar.AddImageButton("../VesselAccounts/VesselAccountsWRHOTDetail.aspx", "Refresh", "refresh.png", "REFRESH");
            MenuOPAList.AccessRights = this.ViewState;
            MenuOPAList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["YEAR"] = DateTime.Today.Year.ToString();
                ddlYear.SelectedYear = DateTime.Today.Year;
                ddlMonth.SelectedMonth = DateTime.Today.Month.ToString();
                ViewState["MONTH"] = DateTime.Today.Month.ToString();
                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                {
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
                    ddlVessel.SelectedVessel = Request.QueryString["vesselid"];
                }
                if (Request.QueryString["month"] != null && Request.QueryString["month"].ToString() != "")
                {
                    ViewState["MONTH"] = Request.QueryString["month"].ToString();
                    ddlMonth.SelectedMonth = Request.QueryString["month"];
                }

                if (Request.QueryString["year"] != null && Request.QueryString["year"].ToString() != "")
                {
                    ViewState["YEAR"] = Request.QueryString["year"].ToString();
                    ddlYear.SelectedYear = int.Parse(Request.QueryString["year"]);
                }

                if (Request.QueryString["signonoffid"] != null && Request.QueryString["signonoffid"].ToString() != "")
                {
                    ViewState["SIGNONOFFID"] = Request.QueryString["signonoffid"].ToString();
                    ddlEmployee.SelectedValue = Request.QueryString["signonoffid"];
                }
                BindEmployee();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("REFRESH"))
            {
                PhoenixVesselAccountsRHMonthlyOT.OTGenerate(General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : ddlVessel.SelectedVessel)
                                             , General.GetNullableInteger(ViewState["MONTH"] != null ? ViewState["MONTH"].ToString() : ddlMonth.SelectedMonth)
                                             , General.GetNullableInteger(ViewState["YEAR"] != null ? ViewState["YEAR"].ToString() : ddlYear.SelectedYear.ToString())
                                             , General.GetNullableInteger(ddlEmployee.SelectedValue)
                                             );
                Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOPA_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
    }
    protected void BindEmployee()
    {
        if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
        {
            ddlEmployee.Items.Clear();
            ddlEmployee.DataSource = PhoenixVesselAccountsRHMonthlyOT.ListOfOnBoardEmployee(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                , General.GetNullableInteger(ViewState["MONTH"] != null ? ViewState["MONTH"].ToString() : ddlMonth.SelectedMonth), General.GetNullableInteger(ViewState["YEAR"] != null ? ViewState["YEAR"].ToString() : ddlYear.SelectedYear.ToString()));

            ddlEmployee.DataTextField = "FLDNAME";
            ddlEmployee.DataValueField = "FLDSIGNONOFFID";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new DropDownListItem("--Select--", ""));
        }
    }
    protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {

        ViewState["SIGNONOFFID"] = ddlEmployee.SelectedValue;
        Rebind();
    }

    protected void ddlvessel_selectedindexchange(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = ddlVessel.SelectedVessel;
        BindEmployee();
    }
    protected void RHGeneral_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("LOCK"))
            {
                PhoenixVesselAccountsRHMonthlyOT.Updatelock(General.GetNullableInteger(ddlMonth.SelectedMonth)
                                                            , General.GetNullableInteger(ddlYear.SelectedYear.ToString())
                                                            , General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : ddlVessel.SelectedVessel)
                                                            , General.GetNullableInteger(ddlEmployee.SelectedValue));
            }
            if (CommandName.ToUpper().Equals("UNLOCK"))
            {
                PhoenixVesselAccountsRHMonthlyOT.UpdateUnlock(General.GetNullableInteger(ddlMonth.SelectedMonth)
                                                          , General.GetNullableInteger(ddlYear.SelectedYear.ToString())
                                                          , General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : ddlVessel.SelectedVessel)
                                                          , General.GetNullableInteger(ddlEmployee.SelectedValue));

            }
            if (CommandName.ToUpper().Equals("SUMMARY"))
            {
                if (ViewState["VESSELID"] != null && ViewState["MONTH"] != null && ViewState["YEAR"] != null && ViewState["SIGNONOFFID"] != null)
                {
                    Response.Redirect("../VesselAccounts/VesselAccountsWRHOT.aspx?&vesselid=" + ViewState["VESSELID"].ToString() + "&month=" + ViewState["MONTH"].ToString() + "&year=" + ViewState["YEAR"].ToString() + "&signonoffid=" + ViewState["SIGNONOFFID"].ToString(), false);
                }
                else
                {
                    Response.Redirect("../VesselAccounts/VesselAccountsWRHOT.aspx");
                }
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvOPA.SelectedIndexes.Clear();
        gvOPA.EditIndexes.Clear();
        gvOPA.DataSource = null;
        gvOPA.Rebind();
    }
    private void ShowError()
    {
        ucError.HeaderMessage = "Navigation Error";
        ucError.ErrorMessage = "Please select a Seafarer and then Navigate to other Tabs";
        ucError.Visible = true;

    }
    private void BindData()
    {
        DataSet ds = new DataSet();
        if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
        {
            string[] alColumns = { "FLDROWNUMBER", "FLDVESSELNAME", "FLDEMPLOYEENAME", "FLDDATE", "FLDTOTALWORKHOURS", "FLDACTUALOT", "FLDPREDICTEDOT" };
            string[] alCaptions = { "Sr.No", "Vessel", "Employee Name", "Date", "Work(Hrs)", "overtime(Hrs)", "Estimated overtime(Hrs)" };

            ds = PhoenixVesselAccountsRHMonthlyOT.RestHourOTDetailSearch(General.GetNullableInteger(ddlEmployee.SelectedValue)
                                                                        , General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : ddlVessel.SelectedVessel)
                                                                        , General.GetNullableInteger(ddlMonth.SelectedMonth), General.GetNullableInteger(ddlYear.SelectedYear.ToString()));
            gvOPA.DataSource = ds.Tables[0];
            gvOPA.VirtualItemCount = ds.Tables[0].Rows.Count;
            General.SetPrintOptions("gvOPA", "Monthly Overtime", alCaptions, alColumns, ds);
        }

    }
    protected void ShowExcel()
    {

        string[] alColumns = { "FLDROWNUMBER", "FLDVESSELNAME", "FLDEMPLOYEENAME", "FLDDATE", "FLDTOTALWORKHOURS", "FLDACTUALOT", "FLDPREDICTEDOT" };
        string[] alCaptions = { "Sr.No", "Vessel", "Employee Name", "Date", "Work(Hrs)", "overtime(Hrs)", "Estimated overtime(Hrs)" };

        DataSet ds = new DataSet();
        ds = PhoenixVesselAccountsRHMonthlyOT.RestHourOTDetailSearch(General.GetNullableInteger(ddlEmployee.SelectedValue)
                                                                       , General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : ddlVessel.SelectedVessel)
                                                                       , General.GetNullableInteger(ddlMonth.SelectedMonth)
                                                                       , General.GetNullableInteger(ddlYear.SelectedYear.ToString())
                                                                           );
        General.ShowExcel("Monthly Overtime", ds.Tables[0], alColumns, alCaptions, null, null);
    }
    private bool IsValidList(string Startid, string employee)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(Startid))
            ucError.ErrorMessage = "Overtime(Hrs) are required";
        if (string.IsNullOrEmpty(employee) || employee == "Dummy")
            ucError.ErrorMessage = "Employee is required";
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void gvOPA_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidList(((UserControlMaskNumber)e.Item.FindControl("txtTotalothour")).Text
                                , General.GetNullableString(ddlEmployee.SelectedValue)))

                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselAccountsRHMonthlyOT.Updateot(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblReshourStartIDedt")).Text)
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblRestHourCalendarIDedt")).Text)
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblMonthlyovertimereportidedt")).Text)
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblPredictedotidedt")).Text)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtTotalothour")).Text)
                    , DateTime.Parse(((RadLabel)e.Item.FindControl("lblDateItem")).Text));

                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOPA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ddlMonth_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["MONTH"] = ddlMonth.SelectedMonth;
        BindEmployee();
    }

    protected void ddlYear_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["YEAR"] = ddlYear.SelectedYear.ToString();
        BindEmployee();
    }
}
