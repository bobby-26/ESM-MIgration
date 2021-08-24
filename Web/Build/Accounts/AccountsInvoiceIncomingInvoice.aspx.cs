using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsInvoiceIncomingInvoice : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        txtDateofDispatch.DatePicker = false;        
       // if (!IsPostBack)
        {
           if (txtRecieveDate.Text == null || txtRecieveDate.Text == "" || txtRecieveDate.Text == "__/__/____")
                txtRecieveDate.Text = DateTime.Now.ToString();   
            PhoenixToolbar toolbar = new PhoenixToolbar();         
            if (Request.QueryString["qEarmarkId"] != null)
            {
                toolbar.AddButton("Received", "SAVE");
                ViewState["EarmarkId"] = Request.QueryString["qEarmarkId"].ToString();
                EarMarkEdit(ViewState["EarmarkId"].ToString());
            }

            MenuCompanyList.AccessRights = this.ViewState;
            MenuCompanyList.MenuList = toolbar.Show();
        }
    }

    protected void CompanyList_TabStripCommand(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1', null, null);";
        Script += "</script>" + "\n";

        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                string csvInvoiceList = string.Empty;
                foreach (ListItem item in CblInvoiceList.Items)
                {
                    if (item.Selected) csvInvoiceList += item.Text  + ",";
                }
                if (ViewState["EarmarkId"] != null)
                {
                    if (txtRecieveDate.Text == null || txtRecieveDate.Text == "")
                        txtRecieveDate.Text = DateTime.Now.ToString();
                    if (Convert.ToDateTime(txtRecieveDate.Text) <= DateTime.Now)
                    {                        
                        PhoenixAccountsInvoice.InvoiceEarmarkDispatchReceive(new Guid(ViewState["EarmarkId"].ToString()), General.GetNullableInteger(ddlEarmarkedCompany.SelectedCompany), csvInvoiceList, PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToDateTime(txtRecieveDate.Text));
                    }
                    else 
                    {
                        ucError.ErrorMessage = "Received date should not be greater than today's date ";
                        ucError.Visible = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
    }   

    private void EarMarkEdit(string strEarMarkId)
    {       
        DataSet ds = PhoenixAccountsInvoice.InvoiceEarmarkEdit(new Guid(strEarMarkId));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtDateofDispatch.Text = dr["FLDDATEOFDISPATCH"].ToString();
            txtBillNumber.Text = dr["FLDAIRWAYBILLNUMBER"].ToString();
            txtPersonPrepared.Text = dr["FLDUSERNAME"].ToString();
            txtRecieveDate.Text = dr["FLDRECEIVEDATE"].ToString();       
            CblInvoiceList.DataSource = PhoenixAccountsInvoice.EarmarkReceiveInvoiceList (General.GetNullableGuid (strEarMarkId));
            CblInvoiceList.DataTextField = "FLDINVOICENUMBER";
            CblInvoiceList.DataValueField = "FLDINVOICECODE";
            CblInvoiceList.DataBind();

            string[] csvInvoiceList = dr["FLDRECEIVEDINVOICECODELIST"].ToString().Split(',');
            foreach (string str in csvInvoiceList)
            {
                if (str == string.Empty) continue;
                CblInvoiceList.Items.FindByText(str).Selected = true;
            }
            ddlEarmarkedCompany.SelectedCompany = dr["FLDEARMARKCOMPANYID"].ToString();
        }
    }  
}
