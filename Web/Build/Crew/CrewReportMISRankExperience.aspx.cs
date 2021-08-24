using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportMISRankExperience : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMISRankExperience.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMISRankExperience.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();
            lnkDetails.Visible = false;
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                rblSelection.SelectedIndex = 0;

            }
            lnkDetails.Attributes.Add("onclick", "javascript:openNewWindow('Crew','','" + Session["sitepath"] + "/Crew/CrewReportMISRankExperienceDetails.aspx?nationality=" + ucNationality.SelectedList + "&status=" + ddlSelect.SelectedHard + "&condition=" + rblSelection.SelectedValue + "&exp=" + txtExperience.Text + "'); return false;");
            // ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["SHOWREPORT"] = null;
                ucManager.SelectedAddress = "";
                ucNationality.SelectedNationalityValue = "";
                ucRank.SelectedRankValue = "";
                ddlSelect.SelectedHard = "";
                rblSelection.SelectedIndex = 0;
                txtExperience.Text = "0";
                lnkDetails.Visible = false;
                ucPrincipal.SelectedAddress = "";
                ShowReport();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ddlSelect.SelectedHard.ToString(),txtExperience.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    LinkButton lbr= new LinkButton();
                    lbr=lnkDetails;
                    string principal = ucPrincipal.SelectedAddress.ToString();
                    lbr.Attributes.Add("onclick", "javascript:openNewWindow('Crew','','" + Session["sitepath"] + "/Crew/CrewReportMISRankExperienceDetails.aspx?nationality=" + ucNationality.SelectedList + "&status=" + ddlSelect.SelectedHard + "&condition=" + rblSelection.SelectedValue + "&exp=" + txtExperience.Text + "&principal=" + principal.ToString() + "&pool=" + ucPool.SelectedPool + "'); return false;");
                    ShowReport();                    
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();

        ds = PhoenixCrewReportMIS.CrewReportMISRankAnalysis(
                (ucManager.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucManager.SelectedAddress.ToString()),
                General.GetNullableString(ucRank.selectedlist.Replace("Dummy", "").TrimStart(',')),
                (ucNationality.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList.ToString()),
                (ddlSelect.SelectedHard.ToString()) == "Dummy" ? null : General.GetNullableInteger(ddlSelect.SelectedHard.ToString()),
                General.GetNullableInteger(rblSelection.SelectedValue.ToString()),
                General.GetNullableInteger(txtExperience.Text),
                (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucPrincipal.SelectedAddress.ToString()),
                 ucPool.SelectedPool.Replace("Dummy", "").TrimStart(','));

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt2 = ds.Tables[2];
                DataTable dtTempHeader = ds.Tables[0];
                DataRow[] drvHeader = dtTempHeader.Select("FLDROWID = " + dt2.Rows[0]["FLDROWID"].ToString());
                int alColumns = 1;
                foreach (DataRow drTempHeader in drvHeader)
                {
                    alColumns++;
                }
                string s;
                s = "Rank Experience for " + (ddlSelect.SelectedHard == "220" ? "Onboard" : (ddlSelect.SelectedHard == "221" ? "Onleave" : ""))
                    + " staff - " + (rblSelection.SelectedValue == "1" ? "greater than " : (rblSelection.SelectedValue == "2" ? "less than " : ""))
                    + txtExperience.Text + " month(s)";

                Response.AddHeader("Content-Disposition", "attachment; filename=MIS_Rank_Experience_Report.xls");
                Response.ContentType = "application/vnd.msexcel";
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
                Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns).ToString() + "'><h5><center>" + s + "</center></h5></td></tr>");
                Response.Write("<tr><td style='font-family:Arial; font-size:10px;' align='left'><b>Date:" + date + "</b></td></tr>");
                Response.Write("<tr><td colspan='" + (alColumns).ToString() + "'>&nbsp;</td>");
                Response.Write("</tr>");
                Response.Write("</TABLE>");
                //Response.Write("<br />");
                Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");

                StringBuilder sb = new StringBuilder();               

                //Printing the Header

                DataTable dtHeader = ds.Tables[4];

                sb.Append("<tr><td><b>Manager</b></td>");

                foreach (DataRow dr in dtHeader.Rows)
                {
                    sb.Append("<td align='right'><b>");
                    sb.Append(dr["FLDRANKCODE"].ToString());
                    sb.Append("</b></td>");
                }

                sb.Append("<td align='right'>Total</td></tr>");

                //Printing the Data
                DataTable dtManager = ds.Tables[2];
                DataTable dtData = ds.Tables[0];

                foreach (DataRow dr in dtManager.Rows)
                {
                    sb.Append("<tr><td align='left'>" + dr["FLDNAME"].ToString() + "</td>");
                    foreach (DataRow dr1 in dtHeader.Rows)
                    {
                        DataRow[] drv = dtData.Select("FLDROWID = " + dr["FLDROWID"].ToString() + " AND FLDCOLUMNID = " + dr1["FLDCOLUMNID"].ToString());
                        if (drv.Length > 0)
                            sb.Append("<td align='right'>" + drv[0]["FLDNOOFSTAFFS"].ToString() + "</td>");
                        else
                            sb.Append("<td align='right'>0</td>");
                    }
                    sb.Append("<td align='right'><b>" + dr["FLDROWTOTAL"].ToString() + "</b></td></tr>");
                }

                string str;
                str = "Total Staff " + (ddlSelect.SelectedHard == "220" ? "Onboard" : (ddlSelect.SelectedHard == "221" ? "Onleave" : ""));

                sb.Append("<tr><td align='left'><b>" + str + "</b></td>");
                int totalstaff = 0;
                foreach (DataRow drTemp in dtHeader.Rows)
                {
                    sb.Append("<td align='right'><b>");
                    sb.Append(drTemp["FLDCOLUMNTOTAL"].ToString());
                    sb.Append("</b></td>");
                    totalstaff += (int)drTemp["FLDCOLUMNTOTAL"];
                }

                sb.Append("<td align='right'><b>" + totalstaff.ToString() + "</b></td></tr>");

                string st;
                st = "Staff " + (ddlSelect.SelectedHard == "220" ? "Onboard" : (ddlSelect.SelectedHard == "221" ? "Onleave" : "")) + " with exp. "
                    + (rblSelection.SelectedValue == "1" ? "greater than " : (rblSelection.SelectedValue == "2" ? "less than " : ""))
                    + txtExperience.Text + " month(s)";

                sb.Append("<tr><td align='left'><b>" + st + "</b></td>");
                float totexpstaff = 0;
                foreach (DataRow dr1 in dtHeader.Rows)
                {
                    DataRow[] drv = ds.Tables[1].Select("FLDCOLUMNID = " + dr1["FLDCOLUMNID"].ToString());
                    if (drv.Length > 0)
                    {
                        totexpstaff += float.Parse(drv[0]["FLDCOLUMNTOTAL"].ToString());
                        sb.Append("<td align='right'>" + drv[0]["FLDCOLUMNTOTAL"].ToString() + "</td>");
                    }
                    else
                        sb.Append("<td align='right'>0</td>");
                }

                sb.Append("<td align='right'><b>" + totexpstaff.ToString() + "</td></tr>");

                sb.Append("<tr><td align='left'><b>Rankwise Percentage</b></td>");
                foreach (DataRow dr1 in dtHeader.Rows)
                {
                    DataRow[] drv = ds.Tables[3].Select("FLDTOTALSTAFFCOLUMNID = " + dr1["FLDCOLUMNID"].ToString());
                    if (drv.Length > 0)
                    {
                        sb.Append("<td align='right'><b>" + drv[0]["FLDPERCENTAGE"].ToString() + "</b></td>");
                    }
                    else
                        sb.Append("<td align='right'><b>0.O</b></td>");
                }
                float per = (totexpstaff / totalstaff) * 100;
                sb.Append("<td align='right'><b>" + per.ToString() + "</td></tr>");

                sb.Append("</table>");

                Response.Write(sb.ToString());
                Response.End();
            }
        }
    }
    private void ShowReport()
    {
        ViewState["SHOWREPORT"] = 1;
        DataSet ds = new DataSet();
        ds = PhoenixCrewReportMIS.CrewReportMISRankAnalysis(
                (ucManager.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucManager.SelectedAddress.ToString()),
                General.GetNullableString(ucRank.selectedlist.Replace("Dummy", "").TrimStart(',')),
                (ucNationality.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList.ToString()),
                (ddlSelect.SelectedHard.ToString()) == "Dummy" ? null : General.GetNullableInteger(ddlSelect.SelectedHard.ToString()),
                General.GetNullableInteger(rblSelection.SelectedValue.ToString()),
                General.GetNullableInteger(txtExperience.Text),
                (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucPrincipal.SelectedAddress.ToString()),                 
                ucPool.SelectedPool.Replace("Dummy", "").TrimStart(','));


        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {                                
                StringBuilder sb = new StringBuilder();

                sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");

                //Printing the Header

                DataTable dtHeader = ds.Tables[4];                

                sb.Append("<tr class=\"DataGrid-HeaderStyle\"><td>Manager</td>");

                foreach (DataRow dr in dtHeader.Rows)
                {
                    sb.Append("<td align='right'>");
                    sb.Append(dr["FLDRANKCODE"].ToString());
                    sb.Append("</td>");
                }

                sb.Append("<td align='right'>Total</td></tr>");

                //Printing the Data
                DataTable dtManager = ds.Tables[2];
                DataTable dtData = ds.Tables[0];

                foreach (DataRow dr in dtManager.Rows)
                {
                    sb.Append("<tr style=\"height:10px;\" ><td align='left'>" + dr["FLDNAME"].ToString() + "</td>");
                    foreach (DataRow dr1 in dtHeader.Rows)
                    {
                        DataRow[] drv = dtData.Select("FLDROWID = " + dr["FLDROWID"].ToString() + " AND FLDCOLUMNID = " + dr1["FLDCOLUMNID"].ToString());
                        if(drv.Length> 0)
                            sb.Append("<td align='right'>" + drv[0]["FLDNOOFSTAFFS"].ToString() + "</td>");
                        else
                            sb.Append("<td align='right'>0</td>");
                    }                  
                    sb.Append("<td align='right'><b>" + dr["FLDROWTOTAL"].ToString() + "</b></td></tr>");
                }

                string str;
                str = "Total Staff " + (ddlSelect.SelectedHard == "220" ? "Onboard" : (ddlSelect.SelectedHard == "221" ? "Onleave" : ""));

                sb.Append("<tr><td align='left'><b>" + str + "</b></td>");
                int totalstaff = 0;
                foreach (DataRow drTemp in dtHeader.Rows)
                {
                    sb.Append("<td align='right'><b>");
                    sb.Append(drTemp["FLDCOLUMNTOTAL"].ToString());
                    sb.Append("</b></td>");
                    totalstaff += (int)drTemp["FLDCOLUMNTOTAL"];
                }

                sb.Append("<td align='right'><b>" + totalstaff.ToString() + "</b></td></tr>");

                string st;
                st = "Staff " + (ddlSelect.SelectedHard == "220" ? "Onboard" : (ddlSelect.SelectedHard == "221" ? "Onleave" : "")) + " with exp. "
                    + (rblSelection.SelectedValue == "1" ? "greater than " : (rblSelection.SelectedValue == "2" ? "less than " : ""))
                    + txtExperience.Text + " month(s)";

                sb.Append("<tr><td align='left'><b>" + st + "</b></td>");
                float totexpstaff = 0;
                foreach (DataRow dr1 in dtHeader.Rows)
                {
                    DataRow[] drv = ds.Tables[1].Select("FLDCOLUMNID = " + dr1["FLDCOLUMNID"].ToString());
                    if (drv.Length > 0)
                    {
                        totexpstaff += float.Parse(drv[0]["FLDCOLUMNTOTAL"].ToString());
                        sb.Append("<td align='right'>" + drv[0]["FLDCOLUMNTOTAL"].ToString() + "</td>");
                    }
                    else
                        sb.Append("<td align='right'>0</td>");
                }

                sb.Append("<td align='right'><b>" + totexpstaff.ToString() + "</td></tr>");

                sb.Append("<tr><td align='left'><b>Rankwise Percentage</b></td>");
                foreach (DataRow dr1 in dtHeader.Rows)
                {
                    DataRow[] drv = ds.Tables[3].Select("FLDTOTALSTAFFCOLUMNID = " + dr1["FLDCOLUMNID"].ToString());                    
                    if (drv.Length > 0)
                    {
                        sb.Append("<td align='right'><b>" + drv[0]["FLDPERCENTAGE"].ToString() + "</b></td>");
                    }
                    else
                        sb.Append("<td align='right'><b>0.O</b></td>");
                }
                float per = (totexpstaff / totalstaff) * 100;
                sb.Append("<td align='right'><b>" + per.ToString() + "</td></tr>");       

                sb.Append("</table>");

                ltGrid.Text = sb.ToString();
                lnkDetails.Visible = true;
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"2\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");

                sb.Append("<tr style=\"height:10px;\" class=\"DataGrid-HeaderStyle\"><td style=\"height:15px;\"></td></tr>");
                sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

                sb.Append("</table>");

                ltGrid.Text = sb.ToString();
            }
        }
    }
    public bool IsValidFilter(string select,string exp)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (select.Equals("") || select.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Select Status";
        }
        if (exp.Equals(""))
        {
            ucError.ErrorMessage = "Enter Experience in Months";
        }
        
        return (!ucError.IsError);
    }
}
