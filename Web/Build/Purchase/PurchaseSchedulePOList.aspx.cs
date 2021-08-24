using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;

public partial class PurchaseSchedulePOList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvSchedulePO.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (!IsPostBack)
        {
            VesselConfiguration();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            toolbar.AddImageButton("../Purchase/PurchaseSchedulePOList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvSchedulePO')", "Print Grid", "icon_print.png", "PRINT");
            //toolbar.AddImageButton("../Purchase/PurchaseSchedulePOGeneral.aspx", "Find", "search.png", "FIND");
            //toolbar.AddImageButton("../Purchase/PurchaseSchedulePOGeneral.aspx", "Clear", "clear-filter.png", "CLEAR");
            MenuMainSchedulePO.AccessRights = this.ViewState;
            MenuMainSchedulePO.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Schedule PO", "SCHEDULEPO");
            toolbar.AddButton("Line Items", "LINEITEM");
           // toolbar.AddButton("Vessels", "VESSEL");
            toolbar.AddButton("History", "HISTORY");

            MenuSchedulePO.AccessRights = this.ViewState;
            MenuSchedulePO.MenuList = toolbar.Show();
            MenuSchedulePO.SelectedMenuIndex = 0;

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            Filter.CurrentSelectedSchedulePO = null;

            if (Request.QueryString["SCHEDULEPOID"] != null && Request.QueryString["SCHEDULEPOID"].ToString() != "")
                ViewState["SCHEDULEPOID"] = Request.QueryString["SCHEDULEPOID"].ToString();         
            else
                ViewState["SCHEDULEPOID"] = "";
        }
        BindData();
        SetPageNavigator();
    }

    protected void MenuMainSchedulePO_TabStripCommand(object sender, EventArgs e)
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
        else if (dce.CommandName.ToUpper().Equals("CLEAR"))
        {
            //Filter.CurrentBudgetBillingFilter = null;
            BindData();
        }
    }

    protected void MenuSchedulePO_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("LINEITEM"))
        {
            Response.Redirect("../Purchase/PurchaseSchedulePOLineItemList.aspx?SCHEDULEPOID=" + ViewState["SCHEDULEPOID"], false);
        }
        if (dce.CommandName.ToUpper().Equals("VESSEL"))
        {
            Response.Redirect("../Purchase/PurchaseSchedulePOLineItemByVesselList.aspx", false);
        }
        if (dce.CommandName.ToUpper().Equals("HISTORY"))
        {
            Response.Redirect("../Purchase/PurchaseSchedulePOHistory.aspx?SCHEDULEPOID=" + ViewState["SCHEDULEPOID"], false);
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int vesselid = -1;

        string[] alColumns = { "FLDFORMNO", "FLDVESSELNAME", "FLDFORMTITLE", "FLDVENDORCODE", "FLDBUDGETCODE", "FLDCURRENCYCODE", "FLDSTARTDATE", "FLDENDDATE", "FLDAPPROVALSTATUS" };
        string[] alCaptions = { "Form Number", "Vessel","Form Title", "Vendor", "Budget Code", "Currency", "Start Date", "End Date", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        DataSet ds = PhoenixPurchaseSchedulePO.SchedulePOSearch(
                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , vesselid
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvSchedulePO", "Schedule PO List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvSchedulePO.DataSource = ds;
            gvSchedulePO.DataBind();

            if (Filter.CurrentSelectedSchedulePO == null)
            {
                ViewState["SCHEDULEPOID"] = ds.Tables[0].Rows[0]["FLDSCHEDULEPOID"].ToString();
                Filter.CurrentSelectedSchedulePO = ds.Tables[0].Rows[0]["FLDSCHEDULEPOID"].ToString();
                gvSchedulePO.SelectedIndex = 0;
            }
            SetRowSelection();
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseSchedulePOGeneral.aspx?SCHEDULEPOID=" + ViewState["SCHEDULEPOID"];
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvSchedulePO);
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseSchedulePOGeneral.aspx?SCHEDULEPOID=";
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int vesselid = -1;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDFORMNO", "FLDVESSELNAME", "FLDFORMTITLE", "FLDVENDORCODE", "FLDBUDGETCODE", "FLDCURRENCYCODE", "FLDSTARTDATE", "FLDENDDATE", "FLDAPPROVALSTATUS" };
        string[] alCaptions = { "Form Number", "Vessel", "Form Title", "Vendor", "Budget Code", "Currency", "Start Date", "End Date", "Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        ds = PhoenixPurchaseSchedulePO.SchedulePOSearch(
                                                         PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                       , vesselid
                                                       , null
                                                       , null
                                                       , null
                                                       , null
                                                       , null
                                                       , sortexpression
                                                       , sortdirection
                                                       , (int)ViewState["PAGENUMBER"]
                                                       , iRowCount
                                                       , ref iRowCount
                                                       , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SchedulePOList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Schedule PO List</h3></td>");
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

    private void SetRowSelection()
    {
        gvSchedulePO.SelectedIndex = -1;

        for (int i = 0; i < gvSchedulePO.Rows.Count; i++)
        {
            if (gvSchedulePO.DataKeys[i].Value.ToString().Equals(ViewState["SCHEDULEPOID"].ToString()))
            {
                gvSchedulePO.SelectedIndex = i;
            }
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvBulkPO_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvBulkPO_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvBulkPO_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(nCurrentRow);
            }
            if (e.CommandName.ToUpper().Equals("POAPPROVE"))
            {
                //if (Filter.CurrentSelectedSchedulePO != null && Filter.CurrentSelectedSchedulePO.ToString() != "")
                //{                    
                //    PhoenixPurchaseSchedulePO.SchedulePOApprovalStatusUpdate(
                //        PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(Filter.CurrentSelectedSchedulePO.ToString()));
                //    ucStatus.Text = "Schedule PO has approved successfully.";
                //    BindData();
                //}
            }
            else if (e.CommandName.ToUpper().Equals("DEAPPROVE"))
            {
                try
                {
                    PhoenixPurchaseSchedulePO.SchedulePORevokeApproval(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                new Guid(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblScheduleId")).Text));

                    ucStatus.Text = "Schedule PO approval is cancelled.";
                    BindData();
                    SetPageNavigator();
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
            if (e.CommandName.ToUpper().Equals("CREATEPO"))
            {
                if (Filter.CurrentSelectedSchedulePO != null && Filter.CurrentSelectedSchedulePO.ToString() != "")
                {                    
                    Label lblScheduleId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblScheduleId");
                    Filter.CurrentSelectedSchedulePO = lblScheduleId.Text;                    
                    PhoenixPurchaseSchedulePO.SchedulePOCopyToOrderForm(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                        new Guid(Filter.CurrentSelectedSchedulePO.ToString()));
                    ucStatus.Text = "PO's are created successfully.";
                    BindData();
                }
            }
            if (e.CommandName.ToUpper().Equals("POCOPYTOORDERFORM"))
            {
                if (Filter.CurrentSelectedSchedulePO != null && Filter.CurrentSelectedSchedulePO.ToString() != "")
                {
                    Guid orderid = new Guid(Filter.CurrentSelectedSchedulePO.ToString());
                    PhoenixPurchaseBulkPurchase.BulkPOCopyToOrderForm(PhoenixSecurityContext.CurrentSecurityContext.UserCode, orderid);
                    BindData();
                }
            }
            if (e.CommandName.ToUpper().Equals("POPOST"))
            {
                if (Filter.CurrentSelectedSchedulePO != null && Filter.CurrentSelectedSchedulePO.ToString() != "")
                {
                    Guid orderid = new Guid(Filter.CurrentSelectedSchedulePO.ToString());
                    PhoenixPurchaseBulkPurchase.BulkPOCopyToInvoice(PhoenixSecurityContext.CurrentSecurityContext.UserCode, orderid);
                    BindData();
                }
            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBulkPO_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvBulkPO_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvBulkPO_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            DataRowView drv = (DataRowView)e.Row.DataItem;

            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            if (db != null)
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

            ImageButton cmdPOApprove = (ImageButton)e.Row.FindControl("cmdPOApprove");
            ImageButton cmdCreatePO = (ImageButton)e.Row.FindControl("cmdCreatePO");
            ImageButton cmdDeApprove = (ImageButton)e.Row.FindControl("cmdDeApprove");

            Label lblApprovedYN = (Label)e.Row.FindControl("lblApprovedYN");
            Label lblCreateFirstTime = (Label)e.Row.FindControl("lblCreateFirstTime");

            cmdPOApprove.Attributes.Add("onclick", "parent.Openpopup('approval', '', '../Common/CommonApproval.aspx?docid=" + drv["FLDSCHEDULEPOID"].ToString() + "&mod=" + PhoenixModule.PURCHASE
                    + "&type=" + drv["FLDSCHEDULEPOAPPROVAL"].ToString() + "&subtype=SCHEDULEPO&user=" + drv["FLDTECHDIRECTOR"].ToString() + "," + drv["FLDFLEETMANAGER"].ToString() + "," + drv["FLDSUPT"].ToString() + "');return false;");

            if (lblApprovedYN != null && lblApprovedYN.Text == "1")
            {
                if (cmdPOApprove != null)
                    cmdPOApprove.Visible = false;
                if (cmdCreatePO != null)
                    cmdCreatePO.Visible = true;
            }

            bool visible = lblApprovedYN.Text.ToUpper().Equals("1") ? true : false;

            if (visible)
                visible = SessionUtil.CanAccess(this.ViewState, cmdDeApprove.CommandName);

            cmdDeApprove.Visible = visible;
            cmdDeApprove.ToolTip = "Revoke approval";

            if (lblCreateFirstTime != null && lblCreateFirstTime.Text == "0")
            {
                cmdCreatePO.Visible = true;
            }
            else
            {
                cmdCreatePO.Visible = false;
            }

            if (cmdPOApprove != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdPOApprove.CommandName)) cmdPOApprove.Visible = false;
            if (cmdCreatePO != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdCreatePO.CommandName)) cmdCreatePO.Visible = false;

            Label lblAppStatus = (Label)e.Row.FindControl("lblApprovalStatusName");
            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipAddress");
            lblAppStatus.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lblAppStatus.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
        }
    }

    protected void gvBulkPO_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvSchedulePO.SelectedIndex = se.NewSelectedIndex;
        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["SCHEDULEPOID"] = ((Label)gvSchedulePO.Rows[rowindex].FindControl("lblScheduleId")).Text;
            Filter.CurrentSelectedSchedulePO = ((Label)gvSchedulePO.Rows[rowindex].FindControl("lblScheduleId")).Text;
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseSchedulePOGeneral.aspx?SCHEDULEPOID=" + ViewState["SCHEDULEPOID"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixCommonPurchase.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Filter.CurrentSelectedSchedulePO == null)
                ViewState["SCHEDULEPOID"] = null;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
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
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvSchedulePO.SelectedIndex = -1;
        gvSchedulePO.EditIndex = -1;
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
