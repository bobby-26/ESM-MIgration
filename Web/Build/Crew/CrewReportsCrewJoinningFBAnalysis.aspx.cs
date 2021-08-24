using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Web;
using Telerik.Web.UI;

public partial class CrewReportsCrewJoinningFBAnalysis : PhoenixBasePage
{
    DataSet dsGrid;
    protected override void Render(HtmlTextWriter writer)
    {
        NameValueCollection nvc = Filter.CurrentFeedBackFilter;
        if (nvc != null)
        {
            ucManager.SelectedAddress = nvc.Get("ucManager");
            ucFromDate.Text = nvc.Get("ucFromDate");
            ucToDate.Text = nvc.Get("ucToDate");
            if (nvc.Get("ucVessel").Equals("Dummy"))
            {
                ucVessel.SelectedVessel = "";
            }
            else
            {
                ucVessel.SelectedVessel = nvc.Get("ucVessel");
            }

            if (nvc.Get("ucVesselType").Equals("Dummy"))
            {
                ucVesselType.SelectedVesseltype = "";
            }
            else
            {
                ucVesselType.SelectedVesseltype = nvc.Get("ucVesselType");
            }

            if (nvc.Get("lstPool").Equals("Dummy"))
            {
                lstPool.SelectedPool = "";
            }
            else
            {
                lstPool.SelectedPool = nvc.Get("lstPool");
            }

            if (nvc.Get("ucRank").Equals("Dummy"))
            {
                ucRank.selectedlist = "";
            }
            else
            {
                ucRank.selectedlist = nvc.Get("ucRank");
            }

            if (nvc.Get("ucZone").Equals("Dummy"))
            {
                ucZone.selectedlist = "";
            }
            else
            {
                ucZone.selectedlist = nvc.Get("ucZone");
            }

        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            PhoenixToolbar toolbarsub = new PhoenixToolbar();

            toolbar.AddButton("Crew Feedback", "CREWFEEDBACK");
            toolbar.AddButton("Feedback Analysis", "ANALYSIS");
            MenuFeedback.MenuList = toolbar.Show();
            MenuFeedback.SelectedMenuIndex = 1;

            toolbarsub.AddButton("Show Report", "SHOWREPORT");
            MenuReport.AccessRights = this.ViewState;
            MenuReport.MenuList = toolbarsub.Show();
            toolbargrid.AddImageButton("../Crew/CrewReportsCrewJoinningFBAnalysis.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageButton("../Crew/CrewReportsCrewJoinningFBAnalysis.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["CURRENTINDEX"] = 1;
                
                NameValueCollection nvc = Filter.CurrentFeedBackFilter;
                if (nvc != null)
                {
                    ucManager.SelectedAddress = nvc.Get("ucManager");
                    ucFromDate.Text = nvc.Get("ucFromDate");
                    ucToDate.Text = nvc.Get("ucToDate");
                    if (nvc.Get("ucVessel").Equals("Dummy"))
                    {
                        ucVessel.SelectedVessel = "";
                    }
                    else
                    {
                        ucVessel.SelectedVessel = nvc.Get("ucVessel");
                    }

                    if (nvc.Get("ucVesselType").Equals("Dummy"))
                    {
                        ucVesselType.SelectedVesseltype = "";
                    }
                    else
                    {
                        ucVesselType.SelectedVesseltype = nvc.Get("ucVesselType");
                    }

                    if (nvc.Get("lstPool").Equals("Dummy"))
                    {
                        lstPool.SelectedPool = "";
                    }
                    else
                    {
                        lstPool.SelectedPool = nvc.Get("lstPool");
                    }

                    if (nvc.Get("ucRank").Equals("Dummy"))
                    {
                        ucRank.selectedlist = "";
                    }
                    else
                    {
                        ucRank.selectedlist = nvc.Get("ucRank");
                    }

                    if (nvc.Get("ucZone").Equals("Dummy"))
                    {
                        ucZone.selectedlist = "";
                    }
                    else
                    {
                        ucZone.selectedlist = nvc.Get("ucZone");
                    }

                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Report_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if ((IsValidFilter()))
                {
                    //NameValueCollection criteria = new NameValueCollection();
                    //criteria.Clear();
                    //criteria.Add("ddlFeedbackStatus", ddlFeedbackStatus.SelectedValue);
                    //criteria.Add("ucManager", ucManager.SelectedAddress);
                    //criteria.Add("ucFromDate", ucFromDate.Text);
                    //criteria.Add("ucToDate", ucToDate.Text);
                    //criteria.Add("ucVessel", ucVessel.SelectedVessel);
                    //criteria.Add("lstPool", lstPool.SelectedPool);
                    //criteria.Add("ucVesselType", ucVesselType.SelectedVesselTypeValue);
                    //criteria.Add("ucRank", ucRank.selectedlist);
                    //criteria.Add("ucZone", ucZone.selectedlist);
                    //Filter.CurrentFeedBackFilter = criteria;
                    //ViewState["PAGENUMBER"] = 1;
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuFeedback_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("CREWFEEDBACK"))
        {
            Response.Redirect("../Crew/CrewReportsCrewJoinningFeedBack.aspx", false);
        }
    }

    private void BindData()
    {
        dsGrid = PhoenixVesselAccountsCrewFeedback.SearchCrewJoinningFeedBackAnalysis(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableString(ucVessel.SelectedVessel)
            , General.GetNullableInteger(ddlFeedbackStatus.SelectedValue)
            , (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
            , (General.GetNullableInteger(ucManager.SelectedAddress))
            , (lstPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(lstPool.SelectedPool)
            , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
            , (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist)
            , General.GetNullableDateTime(ucFromDate.Text)
            , General.GetNullableDateTime(ucToDate.Text));

        if (dsGrid.Tables.Count > 0 && (dsGrid.Tables[0].Rows.Count > 0 && dsGrid.Tables[1].Rows.Count > 0 && dsGrid.Tables[2].Rows.Count > 0))
        {
            DataTable dt = dsGrid.Tables[2];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BoundField answermode = new BoundField();
                answermode.HeaderText = "Answer-Y/N/NA";
                answermode.ControlStyle.BorderColor = System.Drawing.Color.White;
                gvFB.Columns.Add(answermode);

                BoundField desc = new BoundField();
                desc.HeaderText = "Description";
                desc.ControlStyle.BorderColor = System.Drawing.Color.White;
                gvFB.Columns.Add(desc);
            }
            ViewState["ISCOLUMNSBOUND"] = 0;
            gvFB.DataSource = dsGrid;
            gvFB.DataBind();
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            row.Attributes.Add("style", "position:static");
            TableCell cell = new TableCell();
            cell.ColumnSpan = 4;
            row.Cells.Add(cell);
            int k = 0;
            for (int j = 4; j < gvFB.Columns.Count; j++)
            {
                cell = new TableCell();
                cell.ColumnSpan = 2;
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Text = dt.Rows[k]["FLDQUESTIONNAME"].ToString();
                row.Cells.Add(cell);
                j++;
                k++;
            }
            gvFB.Controls[0].Controls.AddAt(0, row);
        }
        else
        {
            DataTable dtHeader = dsGrid.Tables[2];
            ShowNoRecordsFound(dsGrid, gvFB);
        }
    }
    protected void gvFB_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView gv = (GridView)sender;
        DataSet ds = (DataSet)gv.DataSource;
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string empid = drv["FLDEMPLOYEEID"].ToString();
            DataTable header = ds.Tables[2];
            DataTable data = ds.Tables[1];
            int icellCount = 4;
            if (header.Rows.Count > 0 && data.Rows.Count > 0)
            {
                for (int i = 0; i < header.Rows.Count; i++)
                {
                    string budgetcode = gv.Columns[i].HeaderText;
                    string logtype = header.Rows[i]["FLDQUESTIONID"].ToString();
                    DataRow[] dr = data.Select("FLDEMPLOYEEID = " + empid.Trim() + " AND FLDQUESTIONID = '" + logtype.Trim() + "'");
                    e.Row.Cells[icellCount].Text = (dr.Length > 0 ? dr[0]["FLDANSWER"].ToString() : "");
                    icellCount++;
                    e.Row.Cells[icellCount].Text = (dr.Length > 0 ? dr[0]["FLDANSWERDESCRIPTION"].ToString() : "");
                    icellCount++;
                }
            }
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }
   
    private void ShowNoRecordsFound(DataSet ds, GridView gv)
    {
        DataTable dt = ds.Tables[0];
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = ds;
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
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=CrewFedbackAnalysis.xls");
                Response.Charset = "";
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                Response.Write("<br>");
                Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
                Response.Write("<tr>");
                Response.Write("<td align='left'><img src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/esmlogo4_small.png" + "' /></td>");
                Response.Write("<td align='center' colspan='11'>");
                Response.Write("<b>Crew Fedback Analysis Report</b>");
                Response.Write("</td>");
                Response.Write("</TABLE>");
                stringwriter.Write("<table><tr><td colspan=\"" + gvFB.Columns.Count + "\"></td></tr></table>");
                HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
                gvFB.RenderControl(htmlwriter);
                Response.Write(stringwriter.ToString());
                Response.End();
            }
            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                NameValueCollection criteria = Filter.CurrentFeedBackFilter;
                if (criteria != null)
                {
                    criteria.Set("ddlFeedbackStatus", "");
                    criteria.Set("ucManager", "");
                    criteria.Set("ucFromDate", "");
                    criteria.Set("ucToDate", "");
                    criteria.Set("ucVessel", "");
                    criteria.Set("lstPool", "");
                    criteria.Set("ucVesselType", "");
                    criteria.Set("ucRank", "");
                    criteria.Set("ucZone", "");
                }
                ucFromDate.Text = null;
                ucToDate.Text = null;
                ucZone.SelectedZoneValue = "";
                lstPool.SelectedPool = "";
                ucVesselType.SelectedVesselTypeValue = "";
                ucVessel.SelectedVessel = "";
                ucRank.SelectedRankValue = "";
                ddlFeedbackStatus.SelectedValue = "DUMMY";
                ucManager.SelectedAddress = "";
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public bool IsValidFilter()
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ucVessel.SelectedVessel) == null)
        {
            ucError.ErrorMessage = "Vessel required";
        }
        return (!ucError.IsError);

    }
}
