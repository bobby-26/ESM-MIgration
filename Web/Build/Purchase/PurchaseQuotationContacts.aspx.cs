using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseQuotationContacts : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
    
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            txtVenderID.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {  
                if (Request.QueryString["quotationid"] != null)
                   {
                       ViewState["quotationid"] = Request.QueryString["quotationid"].ToString(); 
                       BindFields(ViewState["quotationid"].ToString());
                   }             
            }
            
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFields(string quotationid)
    {
        DataSet quotationdataset = new DataSet();
        quotationdataset = PhoenixPurchaseQuotation.EditQuotation(new Guid(quotationid));
        if (quotationdataset.Tables[0].Rows.Count > 0)
        {
            DataRow dr = quotationdataset.Tables[0].Rows[0];
            txtVenderCode.Text = dr["FLDVENDORCODE"].ToString();
            txtVenderID.Text = dr["FLDVENDORID"].ToString();
            txtVenderName.Text = dr["FLDNAME"].ToString();
            PhoenixPurchaseQuotation.VendorName = dr["FLDNAME"].ToString();
           
           
        }
    }

    protected bool IsValidTax(string strDescription, string strValueType, string strValue)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if(strDescription.Trim() == string.Empty)
            ucError.ErrorMessage = "Email address is required.";
        if (strValue.Trim() == string.Empty)
            ucError.ErrorMessage = "Email option is required.";
        if (strValueType.Trim() == string.Empty)
            ucError.ErrorMessage = "Purpose is required.";
        return (!ucError.IsError);
    }

    
    protected void InsertQuotationTax(string stremailaddress, string relationship, string emailoption)
    {
        try
        {
            
          
            //PhoenixPurchaseQuotation.QuotationContactsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(txtVenderID.Text), stremailaddress, relationship, emailoption, Filter.CurrentPurchaseStockType.ToString(), vesselid);
            PhoenixPurchaseQuotation.QuotationContactsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(txtVenderID.Text), stremailaddress, relationship, emailoption, Filter.CurrentPurchaseStockType.ToString(), Filter.CurrentPurchaseVesselSelection);
         
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void UpdateQuotationTax(Guid addresscontactid, string stremailaddress, string relationship, string emailoption)
    {
        try
        {
            PhoenixPurchaseQuotation.QuotationContactsUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, addresscontactid, int.Parse(txtVenderID.Text), stremailaddress, relationship, emailoption);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void DeleteQuotationTax(Guid relationshipid)
    {
        PhoenixPurchaseQuotation.QuotationContactsDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, relationshipid);
    }
    private bool IsValidVender()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtVenderID.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Vendor is required. Please Select ";
        //if (General.GetDateTimeToString(txtExpirationDate.Text) != null && Convert.ToDateTime(txtExpirationDate.Text) <= Convert.ToDateTime(DateTime.Now.ToString()))
        //    ucError.ErrorMessage = "Expiry  date should be greater than today's date.";  
        if (ViewState["orderid"].ToString() == "0")
            ucError.ErrorMessage = "Please select a form to assign a vendor ";
        return (!ucError.IsError);
    }

    protected void gvSupplierContact_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        //ds = PhoenixPurchaseQuotation.QuotationContactsList(General.GetNullableInteger(txtVenderID.Text), Filter.CurrentPurchaseStockType.ToString(),vesselid);
        ds = PhoenixPurchaseQuotation.QuotationContactsList(General.GetNullableInteger(txtVenderID.Text), Filter.CurrentPurchaseStockType.ToString(), Filter.CurrentPurchaseVesselSelection);
        gvSupplierContact.DataSource = ds;
    }

    protected void gvSupplierContact_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        GridEditableItem item = (GridEditableItem)e.Item;

        if (!IsValidTax(
              ((RadTextBox)item.FindControl("txtEmailAddressEdit")).Text.ToString().Trim(),
              ((RadTextBox)item.FindControl("txtRelationEdit")).Text.ToString(),
              ((RadRadioButtonList)item.FindControl("rblEmailOptionEdit")).SelectedValue.ToString()))
        {
            ucError.Visible = true;
            return;
        }

        if (General.GetNullableGuid(((RadLabel)item.FindControl("lblAddRelationShipId")).Text) != null)
        {
            UpdateQuotationTax(
                             new Guid(((RadLabel)item.FindControl("lblAddRelationShipId")).Text.ToString())
                            , ((RadTextBox)item.FindControl("txtEmailAddressEdit")).Text.ToString().Trim()
                            , ((RadTextBox)item.FindControl("txtRelationEdit")).Text.ToString()
                            , ((RadRadioButtonList)item.FindControl("rblEmailOptionEdit")).SelectedValue.ToString()
                        );
        }
        else
        {
            InsertQuotationTax(((RadTextBox)item.FindControl("txtEmailAddressEdit")).Text.ToString().Trim()
                                , ((RadTextBox)item.FindControl("txtRelationEdit")).Text.ToString()
                                , ((RadRadioButtonList)item.FindControl("rblEmailOptionEdit")).SelectedValue.ToString());
        }

        
    }

    protected void gvSupplierContact_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridDataItem item = (GridDataItem)e.Item;
            DeleteQuotationTax(new Guid(((RadLabel)item.FindControl("lblRelationShipId")).Text.ToString()));
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }

    protected void gvSupplierContact_ItemDataBound1(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton db = (LinkButton)item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
            RadLabel lb1 = (RadLabel)item.FindControl("lblEmailOption");
            RadLabel lb1edit = (RadLabel)item.FindControl("lblEmailOptionEdit");
            RadRadioButtonList rbl = (RadRadioButtonList)item.FindControl("rblEmailOption");
            RadRadioButtonList rblEdit = (RadRadioButtonList)item.FindControl("rblEmailOptionEdit");
            //DataRowView drv = (DataRowView)item.DataItem;
            if (rbl != null && lb1 != null)
            {
                rbl.SelectedValue = lb1.Text;
            }
            if (rblEdit != null&& lb1edit!=null)
            {
                rblEdit.SelectedValue = lb1edit.Text;
            }
        }
        
    }

    protected void gvSupplierContact_ItemCommand(object sender, GridCommandEventArgs e)
    {
        string CommandName = e.CommandName;
    }

    protected void gvSupplierContact_EditCommand(object sender, GridCommandEventArgs e)
    {

    }
}
