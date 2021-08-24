using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;

public partial class PurchaseSchedulePOLineItemByVesselList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        try
        {

            foreach (GridViewRow r in gvSchedulePOLineItemByVessel.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    Page.ClientScript.RegisterForEventValidation(gvSchedulePOLineItemByVessel.UniqueID, "Edit$" + r.RowIndex.ToString());
                }
            }
            base.Render(writer);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Purchase/PurchaseSchedulePOLineItemByVesselList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvSchedulePOLineItemByVessel')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','PurchaseBulkPOLineItemByVesselGeneral.aspx?callfrom=schedulepo')", "Add", "add.png", "ADDVESSEL");

            MenuVessel.AccessRights = this.ViewState;
            MenuVessel.MenuList = toolbar.Show();
            MenuVessel.SetTrigger(pnlOwnreBudgetcodeEntry);

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Purchase/PurchaseSchedulePOLineItemByVesselList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvSchedulePOLineItem')", "Print Grid", "icon_print.png", "PRINT");
            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Schedule PO", "SCHEDULEPO");
            toolbar.AddButton("Line Items", "LINEITEM");
            toolbar.AddButton("Vessels", "VESSEL");
            toolbar.AddButton("History", "HISTORY");

            MenuBulkPO.AccessRights = this.ViewState;
            MenuBulkPO.MenuList = toolbar.Show();
            MenuBulkPO.SelectedMenuIndex = 2;

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            ViewState["PAGENUMBER1"] = 1;
            ViewState["SORTEXPRESSION1"] = null;
            ViewState["SORTDIRECTION1"] = null;
            ViewState["CURRENTINDEX1"] = 1;

            ViewState["LINEITEMID"] = "";
            if (Filter.CurrentSelectedSchedulePO != null && Filter.CurrentSelectedSchedulePO.ToString() != "")
                ViewState["SCHEDULEPOID"] = Filter.CurrentSelectedSchedulePO.ToString();
            else
                ViewState["SCHEDULEPOID"] = "";
        }

        BindLineItem();
        SetPageNavigatorLineItem();
        BindData();
        SetPageNavigator();
    }

    protected void MenuBulkPO_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SCHEDULEPO"))
        {
            Response.Redirect("../Purchase/PurchaseSchedulePOList.aspx?", false);
        }
        if (dce.CommandName.ToUpper().Equals("LINEITEM"))
        {
            Response.Redirect("../Purchase/PurchaseSchedulePOLineItemList.aspx", false);
        }
        if (dce.CommandName.ToUpper().Equals("VESSEL"))
        {
            Response.Redirect("../Purchase/PurchaseSchedulePOLineItemByVesselList.aspx", false);
        }
        if (dce.CommandName.ToUpper().Equals("HISTORY"))
        {
            Response.Redirect("../Purchase/PurchaseSchedulePOHistory.aspx", false);
        }
    }

    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER1"] = 1;
            BindLineItem();
            SetPageNavigatorLineItem();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcelLineItem();
        }
    }

    protected void MenuVessel_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindLineItem()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDLINEITEMNAME", "FLDBUDGETCODE", "FLDUNITNAME", "FLDPRICE", "FLDTOTALORDERQUANTITY" };
        string[] alCaptions = { "Item Name", "Budget Code", "Unit", "Unit Price", "Total Order Quantity" };

        string sortexpression = (ViewState["SORTEXPRESSION1"] == null) ? null : (ViewState["SORTEXPRESSION1"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION1"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION1"].ToString());

        DataSet ds = PhoenixPurchaseSchedulePO.SchedulePOLineItemSearch(
                                                                  General.GetNullableGuid(ViewState["SCHEDULEPOID"].ToString())
                                                                 , sortexpression
                                                                 , sortdirection
                                                                 , (int)ViewState["PAGENUMBER1"]
                                                                 , General.ShowRecords(null)
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);

        General.SetPrintOptions("gvSchedulePOLineItem", "Schedule PO LineItem list", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvSchedulePOLineItem.DataSource = ds;
            gvSchedulePOLineItem.DataBind();
            if (ViewState["LINEITEMID"] == null || ViewState["LINEITEMID"].ToString() == "")
            {
                ViewState["LINEITEMID"] = ds.Tables[0].Rows[0]["FLDLINEITEMID"].ToString();
                Filter.CurrentSelectedSchedulePOLineItemId = ds.Tables[0].Rows[0]["FLDLINEITEMID"].ToString();
                gvSchedulePOLineItem.SelectedIndex = 0;
            }
            SetRowSelectionLineItem();
            BindData();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvSchedulePOLineItem);
        }

        ViewState["ROWCOUNT1"] = iRowCount;
        ViewState["TOTALPAGECOUNT1"] = iTotalPageCount;
    }

    protected void ShowExcelLineItem()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDLINEITEMNAME", "FLDBUDGETCODE", "FLDUNITNAME", "FLDPRICE", "FLDTOTALORDERQUANTITY" };
        string[] alCaptions = { "Item Name", "Budget Code", "Unit", "Unit Price", "Total Order Quantity" };

        int? sortdirection = null;
        string sortexpression = (ViewState["SORTEXPRESSION1"] == null) ? null : (ViewState["SORTEXPRESSION1"].ToString());
        if (ViewState["SORTDIRECTION1"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION1"].ToString());

        if (ViewState["ROWCOUNT1"] == null || Int32.Parse(ViewState["ROWCOUNT1"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT1"].ToString());

        DataSet ds = PhoenixPurchaseSchedulePO.SchedulePOLineItemSearch(
                                                                  General.GetNullableGuid(ViewState["SCHEDULEPOID"].ToString())
                                                                 , sortexpression
                                                                 , sortdirection
                                                                 , (int)ViewState["PAGENUMBER1"]
                                                                 , iRowCount
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SchedulePOLineItem.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Schedule PO LineItem list</h3></td>");
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

    protected void gvSchedulePOLineItem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindData();
        SetPageNavigator();
    }

    protected void gvSchedulePOLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                Label lblLineItemId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblLineItemId");
                if (lblLineItemId != null)
                {
                    ViewState["LINEITEMID"] = lblLineItemId.Text;
                    Filter.CurrentSelectedSchedulePOLineItemId = lblLineItemId.Text;
                    gvSchedulePOLineItem.SelectedIndex = nCurrentRow;
                }
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSchedulePOLineItem_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvSchedulePOLineItem.SelectedIndex = -1;
        gvSchedulePOLineItem.EditIndex = -1;
        ViewState["CURRENTROW1"] = -1;

        ViewState["SORTEXPRESSION1"] = se.SortExpression;

        if (ViewState["SORTDIRECTION1"] != null && ViewState["SORTDIRECTION1"].ToString() == "0")
            ViewState["SORTDIRECTION1"] = 1;
        else
            ViewState["SORTDIRECTION1"] = 0;

        BindLineItem();
        SetPageNavigatorLineItem();
    }

    protected void gvSchedulePOLineItem_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
    }

    protected void gvSchedulePOLineItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvSchedulePOLineItem.SelectedIndex = se.NewSelectedIndex;
        ViewState["LINEITEMID"] = ((Label)gvSchedulePOLineItem.Rows[se.NewSelectedIndex].FindControl("lblLineItemId")).Text;
        Filter.CurrentSelectedSchedulePOLineItemId = ((Label)gvSchedulePOLineItem.Rows[se.NewSelectedIndex].FindControl("lblLineItemId")).Text;
        BindData();
    }

    private void SetRowSelectionLineItem()
    {
        gvSchedulePOLineItem.SelectedIndex = -1;
        for (int i = 0; i < gvSchedulePOLineItem.Rows.Count; i++)
        {
            if (gvSchedulePOLineItem.DataKeys[i].Value.ToString().Equals(ViewState["LINEITEMID"].ToString()))
            {
                gvSchedulePOLineItem.SelectedIndex = i;
            }
        }
    }

    private void SetPageNavigatorLineItem()
    {
        cmdPrevious1.Enabled = IsPreviousEnabled1();
        cmdNext1.Enabled = IsNextEnabled1();
        lblPagenumber1.Text = "Page " + ViewState["PAGENUMBER1"].ToString();
        lblPages1.Text = " of " + ViewState["TOTALPAGECOUNT1"].ToString() + " Pages. ";
        lblRecords1.Text = "(" + ViewState["ROWCOUNT1"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled1()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER1"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT1"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled1()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER1"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT1"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    protected void cmdGo_Click1(object sender, EventArgs e)
    {
        int result;
        gvSchedulePOLineItem.SelectedIndex = -1;
        gvSchedulePOLineItem.EditIndex = -1;
        ViewState["CURRENTROW1"] = -1;

        if (Int32.TryParse(txtnopage1.Text, out result))
        {
            ViewState["PAGENUMBER1"] = Int32.Parse(txtnopage1.Text);

            if ((int)ViewState["TOTALPAGECOUNT1"] < Int32.Parse(txtnopage1.Text))
                ViewState["PAGENUMBER1"] = ViewState["TOTALPAGECOUNT1"];

            if (0 >= Int32.Parse(txtnopage1.Text))
                ViewState["PAGENUMBER1"] = 1;

            if ((int)ViewState["PAGENUMBER1"] == 0)
                ViewState["PAGENUMBER1"] = 1;

            txtnopage1.Text = ViewState["PAGENUMBER1"].ToString();
        }
        ViewState["BUDGETID"] = "";
        BindLineItem();
        SetPageNavigatorLineItem();
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick1(object sender, CommandEventArgs ce)
    {
        gvSchedulePOLineItem.SelectedIndex = -1;
        gvSchedulePOLineItem.EditIndex = -1;
        ViewState["CURRENTROW1"] = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER1"] = (int)ViewState["PAGENUMBER1"] - 1;
        else
            ViewState["PAGENUMBER1"] = (int)ViewState["PAGENUMBER1"] + 1;


        ViewState["BUDGETID"] = "";
        BindLineItem();
        SetPageNavigatorLineItem();
        BindData();
        SetPageNavigator();
    }


    /// <summary>
    /// ///////// Owner Budget Code Map GridView Begins..
    /// </summary>

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDBUDGETCODE", "FLDREQUESTEDQUANTITY", "FLDPRICE", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Vessel Name", "Budget Code", "Order Qty", "Unit Price", "Total" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseSchedulePO.SchedulePOLineItemByVesselSearch(
                                                                   General.GetNullableGuid(ViewState["SCHEDULEPOID"].ToString())
                                                                 , General.GetNullableGuid(ViewState["LINEITEMID"].ToString())
                                                                 , sortexpression
                                                                 , sortdirection
                                                                 , (int)ViewState["PAGENUMBER"]
                                                                 , General.ShowRecords(null)
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);

        General.SetPrintOptions("gvSchedulePOLineItemByVessel", "Schedule PO Line Item list by Vessel", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvSchedulePOLineItemByVessel.DataSource = ds;
            gvSchedulePOLineItemByVessel.DataBind();
            if (ViewState["LINEITEMBYVESSELID"] == null)
            {
                ViewState["LINEITEMBYVESSELID"] = ds.Tables[0].Rows[0]["FLDLINEITEMBYVESSELID"].ToString();
                gvSchedulePOLineItemByVessel.SelectedIndex = 0;
            }
            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvSchedulePOLineItemByVessel);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDBUDGETCODE", "FLDREQUESTEDQUANTITY", "FLDPRICE", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Vessel Name", "Budget Code", "Order Qty", "Unit Price", "Total" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseSchedulePO.SchedulePOLineItemByVesselSearch(
                                                              new Guid(ViewState["SCHEDULEPOID"].ToString())
                                                             , new Guid(ViewState["LINEITEMID"].ToString())
                                                             , sortexpression
                                                             , sortdirection
                                                             , (int)ViewState["PAGENUMBER"]
                                                             , iRowCount
                                                             , ref iRowCount
                                                             , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=LineItemByVessel.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Schedule PO Line Item List by Vessel</h3></td>");
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

    protected void gvSchedulePOLineItemByVessel_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvSchedulePOLineItemByVessel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals(""))
                return;

            int nCurrentRow = int.Parse(e.CommandArgument.ToString());
           
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Label lblLineItemByVesselId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblLineItemByVesselId");
                if (lblLineItemByVesselId != null)
                    ViewState["LINEITEMBYVESSELID"] = lblLineItemByVesselId.Text;

                VesselLineItemDelete(new Guid(ViewState["LINEITEMBYVESSELID"].ToString()));
                ViewState["LINEITEMBYVESSELID"] = null;
                BindData();
                SetPageNavigator();
            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSchedulePOLineItemByVessel_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvSchedulePOLineItemByVessel.SelectedIndex = -1;
        gvSchedulePOLineItemByVessel.EditIndex = -1;
        ViewState["CURRENTROW"] = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvSchedulePOLineItemByVessel_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvSchedulePOLineItemByVessel_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            ViewState["LINEITEMBYVESSELID"] = ((Label)gvSchedulePOLineItemByVessel.Rows[de.NewEditIndex].FindControl("lblLineItemByVesselId")).Text;

            BindData();
            SetPageNavigator();

            ((UserControlMaskNumber)_gridView.Rows[de.NewEditIndex].FindControl("ucRequestedQtyEdit")).SetFocus();
            ((UserControlMaskNumber)_gridView.Rows[de.NewEditIndex].FindControl("ucRequestedQtyEdit")).Attributes.Add("onfocus", "this.select()");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSchedulePOLineItemByVessel_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidData())
            {
                ucError.Visible = true;
                return;
            }
            if (ViewState["LINEITEMBYVESSELID"] != null && ViewState["LINEITEMBYVESSELID"].ToString() != "")
            {
                Label lblVesselId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId"));
                int vesselid = int.Parse(lblVesselId.Text);

                VesselLineItemUpdate(new Guid(ViewState["SCHEDULEPOID"].ToString())
                                    , new Guid(ViewState["LINEITEMID"].ToString())
                                    , new Guid(ViewState["LINEITEMBYVESSELID"].ToString())
                                    , vesselid
                                    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBudgetIdEdit")).Text
                                    , ((UserControlMaskNumber)(_gridView.Rows[nCurrentRow].FindControl("ucRequestedQtyEdit"))).Text.Replace(".", ""));
                                    
            }
            _gridView.EditIndex = -1;
            BindLineItem();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSchedulePOLineItemByVessel_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox tb1 = (TextBox)e.Row.FindControl("txtBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

            tb1 = (TextBox)e.Row.FindControl("txtBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

            tb1 = (TextBox)e.Row.FindControl("txtBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

            ImageButton ib1 = (ImageButton)e.Row.FindControl("btnShowBudgetEdit");

            Label lblVesselId = (Label)e.Row.FindControl("lblVesselId");

            if (ib1 != null && lblVesselId != null)
            {
                ib1.Attributes.Add("onclick", "return showPickList('spnPickListMainBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + lblVesselId.Text + "&budgetdate=" + DateTime.Now.Date + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName)) ib1.Visible = false;
            }

            ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
            if (cmdDelete != null) cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }

    protected void gvSchedulePOLineItemByVessel_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvSchedulePOLineItemByVessel.SelectedIndex = se.NewSelectedIndex;
        Label lblLineItemByVesselId = (Label)gvSchedulePOLineItem.Rows[se.NewSelectedIndex].FindControl("lblLineItemByVesselId");
        if (lblLineItemByVesselId != null)
            ViewState["LINEITEMBYVESSELID"] = lblLineItemByVesselId.Text;
    }

    protected void gvSchedulePOLineItemByVessel_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvSchedulePOLineItemByVessel, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
        SetKeyDownScroll(sender, e);
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

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvSchedulePOLineItemByVessel.SelectedIndex = -1;
        gvSchedulePOLineItemByVessel.EditIndex = -1;
        ViewState["CURRENTROW"] = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvSchedulePOLineItemByVessel.SelectedIndex = -1;
        gvSchedulePOLineItemByVessel.EditIndex = -1;
        ViewState["CURRENTROW"] = -1;

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
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvSchedulePOLineItemByVessel.SelectedIndex = -1;
        gvSchedulePOLineItemByVessel.EditIndex = -1;
        ViewState["CURRENTROW"] = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["LINEITEMBYVESSELID"] = null;
        BindData();
        SetPageNavigator();
    }

    private bool IsValidData()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ViewState["SCHEDULEPOID"] == null || General.GetNullableGuid(ViewState["SCHEDULEPOID"].ToString()) == null)
            ucError.ErrorMessage = "Bulk PO details should be recorded first.";

        if (ViewState["LINEITEMID"] == null || General.GetNullableGuid(ViewState["LINEITEMID"].ToString()) == null)
            ucError.ErrorMessage = "Either Line Items are not available or No Line Item is not selected.";

        return (!ucError.IsError);
    }

    private void SetRowSelection()
    {
        gvSchedulePOLineItemByVessel.SelectedIndex = -1;
        for (int i = 0; i < gvSchedulePOLineItemByVessel.Rows.Count; i++)
        {
            if (gvSchedulePOLineItemByVessel.DataKeys[i].Value.ToString().Equals(ViewState["LINEITEMBYVESSELID"].ToString()))
            {
                gvSchedulePOLineItemByVessel.SelectedIndex = i;
                //PhoenixAccountsVoucher.VoucherNumber = ((Label)gvInvoice.Rows[i].FindControl("lblInvoiceid")).Text;                
            }
        }
    }

    private void VesselLineItemInsert(Guid SCHEDULEPOID, Guid lineitemid, string vessellist, int? recievedqty)
    {
        PhoenixPurchaseBulkPurchase.BulkPOLineItemByVesselInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , SCHEDULEPOID
                                                                , lineitemid
                                                                , General.GetNullableString(vessellist)
                                                                , null
                                                                , recievedqty
                                                                , null);
    }

    private void VesselLineItemUpdate(Guid SCHEDULEPOID, Guid lineitemid, Guid lineitembyvesselid, int vesselid, string budgetid, string requestedqty)
    {
        PhoenixPurchaseSchedulePO.SchedulePOLineItemByVesselUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , SCHEDULEPOID
                                                                , lineitemid
                                                                , lineitembyvesselid
                                                                , vesselid
                                                                , General.GetNullableInteger(budgetid)
                                                                , General.GetNullableInteger(requestedqty));                                                                
    }

    private void VesselLineItemDelete(Guid lineitembyvesselid)
    {
        PhoenixPurchaseSchedulePO.SchedulePOLineItemByVesselDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , lineitembyvesselid);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
