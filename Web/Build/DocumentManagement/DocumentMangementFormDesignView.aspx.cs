using SouthNests.Phoenix.StandardForm;
using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class DocumentManagement_DocumentMangementFormDesignView : PhoenixBasePage
{
    public string strjson = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            json.Attributes.Add("style", "display:none");
            dataJson.Attributes.Add("style", "display:none");

            if (!string.IsNullOrEmpty(Request.QueryString["FormName"]))
            {
                lbltitle.Text = lbltitle.Text + Request.QueryString["FormName"].ToString();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["FormId"]))
            {
                ViewState["FormId"] = Request.QueryString["FormId"].ToString();
                hdnFormId.Value = ViewState["FormId"].ToString();
                ViewState["Status"] = "1";
                bindJson();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["ReportId"]))
            {
                ViewState["ReportId"] = Request.QueryString["ReportId"].ToString();
                hdnReportId.Value = ViewState["ReportId"].ToString();

                bindReportJson();
            }

        }
    }
    private void bindJson()
    {
        DataTable dt = new DataTable();
        dt = PhoenixDocumentManagementForm.FormDesignViewEdit(new Guid(ViewState["FormId"].ToString()));
        if (dt.Rows.Count > 0)
        {
            json.InnerHtml = dt.Rows[0]["FLDJSONSCHEMA"].ToString();
            strjson = dt.Rows[0]["FLDJSONSCHEMA"].ToString();
        }
    }
    private void bindReportJson()
    {
        DataTable dt = new DataTable();
        dt = PhoenixFormBuilder.ReportEdit(new Guid(ViewState["ReportId"].ToString()));
        if (dt.Rows.Count > 0)
        {
            dataJson.InnerHtml = dt.Rows[0]["FLDJSONSCHEMA"].ToString();
            //strjson = dt.Rows[0]["FLDJSONSCHEMA"].ToString();
            ViewState["Status"] = dt.Rows[0]["FLDSTATUS"].ToString();
            ViewState["FormId"] = dt.Rows[0]["FLDFORMID"].ToString();
            hdnFormId.Value = ViewState["FormId"].ToString();
            lbltitle.Text = lbltitle.Text + dt.Rows[0]["FLDREPORTNAME"].ToString();
            bindJson();
        }
    }
}