using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class DashboardCrewCrewList : PhoenixBasePage
{

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SelectedOption();

            ViewState["MOD"] = "CREWCNC";
            gvMeasureResult.PageSize = General.ShowRecords(null);
            ViewState["MEASUREID"] = "";
            ViewState["FLDRANKID"] = "";
            ViewState["ROWCOUNT"] = 10;


        }
        BindToolbar();
        BindMenu();
        BindMeasure();
    }

    private void BindMeasure()
    {
        NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
        DataSet ds = PhoenixDashboardTechnical.DashboardMeasure(ViewState["MOD"].ToString(), General.GetNullableString(nvc.Get("RankList")));

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[3].Rows.Count > 0)
            {
                string utcDate = ds.Tables[3].Rows[0]["FLDSCHEDULEDATE"].ToString();
                lblModifiedDate.Text = utcDate.ToString();
            }

        }
    }


    protected void BindToolbar()
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardCrewCrewList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        gvExport.AccessRights = this.ViewState;
        gvExport.MenuList = toolbargrid.Show();
    }


    protected void GvCrew_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
    {
        RadPivotGrid grid = (RadPivotGrid)sender;

        NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
        DataSet ds = PhoenixDashboardCrew.DashboardMeasure("CREWCNC", General.GetNullableString(nvc.Get("RankList")));
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[2].Rows.Count > 0)
            {
                if (ViewState["MEASUREID"].ToString() == string.Empty)
                    ViewState["MEASUREID"] = ds.Tables[2].Rows[0]["FLDMEASUREID"].ToString();
                if (ViewState["FLDRANKID"].ToString() == string.Empty)
                    ViewState["FLDRANKID"] = ds.Tables[2].Rows[0]["FLDRANKID"].ToString();


                GvCrew.DataSource = ds.Tables[2];
            }
            else
            {

                GvCrew.DataSource = null;
            }

        }

    }

    protected void GvCrew_CellDataBound(object sender, Telerik.Web.UI.PivotGridCellDataBoundEventArgs e)
    {

        RadPivotGrid grid = (RadPivotGrid)sender;
        if (e.Cell is PivotGridRowHeaderCell)
        {
            PivotGridRowHeaderCell cell = (PivotGridRowHeaderCell)e.Cell;
            PivotGridRowHeaderItem item = (PivotGridRowHeaderItem)e.Cell.DataItemContainer;
            string row = cell.ParentIndexes.Length > 0 ? cell.ParentIndexes[0].ToString() : string.Empty;
            System.Collections.ArrayList itemarray = (System.Collections.ArrayList)item.DataItem;
            if (itemarray.Count > 1)
            {
                DataTable dt = (DataTable)grid.DataSource;
                DataRow[] dr = dt.Select("FLDMEASUREID = '" + itemarray[1].ToString() + "'");
                bool isNumeric = true;

                if (cell.Text.Trim() == string.Empty)
                {
                    cell.Text = (isNumeric ? "<a title=\"Color\" alternatetext=\"Color\" href=\"javascript: openNewWindow('color', 'Color', 'Dashboard/DashboardRankKPI.aspx?measureid=" + itemarray[1].ToString() + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                       + "<span class=\"icon\"><i class=\"fas fa-cog\"></i></span>"
                                          + "</a>" + "<a title=\"Chart\" alternatetext=\"Chart\" href=\"javascript: openNewWindow('chart', 'Chart', 'Dashboard/DashboardRankChart.aspx?mod=" + ViewState["MOD"].ToString() + "&mid=" + itemarray[1].ToString() + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                       + "<span class=\"icon\"><i class=\"far fa-chart-bar\"></i></span>"
                                              + "</a>" : String.Empty) + cell.DataItem.ToString();
                }
            }
        }
        if (e.Cell is PivotGridDataCell)
        {
            PivotGridDataCell cell = (PivotGridDataCell)e.Cell;
            if (cell.CellType == PivotGridDataCellType.DataCell)
            {
                DataTable dt = (DataTable)grid.DataSource;
                string code = string.Empty;

                string measureid = cell.ParentRowIndexes.Length > 1 ? cell.ParentRowIndexes[1].ToString() : string.Empty;
                string Rank = cell.ParentColumnIndexes.Length > 0 ? cell.ParentColumnIndexes[0].ToString() : string.Empty;

                DataRow[] dr = dt.Select("FLDMEASUREID = '" + measureid + "' AND FLDRANKNAME='" + Rank + "'");
                if (dr.Length == 0)
                {
                    cell.HorizontalAlign = HorizontalAlign.Right;
                    cell.CssClass = "customIcon";
                    cell.Text = "0";
                }
                foreach (DataRow d in dr)
                {
                    string text = cell.Text;
                    cell.HorizontalAlign = HorizontalAlign.Right;
                    cell.CssClass = "label";


                    string args = d["FLDRANKID"].ToString() + "~" + Rank + "~" + measureid + "~" + d["FLDMEASURENAME"].ToString();

                    if (d["FLDMEASURE"].ToString() != "0")
                    {
                        cell.CssClass = "customIcon";
                        cell.Text = "<a style=\"background-color: " + (d["FLDCOLOR"].ToString().Equals("") ? "#27727B" : d["FLDCOLOR"].ToString()) + ";\" class=\"mlabel\" onclick=\"BindMeasureResult('" + measureid + "', '" + d["FLDRANKID"].ToString() + "','" + args + "')\" >" + cell.DataItem.ToString() + "</a>";
                    }
                    else
                    {
                        cell.CssClass = "customIcon";
                        cell.Text = "0";
                    }

                }
            }
        }
    }


    protected void gvMeasureResult_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
        gvMeasureResult.MasterTableView.Columns.Clear();

        DataSet ds = PhoenixDashboardCrew.DashboardMeasureResult("CREWCNC"
             , General.GetNullableGuid(nvc.Get("MeasureId").ToString())
             , General.GetNullableInteger(nvc.Get("RankId"))
             , null
             , null
             , gvMeasureResult.CurrentPageIndex + 1
             , gvMeasureResult.PageSize
             , ref iRowCount
             , ref iTotalPageCount);
        grid.DataSource = ds;
        grid.VirtualItemCount = iRowCount;
        if (ds.Tables.Count > 0)
        {
            foreach (DataColumn c in ds.Tables[0].Columns)
            {
                GridBoundColumn T = new GridBoundColumn();
                gvMeasureResult.MasterTableView.Columns.Add(T);
                T.HeaderText = c.ColumnName;
                T.UniqueName = c.ColumnName.Replace(" ", "_");
                T.ReadOnly = true;
                T.DataField = c.ColumnName;
                T.DataType = typeof(System.String);
            }
            ViewState["ROWCOUNT"] = iRowCount;
        }
    }

    protected void gvMeasureResult_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "RowClick" && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            GridDataItem item = (GridDataItem)e.Item;
            NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
            nvc["ResultRow"] = item.ItemIndex.ToString();
            if (nvc["SelectedModuleScreen"] == null)
                nvc["SelectedModuleScreen"] = "";

            string employeeid = item["FLDEMPLOYEEID"].Text.ToString();
            nvc["SelectedModuleScreen"] = "../Crew/CrewPersonalGeneral.aspx?empid=" + employeeid.ToString();
            PhoenixDashboardOption.DashboardLastSelected(nvc);
            Filter.CurrentDashboardLastSelection = nvc;
            Filter.CurrentSelectedModule = "Crew";

            Response.Redirect("../Crew/CrewPersonalGeneral.aspx?empid=" + employeeid.ToString());
        }
        if (e.CommandName.ToUpper().Equals("DOALINK"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            string employeeid = item["FLDEMPLOYEEID"].Text.ToString();

            String scriptpopup = String.Format("javascript:parent.Openpopup('codehelp1','','../Crew/CrewDateOfAvailability.aspx?empid=" + employeeid.ToString() + "');");

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
        }


    }


    protected void gvMeasureResult_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void GvCrew_ItemCommand(object sender, PivotGridCommandEventArgs e)
    {

    }

    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        gvMeasureResult.Rebind();
    }

    protected void hdnButton_Click(object sender, EventArgs e)
    {
        NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
        string[] t = args.Value.ToString().Split('~');
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }
        nvc["RankId"] = t[0];
        nvc["RankName"] = t[1];
        nvc["MeasureId"] = t[2];
        nvc["MeasureName"] = t[3];
        //nvc["MeasureCode"] = t[4];
        Filter.CurrentDashboardLastSelection = nvc;
        PhoenixDashboardOption.DashboardLastSelected(nvc);

        hdnMeasureid.Value = t[2];
        hdnRankid.Value = t[0];
        lblName.Text = " [ " + t[1] + " - " + t[3] + " ] ";
        gvMeasureResult.Rebind();
        GvCrew.Rebind();
    }

    protected void gvMeasureResult_PreRender(object sender, EventArgs e)
    {
        if (gvMeasureResult.MasterTableView.Columns.FindByUniqueNameSafe("column1") != null)
            gvMeasureResult.MasterTableView.GetColumn("column1").Visible = false;
        if (gvMeasureResult.MasterTableView.Columns.FindByUniqueNameSafe("FLDEMPLOYEEID") != null)
            gvMeasureResult.MasterTableView.GetColumn("FLDEMPLOYEEID").Visible = false;

    }

    private void SelectedOption()
    {
        DataSet ds = PhoenixDashboardOption.DashboardLastSelectedEdit("CREW", "CNC");
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
            //nvc.Add("ResultRow", ds.Tables[0].Rows[0]["FLDSELECTEDRESULTROW"].ToString());
            nvc.Add("VesselList", ds.Tables[0].Rows[0]["FLDVESSELLIST"].ToString());
            nvc.Add("RankList", ds.Tables[0].Rows[0]["FLDRANKLIST"].ToString());
            //nvc.Add("SelectedModuleScreen", "");

            hdnMeasureid.Value = ds.Tables[0].Rows[0]["FLDMEASUREID"].ToString();
            hdnRankid.Value = ds.Tables[0].Rows[0]["FLDRANKID"].ToString();
            lblName.Text = " [ " + ds.Tables[0].Rows[0]["FLDRANKNAME"].ToString() + " - " + ds.Tables[0].Rows[0]["FLDMEASURENAME"].ToString() + " ] ";
        }
        else
        {
            DataSet d = PhoenixDashboardCrew.DashboardMeasure("APPRAISAL", null);
            nvc.Add("APP", "CREW");
            nvc.Add("Option", "CNC");
            if (d.Tables[1].Rows.Count > 0)
            {
                nvc.Add("RankId", d.Tables[1].Rows[0]["FLDRANKID"].ToString());
                nvc.Add("RankName", d.Tables[1].Rows[0]["FLDRANKNAME"].ToString());

                lblName.Text = " [ " + d.Tables[1].Rows[0]["FLDRANKNAME"].ToString() + " ] ";
            }
            else
            {
                nvc.Add("RankId", "0");
                nvc.Add("RankName", "Rank");
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
                nvc.Add("RankList", "");
                nvc.Add("SelectedModuleScreen", "");
            }
            else
            {
                nvc.Add("MeasureId", "");
                nvc.Add("MeasureName", "");
                nvc.Add("Row", "0");
                nvc.Add("Col", "2");
                nvc.Add("ResultRow", "0");
                nvc.Add("VesselList", "");
                nvc.Add("RankList", "");
                nvc.Add("SelectedModuleScreen", "");
            }
        }

        //lblMeasureTitle.Text = nvc.Get("MeasureName").ToString();
        Filter.CurrentDashboardLastSelection = nvc;
        PhoenixDashboardOption.DashboardLastSelected(nvc);
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
                if (cmdName.ToUpper().Equals("APPRAISAL"))
                {
                    Response.Redirect("../Dashboard/DashboardCrewAppraisals.aspx");
                }
                if (cmdName.ToUpper().Equals("CLP"))
                {
                    Response.Redirect("../Dashboard/DashboardCrewLegal.aspx");
                }
                if (cmdName.ToUpper().Equals("CNC"))
                {
                    Response.Redirect("../Dashboard/DashboardCrewCrewList.aspx");
                }
                if (cmdName.ToUpper().Equals("ICEEXP"))
                {
                    Response.Redirect("../Dashboard/DashboardCrewVesselPosition.aspx");
                }
                if (cmdName.ToUpper().Equals("ODE"))
                {
                    Response.Redirect("../Dashboard/DashboardCrewOnboardDocumentExpiry.aspx");
                }
                if (cmdName.ToUpper().Equals("INVOICE"))
                {
                    Response.Redirect("../Dashboard/DashboardCrewAccounts.aspx");
                }
                if (cmdName.ToUpper().Equals("CREW"))
                {
                    Response.Redirect("../Dashboard/DashboardCrewAndFamily.aspx");
                }
            }
        }
        catch (Exception ex)
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
            ds = PhoenixDashboardOption.DashBoardModule(2);     // 1.TECHNICAL 2.CREW
            if (ds.Tables[0].Rows.Count > 0)
            {
                HtmlGenericControl div;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    div = new HtmlGenericControl("div");
                    if (ds.Tables[0].Rows[i]["FLDCOMMANDNAME"].ToString().ToUpper().Equals("CNC"))
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
                    crewIcons.Controls.AddAt(i, div);

                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvExport_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
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

        if (iRowCount == 0)
        {
            iRowCount = 10;
        }

        DataSet ds = PhoenixDashboardCrew.DashboardMeasureResult("CREWCNC"
             , General.GetNullableGuid(nvc.Get("MeasureId").ToString())
             , General.GetNullableInteger(nvc.Get("RankId"))
             , sortexpression
             , sortdirection
             , 1
             , iRowCount
             , ref iRowCount
             , ref iTotalPageCount);

        string[] ignoreColumns = { "FLDEMPLOYEEID", "Column1" };
        string[] alColumns = PhoenixDashboardOption.GetCaptionForExcel(ds.Tables[0], ignoreColumns);
        string[] alCaptions = alColumns;

        General.ShowExcel(gvExport.Title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

}
