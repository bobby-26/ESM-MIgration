using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersAddressRelation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbarAddressMapping = new PhoenixToolbar();
            toolbarAddressMapping.AddButton("Relation", "RELATION",ToolBarDirection.Right);
            toolbarAddressMapping.AddButton("Address", "ADDRESS",ToolBarDirection.Right);
            MenuAddressRelationMapping.AccessRights = this.ViewState;
            MenuAddressRelationMapping.MenuList = toolbarAddressMapping.Show();
            MenuAddressRelationMapping.SelectedMenuIndex = 1;
            //MenuAddressRelationMapping.SetTrigger(pnlAddressEntry);

            PhoenixToolbar toolbaraddressrelationsearch = new PhoenixToolbar();
            toolbaraddressrelationsearch.AddFontAwesomeButton("../Registers/RegistersAddressRelation.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuAddressRelationSearch.AccessRights = this.ViewState;
            MenuAddressRelationSearch.MenuList = toolbaraddressrelationsearch.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["ADDRESSCODE"] != null)
                {
                    ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }

    protected void MenuAddressRelationMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("RELATION"))
            {
                Response.Redirect("../Registers/RegistersAddressRelationList.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuAddressRelationSearch_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                RadGrid1.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        RadGrid1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        ds = PhoenixRegistersAddressRelation.AddressRelationSearch(
            General.GetNullableString(txtCode.Text)
            ,General.GetNullableString(txtAddressName.Text)
            ,General.GetNullableInteger(ucCountry.SelectedCountry)
            ,sortexpression
            ,sortdirection
            ,RadGrid1.CurrentPageIndex + 1
            ,RadGrid1.PageSize
            , ref iRowCount
            ,ref iTotalPageCount);

        RadGrid1.DataSource = ds;
        RadGrid1.VirtualItemCount = iRowCount;

        //ViewState["ROWCOUNT"] = iRowCount;
        //ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;

        try
        {
            if (e.CommandName.ToUpper().Equals("ADDADDRESS"))
            {
                PhoenixRegistersAddressRelation.InsertAddressRelation(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , ((RadLabel)eeditedItem.FindControl("lblCode")).Text
                    , int.Parse(ViewState["ADDRESSCODE"].ToString())
                    , int.Parse(((RadLabel)eeditedItem.FindControl("lblAddressCode")).Text)
                    , ((RadLabel)eeditedItem.FindControl("lblAddressName")).Text
                    , General.GetNullableString(((RadLabel)eeditedItem.FindControl("lblPhone1")).Text)
                    , General.GetNullableInteger(((RadLabel)eeditedItem.FindControl("lblCountryId")).Text)
                    , General.GetNullableInteger(((RadLabel)eeditedItem.FindControl("lblCityId")).Text)
                    , General.GetNullableInteger(((RadLabel)eeditedItem.FindControl("lblstatusId")).Text)
                    );
                ucStatus.Text = "Address relation is created";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid _gridView = (RadGrid)sender;
        int nCurrentRow = e.Item.ItemIndex;

        LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

    }
  
}
