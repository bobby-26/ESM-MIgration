using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;

public partial class Accounts_AccountsInvoiceVesselList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            maintitle.AccessRights = this.ViewState;
            maintitle.Title = "Invoice Vessel List";
            maintitle.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["INVOICECODE"] = null;
                ViewState["PAGENUMBER"] = 1;
                if (Request.QueryString["INVOICECODE"] != null)
                {
                    ViewState["INVOICECODE"] = Request.QueryString["INVOICECODE"].ToString();
                }
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        try

        {
            int iRowCount = 0;
            if (ViewState["INVOICECODE"] != null)
            {
                DataTable dt = PhoenixAccountsInvoice.InvoiceVesselList(new Guid(ViewState["INVOICECODE"].ToString()));

                gvVesselList.DataSource = dt;
                gvVesselList.VirtualItemCount = iRowCount;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselList_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdRemove");
                RadLabel lblVesselCanRemoveYN = (RadLabel)e.Item.FindControl("lblVesselCanRemoveYN");
                if (db != null)
                {
                    if (General.GetNullableString(lblVesselCanRemoveYN.Text) == "1")
                    {
                        db.Visible = true;
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to remove this vessel?')");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvVesselList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "REMOVEVESSEL")
            {
                string id = ((RadLabel)e.Item.FindControl("lblId")).Text;
                PhoenixAccountsInvoice.InvoiceVesselListRemove(new Guid(ViewState["INVOICECODE"].ToString()), new Guid(id));
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVesselList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvVesselList.SelectedIndexes.Clear();
        gvVesselList.EditIndexes.Clear();
        gvVesselList.DataSource = null;
        gvVesselList.Rebind();
    }
}
