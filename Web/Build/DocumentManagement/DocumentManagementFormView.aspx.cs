using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Integration;
using SouthNests.Phoenix.Common;

public partial class DocumentManagementFormView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
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

    private void BindFormMatches()
    {
        try
        {
            int iRowCount = 0;

            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            DataSet ds = PhoenixDocumentManagementDocument.KeyWordSearchResultInNewForm(
                                                                  General.GetNullableString(ViewState["keyword"].ToString())
                                                                , ref iRowCount
                                                                , companyid
                                                                );


            gvFormMatch.DataSource = ds;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableGuid(ViewState["FORMID"].ToString()) == null)
                {
                    ViewState["FORMID"] = ds.Tables[0].Rows[0]["FLDFORMID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormMatch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindFormMatches();
    }

    protected void gvFormMatch_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            UserControlToolTip ucFormName = (UserControlToolTip)e.Item.FindControl("ucFormName");
            LinkButton lnkFormName = (LinkButton)e.Item.FindControl("lnkFormName");
            RadLabel lblFormId = (RadLabel)e.Item.FindControl("lblFormId");

            RadLabel lbltype = (RadLabel)e.Item.FindControl("lbltype");
            LinkButton lblName = (LinkButton)e.Item.FindControl("lnkFormName");
            if (lnkFormName != null)
            {
                //lnkFormName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucFormName.ToolTip + "', 'visible');");
                //lnkFormName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucFormName.ToolTip + "', 'hidden');");
                ucFormName.Position = ToolTipPosition.TopCenter;
                ucFormName.TargetControlId = lnkFormName.ClientID;

                if (lblFormId != null)
                {

                    if ((lbltype.Text == "5") && (lblName != null))
                    {
                        lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + lblFormId.Text + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "',false,700,700);return false;");
                    }
                    if ((lbltype.Text == "6") && (lblName != null))
                    {
                       lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Common/download.aspx?formid=" + lblFormId.Text + "',false,700,700);return false;");
                    }
                }
            }

            LinkButton cmdDoc = (LinkButton)e.Item.FindControl("cmdDocuments");
            if (cmdDoc != null)
            {
                cmdDoc.Attributes.Add("onclick", "javascript:return openNewWindow('Document', '', 'DocumentManagement/DocumentManagementDocumentLinked.aspx?FORMID=" + lblFormId.Text + "',false,550,300); return false;");
            }

        }
    }

}