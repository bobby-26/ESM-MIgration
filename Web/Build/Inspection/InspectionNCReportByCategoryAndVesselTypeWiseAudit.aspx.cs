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

public partial class InspectionNCReportByCategoryAndVesselTypeWiseAudit : PhoenixBasePage
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
                BindVesselTypeList();
                BindDeficiencyCategoryList();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageLink("../Inspection/InspectionNCReportByCategoryAndVesselTypeWiseAudit.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbar.AddImageLink("javascript:CallPrint('gvCNC')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Inspection/InspectionNCReportByCategoryAndVesselTypeWiseAudit.aspx", "Search", "search.png", "FIND");
            toolbar.AddImageButton("../Inspection/InspectionNCReportByCategoryAndVesselTypeWiseAudit.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuBreakUpNC.AccessRights = this.ViewState;
            MenuBreakUpNC.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Year wise NC Trend", "YEARWISENC");
            toolbar.AddButton("By Category", "CATEGORYWISENC");
            toolbar.AddButton("By Vessel Type", "VESSELTYPEWISENC");
            toolbar.AddButton("By Category & Vessel Type", "BREAKUPWISENC");
            toolbar.AddButton("ISM/ISPS Summary", "AUDITSUMMARY");
            
            MenuBreakUpNCGeneral.AccessRights = this.ViewState;
            MenuBreakUpNCGeneral.MenuList = toolbar.Show();
            MenuBreakUpNCGeneral.SelectedMenuIndex = 3;
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
            ucFromMonth.SelectedHard = (nvc!= null ? nvc["ucFromMonth"] : null );
            ucToMonth.SelectedHard = (nvc != null ? nvc["ucToMonth"] : null);
            ddlFromYear.SelectedQuick = (nvc != null ? nvc["ddlFromYear"] : null);
            ddlToYear.SelectedQuick = (nvc != null ? nvc["ddlToYear"] : null);
        }
    }
    protected void BindDeficiencyCategoryList()
    {
        chkDeficiencyCategory.Items.Clear();
        chkDeficiencyCategory.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 47);
        chkDeficiencyCategory.DataTextField = "FLDQUICKNAME";
        chkDeficiencyCategory.DataValueField = "FLDQUICKCODE";
        chkDeficiencyCategory.DataBind();
    }
    protected void BindVesselTypeList()
    {
        chkVesselType.Items.Clear();
        chkVesselType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 81);
        chkVesselType.DataTextField = "FLDHARDNAME";
        chkVesselType.DataValueField = "FLDHARDCODE";
        chkVesselType.DataBind();
    }

    protected string GetSelectedCategory()
    {
        StringBuilder strCategorytype = new StringBuilder();
        foreach (ListItem item in chkDeficiencyCategory.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strCategorytype.Append(item.Value.ToString());
                strCategorytype.Append(",");
            }
        }

        if (strCategorytype.Length > 1)
            strCategorytype.Remove(strCategorytype.Length - 1, 1);

        string Categorytype = strCategorytype.ToString();
        return Categorytype;
    }
    protected string GetSelectedVesselType()
    {
        StringBuilder strVesselType = new StringBuilder();
        foreach (ListItem item in chkVesselType.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strVesselType.Append(item.Value.ToString());
                strVesselType.Append(",");
            }
        }

        if (strVesselType.Length > 1)
            strVesselType.Remove(strVesselType.Length - 1, 1);

        string VesselList = strVesselType.ToString();
        return VesselList;
    }
    protected void chkVesselTypeAll_Changed(object sender, EventArgs e)
    {
        try
        {
            if (chkVesselTypeAll.Checked == true)
            {
                foreach (ListItem li in chkVesselType.Items)
                    li.Selected = true;
            }
            else
            {
                foreach (ListItem li in chkVesselType.Items)
                    li.Selected = false;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void chkDeficiencyCategoryAll_Changed(object sender, EventArgs e)
    {
        try
        {
            if (chkDeficiencyCategoryAll.Checked == true)
            {
                foreach (ListItem li in chkDeficiencyCategory.Items)
                    li.Selected = true;
            }
            else
            {
                foreach (ListItem li in chkDeficiencyCategory.Items)
                    li.Selected = false;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }   
    private void BindData()  
    {
      
        DataSet ds = new DataSet();
        string VesselTypeList = GetSelectedVesselType();
        string CategoryList = GetSelectedCategory();
        NameValueCollection nvc = Filter.CurrentPeriodFilterForNC;
        
        ds = PhoenixInspectionReports.ListhNCCountByCategoryAndVesselTypeWiseAudit(
                                                                   General.GetNullableString(ddlAuditName.SelectedValue)
                                                                 , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                                                 , General.GetNullableString(VesselTypeList)
                                                                 , General.GetNullableString(CategoryList)
                                                                 , General.GetNullableInteger(nvc != null ? nvc["ucFromMonth"] : null)
                                                                 , General.GetNullableInteger(nvc != null ? nvc["ucToMonth"] : null)
                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlFromYear"] : null)
                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlToYear"] : null)
                                                                 );
       
        if (ds.Tables[0].Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt1 = ds.Tables[0];
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");
            
            DataTable dtTempHeader = ds.Tables[0];
            DataRow[] drvHeader = dtTempHeader.Select("FLDROWID = " + dt1.Rows[0]["FLDROWID"].ToString());
            sb.Append("<tr class=\"DataGrid-HeaderStyle\"><td align='CENTER'><b>Vessel Type</b></td>");
            sb.Append("<td align='CENTER'><b>Total Audits <br/> per Vessel Type</b></td>");       
          
            foreach (DataRow drTempHeader in drvHeader)
            {
                sb.Append("<td align='CENTER'><b>");
                sb.Append(drTempHeader["FLDCOLUMNHEADER"].ToString());
                sb.Append("</b></td>");
            }
            sb.Append("<td align='CENTER'><b>Total</b></td>");
            sb.Append("<td align='CENTER'><b>Avgerage per ship per audit</b></td></tr>");

            //SHORTCODE 
            sb.Append("<tr><td align='CENTER' colspan='2'><b> IDENTITY LETTER</b></td>");           

            foreach (DataRow drTempHeader in drvHeader)
            {
                sb.Append("<td align='CENTER'><b>");
                sb.Append(drTempHeader["FLDSHORTCODE"].ToString());
                sb.Append("</b></td>");
            }
            sb.Append("<td align='CENTER'></td>");
            sb.Append("<td align='CENTER'></td></tr>");
           

            //DATA
            DataTable dt3 = ds.Tables[1];
            foreach (DataRow dr1 in dt3.Rows)
            {
                DataTable dtTemp = ds.Tables[0];

                DataRow[] drv = dtTemp.Select("FLDROWID = " + dr1["FLDROWID"].ToString());

                sb.Append("<tr><td align='left'><b>" + drv[0]["FLDROWHEADER"].ToString() + "</b></td>");
                sb.Append("<td align='center'>" + dr1["FLDTOTALAUDITPERSHIP"].ToString() + "</td>");
                foreach (DataRow drTemp in drv)
                {
                    sb.Append("<td align='center'>");                   
                    sb.Append(drTemp["FLDNCCOUNT"].ToString());
                    sb.Append("</td>");                   
                }         

                sb.Append("<td align='center'><b>" + dr1["FLDROWTOTAL"].ToString() + "</b></td>");
                sb.Append("<td align='center'><b>" + dr1["FLDAVGNCPERSHIPPERAUDIT"].ToString() + "</b></tr>");

            }
            /*total Row*/
            DataTable dt4 = ds.Tables[2];
            DataRow dr2 = dt4.Rows[0];
            sb.Append("<tr><td align='left'><b>TOTAL</b></td>");
            sb.Append("<td align='center'><b>" + dr2["FLDGRANDTOTALAUDITPERSHIP"].ToString() + "</b></td>");
            foreach (DataRow drTemp in ds.Tables[3].Rows)
            {
                sb.Append("<td align='center'><b>");
                sb.Append(drTemp["FLDFINALTOTAL"].ToString());
                sb.Append("</b></td>");
            }

            sb.Append("<td align='center'><b>" + dr2["FLDGRANDTOTAL"].ToString() + "</b></td>");
            sb.Append("<td align='center'><b>" + dr2["FLDGRANDAVGAUDITPERSHIP"].ToString() + "</b></tr>");
            sb.Append("</tr>");
            /*Avg Row*/
            sb.Append("<tr><td align='left'><b>AVGERAGE PER </BR> AUDIT</b></td>");
            sb.Append("<td align='center'> </td>"); //empty
            foreach (DataRow drTemp in ds.Tables[3].Rows)
            {
                sb.Append("<td align='center'><b>");
                sb.Append(drTemp["FLDFINALAVGPERAUDIT"].ToString());
                sb.Append("</b></td>");
            }
            sb.Append("<td align='center'><b>" + dr2["FLDGRANDAVGAUDITPERSHIP"].ToString() + "</b></td>");
            sb.Append("<td align='center'></td></tr>"); //empty
            sb.Append("</tr>");

            sb.Append("</table>");

            ltGrid.Text = sb.ToString();

        }
        else
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");

            sb.Append("<tr style=\"height:10px;\" class=\"DataGrid-HeaderStyle\"><td style=\"height:15px;\"></td></tr>");
            sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

            sb.Append("</table>");

            ltGrid.Text = sb.ToString();
        }
        
    }
    protected void MenuBreakUpNC_TabStripCommand(object sender, EventArgs e)
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
                foreach (ListItem li in chkDeficiencyCategory.Items)
                    li.Selected = false;
                foreach (ListItem li in chkVesselType.Items)
                    li.Selected = false;
                ddlAuditName.SelectedValue = "";
                ucInspectionCategory.SelectedHard = "";
                ucFromMonth.SelectedHard = "";
                ddlFromYear.SelectedQuick = "";
                ucToMonth.SelectedHard = "";
                ddlToYear.SelectedQuick = "";
                chkDeficiencyCategoryAll.Checked = false;
                chkVesselTypeAll.Checked = false;
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
        string VesselTypeList = GetSelectedVesselType();
        string CategoryList = GetSelectedCategory();
        NameValueCollection nvc = Filter.CurrentPeriodFilterForNC;
        ds = PhoenixInspectionReports.ListhNCCountByCategoryAndVesselTypeWiseAudit(
                                                               General.GetNullableString(ddlAuditName.SelectedValue)
                                                             , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                                             , General.GetNullableString(VesselTypeList)
                                                             , General.GetNullableString(CategoryList)
                                                             , General.GetNullableInteger(ucFromMonth.SelectedHard)//(nvc != null ? nvc["ucFromMonth"] : null)
                                                             , General.GetNullableInteger(ucToMonth.SelectedHard)//(nvc != null ? nvc["ucToMonth"] : null)
                                                             , General.GetNullableInteger(ddlFromYear.SelectedQuick)//(nvc != null ? nvc["ddlFromYear"] : null)
                                                             , General.GetNullableInteger(ddlToYear.SelectedQuick)//(nvc != null ? nvc["ddlToYear"] : null)
                                                             );
        string auditCategory = "";
        string auditName = "";
        int colspan = 0;
        auditName = ddlAuditName.SelectedValue;
        if (ucInspectionCategory.SelectedHard == "710")
            auditCategory = "Internal ";
        else if (ucInspectionCategory.SelectedHard == "711")
            auditCategory = "External ";
        colspan = ds.Tables[3].Rows.Count + 4;//(Vesseltype, and Total Columns)

        Response.AddHeader("Content-Disposition", "attachment; filename=CategoryAndVesselTypewiseBreakUpNCReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");//style='font-family:Calibri; font-size:11px;'
        Response.Write("<tr><td colspan='"+ colspan +"'><b><center>Category and Vessel type wise break-up of the " + auditCategory + auditName + " audit NCs </center></b></td></tr>");
                      
        Response.Write("</TABLE>");
        if (ds.Tables.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            if (ds.Tables[0].Rows.Count > 0)
            {

                DataTable dt1 = ds.Tables[0];//rules=\"all\" \\style=\"font-size:11px;width:100%;border-collapse:collapse;\"
                sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" > ");

                DataTable dtTempHeader = ds.Tables[0];
                DataRow[] drvHeader = dtTempHeader.Select("FLDROWID = " + dt1.Rows[0]["FLDROWID"].ToString());
                sb.Append("<tr class=\"DataGrid-HeaderStyle\"><td align='CENTER'><b>Vessel Type</b></td>");
                sb.Append("<td align='CENTER'><b>Total Audits <br/> per Vessel Type</b></td>");

                foreach (DataRow drTempHeader in drvHeader)
                {
                    sb.Append("<td align='CENTER'><b>");
                    sb.Append(drTempHeader["FLDCOLUMNHEADER"].ToString());
                    sb.Append("</b></td>");
                }
                sb.Append("<td align='CENTER'><b>Total</b></td>");
                sb.Append("<td align='CENTER'><b>Avgerage per ship per audit</b></td></tr>");

                //SHORTCODE 
                sb.Append("<tr><td align='CENTER' colspan='2'><b> IDENTITY LETTER</b></td>");

                foreach (DataRow drTempHeader in drvHeader)
                {
                    sb.Append("<td align='CENTER'><b>");
                    sb.Append(drTempHeader["FLDSHORTCODE"].ToString());
                    sb.Append("</b></td>");
                }
                sb.Append("<td align='CENTER'></td>");
                sb.Append("<td align='CENTER'></td></tr>");
           

                //DATA
                DataTable dt3 = ds.Tables[1];
                foreach (DataRow dr1 in dt3.Rows)
                {
                    DataTable dtTemp = ds.Tables[0];

                    DataRow[] drv = dtTemp.Select("FLDROWID = " + dr1["FLDROWID"].ToString());

                    sb.Append("<tr><td align='left'><b>" + drv[0]["FLDROWHEADER"].ToString() + "</b></td>");
                    sb.Append("<td align='center'>" + dr1["FLDTOTALAUDITPERSHIP"].ToString() + "</td>");
                    foreach (DataRow drTemp in drv)
                    {
                        sb.Append("<td align='center'>");
                        sb.Append(drTemp["FLDNCCOUNT"].ToString());
                        sb.Append("</td>");
                    }

                    sb.Append("<td align='center'><b>" + dr1["FLDROWTOTAL"].ToString() + "</b></td>");
                    sb.Append("<td align='center'><b>" + dr1["FLDAVGNCPERSHIPPERAUDIT"].ToString() + "</b></tr>");

                }
                /*total Row*/
                DataTable dt4 = ds.Tables[2];
                DataRow dr2 = dt4.Rows[0];
                sb.Append("<tr><td align='left'><b>TOTAL</b></td>");
                sb.Append("<td align='center'><b>" + dr2["FLDGRANDTOTALAUDITPERSHIP"].ToString() + "</b></td>");
                foreach (DataRow drTemp in ds.Tables[3].Rows)
                {
                    sb.Append("<td align='center'><b>");
                    sb.Append(drTemp["FLDFINALTOTAL"].ToString());
                    sb.Append("</b></td>");
                }

                sb.Append("<td align='center'><b>" + dr2["FLDGRANDTOTAL"].ToString() + "</b></td>");
                sb.Append("<td align='center'><b>" + dr2["FLDGRANDAVGAUDITPERSHIP"].ToString() + "</b></tr>");
                sb.Append("</tr>");
                /*Avg Row*/
                sb.Append("<tr><td align='left'><b>AVGERAGE PER </BR> AUDIT</b></td>");
                sb.Append("<td align='center'> </td>"); //empty
                foreach (DataRow drTemp in ds.Tables[3].Rows)
                {
                    sb.Append("<td align='center'><b>");
                    sb.Append( drTemp["FLDFINALAVGPERAUDIT"].ToString());
                    sb.Append("</b></td>");
                }
                sb.Append("<td align='center'><b>" +dr2["FLDGRANDAVGAUDITPERSHIP"].ToString() + "</b></td>");
                sb.Append("<td align='center'></td></tr>"); //empty
                sb.Append("</tr>");

                sb.Append("</table>");
                ltGrid.Text = sb.ToString();

            }
            else
            {
                sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\"> "); // style=\"font-size:11px;width:100%;border-collapse:collapse;\"

                sb.Append("<tr class=\"DataGrid-HeaderStyle\"><td colspan=\"6\"></td></tr>");
                sb.Append("<tr><td align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

                sb.Append("</table>");

                ltGrid.Text = sb.ToString();
            }
            Response.Write(sb.ToString());
            Response.End();
        }
    
    }
    private bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableString(ddlAuditName.SelectedValue) == null)
            ucError.ErrorMessage = "Audit is required";

        if (General.GetNullableInteger(ucInspectionCategory.SelectedHard) == null)
            ucError.ErrorMessage = "Inspection Category is required";

        string vesseltypelist = GetSelectedVesselType();
        if (General.GetNullableString(vesseltypelist) == null)
             ucError.ErrorMessage = "Vessel Type is required";

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

    protected void MenuBreakUpNCGeneral_TabStripCommand(object sender, EventArgs e)
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
