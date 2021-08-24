using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class DashboardCrewAndFamily : PhoenixBasePage
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

            ViewState["MOD"] = "CREWLIST";
            gvMeasureResult.PageSize = General.ShowRecords(null);
            ViewState["MEASUREID"] = "";
            ViewState["VESSELID"] = "";
            ViewState["ROWCOUNT"] = 10;

            
        }
        BindToolbar();
        BindMenu();
        BindMeasure();
    }
    private void BindMeasure()
    {
        NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
        DataSet ds = PhoenixDashboardTechnical.DashboardMeasure(ViewState["MOD"].ToString(), General.GetNullableString(nvc.Get("VesselList")));
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[3].Rows.Count > 0)
            {
                string utcDate = ds.Tables[3].Rows[0]["FLDSCHEDULEDATE"].ToString();
                //DateTime localtime = DateTime.Parse(utcDate).ToLocalTime();
                lblModifiedDate.Text = utcDate.ToString();
            }

        }
    }
    protected void BindToolbar()
    {      
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardCrewAndFamily.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        gvExport.AccessRights = this.ViewState;
        gvExport.MenuList = toolbargrid.Show();
    }

    protected void GvCrew_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
    {
        RadPivotGrid grid = (RadPivotGrid)sender;
        DataTable dt = PhoenixDashboardTechnical.DashboardOfficeTechnicalByVessel(ViewState["MOD"].ToString());
        GvCrew.DataSource = dt;

        if (dt.Rows.Count > 0)
        {
            if (ViewState["MEASUREID"].ToString() == string.Empty)
                ViewState["MEASUREID"] = dt.Rows[0]["FLDMEASUREID"].ToString();
            if (ViewState["VESSELID"].ToString() == string.Empty)
                ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
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
                if (dr.Length > 0)
                {
                    isNumeric = (dr[0]["FLDISNUMERIC"].ToString() == "1" ? true : false);
                }
                if (cell.Text.Trim() == string.Empty)
                {
                    //"<input type=\"image\" class=\"customIcon\" onclick=\"javascript: return Openpopup('codehelp1', '', '../Dashboard/DashboardKPI.aspx?measureid=e7fe7088-2489-e911-b585-06089601e630'); return false;\" src=\"/Phoenix/css/Theme1/images/settings.svg\" alt=\"Color\" style=\"border-width:0px;\">";
                    cell.Text = (isNumeric ? "<a title=\"Color\" alternatetext=\"Color\" href=\"javascript: openNewWindow('color', 'Color', 'Dashboard/DashboardKPI.aspx?measureid=" + itemarray[1].ToString() + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"fas fa-cog\"></i></span>"
                                           + "</a>" + "<a title=\"Chart\" alternatetext=\"Chart\" href=\"javascript: openNewWindow('chart', 'Chart', 'Dashboard/DashboardV2Chart.aspx?mod=" + ViewState["MOD"].ToString() + "&mid=" + itemarray[1].ToString() + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
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
                string vessel = cell.ParentColumnIndexes.Length > 0 ? cell.ParentColumnIndexes[0].ToString() : string.Empty;

                DataRow[] dr = dt.Select("FLDMEASUREID = '" + measureid + "' AND Vessel='" + vessel + "'");

                foreach (DataRow d in dr)
                {
                    string text = cell.Text;
                    cell.HorizontalAlign = HorizontalAlign.Right;
                    cell.CssClass = "label";

                    string args = d["FLDVESSELID"].ToString() + "~" + vessel + "~" + measureid + "~" + d["Measure"].ToString() + "~" + d["Code"].ToString();

                    if (d["FLDSHOWDETAIL"].ToString() == "1" && d["FLDMEASURE"].ToString() != "0")
                    {
                        cell.CssClass = "customIcon";
                        cell.Text = "<a style=\"background-color: " + (d["FLDCOLOR"].ToString().Equals("") ? "#27727B" : d["FLDCOLOR"].ToString()) + ";\" class=\"mlabel\" onclick=\"BindMeasureResult('" + measureid + "', '" + d["FLDVESSELID"].ToString() + "','" + args + "')\" >" + cell.Text + "</a>";
                    }
                    else
                    {
                        //cell.Text = "<a style=\"background-color: " + (d["FLDCOLOR"].ToString().Equals("") ? "#27727B" : d["FLDCOLOR"].ToString()) + ";\" class=\"mlabel\" >" + cell.Text + "</a>";

                        cell.CssClass = "customIcon";
                        cell.Text = cell.Text;
                    }
                }
            }
        }
    }

    protected void gvMeasureResult_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
        gvMeasureResult.MasterTableView.Columns.Clear();

        DataSet ds = PhoenixDashboardTechnical.DashboardMeasureResult("CREWLIST"
             , General.GetNullableGuid(hdnMeasureid.Value)
             , General.GetNullableInteger(hdnVesselid.Value)
             , null
             , null
             , gvMeasureResult.CurrentPageIndex + 1
             , gvMeasureResult.PageSize
             , ref iRowCount
             , ref iTotalPageCount);

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

            gvMeasureResult.DataSource = ds;
            gvMeasureResult.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
        }
        else
        {

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
            if (nvc["MeasureCode"] == "CREW-CREWLIST-CRS")
            {
                string familyid = item["FLDFAMILYID"].Text.ToString();
                nvc["SelectedModuleScreen"] = "../Crew/CrewFamilyNok.aspx?familyid=" + familyid.ToString();
                PhoenixDashboardOption.DashboardLastSelected(nvc);
                Filter.CurrentDashboardLastSelection = nvc;
                Response.Redirect("../Crew/CrewFamilyNok.aspx?familyid=" + familyid.ToString());
            }
            else
            {
                string employeeid = item["FLDEMPLOYEEID"].Text.ToString();
                nvc["SelectedModuleScreen"] = "../Crew/CrewPersonalGeneral.aspx?empid=" + employeeid.ToString();
                PhoenixDashboardOption.DashboardLastSelected(nvc);
                Filter.CurrentDashboardLastSelection = nvc;
                Response.Redirect("../Crew/CrewPersonalGeneral.aspx?empid=" + employeeid.ToString());
            }

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
        if (nvc != null)
        {
            nvc["VesselId"] = t[0];
            nvc["VesselName"] = t[1];
            nvc["MeasureId"] = t[2];
            nvc["MeasureName"] = t[3];
            nvc["MeasureCode"] = t[4];
            Filter.CurrentDashboardLastSelection = nvc;
            PhoenixDashboardOption.DashboardLastSelected(nvc);
        }
        else
        {
            NameValueCollection nvc2 = new NameValueCollection();
            nvc2.Add("VesselId", t[0]);
            nvc2.Add("VesselName", t[1]);
            nvc2.Add("MeasureId", t[2]);
            nvc2.Add("MeasureName", t[3]);
            nvc2.Add("MeasureCode", t[4]);
            Filter.CurrentDashboardLastSelection = nvc2;
            PhoenixDashboardOption.DashboardLastSelected(nvc2);
        }
        hdnMeasureid.Value = t[2];
        hdnVesselid.Value = t[0];
        lblName.Text = " [ " + t[1] + " - " + t[3] + " ] ";
        gvMeasureResult.Rebind();
        GvCrew.Rebind();
    }


    protected void gvMeasureResult_PreRender(object sender, EventArgs e)
    {
        if (gvMeasureResult.MasterTableView.Columns.FindByUniqueNameSafe("column1") != null)
            gvMeasureResult.MasterTableView.GetColumn("column1").Visible = false;
        if (gvMeasureResult.MasterTableView.Columns.FindByUniqueNameSafe("fldemployeeid") != null)
            gvMeasureResult.MasterTableView.GetColumn("fldemployeeid").Visible = false;
        if (gvMeasureResult.MasterTableView.Columns.FindByUniqueNameSafe("column2") != null)
            gvMeasureResult.MasterTableView.GetColumn("column2").Visible = false;
        if (gvMeasureResult.MasterTableView.Columns.FindByUniqueNameSafe("FLDFAMILYID") != null)
            gvMeasureResult.MasterTableView.GetColumn("FLDFAMILYID").Visible = false;

    }
    private void SelectedOption()
    {
        DataSet ds = PhoenixDashboardOption.DashboardLastSelectedEdit("CREW", "CREWLIST");
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
            nvc.Add("SelectedModuleScreen", "");

            hdnMeasureid.Value = ds.Tables[0].Rows[0]["FLDMEASUREID"].ToString();
            hdnVesselid.Value = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            lblName.Text = " [ " + ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString() + " - " + ds.Tables[0].Rows[0]["FLDMEASURENAME"].ToString() + " ] ";
        }
        else
        {
            DataSet d = PhoenixDashboardTechnical.DashboardMeasure("CREWLIST", null);
            nvc.Add("APP", "CREW");
            nvc.Add("Option", "CREWLIST");
            if (d.Tables[1].Rows.Count > 0)
            {
                nvc.Add("VesselId", d.Tables[1].Rows[0]["FLDVESSELID"].ToString());
                nvc.Add("VesselName", d.Tables[1].Rows[0]["FLDVESSELNAME"].ToString());

                lblName.Text = " [ " + d.Tables[1].Rows[0]["FLDVESSELNAME"].ToString() + " ] ";
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
        // lblName.Text = " [ " + nvc.Get("VesselName").ToString() + " - " + nvc.Get("MeasureName").ToString() + " ] ";
        // lblMeasureTitle.Text = nvc.Get("MeasureName").ToString();
        Filter.CurrentDashboardLastSelection = nvc;
        PhoenixDashboardOption.DashboardLastSelected(nvc);
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
                    if (ds.Tables[0].Rows[i]["FLDCOMMANDNAME"].ToString().ToUpper().Equals("CREW"))
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

        DataSet ds = PhoenixDashboardTechnical.DashboardMeasureResult("CREWLIST"
             , General.GetNullableGuid(nvc.Get("MeasureId").ToString())
             , General.GetNullableInteger(nvc.Get("VesselId"))
             , sortexpression
             , sortdirection
             , 1
             , iRowCount
             , ref iRowCount
             , ref iTotalPageCount);

        string[] ignoreColumns = { "FLDEMPLOYEEID", "FLDFAMILYID", "Column1", "Column2" };
        string[] alColumns = PhoenixDashboardOption.GetCaptionForExcel(ds.Tables[0], ignoreColumns);
        string[] alCaptions = alColumns;

        General.ShowExcel(gvExport.Title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }



}
