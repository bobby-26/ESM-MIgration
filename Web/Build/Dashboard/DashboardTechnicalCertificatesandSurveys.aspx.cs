using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Web.UI.HtmlControls;

public partial class DashboardTechnicalCertificatesandSurveys : PhoenixBasePage
{
    string vessellist = "";
    string valuelist = "";
    string colorlist = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            ViewState["Edit"] = "0";
            if (!IsPostBack)
            {
                SelectedOption();
                //DisplayMeasureChart();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
				crwid.Visible = PhoenixDashboardOption.TabExists(2);
			}

			PhoenixToolbar toolbargrid = new PhoenixToolbar();
			toolbargrid.AddImageButton("../Dashboard/DashboardTechnicalCertificatesandSurveys.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
			DashboardExport.AccessRights = this.ViewState;
			DashboardExport.MenuList = toolbargrid.Show();

			BindMenu();
            BindMeasure();
            BindMeasureResult();
            SetPageNavigator();
            if (!IsPostBack)
            {
                int row = int.Parse(Filter.CurrentDashboardLastSelection["Row"]);
                int col = int.Parse(Filter.CurrentDashboardLastSelection["Col"]);
                //if (row != 0 || col != 0)
                //{
                //    gvMeasure.SelectedIndex = row;
                //    gvMeasure.Rows[row].Cells[col].BackColor = System.Drawing.Color.LightGray;
                //}
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SelectedOption()
    {
        DataSet ds = PhoenixDashboardOption.DashboardLastSelectedEdit("TECH", "CSY");
        NameValueCollection nvc = new NameValueCollection();
        if (ds.Tables[0].Rows.Count > 0)
        {

            nvc.Add("APP", ds.Tables[0].Rows[0]["FLDSELECTEDMENU"].ToString());
            nvc.Add("Option", ds.Tables[0].Rows[0]["FLDSELECTEDOPTION"].ToString());
            nvc.Add("VesselName", ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString());
            nvc.Add("RankName", ds.Tables[0].Rows[0]["FLDRANKNAME"].ToString());
            nvc.Add("MeasureName", ds.Tables[0].Rows[0]["FLDMEASURENAME"].ToString());
            nvc.Add("VesselId", ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());
            nvc.Add("RankId", ds.Tables[0].Rows[0]["FLDRANKID"].ToString());
            nvc.Add("MeasureId", ds.Tables[0].Rows[0]["FLDMEASUREID"].ToString());
			nvc.Add("MeasureCode", ds.Tables[0].Rows[0]["MEASURECODE"].ToString());
			nvc.Add("Row", ds.Tables[0].Rows[0]["FLDSELECTEDROW"].ToString());
            nvc.Add("Col", ds.Tables[0].Rows[0]["FLDSELECTEDCOLUMN"].ToString());
            nvc.Add("ResultRow", ds.Tables[0].Rows[0]["FLDSELECTEDRESULTROW"].ToString());
            nvc.Add("VesselList", ds.Tables[0].Rows[0]["FLDVESSELLIST"].ToString());
        }
        else
        {
            DataSet d = PhoenixDashboardTechnical.DashboardMeasure("CSY", null);
            nvc.Add("APP", "TECH");
            nvc.Add("Option", "CSY");
            if (d.Tables[1].Rows.Count > 0)
            {
                nvc.Add("VesselId", d.Tables[1].Rows[0]["FLDVESSELID"].ToString());
                nvc.Add("VesselName", d.Tables[1].Rows[0]["FLDVESSELNAME"].ToString());
            }
            else
            {
                nvc.Add("VesselId", "0");
                nvc.Add("VesselName", "Vessel");
            }
			if (d.Tables[0].Rows.Count > 0)
			{
				nvc.Add("MeasureId", d.Tables[0].Rows[0]["FLDMEASUREID"].ToString());
				nvc.Add("MeasureCode", d.Tables[0].Rows[0]["MEASURECODE"].ToString());
				nvc.Add("MeasureName", d.Tables[0].Rows[0]["FLDMEASURENAME"].ToString());
				nvc.Add("Row", "0");
				nvc.Add("Col", "2");
				nvc.Add("ResultRow", "0");
				nvc.Add("VesselList", "");
				nvc.Add("SelectedModuleScreen", "");
			}
			else
			{
				nvc.Add("MeasureId", "");
				nvc.Add("MeasureCode", "");
				nvc.Add("MeasureName", "");
				nvc.Add("Row", "0");
				nvc.Add("Col", "2");
				nvc.Add("ResultRow", "0");
				nvc.Add("VesselList", "");
				nvc.Add("SelectedModuleScreen", "");
			}
		}
        lblName.Text = " [ " + nvc.Get("VesselName").ToString() + " - " + nvc.Get("MeasureName").ToString() + " ] ";
       // lblMeasureTitle.Text = nvc.Get("MeasureName").ToString();
        Filter.CurrentDashboardLastSelection = nvc;
        PhoenixDashboardOption.DashboardLastSelected(nvc);
    }

    private void BindMeasure()
    {
        NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
        DataSet ds = PhoenixDashboardTechnical.DashboardMeasure("CSY", General.GetNullableString(nvc.Get("VesselList")));
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[1].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[1];
                if (ViewState["Edit"].ToString() != "1")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        BoundField field = new BoundField();
                        field.HeaderText = dt.Rows[i]["FLDVESSELNAME"].ToString();
                        field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        field.HeaderStyle.Wrap = false;
                        gvMeasure.Columns.Add(field);
                    }
                }
                gvMeasure.DataSource = ds;
                gvMeasure.DataBind();
            }
            else
            {
				if (ds.Tables[0].Rows.Count > 0)
				{
					gvMeasure.DataSource = ds;
					gvMeasure.DataBind();
				}
			}
            if (ds.Tables[3].Rows.Count > 0)
            {
                string utcDate = ds.Tables[3].Rows[0]["FLDSCHEDULEDATE"].ToString();
                lblModifiedDate.Text = utcDate.ToString();
            }
        }
    }

    protected void gvMeasure_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView gv = (GridView)sender;
        DataSet ds = (DataSet)gv.DataSource;
        DataRowView drv = (DataRowView)e.Row.DataItem;
        DataTable header = ds.Tables[1];
        DataTable data = ds.Tables[2];

        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int i = 0; i < header.Rows.Count; i++)
            {
                LinkButton lnk = new LinkButton();
                lnk.ID = "lnk_" + i.ToString();
                lnk.Text = e.Row.Cells[i + 3].Text;
                lnk.CommandName = "VESSEL";
                lnk.Attributes.Add("onclick", "javascript:return Openpopup('codehelp1','','../Dashboard/DashboardVesselParticulars.aspx?vesselid=" + header.Rows[i]["FLDVESSELID"].ToString() + "'); return false;");
                e.Row.Cells[i + 3].Controls.Add(lnk);
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string measureid = ((Label)e.Row.FindControl("lblMeasureId")).Text;
            string measurecode = ((Label)e.Row.FindControl("lblMeasureCode")).Text;
            string measurename = ((Label)e.Row.FindControl("lblMeasureName")).Text;
            string showdetail = ((Label)e.Row.FindControl("lblshowdetail")).Text;

            ImageButton cmdGraph = (ImageButton)e.Row.FindControl("cmdGraph");
            if (cmdGraph != null)
            {
                string argsp = measureid + "~" + measurename;
                cmdGraph.CommandArgument = argsp;
            }
            ImageButton cmdColor = (ImageButton)e.Row.FindControl("cmdColor");
            if (cmdColor != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdColor.CommandName)) cmdColor.Visible = false;
                cmdColor.Attributes.Add("onclick", "javascript:return Openpopup('codehelp1','','../Dashboard/DashboardKPI.aspx?measureid=" + measureid + "'); return false;");
            }

            for (int i = 0; i < header.Rows.Count; i++)
            {
                if (showdetail == "1")
                {
                    DataRow[] dr = data.Select("FLDMEASUREID = '" + measureid + "' AND FLDVESSELID = " + header.Rows[i]["FLDVESSELID"].ToString());
                    if (dr.Length > 0)
                    {

                        string args = e.Row.RowIndex.ToString() + "~" + (i + 3) + "~" + dr[0]["FLDVESSELID"].ToString() + "~" + dr[0]["FLDVESSELNAME"].ToString() + "~" + measureid + "~" + measurename + "~" + measurecode;
                        LinkButton lbl = new LinkButton();
                        lbl.ID = "lbl_" + e.Row.RowIndex + "_" + i + 3;
                        lbl.Text = dr[0]["FLDMEASURE"].ToString();
                        lbl.CommandName = "SELECT";
                        lbl.CommandArgument = args;
                        lbl.Attributes.Add("class", "label tableMeasureName");
                        lbl.BackColor = Color.FromName(dr[0]["FLDCOLOR"].ToString() == string.Empty ? "#27727B" : dr[0]["FLDCOLOR"].ToString());
                        e.Row.Cells[i + 3].Controls.Add(lbl);
                        e.Row.Cells[i + 3].CssClass = "customIcon";

                    }
                    else
                    {

                        e.Row.Cells[i + 3].Text = "0";
                        e.Row.Cells[i + 3].CssClass = "customIcon";
                    }
                }
                if (showdetail == "0")
                {
                    //string args = e.Row.RowIndex.ToString() + "~" + (i + 3) + "~" + header.Rows[i]["FLDVESSELID"].ToString() + "~" + header.Rows[i]["FLDVESSELNAME"].ToString() + "~" + measureid + "~" + measurename;
                    ImageButton ibchart = new ImageButton();
                    ibchart.ID = "lbl_" + e.Row.RowIndex + "_" + i + 3;
                    //ibchart.CommandName = "SELECT";
                    //ibchart.CommandArgument = args;
                    ibchart.ImageUrl = Session["images"] + "/document.svg";
                    ibchart.Attributes.Add("onclick", "javascript:return Openpopup('Component', '', '../Dashboard/DashboardVesselCertificates.aspx?vesselid=" + header.Rows[i]["FLDVESSELID"].ToString()+"&vesselname="+ header.Rows[i]["FLDVESSELNAME"].ToString()+ "'); return false;");
                    ibchart.CssClass = "customIcon";
                    e.Row.Cells[i + 3].Controls.Add(ibchart);
                    e.Row.Cells[i + 3].CssClass = "customIcon";
					cmdGraph.Enabled = false;
					cmdColor.Enabled = false;
				}
            }
        }
    }

    protected void gvMeasure_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["Edit"] = "1";
                string[] args = e.CommandArgument.ToString().Split('~');

                string row = args[0];
                string column = args[1];
                string vesselid = args[2];
                string vesselname = args[3];
                string measureid = args[4];
                string measurename = args[5];

                NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
                nvc["Row"] = row;
                nvc["Col"] = column;
                nvc["VesselId"] = vesselid;
                nvc["VesselName"] = vesselname;
                nvc["MeasureId"] = measureid;
                nvc["MeasureName"] = measurename;
                Filter.CurrentDashboardLastSelection = nvc;
                PhoenixDashboardOption.DashboardLastSelected(nvc);
                lblName.Text = " [ " + vesselname + " - " + measurename + " ] ";
                //lblMeasureTitle.Text = measurename;
                ViewState["PAGENUMBER"] = 1;
                BindMeasureResult();
                DisplayMeasureChart();

                gvMeasure.SelectedIndex = int.Parse(row);
                gvMeasure.Rows[int.Parse(row)].Cells[int.Parse(column)].BackColor = System.Drawing.Color.LightGray;

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "gridsize", "calcW('dataMenu')", true);
            }

            if (e.CommandName.ToUpper().Equals("GRAPH"))
            {
                ViewState["Edit"] = "1";

                string[] args = e.CommandArgument.ToString().Split('~');

                string measureid = args[0];
                string measurename = args[1];

                NameValueCollection nvc = Filter.CurrentDashboardLastSelection;

                nvc["MeasureId"] = measureid;
                nvc["MeasureName"] = measurename;
                Filter.CurrentDashboardLastSelection = nvc;
                PhoenixDashboardOption.DashboardLastSelected(nvc);

                DisplayMeasureChart();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "chart popup", "var dataSName = [" + vessellist + "]; var dataValues= [" + valuelist + "]; var colourList = [" + colorlist + "]; callDia('" + measurename.ToString() + "', '', 'Certificate',dataSName,dataValues,colourList);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindMeasureResult()
    {
        NameValueCollection nvc = Filter.CurrentDashboardLastSelection;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixDashboardTechnical.DashboardMeasureResult("CSY"
             , General.GetNullableGuid(nvc.Get("MeasureId").ToString())
             , General.GetNullableInteger(nvc.Get("VesselId"))
             , sortexpression
             , sortdirection
             , (int)ViewState["PAGENUMBER"]
             , General.ShowRecords(null)
             , ref iRowCount
             , ref iTotalPageCount);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvMeasureResult.DataSource = ds.Tables[0];
                gvMeasureResult.DataBind();

                int nCurrentRow = int.Parse(nvc.Get("ResultRow") != null ? (nvc.Get("ResultRow").ToString() != "" ? nvc.Get("ResultRow").ToString() : "0") : "0");
                if (ds.Tables[0].Rows.Count > nCurrentRow)
                {
                   gvMeasureResult.SelectedIndex = nCurrentRow;
                }
            }
            else
            {
                ShowNoRecordsFound(ds.Tables[0], gvMeasureResult);
                Filter.CurrentDashboardLastSelection["Row"] = "0";
                Filter.CurrentDashboardLastSelection["Col"] = "0";
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();
        }
    }

    private void DisplayMeasureChart()
    {
        NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
        DataSet ds = PhoenixDashboardTechnical.DashboardMeasureChart("CSY"
             , new Guid(nvc.Get("MeasureId"))
            , General.GetNullableString(nvc.Get("VesselList")));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            int i = 0;
            while (i < dt.Rows.Count)
            {
                vessellist = vessellist + "\"" + dt.Rows[i]["FLDVESSELNAME"].ToString() + "\"" + ",";
                valuelist = valuelist + (dt.Rows[i]["FLDMEASURE"].ToString() == string.Empty ? "'-'" : dt.Rows[i]["FLDMEASURE"].ToString()) + ",";
                colorlist = colorlist + "'" + (dt.Rows[i]["FLDCOLOR"].ToString() == string.Empty ? "#27727B" : dt.Rows[i]["FLDCOLOR"].ToString()) + "'" + ",";
                i++;
            }
            vessellist = vessellist.TrimEnd(',');
            valuelist = valuelist.TrimEnd(',');
            colorlist = colorlist.TrimEnd(',');
            //PhoenixCommonChart pcc = new PhoenixCommonChart(ChartMeasure, "");
            //pcc.LegendDocking = Docking.Top;
            //pcc.HideLegend();
            //pcc.ChartType = SeriesChartType.Column;
            //pcc.YSeries("", new YAxisColumn("FLDMEASURE", nvc.Get("MeasureName") != null ? nvc.Get("MeasureName").ToString() : ""));
            //pcc.XSeries("", 1, "FLDVESSELNAME");
            //pcc.Enable3D = false;
            //pcc.Show(dt);

            //if (ds.Tables[0].Rows.Count > 5)
            //    ChartMeasure.Width = 550 + ((ds.Tables[0].Rows.Count - 5) * 10);
        }
    }

    protected void gvMeasureResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;

            Label lblHeader = new Label();
            lblHeader.ID = "lblHeader";
            lblHeader.Text = "Action";
            e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(lblHeader);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;

            LinkButton lnklLink = new LinkButton();
            lnklLink.ID = "lblResultLink";
            lnklLink.Text = e.Row.Cells[3].Text;
            lnklLink.CommandName = "SELECT";
            lnklLink.CommandArgument = e.Row.RowIndex.ToString();
            if (!SessionUtil.CanAccess(this.ViewState, lnklLink.CommandName))
            {
                Label lbl = new Label();
                lbl.Text = e.Row.Cells[3].Text;
                e.Row.Cells[3].Controls.Add(lbl);
            }
            else
            {
                e.Row.Cells[3].Controls.Add(lnklLink);

            }

            e.Row.Cells[4].Visible = true ;

            ImageButton ImgEdit = new ImageButton();
            ImgEdit.ID = "ImgEdit";
            ImgEdit.ImageUrl = Session["sitepath"] + "/css/" + Session["theme"].ToString() + "/images/te_edit.png";
            ImgEdit.CommandName = "SELECT";
            ImgEdit.CommandArgument = e.Row.RowIndex.ToString();
            e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(ImgEdit);

            //if (lnklLink != null) lnklLink.Visible = SessionUtil.CanAccess(this.ViewState, lnklLink.CommandName);
            if (ImgEdit != null) ImgEdit.Visible = SessionUtil.CanAccess(this.ViewState, ImgEdit.CommandName);
        }
    }

    protected void gvMeasureResult_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
                nvc["ResultRow"] = nCurrentRow.ToString();
                if (nvc["SelectedModuleScreen"] == null)
                    nvc["SelectedModuleScreen"] = "";
                nvc["SelectedModuleScreen"] = "../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleList.aspx";
                PhoenixDashboardOption.DashboardLastSelected(nvc);
                Filter.CurrentDashboardLastSelection = nvc;
                if (nvc != null)
                {
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(nvc.Get("VesselId") != null ? nvc.Get("VesselId").ToString() : "0");
                    PhoenixSecurityContext.CurrentSecurityContext.VesselName = nvc.Get("VesselName") != null ? nvc.Get("VesselName").ToString() : "";

                    Filter.CurrentSelectedModule = "Certificates and Surveys";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", "showVessel()", true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ChartMeasure_Click(object sender, ImageMapEventArgs e)
    {
        Chart c = (Chart)sender;
        foreach (Series s in c.Series)
        {
            System.Console.WriteLine(s.XValueMember);
        }
        System.Console.WriteLine(e.PostBackValue);
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
            ViewState["Edit"] = "1";
            BindMeasure();
            BindMeasureResult();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        try
        {

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

            BindMeasureResult();
            SetPageNavigator();
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
        {
            return true;
        }

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
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvMeasureResult.SelectedIndex = -1;
            gvMeasureResult.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindMeasureResult();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PageAccessValidation(object sender, CommandEventArgs ce)
    {
        try
        {
            string cmdName = ce.CommandName.ToString();
            if (!SessionUtil.CanAccess(this.ViewState, cmdName.ToUpper()))
            {
                ucError.ErrorMessage = "Access denied";
                ucError.Visible = true;
            }
            else
            {
                if (cmdName.ToUpper().Equals("PMS"))
                {
                    Response.Redirect("../Dashboard/DashboardTechnicalPms.aspx");
                }
                if (cmdName.ToUpper().Equals("PURCHASE"))
                {
                    Response.Redirect("../Dashboard/DashboardTechnicalPurchase.aspx");
                }
                if (cmdName.ToUpper().Equals("VETTING"))
                {
                    Response.Redirect("../Dashboard/DashboardTechnicalVetting.aspx");
                }
                if (cmdName.ToUpper().Equals("INSPECTION"))
                {
                    Response.Redirect("../Dashboard/DashboardTechnicalInspection.aspx");
                }
                if (cmdName.ToUpper().Equals("WRH"))
                {
                    Response.Redirect("../Dashboard/DashboardTechnicalWorkRestHours.aspx");
                }
                if (cmdName.ToUpper().Equals("CERTIFICATE"))
                {
                    Response.Redirect("../Dashboard/DashboardTechnicalCertificatesandSurveys.aspx");
                }
                if (cmdName.ToUpper().Equals("PERFORMANCE"))
                {
                    Response.Redirect("../Dashboard/DashboardTechnicalPerformance.aspx");
                }
				if (cmdName.ToUpper().Equals("ANALYTICS"))
				{
					Response.Redirect("../Dashboard/QualityPBI.html");
				}
			}
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
	private void BindMenu()
	{
		try
		{
			DataSet ds;
			ds = PhoenixDashboardOption.DashBoardModule(1);     // 1.TECHNICAL 2.CREW
			if (ds.Tables[0].Rows.Count > 0)
			{
				HtmlGenericControl div;
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					div = new HtmlGenericControl("div");
					if (ds.Tables[0].Rows[i]["FLDCOMMANDNAME"].ToString().ToUpper().Equals("CERTIFICATE"))
					{
						div.Attributes["class"] = "icoBlk icoActive"; // for selected or current module
					}
					else
					{
						div.Attributes["class"] = "icoBlk";
					}

					div.Attributes["id"] = "id" + ds.Tables[0].Rows[i]["FLDCOMMANDNAME"].ToString();

					Label L1 = new Label();
					if (ds.Tables[0].Rows[i]["FLDCOMMANDNAME"].ToString() != "OTIS")
					{
						ImageButton ib = new ImageButton
						{
							CommandName = ds.Tables[0].Rows[i]["FLDCOMMANDNAME"].ToString(),
							CommandArgument = i.ToString(),
							ImageUrl = Session["sitepath"] + "/css/" + Session["theme"].ToString() + "/images/" + ds.Tables[0].Rows[i]["FLDIMAGENAME"].ToString()
						};
						ib.Command += PageAccessValidation;
						div.Controls.Add(ib);
					}
					else
					{
						HtmlImage img = new HtmlImage
						{
							Src = Session["sitepath"] + "/css/" + Session["theme"].ToString() + "/images/" + ds.Tables[0].Rows[i]["FLDIMAGENAME"].ToString()
						};
						div.Controls.Add(img);
					}
					L1.Text = "<br>" + ds.Tables[0].Rows[i]["FLDMODULENAME"].ToString();					
					div.Controls.Add(L1);
					techIcons.Controls.AddAt(i, div);

				}
			}

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void DashboardExport_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		try
		{
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
	private void ShowExcel()
	{
		NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;

		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		iRowCount = ViewState["ROWCOUNT"] != null ? Int32.Parse(ViewState["ROWCOUNT"].ToString()) : 10;

		DataSet ds = PhoenixDashboardTechnical.DashboardMeasureResult("CSY"
			 , General.GetNullableGuid(nvc.Get("MeasureId").ToString())
			 , General.GetNullableInteger(nvc.Get("VesselId"))
			 , sortexpression
			 , sortdirection
			 , 1
			 , iRowCount
			 , ref iRowCount
			 , ref iTotalPageCount);

		string[] ignoreColumns = {"FLDDTKEY", "FLDVESSELID", "FLDCERTIFICATECODE", "Column1", };
		string[] alColumns = PhoenixDashboardOption.GetCaptionForExcel(ds.Tables[0], ignoreColumns);
		string[] alCaptions = alColumns;

		General.ShowExcel(lblName.Text, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
	}

}
