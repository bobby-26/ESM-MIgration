using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Export2XL;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;

public partial class InspectionNCReportByAuditSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindCriteria();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageLink("../Inspection/InspectionNCReportByAuditSummary.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbar.AddImageLink("javascript:CallPrint('gvCNC')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Inspection/InspectionNCReportByAuditSummary.aspx", "Search", "search.png", "FIND");
            toolbar.AddImageButton("../Inspection/InspectionNCReportByAuditSummary.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuAuditSummaryNC.AccessRights = this.ViewState;
            MenuAuditSummaryNC.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Year wise NC Trend", "YEARWISENC");
            toolbar.AddButton("By Category", "CATEGORYWISENC");
            toolbar.AddButton("By Vessel Type", "VESSELTYPEWISENC");
            toolbar.AddButton("By Category & Vessel Type", "BREAKUPWISENC");
            toolbar.AddButton("ISM/ISPS Summary", "AUDITSUMMARY");

            MenuAuditSummaryNCGeneral.AccessRights = this.ViewState;
            MenuAuditSummaryNCGeneral.MenuList = toolbar.Show();
            MenuAuditSummaryNCGeneral.SelectedMenuIndex = 4;
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void AssginFilter()
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("ucFromMonth", ucFromMonth.SelectedHard);
        criteria.Add("ucToMonth", ucToMonth.SelectedHard);
        criteria.Add("ddlFromYear", ddlFromYear.SelectedQuick);
        criteria.Add("ddlToYear", ddlToYear.SelectedQuick);
        Filter.CurrentPeriodFilterForNC = criteria;
    }
    protected void BindCriteria()
    {
        NameValueCollection nvc = Filter.CurrentPeriodFilterForNC;
        if (nvc != null)
        {
            ucFromMonth.SelectedHard = (nvc != null ? nvc["ucFromMonth"] : null);
            ucToMonth.SelectedHard = (nvc != null ? nvc["ucToMonth"] : null);
            ddlFromYear.SelectedQuick = (nvc != null ? nvc["ddlFromYear"] : null);
            ddlToYear.SelectedQuick = (nvc != null ? nvc["ddlToYear"] : null);
        }
    }


    private void BindData()
    {

        DataSet ds = new DataSet();
        NameValueCollection nvc = Filter.CurrentPeriodFilterForNC;

        ds = PhoenixInspectionReports.ListhNCSummaryByAudit(General.GetNullableInteger(nvc != null ? nvc["ucFromMonth"] : null)
                                                            , General.GetNullableInteger(nvc != null ? nvc["ucToMonth"] : null)
                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlFromYear"] : null)
                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlToYear"] : null)
                                                           );

        if (ds.Tables[1].Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt1 = ds.Tables[1];
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");

            DataTable dtTempHeader = ds.Tables[1];
            DataRow[] drvHeader = dtTempHeader.Select("FLDROWID = " + dt1.Rows[0]["FLDROWID"].ToString());
            sb.Append("<tr class=\"DataGrid-HeaderStyle\"><td align='CENTER'><b>Type of Audit</b></td>");
            sb.Append("<td  colspan ='4' align='CENTER'><b>NCs in ISM audits </b></td>");
            sb.Append("</tr> <tr><td></td>");

            foreach (DataRow drTempHeader in drvHeader)
            {
                sb.Append("<td colspan='2' align='CENTER'><b>");
                sb.Append(drTempHeader["FLDCOLUMNHEADER"].ToString());
                sb.Append("</b></td>");
            }

            sb.Append("</tr>");


            //ISM DATA
            DataTable dt3 = ds.Tables[0];
            foreach (DataRow dr1 in dt3.Rows)
            {
                DataTable dtTemp = ds.Tables[1];

                DataRow[] drv = dtTemp.Select("FLDROWID = " + dr1["FLDROWID"].ToString());

                sb.Append("<tr><td align='left'><b>" + drv[0]["FLDROWHEADER"].ToString() + "</b></td>");

                foreach (DataRow drTemp in drv)
                {
                    sb.Append("<td align='center'>");
                    sb.Append(drTemp["FLDNCCOUNT"].ToString() + " in " + drTemp["FLDTOTALAUDITPERCATEGORY"].ToString() + " audits");
                    sb.Append("</td>");
                    sb.Append("<td  align='center'>");
                    sb.Append(drTemp["FLDAVGNCPERCATEGORY"].ToString() + "/ audit");
                    sb.Append("</td>");
                }
                sb.Append("</tr>");

            }
            sb.Append("<tr><td colspan='5' style='height: 10px;'></td></tr> </tr>");
            if (ds.Tables[1].Rows.Count > 0)
            {
                sb.Append("<tr class=\"DataGrid-HeaderStyle\"><td align='CENTER'><b>Type of Audit</b></td>");
                sb.Append("<td  colspan ='4' align='CENTER'><b>NCs in ISPS audits </b></td>");
                sb.Append("</tr> <tr><td></td>");

                foreach (DataRow drTempHeader in drvHeader)
                {
                    sb.Append("<td colspan='2' align='CENTER'><b>");
                    sb.Append(drTempHeader["FLDCOLUMNHEADER"].ToString());
                    sb.Append("</b></td>");
                }

                sb.Append("</tr>");


                //ISPS DATA
                dt3 = ds.Tables[0];
                foreach (DataRow dr1 in dt3.Rows)
                {
                    DataTable dtTemp = ds.Tables[2];

                    DataRow[] drv = dtTemp.Select("FLDROWID = " + dr1["FLDROWID"].ToString());

                    sb.Append("<tr><td align='left'><b>" + drv[0]["FLDROWHEADER"].ToString() + "</b></td>");

                    foreach (DataRow drTemp in drv)
                    {
                        sb.Append("<td align='center'>");
                        sb.Append(drTemp["FLDNCCOUNT"].ToString() + " in " + drTemp["FLDTOTALAUDITPERCATEGORY"].ToString() + " audits");
                        sb.Append("</td>");
                        sb.Append("<td  align='center'>");
                        sb.Append(drTemp["FLDAVGNCPERCATEGORY"].ToString() + "/ audit");
                        sb.Append("</td>");
                    }
                    sb.Append("</tr>");

                }

            }
            sb.Append("</table>");
            lblGrid.Text = sb.ToString();
        }

        else
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");

            sb.Append("<tr style=\"height:10px;\" class=\"DataGrid-HeaderStyle\"><td style=\"height:15px;\"></td></tr>");
            sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

            sb.Append("</table>");

            lblGrid.Text = sb.ToString();
        }


    }
    protected void MenuAuditSummaryNC_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                AssginFilter();
                BindData();

            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();

            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {

                ucFromMonth.SelectedHard = "";
                ddlFromYear.SelectedQuick = "";
                ucToMonth.SelectedHard = "";
                ddlToYear.SelectedQuick = "";

                Filter.CurrentPeriodFilterForNC = null;
                BindData();

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
        DataSet ds = new DataSet();
        NameValueCollection nvc = Filter.CurrentPeriodFilterForNC;
        ds = PhoenixInspectionReports.ListhNCSummaryByAudit(General.GetNullableInteger(ucFromMonth.SelectedHard)
                                                             , General.GetNullableInteger(ucToMonth.SelectedHard)
                                                             , General.GetNullableInteger(ddlFromYear.SelectedQuick)
                                                             , General.GetNullableInteger(ddlToYear.SelectedQuick)
                                                             );

        Response.AddHeader("Content-Disposition", "attachment; filename=ISM/ISPS AuditSummaryReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='5'><b><center>ISM / ISPS AUDIT SUMMARY </center></b></td></tr>");

        Response.Write("</TABLE>");
        StringBuilder sb = new StringBuilder();
        if (ds.Tables[1].Rows.Count > 0)
        {
            sb = new StringBuilder();
            DataTable dt1 = ds.Tables[1]; 
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" > ");

            DataTable dtTempHeader = ds.Tables[1];
            DataRow[] drvHeader = dtTempHeader.Select("FLDROWID = " + dt1.Rows[0]["FLDROWID"].ToString());
            sb.Append("<tr class=\"DataGrid-HeaderStyle\"><td align='CENTER'><b>Type of Audit</b></td>");
            sb.Append("<td  colspan ='4' align='CENTER'><b>NCs in ISM audits </b></td>");
            sb.Append("</tr> <tr><td></td>");
            //ISM header
            foreach (DataRow drTempHeader in drvHeader)
            {
                sb.Append("<td colspan='2' align='CENTER'><b>");
                sb.Append(drTempHeader["FLDCOLUMNHEADER"].ToString());
                sb.Append("</b></td>");
            }

            sb.Append("</tr>");


            //ISM DATA
            DataTable dt3 = ds.Tables[0];
            foreach (DataRow dr1 in dt3.Rows)
            {
                DataTable dtTemp = ds.Tables[1];

                DataRow[] drv = dtTemp.Select("FLDROWID = " + dr1["FLDROWID"].ToString());

                sb.Append("<tr><td align='left'><b>" + drv[0]["FLDROWHEADER"].ToString() + " audit </b></td>");

                foreach (DataRow drTemp in drv)
                {
                    sb.Append("<td align='center'>");
                    sb.Append(drTemp["FLDNCCOUNT"].ToString() + " in " + drTemp["FLDTOTALAUDITPERCATEGORY"].ToString() + " audits");
                    sb.Append("</td>");
                    sb.Append("<td  align='center'>");
                    sb.Append(drTemp["FLDAVGNCPERCATEGORY"].ToString() + "/ audit");
                    sb.Append("</td>");
                }
                sb.Append("</tr>");

            }
            sb.Append("<tr><td colspan='5' style='height: 10px;'></td></tr> </tr>");
            if (ds.Tables[1].Rows.Count > 0)
            {
                sb.Append("<tr class=\"DataGrid-HeaderStyle\"><td align='CENTER'><b>Type of Audit</b></td>");
                sb.Append("<td  colspan ='4' align='CENTER'><b>NCs in ISPS audits </b></td>");
                sb.Append("</tr> <tr><td></td>");

                foreach (DataRow drTempHeader in drvHeader)
                {
                    sb.Append("<td colspan='2' align='CENTER'><b>");
                    sb.Append(drTempHeader["FLDCOLUMNHEADER"].ToString());
                    sb.Append("</b></td>");
                }

                sb.Append("</tr>");


                //ISPS DATA
                dt3 = ds.Tables[0];
                foreach (DataRow dr1 in dt3.Rows)
                {
                    DataTable dtTemp = ds.Tables[2];

                    DataRow[] drv = dtTemp.Select("FLDROWID = " + dr1["FLDROWID"].ToString());

                    sb.Append("<tr><td align='left'><b>" + drv[0]["FLDROWHEADER"].ToString() + " audit </b></td>");

                    foreach (DataRow drTemp in drv)
                    {
                        sb.Append("<td align='center'>");
                        sb.Append(drTemp["FLDNCCOUNT"].ToString() + " in " + drTemp["FLDTOTALAUDITPERCATEGORY"].ToString() + " audits");
                        sb.Append("</td>");
                        sb.Append("<td  align='center'>");
                        sb.Append(drTemp["FLDAVGNCPERCATEGORY"].ToString() + "/ audit");
                        sb.Append("</td>");
                    }
                    sb.Append("</tr>");

                }

            }
            sb.Append("</table>");
            lblGrid.Text = sb.ToString();
        }

        else
        {
            sb = new StringBuilder();

            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" > "); 

            sb.Append("<tr  class=\"DataGrid-HeaderStyle\"> <td colspan='5'></td></tr>");
            sb.Append("<tr ><td align=\"center\" colspan=\"5\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

            sb.Append("</table>");

            lblGrid.Text = sb.ToString();
        }

        Response.Write(sb.ToString());
        Response.End();


    }
    private bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ucFromMonth.SelectedHard) == null)
            ucError.ErrorMessage = "From Month is required.";

        if (General.GetNullableString(ddlFromYear.SelectedQuick) == null)
            ucError.ErrorMessage = "From Year is required.";

        if (General.GetNullableString(ucToMonth.SelectedHard) == null)
            ucError.ErrorMessage = "To Month is required.";

        if (General.GetNullableString(ddlToYear.SelectedQuick) == null)
            ucError.ErrorMessage = "To Year is required.";

        if (General.GetNullableString(ddlFromYear.SelectedQuick) != null && General.GetNullableString(ddlToYear.SelectedQuick) != null &&
            General.GetNullableInteger(ddlToYear.SelectedQuick) < General.GetNullableInteger(ddlFromYear.SelectedQuick))
        {
            ucError.ErrorMessage = "To Year should be greater than From Year.";
            return (!ucError.IsError);
        }

        if (General.GetNullableString(ucFromMonth.SelectedHard) != null && General.GetNullableString(ucToMonth.SelectedHard) != null &&
            General.GetNullableInteger(ddlToYear.SelectedQuick) <= General.GetNullableInteger(ddlFromYear.SelectedQuick) &&
            General.GetNullableInteger(ucToMonth.SelectedHard) < General.GetNullableInteger(ucFromMonth.SelectedHard))
        {
            ucError.ErrorMessage = "To Month should be greater than From Month.";
            return (!ucError.IsError);
        }
        return (!ucError.IsError);
    }

    protected void MenuAuditSummaryNCGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("YEARWISENC"))
        {
            Response.Redirect("../Inspection/InspectionNonConformityReportByYearWiseAudit.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("CATEGORYWISENC"))
        {
            Response.Redirect("../Inspection/InspectionNonConformityReportByCategoryWiseAudit.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("VESSELTYPEWISENC"))
        {
            Response.Redirect("../Inspection/InspectionNonConformityReportByVesselTypeWiseAudit.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("BREAKUPWISENC"))
        {
            Response.Redirect("../Inspection/InspectionNCReportByCategoryAndVesselTypeWiseAudit.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("AUDITSUMMARY"))
        {
            Response.Redirect("../Inspection/InspectionNCReportByAuditSummary.aspx");
        }
    }
}
