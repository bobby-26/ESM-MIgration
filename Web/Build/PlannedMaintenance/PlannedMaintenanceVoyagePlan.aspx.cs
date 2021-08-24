using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceVoyagePlan : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVoyagePlan.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVoyagePlan')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        //toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVoyagePlan.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("javascript:showDialog('Add');", "Add Voyage Plan", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        //toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVoyagePlan.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuJobParameter.AccessRights = this.ViewState;
        MenuJobParameter.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvVoyagePlan.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);            
        }
    }
    protected void MenuJobParameter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLEAR"))
            {

            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVOYAGENUMBER", "FLDFROMPORT", "FLDTOPORT", "FLDFROMDATE", "FLDTODATE", "FLDSTATUS", "FLDREVISIONNUMBER" };
        string[] alCaptions = { "No.", "Port From", "Port To", "Date From", "Date To", "Status", "Rev.No" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixPlannedMaintenanceVoyagePlan.Search(PhoenixSecurityContext.CurrentSecurityContext.VesselID, 1
                                         , sortexpression, sortdirection
                                         , 1
                                         , iRowCount
                                         , ref iRowCount
                                         , ref iTotalPageCount);

        General.ShowExcel("Voyage Plan", dt, alColumns, alCaptions, null, null);
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVOYAGENUMBER", "FLDFROMPORT", "FLDTOPORT", "FLDFROMDATE", "FLDTODATE", "FLDSTATUS", "FLDREVISIONNUMBER" };
            string[] alCaptions = { "No.", "Port From", "Port To", "Date From", "Date To", "Status", "Rev.No" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixPlannedMaintenanceVoyagePlan.Search(PhoenixSecurityContext.CurrentSecurityContext.VesselID, 1
                                         , sortexpression, sortdirection
                                         , int.Parse(ViewState["PAGENUMBER"].ToString())
                                         , gvVoyagePlan.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvVoyagePlan", "Voyage Plan", alCaptions, alColumns, ds);

            gvVoyagePlan.DataSource = dt;
            gvVoyagePlan.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVoyagePlan_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            if (e.CommandName.ToUpper() == "PAGE")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper() == "ADD")
            {
                string No = ((RadTextBox)e.Item.FindControl("txtVoyageNumberAdd")).Text;
                string FPort = ((UserControlMultiColumnPort)e.Item.FindControl("ddlFromPortAdd")).SelectedValue;
                string TPort = ((UserControlMultiColumnPort)e.Item.FindControl("ddlToPortAdd")).SelectedValue;
                string FDate = ((UserControlDate)e.Item.FindControl("txtFromDateAdd")).Text;
                string TDate = ((UserControlDate)e.Item.FindControl("txtToDateAdd")).Text;

                if (!IsValidVoyagePlan(No, FPort, TPort, FDate, TDate))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceVoyagePlan.Insert(int.Parse(FPort), int.Parse(TPort)
                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableDateTime(FDate).Value, General.GetNullableDateTime(TDate).Value);                
                gvVoyagePlan.Rebind();
            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string id = drv["FLDPLANID"].ToString();
                string No = ((RadTextBox)e.Item.FindControl("txtVoyageNumber")).Text;
                string FPort = ((UserControlMultiColumnPort)e.Item.FindControl("ddlFromPort")).SelectedValue;
                string TPort = ((UserControlMultiColumnPort)e.Item.FindControl("ddlToPort")).SelectedValue;
                string FDate = ((UserControlDate)e.Item.FindControl("txtFromDate")).Text;
                string TDate = ((UserControlDate)e.Item.FindControl("txtToDate")).Text;

                if (!IsValidVoyagePlan(No, FPort, TPort, FDate, TDate))
                {
                    ucError.Visible = true;
                    e.Canceled = true;
                    return;
                }
                PhoenixPlannedMaintenanceVoyagePlan.Update(new Guid(id), int.Parse(FPort), int.Parse(TPort)
                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableDateTime(FDate).Value, General.GetNullableDateTime(TDate).Value);
                gvVoyagePlan.Rebind();
            }
            else if (e.CommandName.ToUpper() == "EDITR")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDPLANID").ToString();                      
                string script = "function sd(){showDialog('Edit'); $find('"+ RadAjaxPanel2.ClientID + "').ajaxRequest('" + id + ",EDIT');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string id = drv["FLDPLANID"].ToString();                
                gvVoyagePlan.Rebind();
            }
            else if (e.CommandName.ToUpper() == "SELECT")
            {
                string planid = e.CommandArgument.ToString();
                Response.Redirect("~/PlannedMaintenance/PlannedMaintenanceVoyagePlanDetail.aspx?p=" + planid);
            }
            else if (e.CommandName.ToUpper() == "COPY")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDPLANID").ToString();
                string number = ((LinkButton)e.Item.FindControl("lnkDetail")).Text;
                string script = "function sd(){showDialog('Copy "+ number + "'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest('" + id + ",COPY');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "REVISE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDPLANID").ToString();
                PhoenixPlannedMaintenanceVoyagePlan.Revise(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                gvVoyagePlan.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVoyagePlan_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            //LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            //if (db != null)
            //{
            //    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            //    db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
            //    if (drv["FLDSHOWCOPY"].ToString() == "1")
            //    {
            //        db.Visible = false;
            //    }
            //}
            LinkButton cpy = (LinkButton)e.Item.FindControl("cmdCopy");
            if (cpy != null)
            {
                cpy.Visible = SessionUtil.CanAccess(this.ViewState, cpy.CommandName);
                if (drv["FLDSHOWCOPY"].ToString() != "1")
                {
                    cpy.Visible = false;
                }
            }
            LinkButton rev = (LinkButton)e.Item.FindControl("cmdRevise");
            if (rev != null)
            {
                rev.Visible = SessionUtil.CanAccess(this.ViewState, rev.CommandName);
                rev.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you sure you want to revise?'); return false;");
                if (drv["FLDSHOWCOPY"].ToString() != "1")
                {
                    rev.Visible = false;
                }
            }
            if (e.Item.ItemType == GridItemType.EditItem)
            {

            }
            if (e.Item.ItemType == GridItemType.Footer)
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVoyagePlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVoyagePlan.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoyagePlan_SortCommand(object sender, GridSortCommandEventArgs e)
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
    private bool IsValidVoyagePlan(string no, string fport, string toport, string fdate, string todate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (no.Trim().Equals(""))
            ucError.ErrorMessage = "No. is required.";

        if (General.GetNullableInteger(fport) == null)
            ucError.ErrorMessage = "From Port is required.";

        if (General.GetNullableInteger(toport) == null)
            ucError.ErrorMessage = "To Port is required.";

        if (General.GetNullableDateTime(fdate) == null)
            ucError.ErrorMessage = "From Date is required.";
        else if (General.GetNullableDateTime(todate).HasValue && DateTime.Compare(DateTime.Parse(fdate), DateTime.Parse(todate)) > 0)
        {
            ucError.ErrorMessage = "To Date Date should be later than From Date.";
        }

        if (General.GetNullableDateTime(todate) == null)
            ucError.ErrorMessage = "To Date is required.";

        return (!ucError.IsError);
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                if (btnCreate.Text.ToUpper() == "COPY")
                {
                    PhoenixPlannedMaintenanceVoyagePlan.Copy(new Guid(lblPlanId.Text), int.Parse(ddlFromPort.SelectedValue), int.Parse(ddlToPort.SelectedValue)
                           , PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtDateFrom.SelectedDate.Value, txtDateTo.SelectedDate.Value);
                }
                else 
                {
                    if (General.GetNullableGuid(lblPlanId.Text) == null)
                    {
                        PhoenixPlannedMaintenanceVoyagePlan.Insert(int.Parse(ddlFromPort.SelectedValue), int.Parse(ddlToPort.SelectedValue)
                             , PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtDateFrom.SelectedDate.Value, txtDateTo.SelectedDate.Value);
                    }
                    else
                    {
                        PhoenixPlannedMaintenanceVoyagePlan.Update(new Guid(lblPlanId.Text), int.Parse(ddlFromPort.SelectedValue), int.Parse(ddlToPort.SelectedValue)
                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtDateFrom.SelectedDate.Value, txtDateTo.SelectedDate.Value);
                    }
                }                
                gvVoyagePlan.Rebind();
                string script = "function f(){CloseModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                lblPlanId.Text = string.Empty;
                txtNo.Text = string.Empty;
                ddlFromPort.SelectedValue = string.Empty;
                ddlFromPort.Text = string.Empty;
                ddlToPort.SelectedValue = string.Empty;
                ddlToPort.Text = string.Empty;
                txtDateFrom.SelectedDate = null;
                txtDateTo.SelectedDate = null;
                btnCreate.Text = "Create";
            }
            catch (Exception ex)
            {
                RequiredFieldValidator Validator = new RequiredFieldValidator();
                Validator.ErrorMessage = "* " + ex.Message;
                //Validator.ValidationGroup = "Group1";
                Validator.IsValid = false;
                Validator.Visible = false;
                Page.Form.Controls.Add(Validator);
            }
        }
    }
    protected void RadAjaxPanel2_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        string[] args = e.Argument.Split(',');
        string id = args[0];
        string cmd = args[1];
        if (cmd.ToUpper() == "EDIT")
        {
            DataTable dt = PhoenixPlannedMaintenanceVoyagePlan.Edit(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                lblPlanId.Text = id;
                txtNo.Text = dr["FLDVOYAGENUMBER"].ToString();
                ddlFromPort.SelectedValue = dr["FLDFROMPORTID"].ToString();
                ddlFromPort.Text = dr["FLDFROMPORTNAME"].ToString();
                ddlToPort.SelectedValue = dr["FLDTOPORTID"].ToString();
                ddlToPort.Text = dr["FLDTOPORTNAME"].ToString();
                txtDateFrom.SelectedDate = General.GetNullableDateTime(dr["FLDFROMDATE"].ToString());
                txtDateTo.SelectedDate = General.GetNullableDateTime(dr["FLDTODATE"].ToString());
                btnCreate.Text = "Save";
            }
        }
        else if (cmd.ToUpper() == "COPY")
        {
            lblPlanId.Text = id;
            txtNo.Text = string.Empty;
            ddlFromPort.SelectedValue = string.Empty;
            ddlFromPort.Text = string.Empty;
            ddlToPort.SelectedValue = string.Empty;
            ddlToPort.Text = string.Empty;
            txtDateFrom.SelectedDate = General.GetNullableDateTime(string.Empty);
            txtDateTo.SelectedDate = General.GetNullableDateTime(string.Empty);            
            btnCreate.Text = "Copy";
        }
    }
}