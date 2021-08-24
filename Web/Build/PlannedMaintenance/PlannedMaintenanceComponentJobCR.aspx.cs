using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceComponentJobCR : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobCR.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvComponentJob')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobCRFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobCR.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuCompJobCR1.AccessRights = this.ViewState;
            MenuCompJobCR1.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Search", "SEARCH");
                toolbarmain.AddButton("General", "GENERAL");

                MenuCompJobCR.AccessRights = this.ViewState;
                MenuCompJobCR.MenuList = toolbarmain.Show();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["REQUESTID"] = null;

                ViewState["SETCURRENTNAVIGATIONTAB"] = null;

                if (Request.QueryString["REQUESTID"] != null && Request.QueryString["SETCURRENTNAVIGATIONTAB"] != null && Request.QueryString["REQUESTID"].ToString() != "")
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = Request.QueryString["SETCURRENTNAVIGATIONTAB"].ToString();
                    ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();
                }
                if (Request.QueryString["REQUESTID"] != null)
                {
                    ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();
                    //ifMoreInfo.Attributes["src"] = "../Inventory/InventorySpareItemRequestGeneral.aspx?REQUESTID=" + Request.QueryString["REQUESTID"].ToString();
                }
                MenuCompJobCR1.SelectedMenuIndex = 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCompJobCR_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceComponentJobCRFilter.aspx");
            }
            else if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobCRGeneral.aspx?REQUESTID=";
            }

            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["REQUESTID"];

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
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDDISCIPLINENAME", "FLDPRIORITY" };
            string[] alCaptions = { "Component No.", "Component Name", "Job Code", "Job Title", "Frequency", "Resp Discipline", "Priority" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentComponentJobFilter;

            DataTable dt = PhoenixPlannedMaintenanceComponentJobCR.ComponentJobSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
             , nvc != null ? nvc.Get("txtCompNumber").ToString() : null
             , nvc != null ? nvc.Get("txtCompName").ToString() : null
             , nvc != null ? nvc.Get("txtJobcode").ToString() : null
             , nvc != null ? nvc.Get("txtJobTitle").ToString() : null
             , General.GetNullableInteger(nvc != null ? nvc.Get("ucDiscipline").ToString() : string.Empty)
             , sortexpression, sortdirection,
              1,
                         iRowCount,
                         ref iRowCount,
                         ref iTotalPageCount);
            General.ShowExcel("Component Job Change Request", dt, alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCompJobCR1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvComponentJob.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentComponentJobFilter = null;
                BindData();
                gvComponentJob.Rebind();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDDISCIPLINENAME", "FLDPRIORITY" };
            string[] alCaptions = { "Component No.", "Component Name", "Job Code", "Job Title", "Frequency", "Resp Discipline", "Priority" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentComponentJobFilter;

            DataTable dt = PhoenixPlannedMaintenanceComponentJobCR.ComponentJobSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
             , nvc != null ? nvc.Get("txtCompNumber").ToString() : null
             , nvc != null ? nvc.Get("txtCompName").ToString() : null
             , nvc != null ? nvc.Get("txtJobcode").ToString() : null
             , nvc != null ? nvc.Get("txtJobTitle").ToString() : null
             , General.GetNullableInteger(nvc != null ? nvc.Get("ucDiscipline").ToString() : string.Empty)
             , sortexpression, sortdirection,
              Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                         General.ShowRecords(null),
                         ref iRowCount,
                         ref iTotalPageCount);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvComponentJob", "Component Job Change Request", alCaptions, alColumns, ds);


            gvComponentJob.DataSource = dt;
            gvComponentJob.VirtualItemCount = iRowCount;

            if (dt.Rows.Count > 0)
            {

                if (ViewState["REQUESTID"] == null)
                {
                    ViewState["REQUESTID"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();
                    gvComponentJob.SelectedIndexes.Equals(0);
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobCRGeneral.aspx?REQUESTID=";
                }


                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["REQUESTID"];

                SetRowSelection();
                SetTabHighlight();
            }
            else
            {
                gvComponentJob.SelectedIndexes.Clear();
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentJobCRGeneral.aspx";
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (FormatException ex)
        {
            ucError.ErrorMessage = ex.Message + "Error come here";
            ucError.Visible = true;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvComponentJob.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        gvComponentJob.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvComponentJob.Items)
        {
            if (item.GetDataKeyValue("FLDREQUESTID").ToString() == ViewState["REQUESTID"].ToString())
            {
                gvComponentJob.SelectedIndexes.Add(item.ItemIndex);
                break;
            }
        }
    }
    
    protected void SetTabHighlight()
    {
        try
        {
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentJobCRGeneral.aspx"))
            {
                MenuCompJobCR.SelectedMenuIndex = 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponentJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvComponentJob_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridDataItem item = e.Item as GridDataItem;
            Guid REQUESTID = new Guid(((RadLabel)item.FindControl("lblRequestId")).Text.ToString());

            PhoenixPlannedMaintenanceComponentJobCR.DeleteComponentJobChangeRequest(REQUESTID);
            ViewState["REQUESTID"] = null;
            BindData();
            gvComponentJob.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

        
    }

    protected void gvComponentJob_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                LinkButton ap = (LinkButton)e.Item.FindControl("cmdApprove");
                if (ap != null)
                {
                    ap.Attributes.Add("onclick", "return fnConfirmDelete(event, 'Are you sure want to approve this record?'); return false;");
                    ap.Visible = false;
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                        ap.Visible = SessionUtil.CanAccess(this.ViewState, ap.CommandName);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponentJob_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel reqid = (RadLabel)e.Item.FindControl("lblRequestId");
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                Guid g = new Guid(reqid.Text);
                PhoenixPlannedMaintenanceComponentJobCR.ApproveComponentJobChangeRequest(
                     g, PhoenixSecurityContext.CurrentSecurityContext.VesselID
                     );
                ViewState["REQUESTID"] = null;
                BindData();
                gvComponentJob.Rebind();
            }
            if(e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["REQUESTID"] = reqid;
                BindData();
                gvComponentJob.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponentJob_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
