using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;

public partial class PurchaseAmosFormGeneral : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        SessionUtil.PageFieldViewPermission(this.ViewState);   
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        try
        {
            toolbarmain.AddButton("Save", "SAVE");
            MenuFormGeneral.AccessRights = this.ViewState; 
            MenuFormGeneral.MenuList = toolbarmain.Show();
            MenuFormGeneral.SetTrigger(pnlFormGeneral);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            txtVendor.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                if ((Request.QueryString["orderid"] != null) && (Request.QueryString["orderid"] != ""))
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    BindData(Request.QueryString["orderid"].ToString());
                }
            }
            FieldSetViewState();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData(string  orderid)
    {
        DataSet ds = new DataSet();
        ds = PhoenixPurchaseAmosForm.EditAmosForm(new Guid(orderid));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtFormNumber.Text = dr["FLDFORMNO"].ToString();
            txtFromTitle.Text = dr["FLDTITLE"].ToString();
            Title1.Text = "General      (" + dr["FLDFORMNO"].ToString() + ")     ";
            txtFinalTotal.Text = String.Format("{0:##,###,###.00}", dr["XTOTAMT"]);
            txtVendorNumber.Text = dr["XSUP"].ToString();
            //txtVenderEsmeted.Text = String.Format("{0:##,###,###.00}", dr["XAPPAMT"]);
            txtOrderDate.Text = General.GetDateTimeToString(dr["XDATE"].ToString());
            txtType.Text = dr["XDiv"].ToString();
            txtVenderName.Text = dr["FLDNAME"].ToString();
            txtVendor.Text = dr["FLDVENDORID"].ToString();
            ucCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            if (dr["XISDEFECTPO"].ToString() == "1")
                chkIssues.Checked = true;
            txtDescription.Text = dr["XDEFECTDESC"].ToString();
            cmdvendorAddress.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', '../Purchase/PurchaseFormAddress.aspx?addresscode=" + txtVendor.Text + "');return false;");          
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
                UpdateOrderForm();
                ucStatus.Text = "Requisition Updated";
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
       
            }        
          
           
          
            BindData(ViewState["orderid"].ToString());
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }
  
    private void UpdateOrderForm()
    {
        PhoenixPurchaseAmosForm.UpdateAmosForm(
            new Guid(ViewState["orderid"].ToString())
            , chkIssues.Checked?1:0
            , txtDescription.Text
            , General.GetNullableInteger(txtVendor.Text)
            , General.GetNullableInteger(ucCurrency.SelectedCurrency)
            , General.GetNullableDecimal(txtFinalTotal.Text));
    }

    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";  
        if (chkIssues.Checked && txtDescription.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required for Having issues PO.";
        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
   
    private void FieldSetViewState()
    { 
        txtFinalTotal.Visible = IsVisible("txtFinalTotal");
        lblFinalTotal.Visible = IsVisible("lblFinalTotal");
    }

    public bool IsVisible(string command)
    {
        NameValueCollection nvc = null;
        if (ViewState["FIELDVIEWPERMISSION"]== null)
            return true;
        else
        {
            nvc = (NameValueCollection)ViewState["FIELDVIEWPERMISSION"];
            return (nvc[command] == "0") ? false : true;
        }
    }
}
