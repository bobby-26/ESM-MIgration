using System;
using System.Web.UI;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Collections.Specialized;

public partial class InspectionDeficiencyCheckitemAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["INSPECTIONID"] = "";

                ViewState["CHECKITEM"] = "";
                ViewState["COMPANYID"] = string.Empty;

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                if (Request.QueryString["INSPECTIONID"] != null && Request.QueryString["INSPECTIONID"].ToString() != string.Empty)
                    ViewState["INSPECTIONID"] = Request.QueryString["INSPECTIONID"].ToString();

                BindConfig();
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    private void BindConfig()
    {
        try
        {
            DataSet ds = PhoenixInspectionRegisterCheckItems.DeficiencyCheckItemsMapp(new Guid(ViewState["INSPECTIONID"].ToString()));

            lblInspection.Text = ds.Tables[0].Rows[0]["FLDINSPECTIONNAME"].ToString();
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void gvCheckItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvCheckItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("CHECK"))
            {

                int Active = ((RadCheckBox)e.Item.FindControl("chkItem")).Checked == true ? 1 : 0;
                Guid? CheckItemId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblCheckitemId")).Text);
                int? DeficiencyCatecory = General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblDeficiencyCatecory")).Text);
                Guid? Chapterid = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblChapterid")).Text);

                PhoenixInspectionRegisterCheckItems.InspectionCheckItemMappingInsert(General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()),
                        DeficiencyCatecory, CheckItemId, Active, Chapterid);

                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void Rebind()
    {
        gvCheckItem.SelectedIndexes.Clear();
        gvCheckItem.EditIndexes.Clear();
        gvCheckItem.DataSource = null;
        gvCheckItem.Rebind();
    }

    private void BindData()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRegisterCheckItems.CheckItemMapp(new Guid(ViewState["INSPECTIONID"].ToString()), General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
            gvCheckItem.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCheckItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridGroupHeaderItem)
            {
                GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
                DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
                item.DataCell.Text = groupDataRow["Chapter"].ToString();
            }

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkItem");
                cb.Enabled = SessionUtil.CanAccess(this.ViewState, "CHECK");
                cb.Checked = DataBinder.Eval(e.Item.DataItem, "FLDACTIVE").ToString().Equals("1") ? true : false;
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