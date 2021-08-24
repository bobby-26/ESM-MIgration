using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionS8Report : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionS8Report.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionS8Report.aspx", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionS8Report.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuAuditSummaryNC.AccessRights = this.ViewState;
            MenuAuditSummaryNC.MenuList = toolbar.Show();
            if (!IsPostBack)
            {               
                BindYear(ddlFromYear);
                BindYear(ddlToYear);
                BindVesselTypeList();
                BindDeficiencyCategoryList();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindYear(RadComboBox ddl)
    {
        for (int i = 2005; i <= DateTime.Now.Year; i++)
        {
            ddl.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
        }
        ddl.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    private void BindData()
    {
        lblGridISM.Text = PrepareSummaryData("ISM").ToString();
        lblGridISPS.Text = PrepareSummaryData("ISPS").ToString();
        lblGridExternalISM.Text = PrepareNCData("ISM", 711).ToString();
        lblGridExternalISPS.Text = PrepareNCData("ISPS", 711).ToString();
        lblGridInternalISM.Text = PrepareNCData("ISM", 710).ToString();
        lblGridInternalISPS.Text = PrepareNCData("ISPS", 710).ToString();
    }

    protected void ShowExcel()
    {
        Response.AddHeader("Content-Disposition", "attachment; filename=ISM/ISPS AuditSummaryReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='5'><b>1) Analysis of all non-conformities (NC)</b></td></tr>");
        Response.Write("</TABLE>");
        StringBuilder sb = PrepareSummaryData("ISM");
        sb.Append(PrepareSummaryData("ISPS").ToString());
        sb.Append("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        sb.Append("<tr></tr><tr><td><b>2) Category and Vessel type wise break-up of the External ISM audit NCs are:</b></td></tr>");
        sb.Append("</TABLE>");
        sb.Append(PrepareNCData("ISM", 711).ToString());
        sb.Append("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        sb.Append("<tr></tr><tr><td><b>3) Category and Vessel type wise break-up of the External ISPS audit NCs are:</b></td></tr>");
        sb.Append("</TABLE>");
        sb.Append(PrepareNCData("ISPS", 711).ToString());
        sb.Append("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        sb.Append("<tr></tr><tr><td><b>4) Category and Vessel type wise break-up of the Internal ISM audit NCs are:</b></td></tr>");
        sb.Append("</TABLE>");
        sb.Append(PrepareNCData("ISM", 710).ToString());
        sb.Append("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        sb.Append("<tr></tr><tr><td><b>5) Category and Vessel type wise break-up of the Internal ISPS audit NCs are:</b></td></tr>");
        sb.Append("</TABLE>");
        sb.Append(PrepareNCData("ISPS", 710).ToString());
        Response.Write(sb.ToString());
        Response.End();
    }

    protected StringBuilder PrepareSummaryData(string audit)
    {
        DataSet ds = new DataSet();
        string VesselTypeList = GetSelectedVesselType();
        string CategoryList = GetSelectedCategory();

        ds = PhoenixInspectionS8Report.InspectionS8ReportSummary(General.GetNullableInteger(ddlFrommonth.SelectedValue), General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                    , General.GetNullableInteger(ddlTomonth.SelectedValue), General.GetNullableInteger(ddlToYear.SelectedValue)
                                                                    , audit, General.GetNullableString(VesselTypeList), General.GetNullableString(CategoryList)
                                                           );

        StringBuilder sb = new StringBuilder();
        string s = "NCs in " + audit + " audits";
        if (ds.Tables[1].Rows.Count > 0)
        {            
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");
            sb.Append("<tr class='rgHeader'><td align='CENTER'><b>Type of Audit</b></td>");
            sb.Append("<td  colspan ='4' align='CENTER'><b>" + s + "</b></td>");
            sb.Append("</tr> <tr ><td></td>");

            //Printing the Header

            DataTable dtColumns = ds.Tables[2];
            foreach (DataRow drTempHeader in dtColumns.Rows)
            {
                sb.Append("<b><td colspan='2' align='CENTER' >");
                sb.Append(drTempHeader["FLDCOLUMNNAME"].ToString());
                sb.Append("</td></b>");
            }

            //Printing the Data
            DataTable dtRows = ds.Tables[1];
            foreach (DataRow dr in dtRows.Rows)
            {
                DataTable dtData = ds.Tables[0];
                DataRow[] drv = dtData.Select("FLDROWID = " + dr["FLDROWID"].ToString());

                sb.Append("<tr style=\"height:15px;\" ><td align='left'>" + drv[0]["FLDROWNAME"].ToString() + "</td>");

                foreach (DataRow drTemp in drv)
                {
                    sb.Append("<b><td align='center'>");
                    sb.Append(drTemp["FLDTOTALNC"].ToString() + " in " + drTemp["FLDTOTALAUDIT"].ToString() + " audits");
                    sb.Append("</td></b>");
                    sb.Append("<b><td  align='center'>");
                    sb.Append(drTemp["FLDAVERAGEAUDIT"].ToString() + " / audit");
                    sb.Append("</td></b>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
        }
        else
        {
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");
            sb.Append("<tr style=\"height:15px;\"><td style=\"height:15px;\"></td></tr>");
            sb.Append("<tr style=\"height:15px;\"><td align=\"center\" colspan=\"6\" style=\"font-weight:bold;\">NO RECORDS FOUND</td></tr>");
            sb.Append("</table>");
        }
        return sb;
    }

    protected StringBuilder PrepareNCData(string audit, int auditcategory)
    {
        DataSet ds = new DataSet();
        string VesselTypeList = GetSelectedVesselType();
        string CategoryList = GetSelectedCategory();

        ds = PhoenixInspectionS8Report.InspectionS8ReportNCbyAudit(General.GetNullableInteger(ddlFrommonth.SelectedValue), General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                    , General.GetNullableInteger(ddlTomonth.SelectedValue), General.GetNullableInteger(ddlToYear.SelectedValue)
                                                                    , audit ,auditcategory, General.GetNullableString(VesselTypeList), General.GetNullableString(CategoryList)
                                                           );

        StringBuilder sb = new StringBuilder();
        if (ds.Tables[1].Rows.Count > 0)
        {
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");
            sb.Append("<tr class=\"rgHeader\">");

            //Printing the Header

            DataTable dtColumns = ds.Tables[2];
            foreach (DataRow drTempHeader in dtColumns.Rows)
            {
                sb.Append("<td align='left' width=20%><b>");
                sb.Append(drTempHeader["FLDCOLUMNHEADER"].ToString());
                sb.Append("</b></td>");
            }
            sb.Append("</tr>");

            //Identity Letter 
            sb.Append("<tr ><td align='left' width=20%><b> IDENTITY LETTER</b></td>");

            foreach (DataRow drTempHeader in dtColumns.Rows)
            {
                sb.Append("<td align='CENTER' width=20%><b>");
                sb.Append(drTempHeader["FLDSHORTCODE"].ToString());
                sb.Append("</b></td>");
            }
            sb.Append("</tr>");

            //Printing the Data
            DataTable dtRows = ds.Tables[1];
            foreach (DataRow dr in dtRows.Rows)
            {
                DataTable dtData = ds.Tables[0];
                DataRow[] drv = dtData.Select("FLDROWID = " + dr["FLDROWID"].ToString());

                sb.Append("<tr style=\"height:15px;\" ><td align='left' width=20%>" + drv[0]["FLDROWHEADER"].ToString() + "</td>");

                foreach (DataRow drTemp in drv)
                {
                    sb.Append("<b><td align='center' width=20%>");
                    sb.Append(drTemp["FLDNCCOUNT"].ToString());
                    sb.Append("</td></b>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
        }
        else
        {
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");
            sb.Append("<tr style=\"height:15px;\"><td style=\"height:15px;\"></td></tr>");
            sb.Append("<tr style=\"height:15px;\"><td align=\"center\" colspan=\"6\" style=\"font-weight:bold;\">NO RECORDS FOUND</td></tr>");
            sb.Append("</table>");
        }
        return sb;
    }

    protected void BindDeficiencyCategoryList()
    {
        chkDeficiencyCategory.Items.Clear();
        chkDeficiencyCategory.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 47);
        chkDeficiencyCategory.DataBindings.DataTextField = "FLDQUICKNAME";
        chkDeficiencyCategory.DataBindings.DataValueField = "FLDQUICKCODE";
        chkDeficiencyCategory.DataBind();
    }
    protected void BindVesselTypeList()
    {
        chkVesselType.Items.Clear();
        chkVesselType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 81);
        chkVesselType.DataBindings.DataTextField = "FLDHARDNAME";
        chkVesselType.DataBindings.DataValueField = "FLDHARDCODE";
        chkVesselType.DataBind();
    }

    protected void MenuAuditSummaryNC_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlFrommonth.SelectedIndex = 0;
                ddlFromYear.SelectedIndex = 0;
                ddlTomonth.SelectedIndex = 0;
                ddlToYear.SelectedIndex = 0;
                chkDeficiencyCategoryAll.Checked = false;
                chkVesselTypeAll.Checked = false;
                ClearCheckboxList(chkVesselType);
                ClearCheckboxList(chkDeficiencyCategory);
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ClearCheckboxList(RadCheckBoxList cbl)
    {
        foreach (ButtonListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                item.Selected = false;
            }
        }
    }

    protected string GetSelectedVesselType()
    {
        StringBuilder strVesselType = new StringBuilder();
        foreach (ButtonListItem item in chkVesselType.Items)
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

    protected string GetSelectedCategory()
    {
        StringBuilder strCategorytype = new StringBuilder();
        foreach (ButtonListItem item in chkDeficiencyCategory.Items)
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

    private bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ddlFrommonth.SelectedValue) == null)
            ucError.ErrorMessage = "From Month is required.";

        if (General.GetNullableString(ddlFromYear.SelectedValue) == null)
            ucError.ErrorMessage = "From Year is required.";

        if (General.GetNullableString(ddlTomonth.SelectedValue) == null)
            ucError.ErrorMessage = "To Month is required.";

        if (General.GetNullableString(ddlToYear.SelectedValue) == null)
            ucError.ErrorMessage = "To Year is required.";

        if (General.GetNullableString(ddlFromYear.SelectedValue) != null && General.GetNullableString(ddlToYear.SelectedValue) != null &&
            General.GetNullableInteger(ddlToYear.SelectedValue) < General.GetNullableInteger(ddlFromYear.SelectedValue))
        {
            ucError.ErrorMessage = "To Year should be greater than From Year.";
            return (!ucError.IsError);
        }

        if (General.GetNullableString(ddlFrommonth.SelectedValue) != null && General.GetNullableString(ddlTomonth.SelectedValue) != null &&
            General.GetNullableInteger(ddlToYear.SelectedValue) <= General.GetNullableInteger(ddlFromYear.SelectedValue) &&
            General.GetNullableInteger(ddlTomonth.SelectedValue) < General.GetNullableInteger(ddlFrommonth.SelectedValue))
        {
            ucError.ErrorMessage = "To Month should be greater than From Month.";
            return (!ucError.IsError);
        }
        return (!ucError.IsError);
    }

    protected void chkDeficiencyCategoryAll_Changed(object sender, EventArgs e)
    {
        try
        {
            if (chkDeficiencyCategoryAll.Checked == true)
            {
                foreach (ButtonListItem li in chkDeficiencyCategory.Items)
                    li.Selected = true;
            }
            else
            {
                foreach (ButtonListItem li in chkDeficiencyCategory.Items)
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

    protected void chkVesselTypeAll_Changed(object sender, EventArgs e)
    {
        try
        {
            if (chkVesselTypeAll.Checked == true)
            {
                foreach (ButtonListItem li in chkVesselType.Items)
                    li.Selected = true;
            }
            else
            {
                foreach (ButtonListItem li in chkVesselType.Items)
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
}
