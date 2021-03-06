using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using System.Data;
using Telerik.Web.UI;

public partial class InspectionAuditOfficeScheduleFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (General.IsTelerik() && Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper().Equals("MENU"))
        {
            Response.Redirect("../Inspection/InspectionAuditOfficeRecordList.aspx", false);
        }

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        MenuScheduleFilter.AccessRights = this.ViewState;
        MenuScheduleFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            txtDoneDateTo.Text = DateTime.Now.ToShortDateString();

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
                
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            {
                ucStatus.Enabled = true;
            }

            Bind_UserControls(sender, new EventArgs());
            BindInternalInspector();
        }
    }

    protected void MenuScheduleFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        //string Script = "";
        //Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        //Script += "fnReloadList();";
        //Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("ucAuditType", ucAuditType.SelectedHard);
            criteria.Add("ucAuditCategory", ucAuditCategory.SelectedHard);
            criteria.Add("ucAudit", ddlAudit.SelectedValue);
            criteria.Add("ucStatus", ucStatus.SelectedHard);
            criteria.Add("txtDoneFrom", txtDoneDateFrom.Text);
            criteria.Add("txtDoneTo", txtDoneDateTo.Text);
            criteria.Add("txtRefNo", txtRefNo.Text);
            criteria.Add("txtExternalInspector", txtExternalInspector.Text);
            criteria.Add("txtExternalOrganization", txtExternalOrganization.Text);
            criteria.Add("ddlInspectorName", ddlInspectorName.SelectedValue);
            criteria.Add("ddlDefType", ddlNCType.SelectedValue == "0" ? null : ddlNCType.SelectedValue);
            criteria.Add("ucChapter", ucChapter.SelectedChapter);
            criteria.Add("txtKey", txtKey.Text);

            Filter.CurrentAuditOfficeScheduleFilterCriteria = criteria;
            //Response.Redirect("../Inspection/InspectionAuditOfficeRecordList.aspx?callfrom=record&menu=y", false);
        }
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void Bind_UserControls(object sender, EventArgs e)
    {
        string type = PhoenixCommonRegisters.GetHardCode(1, 148, "AUD");
        ddlAudit.DataSource = PhoenixInspection.ListAllInspectionByCompany(General.GetNullableInteger(type)
                                        , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                        , null
                                        , 0
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlAudit.DataTextField = "FLDSHORTCODE";
        ddlAudit.DataValueField = "FLDINSPECTIONID";
        ddlAudit.DataBind();
        ddlAudit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindInternalInspector()
    {
        DataSet ds = PhoenixInspectionAuditSchedule.AuditInternalInspectorSearch(null);
        ddlInspectorName.DataSource = ds.Tables[0];
        ddlInspectorName.DataTextField = "FLDDESIGNATIONNAME";
        ddlInspectorName.DataValueField = "FLDUSERCODE";
        ddlInspectorName.DataBind();
        ddlInspectorName.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ucInspection_Changed(object sender, EventArgs e)
    {
        ucChapter.InspectionId = ddlAudit.SelectedValue;
        ucChapter.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(General.GetNullableInteger(ucAuditType.SelectedHard),
            General.GetNullableInteger(ucAuditCategory.SelectedHard),
            General.GetNullableGuid(ddlAudit.SelectedValue));
    }
}
