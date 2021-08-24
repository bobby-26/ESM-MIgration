using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class Crew_CrewReportSeafarersProvidentFund : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
        CrewMenuGeneral.AccessRights = this.ViewState;
        CrewMenuGeneral.MenuList = toolbarsub.Show();

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddFontAwesomeButton("../Crew/CrewReportSeafarersProvidentFund.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");

        MenuShowExcel.AccessRights = this.ViewState;
        MenuShowExcel.MenuList = toolbar1.Show();

        if (!IsPostBack)
        {         
            chkVessels.DataSource = PhoenixRegistersVessel.ListCommonVessels(null, null, null, null, 1, null, null, null, 1, "VSL");
            chkVessels.DataBind();
        }
        gvCrew.PageSize = 10000;        
    }
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
    }
    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            if (!validreport(ucUnion.SelectedAddress))
            {
                ucError.Visible = true;
                return;
            }
            ShowReport();
        }        
    }

    private bool validreport (string union)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(union)==null)
            ucError.ErrorMessage = "Please select the Union";

        return (!ucError.IsError);
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowReport()
    {
        DataSet ds = new DataSet();

        string vessels = "";
        foreach (ButtonListItem item in chkVessels.Items)
        {
            if (item.Selected == true)
                vessels = vessels + item.Value + ",";
        }

        ds = PhoenixCrewReportSeafarersProvidentFund.CrewReportSingaporeUnion(General.GetNullableInteger(ucUnion.SelectedAddress), txtComponent.Text, General.GetNullableDateTime(ucDate.Text),
                    General.GetNullableDateTime(ucDate1.Text), General.GetNullableString(vessels));

        gvCrew.DataSource = ds;
    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDTHRIFTID", "FLDUNIONNO", "FLDNAME", "FLDISPFNO", "FLDCREWID", "FLDIMONUMBER", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDPASSPORTNO", "FLDADDRESS", "FLDEMAIL", "FLDSHORTNAME", "FLDDATEOFBIRTH", "FLDNATIONALITY","FLDRANK"};
        string[] alCaptions = { "Union Member Name", "Thrift ID", "Union No", "Union Code", "iSPF No", "Crew Id", "IMO No", "Sign In", "Sign Out", "Passport No", "Union Member Address", "Email Address", "Gender", "DOB", "Nationality Code","Rank Description"};

        string vessels = "";
        foreach (ButtonListItem item in chkVessels.Items)
        {
            if (item.Selected==true)
                vessels = vessels + item.Value + ",";
        }

        ds = PhoenixCrewReportSeafarersProvidentFund.CrewReportSingaporeUnion(General.GetNullableInteger(ucUnion.SelectedAddress), txtComponent.Text, General.GetNullableDateTime(ucDate.Text),
                    General.GetNullableDateTime(ucDate1.Text), General.GetNullableString(vessels));

        Response.AddHeader("Content-Disposition", "attachment; filename=SeafarersProvidentFundReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Seafarers Provident Fund Report</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                string value = dr[alColumns[i]].ToString();
                if (alColumns[i].ToString() == "FLDNAME")
                {
                    int lastindex = value.IndexOf("(");
                    value = value.Substring(0, lastindex - 1);
                }
         
                Response.Write("<td>");
                Response.Write(value);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {            
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}