using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class DashboardTechnicalPerformance : PhoenixBasePage
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

            ViewState["MOD"] = "PERFORM";
            ViewState["MEASUREID"] = "";
            ViewState["VESSELID"] = "";

            NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
            if (nvc != null)
            {
                hdnMeasureid.Value = nvc.Get("MeasureId") != null ? nvc.Get("MeasureId").ToString() : "";
                hdnVesselid.Value = nvc.Get("VesselId") != null ? nvc.Get("VesselId").ToString() : "";
            }
        }
        BindMenu();
        exceptionReport();
    }

    protected void GvPMS_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
    {
        RadPivotGrid grid = (RadPivotGrid)sender;
        //var unused = grid.Fields["Code"] as PivotGridRowField;
        //unused.IsHidden = true;
        //unused.CellStyle=
        DataTable dt = PhoenixDashboardTechnical.DashboardOfficeTechnicalByVessel(ViewState["MOD"].ToString());
        GvPMS.DataSource = dt;

        if (dt.Rows.Count > 0)
        {
            if (ViewState["MEASUREID"].ToString() == string.Empty)
                ViewState["MEASUREID"] = dt.Rows[0]["FLDMEASUREID"].ToString();
            if (ViewState["VESSELID"].ToString() == string.Empty)
                ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
        }

    }

    protected void GvPMS_CellDataBound(object sender, Telerik.Web.UI.PivotGridCellDataBoundEventArgs e)
    {

        RadPivotGrid grid = (RadPivotGrid)sender;
        if (e.Cell is PivotGridHeaderCell)
        {
            PivotGridHeaderCell cell = (PivotGridHeaderCell)e.Cell;

            DataTable dt = (DataTable)grid.DataSource;
            DataRow[] dr = dt.Select("Vessel='" + cell.DataItem.ToString() + "'");
            if(dr.Length>0)
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
                DataTable dt = (DataTable)grid.DataSource;
                DataRow[] dr = dt.Select("FLDMEASUREID = '" + itemarray[1].ToString() + "'");
                
                if (dr[0]["Code"].ToString() == "TECH-PERF-VOC")
                    cell.Text = "<a title=\"Info\" alternatetext=\"Info\" href=\"javascript: openNewWindow('Chart', 'Voyage', 'Dashboard/DashboardVoyageInfo.html');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"fas fa-info-circle\"></i></span>" + "</a>"+cell.DataItem.ToString();
                else if (dr[0]["Code"].ToString() == "TECH-PERF-CPF")
                    cell.Text = "<a title=\"Info\" alternatetext=\"Info\" href=\"javascript: openNewWindow('Chart', 'Charter Party Performance', 'Dashboard/DashboardCpInfo.html');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"fas fa-info-circle\"></i></span>" + "</a>"+ cell.DataItem.ToString();
                else if (dr[0]["Code"].ToString() == "TECH-PERF-AEP")
                    cell.Text = "<a title=\"Info\" alternatetext=\"Info\" href=\"javascript: openNewWindow('Chart', 'A/E Performance', 'Dashboard/DashboardAePerformInfo.html');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"fas fa-info-circle\"></i></span>" + "</a>"+ cell.DataItem.ToString();
                else if (dr[0]["Code"].ToString() == "TECH-PER-COM")
                    cell.Text = "<a title=\"Info\" alternatetext=\"Info\" href=\"javascript: openNewWindow('Chart', 'Commercial Performance', 'Dashboard/DashboardCpInfo.html');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"fas fa-info-circle\"></i></span>" + "</a>"+ cell.DataItem.ToString();
                else if (dr[0]["Code"].ToString() == "TECH-PER-MPR")
                    cell.Text = "<a title=\"Info\" alternatetext=\"Info\" href=\"javascript: openNewWindow('Chart', 'Machinery Parameters', 'Dashboard/DashboardMachineryParameters.html');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"fas fa-info-circle\"></i></span>" + "</a>" + cell.DataItem.ToString();

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

                    if (d["Code"].ToString() == "TECH-PERF-VOC")
                        cell.Text = "<a title=\"Chart\" alternatetext=\"Chart\" href=\"javascript: openNewWindow('Chart', 'Voyage', 'Dashboard/DashBoardVoyageOverview.aspx?vesselid=" + d["FLDVESSELID"].ToString() + "&vesselname=" + vessel + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"far fa-chart-bar\"></i></span>" + "</a>";
                    else if (d["Code"].ToString() == "TECH-PERF-CPF")
                        cell.Text = "<a title=\"Chart\" alternatetext=\"Chart\" href=\"javascript: openNewWindow('Chart', 'Charter Party Performance', 'Dashboard/DashBoardCharterPartyPerformanceChart.aspx?vesselid=" + d["FLDVESSELID"].ToString() + "&vesselname=" + vessel + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"far fa-chart-bar\"></i></span>" + "</a>";                    
                    else if (d["Code"].ToString() == "TECH-PERF-AEP")
                        cell.Text = "<a title=\"Chart\" alternatetext=\"Chart\" href=\"javascript: openNewWindow('Chart', 'A/E Performance', 'Dashboard/DashboardTechnicalAuxiliaryPerformance.aspx?vesselid=" + d["FLDVESSELID"].ToString() + "&vesselname="+ vessel + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                       + "<span class=\"icon\"><i class=\"far fa-chart-bar\"></i></span>" + "</a>";
                    else if (d["Code"].ToString() == "TECH-PER-COM")
                        cell.Text = "<a title=\"Chart\" alternatetext=\"Chart\" href=\"javascript: openNewWindow('Chart', 'Commercial Performance', 'Dashboard/DashboardCommercialPerformanceChart.aspx?vesselid=" + d["FLDVESSELID"].ToString() + "&vesselname=" + vessel + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                       + "<span class=\"icon\"><i class=\"far fa-chart-bar\"></i></span>" + "</a>";
                    else if (d["Code"].ToString() == "TECH-PER-MPR")
                        cell.Text = "<a title=\"Chart\" alternatetext=\"Chart\" href=\"javascript: openNewWindow('Chart', 'Machinery Parameters', 'Dashboard/DashboardMachinaryPerformenceChart.aspx?vesselid=" + d["FLDVESSELID"].ToString() + "&vesselname=" + vessel + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                       + "<span class=\"icon\"><i class=\"far fa-chart-bar\"></i></span>" + "</a>";
                }
            }
        }
    }
    
    protected void GvPMS_ItemCommand(object sender, PivotGridCommandEventArgs e)
    {

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

    }

    private void SelectedOption()
    {
        DataSet ds = PhoenixDashboardOption.DashboardLastSelectedEdit("TECH", "PERFORM");
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
            DataSet d = PhoenixDashboardTechnical.DashboardMeasure("PERFORM", null);
            nvc.Add("APP", "TECH");
            nvc.Add("Option", "PERFORM");
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
        //lblMeasureTitle.Text = nvc.Get("MeasureName").ToString();
        Filter.CurrentDashboardLastSelection = nvc;
        PhoenixDashboardOption.DashboardLastSelected(nvc);
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
                    if (ds.Tables[0].Rows[i]["FLDCOMMANDNAME"].ToString().ToUpper().Equals("PERFORMANCE"))
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
                        if (ds.Tables[0].Rows[i]["FLDCOMMANDNAME"].ToString() == "OTIS")
                        {
                            img.Attributes.Add("onclick", "javascript:openNewWindow('otis','OTIS','https://system.stratumfive.com/otis/index.html'); return true;");
                        }
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void exceptionReport()
    {
        string excRpt = "EXCEPTIONRPT";
        if (SessionUtil.CanAccess(this.ViewState, excRpt.ToUpper()))
        {
            vprsAlert.Attributes["src"] = "../Dashboard/DashboardVesselPositionAlerts.aspx";
        }
        else
        {
            vprsAlert.Attributes["src"] = "";
        }
    }
}