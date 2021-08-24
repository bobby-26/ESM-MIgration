using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.ShippingKPI;

public partial class Dashboard_DashboardBSCKPI : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Draft", "Draft", ToolBarDirection.Left);
        toolbar.AddButton("Score Cards", "SC", ToolBarDirection.Left);

        Tabkpi.MenuList = toolbar.Show();
        Tabkpi.SelectedMenuIndex = 0;

        PhoenixToolbar toolbargrid = new PhoenixToolbar();

        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardBSCKPI.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");
        toolbargrid.AddLinkButton("javascript:parent.openNewWindow('issue', 'Add Issue', 'Dashboard/DashboardBSCIssue.aspx?kpiid=" + RadComboKpilist.Value + "', 'false', '720px', '430px'); return false; ", "Add Issue", "Toggle2", ToolBarDirection.Left);
        toolbargrid.AddLinkButton("javascript:parent.openNewWindow('initiative', 'Add Initiative', 'Dashboard/DashboardBSCInitiative.aspx?kpiid=" + RadComboKpilist.Value + "&edit=no" + "', 'false', '720px', '330px'); return false; ", "Add Initiative", "Toggle3", ToolBarDirection.Left);
        toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        TabScorecard.MenuList = toolbargrid.Show();

        if (!Page.IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
           
            radcbmonth.SelectedMonth = DateTime.Now.Month.ToString();
            radcbyear.SelectedYear = DateTime.Now.Year;

            DataTable dt = PheonixDashboardSKKPI.Departmentlist();

            radcbdept.DataSource = dt;
            radcbdept.DataTextField = "FLDDEPARTMENTNAME";
            radcbdept.DataValueField = "FLDDEPARTMENTID";
            radcbdept.DataBind();

            DataTable dt1 = PhoenixDashboardBSC.officestafflist();
            radexecutioncb.DataSource = dt1;
            radexecutioncb.DataTextField = "FLDUSERNAME";
            radexecutioncb.DataValueField = "FLDUSERCODE";
            radexecutioncb.DataBind();

            bingkpidropdown();

        }


        if (RadComboKpilist.Value == string.Empty)
        {

            pidiv.Style.Add("visibility", "hidden");
            lidiv.Style.Add("visibility", "hidden");
            initiativetbl.Style.Add("visibility", "hidden");
            tbexecution.Style.Add("visibility", "hidden");
            kpitable.Visible = false;
            issuetable.Visible = false;
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

    protected void RadRadioButtonkpilevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        bingkpidropdown();
    }

    protected void bingkpidropdown()
    {
        DataTable dt = PheonixDashboardSKKPI.KPIList(RadRadioButtonkpilevel.SelectedValue, General.GetNullableInteger(radcbdept.SelectedValue));
        RadComboKpilist.DataSource = dt;
        RadComboKpilist.DataBind();
        RadComboKpilist.Text = string.Empty;
        RadComboKpilist.Value = null;

    }

    protected void TabScorecard_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                if (!IsValidSearch(General.GetNullableGuid(RadComboKpilist.Value), General.GetNullableInteger(radcbmonth.SelectedMonth), General.GetNullableInteger(radcbyear.SelectedYear.ToString())))
                {
                    ucError.Visible = true;
                    return;
                }
               
                pidiv.Style.Add("visibility", "Visible");
                lidiv.Style.Add("visibility", "Visible");
                initiativetbl.Style.Add("visibility", "Visible");
                tbexecution.Style.Add("visibility", "Visible");
                kpitable.Visible = true;
                issuetable.Visible = true;
                //gvissue.Rebind();
                //gvinitiative.Rebind();
                tblli.Text = string.Empty;
                kpitable.Text = string.Empty;
                pitable.Text = string.Empty;
                tblini.Text = string.Empty;
                issuetable.Text = string.Empty;
                KPI_Data();
                PI_Data();
                LI_Data();
                Initiatives_Data();
                Issue_Data();
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                Guid? KPIid = General.GetNullableGuid(RadComboKpilist.Value);
                string team = string.Empty;
                team = General.GetNullableString(GetCheckedItemsvalues(radexecutioncb, team));
                int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());
                int? month = General.GetNullableInteger(radcbmonth.SelectedMonth.ToString());
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string level = RadRadioButtonkpilevel.SelectedValue;
                if (!IsValidSearch(KPIid, year, month, team))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixDashboardBSC.KPISCInsert(rowusercode, KPIid, month, year, team, level);
                ucstatus.Text = "Sucessfully Saved";
                ucstatus.Visible = true;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private bool IsValidSearch(Guid? KPIid, int? month, int? year, string team)
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (KPIid == null)
        {
            ucError.ErrorMessage = "KPI.";
        }

        if (month == null)
        {
            ucError.ErrorMessage = "Month.";
        }
        if (year == null)
        {
            ucError.ErrorMessage = "Year.";
        }

        if (team == null)
        {
            ucError.ErrorMessage = "Team.";
        }
        return (!ucError.IsError);
    }
    private bool IsValidSearch(Guid? KPIid, int? month, int? year)
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (KPIid == null)
        {
            ucError.ErrorMessage = "KPI.";
        }

        if (month == null)
        {
            ucError.ErrorMessage = "Month.";
        }
        if (year == null)
        {
            ucError.ErrorMessage = "Year.";
        }

       
        return (!ucError.IsError);
    }
    protected static string GetCheckedItemsvalues(RadComboBox comboBox, string checkednames)
    {

        var sb = new StringBuilder();
        var collection = comboBox.CheckedItems;

        if (collection.Count != 0)
        {


            foreach (var item in collection)
                sb.Append(item.Value + ",");


            checkednames = sb.ToString();
        }

        return checkednames;


    }
    protected void KPI_Data()
    {
        Guid? kpiid = General.GetNullableGuid(RadComboKpilist.Value);
        int? month = General.GetNullableInteger(radcbmonth.SelectedMonth);
        int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());

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
        Guid? kpiid = General.GetNullableGuid(RadComboKpilist.Value);
        int? month = General.GetNullableInteger(radcbmonth.SelectedMonth);
        int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());

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

        Guid? kpiid = General.GetNullableGuid(RadComboKpilist.Value);
        int? month = General.GetNullableInteger(radcbmonth.SelectedMonth);
        int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());

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

    protected void radcbdept_TextChanged(object sender, EventArgs e)
    {
        bingkpidropdown();
    }



    protected void Initiatives_Data()
    {
        Guid? kpiid = General.GetNullableGuid(RadComboKpilist.Value);
        int? month = General.GetNullableInteger(radcbmonth.SelectedMonth);
        int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());

        DataTable dt = PhoenixDashboardBSCInitiative.InitiativeSearch(kpiid, year, month);

        if (dt.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" <table  class=\"tblstyle\" border=\"0\"  cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" style=\"font - size: 11px; width: 100%; border - collapse:collapse;text-wrap:normal \">");

            string[] alColumns = { "FLDINITIATIVE", "FLDASSIGNEDTO", "FLDTARGRETDATE", "" };
            string[] alCaptions = { "Details", "Assigned To", "Target Date", "Action" };
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
                    if (alColumns[i] == "FLDTARGRETDATE" || alColumns[i] == "")
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
                    else if (alColumns[i] == "")
                    {
                        sb.Append("<a href=" + "javascript:parent.openNewWindow('initiative','Initiative','Dashboard/DashboardBSCInitiative.aspx?initiativeid=" + dr["FLDINITIATIVEID"] + "&edit=yes" + "','false','720px','330px');" + " ToolTip=" + "Edit" + "><span class=" + "icon" + "><i class=\"fas fa-edit\"></i></span></a>");
                    }
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



    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {


            PI_Data();
            Initiatives_Data();
            Issue_Data();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Issue_Data()
    {
        Guid? kpiid = General.GetNullableGuid(RadComboKpilist.Value);
        int? month = General.GetNullableInteger(radcbmonth.SelectedMonth);
        int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());

        DataTable dt = PhoenixDashboardBSC.PIIssueSearch(kpiid, month, year);

        if (dt.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" <table border=\"1\"  cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" style=\"font-size: 11px; width: 100%; border-collapse:collapse;text-wrap:normal \">");

            string[] alColumns = { "FLDISSUE", "FLDIMPLICATION", "FLDACTIONS", "FLDASSIGNEDTO", "FLDTARGETDATE", "" };
            string[] alCaptions = { "Issues", "Implications", "Actions", "Assigned To", "Target Date", "Action" };
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
                    if (alColumns[i] == "FLDTARGETDATE" || alColumns[i] == "")
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
                    else if (alColumns[i] == "")
                    {
                        sb.Append("<a href=" + "javascript:parent.openNewWindow('issue','Issue','Dashboard/DashboardBSCIssue.aspx?issueid=" + dr["FLDISSUEID"] + "&edit=yes" + "','false','720px','430px');" + " ToolTip=" + "Edit" + "><span class=" + "icon" + "><i class=\"fas fa-edit\"></i></span></a>");
                    }
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


}