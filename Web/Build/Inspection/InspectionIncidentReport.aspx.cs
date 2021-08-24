using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.IO;
using System.Text;
using Telerik.Web.UI;
public partial class InspectionIncidentReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Convert to Word", "WORD", ToolBarDirection.Right);
        MenuClose.AccessRights = this.ViewState;
        MenuClose.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["INCIDENTID"] != null && Request.QueryString["INCIDENTID"].ToString() != "")
                ViewState["INCIDENTID"] = Request.QueryString["INCIDENTID"].ToString();

            if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != "")
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();

            GetData();
        }
    }

    protected int GetBrowserType()
    {
        string name = "";
        System.Web.HttpBrowserCapabilities browserDetection = Request.Browser;

        name = browserDetection.Browser;
        name = name.ToUpper();        //lblBrowserVersion.Text = browserDetection.Version;         

        if (name == "IE")
            return 1;
        else
            return 2;
    }

    private void GetData()
    {
        if (ViewState["INCIDENTID"] != null && ViewState["INCIDENTID"].ToString() != "")
        {
            DataSet ds = PhoenixInspectionIncident.ThirdPartyIncidentReport(new Guid(ViewState["INCIDENTID"].ToString())
                , int.Parse(ViewState["VESSELID"].ToString()));

            String NewLine = System.Environment.NewLine;
            string starttag = @"<tr style=""text-align: left; margin: 0px; font-size: 10pt; "">"; //<td align=""left"">";
            string endtag = "</td></tr>";
            int type = GetBrowserType();

            string tdwidth = (type == 1) ? "300px" : "700px";

            tdExecSummaryHeader.InnerHtml = "1. Executive Summary";
            tdDescHeader.InnerHtml = "2. Description of Incident";
            tdTimelineHeader.InnerHtml = "3. Timeline";
            tdImmediateActionHeader.InnerHtml = "4. Immediate Action Taken onboard";
            tdPostIncidentHeader.InnerHtml = "5. Post Incident";
            tdInvestigationHeader.InnerHtml = "6. Investigation and Evidence";
            tdRCAHeader.InnerHtml = "7.	Root Cause Analysis";
            tdCAHeader.InnerHtml = "8.	Corrective Action";
            tdPAHeader.InnerHtml = "9.	Preventive Action";

            if (ds.Tables[0].Rows.Count > 0)     //to bind the text fields
            {
                DataRow dr = ds.Tables[0].Rows[0];
                tdExecsummary.InnerHtml = dr["FLDEXECUTIVESUMMARY"].ToString();
                tdDesc.InnerHtml = dr["FLDINCIDENTCOMPREHENSIVEDESCRIPTION"].ToString();
                tdImmediateAction.InnerHtml = dr["FLDIMMEDIATEACTION"].ToString();
                tdPostIncident.InnerHtml = dr["FLDPOSTINCIDENT"].ToString();
                tdInvestigation.InnerHtml = dr["FLDINVESTIGATIONANDEVIDENCE"].ToString();
            }
            string text = "";
            if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                text = @"<table id=""tblTimeline"" runat=""server"" border=""1"">";
                text = text + starttag;
                text = text + @"<th>S.No</th>";
                text = text + @"<th>Date / Time</th>";
                text = text + @"<th>Event</th>";
                text = text + @"<th>Remarks</th></tr>";

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    text = text + starttag;
                    text = text + @"<td align=""left"">" + dr["FLDSERIALNUMBER"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDEVENTDATE"].ToString() + " " + dr["FLDEVENTDATETIME"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDEVENT"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDREMARKS"].ToString();
                    text = text + endtag;
                }
                text = text + "</table>";
            }
            tdTimeline.InnerHtml = text;

            text = "";
            if (ds.Tables.Count > 0 && ds.Tables[2].Rows.Count > 0)
            {
                text = @"<table id=""tblRCA"" runat=""server"" border=""1"">";
                text = text + starttag;
                text = text + @"<th>Findings</th>";
                text = text + @"<th>Immediate Cause</th>";
                text = text + @"<th>Remarks</th>";
                text = text + @"<th>Basic Cause</th>";
                text = text + @"<th>Remarks</th>";
                text = text + @"<th>Control Action Needs</th></tr>";

                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    text = text + starttag;
                    text = text + @"<td align=""left"">" + dr["FLDFINDINGS"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDIMMEDIATECAUSE"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDICREMARKS"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDBASICSUBCAUSE"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDBCREMARKS"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDSUBCONTROLACTIONNEEDED"].ToString();
                    text = text + endtag;
                }
                text = text + "</table>";
            }
            tdRCA.InnerHtml = text;

            text = "";
            if (ds.Tables.Count > 0 && ds.Tables[3].Rows.Count > 0)
            {
                text = @"<table id=""tblCA"" runat=""server"" border=""1"">";
                text = text + starttag;
                text = text + @"<th>Corrective Action</th>";
                text = text + @"<th>Department (Assigned to)</th>";
                text = text + @"<th>Target Date</th>";
                text = text + @"<th>Completion Date</th>";
                text = text + @"<th>Verification Level</th></tr>";

                foreach (DataRow dr in ds.Tables[3].Rows)
                {
                    text = text + starttag;
                    text = text + @"<td align=""left"">" + dr["FLDCORRECTIVEACTION"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDDEPARTMENTNAME"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDTARGETDATE"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDCOMPLETIONDATE"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDVERIFICATIONLEVELNAME"].ToString();
                    text = text + endtag;
                }
                text = text + "</table>";
            }
            tdCA.InnerHtml = text;

            text = "";
            if (ds.Tables.Count > 0 && ds.Tables[4].Rows.Count > 0)
            {
                text = @"<table id=""tblPA"" runat=""server"" border=""1"">";
                text = text + starttag;
                text = text + @"<th>Control Action Needs</th>";
                text = text + @"<th>Preventive Action</th>";
                text = text + @"<th>Department (Assigned to)</th>";
                text = text + @"<th>Target Date</th>";
                text = text + @"<th>Completion Date</th>";
                text = text + @"<th>Category</th>";
                text = text + @"<th>Subcategory</th></tr>";

                foreach (DataRow dr in ds.Tables[4].Rows)
                {
                    text = text + starttag;
                    text = text + @"<td align=""left"">" + dr["FLDCONTROLACTIONNEEDS"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDPREVENTIVEACTION"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDDEPARTMENTNAME"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDTARGETDATE"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDCOMPLETIONDATE"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDCATEGORYNAME"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDSUBCATEGORYNAME"].ToString();
                    text = text + endtag;
                }
                text = text + "</table>";
            }
            tdPA.InnerHtml = text;
        }
    }

    protected void MenuClose_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("WORD"))
            {
                //string strHtml = divForm.InnerHtml;

                StringWriter sw = new StringWriter();
                HtmlTextWriter w = new HtmlTextWriter(sw);
                divForm.RenderControl(w);
                string strHtml = sw.GetStringBuilder().ToString();

                PhoenixInspectionIncident.ConvertToWord(strHtml);
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
