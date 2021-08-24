using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselAccounts;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using SouthNests.Phoenix.Reports;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Configuration;

public partial class Crew_CrewAppraisalPendingSeafarercomments : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["FROMEMAILID"] = null;
                gvAQ.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewAppraisalPendingSeafarercomments.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAQ')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewAppraisalPendingcommentSearch.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbar.AddFontAwesomeButton("../Crew/CrewAppraisalPendingSeafarercomments.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Crew/CrewAppraisalPendingSeafarercomments.aspx", "Send bulk email", "<i class=\"fas fa-envelope\"></i>", "SENDBULKEMAIL");
            //if (ViewState["CANEDIT"].Equals("1") && !String.IsNullOrEmpty(ViewState["VSLID"].ToString()))
            //    toolbar.AddImageButton("../Crew/CrewAppraisal.aspx", "Add New Appraisal", "add.png", "ADD");
            //else if (ViewState["CANEDIT"].Equals("1") && String.IsNullOrEmpty(ViewState["VSLID"].ToString()))
            //    toolbar.AddImageButton("../Crew/CrewAppraisal.aspx", "Add Appraisal from PM", "add.png", "CREWADD");

            MenuCrewAppraisal.AccessRights = this.ViewState;
            MenuCrewAppraisal.MenuList = toolbar.Show();

          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewAppraisalMain_TabStripCommand(object sender, EventArgs e)
    {
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvAQ.Rebind();
      
    }
    protected void MenuCrewAppraisal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentPendingCommentsFilter=null;
                BindData();
                gvAQ.Rebind();
            }
            if (CommandName.ToUpper().Equals("SENDBULKEMAIL"))
            {
                int rowcount = gvAQ.Items.Count;
                if (rowcount > 0)
                {

                    for (int i = 0; i < rowcount; i++)
                    {
                        if (((CheckBox)gvAQ.Items[i].FindControl("ckbselect")).Checked)
                        {
                            Guid appraisalid = new Guid(((RadLabel)gvAQ.Items[i].FindControl("lblAppraisalId")).Text);
                            int employeeid = int.Parse(((RadLabel)gvAQ.Items[i].FindControl("lblemployeeid")).Text);
                            int vesselid = int.Parse(((RadLabel)gvAQ.Items[i].FindControl("lblvesselid")).Text);
                            string activeyn = ((RadLabel)gvAQ.Items[i].FindControl("lblactiveyn")).Text;
                            string employeename = ((RadLabel)gvAQ.Items[i].FindControl("lblempname")).Text;
                            string emprank = ((RadLabel)gvAQ.Items[i].FindControl("lblRank")).Text;
                            int? emprankid = General.GetNullableInteger(((RadLabel)gvAQ.Items[i].FindControl("lblRankid")).Text);
                            if (activeyn == "2")
                            {
                                Sendmailforcrewcomments(employeeid, appraisalid, vesselid, employeename, emprank,emprankid);
                            }
                        }
                    }
                }
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDNAME", "FLDFROMDATE", "FLDTODATE", "FLDAPPRAISALDATE", "FLDOCCASSIONFORREPORT", "FLDRANKCODE", "FLDPROMOTIONYESNO", "FLDRECOMMENDEDSTATUSNAME", "FLDMAILCOUNT" };
        string[] alCaptions = { "Vessel", "Name", "From", "To", "Appraisal Date", "Occassion", "Rank", "Promotion Y/N", "Fit Y/N", "Sent Email Count" };
        string sortexpression;
        int? sortdirection = 1;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentPendingCommentsFilter;

        DataSet ds = PhoenixCrewAppraisal.SearchPendingCommentAppraisal(
              nvc != null ? nvc["txtFileNo"] : null
            , General.GetNullableInteger(nvc != null ? nvc["ucVessel"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
              , General.GetNullableDateTime(nvc != null ? nvc["txtaprFrom"] : null)
              , General.GetNullableDateTime(nvc != null ? nvc["txtaprTo"] : null)
              , nvc != null ? nvc["ucPool"] : null
              , nvc != null ? nvc["ucZone"] : null
              , General.GetNullableInteger(nvc != null ? nvc["ucRank"] : null)
              , sortdirection
              , (int)ViewState["PAGENUMBER"]
              , General.ShowRecords(null)
              , ref iRowCount
              , ref iTotalPageCount
              );

        General.ShowExcel("Pending Seafarer Comments", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDNAME", "FLDFROMDATE", "FLDTODATE", "FLDAPPRAISALDATE", "FLDOCCASSIONFORREPORT", "FLDRANKCODE", "FLDPROMOTIONYESNO", "FLDRECOMMENDEDSTATUSNAME", "FLDMAILCOUNT" };
        string[] alCaptions = { "Vessel", "Name", "From", "To", "Appraisal Date", "Occassion", "Rank", "Promotion Y/N", "Fit Y/N", "Sent Email Count" };
        int? sortdirection = 1; //DEFAULT DESC SORT
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {
            NameValueCollection nvc = Filter.CurrentPendingCommentsFilter;

            DataSet ds = PhoenixCrewAppraisal.SearchPendingCommentAppraisal(
               nvc != null ? nvc["txtFileNo"] : null
             , General.GetNullableInteger(nvc != null ? nvc["ucVessel"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
               , General.GetNullableDateTime(nvc != null ? nvc["txtaprFrom"] : null)
               , General.GetNullableDateTime(nvc != null ? nvc["txtaprTo"] : null)
               , nvc != null ? nvc["ucPool"] : null
               , nvc != null ? nvc["ucZone"] : null
               , General.GetNullableInteger(nvc != null ? nvc["ucRank"] : null)
               , sortdirection
               , (int)ViewState["PAGENUMBER"]
               , gvAQ.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               );
            General.SetPrintOptions("gvAQ", "Pending Seafarer Comments", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAQ.DataSource = ds.Tables[0];
           

                ViewState["FROMEMAILID"] = ds.Tables[0].Rows[0]["FLDFROMMAILID"].ToString();
                ViewState["FROMNAME"] = ds.Tables[0].Rows[0]["FLDFROMNAME"].ToString();
                if (Filter.CurrentAppraisalSelection == null)
                {
                    //gvAQ.SelectedIndex = 0;
                    Filter.CurrentAppraisalSelection = ds.Tables[0].Rows[0]["FLDCREWAPPRAISALID"].ToString();
                }
            }
            else
            {
                Filter.CurrentAppraisalSelection = null;
                gvAQ.DataSource = ds.Tables[0];
            }
            gvAQ.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Sendmailforcrewcomments(int employeeid, Guid appraisalid, int vesselid, string empname, string emprank, int? rankid)
    {
        string emailid;
        try
        {
            emailid = "";
            DataSet ds = new DataSet();
            ds = PhoenixCrewAppraisal.selectedcrewmailid(employeeid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                string emailbodytext = "";
                string supemail = "";

                if (ds.Tables[0].Rows[0]["FLDEMAIL"].ToString() != null || ds.Tables[0].Rows[0]["FLDEMAIL"].ToString() != "")
                {
                    emailid = ds.Tables[0].Rows[0]["FLDEMAIL"].ToString();
                }
                if (ds.Tables[0].Rows[1]["FLDEMAIL"].ToString() != null || ds.Tables[0].Rows[1]["FLDEMAIL"].ToString() != "")
                {
                    supemail = ViewState["FROMEMAILID"].ToString();
                }
                try
                {
                    PhoenixCrewAppraisal.InsertWebSession(appraisalid, PhoenixSecurityContext.CurrentSecurityContext.UserCode, "CFC", employeeid.ToString());

                    emailbodytext = PrepareEmailBodyTextForcrew(empname, emprank, appraisalid, employeeid, rankid, vesselid);

                    PhoenixCommoneProcessing.PrepareEmailMessage(
                                emailid, "CFC", appraisalid,
                                "", supemail.Equals("") ? emailid : supemail,
                                empname, "To fill seafarer comments for appraisal.",
                                emailbodytext, "", "");

                    ucConfirm.ErrorMessage = "Email sent to " + empname + "\n";
                    ucConfirm.Visible = true;
                    PhoenixCrewAppraisal.insertmailhistory(employeeid, appraisalid, emailid, supemail);
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected string PrepareEmailBodyTextForcrew(string employeename, string rank, Guid appraisalid, int employeeid, int? rankid,int vesselid)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixCrewAppraisal.GetApraisalDataForEmailBody(PhoenixSecurityContext.CurrentSecurityContext.UserCode, appraisalid, "CFC");
        DataRow dr = ds.Tables[0].Rows[0];

        string fromemail = "";

        if (ConfigurationManager.AppSettings["fromemail"] != null)
        {
            fromemail = ConfigurationManager.AppSettings["fromemail"].ToString();
        }

        sbemailbody.Append("Good Day,");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Please note,the appraisal for " + rank + " - " + employeename + " has been completed.");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append("Please click on the link below and key in the relevant fields indicated. If the link is wrapped, please copy and paste it on the address bar of your browser");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        string url = "\"<" + Session["sitepath"] + "/" + dr["URL"].ToString();
        sbemailbody.AppendLine(url + "?aprid=" + appraisalid + ">\"");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append("Would request you to kindly go through and fill in the seafarer comments and click save on that screen.");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thanks and Regards,");
        sbemailbody.AppendLine(ViewState["FROMNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("This is an automated message.");
        sbemailbody.AppendLine("If you need personal attention, use \"reply all\" to get your communication across to an email id that is monitored.");
        sbemailbody.AppendLine("Please note "+ fromemail +" is NOT monitored.");

        return sbemailbody.ToString();

    }
  

    private bool IsValidateAppraisal(string vessel, string fromdate, string todate, string occassion, string appraisaldate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        int result;
        DateTime resultdate;
        if (!int.TryParse(vessel, out result))
            ucError.ErrorMessage = "Vessel is required.";
        if (fromdate == null || !DateTime.TryParse(fromdate, out  resultdate))
            ucError.ErrorMessage = "From Date is required";
        if (todate == null || !DateTime.TryParse(todate, out resultdate))
            ucError.ErrorMessage = "To Date is required";
        else if (!string.IsNullOrEmpty(fromdate)
              && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }
        if (occassion.ToUpper() == "DUMMY" || occassion == "")
        {
            ucError.Text = "Please Select Occassion For Report";
        }
        if (appraisaldate == null || !DateTime.TryParse(appraisaldate, out  resultdate))
            ucError.ErrorMessage = "Appraisal Date is required";

        return (!ucError.IsError);
    }

    protected void gvAQ_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAQ.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAQ_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SORT") return;
     
        if (e.CommandName.ToString().ToUpper() == "SELECT")
        {
            Filter.CurrentAppraisalSelection = ((RadLabel)e.Item.FindControl("lblAppraisalId")).Text;
            int vesselid = int.Parse(((RadLabel)e.Item.FindControl("lblvesselid")).Text);
            int employeeid = int.Parse(((RadLabel)e.Item.FindControl("lblemployeeid")).Text);

            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                Response.Redirect("~/CrewOffshore/CrewOffshoreAppraisalActivity.aspx?&vslid=" + vesselid + "&empid=" + employeeid, false);
            else
                Response.Redirect("CrewAppraisalActivity.aspx?&vslid=" + vesselid, false);
        }
        if (e.CommandName.ToString().ToUpper() == "SENDMAILTOCREW")
        {
        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvAQ_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        string mnu = Filter.CurrentMenuCodeSelection;
        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
             && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                        db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    else if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                        db.Visible = false;
                }

                LinkButton email = (LinkButton)e.Item.FindControl("cmdsendmail");
                if (email != null)
                {
                    if (drv["FLDACTIVEYN"].ToString().Equals("2"))
                    {
                        email.Visible = true;
                        email.Visible = SessionUtil.CanAccess(this.ViewState, email.CommandName);

                        email.Attributes.Add("onclick", "javascript:return openNewWindow('VesselAccounts','','"+Session["sitepath"]+"/CrewOffshore/CrewOffshoreAppraisalMail.aspx?employeeid=" + drv["FLDEMPLOYEEID"].ToString() + "&vesselid=" + drv["FLDVESSELID"].ToString() +
                            "&apprid=" + drv["FLDCREWAPPRAISALID"].ToString() + "&rankid=" + drv["FLDRANKID"].ToString() + "'); return false;");

                    }
                }
            }

            UserControlOccassionForReport ddlOccassionForReportedit = (UserControlOccassionForReport)e.Item.FindControl("ddlOccassionForReportedit");

            if (ddlOccassionForReportedit != null)
                ddlOccassionForReportedit.SelectedOccassion = drv["FLDOCCASIONID"].ToString();

            UserControlVessel ucVessel = (UserControlVessel)e.Item.FindControl("ddlVesselEdit");

            if (ucVessel != null)
            {
                ucVessel.SelectedVessel = drv["FLDVESSELID"].ToString();
                if (mnu.Contains("VAC-")) ucVessel.Enabled = false;
            }

        }
      
        if (e.Item is GridFooterItem)
        {
            UserControlVessel vsl = (UserControlVessel)e.Item.FindControl("ddlVesselAdd");
            if (vsl != null && mnu.Contains("VAC-")) { vsl.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString(); vsl.Enabled = false; }
        }
    }

    protected void gvAQ_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
