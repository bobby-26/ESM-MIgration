using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceComponentJobPartsRequired : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["vesselid"] != null)
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobPartsRequired.aspx?vesselid="+ Request.QueryString["vesselid"] + "", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        else
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobPartsRequired.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRequiredParts')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListStockItemWithLocation.aspx?mode=multi&compno=" + ViewState["COMPNO"] + "', true);", "Part", "<i class=\"fas fa-cogs\"></i>", "ADDPART");

        MenugvRequiredParts.AccessRights = this.ViewState;
        MenugvRequiredParts.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["COMPONENTJOBID"] = null;
            ViewState["COMPNO"] = null;
            ViewState["REQUIREDPARTS"] = null;
            ViewState["FLDCOMPJOBLINEITEMID"] = null;
            ViewState["COMPONENTID"] = null;
            ViewState["VESSELID"] = null;
            if (Request.QueryString["vesselid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vesselid"];
            else
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            if (Request.QueryString["COMPONENTJOBID"] != null)
                ViewState["COMPONENTJOBID"] = Request.QueryString["COMPONENTJOBID"];
            if (Request.QueryString["Compid"] != null)
                ViewState["COMPONENTID"] = Request.QueryString["Compid"];
            getReportId();
            //MenugvRequiredParts.SetTrigger(pnlgvRequiredParts);
            gvRequiredParts.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    private void getReportId()
    {
        try
        {
            if ((Request.QueryString["COMPONENTJOBID"] != null) && (Request.QueryString["COMPONENTJOBID"] != ""))
            {
                DataSet ds = PhoenixPlannedMaintenanceComponentJob.EditComponentJob(new Guid(Request.QueryString["COMPONENTJOBID"])
                                                                                    , int.Parse(ViewState["VESSELID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (dr["FLDCOMPONENTNUMBER"].ToString().Length > 6)
                    {
                        ViewState["COMPNO"] = dr["FLDCOMPONENTNUMBER"].ToString().Substring(0, 6);
                    }
                    else
                    {
                        ViewState["COMPNO"] = dr["FLDCOMPONENTNUMBER"].ToString();
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDLOCATIONNAME", "FLDQUANTITY", "FLDUNITNAME" };
        string[] alCaptions = { "Part Number", "Part Name", "Location", "Quantity", "Unit" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceComponentJob.ComponentJobPartsRequiredSearch(General.GetNullableGuid(ViewState["COMPONENTJOBID"].ToString())
            , int.Parse(ViewState["VESSELID"].ToString())
            , sortexpression, sortdirection
            , gvRequiredParts.CurrentPageIndex + 1
            , gvRequiredParts.PageSize
            , ref iRowCount, ref iTotalPageCount);
        General.SetPrintOptions("gvRequiredParts", "Stock Used", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRequiredParts.DataSource = ds;
            gvRequiredParts.VirtualItemCount = iRowCount;
        }
        else
        {
            gvRequiredParts.DataSource = "";
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDLOCATIONNAME", "FLDQUANTITY", "FLDUNITNAME" };
        string[] alCaptions = { "Part Number", "Part Name", "Location", "Quantity", "Unit" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPlannedMaintenanceComponentJob.ComponentJobPartsRequiredSearch(General.GetNullableGuid(ViewState["COMPONENTJOBID"].ToString())
                                                                                    , int.Parse(ViewState["VESSELID"].ToString())
                                                                                    , sortexpression
                                                                                    , sortdirection
                                                                                    , gvRequiredParts.CurrentPageIndex + 1
                                                                                    , gvRequiredParts.PageSize
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount);
        General.ShowExcel("Parts Required", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void gvRequiredParts_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvRequiredParts.CurrentPageIndex = 0;
                gvRequiredParts.Rebind();
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

    protected void gvRequiredParts_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvRequiredParts, "Edit$" + e.Row.RowIndex.ToString(), false);
        }

        SetKeyDownScroll(sender, e);
    }

    private bool IsValidRequiredParts(string workorderid, string sparepartiid, string requiredvalue)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (workorderid.Trim().Equals(""))
            ucError.ErrorMessage = "Please first fill general information.";

        if (sparepartiid.Trim().Equals(""))
            ucError.ErrorMessage = "Spare Item is required.";

        if ((requiredvalue.Trim().Equals("")) && General.GetNullableDecimal(requiredvalue) == null)
            ucError.ErrorMessage = "Used Quantity is required";
        return (!ucError.IsError);
    }
    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }
    }
    private void UpdategvRequiredParts(string componentjoblineid, string partused)
    {
        try
        {

            int iMessageCode = 0;
            string iMessageText = "";

            PhoenixPlannedMaintenanceComponentJob.UpdateComponentJobPartsRequired(new Guid(componentjoblineid)
                , General.GetNullableDecimal(partused), ucConfirm.confirmboxvalue, ref iMessageCode, ref iMessageText);

            if (iMessageCode == 1)
                throw new ApplicationException(iMessageText);

        }
        catch (ApplicationException aex)
        {
            ucConfirm.HeaderMessage = "Please Confirm";
            ucConfirm.ErrorMessage = aex.Message;
            ucConfirm.Visible = true;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void ucConfirm_OnClick(object sender, EventArgs e)
    {
        UpdategvRequiredParts(ViewState["FLDCOMPJOBLINEITEMID"].ToString(), ViewState["REQUIREDPARTS"].ToString());
        BindData();
        gvRequiredParts.Rebind();
    }
    private void DeletegvRequiredParts(string componentjoblineid)
    {
        PhoenixPlannedMaintenanceComponentJob.DeleteComponentJobPartsRequired(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(componentjoblineid));
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            string stockitemid = nvc.Get("lblStockItemId").ToString();
            string location = nvc.Get("lblLocationId").ToString();
            string spareitemlocationid = nvc.Get("lblSpareItemLocationId").ToString();

            if ((ViewState["COMPONENTJOBID"] != null) && (ViewState["COMPONENTJOBID"].ToString() != ""))
            {
                PhoenixPlannedMaintenanceComponentJob.InsertComponentJobPartRequired(int.Parse(ViewState["VESSELID"].ToString()), stockitemid, new Guid(ViewState["COMPONENTJOBID"].ToString()),location,spareitemlocationid);
            }
            BindData();
            gvRequiredParts.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRequiredParts_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeletegvRequiredParts(((RadLabel)e.Item.FindControl("lblComponentJobLineID")).Text);
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidRequiredParts(ViewState["COMPONENTJOBID"].ToString()
                        , ((RadLabel)e.Item.FindControl("lblSpareItemId")).Text
                        , ((UserControlDecimal)e.Item.FindControl("txtQuantityEdit")).Text
                        ))
                {
                    ucError.Visible = true;
                    return;
                }

                ViewState["FLDCOMPJOBLINEITEMID"] = ((RadLabel)e.Item.FindControl("lblComponentJobLineID")).Text;
                ViewState["REQUIREDPARTS"] = ((UserControlDecimal)e.Item.FindControl("txtQuantityEdit")).Text;
                UpdategvRequiredParts(ViewState["FLDCOMPJOBLINEITEMID"].ToString(), ViewState["REQUIREDPARTS"].ToString());
                gvRequiredParts.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRequiredParts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvRequiredParts_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        }
    }
}