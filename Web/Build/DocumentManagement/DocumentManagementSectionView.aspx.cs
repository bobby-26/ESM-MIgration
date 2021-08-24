using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.DocumentManagement;

public partial class DocumentManagementSectionView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            ViewState["DOCUMENTID"] = "";

            ViewState["CURRENTINDEXSECTION"] = 1;
            ViewState["SECTIONID"] = "";

            ViewState["CURRENTINDEXFORM"] = 1;
            ViewState["FORMID"] = "";

            ViewState["CURRENTINDEXRISKASSESSMENT"] = 1;
            ViewState["PROCESSID"] = "";

            ViewState["CURRENTINDEXJOBHAZARD"] = 1;
            ViewState["JOBHAZARDID"] = "";

            if (Request.QueryString["keyword"] != null && Request.QueryString["keyword"].ToString() != "")
                ViewState["keyword"] = Request.QueryString["keyword"].ToString();
            else
                ViewState["keyword"] = "";
        }
    }
    protected void gvSectionMatches_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindSectionMatches();
    }

    private void BindSectionMatches()
    {
        int iRowCount = 0;

        DataSet ds = new DataSet();

        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        ds = PhoenixDocumentManagementDocument.KeyWordSearchResultInNewSection(
                                                                  General.GetNullableString(ViewState["keyword"].ToString())
                                                                , ref iRowCount
                                                                , companyid
                                                                );


        gvSectionMatches.DataSource = ds;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (General.GetNullableGuid(ViewState["SECTIONID"].ToString()) == null)
            {
                ViewState["SECTIONID"] = ds.Tables[0].Rows[0]["FLDSECTIONID"].ToString();
            }
        }
    }

    protected void gvSectionMatches_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            UserControlToolTip ucDocumentName = (UserControlToolTip)e.Item.FindControl("ucDocumentName");
            RadLabel lblDocumentName = (RadLabel)e.Item.FindControl("lblDocumentName");
            if (lblDocumentName != null)
            {
                //lblDocumentName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucDocumentName.ToolTip + "', 'visible');");
                //lblDocumentName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucDocumentName.ToolTip + "', 'hidden');");
                ucDocumentName.Position = ToolTipPosition.TopCenter;
                ucDocumentName.TargetControlId = lblDocumentName.ClientID;
            }

            UserControlToolTip ucSectionName = (UserControlToolTip)e.Item.FindControl("ucSectionName");
            LinkButton lnkSectionName = (LinkButton)e.Item.FindControl("lnkSectionName");
            if (lnkSectionName != null)
            {
                //lnkSectionName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucSectionName.ToolTip + "', 'visible');");
                //lnkSectionName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucSectionName.ToolTip + "', 'hidden');");
                ucSectionName.Position = ToolTipPosition.TopCenter;
                ucSectionName.TargetControlId = lnkSectionName.ClientID;
            }
            //string onclickformevent = @"javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementLinkedForms.aspx?sectionid=" + dr["FLDSECTIONID"].ToString() + @"&revisionid=" + dr["FLDREVISONID"].ToString() + @"'); return false;""";

            RadLabel lblSectionId = (RadLabel)e.Item.FindControl("lblSectionId");
            LinkButton lnkReadUnreadSec = (LinkButton)e.Item.FindControl("lnkReadUnreadSec");
            if (lnkReadUnreadSec != null)
            {
                SessionUtil.CanAccess(this.ViewState, lnkReadUnreadSec.CommandName);
                lnkReadUnreadSec.Attributes.Add("onclick", "openNewWindow('ReadUnread', '" + lnkSectionName.Text + " - Read/Unread User List', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionReadUnreadList.aspx?SECTIONID=" + lblSectionId.Text + "');return false;");
            }
            LinkButton cmdForms = (LinkButton)e.Item.FindControl("cmdForms");
            DataRowView dv = (DataRowView)e.Item.DataItem;

            if (dv["FLDFORMSPOSTERCHECKLISTIDS"].ToString() == null || dv["FLDFORMSPOSTERCHECKLISTIDS"].ToString() == string.Empty)
            {
                if (cmdForms != null)
                    cmdForms.Visible = false;
            }
            if (cmdForms != null)
            {
                SessionUtil.CanAccess(this.ViewState, cmdForms.CommandName);
                cmdForms.Attributes.Add("onclick", "openNewWindow('Forms', 'Forms and Checklist', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementLinkedForms.aspx?sectionid=" + lblSectionId.Text + "',false,500,300);return false;");
            }

            if (lnkSectionName != null)
            {
                lnkSectionName.Visible = SessionUtil.CanAccess(this.ViewState, lnkSectionName.CommandName);
                lnkSectionName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + lblSectionId.Text + "',false,900,600);return false;");
            }


        }
    }
}