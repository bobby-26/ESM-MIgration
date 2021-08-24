using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionQuarterlyreport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionQuarterlyreport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionQuarterlyreport.aspx", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionQuarterlyreport.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuQuarterlyReport.AccessRights = this.ViewState;
            MenuQuarterlyReport.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
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

    protected void MenuQuarterlyReport_TabStripCommand(object sender, EventArgs e)
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
                gvCategory.Rebind();
                gvInjury.Rebind();
                gvOffhire.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlToYear.SelectedIndex = 0;
                BindData();
                gvCategory.Rebind();
                gvInjury.Rebind();
                gvOffhire.Rebind();
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
        // Incident Category 
        DataSet ds = PhoenixInspectionQuaterlyReport.InspectionQuarterlycategoryreport(General.GetNullableInteger(ddlToYear.SelectedValue)
                                                                        , General.GetNullableInteger(ddlIncidentNearmiss.SelectedValue));
        gvCategory.DataSource = ds.Tables[0];
        // Incident injury cases 
        DataSet ds1 = PhoenixInspectionQuaterlyReport.InspectionQuarterlyInjuryreport(General.GetNullableInteger(ddlToYear.SelectedValue));

        gvInjury.DataSource = ds1.Tables[0];
        // Incident off hires
        DataSet ds2 = PhoenixInspectionQuaterlyReport.InspectionQuarterlyOffhiresreport(General.GetNullableInteger(ddlToYear.SelectedValue));

        gvOffhire.DataSource = ds2.Tables[0];
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int prevyear = int.Parse(ddlToYear.SelectedValue) - 1;
        Response.AddHeader("Content-Disposition", "attachment; filename=QuarterlyManagementReview_Report.xls");
        Response.ContentType = "application/vnd.msexcel";
        StringBuilder sb = new StringBuilder();
        string[] alColumns = { };
        string[] alCaptions = { };

        // Incident Category
        ds = PhoenixInspectionQuaterlyReport.InspectionQuarterlycategoryreport(General.GetNullableInteger(ddlToYear.SelectedValue)
                                                                                       , General.GetNullableInteger(ddlIncidentNearmiss.SelectedValue));

        alColumns = new string[] { "FLDCATEGORYNAME", "FLD4QPREVYEAR", "FLD1Q", "FLD2Q", "FLD3Q", "FLD4Q" };
        alCaptions = new string[] { "Category", "4Q " + prevyear.ToString(), "1Q " + ddlToYear.SelectedValue, "2Q " + ddlToYear.SelectedValue, "3Q " + ddlToYear.SelectedValue, "4Q " + ddlToYear.SelectedValue };
        sb.Append(PrepareData("a) Quarterly data for no. of incidents/near miss reports with category  details", ds.Tables[0], alColumns, alCaptions));

        // Injury Cases

        ds = PhoenixInspectionQuaterlyReport.InspectionQuarterlyInjuryreport(General.GetNullableInteger(ddlToYear.SelectedValue));

        alColumns = new string[] { "FLDINJURYTYPE", "FLD4QPREVYEAR", "FLD1Q", "FLD2Q", "FLD3Q", "FLD4Q" };
        alCaptions = new string[] { "Injury Cases", "4Q " + prevyear.ToString(), "1Q " + ddlToYear.SelectedValue, "2Q " + ddlToYear.SelectedValue, "3Q " + ddlToYear.SelectedValue, "4Q " + ddlToYear.SelectedValue };
        sb.Append(PrepareData("b) Quarterly data for injury cases", ds.Tables[0], alColumns, alCaptions));

        // Incident Offhires

        ds = PhoenixInspectionQuaterlyReport.InspectionQuarterlyOffhiresreport(General.GetNullableInteger(ddlToYear.SelectedValue));

        alColumns = new string[] { "FLDOFFHIRENAME", "FLD4QPREVYEAR", "FLD1Q", "FLD2Q", "FLD3Q", "FLD4Q" };
        alCaptions = new string[] { "Incident Off Hires", "4Q " + prevyear.ToString(), "1Q " + ddlToYear.SelectedValue, "2Q " + ddlToYear.SelectedValue, "3Q " + ddlToYear.SelectedValue, "4Q " + ddlToYear.SelectedValue };
        sb.Append(PrepareData("c) Incidents involving off hires", ds.Tables[0], alColumns, alCaptions));

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

    protected void gvCategory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
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
    protected void gvOffhire_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
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
    protected void gvInjury_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
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

        if (General.GetNullableString(ddlToYear.SelectedValue) == null)
            ucError.ErrorMessage = "Year is required.";

        return (!ucError.IsError);
    }

    protected void ddlIncidentNearmiss_Changed(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvOffhire_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvInjury_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();

    }

    protected void gvCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();

    }
}
