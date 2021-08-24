using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;

public partial class InventoryComponentType : PhoenixBasePage
{    
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            if (!IsPostBack && ViewState["COMPONENTTYPEID"] != null)
            {
                tvwComponentType.FindNodeByValue(tvwComponentType.ThisTreeView.Nodes, ViewState["COMPONENTTYPEID"].ToString());
            }            
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryComponentType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvComponentType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryComponentTypeFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRegistersComponentType.AccessRights = this.ViewState;
            MenuRegistersComponentType.MenuList = toolbargrid.Show();
            MenuRegistersComponentType.SetTrigger(pnlComponentType);

            if (!IsPostBack)
            {
                if (Request.QueryString["tv"] != null)
                    Title1.Text = "Component Type Hierarchy";
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                //toolbarmain.AddButton("Search", "SEARCH");
                toolbarmain.AddButton("General", "GENERAL");
                toolbarmain.AddButton("Details", "DETAILS");
                toolbarmain.AddButton("Attachment", "ATTACHMENT");               
                //toolbarmain.AddButton("Parts", "PARTS");
                toolbarmain.AddButton("Jobs", "JOBS");
                toolbarmain.AddButton("Vessel", "VESSEL");
                //toolbarmain.AddButton("Counter", "COUNTER");
                MenuComponentType.AccessRights = this.ViewState;
                MenuComponentType.MenuList = toolbarmain.Show();
                MenuComponentType.SetTrigger(pnlComponentType);
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

                ViewState["ISTREENODECLICK"] = false;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["COMPONENTTYPEID"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;

                if (Request.QueryString["COMPONENTTYPEID"] != null)
                {
                    ViewState["COMPONENTTYPEID"] = Request.QueryString["COMPONENTTYPEID"].ToString();
                    ViewState["DTKEY"] = PhoenixInventoryComponentType.ListComponentType(new Guid(ViewState["COMPONENTTYPEID"].ToString())).Tables[0].Rows[0]["FLDDTKEY"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Inventory/InventoryComponentTypeGeneral.aspx?" + (Request.QueryString["tv"] != null ? ("tv=1&") : string.Empty) + "COMPONENTTYPEID=" + Request.QueryString["COMPONENTTYPEID"].ToString();
                }
                if (Request.QueryString["SETCURRENTNAVIGATIONTAB"] != null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = Request.QueryString["SETCURRENTNAVIGATIONTAB"].ToString();
                }
                BindTreeData();

                MenuComponentType.SelectedMenuIndex = 0;

            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
    {

        try
        {
            ViewState["ISTREENODECLICK"] = true;

            TreeViewSelectNodeEvent tvsne = (TreeViewSelectNodeEvent)e;
            if (tvsne.SelectedNode.Value != "Root")
            {
                string selectednode = tvsne.SelectedNode.Value.ToString();
                string selectedvalue = tvsne.SelectedNode.Text.ToString();

                ViewState["COMPONENTTYPEID"] = selectednode;

                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentTypeGeneral.aspx";
                }
                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
                {
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?" + (Request.QueryString["tv"] != null ? ("tv=1&") : string.Empty) + "COMPONENTTYPEID=" + ViewState["COMPONENTTYPEID"];
                }                             
                // Disabling the root node click
                string script = "var ar = document.getElementById(\"tvwComponentType_tvwTreet0\"); ar.href=\"#\"; ar.onclick=null;\r\n";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "disableScript", script, true);                
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void BindTreeData()
    {

        try
        {
            if (Request.QueryString["tv"] != null)
            {
                DataSet ds = new DataSet();
                ds = PhoenixInventoryComponentType.TreeComponentType();
                tvwComponentType.DataTextField = "FLDCOMPONENTNAME";
                tvwComponentType.DataValueField = "FLDCOMPONENTTYPEID";
                tvwComponentType.ParentNodeField = "FLDPARENTID";
                tvwComponentType.XPathField = "xpath";
                tvwComponentType.RootText = "Components";
                tvwComponentType.PopulateTree(ds);
                TreeView tvw = (TreeView)tvwComponentType.FindControl("tvwTree");
                ((TreeNode)tvw.Nodes[0]).Expand();

                // Disabling the root node click
                string script = "var ar = document.getElementById(\"tvwComponentType_tvwTreet0\"); ar.href=\"#\"; ar.onclick=null;\r\n";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "disableScript", script, true);                
            }
            else
            {
                divComponentType.Visible = false;
                ucVerticalSplit.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuComponentType_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        PhoenixInventorySpareItem objinventorystockitem = new PhoenixInventorySpareItem();

        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Inventory/InventoryComponentTypeFilter.aspx");
        }
        else if (dce.CommandName.ToUpper().Equals("GENERAL"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentTypeGeneral.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("DETAILS"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentTypeDetail.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("ATTACHMENT"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
        }
        else if (dce.CommandName.ToUpper().Equals("VESSEL"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentTypeVesselMapping.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("PARTS"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentTypeSpareItem.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("JOBS"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentTypeJobList.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("COUNTER"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentTypeJobList.aspx";
        }

        if (dce.CommandName.ToUpper().Equals("ATTACHMENT"))
        {
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
        }
        else
        {
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?" + (Request.QueryString["tv"] != null ? ("tv=1&") : string.Empty) + "COMPONENTTYPEID=" + ViewState["COMPONENTTYPEID"];
        }
        //Retain the Tree Selection after post back
        if (Request.QueryString["tv"] != null)
        {
            TreeView tvw = tvwComponentType.ThisTreeView;
            ((TreeNode)tvw.Nodes[0]).Expand();
            // Disabling the root node click
            string script = "var ar = document.getElementById(\"tvwComponentType_tvwTreet0\"); ar.href=\"#\"; ar.onclick=null;\r\n";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "disableScript", script, true);
            if ((bool)ViewState["ISTREENODECLICK"] == false)
                tvwComponentType.FindNodeByValue(tvw.Nodes, ViewState["COMPONENTTYPEID"].ToString());
        }
    }

    protected void gvComponentType_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvComponentType.SelectedIndex = se.NewSelectedIndex;
        ViewState["COMPONENTTYPEID"] = ((Label)gvComponentType.Rows[se.NewSelectedIndex].FindControl("lblComponentTypeId")).Text;
        ViewState["DTKEY"] = ((Label)gvComponentType.Rows[se.NewSelectedIndex].FindControl("lbldtkey")).Text;
        BindData();        
    }   
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "MAKERNAME", "VENDORNAME", "COMPONENTCLASSNAME" };
            string[] alCaptions = { "Number", "Name", "Maker", "Preferred Vendor", "Class" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            NameValueCollection nvc = Filter.CurrentComponentTypeFilterCriteria;
            DataSet ds = PhoenixCommonInventory.ComponentTypeSearch(General.GetNullableInteger(nvc != null ? nvc.Get("ddlComponentClass") : string.Empty)
                , nvc != null ? nvc.Get("txtNumber").ToString() : null
                , nvc != null ? nvc.Get("txtName").ToString() : null
                , General.GetNullableInteger(nvc != null ? nvc.Get("txtVendorId").ToString() : string.Empty)
                , General.GetNullableInteger(nvc != null ? nvc.Get("txtMakerid").ToString() : string.Empty), null, sortexpression, sortdirection,
            1,
           iRowCount,
            ref iRowCount,
            ref iTotalPageCount);
            General.ShowExcel("Component Type", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRegistersComponentType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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

    private void BindData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "MAKERNAME", "VENDORNAME", "COMPONENTCLASSNAME" };
            string[] alCaptions = { "Number", "Name", "Maker", "Preferred Vendor", "Class" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            if (Filter.CurrentComponentTypeFilterCriteria != null)
            {
                NameValueCollection nvc = Filter.CurrentComponentTypeFilterCriteria;

                ds = PhoenixCommonInventory.ComponentTypeSearch(General.GetNullableInteger(nvc != null ? nvc.Get("ddlComponentClass") : string.Empty), nvc.Get("txtNumber").ToString(), nvc.Get("txtName").ToString(),
                    General.GetNullableInteger(nvc.Get("txtVendorId").ToString()), General.GetNullableInteger(nvc.Get("txtMakerid").ToString()),
                    nvc != null ? nvc.Get("txtType") : string.Empty, sortexpression, sortdirection,
                    (int)ViewState["PAGENUMBER"], General.ShowRecords(null),
                    ref iRowCount, ref iTotalPageCount);

            }
            else
            {
                ds = PhoenixCommonInventory.ComponentTypeSearch(null, null, null, null, null, null, sortexpression,
                 sortdirection, (int)ViewState["PAGENUMBER"],
                 General.ShowRecords(null),
                 ref iRowCount,
                 ref iTotalPageCount);

            }
            if (Request.QueryString["tv"] == null)
            {
                General.SetPrintOptions("gvComponentType", "Component Type", alCaptions, alColumns, ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvComponentType.DataSource = ds;
                    gvComponentType.DataBind();

                    if (ViewState["COMPONENTTYPEID"] == null)
                    {

                        gvComponentType.SelectedIndex = 0;

                    }

                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    ShowNoRecordsFound(dt, gvComponentType);
                    ifMoreInfo.Attributes["src"] = "../Inventory/InventoryComponentTypeGeneral.aspx";
                }

                ViewState["ROWCOUNT"] = iRowCount;
                ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
                SetPageNavigator();
            }
            else
            {
                divGrid.Visible = false;
                MenuRegistersComponentType.Visible = false;
                string script = "resizeFrame(document.getElementById('ifMoreInfo')); resizeFrame(document.getElementById('divComponentType'));\r\n";                
                ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
            }
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0].ToString() != string.Empty)
            {
                if (ViewState["COMPONENTTYPEID"] == null)
                {
                    ViewState["COMPONENTTYPEID"] = ds.Tables[0].Rows[0][0].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryComponentTypeGeneral.aspx";
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
                {
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?" + (Request.QueryString["tv"] != null ? ("tv=1&") : string.Empty) + "COMPONENTTYPEID=" + ViewState["COMPONENTTYPEID"].ToString();
                }
                if (Request.QueryString["tv"] == null)
                {
                    if ((bool)ViewState["ISTREENODECLICK"] == false)
                        SetRowSelection();
                }
                SetTabHighlight();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvComponentType_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["STORETYPEID"] = null;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;


        ViewState["COMPONENTTYPEID"] = null;

        BindData();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        try
        {

            ImageButton ib = (ImageButton)sender;

            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponentType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {

            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponentType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
    }


    protected void gvComponentType_RowDeleting(object sender, GridViewDeleteEventArgs de)
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

    protected void gvComponentType_RowEditing(object sender, GridViewEditEventArgs de)
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

    protected void gvComponentType_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {

        try
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["SORTEXPRESSION"] != null)
                {
                    HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                    if (img != null)
                    {
                        if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                            img.Src = Session["images"] + "/arrowUp.png";
                        else
                            img.Src = Session["images"] + "/arrowDown.png";

                        img.Visible = true;
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int nCount = 0; nCount < e.Row.Cells.Count; nCount++)
                    e.Row.Cells[nCount].Attributes.Add("columnname", "colname" + nCount);
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
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

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
        {

            int result;
            if (Int32.TryParse(txtnopage.Text, out result))
            {
                ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

                if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

                if (0 >= Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = 1;

                if ((int)ViewState["PAGENUMBER"] == 0)
                    ViewState["PAGENUMBER"] = 1;

                txtnopage.Text = ViewState["PAGENUMBER"].ToString();
            }
            ViewState["COMPONENTTYPEID"] = null;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {

        try
        {

            gvComponentType.SelectedIndex = -1;
            gvComponentType.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;
            ViewState["COMPONENTTYPEID"] = null;
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPageNavigator()
    {
        try
        {

            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
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
            BindTreeData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void SetTabHighlight()
    {
        try
        {
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryComponentTypeGeneral.aspx"))
            {
                MenuComponentType.SelectedMenuIndex = 0;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryComponentTypeDetail.aspx"))
            {
                MenuComponentType.SelectedMenuIndex = 1;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("CommonFileAttachment.aspx"))
            {
                MenuComponentType.SelectedMenuIndex = 2;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentTypeJobList.aspx"))
            {
                MenuComponentType.SelectedMenuIndex = 3;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryComponentTypeVesselMapping.aspx"))
            {
                MenuComponentType.SelectedMenuIndex = 4;
            }
            //else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryComponentTypeSpareItem.aspx"))
            //{
            //    MenuComponentType.SelectedMenuIndex = 4;
            //}            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetRowSelection()
    {
        gvComponentType.SelectedIndex = -1;
        for (int i = 0; i < gvComponentType.Rows.Count; i++)
        {
            if (gvComponentType.DataKeys[i].Value.ToString().Equals(ViewState["COMPONENTTYPEID"].ToString()))
            {
                gvComponentType.SelectedIndex = i;
                PhoenixInventoryComponentType.ComponentTypeItemNumber = ((LinkButton)gvComponentType.Rows[i].FindControl("lnkStockItemName")).Text;

                ViewState["DTKEY"] = ((Label)gvComponentType.Rows[gvComponentType.SelectedIndex].FindControl("lbldtkey")).Text;
            }
        }
    }    
}
