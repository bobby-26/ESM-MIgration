using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class DashboardCrewLegal : PhoenixBasePage
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

            ViewState["MOD"] = "CREWLEGAL";
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
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardCrewLegal.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
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
                        cell.CssClass = "customIcon";
                        cell.Text = cell.Text;
                    }
                }
            }
        }
        if (e.Cell is PivotGridHeaderCell)
        {
            PivotGridHeaderCell cell = (PivotGridHeaderCell)e.Cell;

            DataTable dt = (DataTable)grid.DataSource;
            DataRow[] dr = dt.Select("Vessel='" + cell.DataItem.ToString() + "'");

            if (dr.Length > 0)
                cell.Attributes.Add("onclick", "javascript: openNewWindow('codehelp1', '', 'Dashboard/DashboardVesselDetails.aspx?vesselid=" + dr[0]["FLDVESSELID"].ToString() + "'); return false;");

        }

    }

    protected void gvMeasureResult_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
        gvMeasureResult.MasterTableView.Columns.Clear();

        DataSet ds = PhoenixDashboardTechnical.DashboardMeasureResult("CREWLEGAL"
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
            nvc["SelectedModuleScreen"] = "../Inspection/InspectionPNIOperation.aspx?PNIID=" + item["FLDPNIMEDICALCASEID"].Text.ToString();
            PhoenixDashboardOption.DashboardLastSelected(nvc);
            Filter.CurrentDashboardLastSelection = nvc;

            if (nvc != null)
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();


                PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(nvc.Get("VesselId") != null ? nvc.Get("VesselId").ToString() : "0");
                PhoenixSecurityContext.CurrentSecurityContext.VesselName = "";

                Filter.CurrentSelectedModule = "Inspection";
                
                Response.Redirect("../Inspection/InspectionPNIOperation.aspx?PNIID=" + item["FLDPNIMEDICALCASEID"].Text.ToString());
             
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
        if (gvMeasureResult.MasterTableView.Columns.FindByUniqueNameSafe("FLDEMPLOYEEID") != null)
            gvMeasureResult.MasterTableView.GetColumn("FLDEMPLOYEEID").Visible = false;
        if (gvMeasureResult.MasterTableView.Columns.FindByUniqueNameSafe("FLDPNIMEDICALCASEID") != null)
            gvMeasureResult.MasterTableView.GetColumn("FLDPNIMEDICALCASEID").Visible = false;
        if (gvMeasureResult.MasterTableView.Columns.FindByUniqueNameSafe("Claim_Status") != null)
            gvMeasureResult.MasterTableView.GetColumn("Claim_Status").Visible = false;
    }
    private void SelectedOption()
    {
        DataSet ds = PhoenixDashboardOption.DashboardLastSelectedEdit("CREW", "LGL");
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
            DataSet d = PhoenixDashboardTechnical.DashboardMeasure("CREWLEGAL", null);
            nvc.Add("APP", "CREW");
            nvc.Add("Option", "LGL");
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






    //protected void gvMeasureResult_RowDataBound(object sender, GridViewRowEventArgs e)
    //{

    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        Label lblHeader = new Label();
    //        lblHeader.ID = "lblHeader";
    //        lblHeader.Text = "Action";
    //        e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(lblHeader);

    //        e.Row.Cells[11].Visible = false;  // Emp id Column Header
    //        e.Row.Cells[12].Visible = false;  // Claim Status Column
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {

    //        e.Row.Cells[11].Visible = false;    // Emp id Column
    //        e.Row.Cells[12].Visible = false;    // Claim Status Column

    //        Label lblClaim = new Label();
    //        lblClaim.ID = "lblClaimStatus";
    //        lblClaim.Text = e.Row.Cells[12].Text;
    //        e.Row.Cells[12].Controls.Add(lblClaim);

    //        if (lblClaim.Text == "1") e.Row.CssClass = "redfont";

    //        LinkButton lnklLink = new LinkButton();
    //        lnklLink.ID = "lblResultLink";
    //        lnklLink.Text = e.Row.Cells[1].Text;
    //        lnklLink.CommandName = "SELECT";
    //        lnklLink.CommandArgument = e.Row.RowIndex.ToString();
    //        if (!SessionUtil.CanAccess(this.ViewState, lnklLink.CommandName))
    //        {
    //            Label lbl = new Label();
    //            lbl.Text = e.Row.Cells[1].Text;
    //            e.Row.Cells[1].Controls.Add(lbl);
    //        }
    //        else
    //        {
    //            e.Row.Cells[1].Controls.Add(lnklLink);

    //        }

    //        Label lblEmpId = new Label();
    //        lblEmpId.ID = "lblId";
    //        lblEmpId.Text = e.Row.Cells[11].Text;

    //        LinkButton lnkName = new LinkButton();
    //        lnkName.ID = "lblEmpName";
    //        lnkName.Text = e.Row.Cells[3].Text;
    //        lnkName.CommandName = "EMPLINK";
    //        lnkName.CommandArgument = e.Row.RowIndex.ToString();
    //        lnkName.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewListForAPeriod','','../Crew/CrewPersonalGeneral.aspx?empid=" + lblEmpId.Text + "'); return false;");
    //        if (!SessionUtil.CanAccess(this.ViewState, lnkName.CommandName))
    //        {
    //            Label lbl = new Label();
    //            lbl.Text = e.Row.Cells[3].Text;
    //            e.Row.Cells[3].Controls.Add(lbl);
    //        }
    //        else
    //        {
    //            e.Row.Cells[3].Controls.Add(lnkName);

    //        }

    //        Label lblPNICase = new Label();
    //        lblPNICase.ID = "lblPNI";
    //        lblPNICase.Text = e.Row.Cells[13].Text;

    //        LinkButton lnkClaimAmt = new LinkButton();
    //        lnkClaimAmt.ID = "lblClaimableAmt";
    //        lnkClaimAmt.Text = e.Row.Cells[10].Text;
    //        lnkClaimAmt.CommandName = "CLAIMLINK";
    //        lnkClaimAmt.CommandArgument = e.Row.RowIndex.ToString();
    //        lnkClaimAmt.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp','','../Inspection/InspectionPNITravelDocumentCheckList.aspx?PNIID=" + lblPNICase.Text + "'); return false;");
    //        if (!SessionUtil.CanAccess(this.ViewState, lnkClaimAmt.CommandName))
    //        {
    //            Label lbl = new Label();
    //            lbl.Text = e.Row.Cells[10].Text;
    //            e.Row.Cells[10].Controls.Add(lbl);
    //        }
    //        else
    //        {
    //            e.Row.Cells[10].Controls.Add(lnkClaimAmt);

    //        }

    //        ImageButton ImgEdit = new ImageButton();
    //        ImgEdit.ID = "ImgEdit";
    //        ImgEdit.ImageUrl = Session["sitepath"] + "/css/" + Session["theme"].ToString() + "/images/te_edit.png";
    //        ImgEdit.CommandName = "SELECT";
    //        ImgEdit.CommandArgument = e.Row.RowIndex.ToString();
    //        e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(ImgEdit);

    //        //if (lnklLink != null) lnklLink.Visible = SessionUtil.CanAccess(this.ViewState, lnklLink.CommandName);
    //        if (ImgEdit != null) ImgEdit.Visible = SessionUtil.CanAccess(this.ViewState, ImgEdit.CommandName);
    //    }
    //}
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
                    if (ds.Tables[0].Rows[i]["FLDCOMMANDNAME"].ToString().ToUpper().Equals("CLP"))
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
                    Response.Redirect("../Dashboard/DashboardCrewAccounts.aspx");
                }
                if (cmdName.ToUpper().Equals("INVOICE"))
                {
                    Response.Redirect("../Dashboard/DashboardCrewOnboardDocumentExpiry.aspx");
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

		DataSet ds = PhoenixDashboardTechnical.DashboardMeasureResult("CREWLEGAL"
			 , General.GetNullableGuid(nvc.Get("MeasureId").ToString())
			 , General.GetNullableInteger(nvc.Get("VesselId"))
			 , sortexpression
			 , sortdirection
			 , 1
			 , iRowCount
			 , ref iRowCount
			 , ref iTotalPageCount);

		string[] ignoreColumns = { "FLDEMPLOYEEID", "FLDPNIMEDICALCASEID", "Column1" };
		string[] alColumns = PhoenixDashboardOption.GetCaptionForExcel(ds.Tables[0], ignoreColumns);
		string[] alCaptions = alColumns;

		General.ShowExcel(gvExport.Title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
	}
}
