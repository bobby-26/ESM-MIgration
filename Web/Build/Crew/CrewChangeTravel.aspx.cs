using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web;

public partial class CrewChangeTravel : PhoenixBasePage
{
    string strVesselId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (!IsPostBack)
            {
                //ucConfirm.Visible = false;
                SessionUtil.PageAccessRights(this.ViewState);

                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                cmdHiddenPick.Attributes.Add("style", "display:none");

                ViewState["REQUSERCANCELLED"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 130, "CND");
                ViewState["REQUSERISSUED"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 130, "ISS");
                ViewState["REQPENDING"] = PhoenixCommonRegisters.GetHardCode(1, 130, "TRQ");
                ViewState["REISSUED"] = PhoenixCommonRegisters.GetHardCode(1, 130, "ISS");

                SetInformation();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["RPAGENUMBER"] = 1;

                ViewState["EDITROW"] = "0";
                ViewState["CURRENTROW"] = null;
                ViewState["DTKEY"] = null;

                ViewState["port"] = null;
                ViewState["vessel"] = null;
                ViewState["date"] = null;
                ViewState["travelid"] = null;
                ViewState["from"] = Request.QueryString["from"].ToString();

                if (Request.QueryString["port"] != null)
                {
                    ViewState["port"] = Request.QueryString["port"].ToString();
                    ucport.SelectedValue = Request.QueryString["port"].ToString();
                }

                if (Request.QueryString["vessel"] != null)
                {
                    ViewState["vessel"] = Request.QueryString["vessel"].ToString();
                    strVesselId = ViewState["vessel"].ToString();
                }

                if (Request.QueryString["date"] != null)
                {
                    ViewState["date"] = Request.QueryString["date"].ToString();
                }

                if (Request.QueryString["travelid"] != null)
                    ViewState["travelid"] = Request.QueryString["travelid"].ToString();

                if (Request.QueryString["travelrequestedit"] != null)
                    ViewState["EDITTRAVELREQUEST"] = Request.QueryString["travelrequestedit"].ToString();

                BindVesselAccount();
                BindBreakUpSeafarer();
                BindTravelRequest();

                gvRecentTravel.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvCCT.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            if (ViewState["from"].ToString() == "crewchange")
            {
                toolbar.AddButton("List", "VESSELLIST", ToolBarDirection.Right);
                // toolbar.AddButton("Crew Change", "CREWCHANGE");
                CrewMenu.AccessRights = this.ViewState;
                CrewMenu.MenuList = toolbar.Show();
            }
            if (ViewState["from"].ToString() == "TRAVEL")
            {
                toolbar.AddButton("Back", "TRAVELPLAN", ToolBarDirection.Right);
                CrewMenu.AccessRights = this.ViewState;
                CrewMenu.MenuList = toolbar.Show();
            }

            ViewState["Offshore"] = null;

            if (Request.QueryString["launchedfrom"] != null)
                ViewState["Offshore"] = Request.QueryString["launchedfrom"].ToString();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewChangeTravel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCCT')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelPlanFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewChangeTravel.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuBreakUpAssign.AccessRights = this.ViewState;
            MenuBreakUpAssign.MenuList = toolbar.Show();

            PhoenixToolbar toolbarRecent = new PhoenixToolbar();
            toolbarRecent.AddFontAwesomeButton("../Crew/CrewChangeTravel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarRecent.AddFontAwesomeButton("javascript:CallPrint('gvRecentTravel')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarRecent.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelRecentFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarRecent.AddFontAwesomeButton("../Crew/CrewChangeTravel.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuRecentTravel.AccessRights = this.ViewState;
            MenuRecentTravel.MenuList = toolbarRecent.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CREWCHANGE"))
            {
                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                    Response.Redirect("../Crew/CrewChangeRequest.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), false);
                else
                    Response.Redirect("CrewChangeRequest.aspx" + (Request.QueryString["access"] != null ? "?access=1" : string.Empty), false);
            }

            if (CommandName.ToUpper().Equals("VESSELLIST"))
            {
                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                    Response.Redirect("../CrewOffshore/CrewOffshoreReliefPlan.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), false);
                else
                    Response.Redirect("../Crew/CrewChangePlanFilter.aspx" + (Request.QueryString["access"] != null ? "?access=1" : string.Empty), false);
            }
            if (CommandName.ToUpper().Equals("TRAVELPLAN"))
            {
                Response.Redirect("../Crew/CrewTravelRequestGeneral.aspx?travelid=" + ViewState["travelid"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }

            if (CommandName.ToUpper().Equals("TRAVELREQUEST"))
            {
                Response.Redirect("CrewTravelRequest.aspx", false);

            }
            if (CommandName.ToUpper().Equals("AGENT"))
            {

                ViewState["TRAVELID"] = Request.QueryString["travelid"];
                Response.Redirect("../Crew/CrewTravelQuotationAgentDetail.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "&TRAVELREQUESTID=" + ViewState["TRAVELREQUESTID"].ToString() + "&port=" + ViewState["port"].ToString()
                    + "&date=" + General.GetDateTimeToString(txtDateOfCrewChange.Text)
                    + "&vessel=" + ViewState["vessel"]);

            }
            if (CommandName.ToUpper().Equals("TICKET"))
            {

                ViewState["TRAVELID"] = Request.QueryString["travelid"];
                Response.Redirect("../Crew/CrewTravelQuoteTicketList.aspx?TRAVELID=" + ViewState["TRAVELID"].ToString() + "&port=" + Request.QueryString["port"].ToString()
                    + "&date=" + General.GetDateTimeToString(Request.QueryString["date"])
                    + "&vessel=" + Request.QueryString["vessel"]);

            }
            if (CommandName.ToUpper().Equals("INVOICE"))
            {
                ViewState["TRAVELID"] = Request.QueryString["travelid"];
                Response.Redirect("CrewTravelInvoice.aspx?travelid=" + ViewState["TRAVELID"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void GenerateTravel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GENERATETRAVELREQ"))
            {


            }
            if (CommandName.ToUpper().Equals("NEXT"))
            {
                ViewState["TRAVELID"] = Request.QueryString["travelid"];
                Response.Redirect("../Crew/CrewTravelQuotationAgentDetail.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "&TRAVELREQUESTID=" + ViewState["TRAVELREQUESTID"].ToString() + "&port=" + ViewState["port"].ToString()
                    + "&date=" + General.GetDateTimeToString(txtDateOfCrewChange.Text)
                    + "&vessel=" + ViewState["vessel"]);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected string PrepareEmailBodyText(string formno, string sendto, string username, string passengers)
    {

        StringBuilder sbemailbody = new StringBuilder();
        sbemailbody.Append("Dear  " + sendto + " ,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby inform you that, travel Request[Requisition NO : " + formno + "] has been intiated for the below crew members");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine(passengers + "<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append(username);
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"] );
        sbemailbody.AppendLine("(As Agents for Owners)");
        sbemailbody.AppendLine("<br/>");

        return sbemailbody.ToString();
    }

    private void SendMail(Guid? Crewtravelid)
    {
        try
        {
            string emailbodytext = "";
            DataSet ds = new DataSet();
            ds = PhoenixCrewTravelRequest.travelrequestmailsearch(Crewtravelid);
            DataRow dr = ds.Tables[0].Rows[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                emailbodytext = PrepareEmailBodyText(dr["FLDREQUISITIONNO"].ToString(), dr["FLDSENDTO"].ToString()
                                                    , dr["FLDSENDBY"].ToString(), dr["FLDPASSENGERS"].ToString());

                PhoenixMail.SendMail(ds.Tables[0].Rows[0]["FLDTRAVELPICEMAIL"].ToString(), ds.Tables[0].Rows[0]["FLDEMAIL2"].ToString().TrimEnd(','), null, ds.Tables[0].Rows[0]["FLDSUBJECT"].ToString()
                                            , emailbodytext, true, System.Net.Mail.MailPriority.Normal, "");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetInformation()
    {
        if (Request.QueryString["from"].ToString() == "travel")
        {
            DataSet ds = PhoenixCrewTravelRequest.EditTravel(new Guid(Request.QueryString["travelid"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtVessel.Text = dr["FLDVESSELNAME"].ToString();
                Filter.CurrentTraveltoVesselName = txtVessel.Text;
                txtDateOfCrewChange.Text = String.Format("{0:dd/MM/yyyy}", General.GetNullableDateTime(dr["FLDDATEOFCREWCHANGE"].ToString()));
                ucport.SelectedValue = dr["FLDSEAPORTID"].ToString();
                ucport.Text = dr["FLDPORTNAME"].ToString();
                ViewState["vessel"] = dr["FLDVESSELID"].ToString();
                ViewState["port"] = dr["FLDPORTOFCREWCHANGE"].ToString();
                string vsl = "";
                if (Filter.CurrentTraveltoVesselName != null)
                    vsl = Filter.CurrentTraveltoVesselName.ToString();
                //Title1.Text = Title1.Text + " (" + dr["FLDREQUISITIONNO"].ToString() + " ) Vessel: " + vsl.ToString();
            }
        }
        else
        {
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(Request.QueryString["vessel"].ToString()));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                txtDateOfCrewChange.Text = String.Format("{0:dd/MM/yyyy}", General.GetNullableDateTime(Request.QueryString["date"].ToString()));
                ucCrewChangeReason.SelectedValue = "1";
                if (Request.QueryString["port"].ToString() != null && Request.QueryString["port"].ToString() != "Dummy" && Request.QueryString["port"].ToString() != string.Empty)
                {
                    string port = Request.QueryString["port"].ToString();

                    DataTable dt = PhoenixRegistersSeaport.EditSeaport(General.GetNullableInteger(port));
                    ucport.SelectedValue = dt.Rows[0]["FLDSEAPORTID"].ToString();
                    ucport.Text = dt.Rows[0]["FLDSEAPORTNAME"].ToString();
                }
            }
        }
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER","FLDFILENO", "FLDNAME","FLDFNAME","FLDRANKNAME", "FLDONSIGNERYESNO", "FLDDATEOFBIRTH", "FLDPASSPORTNO","FLDSEAMANBOOKNO","FLDUSVISANUMBER", "FLDOTHERVISADETAILS","FLDPAYMENTMODENAME",
                                "FLDORIGIN", "FLDDESTINATION", "FLDTRAVELDATE", "FLDARRIVALDATE"};
            string[] alCaptions = { "S.No","File No", "Passenger","NOK of Employee","Rank", "On/Off-Signer", "D.O.B", "PP No","CDC No","US Visa", "Other Visa","Payment", "Origin", "Destination", "Departure",
                                    "Arrival"};

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            string travelid = (ViewState["travelid"] == null) ? "" : ViewState["travelid"].ToString();

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentTravelPlanFilter;

            DataSet ds = PhoenixCrewTravelRequest.SearchTravelRequest(
                int.Parse(ViewState["vessel"].ToString())
                , General.GetNullableInteger(ViewState["port"].ToString())
                , General.GetNullableDateTime(txtDateOfCrewChange.Text)
                , General.GetNullableGuid(travelid)
                //(nvc != null ? nvc["txtReqNo"] : string.Empty)
                , (nvc != null ? nvc["txtFileno"] : string.Empty)
                , (nvc != null ? nvc["txtName"] : string.Empty),
                General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty),
                General.GetNullableInteger(nvc != null ? nvc["ucZone"] : string.Empty),
                sortexpression, sortdirection,
                int.Parse(ViewState["PAGENUMBER"].ToString()),
                gvCCT.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("gvCCT", "Travel Plan", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCCT.DataSource = ds.Tables[0];
                gvCCT.VirtualItemCount = iRowCount;
            }
            else
            {
                gvCCT.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindRecentTravel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER","FLDREQUISITIONNO","FLDFILENO" ,"FLDNAME","FLDFNAME","FLDRANKNAME", "FLDONSIGNERYESNO", "FLDDATEOFBIRTH", "FLDZONE","FLDPASSPORTNO","FLDSEAMANBOOKNO","FLDUSVISANUMBER", "FLDOTHERVISADETAILS","FLDPAYMENTMODENAME",
                                "FLDORIGIN", "FLDDESTINATION", "FLDTRAVELDATE", "FLDARRIVALDATE","FLDSTATUS"};
            string[] alCaptions = { "S.No","Request No","File No", "Passenger","NOK of Employee","Rank", "On/Off-Signer", "D.O.B","Zone" ,"PP No","CDC No","US Visa", "Other Visa","Payment", "Origin", "Destination", "Departure",
                                    "Arrival","Status"};

            string travelid = (ViewState["travelid"] == null) ? "" : ViewState["travelid"].ToString();

            NameValueCollection nvc = Filter.CurrentTravelRecentFilter;

            DataSet ds = PhoenixCrewTravelRequest.SearchTravelRecentRequest(
                int.Parse(ViewState["vessel"].ToString()),
                  (nvc != null ? nvc["txtReqNo"] : string.Empty)
                , (nvc != null ? nvc["txtFileno"] : string.Empty)
                , (nvc != null ? nvc["txtName"] : string.Empty),
                General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty),
                General.GetNullableInteger(nvc != null ? nvc["ucZone"] : string.Empty),
                (int)ViewState["RPAGENUMBER"],
                gvRecentTravel.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("gvRecentTravel", "Recent Travel Request", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvRecentTravel.DataSource = ds.Tables[0];
                gvRecentTravel.VirtualItemCount = iRowCount;
            }
            else
            {
                gvRecentTravel.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void TravelRecent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["RPAGENUMBER"] = 1;
                Filter.CurrentTravelRecentFilter = null;
                BindRecentTravel();
                gvRecentTravel.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDROWNUMBER","FLDREQUISITIONNO","FLDFILENO" ,"FLDNAME","FLDFNAME","FLDRANKNAME", "FLDONSIGNERYESNO", "FLDDATEOFBIRTH", "FLDZONE","FLDPASSPORTNO","FLDSEAMANBOOKNO","FLDUSVISANUMBER", "FLDOTHERVISADETAILS","FLDPAYMENTMODENAME",
                                "FLDORIGIN", "FLDDESTINATION", "FLDTRAVELDATE", "FLDARRIVALDATE","FLDSTATUS"};
                string[] alCaptions = { "S.No","Request No","File No", "Passenger","NOK of Employee","Rank", "On/Off-Signer", "D.O.B","Zone" ,"PP No","CDC No","US Visa", "Other Visa","Payment", "Origin", "Destination", "Departure",
                                    "Arrival","Status"};

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


                string travelid = (ViewState["travelid"] == null) ? "" : ViewState["travelid"].ToString();

                NameValueCollection nvc = Filter.CurrentTravelRecentFilter;

                DataSet ds = PhoenixCrewTravelRequest.SearchTravelRecentRequest(
                    int.Parse(ViewState["vessel"].ToString()),
                       (nvc != null ? nvc["txtReqNo"] : string.Empty)
                , (nvc != null ? nvc["txtFileno"] : string.Empty)
                , (nvc != null ? nvc["txtName"] : string.Empty),
                General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty),
                General.GetNullableInteger(nvc != null ? nvc["ucZone"] : string.Empty),
                    (int)ViewState["RPAGENUMBER"],
                   10,// General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Recent Travel Request", ds.Tables[0], alColumns, alCaptions, null, null);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void ShowExcelTravelPlan()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER","FLDFILENO" ,"FLDNAME","FLDFNAME","FLDRANKNAME", "FLDONSIGNERYESNO", "FLDDATEOFBIRTH", "FLDZONE","FLDPASSPORTNO","FLDSEAMANBOOKNO","FLDUSVISANUMBER", "FLDOTHERVISADETAILS","FLDPAYMENTMODENAME",
                                "FLDORIGIN", "FLDDESTINATION", "FLDTRAVELDATE", "FLDARRIVALDATE"};
        string[] alCaptions = { "S.No","File No", "Passenger","NOK of Employee","Rank", "On/Off-Signer", "D.O.B","Zone" ,"PP No","CDC No","US Visa", "Other Visa","Payment", "Origin", "Destination", "Departure",
                                    "Arrival"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        string trvalid = (ViewState["travelid"] == null) ? "" : ViewState["travelid"].ToString();

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewTravelRequest.SearchTravelRequest(
            int.Parse(ViewState["vessel"].ToString())
            , General.GetNullableInteger(ViewState["port"].ToString())
            , General.GetNullableDateTime(txtDateOfCrewChange.Text),
            General.GetNullableGuid(trvalid),
            sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
             gvCCT.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=TravelPlan.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Plan</h3></td>");
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
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvCCT.Rebind();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindDataTravelBreakUp()
    {
        try
        {

            DataSet ds = PhoenixCrewTravelRequest.SearchTravelBreakUp(
                General.GetNullableGuid(ViewState["TRAVELREQUESTID"] == null ? "" : ViewState["TRAVELREQUESTID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCTBreakUp.DataSource = ds;

                //MenuBreakUpAssign.Visible = true;
            }
            else
            {
                gvCTBreakUp.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    

    protected void BreakUpCopy_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper() == "COPYBREAKUPDETAILS")
            {
                string StrRequestId;
                string strVesselId;
                if (ViewState["TRAVELREQUESTID"] != null)
                {
                    StrRequestId = ViewState["TRAVELREQUESTID"].ToString();
                    strVesselId = ViewState["vessel"].ToString();
                    if (StrRequestId == "")
                    {
                        ucError.ErrorMessage = " Plan not created yet for the selected seafarer";
                        ucError.Visible = true;
                        return;
                    }
                    String scriptpopup = String.Format("javascript:parent.Openpopup('codehelp1','','../Crew/CrewTravelPlanBreakupCopy.aspx?VESSELID= " + strVesselId + " &REQUESTID= " + StrRequestId + " ');");
                }
                else
                {
                    ucError.ErrorMessage = "Select the seafarer";
                    ucError.Visible = true;
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gvCCT_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;


        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;
        ViewState["TRAVELREQUESTID"] = ((Label)_gridView.Rows[de.NewEditIndex].FindControl("lblTravelRequestId")).Text;
        string seafarer = ((LinkButton)_gridView.Rows[de.NewEditIndex].FindControl("lnkName")).Text;

        ddlSeafarerBreakup.SelectedValue = "";
        lblBreakJourneyDetails.Text = "Break Journey Details -" + seafarer;
        BindData();
        //SetPageNavigator();

        (((UserControlDate)_gridView.Rows[de.NewEditIndex].FindControl("txtArrivalDate")).FindControl("txtDate")).Focus();

    }

    protected void gvCCT_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }

    private bool IsValidCrewPlanRequest(string port, string purpose, string vesselaccount, string crewchangedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (General.GetNullableInteger(port) == null)
            ucError.ErrorMessage = "Port is required";

        if (General.GetNullableInteger(purpose) == null)
            ucError.ErrorMessage = "Reason is required";

        if (General.GetNullableInteger(vesselaccount) == null)
            ucError.ErrorMessage = "Vessel Account is required";

        if (General.GetNullableDateTime(crewchangedate) == null)
            ucError.ErrorMessage = "Crew change date is required.";

        return (!ucError.IsError);
    }


    private bool IsValidCrewChangeRequest(string onsigneryn, string departuredate, string arrivaldate, string origin, string destination,
                                            string paymentmode, string depdateoly, string arrdateoly, string port, string purpose, string vesselaccount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;

        if (string.IsNullOrEmpty(origin.Trim()))
            ucError.ErrorMessage = "Origin is required.";

        if (string.IsNullOrEmpty(destination.Trim()))
            ucError.ErrorMessage = "Destination is required.";

        if (General.GetNullableDateTime(depdateoly) == null)
            ucError.ErrorMessage = "Departure is required.";

        if (General.GetNullableDateTime(arrdateoly) == null)
            ucError.ErrorMessage = "Arrival is required.";

        else if (DateTime.TryParse(departuredate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
            ucError.ErrorMessage = "Arrival should be later than departure";

        if (General.GetNullableInteger(paymentmode) == null)
            ucError.ErrorMessage = "Payment is required.";

        if (General.GetNullableInteger(port) == null)
            ucError.ErrorMessage = "Port is required";

        if (General.GetNullableInteger(purpose) == null)
            ucError.ErrorMessage = "Reason is required";

        if (General.GetNullableInteger(vesselaccount) == null)
            ucError.ErrorMessage = "Vessel Account is required";


        return (!ucError.IsError);
    }

    protected void BreakUpAssign_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentTravelPlanFilter = null;

                BindData();
                gvCCT.Rebind();
                BindDataTravelBreakUp();
                BindRecentTravel();
                gvRecentTravel.Rebind();
            }

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelTravelPlan();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private bool IsValidTravelBreakUp(string olddeparturedate, string oldarrivaldate, string oldorigin, string olddestination, string oldpurpose
        , string departuredate, string arrivaldate, string origin, string destination, string purpose, string olddepdateoly, string oldarrdateoly, string depdateoly, string arrdateoly)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;


        if (oldorigin.Trim() == "" || origin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";
        if (olddestination.Trim() == "" || destination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";
        else if (olddestination.Trim().ToString().Equals(oldorigin.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same in Ist sector.";
        else if (destination.Trim().ToString().Equals(origin.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same in 2nd sector.";


        if (General.GetNullableDateTime(olddepdateoly) == null || General.GetNullableDateTime(depdateoly) == null)
            ucError.ErrorMessage = "Departure is required.";
        if (General.GetNullableDateTime(oldarrdateoly) == null || General.GetNullableDateTime(arrdateoly) == null)
            ucError.ErrorMessage = "Arrival is required";
        else if (DateTime.TryParse(olddeparturedate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(oldarrivaldate)) > 0)
            ucError.ErrorMessage = "Arrival should be later than departure in Ist sector ";

        else if (DateTime.TryParse(departuredate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
            ucError.ErrorMessage = "Arrival should be later than departure in 2nd sector";

        else if (DateTime.TryParse(oldarrivaldate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(departuredate)) > 0)
            ucError.ErrorMessage = "Departure of 2nd sector should be later than the Arrival of the Ist sector.";


        if (General.GetNullableInteger(purpose) == null || General.GetNullableInteger(oldpurpose) == null)
            ucError.ErrorMessage = "Purpose is required.";


        return (!ucError.IsError);
    }

    private bool IsValidTravelBreakUp(string departuredate, string arrivaldate, string origin, string destination, string depdateoly, string arrdateoly)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;


        if (origin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";
        if (destination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";

        else if (destination.Trim().ToString().Equals(origin.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same.";

        if (General.GetNullableDateTime(depdateoly) == null)
            ucError.ErrorMessage = "Departure is required.";

        if (General.GetNullableDateTime(arrdateoly) == null)
            ucError.ErrorMessage = "Arrival is required";

        else if (DateTime.TryParse(departuredate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
            ucError.ErrorMessage = "Arrival should be later than departure";


        return (!ucError.IsError);
    }

    private bool IsValidTravelBreakUp(string departuredate, string arrivaldate, string origin, string destination, string purpose)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (origin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";
        if (destination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";

        else if (destination.Trim().ToString().Equals(origin.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same.";

        if (General.GetNullableDateTime(departuredate) == null)
            ucError.ErrorMessage = "Departure is required.";

        if (General.GetNullableDateTime(arrivaldate) == null)
            ucError.ErrorMessage = "Arrival is required";

        if (General.GetNullableInteger(purpose) == null)
            ucError.ErrorMessage = "Purpose is required.";



        return (!ucError.IsError);
    }


    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        if (Filter.CurrentPickListSelection == null)
            return;

        //if (ViewState["CURRENTROW"] != null)
        //{
        //    int ncurrentrow = ViewState["CURRENTROW"] == null ? 0 : int.Parse(ViewState["CURRENTROW"].ToString());
        //    TextBox txtoriginname = (TextBox)gvCTBreakUp.Rows[ncurrentrow].FindControl("txtOriginNameBreakup");
        //    TextBox txtoriginid = (TextBox)gvCTBreakUp.Rows[ncurrentrow].FindControl("txtOriginIdBreakup");
        //    if (txtoriginid != null && txtoriginname != null)
        //    {
        //        txtoriginname.Text = Filter.CurrentPickListSelection.Get(1);
        //        txtoriginid.Text = Filter.CurrentPickListSelection.Get(2);
        //    }
        //}

    }

    public void BindVesselAccount()
    {
        ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
           General.GetNullableInteger(strVesselId) == 0 ? null : General.GetNullableInteger(strVesselId), 1);

        ddlAccountDetails.DataBind();
        if (ddlAccountDetails.Items.Count > 1)
            ddlAccountDetails.SelectedIndex = 1;

    }

    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    public void BindBreakUpSeafarer()
    {
        strVesselId = ViewState["vessel"].ToString();

        ddlSeafarerBreakup.DataSource = PhoenixCrewTravelRequest.ListBreakUpSeafarer(
            General.GetNullableInteger(strVesselId) == 0 ? null : General.GetNullableInteger(strVesselId));
        ddlSeafarerBreakup.DataBind();
    }

    protected void ddlSeafarerBreakup_DataBound(object sender, EventArgs e)
    {
        ddlSeafarerBreakup.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void ddlseafarerBreakup(object sender, EventArgs e)
    {

    }

    public void BindTravelRequest()
    {
        strVesselId = ViewState["vessel"].ToString();

        ddlTravelRequest.DataSource = PhoenixCrewTravelRequest.CrewTravelRequestList(General.GetNullableInteger(strVesselId) == 0 ? null : General.GetNullableInteger(strVesselId));
        ddlTravelRequest.DataBind();
    }

    protected void ddlTravelRequest_DataBound(object sender, EventArgs e)
    {
        ddlTravelRequest.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void gvRecentTravel_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancelTravel");
                if (cancel != null)
                {
                    cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
                    cancel.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to cancel this Request?')");
                }
                RadLabel lb = (RadLabel)e.Item.FindControl("lnkReqNo");
                RadLabel lbTravel = (RadLabel)e.Item.FindControl("lblTravelId");
                RadLabel lblTravelRequeststatusId = (RadLabel)e.Item.FindControl("lblStatusID");

                if (lblTravelRequeststatusId.Text == ViewState["REISSUED"].ToString())
                    cancel.Visible = false;
                else
                    cancel.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRecentTravel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["RPAGENUMBER"] = ViewState["RPAGENUMBER"] != null ? ViewState["RPAGENUMBER"] : gvRecentTravel.CurrentPageIndex + 1;
            BindRecentTravel();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRecentTravel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "CANCELTRAVEL")
            {
                RadLabel lblRequestId = (RadLabel)e.Item.FindControl("lblRequestId");
                if (lblRequestId.Text != "")
                {
                    PhoenixCrewTravelRequest.CancelTravelPassenger(new Guid(lblRequestId.Text));
                }
                BindRecentTravel();
                gvRecentTravel.Rebind();
                BindData();
                gvCCT.Rebind();
            }           
            else if (e.CommandName == "Page")
            {
                ViewState["RPAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvCCT_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper() == "SORT")
                return;
           
            if (e.CommandName.ToUpper() == "TRAVELREQUEST")
            {
                if (!IsValidCrewPlanRequest(ucport.SelectedValue, ucCrewChangeReason.SelectedReason, ddlAccountDetails.SelectedValue, txtDateOfCrewChange.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string lblEmployeeId = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                RadLabel lblRequestId = (RadLabel)e.Item.FindControl("lblTravelRequestId");
                string lblFamilyId = ((RadLabel)e.Item.FindControl("lblFamilyId")).Text;
                RadLabel lblOriginName = (RadLabel)e.Item.FindControl("ltOriginName");
                string requestid = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
                string strVesselId = ViewState["vessel"].ToString();

                PhoenixCrewTravelRequest.CheckTravelRequest(int.Parse(strVesselId), General.GetNullableGuid(requestid), int.Parse(lblEmployeeId), General.GetNullableInteger(lblFamilyId));

                if (ddlTravelRequest.SelectedValue != "")
                {

                    Guid? EmailTravelid = null;

                    PhoenixCrewTravelRequest.InsertTravelPlanRequest(int.Parse(strVesselId), General.GetNullableGuid(requestid), General.GetNullableInteger(ucport.SelectedValue)
                                                                     , General.GetNullableDateTime(txtDateOfCrewChange.Text)
                                                                     , General.GetNullableInteger(ucCrewChangeReason.SelectedReason)
                                                                     , General.GetNullableInteger(ddlAccountDetails.SelectedValue)
                                                                     , General.GetNullableGuid(ddlTravelRequest.SelectedValue)
                                                                     , ref EmailTravelid);
                    //  SendMail(EmailTravelid);
                    ViewState["TRAVELREQUESTID"] = null;
                    lblBreakJourneyDetails.Text = "Break Journey Details";

                    BindData();
                    BindRecentTravel();
                    BindDataTravelBreakUp();
                    BindBreakUpSeafarer();
                }
                else
                {
                    ucError.ErrorMessage = "Select any Existing Travel";
                    ucError.Visible = true;
                }
            }

            else if (e.CommandName.ToUpper() == "NEWTRAVELREQUEST")
            {
                if (!IsValidCrewPlanRequest(ucport.SelectedValue, ucCrewChangeReason.SelectedReason, ddlAccountDetails.SelectedValue, txtDateOfCrewChange.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string lblEmployeeId = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                string lblFamilyId = ((RadLabel)e.Item.FindControl("lblFamilyId")).Text;
                string requestid = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
                string strVesselId = ViewState["vessel"].ToString();

                PhoenixCrewTravelRequest.CheckTravelRequest(int.Parse(strVesselId), General.GetNullableGuid(requestid), int.Parse(lblEmployeeId), General.GetNullableInteger(lblFamilyId));

                Guid? EmailTravelid = null;

                PhoenixCrewTravelRequest.InsertTravelPlanRequest(int.Parse(ViewState["vessel"].ToString()), General.GetNullableGuid(requestid), General.GetNullableInteger(ucport.SelectedValue)
                                                                 , General.GetNullableDateTime(txtDateOfCrewChange.Text)
                                                                 , General.GetNullableInteger(ucCrewChangeReason.SelectedReason)
                                                                 , General.GetNullableInteger(ddlAccountDetails.SelectedValue)
                                                                 , null
                                                                 , ref EmailTravelid);
                ViewState["TRAVELREQUESTID"] = null;
                lblBreakJourneyDetails.Text = "Break Journey Details";

                BindData();
                BindDataTravelBreakUp();
                BindRecentTravel();
                BindTravelRequest();
                // SendMail(EmailTravelid);
                BindBreakUpSeafarer();
            }

            else if (e.CommandName.ToUpper() == "DELETE")
            {
                string requestid = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
                string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                string crewplanid = ((RadLabel)e.Item.FindControl("lblCrewPlanId")).Text;
                string onsigneryn = ((RadLabel)e.Item.FindControl("lblOnSignerYN")).Text;

                if (General.GetNullableGuid(requestid) != null)
                {
                    PhoenixCrewTravelRequest.DeleteTravelRequest(new Guid(requestid));
                    BindData();
                    gvCCT.Rebind();
                    //gvCCT_SelectedIndexChanging(gvCCT, new GridViewSelectEventArgs(gvCCT.SelectedIndex));

                    BindDataTravelBreakUp();
                    gvRecentTravel.Rebind();
                    BindBreakUpSeafarer();
                    gvCTBreakUp.Rebind();
                }
            }

            else if (e.CommandName.ToUpper() == "UPDATE")
            {
                string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                string travelrequestid = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
                string vesselid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                string onsigneryn = ((RadLabel)e.Item.FindControl("lblOnSignerYN")).Text;
                string crewchangeport = ((RadLabel)e.Item.FindControl("lblCrewChangePort")).Text;
                string departuredate = ((UserControlDate)e.Item.FindControl("txtDepartureDate")).Text;
                string arrivaldate = ((UserControlDate)e.Item.FindControl("txtArrivalDate")).Text;
                string origin = ((RadTextBox)e.Item.FindControl("txtoriginIdEdit")).Text;
                string destination = ((RadTextBox)e.Item.FindControl("txtDestinationIdedit")).Text;
                string paymentmode = ((UserControlHard)e.Item.FindControl("ucPaymentmode")).SelectedHard;
                string departureampm = ((RadComboBox)e.Item.FindControl("ddlampmdeparture")).SelectedValue;
                string arrivalampm = ((RadComboBox)e.Item.FindControl("ddlampmarrival")).SelectedValue;

                string port = ucport.SelectedValue;
                string reason = ucCrewChangeReason.SelectedReason;
                string veccelacoount = ddlAccountDetails.SelectedValue;

                string strdeparturedatetimeedit = departuredate + " " + "00:00" + (departureampm == "1" ? "AM" : "PM");
                string strarrivaldatetimeedit = arrivaldate + " " + "00:00" + (arrivalampm == "1" ? "AM" : "PM");

                if (!IsValidCrewChangeRequest(onsigneryn, strdeparturedatetimeedit, strarrivaldatetimeedit, origin, destination, paymentmode, departuredate, arrivaldate, port, reason, veccelacoount))
                {
                    ucError.Visible = true;
                    return;
                }

                string strCrewPlanId = ((RadLabel)e.Item.FindControl("lblCrewPlanId")).Text;

                if (ViewState["Offshore"] != null && ViewState["Offshore"].ToString() != "")
                {
                    int? proceedyn = null;
                    string errormsg = null;

                    PhoenixCrewTravelRequest.ValidateOffshoreTravel(strCrewPlanId.ToString(), ref proceedyn, ref errormsg);

                    if (proceedyn == 0)
                    {
                        ucError.ErrorMessage = errormsg;
                        ucError.Visible = true;
                        return;
                    }
                }
                PhoenixCrewTravelRequest.UpdateTravelRequest(
                    General.GetNullableGuid(travelrequestid),
                    General.GetNullableDateTime(departuredate), General.GetNullableDateTime(arrivaldate),
                   General.GetNullableInteger(origin),
                   General.GetNullableInteger(destination),
                   int.Parse(paymentmode),
                   int.Parse(departureampm),
                   int.Parse(arrivalampm),
                   General.GetNullableInteger(onsigneryn),
                   General.GetNullableInteger(vesselid),
                   General.GetNullableInteger(employeeid),
                   General.GetNullableInteger(ucport.SelectedValue),
                   General.GetNullableInteger(ucCrewChangeReason.SelectedReason),
                   General.GetNullableInteger(ddlAccountDetails.SelectedValue),
                   General.GetNullableGuid(strCrewPlanId)
                );

                BindBreakUpSeafarer();
                //gvCTBreakUp.Rebind();
                BindData();
                gvCCT.Rebind();
                ViewState["TRAVELREQUESTID"] = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
            }
            else if (e.CommandName.ToUpper() == "EDIT")
            {
            
                ViewState["TRAVELREQUESTID"] = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
                string seafarer = ((LinkButton)e.Item.FindControl("lnkName")).Text;

                ddlSeafarerBreakup.SelectedValue = "";
                lblBreakJourneyDetails.Text = "Break Journey Details -" + seafarer;
                BindData();
                gvCCT.Rebind();
               
            }
            else if (e.CommandName.ToUpper() == "SELECT")
            {
                ViewState["TRAVELREQUESTID"] = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
                string seafarer = ((LinkButton)e.Item.FindControl("lnkName")).Text;
                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();
                ddlSeafarerBreakup.SelectedValue = "";
                lblBreakJourneyDetails.Text = "Break Journey Details -" + seafarer;
                ViewState["ROWINDEX"] = e.Item.RowIndex;
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvCCT_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCCT.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCCT_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {

                string onsigneryn = ((RadLabel)e.Item.FindControl("lblOnSignerYN")).Text;

                UserControlDate departuredate = ((UserControlDate)e.Item.FindControl("txtDepartureDate"));
                UserControlDate arrivaldate = ((UserControlDate)e.Item.FindControl("txtArrivalDate"));

                RadLabel travelrequestid = ((RadLabel)e.Item.FindControl("lblTravelRequestId"));
                RadLabel lblTravelReqNo = ((RadLabel)e.Item.FindControl("lblTravelReqNo"));

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                LinkButton dbcpy = (LinkButton)e.Item.FindControl("cmdCopy");
                if (dbcpy != null) dbcpy.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to copy?'); return false;");

                LinkButton dbRequest = (LinkButton)e.Item.FindControl("cmdNewRequest");
                if (dbRequest != null) dbRequest.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Initiate Request?'); return false;");

                LinkButton dbEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                //ImageButton cmdAddInstruction = (ImageButton)e.Row.FindControl("cmdAddInstruction");
                RadLabel lblIsEdit = ((RadLabel)e.Item.FindControl("lblIsEdit"));
                if (dbEdit != null)
                {
                    if (lblTravelReqNo != null && lblTravelReqNo.Text != "")
                    {
                        db.Visible = false;
                        dbEdit.Visible = false;
                        if (lblIsEdit.Text == "1")
                        {
                            dbEdit.Visible = true;
                        }
                    }
                }
            }
            UserControlHard ucpayment = (UserControlHard)e.Item.FindControl("ucPaymentmode");
            DataRowView drvpayment = (DataRowView)e.Item.DataItem;
            if (ucpayment != null) ucpayment.SelectedHard = drvpayment["FLDPAYMENTMODE"].ToString();

            RadComboBox AMPMDEPARTURE = (RadComboBox)e.Item.FindControl("ddlampmdeparture");
            RadComboBox AMPMARRIVAL = (RadComboBox)e.Item.FindControl("ddlampmarrival");

            if (drvpayment != null)
            {
                if (AMPMDEPARTURE != null && (drvpayment["FLDDEPARTUREAMPMID"].ToString().Equals("1") || drvpayment["FLDDEPARTUREAMPMID"].ToString().Equals("2")))
                    AMPMDEPARTURE.SelectedValue = drvpayment["FLDDEPARTUREAMPMID"].ToString();
                if (AMPMARRIVAL != null && (drvpayment["FLDDEPARTUREAMPMID"].ToString().Equals("1") || drvpayment["FLDDEPARTUREAMPMID"].ToString().Equals("2")))
                    AMPMARRIVAL.SelectedValue = drvpayment["FLDARRIVALAMPMID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCTBreakUp_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT")
                return;

            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper() == "DELETE")
            {
                string breakupid = ((RadLabel)e.Item.FindControl("lblBreakUpId")).Text;

                if (General.GetNullableGuid(breakupid) != null)
                {
                    PhoenixCrewTravelRequest.DeleteTravelBreakUp(new Guid(breakupid));
                    BindDataTravelBreakUp();
                    gvCTBreakUp.Rebind();
                }
            }

            else if (e.CommandName.ToUpper() == "SAVE")
            {
                string breakupid = ((RadLabel)e.Item.FindControl("lblBreakUpIdEdit")).Text;
                string purposeid = ((UserControlTravelReason)e.Item.FindControl("ucPurposeOld")).SelectedReason;
                string departuredate = ((UserControlDate)e.Item.FindControl("txtDepartureDateOld")).Text;
                string arrivaldate = ((UserControlDate)e.Item.FindControl("txtArrivalDateOld")).Text;
                string origin = ((RadTextBox)e.Item.FindControl("txtOriginIdOldBreakup")).Text;
                string destination = ((RadTextBox)e.Item.FindControl("txtDestinationIdOldBreakup")).Text;
                string departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampmold")).SelectedValue;
                string arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampmold")).SelectedValue;

                string strdeparturedatetimeedit = departuredate + " " + "00:00" + (departureampm == "1" ? "AM" : "PM");
                string strarrivaldatetimeedit = arrivaldate + " " + "00:00" + (arrivalampm == "1" ? "AM" : "PM");

                if (!IsValidTravelBreakUp(strdeparturedatetimeedit, strarrivaldatetimeedit, origin, destination, departuredate, arrivaldate))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewTravelRequest.UpdateTravelBreakUp(
                    new Guid(breakupid),
                    General.GetNullableInteger(purposeid), DateTime.Parse(departuredate), DateTime.Parse(arrivaldate),
                   General.GetNullableInteger(origin), General.GetNullableInteger(destination),
                   General.GetNullableInteger(departureampm), General.GetNullableInteger(arrivalampm));

                //_gridView.EditIndex = -1;
                BindData();
                gvCCT.Rebind();
                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "BREAKUPADD")
            {
                int? employeeid = null;
                string requestid;
                string vesselid;
                int rowindex = 0;
                if (ViewState["ROWINDEX"] != null)
                {
                    rowindex = General.GetNullableInteger(ViewState["ROWINDEX"].ToString()).Value - 2;
                    employeeid = General.GetNullableInteger(((RadLabel)gvCCT.Items[rowindex].FindControl("lblEmployeeId")).Text);
                    requestid = ((RadLabel)gvCCT.Items[rowindex].FindControl("lblTravelRequestId")).Text;
                    vesselid = ViewState["vessel"].ToString();

                    string orginid = ((RadTextBox)e.Item.FindControl("txtOriginIdBreakupAdd")).Text;
                    string destinationid = ((RadTextBox)e.Item.FindControl("txtDestinationIdBreakupAdd")).Text;
                    string departuredate = ((UserControlDate)e.Item.FindControl("txtDepartureDateAdd")).Text;
                    string arrivaldate = ((UserControlDate)e.Item.FindControl("txtArrivalDateAdd")).Text;
                    string departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampmAdd")).SelectedValue;
                    string arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampmAdd")).SelectedValue;
                    string purposeid = ((UserControlTravelReason)e.Item.FindControl("ucPurposeAdd")).SelectedReason;

                    if (!IsValidTravelBreakUp(departuredate, arrivaldate, orginid, destinationid, purposeid))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixCrewTravelRequest.InsertTravelBreakUp(
                        employeeid,
                      General.GetNullableInteger(vesselid),
                       General.GetNullableInteger(orginid),
                       General.GetNullableInteger(destinationid),
                       DateTime.Parse(departuredate),
                       General.GetNullableInteger(departureampm),
                       DateTime.Parse(arrivaldate),
                       General.GetNullableInteger(arrivalampm), General.GetNullableInteger(purposeid),
                       General.GetNullableGuid(requestid));

                    BindDataTravelBreakUp();
                    gvCTBreakUp.Rebind();
                    BindBreakUpSeafarer();
                }
                else
                {
                    ucError.ErrorMessage = "Please Select the Seafarer to create a breakup";
                    ucError.Visible = true;
                }
            }
            else if (e.CommandName.ToUpper() == "EDITROW")
            {
                ViewState["EDITROW"] = "1";
                //gvCTBreakUp.EditIndex = nCurrentRow;

                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();
            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {
                string breakupid = ((RadLabel)e.Item.FindControl("lblBreakUpIdEdit")).Text;
                string purposeid = ((UserControlTravelReason)e.Item.FindControl("ucPurposeOld")).SelectedReason;
                string departuredate = ((UserControlDate)e.Item.FindControl("txtDepartureDateOld")).Text;
                string arrivaldate = ((UserControlDate)e.Item.FindControl("txtArrivalDate")).Text;
                string origin = ((RadTextBox)e.Item.FindControl("txtOriginIdOldBreakup")).Text;
                string destination = ((RadTextBox)e.Item.FindControl("txtDestinationIdOldBreakup")).Text;
                string departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampmold")).SelectedValue;
                string arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampm")).SelectedValue;

                string strdeparturedatetimeedit = departuredate + " " + "00:00" + (departureampm == "1" ? "AM" : "PM");
                string strarrivaldatetimeedit = arrivaldate + " " + "00:00" + (arrivalampm == "1" ? "AM" : "PM");

                if (!IsValidTravelBreakUp(strdeparturedatetimeedit, strarrivaldatetimeedit, origin, destination, departuredate, arrivaldate))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewTravelRequest.UpdateTravelBreakUp(
                    new Guid(breakupid),
                    General.GetNullableInteger(purposeid), DateTime.Parse(departuredate), DateTime.Parse(arrivaldate),
                   General.GetNullableInteger(origin), General.GetNullableInteger(destination),
                   General.GetNullableInteger(departureampm), General.GetNullableInteger(arrivalampm));
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCTBreakUp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCTBreakUp.CurrentPageIndex + 1;
            BindDataTravelBreakUp();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCTBreakUp_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            UserControlTravelReason ucPurposeOld = (UserControlTravelReason)e.Item.FindControl("ucPurposeOld");
            if (ucPurposeOld != null) ucPurposeOld.SelectedReason = drv["FLDPURPOSEID"].ToString();

            RadComboBox departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampmold"));
            RadComboBox arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampm"));

            if (departureampm != null && (drv["FLDDEPARTUREAMPMID"].ToString().Equals("1") || drv["FLDDEPARTUREAMPMID"].ToString().Equals("2")))
                departureampm.SelectedValue = drv["FLDDEPARTUREAMPMID"].ToString();
            if (arrivalampm != null && (drv["FLDARRIVALAMPMID"].ToString().Equals("1") || drv["FLDARRIVALAMPMID"].ToString().Equals("2")))
                arrivalampm.SelectedValue = drv["FLDARRIVALAMPMID"].ToString();

        }
    }

    protected void lnkTravelBreakUpCopy_Click(object sender, EventArgs e)
    {
        try
        {
            String fromrequestid;
            if (ViewState["TRAVELREQUESTID"] != null)
            {

                fromrequestid = ViewState["TRAVELREQUESTID"].ToString();

                if (fromrequestid != "")
                {
                    String torequestid = ddlSeafarerBreakup.SelectedValue;
                    if (torequestid == "")
                    {
                        ucError.ErrorMessage = " Plan not created yet for the selected seafarer";
                        ucError.Visible = true;
                        return;
                    }
                    if (ddlSeafarerBreakup.SelectedValue != "")
                    {
                        PhoenixCrewTravelRequest.CopyTravelPlanBreakUp(
                                new Guid(ddlSeafarerBreakup.SelectedValue)
                               , General.GetNullableInteger(ViewState["vessel"].ToString())
                               , new Guid(fromrequestid)
                             );
                        ucStatus.Text = "Employee Breakup details copied successfully.";
                    }

                }
                else
                {
                    ucError.ErrorMessage = "Cannot copy BreakUp from selected Seafarer. Breakup not created yet";
                    ucError.Visible = true;
                    return;
                }
            }
            else
            {
                ucError.ErrorMessage = "Select the Seafarer from which details will copy";
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}