using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.IO;
using System.Text;

public partial class InspectionDeficiencyReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Convert to Word", "WORD");
            MenuClose.AccessRights = this.ViewState;
            MenuClose.MenuList = toolbar.Show();

            if (Request.QueryString["DEFICIENCYID"] != null && Request.QueryString["DEFICIENCYID"].ToString() != "")
                ViewState["DEFICIENCYID"] = Request.QueryString["DEFICIENCYID"].ToString();

            if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != "")
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            if (ViewState["VESSELID"].ToString() == "0")
                lblVessel.Text = "Company";

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
        if (ViewState["DEFICIENCYID"] != null && ViewState["DEFICIENCYID"].ToString() != "")
        {
            DataSet ds = PhoenixInspectionDeficiency.DeficiencyReport(new Guid(ViewState["DEFICIENCYID"].ToString())
                , int.Parse(ViewState["VESSELID"].ToString()));

            String NewLine = System.Environment.NewLine;
            string starttag = @"<tr style=""text-align: left; margin: 0px; font-size: 10pt; "">"; //<td align=""left"">";
            string endtag = "</td></tr>";
            int type = GetBrowserType();

            string tdwidth = (type == 1) ? "300px" : "700px";

            if (ds.Tables[0].Rows.Count > 0)     //to bind the text fields
            {
                DataRow dr = ds.Tables[0].Rows[0];
                lblCompanyName.Text = dr["FLDCOMPANYNAME"].ToString();
                tdRefNumber.InnerHtml = dr["FLDREFERENCENUMBER"].ToString();
                tdIssuedDate.InnerHtml = dr["FLDISSUEDDATE"].ToString();
                tdVessel.InnerHtml = dr["FLDVESSELNAME"].ToString();
                tdStatus.InnerHtml = dr["FLDSTATUS"].ToString();
                tdDeficiencyType.InnerHtml = dr["FLDDEFICIENCYTYPE"].ToString();
                tdSource.InnerHtml = dr["FLDSOURCE"].ToString();
                tdDeficiencyCategory.InnerHtml = dr["FLDDEFICIENCYCATEGORY"].ToString();
                tdItem.InnerHtml = dr["FLDITEM"].ToString();
                tdChapterNo.InnerHtml = dr["FLDCHAPTERNUMBER"].ToString();
                tdChecklistReference.InnerHtml = dr["FLDCHECKLISTREFERENCENUMBER"].ToString();
                tdKey.InnerHtml = dr["FLDKEYNAME"].ToString();
                tdDeficiencyDesc.InnerHtml = dr["FLDDEFICIENCYDESCRIPTION"].ToString();
                tdRCAReq.InnerHtml = dr["FLDISRCAREQUIRED"].ToString();
                tdRCATargetDate.InnerHtml = dr["FLDRCATARGETDATE"].ToString();
                tdRCACompleted.InnerHtml = dr["FLDISRCACOMPLETED"].ToString();
                tdDate.InnerHtml = dr["FLDRCACOMPLETIONDATE"].ToString();
                tdCloseOutDate.InnerHtml = dr["FLDCLOSEDATE"].ToString();
                tdCloseOutBy.InnerHtml = dr["FLDCLOSEOUTBYNAME"].ToString() + " - " + dr["FLDCLOSEOUTBYDESIGNATION"].ToString();
                tdCloseOutRemarks.InnerHtml = dr["FLDCLOSEOUTREMARKS"].ToString();
            }
            string text = ""; 
            if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                text = @"<table id=""tblRCA"" runat=""server"" border=""1"">";
                text = text + starttag;
                //text = text + @"<th>Findings</th>";
                text = text + @"<th>CAR</th>";
                text = text + @"<th>Immediate Cause</th>";
                text = text + @"<th>Remarks</th>";
                text = text + @"<th>Basic Cause</th>";
                text = text + @"<th>Remarks</th>";
                text = text + @"<th>Control Action Needs</th></tr>";

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    text = text + starttag;
                    //text = text + @"<td align=""left"">" + dr["FLDFINDINGS"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDDEFICIENCYDETAILS"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDIMMEDIATECAUSE"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDICREMARKS"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDBASICSUBCAUSE"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDBCREMARKS"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDSUBCONTROLACTIONNEEDED"].ToString();
                    text = text + endtag;
                }
                text = text + "</table>";
            }
            if (ds.Tables[1].Rows.Count == 0)
            {
                trRCA.Visible = false;
                trRCANIL.Visible = true;
            }
            tdRCA.InnerHtml = text;

            text = "";
            if (ds.Tables.Count > 0 && ds.Tables[2].Rows.Count > 0)
            {
                text = @"<table id=""tblCA"" runat=""server"" border=""1"">";
                text = text + starttag;
                text = text + @"<th>Chklist Ref No</th>";
                text = text + @"<th>Deficiency Details</th>";
                text = text + @"<th>Corrective Action</th>";
                text = text + @"<th>Department (Assigned to)</th>";
                text = text + @"<th>Target Date</th>";
                text = text + @"<th>Completion Date</th>";
                text = text + @"<th>Verification Level</th></tr>";

                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    text = text + starttag;
                    text = text + @"<td align=""left"">" + dr["CHECKLISTREFNUMBER"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDDEFICIENCYDETAILS"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDCORRECTIVEACTION"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDDEPARTMENTNAME"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDTARGETDATE"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDCOMPLETIONDATE"].ToString() + "</td>";
                    text = text + @"<td align=""left"">" + dr["FLDVERIFICATIONLEVELNAME"].ToString();
                    text = text + endtag;
                }
                text = text + "</table>";
            }
            if (ds.Tables[2].Rows.Count == 0)
            {
                trCA.Visible = false;
                trCANIL.Visible = true;
            }
            tdCA.InnerHtml = text;

            text = "";
            if (ds.Tables.Count > 0 && ds.Tables[3].Rows.Count > 0)
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

                foreach (DataRow dr in ds.Tables[3].Rows)
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
            if (ds.Tables[3].Rows.Count == 0)
            {
                trPA.Visible = false;
                trPANIL.Visible = true;
            }
            tdPA.InnerHtml = text;
        }
    }

    protected void MenuClose_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("WORD"))
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
