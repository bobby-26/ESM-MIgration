using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.PayRoll;

public partial class PayRollEmployeeTax : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    int Id = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        if (string.IsNullOrWhiteSpace(Request.QueryString["id"]) == false)
        {
            Id = int.Parse(Request.QueryString["id"]);
        }
        ShowToolBarTax();
        ShowToolBarTDS();

        if (IsPostBack == false)
        {
            ViewState["PAGENUMBER"] = 1;
          
            gvadvancetax.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvTDS.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
    }

    public void ShowToolBarTax()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/HR/PayRollAdvanceTaxindiaAdd.aspx?employeeid=" + Id + "'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        gvTabStripAdvanceTax.MenuList = toolbarmain.Show();
    }
    public void ShowToolBarTDS()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/HR/PayRollTDSindiaAdd.aspx?employeeid=" + Id + "'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        gvTabStripTDS.MenuList = toolbarmain.Show();
    }
 
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvadvancetax.Rebind();
            gvTDS.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #region Grid Events AdvanceTax
    protected void gvadvancetax_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvadvancetax.CurrentPageIndex + 1;

        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;
        ds = PhoenixPayRollIndia.PayRollAdvanceTaxindiaSearch(usercode,Id, General.GetNullableString(txtcode.Text), (int)ViewState["PAGENUMBER"], gvadvancetax.PageSize, ref iRowCount, ref iTotalPageCount);
        gvadvancetax.DataSource = ds;
        gvadvancetax.VirtualItemCount = iRowCount;
    }

    protected void gvadvancetax_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.IsInEditMode)
        {

        }
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
        if (e.Item is GridDataItem)
        {
            LinkButton attachmentBtn = (LinkButton)e.Item.FindControl("cmdAttachment");

        }
        if (e.Item is GridDataItem && e.Item.IsInEditMode == false)
        {
            LinkButton editBtn = (LinkButton)e.Item.FindControl("cmdEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (editBtn != null)
            {
                if (string.IsNullOrWhiteSpace(drv["FLDPAYROLLADVANCETAXID"].ToString()) == false)
                {
                    string script = string.Format("javascript:openNewWindow('MoreInfo', '', '{0}/HR/PayRollAdvanceTaxindiaAdd.aspx?id={1}'); return false", Session["sitepath"], drv["FLDPAYROLLADVANCETAXID"].ToString());
                    editBtn.Attributes.Add("onclick", script);
                }
            }
        }
    }

    protected void gvadvancetax_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                LinkButton db1 = (LinkButton)e.Item.FindControl("cmdSave");
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                LinkButton db1 = (LinkButton)e.Item.FindControl("cmdDelete");
                Guid Id = new Guid(e.CommandArgument.ToString());
                PhoenixPayRollIndia.PayRollAdvanceTaxindiaDelete(usercode, Id);
                gvadvancetax.Rebind();
            }
            if (e.CommandName == "VIEW")
            {

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
    #endregion

    #region Grid Events TDS
    protected void gvTDS_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTDS.CurrentPageIndex + 1;

        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;
        ds = PhoenixPayRollIndia.PayRollTDSindiaSearch(usercode,Id,(int)ViewState["PAGENUMBER"], gvTDS.PageSize, ref iRowCount, ref iTotalPageCount);
        gvTDS.DataSource = ds;
        gvTDS.VirtualItemCount = iRowCount;
    }

    protected void gvTDS_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.IsInEditMode)
        {

        }
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
        if (e.Item is GridDataItem)
        {
            LinkButton attachmentBtn = (LinkButton)e.Item.FindControl("cmdAttachment");

        }
        if (e.Item is GridDataItem && e.Item.IsInEditMode == false)
        {
            LinkButton editBtn = (LinkButton)e.Item.FindControl("cmdEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (editBtn != null)
            {
                if (string.IsNullOrWhiteSpace(drv["FLDPAYROLLTDSID"].ToString()) == false)
                {
                    string script = string.Format("javascript:openNewWindow('MoreInfo', '', '{0}/HR/PayRollTDSindiaAdd.aspx?id={1}'); return false", Session["sitepath"], drv["FLDPAYROLLTDSID"].ToString());
                    editBtn.Attributes.Add("onclick", script);
                }
            }
        }
    }

    protected void gvTDS_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                LinkButton db1 = (LinkButton)e.Item.FindControl("cmdSave");
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                LinkButton db1 = (LinkButton)e.Item.FindControl("cmdDelete");
                Guid Id = new Guid(e.CommandArgument.ToString());
                PhoenixPayRollIndia.PayRollTDSindiaDelete(usercode, Id);
                gvTDS.Rebind();
            }
            if (e.CommandName == "VIEW")
            {

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
    #endregion
    protected void gvTabStripAdvanceTax_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTabStripTDS_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}