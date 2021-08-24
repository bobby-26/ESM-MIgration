using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class Inspection_InspectionProsperEmployeewiseList : PhoenixBasePage
{

    string empid;

    string strPreviousRowID = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbar = new PhoenixToolbar();

            string id = filterprosper.CurrentSelectedProsperEmployee;
            toolbar.AddFontAwesomeButton("../Inspection/InspectionProsperEmployeewiseList.aspx?empid=" + id + "&cycleid=" + ddlcycle.SelectedValue, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");

            ProsperMenu.MenuList = toolbar.Show();

            PhoenixToolbar toolbarreport = new PhoenixToolbar();

            string reportid = filterprosper.CurrentSelectedProsperEmployee;
            toolbarreport.AddImageButton("../Inspection/InspectionProsperEmployeewiseList.aspx?empid=" + id + "&cycleid=" + ddlcycle.SelectedValue, "Export to Excel", "icon_xls.png", "Excel");

            ProsperMenu.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);

            MenuProgress.AccessRights = this.ViewState;
            MenuProgress.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                empid = Request.QueryString["empid"];
                bindcyclelist();
                BindTenureCycleList();
                if (Request.QueryString["cycleid"] != null && Request.QueryString["cycleid"] != string.Empty)
                    ddlcycle.SelectedValue = Request.QueryString["cycleid"];
                else
                    ddlcycle.SelectedIndex = 1;
            }
            BindData();
            BindDataScore();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SetSelectedTab(string currenttab)
    {
        if (currenttab == "BACK")
        {
            ViewState["URL"] = "../Inspection/InspectionProsperReports.aspx";
            Response.Redirect(ViewState["URL"].ToString(), false);
        }
    }

    public void bindcyclelist()
    {
        DataTable dt = PhoenixProsper.ProsperCycleList(General.GetNullableInteger(Request.QueryString["empid"]));
        if (dt.Rows.Count > 0)
        {
            ddlcycle.DataSource = dt;
            ddlcycle.DataBind();
        }
        else
        {
        }
    }

    public void BindTenureCycleList()
    {
        DataTable dt = PhoenixProsper.ProsperTenureCycleList(General.GetNullableInteger(ddlcycle.SelectedValue.ToString()));
        if (dt.Rows.Count > 0)
        {
            ddltenurecycle.DataSource = dt;
            ddltenurecycle.DataBind();
            BindDataScore();
        }
        else
        {
            BindDataScore();
        }
    }
    public void BindDataScore()
    {
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {
            DataSet ds = PhoenixProsper.ProsperEmployeescoresearch(
                            General.GetNullableInteger(Request.QueryString["empid"])
                            , General.GetNullableInteger(ddlcycle.SelectedValue)
                            , General.GetNullableGuid(ddltenurecycle.SelectedValue.ToString()));

            gvEmployeeProsper.DataSource = ds.Tables[0];
            gdprosperreport.DataSource = ds.Tables[2];

            gdprosperreport.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
            gdprosperreport.MasterTableView.GetColumn("AVGDEFICIENCY").FooterText = "Total";

            if (ds.Tables[2].Rows.Count > 0)
            {
                gdprosperreport.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                gdprosperreport.MasterTableView.GetColumn("SCORE").FooterText = ds.Tables[1].Rows[0]["FLDTOTALSCORE"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {
            DataTable dt = PhoenixProsper.ProsperEmployeeList(General.GetNullableInteger(Request.QueryString["empid"]));

            lblfname.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
            txtfromdate.Text = dt.Rows[0]["FLDCYCLESTARTDATE"].ToString();
            txttodate.Text = dt.Rows[0]["FLDCYCLEENDDATE"].ToString();
            lblrname.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
            lbllastmodified.Text = "Last Modified: " + dt.Rows[0]["FLDMODIFIEDDATE"].ToString();

            DataSet ds = PhoenixProsper.ProsperEmployeeWiseReport(
                             General.GetNullableInteger(Request.QueryString["empid"])
                            , General.GetNullableInteger(ddlcycle.SelectedValue)
                            , General.GetNullableGuid(ddltenurecycle.SelectedValue.ToString())
                            , sortexpression
                            , sortdirection
                            //, gvProsperemplist.CurrentPageIndex + 1
                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                            , gvProsperemplist.PageSize
                            , ref iRowCount
                            , ref iTotalPageCount);
            gvProsperemplist.DataSource = ds.Tables[0];
            gvProsperemplist.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                
                gvProsperemplist.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                if (ds.Tables[1].Rows[0]["SUMFLDPSCDEF"].ToString()!=null)
                {

                
                    gvProsperemplist.MasterTableView.GetColumn("PSCDEF").FooterText = ds.Tables[1].Rows[0]["SUMFLDPSCDEF"].ToString();
                }
                gvProsperemplist.MasterTableView.GetColumn("PSCDET").FooterText = ds.Tables[1].Rows[0]["SUMFLDPSCDET"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("VETTINGDEF").FooterText = ds.Tables[1].Rows[0]["SUMFLDVETTINGDEF"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("VETTINGREJ").FooterText = ds.Tables[1].Rows[0]["SUMFLDVETTINGREJ"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("INCIDENTACATEGORY").FooterText = ds.Tables[1].Rows[0]["SUMFLDINCIDENTACATEGORY"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("INCIDENTBCATEGORY").FooterText = ds.Tables[1].Rows[0]["SUMFLDINCIDENTBCATEGORY"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("INCIDENTCCATEGORY").FooterText = ds.Tables[1].Rows[0]["SUMFLDINCIDENTCCATEGORY"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("HNSACATEGORY").FooterText = ds.Tables[1].Rows[0]["SUMFLDHNSACATEGORY"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("HNSBCATEGORY").FooterText = ds.Tables[1].Rows[0]["SUMFLDHNSBCATEGORY"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("HNSCCATEGORY").FooterText = ds.Tables[1].Rows[0]["SUMFLDHNSCCATEGORY"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("FEEDBACKPOSITIVE").FooterText = ds.Tables[1].Rows[0]["SUMFLDFEEDBACKPOSITIVE"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("FEEDBACKNEGATIVE").FooterText = ds.Tables[1].Rows[0]["SUMFLDFEEDBACKNEGATIVE"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("FEEDBACKWARNING").FooterText = ds.Tables[1].Rows[0]["SUMFLDFEEDBACKWARNING"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("TPIDEF").FooterText = ds.Tables[1].Rows[0]["SUMFLDTPIDEF"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("TPIDET").FooterText = ds.Tables[1].Rows[0]["SUMFLDTPIDET"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("EXTDEF").FooterText = ds.Tables[1].Rows[0]["SUMFLDEXTDEF"].ToString();
                gvProsperemplist.MasterTableView.GetColumn("EXTDET").FooterText = ds.Tables[1].Rows[0]["SUMFLDEXTDET"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void QualityProgress_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("BACK"))
            {
                SetSelectedTab("BACK");
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    public void ShowExcelsummary()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDDATE", "FLDINSPECTIONNAME", "FLDVESSELNAME", "FLDTYPEDESCRIPTION", "FLDPSCDEF", "FLDPSCDET", "FLDVETTINGDEF", "FLDVETTINGREJ", "FLDINCIDENTACATEGORY", "FLDINCIDENTBCATEGORY", "FLDINCIDENTCCATEGORY", "FLDHNSACATEGORY", "FLDHNSBCATEGORY", "FLDHNSCCATEGORY", "FLDFEEDBACKPOSITIVE", "FLDFEEDBACKNEGATIVE", "FLDFEEDBACKWARNING", "FLDTPIDEF", "FLDTPIDET", "FLDEXTDEF", "FLDEXTDET" };
            string[] alCaptions = { "Date", "Name", "Vessel", "Vessel Type", "PSC DEF", "PSC DET", "VETTING DEF", "VETTING REJ", "INCIDENT A CATEGORY", "INCIDENT B CATEGORY", "INCIDENT C CATEGORY", "HNS A CATEGORY", "HNS B CATEGORY", "HNS C CATEGORY", "FB POS", "FB NEG", "FB WRN", "TPI DEF", "TPI REJ", "EXT DEF", "EXT REJ" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = new DataSet();

            DataTable dt = PhoenixProsper.ProsperEmployeeList(General.GetNullableInteger(Request.QueryString["empid"]));

            lblfname.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
            txtfromdate.Text = dt.Rows[0]["FLDCYCLESTARTDATE"].ToString();
            txttodate.Text = dt.Rows[0]["FLDCYCLEENDDATE"].ToString();
            lblrname.Text = dt.Rows[0]["FLDRANKNAME"].ToString();

            ds = PhoenixProsper.ProsperEmployeeWiseReport(
                             General.GetNullableInteger(Request.QueryString["empid"])
                            , General.GetNullableInteger(ddlcycle.SelectedValue)
                            , General.GetNullableGuid(ddltenurecycle.SelectedValue.ToString())
                            , sortexpression
                            , sortdirection
                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                            , gvProsperemplist.PageSize
                            , ref iRowCount
                            , ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=" + dt.Rows[0]["FLDFIRSTNAME"].ToString() + txtfromdate.Text + "_" + txttodate.Text + "ProsperReport.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td colspan=6><h4>PROSPER Score for " + lblfname.Text + " " + txtfromdate.Text + "-" + txttodate.Text + "</h4></td>");
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
            string[] sum = { "SUMFLDPSCDEF", "SUMFLDPSCDET", "SUMFLDVETTINGDEF", "SUMFLDVETTINGREJ", "SUMFLDINCIDENTACATEGORY", "SUMFLDINCIDENTBCATEGORY", "SUMFLDINCIDENTCCATEGORY", "SUMFLDHNSACATEGORY", "SUMFLDHNSBCATEGORY", "SUMFLDHNSCCATEGORY", "SUMFLDFEEDBACKPOSITIVE", "SUMFLDFEEDBACKNEGATIVE", "SUMFLDFEEDBACKWARNING", "SUMFLDTPIDEF", "SUMFLDTPIDET", "SUMFLDEXTDEF", "SUMFLDEXTDET" };
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                Response.Write("<tr>");
                Response.Write("<td colspan=4>Total</td>");
                for (int i = 0; i < sum.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[sum[i]]);
                    Response.Write("</td>");

                }
                Response.Write("</tr>");
            }

            Response.Write("</TABLE>");


            //Score list
            string[] alColumns1 = { "FLDCATEGORYNAME", "FLDREJDET", "FLDNOOFINSPECTION", "FLDCOUNT", "FLDAVERAGE", "FLDCALCULATED" };
            string[] alCaptions1 = { "", "*Detention/Rejection/High Risk", "No. of Inspection", "Total No. of Deficiency", "Average Deficiency", "Score" };

            DataSet ds1 = PhoenixProsper.ProsperEmployeescoresearch(
                           General.GetNullableInteger(Request.QueryString["empid"])
                           , General.GetNullableInteger(ddlcycle.SelectedValue)
                           , General.GetNullableGuid(ddltenurecycle.SelectedValue.ToString()));
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions1.Length; i++)
            {
                Response.Write("<td width='20%'>");
                Response.Write("<b>" + alCaptions1[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");


            foreach (DataRow dr in ds1.Tables[2].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns1.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns1[i]]);
                    Response.Write("</td>");

                }
                Response.Write("</tr>");
            }
            string[] sum1 = { "FLDTOTALSCORE" };
            foreach (DataRow dr in ds1.Tables[1].Rows)
            {
                Response.Write("<tr>");
                Response.Write("<td colspan=5>Total</td>");
                for (int i = 0; i < sum1.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[sum1[i]]);
                    Response.Write("</td>");

                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Prosper_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelsummary();
            }
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["EDIT"] = "1";
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProsperemplist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nCurrentRow = e.Item.ItemIndex;

                if (e.CommandName.ToUpper().Equals("GETEMPLOYEE"))
                {
                    RadLabel lblemployeeid = ((RadLabel)e.Item.FindControl("lblEmployeeid"));
                    LinkButton lb = (LinkButton)e.Item.FindControl("lnkEmployeeid");
                    lb.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../Crew/CrewPersonalGeneral.aspx?empid=" + lblemployeeid.Text + "'); return false;");
                }

                if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    int cycleid = int.Parse(e.Item.ToString());

                    PhoenixProsper.ProsperEmployeeScoreDelete(cycleid);
                    BindData();
                }
                else if (e.CommandName.ToUpper().Equals("UPDATE"))
                {

                    int cycleid = int.Parse(e.Item.ToString());
                    string fromdate = ((UserControlDate)e.Item.FindControl("txtfromdateedit")).Text;
                    string todate = ((UserControlDate)e.Item.FindControl("txttodateedit")).Text;

                    PhoenixProsper.ProsperEmployeeCycleUpdate(
                         cycleid
                        , General.GetNullableDateTime(fromdate)
                        , General.GetNullableDateTime(todate));

                    BindData();
                }
                BindData();
                gvEmployeeProsper.Rebind();
                //gvProsperemplist.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            Filter.CurrentIncidentID = ((RadLabel)gvProsperemplist.Items[rowindex].FindControl("lblsource")).Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void PrepareGridViewForExport(Control gridView)
    {
        for (int i = 0; i < gridView.Controls.Count; i++)
        {
            //Get the control
            Control currentControl = gridView.Controls[i];
            if (currentControl is LinkButton)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as LinkButton).Text));
            }
            else if (currentControl is ImageButton)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as ImageButton).AlternateText));
            }
            else if (currentControl is HyperLink)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as HyperLink).Text));
            }
            else if (currentControl is DropDownList)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as DropDownList).SelectedItem.Text));
            }
            else if (currentControl is CheckBox)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as CheckBox).Checked ? "True" : "False"));
            }
            if (currentControl.HasControls())
            {
                // if there is any child controls, call this method to prepare for export
                PrepareGridViewForExport(currentControl);
            }
        }
    }
    protected void OnDataBound(object sender, EventArgs e)
    {
        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        TableHeaderCell cell = new TableHeaderCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.Text = "";
        cell.ColumnSpan = 4;
        row.Controls.Add(cell);

        cell = new TableHeaderCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.ColumnSpan = 2;
        cell.Text = "PSC";
        row.Controls.Add(cell);

        cell = new TableHeaderCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.ColumnSpan = 2;
        cell.Text = "Vetting";
        row.Controls.Add(cell);

        cell = new TableHeaderCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.ColumnSpan = 3;
        cell.Text = "Incident";
        row.Controls.Add(cell);

        cell = new TableHeaderCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.ColumnSpan = 3;
        cell.Text = "Health & Safety";
        row.Controls.Add(cell);

        cell = new TableHeaderCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.ColumnSpan = 3;
        cell.Text = "Feedback";
        row.Controls.Add(cell);

        cell = new TableHeaderCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.ColumnSpan = 2;
        cell.Text = "Third Party";
        row.Controls.Add(cell);

        cell = new TableHeaderCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.ColumnSpan = 2;
        cell.Text = "External <br/> Nav & Env";
        row.Controls.Add(cell);
        //  gvProsperemplist.HeaderRow.Parent.Controls.AddAt(0, row);

    }
    protected void gvProspercomplist_RowCreated(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            RadLabel lblEVENT = ((RadLabel)e.Row.FindControl("lblEVENT"));
            LinkButton addButton = (LinkButton)e.Row.FindControl("lbldate");

            if (lblEVENT.Text.ToUpper() == "FEEDBACK")
                addButton.Enabled = false;
        }
    }
    protected void gvProspercomplist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
            {
                LinkButton lbldate = ((LinkButton)e.Row.FindControl("lbldate"));
                if (lbldate != null)
                {
                    RadLabel lblsource = ((RadLabel)e.Row.FindControl("lblsource"));
                    RadLabel lblEVENT = ((RadLabel)e.Row.FindControl("lblEVENT"));

                    if (lblEVENT.Text.ToUpper() == "AUDIT")
                    {
                        lbldate.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../Inspection/InspectionAuditRecordGeneral.aspx?AUDITSCHEDULEID=" + lblsource.Text + "'); return false;");

                    }
                    else if (lblEVENT.Text.ToUpper() == "INSPECTION")
                    {
                        lbldate.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../Inspection/InspectionVettingReport.aspx?SCHEDULEID=" + lblsource.Text + "'); return false;");
                    }
                    else if (lblEVENT.Text.ToUpper() == "INCIDENT")
                    {
                        Filter.CurrentIncidentID = ((RadLabel)e.Row.FindControl("lblsource")).Text;
                        Filter.CurrentIncidentTab = "INCIDENTDETAILS";
                        lbldate.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../Inspection/InspectionIncidentList.aspx?callfrom=irecord'); return false;");
                    }
                    else if (lblEVENT.Text.ToUpper() == "FEEDBACK")
                    {
                        // lbldate.Enabled = false;
                        RadLabel lblvesselid = ((RadLabel)e.Row.FindControl("lblvesselid"));

                        lbldate.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../Inspection/InspectionSupdtEventFeedbackBannerEdit.aspx?fid=" + lblsource.Text + "&eid=" + Request.QueryString["empid"].ToString() + "&vid=" + lblvesselid.Text + "'); return false;");
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
    protected void gvEmployeeProsper_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#bbddff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");

            RadLabel lblCategory = (RadLabel)e.Row.FindControl("lblCategory");
            RadLabel lblMeasure = (RadLabel)e.Row.FindControl("lblMeasure");
        }
    }

    protected void gdprosperreport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#bbddff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");

            RadLabel lblCategory = (RadLabel)e.Row.FindControl("lblCategory");
            RadLabel lblMeasure = (RadLabel)e.Row.FindControl("lblMeasure");
        }
    }

    // int cnt = 1;
    protected void gvEmployeeProsper_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }


    protected void ddlcycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["EDIT"] = "1";
        BindTenureCycleList();
        BindData();
    }

    protected void ddltenurecycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["EDIT"] = "1";
        gvProsperemplist.Rebind();
    }

    protected void gvProsperemplist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
