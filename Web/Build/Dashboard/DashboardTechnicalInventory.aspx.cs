using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;
using System.Web.UI.DataVisualization.Charting;

public partial class DashboardTechnicalInventory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdChart.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Dashboard/DashboardChart.aspx?measure=INVENTORY&type=1'); return false;");
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            ViewState["Edit"] = "0";
            if (!IsPostBack)
            {
                SelectedOption();
                DisplayMeasureChart();
            }
            BindMeasure();
            BindMeasureResult();
            if (!IsPostBack)
            {
                int row = int.Parse(Filter.CurrentDashboardLastSelection["Row"]);
                int col = int.Parse(Filter.CurrentDashboardLastSelection["Col"]);
                if (row != 0 || col != 0)
                {
                    gvMeasure.SelectedIndex = row;
                    gvMeasure.Rows[row].Cells[col].BackColor = System.Drawing.Color.LightGray;
                }
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
        DataSet ds = PhoenixDashboardOption.DashboardLastSelectedEdit("TECH", "INY");
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
            nvc.Add("Row", ds.Tables[0].Rows[0]["FLDSELECTEDROW"].ToString());
            nvc.Add("Col", ds.Tables[0].Rows[0]["FLDSELECTEDCOLUMN"].ToString());
            nvc.Add("ResultRow", ds.Tables[0].Rows[0]["FLDSELECTEDRESULTROW"].ToString());
            nvc.Add("VesselList", ds.Tables[0].Rows[0]["FLDVESSELLIST"].ToString());
            
        }
        else
        {
            DataSet d = PhoenixDashboardTechnical.DashboardMeasure("INVENTORY", null);
            nvc.Add("APP", "TECH");
            nvc.Add("Option", "INY");
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
            nvc.Add("MeasureId", d.Tables[0].Rows[0]["FLDMEASUREID"].ToString());
            nvc.Add("MeasureName", d.Tables[0].Rows[0]["FLDMEASURENAME"].ToString());
            nvc.Add("Row", "0");
            nvc.Add("Col", "2");
            nvc.Add("ResultRow", "0");
            nvc.Add("VesselList", "");
            nvc.Add("SelectedModuleScreen", "");
        }
        lblName.Text = " [ " + nvc.Get("VesselName").ToString() + " - " + nvc.Get("MeasureName").ToString() + " ] ";
        lblMeasureTitle.Text = nvc.Get("MeasureName").ToString();
        Filter.CurrentDashboardLastSelection = nvc;
        PhoenixDashboardOption.DashboardLastSelected(nvc);
    }
   
    private void BindMeasure()
    {
        NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
        DataSet ds = PhoenixDashboardTechnical.DashboardMeasure("INVENTORY", General.GetNullableString(nvc.Get("VesselList")));
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
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
                        gvMeasure.Columns.Add(field);
                    }
                }
                gvMeasure.DataSource = ds;
                gvMeasure.DataBind();             
            }
            else
            {
                ShowNoRecordsFound(ds.Tables[0], gvMeasure);
            }
            if (ds.Tables[3].Rows.Count > 0)
            {
                string utcDate = ds.Tables[3].Rows[0]["FLDSCHEDULEDATE"].ToString();
                //DateTime localtime = DateTime.Parse(utcDate).ToLocalTime();
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
                lnk.Text = e.Row.Cells[i + 2].Text;
                lnk.CommandName = "VESSEL";
                lnk.CommandArgument = i.ToString() + "_" + header.Rows[i]["FLDVESSELID"].ToString() + "_" + header.Rows[i]["FLDVESSELNAME"].ToString();
                lnk.ForeColor = System.Drawing.Color.White;
                e.Row.Cells[i + 2].Controls.Add(lnk);
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string measureid = ((Label)e.Row.FindControl("lblMeasureId")).Text;
            string measurename = ((Label)e.Row.FindControl("lblMeasureName")).Text;
            for (int i = 0; i < header.Rows.Count; i++)
            {
                DataRow[] dr = data.Select("FLDMEASUREID = '" + measureid + "' AND FLDVESSELID = " + header.Rows[i]["FLDVESSELID"].ToString());
                if (dr.Length > 0)
                {
                    LinkButton lbl = new LinkButton();
                    lbl.ID = "lbl_" + e.Row.RowIndex + "_" + i + 2;
                    lbl.Text = dr[0]["FLDMEASURE"].ToString();
                    lbl.CommandName = "SELECT";
                    lbl.CommandArgument = e.Row.RowIndex.ToString() + "~" + (i + 2) + "~" + dr[0]["FLDVESSELID"].ToString() + "~" + dr[0]["FLDVESSELNAME"].ToString() + "~" + measureid + "~" + measurename;
                    e.Row.Cells[i + 2].Controls.Add(lbl);
                    e.Row.Cells[i + 2].Attributes.Add("style", "background-color:" + dr[0]["FLDCOLOR"].ToString());
                }
                else
                {
                    e.Row.Cells[i + 2].Text = "-";
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
                lblMeasureTitle.Text = measurename;
                BindMeasureResult();
                DisplayMeasureChart();
                gvMeasure.SelectedIndex = int.Parse(row);
                gvMeasure.Rows[int.Parse(row)].Cells[int.Parse(column)].BackColor = System.Drawing.Color.LightGray;
            }
            if (e.CommandName.ToUpper().Equals("COLOR"))
            {
                int nCurrentRow = int.Parse(e.CommandArgument.ToString());
                string measureid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblMeasureId")).Text;
                String script = "parent.Openpopup('codehelp1','','../Dashboard/DashboardKPI.aspx?measureid=" + measureid + "')";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
            }
            if (e.CommandName.ToUpper().Equals("VESSEL"))
            {
                string[] args = e.CommandArgument.ToString().Split('_');
                string vesselid = args[1];
                string vesselname = args[2];

                NameValueCollection nvc = new NameValueCollection();
                nvc.Clear();
                nvc.Add("Url", HttpContext.Current.Request.ApplicationPath + "/DashBoard/DashboardCommonVesselParticulars.aspx");
                nvc.Add("App", "TECH");
                nvc.Add("Option", "VOV");
                nvc.Add("VesselId", vesselid);
                nvc.Add("VesselName", vesselname);
                PhoenixDashboardOption.DashboardLastSelected(nvc);

                PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(vesselid);
                PhoenixSecurityContext.CurrentSecurityContext.VesselName = vesselname;
                Filter.CurrentOrderFormFilterCriteria = null;

                DataSet dsr = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                PhoenixGeneralSettings.CurrentGeneralSetting = new GeneralSetting(dsr);
                SessionUtil.ReBuildMenu();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", "showVessel();", true);
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
        DataSet ds = PhoenixDashboardTechnical.DashboardMeasureResult("INVENTORY"
             , General.GetNullableGuid(nvc.Get("MeasureId").ToString())
             , General.GetNullableInteger(nvc.Get("VesselId")));  
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
        }
    }

    protected void gvMeasureResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            Label lblHeader = new Label();
            lblHeader.ID = "lblHeader";
            lblHeader.Text = "Action";
            e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(lblHeader);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnklLink = new LinkButton();
            lnklLink.ID = "lblResultLink";
            lnklLink.Text = e.Row.Cells[1].Text;
            lnklLink.CommandName = "SELECT";
            lnklLink.CommandArgument = e.Row.RowIndex.ToString();
            e.Row.Cells[1].Controls.Add(lnklLink);

            ImageButton ImgEdit = new ImageButton();
            ImgEdit.ID = "ImgEdit";
            ImgEdit.ImageUrl = Session["sitepath"] + "/css/" + Session["theme"].ToString() + "/images/te_edit.png";
            ImgEdit.CommandName = "SELECT";
            ImgEdit.CommandArgument = e.Row.RowIndex.ToString();
            e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(ImgEdit);

            if (lnklLink != null) lnklLink.Visible = SessionUtil.CanAccess(this.ViewState, lnklLink.CommandName);
            if (ImgEdit != null) ImgEdit.Visible = SessionUtil.CanAccess(this.ViewState, ImgEdit.CommandName);
        }
    }

    protected void gvMeasureResult_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
                nvc["ResultRow"] = nCurrentRow.ToString();
                if (nvc["SelectedModuleScreen"] == null)
                    nvc["SelectedModuleScreen"] = "";
                nvc["SelectedModuleScreen"] = "../Inventory/InventorySpareItem.aspx";
                PhoenixDashboardOption.DashboardLastSelected(nvc);
                Filter.CurrentDashboardLastSelection = nvc;
                if (nvc != null)
                {
                    NameValueCollection criteria = new NameValueCollection();
                    criteria.Clear();

                    criteria.Add("txtNumber", _gridView.Rows[nCurrentRow].Cells[1].Text);
                    criteria.Add("txtName", "");
                    criteria.Add("txtMakerid", "");
                    criteria.Add("txtVendorId", "");
                    criteria.Add("isGolbleSearch", "");
                    criteria.Add("chkCritical", string.Empty);
                    criteria.Add("txtMakerReference", "");
                    criteria.Add("txtDrawing", "");
                    criteria.Add("chkROB", "0");
                    criteria.Add("txtComponentNumber", "");
                    criteria.Add("txtComponentName", "");

                    Filter.CurrentSpareItemFilterCriteria = criteria;

                    PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(nvc.Get("VesselId") != null ? nvc.Get("VesselId").ToString() : "0");
                    PhoenixSecurityContext.CurrentSecurityContext.VesselName = nvc.Get("VesselName") != null ? nvc.Get("VesselName").ToString() : "";

                    Filter.CurrentSelectedModule = "Inventory";

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

    protected void ChartMeasure_Click(object sender, ImageMapEventArgs e)
    {
        Chart c = (Chart)sender;
        foreach (Series s in c.Series)
        {
            System.Console.WriteLine(s.XValueMember);
        }
        System.Console.WriteLine(e.PostBackValue);
    }

    private void DisplayMeasureChart()
    {       
        NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
        DataSet ds = PhoenixDashboardTechnical.DashboardMeasureChart("INVENTORY"
            , new Guid(nvc.Get("MeasureId"))
            , General.GetNullableString(nvc.Get("VesselList")));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            PhoenixCommonChart pcc = new PhoenixCommonChart(ChartMeasure, "");
            pcc.LegendDocking = Docking.Top;
            pcc.HideLegend();
            pcc.ChartType = SeriesChartType.Column;
            pcc.YSeries("", new YAxisColumn("FLDMEASURE", nvc.Get("MeasureName") != null ? nvc.Get("MeasureName").ToString() : ""));
            pcc.XSeries("", 1, "FLDVESSELNAME");
            pcc.Enable3D = false;
            pcc.Show(dt);

            if (ds.Tables[0].Rows.Count > 5)
                ChartMeasure.Width = 550 + ((ds.Tables[0].Rows.Count - 5) * 10);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Edit"] = "1";
            BindMeasure();
            DisplayMeasureChart();
            BindMeasureResult();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
