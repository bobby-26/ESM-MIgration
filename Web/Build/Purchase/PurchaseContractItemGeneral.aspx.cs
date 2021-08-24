using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;

public partial class PurchaseContractItemGeneral : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            toolbarmain.AddButton("New", "NEW");
            toolbarmain.AddButton("Save", "SAVE");
            MenuFormGeneral.MenuList = toolbarmain.Show();
            MenuFormGeneral.SetTrigger(pnlFormGeneral);
            if (!IsPostBack)
            {
                rdlistDiscount.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.DISCOUNT));
                rdlistDiscount.DataBind();
                Title1.Text = " General(" + PhoenixPurchaseContract.ItemName + ")";
                if (Request.QueryString["contractid"] != null)
                    ViewState["contractid"] = Request.QueryString["contractid"].ToString();
                else
                    ViewState["contractitemid"] = "";
                if (Request.QueryString["contractitemid"] != null)
                {
                    ViewState["contractitemid"] = Request.QueryString["contractitemid"].ToString();
                    BindData(Request.QueryString["contractitemid"].ToString());
                    ViewState["SaveStatus"] = "Edit";
                   
                }
                else
                    ViewState["SaveStatus"] = "New";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    
    private void BindData(string  contractitemid)
    {

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseContract.EditContractItem(new Guid(contractitemid));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];   
            txtItemNumber.Text = dr["FLDPARTNUMBER"].ToString();
            txtItemName.Text = dr["FLDPARTNAME"].ToString();
            txtMinQuantity.Text = String.Format("{0:##,##0.00}",dr["FLDMINQUANTITY"]);
            txtMaxQuantity.Text = String.Format("{0:###,##0.00}",dr["FLDMAXQUANTITY"]);
            txtItemComments.Text = dr["FLDCOMMENTS"].ToString();
            if( dr["FLDDISCOUNTTYPE"].ToString()!=null) 
            rdlistDiscount.SelectedValue = dr["FLDDISCOUNTTYPE"].ToString();
            txtDiscount.Text = String.Format("{0:#0.00}",dr["FLDDISCOUNT"]); 
           
        }


    }
    protected void MenuFormGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidForm())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["SaveStatus"].ToString().Equals("New"))
                {
                    InsertContractItem();
                }
                else if (ViewState["SaveStatus"].ToString().Equals("Edit"))
                {
                    UpdateContractItem();
                }
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["SaveStatus"] = "New";
                ClearText();
            }
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClearText()
    {
        txtItemNumber.Text = "";
        txtItemName.Text = "";
        txtMinQuantity.Text = "";
        txtMaxQuantity.Text = "";
        txtItemComments.Text = "";
        txtDiscount.Text = "";
       
    }

  

    private void InsertContractItem()
    {
        PhoenixPurchaseContract.InsertContractItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["contractid"].ToString())
                                                     ,txtItemNumber.Text,txtItemName.Text
                                                     ,General.GetNullableDecimal(txtMinQuantity.Text),General.GetNullableDecimal(txtMaxQuantity.Text) 
                                                    ,txtItemComments.Text,General.GetNullableInteger(rdlistDiscount.SelectedValue),General.GetNullableDecimal(txtDiscount.Text) );   
    }
    private void UpdateContractItem()
    {
        PhoenixPurchaseContract.UpdateContractItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["contractitemid"].ToString())
                                                   , txtItemNumber.Text, txtItemName.Text
                                                   , General.GetNullableDecimal(txtMinQuantity.Text), General.GetNullableDecimal(txtMaxQuantity.Text)
                                                  , txtItemComments.Text, General.GetNullableInteger(rdlistDiscount.SelectedValue), General.GetNullableDecimal(txtDiscount.Text));   
    }
    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";   

        if (txtItemNumber.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Item number is required.";
        if(txtItemName.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Item name is required.";
        return (!ucError.IsError);
    }
}
