using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;

public partial class InspectionRecordList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvInspectionRecordList.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$lnkDoubleClick");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$lnkDoubleClickEdit");
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            ucConfirmComplete.Visible = false;
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Inspection/InspectionRecordList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvInspectionRecordList')", "Print Grid", "icon_print.png", "PRINT");
            MenuInspectionRecordList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                VesselConfiguration();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                
                if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() == "log")
                {
                    Filter.CurrentInspectionScheduleId = null;
                    Filter.CurrentInspectionMenu = "log";
                    lblNote.Text = "Note: <br>1. Click on the 'Inspection Name' to navigate to record screen. ";
                }
                else if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() == "record")
                {
                    Filter.CurrentInspectionScheduleId = null;
                    Filter.CurrentInspectionMenu = null;
                }
            }
            if (Filter.CurrentInspectionMenu == null)
                ucTitle.Text = "Inspection List";
            else
                ucTitle.Text = "Inspection Log";
            Filter.CurrentInspectionRecordResponseFilter = null;
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int vesselid = -1;
        string status = null;

        string[] alColumns = { "FLDREFERENCENUMBER", "FLDINSPECTIONNAME", "FLDVESSELSHORTCODE", "FLDRANGEFROMDATE", "FLDNAMEOFINSPECTOR", "FLDSEAPORTNAME", "FLDSTATUSNAME" };
        string[] alCaptions = { "Reference Number", "Vetting Name", "Vessel Code", "Planned Date", "Name of Inspector", "Port of Vetting", "Status" };

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

        if (Filter.CurrentInspectionMenu != null && Filter.CurrentInspectionMenu.ToString() == "log")
            status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 146, "CLD");
        else if (Filter.CurrentInspectionMenu == null)
        {
            status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 146, "ASG"); //planned
            status = status + "," + PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 146, "CMP"); //completed
        }

        DataSet ds = PhoenixInspectionSchedule.InspectionScheduleSearch(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                ,null,null,null,vesselid,null,status,null,null
                , sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);
        
        Response.AddHeader("Content-Disposition", "attachment; filename=InspectionScheduleList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Inspection List</h3></td>");
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

    protected void InspectionRecordList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int vesselid = -1;
        string status = null;

        string[] alColumns = { "FLDREFERENCENUMBER", "FLDINSPECTIONNAME", "FLDVESSELSHORTCODE", "FLDRANGEFROMDATE", "FLDNAMEOFINSPECTOR", "FLDSEAPORTNAME", "FLDSTATUSNAME" };
        string[] alCaptions = { "Reference Number", "Vetting Name", "Vessel Code", "Planned Date", "Name of Inspector", "Port of Vetting", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        
        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (Filter.CurrentInspectionMenu != null && Filter.CurrentInspectionMenu.ToString() == "log")
            status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 146, "CLD");
        else if (Filter.CurrentInspectionMenu == null)
        {
            status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 146, "ASG");
            status = status + "," + PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 146, "CMP");
        }

        DataSet ds = PhoenixInspectionSchedule.InspectionScheduleSearch(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , null, null, null, vesselid, null, status, null, null
                , sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        General.SetPrintOptions("gvInspectionRecordList", "Inspection List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvInspectionRecordList.DataSource = ds;
            gvInspectionRecordList.DataBind();

            if (Filter.CurrentInspectionScheduleId == null)
            {
                Filter.CurrentInspectionScheduleId = ds.Tables[0].Rows[0]["FLDINSPECTIONSCHEDULEID"].ToString();
                gvInspectionRecordList.SelectedIndex = 0;
            }
            SetRowSelection();
        }
        else
        {
            ds.Tables[0].Rows.Clear();
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvInspectionRecordList);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void SetRowSelection()
    {
        string a = Filter.CurrentInspectionScheduleId.ToString();
        gvInspectionRecordList.SelectedIndex = -1;
        for (int i = 0; i < gvInspectionRecordList.Rows.Count; i++)
        {
            if (gvInspectionRecordList.DataKeys[i].Value.ToString().Equals(Filter.CurrentInspectionScheduleId.ToString()))
            {
                gvInspectionRecordList.SelectedIndex = i;
            }
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    }

    protected void gvInspectionRecordList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("NAVIGATE"))
            {
                BindPageURL(nCurrentRow);
                Response.Redirect("../Inspection/InspectionRecordAndResponse.aspx?", false);
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string inspectionscheduleid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblInspectionScheduleId")).Text;
                if (inspectionscheduleid != null)
                {
                    DeleteInspectionSchedule(new Guid(inspectionscheduleid));
                    Filter.CurrentInspectionScheduleId = null;
                }
            }
            if (e.CommandName.ToUpper().Equals("CLOSEINSPECTION"))
            {
                BindPageURL(nCurrentRow);
                SetRowSelection();
                if (Filter.CurrentInspectionScheduleId != null)
                {
                    PhoenixInspectionSchedule.InspectionCloseUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                new Guid(Filter.CurrentInspectionScheduleId));
                    BindData();
                    SetPageNavigator();
                    ucStatus.Text = "Inspection is closed.";
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInspectionRecordList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = sender as GridView;
        int nCurrentRow = e.RowIndex;

        Label lblInspectionScheduleId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblInspectionScheduleId");
        if (lblInspectionScheduleId != null)
            ViewState["SCHEDULEID"] = lblInspectionScheduleId.Text;
        ucConfirmComplete.Visible = true;
        ucConfirmComplete.Text = "Are you sure you want to Complete the 'Inspection'?";  
    }

    protected void gvInspectionRecordList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.NewSelectedIndex;
        BindPageURL(nCurrentRow);
    }

    private void DeleteInspectionSchedule(Guid inspectionscheduleid)
    {
        PhoenixInspectionSchedule.DeleteInspectionSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, inspectionscheduleid);        
    }

    protected void gvInspectionRecordList_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvInspectionRecordList_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }

    protected void gvInspectionRecordList_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvInspectionRecordList.SelectedIndex = -1;
        gvInspectionRecordList.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvInspectionRecordList_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            DataRowView dr = (DataRowView)e.Row.DataItem;
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);            

            //LinkButton lbtn = (LinkButton)e.Row.FindControl("lnkInspection");
            //lbtn.Attributes.Add("onclick", "Openpopup('AddAddress', '', 'InspectionAuditRecordAndResponse.aspx'); return false;");
        
            db = (ImageButton)e.Row.FindControl("cmdEdit");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

            Image imgFlag = e.Row.FindControl("imgFlag") as Image;

            if (imgFlag != null && dr["FLDOVERDUEYN"].ToString().Equals("2"))
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + "red-symbol.png";
                imgFlag.ToolTip = "Over Due";
            }
            else if (imgFlag != null && dr["FLDOVERDUEYN"].ToString().Equals("1"))
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + "yellow-symbol.png";
                imgFlag.ToolTip = "Due";
            }
            else if (imgFlag != null && dr["FLDOVERDUEYN"].ToString().Equals("0"))
                imgFlag.Visible = false; 

            ImageButton ib = (ImageButton)e.Row.FindControl("cmdComplete");
            if (ib != null) ib.Visible = SessionUtil.CanAccess(this.ViewState, ib.CommandName);
            if (ib != null)
            {
                if (Filter.CurrentInspectionMenu != null && Filter.CurrentInspectionMenu.ToString().Equals("log"))                
                    ib.Visible = false;                 
                else
                    ib.Visible = true;
            }

            ImageButton cl = (ImageButton)e.Row.FindControl("cmdClose");
            if (cl != null) cl.Visible = SessionUtil.CanAccess(this.ViewState, cl.CommandName);
            if (cl != null)
            {
                if (Filter.CurrentInspectionMenu != null && Filter.CurrentInspectionMenu.ToString().Equals("log"))
                    cl.Visible = false;
                else
                    cl.Visible = true;
            }

            if (dr["FLDSTATUS"] != null && dr["FLDSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 146, "CMP"))
            {
                if (ib != null) ib.Visible = false;                
                if (cl != null) cl.Visible = true;                
            }
            if (dr["FLDSTATUS"] != null && dr["FLDSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 146, "ASG"))
            {
                if (ib != null) ib.Visible = true;                
                if (cl != null) cl.Visible = false;               
            }
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            Label lblInspectionScheduleId = (Label)gvInspectionRecordList.Rows[rowindex].FindControl("lblInspectionScheduleId");
            if (lblInspectionScheduleId != null)
            {
                Filter.CurrentInspectionScheduleId = lblInspectionScheduleId.Text;                
            }
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
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvInspectionRecordList.SelectedIndex = -1;
        gvInspectionRecordList.EditIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvInspectionRecordList.SelectedIndex = -1;
        gvInspectionRecordList.EditIndex = -1;
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
        gvInspectionRecordList.SelectedIndex = -1;
        gvInspectionRecordList.EditIndex = -1;
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
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void btnComplete_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                if (ViewState["SCHEDULEID"] != null)
                {
                    PhoenixInspectionSchedule.UpdateInspectionScheduleStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["SCHEDULEID"].ToString()));
                    BindData();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
