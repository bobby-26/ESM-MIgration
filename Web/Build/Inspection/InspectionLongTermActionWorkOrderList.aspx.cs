using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;

public partial class InspectionLongTermActionWorkOrderList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvLongTermActionWorkOrder.Rows)
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
            Session["New"] = "N";
            if (Session["CHECKED_ITEMS"] != null)
                Session.Remove("CHECKED_ITEMS");

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;

            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }

            VesselConfiguration();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            toolbar.AddImageButton("../Inspection/InspectionLongTermActionWorkOrderList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvLongTermActionWorkOrder')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("javascript:Openpopup('Filter','','InspectionWorkOrderFilter.aspx'); return false;", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Inspection/InspectionLongTermActionWorkOrderList.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuLongTermAction.AccessRights = this.ViewState;
            MenuLongTermAction.MenuList = toolbar.Show();
            
            toolbar = new PhoenixToolbar();
            //toolbar.AddButton("Search", "SEARCH");
            //toolbar.AddButton("Work Order", "WORKORDER");
            

            MenuBulkPO.AccessRights = this.ViewState;
            //MenuBulkPO.MenuList = toolbar.Show();
            //MenuBulkPO.SelectedMenuIndex = 1;

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            if (Request.QueryString["WORKORDERID"] != null && Request.QueryString["WORKORDERID"].ToString() != "")
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
            else
                ViewState["WORKORDERID"] = null;

            if (Request.QueryString["DTKEY"] != null && Request.QueryString["DTKEY"].ToString() != null)
                ViewState["DTKEY"] = Request.QueryString["DTKEY"];
            else
                ViewState["DTKEY"] = null;

            if (Request.QueryString["VESSELID"] != null)
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            else
                ViewState["VESSELID"] = null;

            if (Request.QueryString["STATUS"] != null)
                ViewState["STATUS"] = Request.QueryString["STATUS"].ToString();
            else
                ViewState["STATUS"] = null;

            if (Request.QueryString["DEPARTMENTID"] != null)
                ViewState["DEPARTMENTID"] = Request.QueryString["DEPARTMENTID"].ToString();
            else
                ViewState["DEPARTMENTID"] = null;

            if (Request.QueryString["WORKORDERID"] != null)
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
            else
                ViewState["WORKORDERID"] = null;

            if (Request.QueryString["TASKID"] != null && Request.QueryString["TASKID"].ToString() != "")
                ViewState["TASKID"] = Request.QueryString["TASKID"].ToString();
            else
                ViewState["TASKID"] = null;

            SetRowSelection();
        }
        BindData();
        SetPageNavigator();
    }

    protected void MenuLongTermAction_TabStripCommand(object sender, EventArgs e)
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
            Filter.CurrentOfficeWorkOrderFilter = null;
            BindData();
        }
    }

    protected void MenuBulkPO_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        
        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Inspection/InspectionWorkOrderFilter.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentOfficeTaskFilter = null;
            BindData();
            SetPageNavigator();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDWONUMBER", "FLDWODESCRIPTION", "FLDDEPARTMENTNAME", "FLDACTIONTAKEN", "FLDSTATUSNAME", "FLDCOMPLETEDDATE" };
        string[] alCaptions = { "Work Order Number", "Description", "Department", "Action Taken", "Status", "Closed Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        
        NameValueCollection nvc = Filter.CurrentOfficeWorkOrderFilter;

        DataSet ds = PhoenixInspectionLongTermAction.WorkOrderSearch(
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                , nvc !=null  ? General.GetNullableString(nvc.Get("txtWorkOrderNo")): null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucDepartment")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneFrom")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneTo")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlStatus")) : null
                                                                , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                                );


        General.SetPrintOptions("gvLongTermActionWorkOrder", "Work Order List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvLongTermActionWorkOrder.DataSource = ds;
            gvLongTermActionWorkOrder.DataBind();

            if (ViewState["WORKORDERID"] == null)
            {
                ViewState["WORKORDERID"] = ds.Tables[0].Rows[0]["FLDWORKORDERID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();                
                gvLongTermActionWorkOrder.SelectedIndex = 0;
            }
            SetRowSelection();
            ifMoreInfo.Attributes["src"] = "../Inspection/InspectionLongTermActionWorkOrderGeneral.aspx?WORKORDERID=" + ViewState["WORKORDERID"] + "&DTKEY=" + ViewState["DTKEY"];
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvLongTermActionWorkOrder);
            ifMoreInfo.Attributes["src"] = "../Inspection/InspectionLongTermActionWorkOrderGeneral.aspx?WORKORDERID=";
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDWONUMBER", "FLDWODESCRIPTION", "FLDDEPARTMENTNAME", "FLDACTIONTAKEN", "FLDSTATUSNAME", "FLDCOMPLETEDDATE" };
        string[] alCaptions = { "Work Order Number", "Description","Department","Action Taken","Status","Closed Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentOfficeWorkOrderFilter;

        ds  =   PhoenixInspectionLongTermAction.WorkOrderSearch(
                                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , sortexpression
                                                    , sortdirection
                                                    , (int)ViewState["PAGENUMBER"]
                                                    , iRowCount
                                                    , ref iRowCount
                                                    , ref iTotalPageCount
                                                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtWorkOrderNo")) : null
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ucDepartment")) : null
                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneFrom")) : null
                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneTo")) : null
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlStatus")) : null
                                                    , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=WorkOrderList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Work Order List</h3></td>");
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
        gvLongTermActionWorkOrder.SelectedIndex = -1;

        for (int i = 0; i < gvLongTermActionWorkOrder.Rows.Count; i++)
        {
            if (gvLongTermActionWorkOrder.DataKeys[i].Value.ToString().Equals(ViewState["WORKORDERID"].ToString()))
            {
                gvLongTermActionWorkOrder.SelectedIndex = i;
            }
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvLongTermActionWorkOrder_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvLongTermActionWorkOrder_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvLongTermActionWorkOrder_RowCommand(object sender, GridViewCommandEventArgs e)
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
            else if (e.CommandName.ToUpper().Equals("ACCEPT"))
            {
                BindPageURL(nCurrentRow);
                //if (General.GetNullableGuid(ViewState["WORKORDERID"].ToString()) != null)
                //{
                //    PhoenixInspectionLongTermAction.LongTermActionAcceptanceUpdate(
                //        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //      , new Guid(ViewState["WORKORDERID"].ToString())
                //      , null);
                //    BindData();
                //}USER
            }
            else if (e.CommandName.ToUpper().Equals("CANCEL"))
            {
                BindPageURL(nCurrentRow);
                //if (General.GetNullableGuid(ViewState["WORKORDERID"].ToString()) != null)
                //{
                //    PhoenixInspectionLongTermAction.LongTermActionAcceptanceUpdate(
                //        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //      , new Guid(ViewState["WORKORDERID"].ToString())
                //      , 1);
                //    ucStatus.Text = "";
                //    BindData();
                //}
            }
            if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                BindPageURL(nCurrentRow);
                SetRowSelection();
            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLongTermActionWorkOrder_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvLongTermActionWorkOrder_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvLongTermActionWorkOrder_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

            ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.QUALITY + "&type=OFFICETASKWO&cmdname=WOUPLOAD" + "'); return true;");
            }
            Label lblActionTaken = (Label)e.Row.FindControl("lblActionTaken");
            UserControlToolTip ucToolTipActionTaken = (UserControlToolTip)e.Row.FindControl("ucToolTipActionTaken");
            lblActionTaken.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipActionTaken.ToolTip + "', 'visible');");
            lblActionTaken.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipActionTaken.ToolTip + "', 'hidden');");

            Label lblDescription = (Label)e.Row.FindControl("lblDescription");
            UserControlToolTip ucToolTipDescription = (UserControlToolTip)e.Row.FindControl("ucToolTipDescription");
            lblDescription.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipDescription.ToolTip + "', 'visible');");
            lblDescription.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipDescription.ToolTip + "', 'hidden');");



        }
    }

    protected void gvLongTermActionWorkOrder_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvLongTermActionWorkOrder.SelectedIndex = se.NewSelectedIndex;
        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {

            Label lblWorOrderId = ((Label)gvLongTermActionWorkOrder.Rows[rowindex].FindControl("lblWorOrderId"));
            Label lblDTKey = ((Label)gvLongTermActionWorkOrder.Rows[rowindex].FindControl("lblDTKey"));
            if (lblWorOrderId != null)
                ViewState["WORKORDERID"] = lblWorOrderId.Text;
            if (lblDTKey != null)
                ViewState["DTKEY"] = lblDTKey.Text;
            ifMoreInfo.Attributes["src"] = "../Inspection/InspectionLongTermActionWorkOrderGeneral.aspx?WORKORDERID=" + ViewState["WORKORDERID"] + "&DTKEY=" + ViewState["DTKEY"];
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
            if (Session["NewTask"] != null && Session["NewTask"].ToString() == "Y")
            {
                ViewState["WORKORDERID"] = null;
                Session["NewTask"] = "N";
            }
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
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
        gvLongTermActionWorkOrder.SelectedIndex = -1;
        gvLongTermActionWorkOrder.EditIndex = -1;
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

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }   
}
