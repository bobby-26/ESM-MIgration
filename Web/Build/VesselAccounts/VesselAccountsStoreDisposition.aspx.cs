using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsStoreDisposition : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState); PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsStoreDisposition.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuStoreItem.AccessRights = this.ViewState;
            MenuStoreItem.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Store Item Opening", "OPENING");
            toolbar.AddButton("Store Disposition", "DISPOSITION");
            toolbar.AddButton("Purchase Order UnConfirm", "UCPROVISIONANDBOND");
            toolbar.AddButton("Issue of Bonded Stores", "BONDEDSTOREISSUE");
           // toolbar.AddButton("Round Off", "ROUNDOFF");
           // toolbar.AddButton("Rob Initialize", "ROB");
            MenuStoreAdmin.AccessRights = this.ViewState;
            MenuStoreAdmin.MenuList = toolbar.Show();
            MenuStoreAdmin.SelectedMenuIndex = 1;
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvStoreItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStoreAdmin_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("OPENING"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsStoreItemOpening.aspx";
            }
            else if (CommandName.ToUpper().Equals("DISPOSITION"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsStoreDisposition.aspx";
            }
            else if (CommandName.ToUpper().Equals("UCPROVISIONANDBOND"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsOrderFormUnConfirm.aspx";
            }
            else if (CommandName.ToUpper().Equals("BONDEDSTOREISSUE"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsAdminBondedStoreIssue.aspx";
            }
            else if (CommandName.ToUpper().Equals("ROUNDOFF"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsRoundOffUpdate.aspx";
            }
            else if (CommandName.ToUpper().Equals("ROB"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsOpeningRobExcelUpload.aspx";
            }
            Response.Redirect(ViewState["CURRENTTAB"].ToString(), false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStoreItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvStoreItem.SelectedIndexes.Clear();
        gvStoreItem.EditIndexes.Clear();
        gvStoreItem.DataSource = null;
        gvStoreItem.Rebind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDOPENINGSTOCK", "FLDPURCHASEDSTOCK", "FLDCLOSINGSTOCK", "FLDCONSUMEDSTOCK" };
            string[] alCaptions = { "Number", "Name", "Unit", "Opening Stock", "Purchased Stock", "Closing Stock", "Consumption" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixVesselAccountsCorrections.SearchStoreDisposition(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , General.GetNullableInteger(ddlDispositionType.SelectedHard), General.GetNullableInteger(ddlStockClass.SelectedHard)
                , General.GetNullableDateTime(txtFDate.Text), General.GetNullableDateTime(txtTDate.Text)
                , txtNumber.Text, txtName.Text, sortexpression, sortdirection, Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), gvStoreItem.PageSize,
                ref iRowCount, ref iTotalPageCount);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvStoreItem", "Stock check of Provision Items", alCaptions, alColumns, ds);
            gvStoreItem.DataSource = dt;
            gvStoreItem.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreItem_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton de = (LinkButton)e.Item.FindControl("cmdDelete");
            if (de != null)
            {
                de.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName);
            }
        }
    }
    protected void gvStoreItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string dispositionid = ((RadLabel)e.Item.FindControl("lblDispositionID")).Text;
                PhoenixVesselAccountsCorrections.DeleteStoreDisposition(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(dispositionid));
                Rebind();
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
    protected void gvStoreItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvStoreItem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}
