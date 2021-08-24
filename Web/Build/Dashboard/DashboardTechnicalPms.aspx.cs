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
using System.Collections.Generic;
using Telerik.Web.UI;

public partial class DashboardTechnicalPms : PhoenixBasePage
{
		
	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
		try
		{
			cmdHiddenSubmit.Attributes.Add("style", "display:none");
			ViewState["Edit"] = "0";
			if (!IsPostBack)
			{
				SelectedOption();
				//DisplayMeasureChart(); 
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				crwid.Visible = PhoenixDashboardOption.TabExists(2);
				ViewState["MOD"] = "PMS";
			}
			PhoenixToolbar toolbargrid = new PhoenixToolbar();
			toolbargrid.AddImageButton("../Dashboard/DashboardTechnicalPms.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
			DashboardExport.AccessRights = this.ViewState;
			DashboardExport.MenuList = toolbargrid.Show();
			BindMenu();
			//SelectedOption();
			BindMeasure();

			if (!IsPostBack)
			{
				int row = int.Parse(Filter.CurrentDashboardLastSelection["Row"]);
				int col = int.Parse(Filter.CurrentDashboardLastSelection["Col"]);
				if (row != 0 || col != 0)
				{
					//gvMeasure.SelectedIndex = row;
					//gvMeasure.Rows[row].Cells[col].BackColor = System.Drawing.Color.LightGray;
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
		DataSet ds = PhoenixDashboardOption.DashboardLastSelectedEdit("TECH", "PMS");
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
			DataSet d = PhoenixDashboardTechnical.DashboardMeasure("PMS", null);
			nvc.Add("APP", "TECH");
			nvc.Add("Option", "PMS");
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
		//lblMeasureTitle.Text = nvc.Get("MeasureName").ToString();
		Filter.CurrentDashboardLastSelection = nvc;
		PhoenixDashboardOption.DashboardLastSelected(nvc);
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
			//gvMeasure.DataSource = ds;
			//gvMeasure.DataBind();
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
				string argsp = measureid + "~" + measurename + "~" + showdetail;
				cmdGraph.CommandArgument = argsp;
			}
			ImageButton cmdColor = (ImageButton)e.Row.FindControl("cmdColor");
			if (cmdColor != null)
			{
				if (!SessionUtil.CanAccess(this.ViewState, cmdColor.CommandName))
				{
					cmdColor.Visible = false;
				}

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
					DataRow[] dr = data.Select("FLDMEASUREID = '" + measureid + "' AND FLDVESSELID = " + header.Rows[i]["FLDVESSELID"].ToString());
					if (dr.Length > 0)
					{

						//string args = e.Row.RowIndex.ToString() + "~" + (i + 3) + "~" + dr[0]["FLDVESSELID"].ToString() + "~" + dr[0]["FLDVESSELNAME"].ToString() + "~" + measureid + "~" + measurename + "~" + measurecode;
						//Label lb = new Label();
						//lb.Text = dr[0]["FLDMEASURE"].ToString();
						//e.Row.Cells[i + 3].Controls.Add(lb);
						e.Row.Cells[i + 3].Text = dr[0]["FLDMEASURE"].ToString();
						e.Row.Cells[i + 3].CssClass = "customIcon";

					}
					else
					{
						e.Row.Cells[i + 3].Text = "-";
						e.Row.Cells[i + 3].CssClass = "customIcon";
					}
					e.Row.Enabled = false;
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
				string measurecode = args[6];

				NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
				nvc["Row"] = row;
				nvc["Col"] = column;
				nvc["VesselId"] = vesselid;
				nvc["VesselName"] = vesselname;
				nvc["MeasureId"] = measureid;
				nvc["MeasureName"] = measurename;
				nvc["MeasureCode"] = measurecode;
				Filter.CurrentDashboardLastSelection = nvc;
				PhoenixDashboardOption.DashboardLastSelected(nvc);
				lblName.Text = " [ " + vesselname + " - " + measurename + " ] ";
				//lblMeasureTitle.Text = measurename;
				ViewState["PAGENUMBER"] = 1;

				//gvMeasure.SelectedIndex = int.Parse(row);
				//gvMeasure.Rows[int.Parse(row)].Cells[int.Parse(column)].BackColor = System.Drawing.Color.LightGray;
				ScriptManager.RegisterStartupScript(Page, Page.GetType(), "gridsize", "calcW('dataMenu')", true);
			}
			
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
			gvWorkOrder.Rebind();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	
	public StateBag ReturnViewState()
	{
		return ViewState;
	}
	protected void PageAccessValidation(object sender, CommandEventArgs ce)
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
					if (ds.Tables[0].Rows[i]["FLDCOMMANDNAME"].ToString().ToUpper().Equals("PMS"))
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
							Src  = Session["sitepath"] + "/css/" + Session["theme"].ToString() + "/images/" + ds.Tables[0].Rows[i]["FLDIMAGENAME"].ToString()
						};
						div.Controls.Add(img);
					}
					
					L1.Text = "<br>" + ds.Tables[0].Rows[i]["FLDMODULENAME"].ToString();					
					div.Controls.Add(L1);
					techIcons.Controls.AddAt(i, div);

				}
			}

		}
		catch(Exception ex)
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

		DataSet ds = PhoenixDashboardTechnical.DashboardMeasureResult("PMS"
			 , General.GetNullableGuid(nvc.Get("MeasureId").ToString())
			 , General.GetNullableInteger(nvc.Get("VesselId"))
			 , sortexpression
			 , sortdirection
			 , 1
			 , General.ShowRecords(null)
			 , ref iRowCount
			 , ref iTotalPageCount);

		string[] ignoreColumns = { "FLDWORKORDERID", "FLDVESSELID", "Column1" };
		string[] alColumns = PhoenixDashboardOption.GetCaptionForExcel(ds.Tables[0], ignoreColumns);
		string[] alCaptions = alColumns;

		General.ShowExcel(lblName.Text, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
	}
	//private string[] CaptionForExcel(DataTable dt,string[] ignoreColumns)
	//{
	//	List<string> capnamelist = new List<string>();
	//	for (int i = 0; i < dt.Columns.Count; i++)
	//	{
	//		if(ignoreColumns.Length == 0 || Array.IndexOf(ignoreColumns, dt.Columns[i].ColumnName) < 0)
	//		capnamelist.Add(dt.Columns[i].ColumnName);
	//	}
	//	string[] terms = capnamelist.ToArray();
	//	return terms;
	//}


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
		catch(Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvWorkOrder_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
	{
		RadGrid grid = (RadGrid)sender;
		NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;

		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		DataSet ds = PhoenixDashboardTechnical.DashboardMeasureResult("PMS"
			 , General.GetNullableGuid(nvc.Get("MeasureId").ToString())
			 , General.GetNullableInteger(nvc.Get("VesselId"))
			 , sortexpression
			 , sortdirection
			 , (int)ViewState["PAGENUMBER"]
			 , General.ShowRecords(null)
			 , ref iRowCount
			 , ref iTotalPageCount);

		grid.DataSource = ds.Tables[0];
		grid.VirtualItemCount = iRowCount;

		if (ds.Tables.Count > 0)
		{
			if (ds.Tables[0].Rows.Count > 0)
			{
				
				//int nCurrentRow = int.Parse(nvc.Get("ResultRow") != null ? (nvc.Get("ResultRow").ToString() != "" ? nvc.Get("ResultRow").ToString() : "0") : "0");
				//if (ds.Tables[0].Rows.Count > nCurrentRow)
				//{
				//	grid.SelectedIndexes = nCurrentRow;
				//}
			}
			else
			{
				Filter.CurrentDashboardLastSelection["Row"] = "0";
				Filter.CurrentDashboardLastSelection["Col"] = "0";
			}
			ViewState["ROWCOUNT"] = iRowCount;
			ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
		}
	}

	protected void gvWorkOrder_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
	{
		if (e.Item is GridHeaderItem)
		{
			e.Item.Cells[2].Visible = false;
			e.Item.Cells[3].Visible = false;

			e.Item.Cells[e.Item.Cells.Count - 1].Text = "Action";
			e.Item.Cells[e.Item.Cells.Count - 1].HorizontalAlign = HorizontalAlign.Center;
		}
		if (e.Item is GridDataItem)
		{
			e.Item.Cells[2].Visible = false;
			e.Item.Cells[3].Visible = false;


			LinkButton lnklLink = new LinkButton
			{
				ID = "lblResultLink",
				Text = e.Item.Cells[4].Text,
				CommandName = "SELECT",
				CommandArgument = e.Item.ItemIndex.ToString()
			};
			lnklLink.Attributes["onclick"] = "FireCommand('SELECT'," + e.Item.ItemIndex.ToString() + "); return false;";
			if (!SessionUtil.CanAccess(this.ViewState, lnklLink.CommandName))
			{
				Label lbl = new Label
				{
					Text = e.Item.Cells[4].Text
				};
				e.Item.Cells[4].Controls.Add(lbl);
			}
			else
			{
				e.Item.Cells[4].Controls.Add(lnklLink);

			}

			ImageButton ImgEdit = new ImageButton
			{
				ID = "ImgEdit",
				ImageUrl = Session["sitepath"] + "/css/" + Session["theme"].ToString() + "/images/te_edit.png",
				CommandName = "SELECT",
				CommandArgument = e.Item.RowIndex.ToString()
			};
			e.Item.Cells[e.Item.Cells.Count - 1].Controls.Add(ImgEdit);
			e.Item.Cells[e.Item.Cells.Count - 1].HorizontalAlign = HorizontalAlign.Center;
			// if (lnklLink != null) lnklLink.Visible = SessionUtil.CanAccess(this.ViewState, lnklLink.CommandName);
			if (ImgEdit != null) ImgEdit.Visible = SessionUtil.CanAccess(this.ViewState, ImgEdit.CommandName);
		}
	}

	protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
	{
		try
		{
			RadGrid _gridView = (RadGrid)sender;			
			if (e.CommandName.ToUpper().Equals("SELECT"))
			{
				int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
				NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
				nvc["ResultRow"] = nCurrentRow.ToString();
				if (nvc["SelectedModuleScreen"] == null)
					nvc["SelectedModuleScreen"] = "";
				if (nvc["MeasureCode"] == "TECH-PMS-PPA")
				{
					string workordernumber = _gridView.Items[nCurrentRow].Cells[4].Text;
					nvc["SelectedModuleScreen"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderPostponedApproval.aspx?workordernumber=" + workordernumber;
				}
				else if (nvc["MeasureCode"] == "TECH-PMS-CNU")
				{
					nvc["SelectedModuleScreen"] = "../PlannedMaintenance/PlannedMaintenanceCounterUpdate.aspx";
				}
				else
				{
					nvc["SelectedModuleScreen"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrder.aspx";
					//PhoenixDashboardOption.DashboardLastSelected(nvc);
					Filter.CurrentDashboardLastSelection = nvc;
					if (nvc != null)
					{
						NameValueCollection criteria = new NameValueCollection();
						criteria.Clear();
						criteria.Add("txtWorkOrderNumber", _gridView.Items[nCurrentRow].Cells[4].Text);
						criteria.Add("txtWorkOrderName", string.Empty);
						criteria.Add("txtComponentNumber", string.Empty);
						criteria.Add("txtComponentName", string.Empty);
						criteria.Add("ucRank", string.Empty);
						criteria.Add("txtDateFrom", string.Empty);
						criteria.Add("txtDateTo", string.Empty);
						if (nvc["MeasureName"] == "CANCELLED")
							criteria.Add("status", "25");
						else
							criteria.Add("status", string.Empty);
						criteria.Add("planning", string.Empty);
						criteria.Add("jobclass", string.Empty);
						criteria.Add("ucMainType", string.Empty);
						criteria.Add("ucMainCause", string.Empty);
						criteria.Add("ucMaintClass", string.Empty);
						criteria.Add("chkUnexpected", string.Empty);
						criteria.Add("txtPriority", string.Empty);
						criteria.Add("chkDefect", string.Empty);
						criteria.Add("txtClassCode", string.Empty);
						Filter.CurrentWorkOrderFilter = criteria;

					}
				}
				PhoenixDashboardOption.DashboardLastSelected(nvc);
				PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(nvc.Get("VesselId") != null ? nvc.Get("VesselId").ToString() : "0");
				PhoenixSecurityContext.CurrentSecurityContext.VesselName = nvc.Get("VesselName") != null ? nvc.Get("VesselName").ToString() : "";

				Filter.CurrentSelectedModule = "PlannedMaintenance";

				Response.Redirect(nvc["SelectedModuleScreen"].ToString(), true);
			}			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void GvMeasure_NeedDataSource(object sender, PivotGridNeedDataSourceEventArgs e)
	{
		RadPivotGrid pivot = (RadPivotGrid)sender;
		NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
		DataTable dt = PhoenixDashboardTechnical.DashboardOfficeTechnicalByVessel("PMS");
		pivot.DataSource = dt;
	}

	protected void GvMeasure_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
	{
		RadPivotGrid grid = (RadPivotGrid)sender;
		DataTable dt = (DataTable)grid.DataSource;
		if (e.Cell is PivotGridHeaderCell)
		{
			PivotGridHeaderCell cell = (PivotGridHeaderCell)e.Cell;
			DataRow[] dr = dt.Select("Vessel='" + cell.DataItem.ToString() + "'");

			if (dr.Length > 0)
				cell.Attributes.Add("onclick", "javascript: openNewWindow('codehelp1', '', 'Dashboard/DashboardVesselDetails.aspx?vesselid=" + dr[0]["FLDVESSELID"].ToString() + "'); return false;");

		}
		if (e.Cell is PivotGridRowHeaderCell)
		{
			PivotGridRowHeaderCell cell = (PivotGridRowHeaderCell)e.Cell;
			PivotGridRowHeaderItem item = (PivotGridRowHeaderItem)e.Cell.DataItemContainer;
			string row = cell.ParentIndexes.Length > 0 ? cell.ParentIndexes[0].ToString() : string.Empty;
			System.Collections.ArrayList itemarray = (System.Collections.ArrayList)item.DataItem;
			if (itemarray.Count > 1)
			{
				
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
				string code = string.Empty;
				//string measure = string.Empty;                
				string measureid = cell.ParentRowIndexes.Length > 1 ? cell.ParentRowIndexes[1].ToString() : string.Empty;
				string vessel = cell.ParentColumnIndexes.Length > 0 ? cell.ParentColumnIndexes[0].ToString() : string.Empty;
				//if (row.IndexOf('~') > -1)
				//{
				//    string[] arr = row.Split('~');
				//    code = arr[0].Trim();
				//    measure = arr[1].Trim();
				//}
				DataRow[] dr = dt.Select("FLDMEASUREID = '" + measureid + "' AND Vessel='" + vessel + "'");
				//string employees = string.Empty;
				foreach (DataRow d in dr)
				{
					string text = cell.Text;
					cell.HorizontalAlign = HorizontalAlign.Right;
					cell.CssClass = "label";

					string args = d["FLDVESSELID"].ToString() + "~" + vessel + "~" + measureid + "~" + d["Measure"].ToString() + "~" + d["Code"].ToString();

					cell.Text = "<a style=\"background-color: " + (d["FLDCOLOR"].ToString().Equals("") ? "#27727B" : d["FLDCOLOR"].ToString()) + ";\" class=\"mlabel\" onclick=\"BindMeasureResult('" + measureid + "', '" + d["FLDVESSELID"].ToString() + "','" + args + "');\" >" + cell.Text + "</a>";

					//LinkButton lbl = new LinkButton();
					//lbl.ID = "lbl_" + cell.ColumnIndex + "_";
					//lbl.Text = d["FLDMEASURE"].ToString();
					//lbl.CommandName = "SELECT";
					//lbl.CommandArgument = args;
					//lbl.Attributes.Add("class", "mlabel");
					//lbl.BackColor = System.Drawing.Color.FromName(d["FLDCOLOR"].ToString().Equals("") ? "#27727B" : d["FLDCOLOR"].ToString());
					//cell.Controls.Add(lbl);
				}
			}
		}
	}

	protected void GvMeasure_ItemCommand(object sender, PivotGridCommandEventArgs e)
	{
		if (e.CommandName.ToUpper().Equals("SELECT"))
		{
			
		}
	}

	protected void gvWorkOrder_PreRender(object sender, EventArgs e)
	{
		RadGrid grid = (RadGrid)sender;
		//grid.Rebind();
	}

	protected void hdnButton_Click(object sender, EventArgs e)
	{
		NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
		string[] t = args.Value.ToString().Split('~');
		if (nvc == null)
		{
			nvc = new NameValueCollection();
		}
		nvc["VesselId"] = t[0];
		nvc["VesselName"] = t[1];
		nvc["MeasureId"] = t[2];
		nvc["MeasureName"] = t[3];
		nvc["MeasureCode"] = t[4];
		Filter.CurrentDashboardLastSelection = nvc;
		PhoenixDashboardOption.DashboardLastSelected(nvc);
		lblName.Text = " [ " + t[1] + " - " + t[3] + " ] ";
		GvMeasure.Rebind();
		gvWorkOrder.Rebind();
	}
}
