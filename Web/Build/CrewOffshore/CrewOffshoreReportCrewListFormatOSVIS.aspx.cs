using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreReportCrewListFormatOSVIS : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"] != string.Empty)
            {
                ViewState["VesselName"] = Request.QueryString["Vesselname"];
                ViewState["vesselid"] = Request.QueryString["vesselid"];
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreReportCrewListFormatOSVIS.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
                MenuOSVISClewList.AccessRights = this.ViewState;
                MenuOSVISClewList.MenuList = toolbar.Show();
                DataSet ds = PhoenixCrewOffshoreCrewList.SearchVesselCrewList(General.GetNullableInteger(ViewState["vesselid"].ToString()));
                if (ds.Tables.Count > 0)
                {
                    ltDeck.Text = generateexcel(ds.Tables[1], ds.Tables[0]);                    
                    ltEngine.Text = generateexcel(ds.Tables[2], ds.Tables[0]);
                   
                }
            }
        }
    }
    private void ShowExcel()
    {
        DataSet ds = PhoenixCrewOffshoreCrewList.SearchVesselCrewList(General.GetNullableInteger(ViewState["vesselid"].ToString()));
        if (ds.Tables.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            int colspan = 0;
            Response.AddHeader("Content-Disposition", "attachment; filename=CrewListFormatOSVIS.xls");
            Response.ContentType = "application/vnd.msexcel";
            if (ds.Tables[1].Rows.Count > 0)
            {
                colspan = ds.Tables[1].Rows.Count;
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");//style='font-family:Calibri; font-size:11px;'
                Response.Write("<tr><td colspan='" + colspan / 2 + "'><b><left> Vessel: " + ViewState["VesselName"].ToString() + " </left></b></td>" +
                       "<td colspan='" + colspan / 2 + "'><b><left>Date: " + string.Format("{0:dd/MM/yyyy}", DateTime.Today) + "</tr>");
                Response.Write("<tr><td colspan='" + colspan + "'><b><left> </center></b></td></tr>");
                Response.Write("<tr><td colspan='" + colspan + "'><b><left><underline>" + ds.Tables[1].Rows[0]["FLDDECKTITLE"].ToString() + " </underline></left></b></td></tr>");
                Response.Write("</TABLE>");
                Response.Write("</br>");
                Response.Write(generateexcel(ds.Tables[1], ds.Tables[0]));
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                colspan = ds.Tables[2].Rows.Count;
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");//style='font-family:Calibri; font-size:11px;'
                Response.Write("<tr><td colspan='" + colspan + "'><b><left> </center></b></td></tr>");
                Response.Write("</TABLE>");
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");//style='font-family:Calibri; font-size:11px;'
                Response.Write("<tr><td colspan='" + colspan + "'><b><left> " + ds.Tables[2].Rows[0]["FLDENGINETITLE"].ToString() + " </left></b></td></tr>");
                Response.Write("</TABLE>");
                Response.Write("</br>");
                Response.Write(generateexcel(ds.Tables[2], ds.Tables[0]));
            }
            Response.End();
        }
    }
    protected void MenuOSVISClewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
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
    private string generateexcel(DataTable dtCol, DataTable dtRow)
    {
        if (dtCol.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;overflow:auto;width:100%;border-collapse:collapse;\"> ");
            int icount = 0;
            if (dtRow.Rows.Count > 0)
            {
                foreach (DataRow drrow in dtRow.Rows)
                {
                    icount++;
                    if (icount == 1)
                        sb.Append("<tr class=\"DataGrid-HeaderStyle\"><td align='left' style=\"font-weight:bold;\"><b>" + drrow["FLDROWHEADER"].ToString() + "</b></td>");
                    else
                        sb.Append("<tr><td align='left' style=\"font-weight:bold;\"><b>" + drrow["FLDROWHEADER"].ToString() + "</b></td>");
                    string srowname = drrow["FLDROWNAME"].ToString();
                    foreach (DataRow dr in dtCol.Rows)
                    {
                        sb.Append("<td align='CENTER' style=\"font-weight:lighter;\"><b>");
                        sb.Append(dr[srowname].ToString());
                        sb.Append("</b></td>");
                    }
                    sb.Append("</tr>");
                }
            }
            return sb.ToString();
        }
        else
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");

            sb.Append("<tr style=\"height:10px;\" class=\"DataGrid-HeaderStyle\"><td style=\"height:15px;\"></td></tr>");
            sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

            sb.Append("</table>");

            return sb.ToString();
        }
    }
}
