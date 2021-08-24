using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.ShippingKPI;

public partial class Dashboard_DashboardBSCView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Guid? Scorecardid = General.GetNullableGuid(Request.QueryString["scorecardid"]);
        ViewState["SCORECARDID"] = Scorecardid;

        DataTable dt1 = PhoenixDashboardBSC.officestafflist();
        radexecutioncb.DataSource = dt1;
        radexecutioncb.DataTextField = "FLDUSERNAME";
        radexecutioncb.DataValueField = "FLDUSERCODE";
        radexecutioncb.DataBind();

        DataTable dt = PhoenixDashboardBSC.scorecarddetails(Scorecardid);
        ViewState["KPIID"] = dt.Rows[0]["FLDKPIID"].ToString();
        ViewState["MONTH"] = dt.Rows[0]["FLDMONTH"].ToString();
        ViewState["YEAR"] = dt.Rows[0]["FLDYEAR"].ToString();
        System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
      

        radlblmonthyear.Text = mfi.GetMonthName(Int32.Parse(dt.Rows[0]["FLDMONTH"].ToString())).ToString()  + ' ' + dt.Rows[0]["FLDYEAR"].ToString();
        BindCheckBoxList(radexecutioncb, dt.Rows[0]["FLDEXECUTIONTEAM"].ToString());
        KPI_Data();
        PI_Data();
        LI_Data();
        Initiatives_Data();
        Issue_Data();
    }

    protected void KPI_Data()
    {
        Guid? kpiid = General.GetNullableGuid(ViewState["KPIID"].ToString());
        int? month = General.GetNullableInteger(ViewState["MONTH"].ToString());
        int? year = General.GetNullableInteger(ViewState["YEAR"].ToString());

        DataTable dt = PhoenixDashboardBSC.KPISearch(kpiid, month, year);

        if (dt.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" <table border=\"0\"  cellspacing=\"0\" class=\"tblstyle\" cellpadding=\"3\" rules=\"all\" style=\"font - size: 11px; width: 100%; border - collapse:collapse;text-wrap:normal \">");

            string[] alColumns = { "FLDREFERENCENO", "FLDKPINAME", "FLDKPIDESCRIPTION", "FLDKPIACHIEVED", "FLDKPITARGETVALUE", "FLDUSERNAME" };
            string[] alCaptions = { "Ref#", "KPI", "KPI Description/Definition", "KPI Acheieved", "KPI Target", "Objective Owner" };
            sb.Append("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                sb.Append("<td align=\"center\" style=\"background-color:#f8cbad\">");
                sb.Append("<b>" + alCaptions[i] + "</b>");
                sb.Append("</td>");
            }
            sb.Append("</tr>");
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    if (alColumns[i] == "FLDREFERENCENO")
                    { sb.Append("<td align=\"center\" style=\"width:5%\">"); }
                    else if (alColumns[i] == "FLDKPINAME")
                    {
                        sb.Append("<td style=\"width:15%\">");
                    }
                    else if (alColumns[i] == "FLDKPIACHIEVED")
                    {
                        sb.Append("<td style=\"width:8.5%\">");
                    }
                    else if (alColumns[i] == "FLDKPIDESCRIPTION")
                    {
                        sb.Append("<td style=\"width:35%\">");
                    }
                    else if (alColumns[i] == "FLDKPITARGETVALUE" || alColumns[i] == "FLDUSERNAME")
                    { sb.Append("<td align=\"center\">"); }
                    else
                    {
                        sb.Append("<td >");
                    }
                    sb.Append(dr[alColumns[i]]);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</TABLE>");



            kpitable.Text = sb.ToString();
        }
        else
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" <table border=\"0\"  cellspacing=\"0\" class=\"tblstyle\" cellpadding=\"3\" rules=\"all\" style=\"font - size: 11px; width: 100%; border - collapse:collapse;text-wrap:normal \">");
            string[] alCaptions = { "Ref#", "KPI", "KPI Description/Definition", "KPI Acheieved", "KPI Target", "Objective Owner" };
            sb.Append("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                sb.Append("<td align=\"center\" style=\"background-color:#f8cbad\">");
                sb.Append("<b>" + alCaptions[i] + "</b>");
                sb.Append("</td>");
            }
            sb.Append("</tr>");

            sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Black;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

            sb.Append("</table>");

            kpitable.Text = sb.ToString();
        }

    }

    protected void PI_Data()
    {
        Guid? kpiid = General.GetNullableGuid(ViewState["KPIID"].ToString());
        int? month = General.GetNullableInteger(ViewState["MONTH"].ToString());
        int? year = General.GetNullableInteger(ViewState["YEAR"].ToString());

        DataTable dt = PhoenixDashboardBSC.PISearch(kpiid, month, year);


        if (dt.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" <table border=\"0\"  cellspacing=\"0\" class=\"tblstyle\" cellpadding=\"3\" rules=\"all\" style=\"font - size: 11px; width: 100%; border - collapse:collapse;text-wrap:normal \">");

            string[] alColumns = { "FLDPICODE", "FLDPINAME", "FLDPIACHIEVED", "FLDISSUE", "FLDIMPLICATION" };
            string[] alCaptions = { "No.", "Performance Indicator(PI)", "Acheieved", "Issues", "Implications" };
            sb.Append("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                if (alColumns[i] == "FLDPICODE")
                { sb.Append("<td align=\"center\" style=\"width:5.75%;background-color:#bdd7ee\" > "); }
                else if (alColumns[i] == "FLDPINAME")
                { sb.Append("<td align=\"center\" style=\"background-color:#bdd7ee\" > "); }
                else
                {
                    sb.Append("<td align=\"center\"style=\"background-color:#bdd7ee\" > ");
                }
                sb.Append("<b>" + alCaptions[i] + "</b>");
                sb.Append("</td>");
            }
            sb.Append("</tr>");
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    if (alColumns[i] == "FLDPICODE")
                    { sb.Append("<td align=\"center\" style=\"width:5.75%;\" > "); }
                    else if (alColumns[i] == "FLDPINAME")
                    {
                        sb.Append("<td style=\"width:41.25%\">");
                    }
                    else if (alColumns[i] == "FLDPIACHIEVED")
                    {
                        sb.Append("<td style=\"width:10%\">");
                    }
                    else if (alColumns[i] == "FLDISSUE")
                    {
                        sb.Append("<td style=\"width:17.25%\">");
                    }
                    else if (alColumns[i] == "FLDKPITARGETVALUE" || alColumns[i] == "FLDUSERNAME")
                    { sb.Append("<td align=\"center\">"); }
                    else
                    { sb.Append("<td>"); }

                    sb.Append(dr[alColumns[i]]);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</TABLE>");



            pitable.Text = sb.ToString();
        }
        else
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table cellspacing=\"0\" class=\"tblstyle\" cellpadding=\"3\" rules=\"all\" border=\"0\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");
            string[] alCaptions = { "No.", "Performance Indicator(PI)", "Acheieved", "Issues", "Implications" };
            string[] alColumns = { "FLDPICODE", "FLDPINAME", "FLDPIACHIEVED", "FLDISSUE", "FLDIMPLICATION" };

            sb.Append("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                if (alColumns[i] == "FLDPICODE")
                { sb.Append("<td align=\"center\" style=\"width:5.75%;background-color:#bdd7ee\" > "); }
                else if (alColumns[i] == "FLDPINAME")
                { sb.Append("<td align=\"center\" style=\"background-color:#bdd7ee\" > "); }
                else
                {
                    sb.Append("<td align=\"center\"style=\"background-color:#bdd7ee\" > ");
                }
                sb.Append("<b>" + alCaptions[i] + "</b>");
                sb.Append("</td>");
            }
            sb.Append("</tr>");

            sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Black;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

            sb.Append("</table>");

            pitable.Text = sb.ToString();
        }

    }

    protected void LI_Data()
    {

        Guid? kpiid = General.GetNullableGuid(ViewState["KPIID"].ToString());
        int? month = General.GetNullableInteger(ViewState["MONTH"].ToString());
        int? year = General.GetNullableInteger(ViewState["YEAR"].ToString());

        DataTable dt = PhoenixDashboardBSC.LISearch(kpiid, month, year);

        if (dt.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" <table border=\"0\" class=\"tblstyle\"  cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" style=\"font - size: 11px; width: 100%; border - collapse:collapse;text-wrap:normal \">");

            string[] alColumns = { "FLDLICODE", "FLDLINAME", "FLDPICODE", "FLDACTIONBY", "FLDFREQUENCY", "FLDCOMPLIANCE" };
            string[] alCaptions = { "No.", "Action Item(Leading Indicator)", "For PI No", "Action By", "Frequency", "Compliance" };
            sb.Append("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                if (alColumns[i] == "FLDLICODE")
                { sb.Append("<td align=\"center\"style=\"width:5.75%;background-color:#ffd966;\" > "); }
                else
                    sb.Append("<td align=\"center\"style=\"background-color:#ffd966;\" > ");

                sb.Append("<b>" + alCaptions[i] + "</b>");
                sb.Append("</td>");
            }
            sb.Append("</tr>");
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    if (alColumns[i] == "FLDCOMPLIANCE")
                    { sb.Append("<td align=\"center\">"); }
                    else if (alColumns[i] == "FLDLICODE")
                    { sb.Append("<td align=\"center\" style=\"width:5.75%;\" > "); }
                    else if (alColumns[i] == "FLDLINAME")
                    {
                        sb.Append("<td style=\"width:41.25%\">");
                    }
                    else if (alColumns[i] == "FLDPICODE")
                    {
                        sb.Append("<td align=\"center\" style=\"width:10%;\" >");
                    }
                    else if (alColumns[i] == "FLDACTIONBY")
                    {
                        sb.Append("<td align=\"center\" style=\"width:17.25%;\" >");
                    }
                    else if (alColumns[i] == "FLDFREQUENCY")
                    {
                        sb.Append("<td align=\"center\" style=\"width:16%;\" >");
                    }
                    else
                    { sb.Append("<td>"); }

                    sb.Append(dr[alColumns[i]]);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</TABLE>");



            tblli.Text = sb.ToString();
        }
        else
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"0\" class=\"tblstyle\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");
            string[] alCaptions = { "No.", "Action Item(Leading Indicator)", "For PI No", "Action By", "Frequency", "Compliance" };
            sb.Append("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                if (alCaptions[i] == "No.")
                { sb.Append("<td align=\"center\"style=\"width:5.75%;background-color:#ffd966;\" > "); }
                else
                    sb.Append("<td align=\"center\"style=\"background-color:#ffd966;\" > ");

                sb.Append("<b>" + alCaptions[i] + "</b>");
                sb.Append("</td>");
            }
            sb.Append("</tr>");

            sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Black;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

            sb.Append("</table>");

            tblli.Text = sb.ToString();
        }



    }

    protected void Initiatives_Data()
    {
        Guid? kpiid = General.GetNullableGuid(ViewState["KPIID"].ToString());
        int? month = General.GetNullableInteger(ViewState["MONTH"].ToString());
        int? year = General.GetNullableInteger(ViewState["YEAR"].ToString());

        DataTable dt = PhoenixDashboardBSCInitiative.InitiativeSearch(kpiid, year, month);

        if (dt.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" <table  class=\"tblstyle\" border=\"0\"  cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" style=\"font - size: 11px; width: 100%; border - collapse:collapse;text-wrap:normal \">");

            string[] alColumns = { "FLDINITIATIVE", "FLDASSIGNEDTO", "FLDTARGRETDATE"};
            string[] alCaptions = { "Details", "Assigned To", "Target Date" };
            sb.Append("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                sb.Append("<td align=\"center\"style=\"background-color:#a9d08e;\" > ");

                sb.Append("<b>" + alCaptions[i] + "</b>");
                sb.Append("</td>");
            }
            sb.Append("</tr>");
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    if (alColumns[i] == "FLDTARGRETDATE" )
                    { sb.Append("<td align=\"center\">"); }
                    else if (alColumns[i] == "FLDINITIATIVE")
                    {
                        sb.Append("<td style=\"width:43.5%;\" >");
                    }
                    else if (alColumns[i] == "FLDASSIGNEDTO")
                    {
                        sb.Append("<td align=\"center\" style=\"width:29%;\" >");
                    }
                    else
                    { sb.Append("<td>"); }
                    if (alColumns[i] == "FLDTARGRETDATE")
                        sb.Append(General.GetDateTimeToString(dr[alColumns[i]]));
                    else
                        sb.Append(dr[alColumns[i]]);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</TABLE>");



            tblini.Text = sb.ToString();
        }
        else
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table cellspacing=\"0\" class=\"tblstyle\" cellpadding=\"3\" rules=\"all\" border=\"0\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");
            string[] alCaptions = { "Details", "Assigned To", "Target Date" };
            sb.Append("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                sb.Append("<td align=\"center\"style=\"background-color:#a9d08e;\" > ");

                sb.Append("<b>" + alCaptions[i] + "</b>");
                sb.Append("</td>");
            }
            sb.Append("</tr>");
            sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Black;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

            sb.Append("</table>");

            tblini.Text = sb.ToString();
        }


    }

    protected void Issue_Data()
    {
        Guid? kpiid = General.GetNullableGuid(ViewState["KPIID"].ToString());
        int? month = General.GetNullableInteger(ViewState["MONTH"].ToString());
        int? year = General.GetNullableInteger(ViewState["YEAR"].ToString());

        DataTable dt = PhoenixDashboardBSC.PIIssueSearch(kpiid, month, year);

        if (dt.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" <table border=\"1\"  cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" style=\"font-size: 11px; width: 100%; border-collapse:collapse;text-wrap:normal \">");

            string[] alColumns = { "FLDISSUE", "FLDIMPLICATION", "FLDACTIONS", "FLDASSIGNEDTO", "FLDTARGETDATE" };
            string[] alCaptions = { "Issues", "Implications", "Actions", "Assigned To", "Target Date" };
            sb.Append("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                sb.Append("<td align=\"center\" > ");

                sb.Append("<b>" + alCaptions[i] + "</b>");
                sb.Append("</td>");
            }
            sb.Append("</tr>");
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    if (alColumns[i] == "FLDTARGETDATE" )
                    { sb.Append("<td align=\"center\">"); }
                    else if (alColumns[i] == "FLDISSUE")
                    {
                        sb.Append("<td style=\"width:20%\">");
                    }
                    else if (alColumns[i] == "FLDIMPLICATION")
                    {
                        sb.Append("<td style=\"width:17.4%\">");
                    }
                    else if (alColumns[i] == "FLDACTIONS")
                    {
                        sb.Append("<td style=\"width:17.4%\">");
                    }
                    else if (alColumns[i] == "FLDASSIGNEDTO")
                    {
                        sb.Append("<td align=\"center\" style=\"width:23.2%\">");
                    }
                    else
                    { sb.Append("<td>"); }
                    if (alColumns[i] == "FLDTARGETDATE")
                        sb.Append(General.GetDateTimeToString(dr[alColumns[i]]));
                    
                    else
                        sb.Append(dr[alColumns[i]]);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</TABLE>");



            issuetable.Text = sb.ToString();
        }
        else
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table cellspacing=\"0\" class=\"tblstyle\" cellpadding=\"3\" rules=\"all\" border=\"0\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");
            string[] alCaptions = { "Issues", "Implications", "Actions", "Assigned To", "Target Date", "Action" };
            sb.Append("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                sb.Append("<td align=\"center\" > ");

                sb.Append("<b>" + alCaptions[i] + "</b>");
                sb.Append("</td>");
            }
            sb.Append("</tr>");
            sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Black;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

            sb.Append("</table>");

            issuetable.Text = sb.ToString();
        }



    }

    public static void BindCheckBoxList(RadComboBox cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                if (cbl.Items.FindItemByValue(item) != null)
                    cbl.Items.FindItemByValue(item).Checked = true;
            }
        }

    }

    protected void Tabkpi_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("DRAFT"))
            {
                Response.Redirect("~/Dashboard/DashboardBSCKPI.aspx");
            }
            if (CommandName.ToUpper().Equals("SC"))
            {

                Response.Redirect("~/Dashboard/DashboardBSCSearch.aspx");
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
}