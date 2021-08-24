using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System.Collections;
using System.Collections.Specialized;

public partial class InspectionLongTermActionWorkOrderDetails : PhoenixBasePage
{
    PhoenixToolbar toolbar = new PhoenixToolbar();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                Session["NewList"] = "N";
                if (Filter.CurrentSelectedWOTasks != null)
                    Filter.CurrentSelectedWOTasks = null;
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                ViewState["WORKORDERID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (Request.QueryString["WORKORDERID"] != null && Request.QueryString["WORKORDERID"].ToString() != "")
                {
                    ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
                    BindActionDetails();
                }
                toolbar.AddButton("Work Order", "WORKORDER");
                toolbar.AddButton("Attachment","ATTACHMENT");
                MenuInspectionDetails.AccessRights = this.ViewState;
                MenuInspectionDetails.MenuList = toolbar.Show();
                MenuInspectionDetails.SelectedMenuIndex = 0;

                PhoenixToolbar toolbar1 = new PhoenixToolbar();
                toolbar1.AddImageButton("../Inspection/InspectionLongTermActionWorkOrderDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar1.AddImageLink("javascript:CallPrint('gvLongTermAction')", "Print Grid", "icon_print.png", "PRINT");

                MenuLongTermAction.AccessRights = this.ViewState;
                MenuLongTermAction.MenuList = toolbar1.Show();
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuLongTermAction_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindActionDetails()
    {
        DataSet ds = new DataSet();

        ds = PhoenixInspectionLongTermAction.WorkOrderEdit(new Guid(ViewState["WORKORDERID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtWorkOrder.Text = ds.Tables[0].Rows[0]["FLDWONUMBER"].ToString();
            txtGeneratedBy.Text = ds.Tables[0].Rows[0]["FLDWOGENERATEDBYNAME"].ToString();
            txtDescription.Text = ds.Tables[0].Rows[0]["FLDWODESCRIPTION"].ToString();
            txtActionTaken.Text = ds.Tables[0].Rows[0]["FLDACTIONTAKEN"].ToString();
            txtCloseddDate.Text = ds.Tables[0].Rows[0]["FLDCOMPLETEDDATE"].ToString();
            lblDTKey.Text  = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
            ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
        
        }
    }

    protected void MenuInspectionDetails_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("ATTACHMENT"))
            {

                if (ViewState["DTKEY"] != null)
                    Response.Redirect("../Inspection/InspectionLongTermActionWorkOrderAttachment.aspx?DTKEY=" + lblDTKey.Text + "&WORKORDERID=" + ViewState["WORKORDERID"] + "&MOD=" + PhoenixModule.QUALITY + "&type=OFFICETASKWO"
                                                  + "&U=0", true);
                    //Response.Redirect("../Inspection/InspectionLongTermActionWorkOrderAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&WORKORDERID=" + ViewState["WORKORDERID"] + "&MOD=" + PhoenixModule.QUALITY + "&type=OFFICETASKWO"
                    //                                 + "&U=0", true);
                  
            }
            if (dce.CommandName.ToUpper().Equals("WORKORDER"))
            {
                if (ViewState["DTKEY"] != null)
                    Response.Redirect("../Inspection/InspectionLongTermActionWorkOrderDetails.aspx?WORKORDERID=" + ViewState["WORKORDERID"] + "&DTKEY=" + ViewState["DTKEY"],true);
            
            }
           
           
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDPREVENTIVEACTION", "FLDTASKCATEGORYNAME", "FLDTASKSUBCATEGORYNAME", "FLDCREATEDFROMNAME", "FLDDEPARTMENTNAME", "FLDACCEPTEDBYNAME", "FLDTARGETDATE", "FLDSTATUSNAME" };
        string[] alCaptions = { "Vessel", "Task", "Category", "Sub Category", "Source", "Assigned Department", "Accepted by", "Target Date", "Task Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string vesselid = "";
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = Filter.CurrentVesselConfiguration.ToString();

        DataSet ds = PhoenixInspectionLongTermAction.LongTermActionSearch(
                                                                  null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , General.GetNullableGuid(ViewState["WORKORDERID"].ToString()) == null ? Guid.NewGuid() : General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , null, null, null, null, null, null, null, null, null, null, null, null
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        General.SetPrintOptions("gvLongTermAction", "Long Term Action", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvLongTermAction.DataSource = ds;
            gvLongTermAction.DataBind();

            if (ViewState["TASKID"] == null)
            {
                ViewState["TASKID"] = ds.Tables[0].Rows[0]["FLDINSPECTIONPREVENTIVEACTIONID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();

            }
           
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvLongTermAction);
         
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDPREVENTIVEACTION", "FLDTASKCATEGORYNAME", "FLDTASKSUBCATEGORYNAME", "FLDCREATEDFROMNAME", "FLDDEPARTMENTNAME", "FLDACCEPTEDBYNAME", "FLDTARGETDATE", "FLDSTATUSNAME" };
        string[] alCaptions = { "Vessel", "Task", "Category", "Sub Category", "Source", "Assigned Department", "Accepted by", "Target Date", "Task Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //string BudgetBillingSearch = (txtSearchBudgetBillingList.Text == null) ? "" : txtSearchBudgetBillingList.Text;

        DataSet ds = PhoenixInspectionLongTermAction.LongTermActionSearch(
                                                                  null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , General.GetNullableGuid(ViewState["WORKORDERID"].ToString()) == null ? Guid.NewGuid() : General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , null, null, null, null, null, null, null, null, null, null, null, null
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=BulkPOList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Long Term Action</h3></td>");
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
        gvLongTermAction.SelectedIndex = -1;

        for (int i = 0; i < gvLongTermAction.Rows.Count; i++)
        {
            if (gvLongTermAction.DataKeys[i].Value.ToString().Equals(ViewState["TASKID"].ToString()))
            {
                gvLongTermAction.SelectedIndex = i;
            }
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvLongTermAction_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvLongTermAction_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvLongTermAction_RowCommand(object sender, GridViewCommandEventArgs e)
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
          
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLongTermAction_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvLongTermAction_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvLongTermAction_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            DataRowView dr = (DataRowView)e.Row.DataItem;
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");

            if (lblStatus != null && lblStatus.Text == "2")
            {
                if (chkSelect != null)
                    chkSelect.Visible = false;
            }

            Label lblTask = (Label)e.Row.FindControl("lblTask");
            UserControlToolTip ucToolTip = (UserControlToolTip)e.Row.FindControl("ucToolTip");
            lblTask.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTip.ToolTip + "', 'visible');");
            lblTask.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTip.ToolTip + "', 'hidden');");

            Label lblCreatedFrom = (Label)e.Row.FindControl("lblCreatedFrom");
            UserControlToolTip ucToolTipSource = (UserControlToolTip)e.Row.FindControl("ucToolTipSource");
            lblCreatedFrom.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipSource.ToolTip + "', 'visible');");
            lblCreatedFrom.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipSource.ToolTip + "', 'hidden');");      

       }
    }

    protected void gvLongTermAction_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvLongTermAction.SelectedIndex = se.NewSelectedIndex;
        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {

            Label lblLongTermActionId = ((Label)gvLongTermAction.Rows[rowindex].FindControl("lblLongTermActionId"));
            Label lblDTKey = ((Label)gvLongTermAction.Rows[rowindex].FindControl("lblDTKey"));
            if (lblLongTermActionId != null)
                ViewState["TASKID"] = lblLongTermActionId.Text;
            if (lblDTKey != null)
                ViewState["DTKEY"] = lblDTKey.Text;
            // ifMoreInfo.Attributes["src"] = "../Inspection/InspectionLongTermActionGeneral.aspx?TASKID=" + ViewState["TASKID"] + "&DTKEY=" + ViewState["DTKEY"];
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
                ViewState["TASKID"] = null;
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
        gvLongTermAction.SelectedIndex = -1;
        gvLongTermAction.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
        GetSelectedPvs();
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

    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvLongTermAction$ctl01$chkAllRemittance")
        {
            CheckBox chkAll = (CheckBox)gvLongTermAction.HeaderRow.FindControl("chkAllRemittance");
            foreach (GridViewRow row in gvLongTermAction.Rows)
            {
                CheckBox cbSelected = (CheckBox)row.FindControl("chkSelect");
                if (chkAll.Checked == true)
                {
                    cbSelected.Checked = true;
                }
                else
                {
                    cbSelected.Checked = false;
                }
            }
        }
    }

    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedPvs = new ArrayList();
        Guid index = new Guid();
        foreach (GridViewRow gvrow in gvLongTermAction.Rows)
        {
            bool result = false;
            index = new Guid(gvLongTermAction.DataKeys[gvrow.RowIndex].Value.ToString());

            if (((CheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
            }

            // Check in the Session
            if (Filter.CurrentSelectedWOTasks != null)
                SelectedPvs = (ArrayList)Filter.CurrentSelectedWOTasks;
            if (result)
            {
                if (!SelectedPvs.Contains(index))
                    SelectedPvs.Add(index);
            }
            else
                SelectedPvs.Remove(index);
        }
        if (SelectedPvs != null && SelectedPvs.Count > 0)
            Filter.CurrentSelectedWOTasks = SelectedPvs;
    }

    private void GetSelectedPvs()
    {
        if (Filter.CurrentSelectedWOTasks != null)
        {
            ArrayList SelectedPvs = (ArrayList)Filter.CurrentSelectedWOTasks;
            Guid index = new Guid();
            if (SelectedPvs != null && SelectedPvs.Count > 0)
            {
                foreach (GridViewRow row in gvLongTermAction.Rows)
                {
                    CheckBox chk = (CheckBox)(row.Cells[0].FindControl("chkSelect"));
                    index = new Guid(gvLongTermAction.DataKeys[row.RowIndex].Value.ToString());
                    if (SelectedPvs.Contains(index))
                    {
                        CheckBox myCheckBox = (CheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }
    }

    protected void ddlAcceptance_Changed(Object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void ucDepartment_Changed(Object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }
}

