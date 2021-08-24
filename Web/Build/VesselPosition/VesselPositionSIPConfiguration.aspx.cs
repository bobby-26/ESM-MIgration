using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using System.Web;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections;
using Telerik.Web.UI;

public partial class VesselPositionSIPConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
          //  SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionSIPConfiguration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvProcedure')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionSIPConfiguration.aspx", "Export To Pdf", "<i class=\"fas fa-file-pdf\"></i>", "PDF");

            MenuLocation.AccessRights = this.ViewState;
            MenuLocation.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                }
                else
                {
                    NameValueCollection nvc = Filter.CurrentSIPVesselFilter;
                    if(nvc!=null)
                    {
                        UcVessel.SelectedVessel = (nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString();
                    }
                }
                UcVessel.DataBind();
                UcVessel.bind();
                gvProcedure.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Location_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("PDF"))
            {
                ConvertToPdf(PrepareHTML());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvProcedure_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper() == "NAV")
            {
                string sipconfigid = ((RadLabel)e.Item.FindControl("lblProcedureId")).Text;
                
                string Table = ((RadLabel)e.Item.FindControl("lblProcedureCode")).Text;
                string lblsipDtkey = ((RadLabel)e.Item.FindControl("lblsipDtkey")).Text;


                if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
                {
                    DataSet ds = PhoenixVesselPositionSIPConfiguration.SIPVesselCompany(int.Parse(UcVessel.SelectedVessel.ToString()));
                    string company = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Table.ToUpper() == "RAM") { Response.Redirect("../VesselPosition/vesselpositionsipriskassessmentmitigationplan.aspx?VESSELID=" + UcVessel.SelectedVessel + "&SIPCONFIGID=" + sipconfigid + "&COMPANYID=" + company); }
                        if (Table.ToUpper() == "FOS") { Response.Redirect("../VesselPosition/VesselPositionSIPFuelSystemOilModification.aspx?VESSELID=" + UcVessel.SelectedVessel + "&COMPANYID=" + company + "&SIPCONFIGID=" + sipconfigid); }
                        if (Table.ToUpper() == "TCF") { Response.Redirect("../VesselPosition/VesselPositionSIPTankNewFuelCofiguration.aspx?VESSELID=" + UcVessel.SelectedVessel + "&COMPANYID=" + company + "&SIPCONFIGID=" + sipconfigid); }
                        if (Table.ToUpper() == "TCB") { Response.Redirect("../VesselPosition/VesselPositionSIPTankClean.aspx?VESSELID=" + UcVessel.SelectedVessel + "&COMPANYID=" + company + "&SIPCONFIGID=" + sipconfigid); }
                        if (Table.ToUpper() == "PCF") { Response.Redirect("../VesselPosition/VesselPositionSIPProcurementofCompliantFuel.aspx?VESSELID=" + UcVessel.SelectedVessel + "&COMPANYID=" + company + "&SIPCONFIGID=" + sipconfigid); }
                        if (Table.ToUpper() == "FOC") { Response.Redirect("../VesselPosition/VesselPositionSIPFuelOilchangeoverplan.aspx?VESSELID=" + UcVessel.SelectedVessel + "&COMPANYID=" + company + "&SIPCONFIGID=" + sipconfigid + "&DTKEY=" + lblsipDtkey); }
                        if (Table.ToUpper() == "DOR") { Response.Redirect("../VesselPosition/VesselPositionSIPDocumentationandReporting.aspx?VESSELID=" + UcVessel.SelectedVessel + "&COMPANYID=" + company + "&SIPCONFIGID=" + sipconfigid ); }
                        if (Table.ToUpper() == "ATP") { Response.Redirect("../VesselPosition/VesselPositionSIPAnnexToPlan.aspx?VESSELID=" + UcVessel.SelectedVessel + "&COMPANYID=" + company + "&SIPCONFIGID=" + sipconfigid); }

                    }
                }
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSORTORDER", "FLDNAME" };
        string[] alCaptions = { "No.", "Name" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixVesselPositionSIPConfiguration.SIPConfigurationSearch(
             null, null, General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), sortexpression,
             sortdirection,
             (int)ViewState["PAGENUMBER"],
             gvProcedure.PageSize,
             ref iRowCount,
             ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"ShipImplementationPlan.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Ship Implementation Plan</h3></td>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void gvProcedure_Sorting(object sender, GridSortCommandEventArgs e)
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
        Rebind();
    }
    protected void UcVessel_TextChangedEvent(object sender, EventArgs e)
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("ddlVessel", UcVessel.SelectedVessel);
        Filter.CurrentSIPVesselFilter = criteria;

        Rebind();
    }
    private string PrepareHTML()
    {
        DataSet dsRisk = PhoenixVesselPositionSIPriskassessmentmitigationplan.SIPriskassessmentmitigationplanEdit(General.GetNullableInteger(UcVessel.SelectedValue.ToString()));
        string isrisk="",riskdis = "", linkedonboard = "",document="";
        if (dsRisk.Tables[0].Rows.Count > 0)
        {
            isrisk = dsRisk.Tables[0].Rows[0]["FLDIMPACTPERFORMEDYN"].ToString()=="0"?"Yes": dsRisk.Tables[0].Rows[0]["FLDIMPACTPERFORMEDYN"].ToString() == "1" ? "No" : "NA";
            riskdis = dsRisk.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString();
            linkedonboard = dsRisk.Tables[0].Rows[0]["FLDLINKEDTOONBOARDSAFTYYN"].ToString() == "0" ? "Yes" : dsRisk.Tables[0].Rows[0]["FLDLINKEDTOONBOARDSAFTYYN"].ToString() == "1" ? "No" : "NA";
            document = dsRisk.Tables[0].Rows[0]["FLDRELEVANTDOC"].ToString();
        }

        DataSet dsvessel = PhoenixVesselPositionSIPConfiguration.ListVesselInfo(General.GetNullableInteger(UcVessel.SelectedValue.ToString()));

        string vesselname = "",callsign="",imonumber="",classification="",revision="";
        if(dsvessel.Tables[0].Rows.Count>0)
        {
            vesselname = dsvessel.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            callsign = dsvessel.Tables[0].Rows[0]["FLDCALLSIGN"].ToString();
            imonumber = dsvessel.Tables[0].Rows[0]["FLDIMONUMBER"].ToString();
            classification = dsvessel.Tables[0].Rows[0]["FLDCALSS"].ToString();
            revision = dsvessel.Tables[0].Rows[0]["FLDLASTREVISIONDATE"].ToString();
        }

        StringBuilder DsHtmlcontent = new StringBuilder();
        DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
        DsHtmlcontent.Append("<font size='2' face='Helvetica'>");

        DsHtmlcontent.Append("<table ID='tblhdr'  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td align='center'><b> 2020 Sulphur Cap - Ship Implementation Plan</b> </td></tr>");
        DsHtmlcontent.Append("</table></br>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b> Revision Date :</ b> </td><td><i>" + revision + " </i></td></tr>");
        DsHtmlcontent.Append("</table></br>");


        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b> Particulars of ship:</ b> </td></tr>");
        DsHtmlcontent.Append("</table></br>");
        DsHtmlcontent.Append("<br/>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td>Name:</td><td><i>" + vesselname + "</i></td></tr>");
        DsHtmlcontent.Append("<tr><td>Call Sign:</td><td><i>" + callsign + "</i></td></tr>");
        DsHtmlcontent.Append("<tr><td>IMO:</td><td><i>" + imonumber + "</i></td></tr>");
        DsHtmlcontent.Append("<tr><td>Class:</td><td><i>" + classification + "</i></td></tr>");
        DsHtmlcontent.Append("</table></br>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>1. Risk assessment and mitigation plan</ b> </td></tr>");
        DsHtmlcontent.Append("</table></br>");
        DsHtmlcontent.Append("<br/>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td>Is risk assessment (impact of new fuels) performed? </td><td><i>" + isrisk + "</i></td></tr>");
        DsHtmlcontent.Append("</table></br>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Details</b></td></tr>");
        DsHtmlcontent.Append("<tr><td><i>" + riskdis + "</i></td></tr>");
        DsHtmlcontent.Append("</table></br>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td>Linked to onboard Safety Management System (SMS)? </td><td><i>" + linkedonboard + "</i></td></tr>");
        DsHtmlcontent.Append("</table></br>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Relevant documents: </b></td></tr>");

        string[] document1 = document.Split(';');
        if (document1.Length > 0)
        {
            foreach (string str in document1)
                DsHtmlcontent.Append("<tr><td><i>" + str + "</i></td></tr>");
        }
        else
        {
            DsHtmlcontent.Append("<tr><td><i>" + document + "</i></td></tr>");
        }
        DsHtmlcontent.Append("</table></br>");

        int iRowCount=0, iTotalPageCount=0;
        DataSet dsfo = PhoenixVesselPositionSIPFuelOilSystemModification.SIPFueloilsystemmodificationsSearch(
           General.GetNullableInteger(UcVessel.SelectedVessel),
           0,
           null,
           null,
           1,
           1000,
           ref iRowCount,
           ref iTotalPageCount);

        DataSet dsfoclass = PhoenixVesselPositionSIPFuelOilSystemModification.SIPFueloilsystemmodificationsSearch(
           General.GetNullableInteger(UcVessel.SelectedVessel),
           1,
           null,
           null,
           1,
           1000,
           ref iRowCount,
           ref iTotalPageCount);
        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>2. Fuel oil system modifications</ b> </td></tr>");
        DsHtmlcontent.Append("</table></br>");
        DsHtmlcontent.Append("<br/>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Schedule for meeting with manufacturer:</b></td></tr>");
        DsHtmlcontent.Append("</table></br>");
        for (int i = 0; i < dsfo.Tables[0].Rows.Count; i++)
        {
            DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
            DsHtmlcontent.Append("<tr><td>Manufacturer:</td><td><i>" + dsfo.Tables[0].Rows[i]["FLDMANUFACTCLASS"].ToString()+ "</i></td></tr>");
            DsHtmlcontent.Append("<tr><td>Date:</td><td><i>" + dsfo.Tables[0].Rows[i]["FLDDATE"].ToString() + "</i></td></tr>");
            DsHtmlcontent.Append("<tr><td>Details:</td><td><i>" + dsfo.Tables[0].Rows[i]["FLDDETAILS"].ToString() + "</i></td></tr>");
            DsHtmlcontent.Append("</table></br>");
        }

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Schedule for meeting with Class:</b></td></tr>");
        DsHtmlcontent.Append("</table></br>");
        for (int i = 0; i < dsfoclass.Tables[0].Rows.Count; i++)
        {
            DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
            DsHtmlcontent.Append("<tr><td>Class:</td><td><i>" + dsfoclass.Tables[0].Rows[i]["FLDMANUFACTCLASS"].ToString() + "</i></td></tr>");
            DsHtmlcontent.Append("<tr><td>Date:</td><td><i>" + dsfoclass.Tables[0].Rows[i]["FLDDATE"].ToString() + "</i></td></tr>");
            DsHtmlcontent.Append("<tr><td>Details:</td><td><i>" + dsfoclass.Tables[0].Rows[i]["FLDDETAILS"].ToString() + "</i></td></tr>");
            DsHtmlcontent.Append("</table></br>");

        }


        DataSet dsfomod = PhoenixVesselPositionSIPFuelOilSystemModification.SIPFueloilsystemmodificationsDetailEdit(General.GetNullableInteger(UcVessel.SelectedVessel));
        string strcmod = "", scrbryn = "", otherdocument = "", decicatedfodes = "", dedicatefoyn = "", fuelstoreagesustemyn = "", fueltransferyn = "", cumbstionyn = "",
                transtartdate = "", tranenddate = "", trandesc = "", cumbstartdate = "", cumbenddate = "", cumbdesc = "";
        DataRow drmod = null;
        if (dsfomod.Tables[0].Rows.Count>0)
        {
            drmod = dsfomod.Tables[0].Rows[0];
            strcmod = dsfomod.Tables[0].Rows[0]["FLDSTRUCTUREMODIFYYN"].ToString()=="0"?"Yes": dsfomod.Tables[0].Rows[0]["FLDSTRUCTUREMODIFYYN"].ToString() == "1" ? "No" : "NA";
            scrbryn = dsfomod.Tables[0].Rows[0]["FLDSCRUBBERWILLBEINSTALLED"].ToString() == "0" ? "Yes" : dsfomod.Tables[0].Rows[0]["FLDSCRUBBERWILLBEINSTALLED"].ToString() == "1" ? "No" : "NA";
            otherdocument = dsfomod.Tables[0].Rows[0]["FLDRELEVANTDOC"].ToString();
            dedicatefoyn = dsfomod.Tables[0].Rows[0]["FLDDEDICATEDFOSAMPLINGYN"].ToString() == "0" ? "Yes" : dsfomod.Tables[0].Rows[0]["FLDDEDICATEDFOSAMPLINGYN"].ToString() == "1" ? "No" : "NA";
            decicatedfodes = dsfomod.Tables[0].Rows[0]["FLDDEDICATEDFOSAMPLING"].ToString();

            fuelstoreagesustemyn = dsfomod.Tables[0].Rows[0]["FLDFUELSTORAGEYN"].ToString() == "1" ? "Yes" : "No";
            fueltransferyn = dsfomod.Tables[0].Rows[0]["FLDFUELTRANSFERYN"].ToString() == "1" ? "Yes" : "No";
            transtartdate = dsfomod.Tables[0].Rows[0]["FLDFUELTRANSFERSTARTDATE"].ToString();
            tranenddate = dsfomod.Tables[0].Rows[0]["FLDFUELTRANSFERENDDATE"].ToString();
            trandesc = dsfomod.Tables[0].Rows[0]["FLDFUELTRANSFERDETAIL"].ToString();
            cumbstionyn = dsfomod.Tables[0].Rows[0]["FLDCOMBUSTIONYN"].ToString() == "1" ? "Yes" : "No";
            cumbstartdate = dsfomod.Tables[0].Rows[0]["FLDCOMBUSTIONSTARTDATE"].ToString();
            cumbenddate = dsfomod.Tables[0].Rows[0]["FLDCOMBUSTIONENDDATE"].ToString();
            cumbdesc = dsfomod.Tables[0].Rows[0]["FLDCOMBUSTIONDETAIL"].ToString();
        }

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td>Is structural modifications (installation of fuel oil systems and/or tankage) required? </td><td><i>"+ strcmod + "</i></td></tr>");
        DsHtmlcontent.Append("</table></br>");

        if (strcmod == "Yes")
        {
            DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
            DsHtmlcontent.Append("<tr><td><b>Provide details about modifications, time schedules and yard bookings:</b></td></tr>");
            DsHtmlcontent.Append("</table>");
            if (fuelstoreagesustemyn == "Yes")
            {
                DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
                DsHtmlcontent.Append("<tr><td><b>Fuel oil storage system</b></td><td>" + fuelstoreagesustemyn + "</td></tr>");
                DsHtmlcontent.Append("</table>");

                DsHtmlcontent.Append("<table border='1'>");

                DsHtmlcontent.Append("<tr>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Start</th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>End</th>");
                DsHtmlcontent.Append("</tr>");

                DsHtmlcontent.Append("<tr>");
                DsHtmlcontent.Append("<td><i>" + drmod["FLDSTARTDATE"].ToString() + "</i></td>");
                DsHtmlcontent.Append("<td><i>" + drmod["FLDENDDATE"].ToString() + "</i></td>");
                DsHtmlcontent.Append("</tr>");

                DsHtmlcontent.Append("</table>");

                //DsHtmlcontent.Append("<table border='1'>");

                //DsHtmlcontent.Append("<tr>");
                //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Region:</th>");
                //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Country:</th>");
                //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Yard:</th>");
                //DsHtmlcontent.Append("</tr>");

                //DsHtmlcontent.Append("<tr>");
                //DsHtmlcontent.Append("<td><i>" + drmod["FLDREGIONNAME"].ToString() + "</i></td>");
                //DsHtmlcontent.Append("<td><i>" + drmod["FLDCOUNTRYNAME"].ToString() + "</i></td>");
                //DsHtmlcontent.Append("<td><i>" + drmod["FLDYARD"].ToString() + "</i></td>");
                //DsHtmlcontent.Append("</tr>");

                //DsHtmlcontent.Append("</table></br>");

                DsHtmlcontent.Append("<table><tr><td bgcolor='#f1f1f1'><b>Description:</b></td></tr></table>");
                DsHtmlcontent.Append("<table><tr><td ><i>" + drmod["FLDDISCRIPTION"].ToString() + "</i></td></tr></table>");
            }
            if (fueltransferyn == "Yes")
            {
                DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
                DsHtmlcontent.Append("<tr><td><b>Fuel transfer/filtration/delivery system</b></td><td>" + fueltransferyn + "</td></tr>");
                DsHtmlcontent.Append("</table>");

                DsHtmlcontent.Append("<table border='1'>");

                DsHtmlcontent.Append("<tr>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Start</th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>End</th>");
                DsHtmlcontent.Append("</tr>");

                DsHtmlcontent.Append("<tr>");
                DsHtmlcontent.Append("<td><i>" + transtartdate + "</i></td>");
                DsHtmlcontent.Append("<td><i>" + tranenddate + "</i></td>");
                DsHtmlcontent.Append("</tr>");

                DsHtmlcontent.Append("</table>");

                DsHtmlcontent.Append("<table><tr><td bgcolor='#f1f1f1'><b>Description:</b></td></tr></table>");
                DsHtmlcontent.Append("<table><tr><td ><i>" + trandesc + "</i></td></tr></table>");
            }
            if (cumbstionyn == "Yes")
            {
                DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
                DsHtmlcontent.Append("<tr><td><b>Fuel transfer/filtration/delivery system</b></td><td>" + cumbstionyn + "</td></tr>");
                DsHtmlcontent.Append("</table>");

                DsHtmlcontent.Append("<table border='1'>");

                DsHtmlcontent.Append("<tr>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Start</th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>End</th>");
                DsHtmlcontent.Append("</tr>");

                DsHtmlcontent.Append("<tr>");
                DsHtmlcontent.Append("<td><i>" + cumbstartdate + "</i></td>");
                DsHtmlcontent.Append("<td><i>" + cumbenddate + "</i></td>");
                DsHtmlcontent.Append("</tr>");

                DsHtmlcontent.Append("</table>");

                DsHtmlcontent.Append("<table><tr><td bgcolor='#f1f1f1'><b>Description:</b></td></tr></table>");
                DsHtmlcontent.Append("<table><tr><td ><i>" + cumbdesc + "</i></td></tr></table>");
            }
        }

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Relevant documents: </b></td></tr>");

        string[] otherdocument1 = otherdocument.Split(';');
        if (otherdocument1.Length > 0)
        {
            foreach (string str in otherdocument1)
                DsHtmlcontent.Append("<tr><td><i>" + str + "</i></td></tr>");
        }
        else
        {
            DsHtmlcontent.Append("<tr><td><i>" + otherdocument + "</i></td></tr>");
        }

        DsHtmlcontent.Append("</table></br>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td>Are scrubbers going to be installed? </td><td><i>" + scrbryn + "</i></td></tr>");
        DsHtmlcontent.Append("</table></br>");

        if (scrbryn == "Yes")
        {
            DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
            DsHtmlcontent.Append("<tr><td><b>Provide details about scrubber installation:</b></td></tr>");
            DsHtmlcontent.Append("</table></br>");

            DsHtmlcontent.Append("<table border='1'>");

            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Start</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>End</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'></th>");
            DsHtmlcontent.Append("</tr>");

            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td><i>" + drmod["FLDSCRBSTARTDATE"].ToString() + "</i></td>");
            DsHtmlcontent.Append("<td><i>" + drmod["FLDSCRBENDDATE"].ToString() + "</i></td>");
            DsHtmlcontent.Append("<td></td>");
            DsHtmlcontent.Append("</tr>");

            DsHtmlcontent.Append("</table>");

            DsHtmlcontent.Append("<table border='1'>");

            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Region:</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Country:</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Yard:</th>");
            DsHtmlcontent.Append("</tr>");

            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td><i>" + drmod["FLDSCRBRREGIONNAME"].ToString() + "</i></td>");
            DsHtmlcontent.Append("<td><i>" + drmod["FLDSCRBRCOUNTRYNAME"].ToString() + "</i></td>");
            DsHtmlcontent.Append("<td><i>" + drmod["FLDSCRBYARD"].ToString() + "</i></td>");
            DsHtmlcontent.Append("</tr>");

            DsHtmlcontent.Append("</table></br>");

            DsHtmlcontent.Append("<table><tr><td bgcolor='#f1f1f1'><b>Description of installation:</b></td></tr></table>");
            DsHtmlcontent.Append("<table><tr><td><i>" + drmod["FLDSCRBDISCRIPTION"].ToString() + "</i></td></tr></table>");

        }
        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td>Has a dedicated FO sampling point been designated? </td><td><i>" + dedicatefoyn + "</i></td></tr>");
        DsHtmlcontent.Append("</table></br>");

        if (dedicatefoyn == "Yes")
        {
            DsHtmlcontent.Append("<table><tr><td bgcolor='#f1f1f1'><b>Description:</b></td></tr></table>");
            DsHtmlcontent.Append("<table><tr><td><i>" + decicatedfodes + "</i></td></tr></table>");
        }
        //////////////////////////////////////////////////

        int? count = 0;
        DataSet dsagg = PhoenixVesselPositionSIPTankConfuguration.SIPFuelTypeAggregate(
                          General.GetNullableInteger(UcVessel.SelectedVessel)
                          , 0
                          , ref count
                          );

        DsHtmlcontent.Append("<table><tr><td bgcolor='#f1f1f1'><b>Fuel Tanks</b></td></tr></table>");
        DsHtmlcontent.Append("<table border='1'>");
        int j = 1;
        for (int i = 0; i < dsagg.Tables[0].Rows.Count; i++)
        {
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td>3."+j.ToString()+"</td>");
            j = j + 1;
            DsHtmlcontent.Append("<td>Expected number of bunker tanks designated to store "+ dsagg.Tables[0].Rows[i]["FLDSULPHURPERCENT"].ToString()+ " sulphur compliant fuel oil</td>");
            DsHtmlcontent.Append("<td>" + dsagg.Tables[0].Rows[i]["FLDNOOFTANK"].ToString() + " Tanks</td>");
            DsHtmlcontent.Append("</tr>");

            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td><i>3." + j.ToString() + "</i></td>");
            j = j + 1;
            DsHtmlcontent.Append("<td><i>Expected total storage capacity (m3) for "+ dsagg.Tables[0].Rows[i]["FLDSULPHURPERCENT"].ToString() + " sulphur compliant fuel oil</i></td>");
            DsHtmlcontent.Append("<td><i>" + dsagg.Tables[0].Rows[i]["FLDFUELCAPACITY"].ToString() + "</i></td>");
            DsHtmlcontent.Append("</tr>");
        }
        DsHtmlcontent.Append("</table></br>");

        count = 0;
        DataSet dsaggs = PhoenixVesselPositionSIPTankConfuguration.SIPFuelTypeAggregate(
                          General.GetNullableInteger(UcVessel.SelectedVessel)
                          , 1
                          , ref count
                          );
        DsHtmlcontent.Append("<table><tr><td bgcolor='#f1f1f1'><b>Service/Settling Tanks</b></td></tr></table>");
        DsHtmlcontent.Append("<table border='1'>");
        j = 1;
       
        for (int i = 0; i < dsaggs.Tables[0].Rows.Count; i++)
        {
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td>3." + j.ToString() + "</td>");
            j = j + 1;
            DsHtmlcontent.Append("<td>Expected number of bunker tanks designated to store " + dsaggs.Tables[0].Rows[i]["FLDSULPHURPERCENT"].ToString() + " sulphur compliant fuel oil</td>");
            DsHtmlcontent.Append("<td>" + dsaggs.Tables[0].Rows[i]["FLDNOOFTANK"].ToString() + " Tanks</td>");
            DsHtmlcontent.Append("</tr>");

            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td><i>3." + j.ToString() + "</i></td>");
            j = j + 1;
            DsHtmlcontent.Append("<td><i>Expected total storage capacity (m3) for " + dsaggs.Tables[0].Rows[i]["FLDSULPHURPERCENT"].ToString() + " sulphur compliant fuel oil</i></td>");
            DsHtmlcontent.Append("<td><i>" + dsaggs.Tables[0].Rows[i]["FLDFUELCAPACITY"].ToString() + "</i></td>");
            DsHtmlcontent.Append("</tr>");
        }
        DsHtmlcontent.Append("</table></br>");

        count = 0;
        DataSet dsaggl = PhoenixVesselPositionSIPTankConfuguration.SIPFuelTypeAggregate(
                          General.GetNullableInteger(UcVessel.SelectedVessel)
                          , 2
                          , ref count
                          );

        DsHtmlcontent.Append("<table><tr><td bgcolor='#f1f1f1'><b>Lubricating Oil Tanks</b></td></tr></table>");
        DsHtmlcontent.Append("<table border='1'>");

        j = 1;
        for (int i = 0; i < dsaggl.Tables[0].Rows.Count; i++)
        {
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td>3." + j.ToString() + "</td>");
            j = j + 1;
            DsHtmlcontent.Append("<td>Expected number of bunker tanks designated to store " + dsaggl.Tables[0].Rows[i]["FLDSULPHURPERCENT"].ToString() + " sulphur compliant fuel oil</td>");
            DsHtmlcontent.Append("<td>" + dsaggl.Tables[0].Rows[i]["FLDNOOFTANK"].ToString() + " Tanks</td>");
            DsHtmlcontent.Append("</tr>");

            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td><i>3." + j.ToString() + "</i></td>");
            j = j + 1;
            DsHtmlcontent.Append("<td><i>Expected total storage capacity (m3) for " + dsaggl.Tables[0].Rows[i]["FLDSULPHURPERCENT"].ToString() + " sulphur compliant fuel oil</i></td>");
            DsHtmlcontent.Append("<td><i>" + dsaggl.Tables[0].Rows[i]["FLDFUELCAPACITY"].ToString() + "</i></td>");
            DsHtmlcontent.Append("</tr>");
        }
        DsHtmlcontent.Append("</table></br>");

        DsHtmlcontent.Append("<br/>");
        DsHtmlcontent.Append("<table>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>3. Tank capacity and cleaning</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Fuel Tanks</b></td></tr>");
        DsHtmlcontent.Append("</table>");


        DataSet dsFOTC = PhoenixVesselPositionSIPTankCleaning.SIPFuelTankCleanSearch(General.GetNullableInteger(UcVessel.SelectedVessel));

        DataRow drfotc = null;
        string tcapacity = "",tmethod="",tregion="",tadditive="",tsupplyregion="",tcomment="",tbdate="",tbregion="";
        if (dsFOTC.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsFOTC.Tables[0].Rows.Count; i++)
            {
                drfotc = dsFOTC.Tables[0].Rows[i];
                if (drfotc["FLDTANKCLEANNOTREQYN"].ToString() == "Yes" && General.GetNullableGuid(drfotc["FLDSIPTANKCLEANANDBUNKERID"].ToString()) != null)
                {
                    tcapacity = General.GetNullableString(drfotc["FLDFUELCAPACITY"].ToString()) == null ? "-" : drfotc["FLDFUELCAPACITY"].ToString();
                    tmethod = General.GetNullableString(drfotc["FLDMETHODNAME"].ToString()) == null ? "-" : drfotc["FLDMETHODNAME"].ToString();
                    tregion = General.GetNullableString(drfotc["FLDDOCREGIONNAME"].ToString()) == null ? "-" : drfotc["FLDDOCREGIONNAME"].ToString();
                    tadditive = General.GetNullableString(drfotc["FLDAMOUNTADDITIVEREQ"].ToString()) == null ? "-" : drfotc["FLDAMOUNTADDITIVEREQ"].ToString();
                    tsupplyregion = General.GetNullableString(drfotc["FLDSUPPLYREGIONNAME"].ToString()) == null ? "-" : drfotc["FLDSUPPLYREGIONNAME"].ToString();
                    tcomment = General.GetNullableString(drfotc["FLDCOMMENT"].ToString()) == null ? "-" : drfotc["FLDCOMMENT"].ToString();
                    tbdate = General.GetNullableString(drfotc["FLDFIRSTBUNKERNOLATERTHANDATE"].ToString()) == null ? "-" : drfotc["FLDFIRSTBUNKERNOLATERTHANDATE"].ToString();
                    tbregion = General.GetNullableString(drfotc["FLDBYNKERREGIONNAME"].ToString()) == null ? "-" : drfotc["FLDBYNKERREGIONNAME"].ToString();

                    DsHtmlcontent.Append("<table border='1'>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>" + drfotc["FLDTANKNAME"].ToString() + "</td>");

                    DsHtmlcontent.Append("<td colspan='3'>");

                    DsHtmlcontent.Append("<table>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Capacity (m3)</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Current fuel type</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>New fuel type</th>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<td><i>" + tcapacity + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfotc["FLDOILTYPENAME"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfotc["FLDNEWOILTYPENAME"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("</table>");

                    DsHtmlcontent.Append("<table>");
                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Cleaning</td>");
                    DsHtmlcontent.Append("</tr>");
                    DsHtmlcontent.Append("</table>");


                    DsHtmlcontent.Append("<table>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Method:</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Option:</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Start:</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>End:</th>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<td><i>" + tmethod + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfotc["FLDOPTIONNAME"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfotc["FLDCLEANINGSTARTDATE"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfotc["FLDCLEANINGENDDATE"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("</table>");



                    //DsHtmlcontent.Append("<table>");

                    //DsHtmlcontent.Append("<tr>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Region:</th>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Country:</th>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Yard:</th>");
                    //DsHtmlcontent.Append("</tr>");

                    //DsHtmlcontent.Append("<tr>");
                    //DsHtmlcontent.Append("<td><i>" + tregion + "</i></td>");
                    //DsHtmlcontent.Append("<td><i>" + drfotc["FLDDOCCOUNTRY"].ToString() + "</i></td>");
                    //DsHtmlcontent.Append("<td><i>" + drfotc["FLDYARDNAME"].ToString() + "</i></td>");
                    //DsHtmlcontent.Append("</tr>");

                    //DsHtmlcontent.Append("</table>");


                    DsHtmlcontent.Append("<table>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Additive required (m3):</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Additive supplier:</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Supply, no later than: </th>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<td><i>" + tadditive + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfotc["FLDSUPPLIER"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfotc["FLDSUPPLYNOLATERTHANDATE"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("</table>");

                    //DsHtmlcontent.Append("<table>");

                    //DsHtmlcontent.Append("<tr>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Supply Region:</th>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Supply Country:</th>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Supply Port:</th>");
                    //DsHtmlcontent.Append("</tr>");

                    //DsHtmlcontent.Append("<tr>");
                    //DsHtmlcontent.Append("<td><i>" + tsupplyregion + "</i></td>");
                    //DsHtmlcontent.Append("<td><i>" + drfotc["FLDSUPPLYCOUNTRYNAME"].ToString() + "</i></td>");
                    //DsHtmlcontent.Append("<td><i>" + drfotc["FLDSUPPLYPORTNAME"].ToString() + "</i></td>");
                    //DsHtmlcontent.Append("</tr>");

                    //DsHtmlcontent.Append("</table>");

                    DsHtmlcontent.Append("<table><tr><td bgcolor='#f1f1f1'>Comment:</td></tr></table>");
                    DsHtmlcontent.Append("<table><tr><td><i>" + tcomment + "</i></td></tr></table>");

                    DsHtmlcontent.Append("<table>");
                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Bunkering, no later than</td>");
                    DsHtmlcontent.Append("</tr>");
                    DsHtmlcontent.Append("</table>");

                    DsHtmlcontent.Append("<table>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Date</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'></th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'></th>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<td><i>" + tbdate + "</i></td>");
                    DsHtmlcontent.Append("<td></td>");
                    DsHtmlcontent.Append("<td></td>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("</table>");


                    DsHtmlcontent.Append("<table>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Region:</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Country:</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Port:</th>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<td><i>" + tbregion + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfotc["FLDBUNKERCOUNTRYNAME"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfotc["FLDBUNKERPORTNAME"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("</table>");

                    DsHtmlcontent.Append("</td>");
                    DsHtmlcontent.Append("</tr>");
                    DsHtmlcontent.Append("</table></br>");

                    DsHtmlcontent.Append("</br><table>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Flushing of piping system</b></td></tr>");
                    DsHtmlcontent.Append("</table>");

                    DsHtmlcontent.Append("<table>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Flushing Date</td><td><i>" + drfotc["FLDFLUSHINGDATE"].ToString() + "</i></td><td bgcolor='#f1f1f1'>Amount of Fuel needed (m3)</td><td><i>" + drfotc["FLDAMOUNTOFFUELNEEDED"].ToString() + "</i></td></tr>");
                    DsHtmlcontent.Append("</table></br>");
                }
            }
        }

       


        DsHtmlcontent.Append("</br><table>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Settling and service tanks</b></td></tr>");
        DsHtmlcontent.Append("</table></br>");


        DataSet dsFOSS = PhoenixVesselPositionSIPTankCleaning.SIPSettlingServiceTankCleanSearch(General.GetNullableInteger(UcVessel.SelectedVessel));

        if (dsFOSS.Tables[0].Rows.Count > 0)
        {
            DataRow drfoss = null;
            for (int i = 0; i < dsFOSS.Tables[0].Rows.Count; i++)
            {
                drfoss = dsFOSS.Tables[0].Rows[i];
                if (drfoss["FLDTANKCLEANNOTREQYN"].ToString() == "Yes" && General.GetNullableGuid(drfoss["FLDSIPTANKCLEANANDBUNKERID"].ToString()) != null)
                {
                    tcapacity = General.GetNullableString(drfoss["FLDFUELCAPACITY"].ToString()) == null ? "-" : drfoss["FLDFUELCAPACITY"].ToString();
                    tmethod = General.GetNullableString(drfoss["FLDMETHODNAME"].ToString()) == null ? "-" : drfoss["FLDMETHODNAME"].ToString();
                    tregion = General.GetNullableString(drfoss["FLDDOCREGIONNAME"].ToString()) == null ? "-" : drfoss["FLDDOCREGIONNAME"].ToString();
                    tadditive = General.GetNullableString(drfoss["FLDAMOUNTADDITIVEREQ"].ToString()) == null ? "-" : drfoss["FLDAMOUNTADDITIVEREQ"].ToString();
                    tsupplyregion = General.GetNullableString(drfoss["FLDSUPPLYREGIONNAME"].ToString()) == null ? "-" : drfoss["FLDSUPPLYREGIONNAME"].ToString();
                    tcomment = General.GetNullableString(drfoss["FLDCOMMENT"].ToString()) == null ? "-" : drfoss["FLDCOMMENT"].ToString();
                    tbdate = General.GetNullableString(drfoss["FLDFIRSTBUNKERNOLATERTHANDATE"].ToString()) == null ? "-" : drfoss["FLDFIRSTBUNKERNOLATERTHANDATE"].ToString();
                    tbregion = General.GetNullableString(drfoss["FLDBYNKERREGIONNAME"].ToString()) == null ? "-" : drfoss["FLDBYNKERREGIONNAME"].ToString();


                    DsHtmlcontent.Append("<table border='1'>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>" + drfoss["FLDTANKNAME"].ToString() + "</td>");

                    DsHtmlcontent.Append("<td colspan='3'>");

                    DsHtmlcontent.Append("<table>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Capacity (m3)</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Current fuel type</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>New fuel type</th>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<td><i>" + tcapacity + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfoss["FLDOILTYPENAME"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfoss["FLDNEWOILTYPENAME"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("</table>");

                    DsHtmlcontent.Append("<table>");
                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Cleaning</td>");
                    DsHtmlcontent.Append("</tr>");
                    DsHtmlcontent.Append("</table>");


                    DsHtmlcontent.Append("<table>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Method:</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Option:</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Start:</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>End:</th>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<td><i>" + tmethod + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfoss["FLDOPTIONNAME"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfoss["FLDCLEANINGSTARTDATE"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfoss["FLDCLEANINGENDDATE"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("</table>");



                    //DsHtmlcontent.Append("<table>");

                    //DsHtmlcontent.Append("<tr>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Region:</th>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Country:</th>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Yard:</th>");
                    //DsHtmlcontent.Append("</tr>");

                    //DsHtmlcontent.Append("<tr>");
                    //DsHtmlcontent.Append("<td><i>" + tregion + "</i></td>");
                    //DsHtmlcontent.Append("<td><i>" + drfoss["FLDDOCCOUNTRY"].ToString() + "</i></td>");
                    //DsHtmlcontent.Append("<td><i>" + drfoss["FLDYARDNAME"].ToString() + "</i></td>");
                    //DsHtmlcontent.Append("</tr>");

                    //DsHtmlcontent.Append("</table>");


                    DsHtmlcontent.Append("<table>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Additive required (m3):</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Additive supplier:</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Supply, no later than: </th>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<td><i>" + tadditive + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfoss["FLDSUPPLIER"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfoss["FLDSUPPLYNOLATERTHANDATE"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("</table>");

                    //DsHtmlcontent.Append("<table>");

                    //DsHtmlcontent.Append("<tr>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Supply Region:</th>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Supply Country:</th>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Supply Port:</th>");
                    //DsHtmlcontent.Append("</tr>");

                    //DsHtmlcontent.Append("<tr>");
                    //DsHtmlcontent.Append("<td><i>" + tsupplyregion + "</i></td>");
                    //DsHtmlcontent.Append("<td><i>" + drfoss["FLDSUPPLYCOUNTRYNAME"].ToString() + "</i></td>");
                    //DsHtmlcontent.Append("<td><i>" + drfoss["FLDSUPPLYPORTNAME"].ToString() + "</i></td>");
                    //DsHtmlcontent.Append("</tr>");

                    //DsHtmlcontent.Append("</table>");

                    DsHtmlcontent.Append("<table><tr><td bgcolor='#f1f1f1'>Comment:</td></tr></table>");
                    DsHtmlcontent.Append("<table><tr><td><i>" + tcomment + "</i></td></tr></table>");

                    //DsHtmlcontent.Append("<table>");
                    //DsHtmlcontent.Append("<tr>");
                    //DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Bunkering, no later than</td>");
                    //DsHtmlcontent.Append("</tr>");
                    //DsHtmlcontent.Append("</table>");

                    //DsHtmlcontent.Append("<table>");

                    //DsHtmlcontent.Append("<tr>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Date</th>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'></th>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'></th>");
                    //DsHtmlcontent.Append("</tr>");

                    //DsHtmlcontent.Append("<tr>");
                    //DsHtmlcontent.Append("<td><i>" + tbdate + "</i></td>");
                    //DsHtmlcontent.Append("<td></td>");
                    //DsHtmlcontent.Append("<td></td>");
                    //DsHtmlcontent.Append("</tr>");

                    //DsHtmlcontent.Append("</table>");


                    //DsHtmlcontent.Append("<table>");

                    //DsHtmlcontent.Append("<tr>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Region:</th>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Country:</th>");
                    //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Port:</th>");
                    //DsHtmlcontent.Append("</tr>");

                    //DsHtmlcontent.Append("<tr>");
                    //DsHtmlcontent.Append("<td><i>" + tbregion + "</i></td>");
                    //DsHtmlcontent.Append("<td><i>" + drfoss["FLDBUNKERCOUNTRYNAME"].ToString() + "</i></td>");
                    //DsHtmlcontent.Append("<td><i>" + drfoss["FLDBUNKERPORTNAME"].ToString() + "</i></td>");
                    //DsHtmlcontent.Append("</tr>");

                    //DsHtmlcontent.Append("</table>");

                    DsHtmlcontent.Append("</td>");
                    DsHtmlcontent.Append("</tr>");
                    DsHtmlcontent.Append("</table></br>");

                    DsHtmlcontent.Append("</br><table>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Flushing of piping system</b></td></tr>");
                    DsHtmlcontent.Append("</table>");

                    DsHtmlcontent.Append("<table>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Flushing Date</td><td><i>" + drfoss["FLDFLUSHINGDATE"].ToString() + "</i></td><td bgcolor='#f1f1f1'>Amount of Fuel needed (m3)</td><td><i>" + drfoss["FLDAMOUNTOFFUELNEEDED"].ToString() + "</i></td></tr>");
                    DsHtmlcontent.Append("</table></br>");
                }
            }
        }

        ////////////////

        DsHtmlcontent.Append("</br><table>");
        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Lubricating oil tanks:</b></td></tr>");
        DsHtmlcontent.Append("</table></br>");


        DataSet dsFOlb = PhoenixVesselPositionSIPTankCleaning.SIPLubeOilTankCleanSearch(General.GetNullableInteger(UcVessel.SelectedVessel));

        if (dsFOlb.Tables[0].Rows.Count > 0)
        {
            DataRow drfolb = null;
            for (int i = 0; i < dsFOlb.Tables[0].Rows.Count; i++)
            {
                drfolb = dsFOlb.Tables[0].Rows[i];
                if (drfolb["FLDTANKCLEANNOTREQYN"].ToString() == "Yes" && General.GetNullableGuid(drfolb["FLDSIPTANKCLEANANDBUNKERID"].ToString()) !=  null)
                {
                    tcapacity = General.GetNullableString(drfolb["FLDFUELCAPACITY"].ToString()) == null ? "-" : drfolb["FLDFUELCAPACITY"].ToString();
                    tbdate = General.GetNullableString(drfolb["FLDFIRSTBUNKERNOLATERTHANDATE"].ToString()) == null ? "-" : drfolb["FLDFIRSTBUNKERNOLATERTHANDATE"].ToString();
                    tbregion = General.GetNullableString(drfolb["FLDBYNKERREGIONNAME"].ToString()) == null ? "-" : drfolb["FLDBYNKERREGIONNAME"].ToString();


                    DsHtmlcontent.Append("<table border='1'>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>" + drfolb["FLDTANKNAME"].ToString() + "</td>");

                    DsHtmlcontent.Append("<td colspan='3'>");

                    DsHtmlcontent.Append("<table>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Capacity (m3)</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Current Lubricating oil type (BN number):</ th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>New Lubricating oil type (BN number):</ th>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<td><i>" + tcapacity + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfolb["FLDOILTYPENAME"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfolb["FLDNEWOILTYPENAME"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("</table>");


                    DsHtmlcontent.Append("<table>");
                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Supply of Lubricating oil, no later than:</td>");
                    DsHtmlcontent.Append("</tr>");
                    DsHtmlcontent.Append("</table>");

                    DsHtmlcontent.Append("<table>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Date</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'></th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'></th>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<td><i>" + tbdate + "</i></td>");
                    DsHtmlcontent.Append("<td></td>");
                    DsHtmlcontent.Append("<td></td>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("</table>");


                    DsHtmlcontent.Append("<table>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Region:</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Country:</th>");
                    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Port:</th>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("<tr>");
                    DsHtmlcontent.Append("<td><i>" + tbregion + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfolb["FLDBUNKERCOUNTRYNAME"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("<td><i>" + drfolb["FLDBUNKERPORTNAME"].ToString() + "</i></td>");
                    DsHtmlcontent.Append("</tr>");

                    DsHtmlcontent.Append("</table>");

                    DsHtmlcontent.Append("</td>");
                    DsHtmlcontent.Append("</tr>");
                    DsHtmlcontent.Append("</table></br>");

                    DsHtmlcontent.Append("</br><table>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Flushing of piping system</b></td></tr>");
                    DsHtmlcontent.Append("</table>");

                    DsHtmlcontent.Append("<table>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'>Flushing Date</td><td><i>" + drfolb["FLDFLUSHINGDATE"].ToString() + "</i></td><td bgcolor='#f1f1f1'>Amount of Fuel needed (m3)</td><td><i>" + drfolb["FLDAMOUNTOFFUELNEEDED"].ToString() + "</i></td></tr>");
                    DsHtmlcontent.Append("</table></br>");
                }
            }
        }
        //////////////////
        string purchdetail = "", bunkerdate = "", ischarter = "", acceptch = "", alterstep = "", suplieryn = "", noavaildetail = "",
            noncompdate = "", region = "", country = "", port = "", disposedetail = "",purchasefile="",alternatefile="",complientfuelfile="",noncomplientfile="";


        DataSet dsproc = PhoenixVesselPositionSIPProcurementofCompliantFuel.SIPFueloilsystemmodificationsDetailEdit(General.GetNullableInteger(UcVessel.SelectedVessel));
        
        if(dsproc.Tables[0].Rows.Count>0)
        {
            purchdetail = dsproc.Tables[0].Rows[0]["FLDFUELPURCHASEDETAIL"].ToString();
            bunkerdate = dsproc.Tables[0].Rows[0]["FLDFIRSTBUNKERDATE"].ToString();
            ischarter = dsproc.Tables[0].Rows[0]["FLDISCHARTERERESPONSIBLE"].ToString() == "0" ? "Yes" : dsproc.Tables[0].Rows[0]["FLDISCHARTERERESPONSIBLE"].ToString() == "1" ? "No" : "NA";
            acceptch = dsproc.Tables[0].Rows[0]["FLDACCEPTCONTRACT"].ToString() == "0" ? "Yes" : dsproc.Tables[0].Rows[0]["FLDACCEPTCONTRACT"].ToString() == "1" ? "No" : "NA";
            alterstep = dsproc.Tables[0].Rows[0]["FLDALTERNATESTEPDETAIL"].ToString();
            suplieryn = dsproc.Tables[0].Rows[0]["FLDCONFIRMEDFROMSUPLIER"].ToString() == "0" ? "Yes" : dsproc.Tables[0].Rows[0]["FLDCONFIRMEDFROMSUPLIER"].ToString() == "1" ? "No" : "NA";
            noavaildetail = dsproc.Tables[0].Rows[0]["FLDALTERNATEFUELDETAIL"].ToString();
            noncompdate = dsproc.Tables[0].Rows[0]["FLDFUELDISPOSEDATE"].ToString();
            region = dsproc.Tables[0].Rows[0]["FLDDISPREGIONNAME"].ToString();
            country = dsproc.Tables[0].Rows[0]["FLDDISPCOUNTRY"].ToString();
            port = dsproc.Tables[0].Rows[0]["FLDDISPPORT"].ToString();
            disposedetail = dsproc.Tables[0].Rows[0]["FLDDISPOSEDETAILS"].ToString();
            purchasefile = dsproc.Tables[0].Rows[0]["FLDSIPFUELMODIFICATION"].ToString();
            alternatefile = dsproc.Tables[0].Rows[0]["FLDSIPALTERNATESTEP"].ToString();
            complientfuelfile = dsproc.Tables[0].Rows[0]["FLDSIPAARANGEMENTS"].ToString();
            noncomplientfile = dsproc.Tables[0].Rows[0]["FLDSIPCOMPLIENTFUEL"].ToString();
        }

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>4. Procurement of compliant fuel oil</ b> </td></tr>");
        DsHtmlcontent.Append("</table></br>");
        DsHtmlcontent.Append("<br/>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td>Details of fuel purchasing procedure to source compliant fuels, including procedures in cases where compliant fuel oil is not readily available</td></tr>");
        DsHtmlcontent.Append("<tr><td><i>" + purchdetail + "</i></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Documents</b></td></tr>");
        string[] purchasefile1 = purchasefile.Split(';');
        if (purchasefile1.Length > 0)
        {
            foreach (string str in purchasefile1)
                DsHtmlcontent.Append("<tr><td>" + str + "</td></tr>");
        }
        else
        {
            DsHtmlcontent.Append("<tr><td>" + purchasefile + "</td></tr>");
        }

        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td>Estimated date for first bunkering of compliant fuel oil, not later than 24:00 hrs, 31 December 2019</td><td><i>" + bunkerdate + "</i></td></tr>");
        DsHtmlcontent.Append("<tr><td>Is charterer responsible for fuel? </td><td><i>" + ischarter + "</i></td></tr>");
        DsHtmlcontent.Append("<tr><td>Accept charter party contracts that don’t have specified obligation to provide compliant fuel after 01 june 2019 or other date to be identified:? </td><td><i>" + acceptch + "</i></td></tr>");
        DsHtmlcontent.Append("</table></br>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Details of alternate steps taken:</b></td></tr>");
        DsHtmlcontent.Append("<tr><td><i>" + alterstep + "</i></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Documents</b></td></tr>");
        string[] alternatefile1 = alternatefile.Split(';');
        if (alternatefile1.Length > 0)
        {
            foreach (string str in alternatefile1)
                DsHtmlcontent.Append("<tr><td><i>" + str + "</i></td></tr>");
        }
        else
        {
            DsHtmlcontent.Append("<tr><td><i>" + alternatefile + "</i></td></tr>");
        }
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td>Is there confirmation from bunker supplier(s) to provide compliant fuel oil on the specified date? </td><td><i>" + suplieryn + "</i></td></tr>");
        DsHtmlcontent.Append("</table></br>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Details of alternate steps taken to ensure timely availability of compliant fuel oil:</b></td></tr>");
        DsHtmlcontent.Append("<tr><td><i>" + noavaildetail + "</i></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Documents</b></td></tr>");
        string[] complientfuelfile1 = complientfuelfile.Split(';');
        if (complientfuelfile1.Length > 0)
        {
            foreach (string str in complientfuelfile1)
                DsHtmlcontent.Append("<tr><td><i>" + str + "</i></td></tr>");
        }
        else
        {
            DsHtmlcontent.Append("<tr><td><i>" + complientfuelfile + "</i></td></tr>");
        }
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Details of arrangements (if any planned) to dispose of any remaining non-compliant fuel oil:</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        //DsHtmlcontent.Append("<table border='1'>");

        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Date</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'></th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'></th>");
        //DsHtmlcontent.Append("</tr>");

        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<td><i>" + noncompdate + "</i></td>");
        //DsHtmlcontent.Append("<td></td>");
        //DsHtmlcontent.Append("<td></td>");
        //DsHtmlcontent.Append("</tr>");

        //DsHtmlcontent.Append("</table>");


        //DsHtmlcontent.Append("<table border='1'>");

        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Region:</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Country:</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Port:</th>");
        //DsHtmlcontent.Append("</tr>");

        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<td><i>" + region + "</i></td>");
        //DsHtmlcontent.Append("<td><i>" + country + "</i></td>");
        //DsHtmlcontent.Append("<td><i>" + port + "</i></td>");
        //DsHtmlcontent.Append("</tr>");

        //DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Description:</b></td></tr>");
        DsHtmlcontent.Append("<tr><td><i>" + disposedetail + "</i></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Documents</b></td></tr>");
        string[] noncomplientfile1 = noncomplientfile.Split(';');
        if (noncomplientfile1.Length > 0)
        {
            foreach (string str in noncomplientfile1)
                DsHtmlcontent.Append("<tr><td><i>" + str + "</i></td></tr>");
        }
        else
        {
            DsHtmlcontent.Append("<tr><td><i>" + noncomplientfile + "</i></td></tr>");
        }
        DsHtmlcontent.Append("</table>");

        ////////////////////////////

        string isshipspcific = "", details = "", maxperreq = "", expdate = "", istainingneed = "", tainingdetails = "",fuelchangefile="",ratingfile="";


        DataSet dsfoc = PhoenixVesselPositionSIPFuelOilchangeoverplan.SIPSIPFuelOilchangeoverplanEdit(General.GetNullableInteger(UcVessel.SelectedVessel));

        if (dsfoc.Tables[0].Rows.Count > 0)
        {
            isshipspcific = dsfoc.Tables[0].Rows[0]["FLDSTSTRANSFERYN"].ToString() == "0" ? "Yes" : dsfoc.Tables[0].Rows[0]["FLDSTSTRANSFERYN"].ToString() == "1" ? "No" : "NA";
            details = dsfoc.Tables[0].Rows[0]["FLDSTSDETAIL"].ToString();
            maxperreq = dsfoc.Tables[0].Rows[0]["FLDMAXIMUMPERIODREQ"].ToString();
            expdate = dsfoc.Tables[0].Rows[0]["FLDCHANGEOVERETD"].ToString();
            istainingneed = dsfoc.Tables[0].Rows[0]["FLDTRAININGNEEDED"].ToString() == "0" ? "Yes" : dsfoc.Tables[0].Rows[0]["FLDTRAININGNEEDED"].ToString() == "1" ? "No" : "NA";
            tainingdetails = dsfoc.Tables[0].Rows[0]["FLDTRAININGDETAILS"].ToString();
            fuelchangefile = dsfoc.Tables[0].Rows[0]["FLDSIPFUELCHANGEOVER"].ToString();
            ratingfile = dsfoc.Tables[0].Rows[0]["FLDSIPTRAINING"].ToString();
        }

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>5.Fuel oil changeover plan</b></td></tr>");
        DsHtmlcontent.Append("</table></br>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td>Is a ship-specific fuel changeover plan available? </td><td><i>" + isshipspcific + "</i></td></tr>");
        DsHtmlcontent.Append("</table></br>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Details</b></td></tr>");
        DsHtmlcontent.Append("<tr><td><i>" + details + "</i></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Documents</b></td></tr>");
        string[] fuelchangefile1 = fuelchangefile.Split(';');
        if (fuelchangefile1.Length > 0)
        {
            foreach (string str in fuelchangefile1)
                DsHtmlcontent.Append("<tr><td><i>" + str + "</i></td></tr>");
        }
        else
        {
            DsHtmlcontent.Append("<tr><td><i>" + fuelchangefile + "</i></td></tr>");
        }
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td>Maximum period required to changeover fuel at all combustion units:</td><td><i>" + maxperreq + "</i></td></tr>");
        DsHtmlcontent.Append("<tr><td>Expected date of completion of changeover procedure:</td><td><i>" + expdate + "</i></td></tr>");
        DsHtmlcontent.Append("<tr><td>Is training of officers and crew needed?</td><td><i>" + istainingneed + "</i></td></tr>");
        DsHtmlcontent.Append("</table></br>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Details</b></td></tr>");
        DsHtmlcontent.Append("<tr><td><i>" + tainingdetails + "</i></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Documents</b></td></tr>");
        string[] ratingfile1 = ratingfile.Split(';');
        if (ratingfile1.Length > 0)
        {
            foreach (string str in ratingfile1)
                DsHtmlcontent.Append("<tr><td><i>" + str + "</i></td></tr>");
        }
        else
        {
            DsHtmlcontent.Append("<tr><td><i>" + ratingfile + "</i></td></tr>");
        }
        DsHtmlcontent.Append("</table>");

        ///////////////////////////////

        string implidesc = "", fononavail = "", shipboardfile = "", bookletfile = "", trimbook = "", others = "", nonvail = "", docother = "";


        DataSet dsdoc = PhoenixVesselPositionSIPDocumentationandReporting.SIPDocumentationandReportingEdit(General.GetNullableInteger(UcVessel.SelectedVessel));

        if (dsdoc.Tables[0].Rows.Count > 0)
        {
            implidesc = dsdoc.Tables[0].Rows[0]["FLDNOCOMPLIENTFUELDETAIL"].ToString();// == "0" ? "Yes" : dsdoc.Tables[0].Rows[0]["FLDNOCOMPLIENTFUELDETAIL"].ToString() == "1" ? "No" : "NA";
            fononavail = dsdoc.Tables[0].Rows[0]["FLDFONONAVAILABLITYREPORT"].ToString();
            shipboardfile = dsdoc.Tables[0].Rows[0]["FLDSHIPBOARD"].ToString();
            bookletfile = dsdoc.Tables[0].Rows[0]["FLDSTABLITY"].ToString();
            trimbook = dsdoc.Tables[0].Rows[0]["FLDBOOKLET"].ToString();
            others = dsdoc.Tables[0].Rows[0]["FLDOTHERS"].ToString();
            nonvail = dsdoc.Tables[0].Rows[0]["FLDSINONAVAILABILITYFILES"].ToString();
            docother = dsdoc.Tables[0].Rows[0]["FLDSIPOTHERDOCFILES"].ToString();
        }

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>6. Documentation and reporting</b></td></tr>");
        DsHtmlcontent.Append("</table></br>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td>Shipboard fuel oil tank management plans:</td></tr>");
        string[] shipboardfile1 = shipboardfile.Split(';');
        if (shipboardfile1.Length > 0)
        {
            foreach (string str in shipboardfile1)
                DsHtmlcontent.Append("<tr><td><i>" + str + "</i></td></tr>");
        }
        else
        {
            DsHtmlcontent.Append("<tr><td><i>" + shipboardfile + "</i></td></tr>");
        }
        DsHtmlcontent.Append("<tr><td >Stability booklets:</td></tr>");
        string[] bookletfile1 = bookletfile.Split(';');
        if (bookletfile1.Length > 0)
        {
            foreach (string str in bookletfile1)
                DsHtmlcontent.Append("<tr><td><i>" + str + "</i></td></tr>");
        }
        else
        {
            DsHtmlcontent.Append("<tr><td><i>" + bookletfile + "</i></td></tr>");
        }

        DsHtmlcontent.Append("<tr><td>Trim booklets:</td></tr>");
        string[] trimbook1 = trimbook.Split(';');
        if (trimbook1.Length > 0)
        {
            foreach (string str in trimbook1)
                DsHtmlcontent.Append("<tr><td><i>" + str + "</i></td></tr>");
        }
        else
        {
            DsHtmlcontent.Append("<tr><td><i>" + trimbook + "</i></td></tr>");
        }
        DsHtmlcontent.Append("<tr><td>Other:</td></tr>");
        string[] others1 = others.Split(';');
        if (others1.Length > 0)
        {
            foreach (string str in others1)
                DsHtmlcontent.Append("<tr><td><i>" + str + "</i></td></tr>");
        }
        else
        {
            DsHtmlcontent.Append("<tr><td><i>" + others + "</i></td></tr>");
        }
        DsHtmlcontent.Append("</table></br>");


        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>If there are any modifications planned to the fuel oil system, related documents should be consequently updated.</b></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td>If when following the implementation plan the ship has to bunker and use non-compliant fuel oil due to unavailability of compliant fuel oil safe for use on board the ship, steps to limit the impact of using non-compliant fuel oil could be:</td></tr>");
        DsHtmlcontent.Append("<tr><td><i>" + implidesc + "</i></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Documents</b></td></tr>");
        string[] nonvail1 = nonvail.Split(';');
        if (nonvail1.Length > 0)
        {
            foreach (string str in nonvail1)
                DsHtmlcontent.Append("<tr><td><i>" + str + "</i></td></tr>");
        }
        else
        {
            DsHtmlcontent.Append("<tr><td><i>" + nonvail + "</i></td></tr>");
        }

        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td>The ship should have a procedure for Fuel Oil Non-Availability Reporting (FONAR). The master and chief engineer should be conversant about when and how FONAR should be used and who it should be reported to.</td></tr>");
        DsHtmlcontent.Append("<tr><td><i>" + fononavail + "</i></td></tr>");
        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("<table  cellpadding='7' cellspacing='0'>");
        DsHtmlcontent.Append("<tr><td><b>Other Documents</b></td></tr>");
        string[] docother1 = docother.Split(';');
        if (docother1.Length > 0)
        {
            foreach (string str in docother1)
                DsHtmlcontent.Append("<tr><td><i>" + str + "</i></td></tr>");
        }
        else
        {
            DsHtmlcontent.Append("<tr><td><i>" + docother + "</i></td></tr>");
        }

        DsHtmlcontent.Append("</table>");

        DsHtmlcontent.Append("</font>");
        DsHtmlcontent.Append("</div>");

        return DsHtmlcontent.ToString();
    }
    public void ConvertToPdf(string HTMLString)
    {
        try
        {
            if (HTMLString != "")
            {
                using (var ms = new MemoryStream())
                {
                    iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(595f, 842f));
                    document.SetMargins(36f, 36f, 36f, 0f);
                    //document.SetPageSize(iTextSharp.text.PageSize.LEGAL.Rotate());
                    string filefullpath = "ShipImplementationPlan_" + UcVessel.SelectedVesselName.Replace(" ", "_") + ".pdf";
                    PdfWriter.GetInstance(document, ms);
                    document.Open();
                    string imageURL = "http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png";

                    StyleSheet styles = new StyleSheet();
                    styles.LoadStyle(".headertable td", "background-color", "Blue");
                    ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);

                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Server.MapPath("../../" + Session["images"] + "/esmlogo4_small.png"));
                    image.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                    document.Add(image);

                    for (int k = 0; k < htmlarraylist.Count; k++)
                    {
                        document.Add((iTextSharp.text.IElement)htmlarraylist[k]);

                    }
                    document.Close();
                    Response.Buffer = true;
                    var bytes = ms.ToArray();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filefullpath);
                    Response.OutputStream.Write(bytes, 0, bytes.Length);
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcedure_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvProcedure.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSORTORDER", "FLDNAME" };
        string[] alCaptions = { "No.", "Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixVesselPositionSIPConfiguration.SIPConfigurationSearch(
            null, null, General.GetNullableInteger(UcVessel.SelectedVessel)
            , sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvProcedure.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvProcedure", "Ship Implementation Plan", alCaptions, alColumns, ds);

        gvProcedure.DataSource = ds;
        gvProcedure.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void Rebind()
    {
        gvProcedure.SelectedIndexes.Clear();
        gvProcedure.EditIndexes.Clear();
        gvProcedure.DataSource = null;
        gvProcedure.Rebind();
    }
}