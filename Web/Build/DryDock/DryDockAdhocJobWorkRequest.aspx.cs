using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.DryDock;
using Telerik.Web.UI;
using System.Web.UI;

public partial class DryDockAdhocJobWorkRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);


            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Work Requests", "WORKREQUESTS");

            toolbar.AddButton("Details", "DETAIL");
          
            MenuWorkOrder.AccessRights = this.ViewState;
            MenuWorkOrder.MenuList = toolbar.Show();
            MenuWorkOrder.SelectedMenuIndex = 0;

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockAdhocJobWorkRequest.aspx?ADHOCJOBID=" + Request.QueryString["ADHOCJOBID"].ToString() + "&pno=" + Request.QueryString["pno"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["JOBID"] = Request.QueryString["ADHOCJOBID"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                gvWorkOrder.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNAME", "FLDPLANINGPRIORITY", "FLDPLANNINGDUEDATE", "FLDSTATUSNAME", "FLDREASON", "FLDPOSTPONEDDATE", "FLDCOMPSATISFACTORILYNAME", "FLDWORKDONEDATE", "FLDWORKDONEBY" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component", "Priority", "Due Date", "Status", "Reason", "Postponed Date", "Completed Satisfactorily", "Done Date", "Done By" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            DataTable dt = PhoenixDryDockJob.SearchDryDockWorkRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableGuid(ViewState["JOBID"].ToString())
                    , sortexpression, sortdirection,
                    1, iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Work Done History", dt, alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDivWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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

    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("DETAIL"))
        {
            Response.Redirect("../DryDock/DryDockAdhocJob.aspx?ADHOCJOBID=" + ViewState["JOBID"] + "&pno=" + Request.QueryString["pno"], false);
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNAME", "FLDPLANINGPRIORITY", "FLDPLANNINGDUEDATE", "FLDSTATUSNAME", "FLDREASON", "FLDPOSTPONEDDATE", "FLDCOMPSATISFACTORILYNAME", "FLDWORKDONEDATE", "FLDWORKDONEBY" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component", "Priority", "Due Date", "Status", "Reason", "Postponed Date", "Completed Satisfactorily", "Done Date", "Done By" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataTable dt = PhoenixDryDockJob.SearchDryDockWorkRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableGuid(ViewState["JOBID"].ToString())
                    , sortexpression, sortdirection,
                    gvWorkOrder.CurrentPageIndex+1,
                   gvWorkOrder.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvWorkOrder", "Work Done History", alCaptions, alColumns, ds);
            gvWorkOrder.DataSource = dt;
            gvWorkOrder.VirtualItemCount = iRowCount;

          
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
           // SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkOrder_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvWorkOrder_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = de.NewEditIndex;
        BindData();
    }

    //protected void gvWorkOrder_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        Guid workorderid = General.GetNullableGuid(_gridView.DataKeys[nCurrentRow].Value.ToString()).Value;
    //        string vesselid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text;
    //        string jobregister = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJobRegister")).Text;
    //        string status = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStatus")).SelectedValue;
    //        string reason = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReason")).Text;
    //        string postponeddate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtPostponedDate")).Text;
    //        string tempstatus = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStatus")).Text;
    //        string completedsatisfactorily = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCompletedStatisfactorilyYN")).SelectedValue;
    //        if (!IsValidWorkOrder(status, reason, postponeddate, tempstatus))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        PhoenixDryDockJob.UpdateDryDockWorkRequest(int.Parse(vesselid), new Guid(ViewState["JOBID"].ToString()), workorderid, byte.Parse(status)
    //            , (byte?)General.GetNullableInteger(jobregister), reason, General.GetNullableInteger(""), (byte?)General.GetNullableInteger(completedsatisfactorily)
    //            , General.GetNullableDateTime(postponeddate));
    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvWorkOrder_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

    //        ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
    //        if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

    //        ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
    //        if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

    //        UserControlDate date = (UserControlDate)e.Row.FindControl("txtPostponedDate");
    //        if (date != null)
    //        {
    //            if (drv["FLDSTATUS"].ToString() != "2")
    //            {
    //                date.CssClass = "readonlytextbox";
    //                date.Enabled = false;
    //            }
    //            else
    //            {
    //                date.CssClass = "input_mandatory";
    //                date.Enabled = true;
    //            }                
    //        }

    //        DropDownList ddl = (DropDownList)e.Row.FindControl("ddlStatus");
    //        if (ddl != null) ddl.SelectedValue = drv["FLDSTATUS"].ToString();

    //        ddl = (DropDownList)e.Row.FindControl("ddlCompletedStatisfactorilyYN");
    //        if (ddl != null) ddl.SelectedValue = drv["FLDCOMPSATISFACTORILYYN"].ToString();
    //    }
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
    //        if (db != null)
    //        {
    //            if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
    //                db.Visible = false;
    //        }
    //    }
    //}
   
           
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridViewRow gvrow = (GridViewRow)ddl.Parent.Parent;
        UserControlDate date = (UserControlDate)gvrow.FindControl("txtPostponedDate");
        if (ddl.SelectedValue == "2")
        {
            date.CssClass = "input_mandatory";
            date.Enabled = true;
        }
        else
        {
            date.Enabled = false;
            date.CssClass = "readonlytextbox";
            date.Text = "";
        }
    }
    private bool IsValidWorkOrder(string Status, string Reason, string postponeddate, string tempstatus)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;
        if (!General.GetNullableInteger(Status).HasValue)
            ucError.ErrorMessage = "Status is required.";
        if (tempstatus.Trim().Equals(""))
            tempstatus = "1";
        if (Reason.Trim().Equals("") && tempstatus != Status)
            ucError.ErrorMessage = "Reason for Status Change is required.";

        if (General.GetNullableInteger(Status).HasValue && General.GetNullableInteger(Status).Value == 2)
        {
            if (!General.GetNullableDateTime(postponeddate).HasValue)
                ucError.ErrorMessage = "Postponed Date is required.";
            else if (DateTime.TryParse(postponeddate, out resultdate) && DateTime.Compare(DateTime.Now, resultdate) > 0)
                ucError.ErrorMessage = "Postponed Date should be later than current date";

        }
        return (!ucError.IsError);
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvWorkOrder_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
           
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlDate date = (UserControlDate)e.Item.FindControl("txtPostponedDate");
            if (date != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDSTATUS").ToString() != "2")
                {
                    date.CssClass = "readonlytextbox";
                    date.Enabled = false;
                }
                else
                {
                    date.CssClass = "input_mandatory";
                    date.Enabled = true;
                }
            }

            RadDropDownList ddl = (RadDropDownList)e.Item.FindControl("ddlStatus");
            if (ddl != null) ddl.SelectedValue = DataBinder.Eval(e.Item.DataItem, "FLDSTATUS").ToString();

            ddl = (RadDropDownList)e.Item.FindControl("ddlCompletedStatisfactorilyYN");
            if (ddl != null) ddl.SelectedValue = DataBinder.Eval(e.Item.DataItem, "FLDCOMPSATISFACTORILYYN").ToString();
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

    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvWorkOrder_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;

                     
            Guid workorderid = General.GetNullableGuid(eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDWORKORDERID"].ToString()).Value;
            string vesselid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
            string jobregister = ((RadLabel)e.Item.FindControl("lblJobRegister")).Text;
            string status = ((RadDropDownList)e.Item.FindControl("ddlStatus")).SelectedValue;
            string reason = ((RadTextBox)e.Item.FindControl("txtReason")).Text;
            string postponeddate = ((UserControlDate)e.Item.FindControl("txtPostponedDate")).Text;
            string tempstatus = ((RadLabel)e.Item.FindControl("lblStatus")).Text;
            string completedsatisfactorily = ((RadDropDownList)e.Item.FindControl("ddlCompletedStatisfactorilyYN")).SelectedValue;
            if (!IsValidWorkOrder(status, reason, postponeddate, tempstatus))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixDryDockJob.UpdateDryDockWorkRequest(int.Parse(vesselid), new Guid(ViewState["JOBID"].ToString()), workorderid, byte.Parse(status)
                , (byte?)General.GetNullableInteger(jobregister), reason, General.GetNullableInteger(""), (byte?)General.GetNullableInteger(completedsatisfactorily)
                , General.GetNullableDateTime(postponeddate));

            //BindData();
            gvWorkOrder.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}