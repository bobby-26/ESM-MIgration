using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PayRoll_PayRollTaxRateIndia : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    Guid Id = Guid.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        if (string.IsNullOrWhiteSpace(Request.QueryString["id"]) == false)
        {
            Id = new Guid(Request.QueryString["id"]);
        }
        ShowToolBar();

        if (IsPostBack == false)
        {
            ViewState["PAGENUMBER"] = 1;
            gvTaxRate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("javascript:openNewWindow('MoreInfo','','" + Session["sitepath"] + "/PayRoll/PayRollTaxRateIndiaAdd.aspx'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        //toolbarmain.AddFontAwesomeButton("../PayRoll/PayRollTaxRateIndia.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        //toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvTaxRate')", "Print", "<i class=\"fas fa-print\"></i>", "PRINT");
        gvTabStrip.MenuList = toolbarmain.Show();
        gvTabStrip.AccessRights = this.ViewState;

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvTaxRate.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

     #region Grid Events
    protected void gvTaxRate_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
		DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;
        gvTaxRate.DataSource = PhoenixPayRollIndia.PayRollTaxRateSearch(usercode, (int)ViewState["PAGENUMBER"], gvTaxRate.PageSize, ref iRowCount, ref iTotalPageCount);
        gvTaxRate.VirtualItemCount = iRowCount;
    }

    protected void gvTaxRate_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
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
                if (string.IsNullOrWhiteSpace(drv["FLDTAXRATEID"].ToString()) == false)
                {
                    string script = string.Format("javascript:openNewWindow('MoreInfo', '', '{0}/PayRoll/PayRollTaxRateIndiaAdd.aspx?id={1}', 'true'); return false", Session["sitepath"], drv["FLDTAXRATEID"].ToString());
                    editBtn.Attributes.Add("onclick", script);
                }
            }
        }
    }

    protected void gvTaxRate_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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
                PhoenixPayRollIndia.PayRollTaxRateDelete(usercode, Id);
                gvTaxRate.Rebind();
            }
            if (e.CommandName == "VIEW")
            {
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    #endregion


    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
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