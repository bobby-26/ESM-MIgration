using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using System.Collections;

public partial class InspectionCheckItemChapterMappingAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["CHECKITEMID"] = "";
            ViewState["COMPANYID"] = "";
            ViewState["Chapterlist"] = string.Empty;

            if (Request.QueryString["CHECKITEMID"] != null && Request.QueryString["CHECKITEMID"].ToString() != string.Empty)
                ViewState["CHECKITEMID"] = Request.QueryString["CHECKITEMID"].ToString();

            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }
            BindInspection();
        }

    }
    private void BindData()
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixInspectionRegisterCheckItems.CheckitemChapterList(General.GetNullableGuid(ViewState["CHECKITEMID"].ToString())
                , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                , General.GetNullableGuid(ddlAudit.SelectedValue)
                , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

            gvChapter.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvChapter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvChapter_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("CHECK"))
            {
                RadLabel lblinspectionid = (RadLabel)e.Item.FindControl("lblinspectionid");
                RadLabel lblChapterid = (RadLabel)e.Item.FindControl("lblChapterid");

                PhoenixInspectionRegisterCheckItems.ChapterMappingInsert(General.GetNullableGuid(ViewState["CHECKITEMID"].ToString())
                    , General.GetNullableGuid(lblinspectionid.Text)
                    , General.GetNullableGuid(lblChapterid.Text)
                    , General.GetNullableString(null));

                gvChapter.Rebind();

                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo','','');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void BindInspection()
    {
        ddlAudit.DataSource = PhoenixInspection.ListAllInspectionByCompany(General.GetNullableInteger(null)
                                        , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                        , null
                                        , 1
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlAudit.DataTextField = "FLDSHORTCODE";
        ddlAudit.DataValueField = "FLDINSPECTIONID";
        ddlAudit.DataBind();
        ddlAudit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void gvChapter_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridGroupHeaderItem)
            {
                GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
                DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
                item.DataCell.Text = groupDataRow["Inspection"].ToString();
            }

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkItem");
                cb.Enabled = SessionUtil.CanAccess(this.ViewState, "CHECK");
                cb.Checked = DataBinder.Eval(e.Item.DataItem, "FLDCHECK").ToString().Equals("1") ? true : false;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void chkItem_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridDataItem gvrow in gvChapter.Items)
        {
            if (((RadCheckBox)(gvrow.FindControl("chkItem"))).Checked == true)
            {
                ViewState["Chapterlist"] += gvrow.GetDataKeyValue("FLDCHAPTERID").ToString() + ',';
            }
            else
                ViewState["Chapterlist"] = ViewState["Chapterlist"].ToString();
           
        }
        
    }
    protected void ucAuditCategory_TextChangedEvent(object sender, EventArgs e)
    {
        ddlAudit.ClearSelection();
        BindInspection();
        gvChapter.Rebind();
    }

    protected void ddlAudit_TextChanged(object sender, EventArgs e)
    {
        gvChapter.Rebind();
    }
}
