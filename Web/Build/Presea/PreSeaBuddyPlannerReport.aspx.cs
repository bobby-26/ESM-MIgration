using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaBuddyPlannerReport : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);              

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddImageButton("../PreSea/PreSeaBuddyPlannerReport.aspx", "Export to Excel", "icon_xls.png", "Excel");
        //toolbarmain.AddImageLink("javascript:CallPrint('gvPreSeaPlannerReport')", "Print Grid", "icon_print.png", "PRINT");
        toolbarmain.AddImageButton("../PreSea/PreSeaBuddyPlannerReport.aspx", "Find", "search.png", "FIND");
        toolbarmain.AddImageButton("../PreSea/PreSeaBuddyPlannerReport.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
        MenuPreSeaPlannerReport.AccessRights = this.ViewState;
        MenuPreSeaPlannerReport.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            BindFacultyList();
        }

        BindData();

    }

    protected void BindFacultyList()
    {
        chkFacultyID.Items.Clear();
        chkFacultyID.DataSource = PhoenixPreSeaBuddyPlanner.PreSeaFacultySearch();
        chkFacultyID.DataTextField = "FLDUSERNAME";
        chkFacultyID.DataValueField = "FLDUSERCODE";
        chkFacultyID.DataBind();

    }

    protected void MenuPreSeaPlannerReport_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            if (!IsValidInput())
            {
                ucError.Visible = true;
                return;
            } 
            BindData();
        }
        if (dce.CommandName.ToUpper().Equals("CLEAR"))
        {
            ucFromDate.Text = "";
            ucToDate.Text = "";
            foreach (ListItem li in chkFacultyID.Items)
                li.Selected = false;
            chkFacultyIDAll.Checked = false;
            ddlStatus.SelectedValue = "";
            BindData();
        }

    }

    private void ShowExcel()
    {
        string[] alColumns = { "FLDFACULTYNAME", "FLDPLANNEDDDATE" };
        string[] alCaptions = { "Faculty Name", "Planned Date" };

        DataSet ds = new DataSet();
        string FacultyIdList = "";
        ds = PhoenixPreSeaWeeklyPlannerReport.BuddyPlannerStatusReport(General.GetNullableDateTime(ucFromDate.Text)
                                                                , General.GetNullableDateTime(ucToDate.Text)
                                                                , General.GetNullableString(FacultyIdList)
                                                                , General.GetNullableInteger(ddlStatus.SelectedValue));

        DataTable dt = ds.Tables[0];

        Response.AddHeader("Content-Disposition", "attachment; filename=BuddyPlannerStatusReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        //if (ds.Tables[2].Rows.Count > 0)
        //{
            string Header = "<table>";
            Header += "<tr><td colspan='2' rowspan='5'><img width='99' height='99' src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/sims.png" + "' /></td>";
            Header += "<td colspan='6' align='center'  style='font-weight:bold;'>Samundra Institute of Maritime Studies, Lonavla</td></tr>";
            Header += "<tr><td colspan='6'  align='center'  style='font-weight:bold;'>";
            Header += "FACULTY DEVELOPMENT PROGRAMME (BUDDY SYSTEM)  PLANNER";
            Header += "</td></tr>";
            Header += "</table>";
            Response.Write(Header);
        //}
        Response.Write("<br />");
        StringBuilder sb = new StringBuilder();
        if (ds.Tables[0].Rows.Count > 0)
        {

            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"width:100%;border-collapse:collapse;\"> ");

            sb.Append("<tr class=\"DataGrid-HeaderStyle\"><td align='CENTER'><b>S.No</b></td><td align='CENTER'><b>Faculty Name</b></td>");
            sb.Append("<td align='CENTER'><b>Date</b></td></tr>");

            //DATA                 
            DataTable dt1 = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            DataTable dt3 = ds.Tables[2];
            int FacultyId;
            FacultyId = 0;
            foreach (DataRow dr1 in dt1.Rows)
            {
                DataTable dtTempFaculty = ds.Tables[1];

                DataRow[] drv = dtTempFaculty.Select("FLDROLEID = " + dr1["FLDROLEID"].ToString());

                sb.Append("<tr> <td></td> <td align='CENTER'> <b>" + dr1["FLDROLENAME"] + "</b></td> <td></td> </tr>");
                foreach (DataRow drFac in drv)
                {
                    DataTable dtTempDates = ds.Tables[2];

                    DataRow[] drv1 = dtTempDates.Select("FLDFACULTYID = " + drFac["FLDFACULTYID"].ToString());
                    FacultyId = FacultyId + 1;
                    sb.Append("<tr> <td align='CENTER'>" + FacultyId + " </td>");
                    sb.Append("<td align='left'>" + drFac["FLDFACULTYNAME"] + "</td>");
                    sb.Append("<td align='left'>");
                    foreach (DataRow drTemp in drv1)
                    {
                        sb.Append(drTemp["FLDPLANNEDDDATE"].ToString() + " , ");
                        //sb.Append("<td align='CENTER'>" + drTemp["FLDISCOMPLETED"] + " , </td>");
                    }
                    sb.Append("</td>");

                }
            }

            sb.Append("</tr>");

            sb.Append("</table>");

            ltGrid.Text = sb.ToString();
        }
        else
        {
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" > ");

            sb.Append("<tr class=\"DataGrid-HeaderStyle\"><td colspan=\"6\"></td></tr>");
            sb.Append("<tr><td align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

            sb.Append("</table>");
            ltGrid.Text = sb.ToString();
        }
        Response.Write(sb.ToString());
        Response.End();
    }

    private void BindData()
    {
        string[] alColumns = {"FLDFACULTYID", "FLDFACULTYNAME", "FLDPLANNEDDDATE" };
        string[] alCaptions = { "S.No", "Faculty Name", "Planned Date" };

        DataSet ds = new DataSet();
        string FacultyIdList = "";
        FacultyIdList = GetSelectedFacultyID();
        ds = PhoenixPreSeaWeeklyPlannerReport.BuddyPlannerStatusReport(General.GetNullableDateTime(ucFromDate.Text)
                                                                 , General.GetNullableDateTime(ucToDate.Text)
                                                                 , General.GetNullableString(FacultyIdList)
                                                                 , General.GetNullableInteger(ddlStatus.SelectedValue));

        StringBuilder sb = new StringBuilder();
        if (ds.Tables[0].Rows.Count > 0)
        {

            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");

            sb.Append("<tr class=\"DataGrid-HeaderStyle\"><td align='CENTER'><b>S.No</b></td><td align='CENTER'><b>Faculty Name</b></td>");
            sb.Append("<td align='CENTER'><b>Date</b></td></tr>");

            //DATA                 
            DataTable dt1 = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            DataTable dt3 = ds.Tables[2];
            int FacultyId;
            FacultyId = 0;
            foreach (DataRow dr1 in dt1.Rows)
            {
                DataTable dtTempFaculty = ds.Tables[1];

                DataRow[] drv = dtTempFaculty.Select("FLDROLEID = " + dr1["FLDROLEID"].ToString()); //RoleNames

                sb.Append("<tr> <td></td> <td align='CENTER'> <b>" + dr1["FLDROLENAME"] + "</b></td> <td></td> </tr>");
                
                foreach (DataRow drFac in drv)
                {
                    DataTable dtTempDates = ds.Tables[2];

                    DataRow[] drv1 = dtTempDates.Select("FLDFACULTYID = " + drFac["FLDFACULTYID"].ToString()); //Faculty Names
                    FacultyId = FacultyId + 1;
                    sb.Append("<tr> <td align='CENTER'>" + FacultyId + " </td>");
                    sb.Append("<td align='left'>" + drFac["FLDFACULTYNAME"] + "</td>");
                    sb.Append("<td align='left'>");
                    foreach (DataRow drTemp in drv1)
                    {
                        sb.Append(drTemp["FLDPLANNEDDDATE"].ToString() + " , ");
                       
                    }
                    sb.Append("</td>");

                }
            }

            sb.Append("</tr>");

            sb.Append("</table>");

            ltGrid.Text = sb.ToString();
        }
        else
        {
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");

            sb.Append("<tr style=\"height:10px;\" class=\"DataGrid-HeaderStyle\"><td style=\"height:15px;\"></td></tr>");
            sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

            sb.Append("</table>");
            ltGrid.Text = sb.ToString();
        }
    }

    protected void chkFacultyIDAll_Changed(object sender, EventArgs e)
    {
        try
        {
            if (chkFacultyIDAll.Checked == true)
            {
                foreach (ListItem li in chkFacultyID.Items)
                    li.Selected = true;
            }
            else
            {
                foreach (ListItem li in chkFacultyID.Items)
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
    protected string GetSelectedFacultyID()
    {
        StringBuilder strFacultyID = new StringBuilder();
        foreach (ListItem item in chkFacultyID.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strFacultyID.Append(item.Value.ToString());
                strFacultyID.Append(",");
            }
        }

        if (strFacultyID.Length > 1)
            strFacultyID.Remove(strFacultyID.Length - 1, 1);

        string FacultyIDList = strFacultyID.ToString();
        return FacultyIDList;
    }
    private bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(ucFromDate.Text) == null)
            ucError.ErrorMessage = "From Date is required.";

        if (General.GetNullableDateTime(ucToDate.Text) == null)
            ucError.ErrorMessage = "To Date is required.";

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
