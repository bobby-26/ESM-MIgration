using SouthNests.Phoenix.StandardForm;
using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
public partial class StandardFormFBformView : PhoenixBasePage
{
	public string strjson = "";
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{            
            json.Attributes.Add("style", "display:none");
            //dataJson.Attributes.Add("style", "display:none");
            ViewState["REFID"] = String.Empty;
            ViewState["workorderId"] = string.Empty;
            ViewState["DATAJSON"] = "{}";
            ViewState["REVID"] = "";
            if (Request.QueryString["workorderId"] != null)
            {
                ViewState["workorderId"] = Request.QueryString["workorderId"].ToString();
            }
            ViewState["DWPAID"] = string.Empty;
            if (Request.QueryString["dwpaid"] != null)
            {
                ViewState["DWPAID"] = Request.QueryString["dwpaid"];
            }
            ViewState["DWPWOID"] = string.Empty;
            if (Request.QueryString["dwpwoid"] != null)
            {
                ViewState["DWPWOID"] = Request.QueryString["dwpwoid"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["FORMTYPE"]))
                ViewState["DMSFORMTYPE"] = Request.QueryString["FORMTYPE"].ToString();
            else
                ViewState["DMSFORMTYPE"] = null;

            if (!string.IsNullOrEmpty(Request.QueryString["ReportId"]))
            {
                ViewState["ReportId"] = Request.QueryString["ReportId"].ToString();
                hdnReportId.Value = ViewState["ReportId"].ToString();

                bindReportJson();
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["FormId"]))
            {
                ViewState["FormId"] = Request.QueryString["FormId"].ToString();
                hdnFormId.Value = ViewState["FormId"].ToString();
                ViewState["Status"] = "1";
                bindJson();
            }
            if (Request.QueryString["FORMREVISIONID"] != null)
            {
                ViewState["REVID"] = Request.QueryString["FORMREVISIONID"];
            }

            if (!string.IsNullOrEmpty(Request.QueryString["FormName"]))
			{
				//ucTitle.Text = ucTitle.Text + Request.QueryString["FormName"].ToString();
			}

                     
                      
        }
	}
	private void bindJson()
	{
        DataTable dt = new DataTable();
       // string formtype = General.GetNullableString(ViewState["DMSFORMTYPE"].ToString());
        if (ViewState["DMSFORMTYPE"] != null && ViewState["DMSFORMTYPE"].ToString() == "DMSForm")
        {
            if (!string.IsNullOrEmpty(Request.QueryString["FORMREVISIONID"]) && !string.IsNullOrEmpty(Request.QueryString["FormId"]))
            {
                DataSet ds = PhoenixDocumentManagementForm.FormRevisionEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["FormId"].ToString()), General.GetNullableGuid(Request.QueryString["FORMREVISIONID"].ToString()));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //json.InnerHtml = ds.Tables[0].Rows[0]["FLDJSONSCHEMA"].ToString();
                    //renderJson.Value = ds.Tables[0].Rows[0]["FLDJSONSCHEMA"].ToString();
                    string json = ds.Tables[0].Rows[0]["FLDJSONSCHEMA"].ToString();
                    ViewState["jsonstring"] = json.Trim().Equals("") ? "{}" : json;
                    ViewState["REFID"] = ds.Tables[0].Rows[0]["FLDREFERENCEID"].ToString();
                }
            }
            else
            {
                DataSet ds = PhoenixDocumentManagementForm.FormEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["FormId"].ToString()));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //json.InnerHtml = ds.Tables[0].Rows[0]["FLDJSONSCHEMA"].ToString();
                    //renderJson.Value = ds.Tables[0].Rows[0]["FLDJSONSCHEMA"].ToString();
                    string json = ds.Tables[0].Rows[0]["FLDJSONSCHEMA"].ToString();
                    ViewState["jsonstring"] = json.Trim().Equals("") ? "{}" : json;
                }            
            }
        }        
	}
    private void bindReportJson()
    {
        DataTable dt = new DataTable();
        dt = PhoenixFormBuilder.ReportEdit(new Guid(ViewState["ReportId"].ToString()));
        if (dt.Rows.Count > 0)
        {
            //dataJson.InnerHtml = dt.Rows[0]["FLDJSONSCHEMA"].ToString().Replace(" ", "+");
            //strjson = dt.Rows[0]["FLDJSONSCHEMA"].ToString();
            ViewState["Status"] = dt.Rows[0]["FLDSTATUS"].ToString();
            ViewState["FormId"] = dt.Rows[0]["FLDFORMID"].ToString();
            ViewState["DATAJSON"] = dt.Rows[0]["FLDJSONSCHEMA"].ToString();
            hdnFormId.Value = ViewState["FormId"].ToString();
            //ucTitle.Text = ucTitle.Text + dt.Rows[0]["FLDREPORTNAME"].ToString();
            bindJson();
        }
    }

    //private void bindDmsReportJson()
    //{
    //    DataTable dt = new DataTable();
    //    dt = PhoenixFormBuilder.DmsReportEdit(new Guid(ViewState["ReportId"].ToString()));
    //    if (dt.Rows.Count > 0)
    //    {
    //        dataJson.InnerHtml = dt.Rows[0]["FLDJSONSCHEMA"].ToString();
    //        //strjson = dt.Rows[0]["FLDJSONSCHEMA"].ToString();
    //        ViewState["Status"] = dt.Rows[0]["FLDSTATUS"].ToString();
    //        ViewState["FormId"] = dt.Rows[0]["FLDFORMID"].ToString();
    //        hdnFormId.Value = ViewState["FormId"].ToString();
    //        ucTitle.Text = ucTitle.Text + dt.Rows[0]["FLDREPORTNAME"].ToString();
    //        bindJson();
    //    }
    //}
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hdnReportId.Value) && !string.IsNullOrEmpty(ViewState["workorderId"].ToString()) && ViewState["FormId"] != null)
        {
            PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderPtwMap(new Guid(ViewState["FormId"].ToString()), new Guid(hdnReportId.Value.ToString())
            , new Guid(ViewState["workorderId"].ToString()), new Guid(Request.QueryString["FORMREVISIONID"]));
        }
		if (!string.IsNullOrEmpty(hdnReportId.Value) && ViewState["FormId"] != null 
            && (ViewState["DWPAID"].ToString() != string.Empty || ViewState["DWPWOID"].ToString() != string.Empty))
        {            
            PhoenixPlannedMaintenanceDailyWorkPlan.InsertFormsCheckList(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , new Guid(Request.QueryString["FORMREVISIONID"]), new Guid(hdnReportId.Value.ToString())
                , General.GetNullableGuid(ViewState["DWPAID"].ToString()), General.GetNullableGuid(ViewState["DWPWOID"].ToString()));
        }
    }
}
