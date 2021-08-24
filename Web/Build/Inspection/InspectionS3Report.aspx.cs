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
using Telerik.Web.UI;

public partial class InspectionS3Report : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionS3Report.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionS3Report.aspx", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionS3Report.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuS3Report.AccessRights = this.ViewState;
            MenuS3Report.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                BindYear(ddlFromYear);
                BindYear(ddlToYear);
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

    protected void MenuS3Report_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlFrommonth.SelectedIndex = 0;
                ddlFromYear.SelectedIndex = 0;
                ddlTomonth.SelectedIndex = 0;
                ddlToYear.SelectedIndex = 0;
                BindData();
                gvCategoryStatistics.Rebind();
                gvExposureHours.Rebind();
                gvInjuryType.Rebind();
                gvPartOfBody.Rebind();
                gvPersonalInjury.Rebind();
                gvQ5.Rebind();
                gvVslTypeStatistics.Rebind();
                gvIncidentComparision.Rebind();
               
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        string[] alColumns = { "FLDPERIODNAME", "FLDPERSONALINJURY", "FLDENVRELEASE", "FLDNEARMISS", "FLDPROPERTYDAMAGE", "FLDPROCESSLOSS", "FLDSECURITY", "FLDTOTAL" };
        string[] alCaptions = { "Incident Comparison", "Personal Injury", "Environment Release", "Near Miss", "Property Damage", "Process Loss", "Security", "Total" };

        // Incident Comparison

        DataSet ds = PhoenixInspectionS3Report.InspectionS3IncidentComparision(General.GetNullableInteger(ddlFrommonth.SelectedValue)
                                                                        , General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                        , General.GetNullableInteger(ddlTomonth.SelectedValue)
                                                                        , General.GetNullableInteger(ddlToYear.SelectedValue));

        if (ds.Tables[0].Rows.Count > 0)
        {
            // gvIncidentComparision.DataSource = ds.Tables[0];
            // gvIncidentComparision.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            // ShowNoRecordsFound(dt, gvIncidentComparision);
        }
        gvIncidentComparision.DataSource = ds.Tables[0];
        // Exposure hours

        if (ds.Tables[1].Rows.Count > 0)
        {
            //  gvExposureHours.DataSource = ds.Tables[1];
            // gvExposureHours.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[1];
            // ShowNoRecordsFound(dt, gvExposureHours);
        }
        gvExposureHours.DataSource = ds.Tables[1];
        // Vessel Type Wise Incident Statistics

        ds = PhoenixInspectionS3Report.InspectionS3VsltypewiseStatistics(General.GetNullableInteger(ddlFrommonth.SelectedValue)
                                                                        , General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                        , General.GetNullableInteger(ddlTomonth.SelectedValue)
                                                                        , General.GetNullableInteger(ddlToYear.SelectedValue));

        if (ds.Tables[0].Rows.Count > 0)
        {
            //  gvVslTypeStatistics.DataSource = ds.Tables[0];
            gvVslTypeStatistics.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            // ShowNoRecordsFound(dt, gvVslTypeStatistics);
        }
        gvVslTypeStatistics.DataSource = ds.Tables[0];
        // Category Wise Incident Statistics

        ds = PhoenixInspectionS3Report.InspectionS3CategorywiseStatistics(General.GetNullableInteger(ddlFrommonth.SelectedValue)
                                                                        , General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                        , General.GetNullableInteger(ddlTomonth.SelectedValue)
                                                                        , General.GetNullableInteger(ddlToYear.SelectedValue));

        if (ds.Tables[0].Rows.Count > 0)
        {
            //gvCategoryStatistics.DataSource = ds.Tables[0];
            // gvCategoryStatistics.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            //ShowNoRecordsFound(dt, gvCategoryStatistics);
        }
        gvCategoryStatistics.DataSource = ds.Tables[0];
        // Personal Injuries

        ds = PhoenixInspectionS3Report.InspectionS3PersonalInjury(General.GetNullableInteger(ddlToYear.SelectedValue));

        if (ds.Tables[0].Rows.Count > 0)
        {
            // gvPersonalInjury.DataSource = ds.Tables[0];
            // gvPersonalInjury.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            // ShowNoRecordsFound(dt, gvPersonalInjury);
        }
        gvPersonalInjury.DataSource = ds.Tables[0];
        // Injury Type Comparison

        ds = PhoenixInspectionS3Report.InspectionS3InjuryTypeComparision(General.GetNullableInteger(ddlToYear.SelectedValue));

        if (ds.Tables[0].Rows.Count > 0)
        {
            // gvInjuryType.DataSource = ds.Tables[0];
            //  gvInjuryType.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            //ShowNoRecordsFound(dt, gvInjuryType);
        }
        gvInjuryType.DataSource = ds.Tables[0];
        // Q5 Categorywise
        string Q5category = PhoenixCommonRegisters.GetHardCode(1, 208, "UAT");
        ds = PhoenixInspectionS3Report.InspectionS3Q5CategorywiseList(General.GetNullableInteger(Q5category), General.GetNullableInteger(ddlToYear.SelectedValue));

        if (ds.Tables[0].Rows.Count > 0)
        {
            // gvQ5.DataSource = ds.Tables[0];
            //  gvQ5.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            // ShowNoRecordsFound(dt, gvQ5);
        }
        gvQ5.DataSource = ds.Tables[0];
        // Part of the Body

        ds = PhoenixInspectionS3Report.InspectionS3PartOfBodyList(General.GetNullableInteger(ddlFrommonth.SelectedValue)
                                                                        , General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                        , General.GetNullableInteger(ddlTomonth.SelectedValue)
                                                                        , General.GetNullableInteger(ddlToYear.SelectedValue));

        if (ds.Tables[0].Rows.Count > 0)
        {
            //   gvPartOfBody.DataSource = ds.Tables[0];
            //  gvPartOfBody.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            // ShowNoRecordsFound(dt, gvPartOfBody);
        }
        gvPartOfBody.DataSource = ds.Tables[0];
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDPERIODNAME", "FLDPERSONALINJURY", "FLDENVRELEASE", "FLDNEARMISS", "FLDPROPERTYDAMAGE", "FLDPROCESSLOSS", "FLDSECURITY", "FLDTOTAL" };
        string[] alCaptions = { "Incident Comparison", "Personal Injury", "Environment Release", "Near Miss", "Property Damage", "Process Loss", "Security", "Total" };
        int prevyear = int.Parse(ddlToYear.SelectedValue) - 1;

        // Incident Comparison

         ds = PhoenixInspectionS3Report.InspectionS3IncidentComparision(General.GetNullableInteger(ddlFrommonth.SelectedValue)
                                                                        , General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                        , General.GetNullableInteger(ddlTomonth.SelectedValue)
                                                                        , General.GetNullableInteger(ddlToYear.SelectedValue));


        Response.AddHeader("Content-Disposition", "attachment; filename=S3_Report.xls");
        Response.ContentType = "application/vnd.msexcel";
        StringBuilder sb = new StringBuilder();
        sb.Append(PrepareData("A) Incident Comparison", ds.Tables[0], alColumns, alCaptions));

        // Exposure Hours
        alColumns = new string[] { "FLDPERIOD", "FLDEXPOSUREHOUR" };
        alCaptions = new string[] { "Period", "Exposure Hours" };
        sb.Append(PrepareData("Exposure Hours", ds.Tables[1], alColumns, alCaptions));

        // Vessel Type Wise Incident Statistics

        ds = PhoenixInspectionS3Report.InspectionS3VsltypewiseStatistics(General.GetNullableInteger(ddlFrommonth.SelectedValue)
                                                                        , General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                        , General.GetNullableInteger(ddlTomonth.SelectedValue)
                                                                        , General.GetNullableInteger(ddlToYear.SelectedValue));

        alColumns = new string[] { "FLDVESSELTYPENAME", "FLDPERSONALINJURY", "FLDENVRELEASE", "FLDNEARMISS", "FLDPROPERTYDAMAGE", "FLDPROCESSLOSS", "FLDSECURITY", "FLDTOTAL" };
        alCaptions = new string[] { "Vessel Type Wise Incident Statistics", "Personal Injury", "Environment Release", "Near Miss", "Property Damage", "Process Loss", "Security", "Total" };
        sb.Append(PrepareData("B)1) Vessel Type Wise Incident Statistics", ds.Tables[0], alColumns, alCaptions));

        // Category Wise Incident Statistics

        ds = PhoenixInspectionS3Report.InspectionS3CategorywiseStatistics(General.GetNullableInteger(ddlFrommonth.SelectedValue)
                                                                      , General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                      , General.GetNullableInteger(ddlTomonth.SelectedValue)
                                                                      , General.GetNullableInteger(ddlToYear.SelectedValue));

        alColumns = new string[] { "FLDCATEGORYNAME", "FLDPERSONALINJURY", "FLDENVRELEASE", "FLDNEARMISS", "FLDPROPERTYDAMAGE", "FLDPROCESSLOSS", "FLDSECURITY", "FLDTOTAL" };
        alCaptions = new string[] { "Category Wise Incident Statistics", "Personal Injury", "Environment Release", "Near Miss", "Property Damage", "Process Loss", "Security", "Total" };
        sb.Append(PrepareData("2) Category Wise Incident Statistics", ds.Tables[0], alColumns, alCaptions));

        // Personal Injuries

        ds = PhoenixInspectionS3Report.InspectionS3PersonalInjury(General.GetNullableInteger(ddlToYear.SelectedValue));
        alColumns = new string[] { "FLDCATEGORYNAME", "FLD4QPREVYEAR", "FLD1Q", "FLD2Q", "FLD3Q", "FLD4Q" };
        alCaptions = new string[] { "Injury Category", "4Q " + prevyear.ToString(), "1Q " + ddlToYear.SelectedValue, "2Q " + ddlToYear.SelectedValue, "3Q " + ddlToYear.SelectedValue, "4Q " + ddlToYear.SelectedValue };
        sb.Append(PrepareData("3) Personal Injuries", ds.Tables[0], alColumns, alCaptions));

        // Injury Type Comparison

        ds = PhoenixInspectionS3Report.InspectionS3InjuryTypeComparision(General.GetNullableInteger(ddlToYear.SelectedValue));
        alColumns = new string[] { "FLDINJURYTYPE", "FLD4QPREVYEAR", "FLD1Q", "FLD2Q", "FLD3Q", "FLD4Q" };
        alCaptions = new string[] { "Injury Type", "4Q " + prevyear.ToString(), "1Q " + ddlToYear.SelectedValue, "2Q " + ddlToYear.SelectedValue, "3Q " + ddlToYear.SelectedValue, "4Q " + ddlToYear.SelectedValue };
        sb.Append(PrepareData("4) Injury Type Comparison", ds.Tables[0], alColumns, alCaptions));

        // Q5 Categorywise
        string Q5category = PhoenixCommonRegisters.GetHardCode(1, 208, "UAT");
        ds = PhoenixInspectionS3Report.InspectionS3Q5CategorywiseList(General.GetNullableInteger(Q5category), General.GetNullableInteger(ddlToYear.SelectedValue));
        alColumns = new string[] { "FLDCATEGORY", "FLD4QPREVYEAR", "FLD1Q", "FLD2Q", "FLD3Q", "FLD4Q" };
        alCaptions = new string[] { "Q5-Category", "4Q " + prevyear.ToString(), "1Q " + ddlToYear.SelectedValue, "2Q " + ddlToYear.SelectedValue, "3Q " + ddlToYear.SelectedValue, "4Q " + ddlToYear.SelectedValue };
        sb.Append(PrepareData("5) Q5-Categorywise", ds.Tables[0], alColumns, alCaptions));

        // Part of the Body

        ds = PhoenixInspectionS3Report.InspectionS3PartOfBodyList(General.GetNullableInteger(ddlFrommonth.SelectedValue)
                                                                        , General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                        , General.GetNullableInteger(ddlTomonth.SelectedValue)
                                                                        , General.GetNullableInteger(ddlToYear.SelectedValue));
        alColumns = new string[] { "FLDPARTOFBODY", "FLDCOUNT" };
        alCaptions = new string[] { "Part of the Body", "Count" };
        sb.Append(PrepareData("6) Part of the Body", ds.Tables[0], alColumns, alCaptions));

        Response.Write(sb.ToString());
        Response.End();
    }

    protected StringBuilder PrepareData(string title, DataTable dt, string[] alColumns, string[] alCaptions)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        sb.Append("<tr>");
        //sb.Append("<td colspan='2'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        sb.Append("<td colspan='" + (alColumns.Length - 2).ToString() + "'><h3>" + title + "</h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        sb.Append("</tr>");
        sb.Append("</TABLE>");
        sb.Append("<br />");
        sb.Append("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        sb.Append("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            sb.Append("<td width='20%'>");
            sb.Append("<b>" + alCaptions[i] + "</b>");
            sb.Append("</td>");
        }
        sb.Append("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            sb.Append("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                sb.Append("<td>");
                sb.Append(dr[alColumns[i]]);
                sb.Append("</td>");

            }
            sb.Append("</tr>");
        }
        sb.Append("</TABLE>");
        return sb;
    }

    protected void gvPersonalInjury_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            if (General.GetNullableInteger(ddlToYear.SelectedValue) != null)
            {
                int prevyear = int.Parse(ddlToYear.SelectedValue) - 1;
                RadLabel lblPrev4QHeader = (RadLabel)e.Item.FindControl("lblPrev4QHeader");
                if (lblPrev4QHeader != null)
                    lblPrev4QHeader.Text = "4Q " + prevyear.ToString();

                RadLabel lbl1QHeader = (RadLabel)e.Item.FindControl("lbl1QHeader");
                if (lbl1QHeader != null)
                    lbl1QHeader.Text = "1Q " + ddlToYear.SelectedValue;

                RadLabel lbl2QHeader = (RadLabel)e.Item.FindControl("lbl2QHeader");
                if (lbl2QHeader != null)
                    lbl2QHeader.Text = "2Q " + ddlToYear.SelectedValue;

                RadLabel lbl3QHeader = (RadLabel)e.Item.FindControl("lbl3QHeader");
                if (lbl3QHeader != null)
                    lbl3QHeader.Text = "3Q " + ddlToYear.SelectedValue;

                RadLabel lbl4QHeader = (RadLabel)e.Item.FindControl("lbl4QHeader");
                if (lbl4QHeader != null)
                    lbl4QHeader.Text = "4Q " + ddlToYear.SelectedValue;
            }
        }
    }

    protected void gvInjuryType_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            if (General.GetNullableInteger(ddlToYear.SelectedValue) != null)
            {
                int prevyear = int.Parse(ddlToYear.SelectedValue) - 1;
                RadLabel lblPrev4QHeader = (RadLabel)e.Item.FindControl("lblPrev4QHeader");
                if (lblPrev4QHeader != null)
                    lblPrev4QHeader.Text = "4Q " + prevyear.ToString();

                RadLabel lbl1QHeader = (RadLabel)e.Item.FindControl("lbl1QHeader");
                if (lbl1QHeader != null)
                    lbl1QHeader.Text = "1Q " + ddlToYear.SelectedValue;

                RadLabel lbl2QHeader = (RadLabel)e.Item.FindControl("lbl2QHeader");
                if (lbl2QHeader != null)
                    lbl2QHeader.Text = "2Q " + ddlToYear.SelectedValue;

                RadLabel lbl3QHeader = (RadLabel)e.Item.FindControl("lbl3QHeader");
                if (lbl3QHeader != null)
                    lbl3QHeader.Text = "3Q " + ddlToYear.SelectedValue;

                RadLabel lbl4QHeader = (RadLabel)e.Item.FindControl("lbl4QHeader");
                if (lbl4QHeader != null)
                    lbl4QHeader.Text = "4Q " + ddlToYear.SelectedValue;
            }
        }
    }

    protected void gvQ5_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            if (General.GetNullableInteger(ddlToYear.SelectedValue) != null)
            {
                int prevyear = int.Parse(ddlToYear.SelectedValue) - 1;
                RadLabel lblPrev4QHeader = (RadLabel)e.Item.FindControl("lblPrev4QHeader");
                if (lblPrev4QHeader != null)
                    lblPrev4QHeader.Text = "4Q " + prevyear.ToString();

                RadLabel lbl1QHeader = (RadLabel)e.Item.FindControl("lbl1QHeader");
                if (lbl1QHeader != null)
                    lbl1QHeader.Text = "1Q " + ddlToYear.SelectedValue;

                RadLabel lbl2QHeader = (RadLabel)e.Item.FindControl("lbl2QHeader");
                if (lbl2QHeader != null)
                    lbl2QHeader.Text = "2Q " + ddlToYear.SelectedValue;

                RadLabel lbl3QHeader = (RadLabel)e.Item.FindControl("lbl3QHeader");
                if (lbl3QHeader != null)
                    lbl3QHeader.Text = "3Q " + ddlToYear.SelectedValue;

                RadLabel lbl4QHeader = (RadLabel)e.Item.FindControl("lbl4QHeader");
                if (lbl4QHeader != null)
                    lbl4QHeader.Text = "4Q " + ddlToYear.SelectedValue;
            }
        }
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

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
}
