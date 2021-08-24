using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PayRoll_PayRollTaxIndia : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        ShowToolBar();

        if (IsPostBack == false)
        {
            ViewState["PAGENUMBER"] = 1;
            gvTaxIndia.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("javascript:openNewWindow('MoreInfo','','" + Session["sitepath"] + "/HR/PayRollTaxIndiaAdd.aspx'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        gvTabStrip.MenuList = toolbarmain.Show();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvTaxIndia.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

     #region Grid Events
    protected void gvTaxIndia_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
		DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;
        gvTaxIndia.DataSource = PhoenixPayRollIndia.TaxIndiaSearch(usercode, (int)ViewState["PAGENUMBER"], gvTaxIndia.PageSize, ref iRowCount, ref iTotalPageCount);
        gvTaxIndia.VirtualItemCount = iRowCount;
    }

    protected void gvTaxIndia_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
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
                if (string.IsNullOrWhiteSpace(drv["FLDPAYROLLTAXID"].ToString()) == false)
                {
                    string script = string.Format("javascript:openNewWindow('MoreInfo', '', '{0}/HR/PayRollTaxIndiaAdd.aspx?id={1}'); return false", Session["sitepath"], drv["FLDPAYROLLTAXID"].ToString());
                    editBtn.Attributes.Add("onclick", script);
                }
            }
        }
    }

    protected void gvTaxIndia_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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
                PhoenixPayRollIndia.TaxIndiaDelete(usercode, Id);
                gvTaxIndia.Rebind();
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