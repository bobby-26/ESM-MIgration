using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsInvoiceOutgoingInvoice : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
      //  if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (Request.QueryString["qEarmarkId"] != null)
            {
                toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
                ViewState["EarmarkId"] = Request.QueryString["qEarmarkId"].ToString();
                EarMarkEdit(ViewState["EarmarkId"].ToString());
                txtPersonPrepared.ReadOnly = true;
            }
            else
            {
                txtDateofDispatch.Text = DateTime.Now.ToString();
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                lblPersonPrepared.Visible = false;
                txtPersonPrepared.Visible = false;
            }

            MenuCompanyList.AccessRights = this.ViewState;
            MenuCompanyList.Title = "Out Going Invoice";
            MenuCompanyList.MenuList = toolbar.Show();
        }
    }

    protected void CompanyList_TabStripCommand(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1', null, null);";
        Script += "</script>" + "\n";

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidEarmark(txtDateofDispatch.Text, txtBillNumber.Text, ddlEarmarkedCompany.SelectedCompany))
            {
                ucError.Visible = true;
                return;
            }

            try
            {
                string csvInvoiceList = string.Empty;
                foreach (ListItem item in CblInvoiceList.Items)
                {
                    if (item.Selected) csvInvoiceList += item.Text + ",";
                }
                if (ViewState["EarmarkId"] != null)
                {
                    PhoenixAccountsInvoice.InvoiceEarmarkUpdate(
                                                                    new Guid(ViewState["EarmarkId"].ToString()),
                                                                    csvInvoiceList,
                                                                    General.GetNullableDateTime(txtDateofDispatch.Text),
                                                                    General.GetNullableString(txtBillNumber.Text),
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    General.GetNullableInteger(ddlEarmarkedCompany.SelectedCompany),
                                                                    PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    );
                }

                else
                {
                    PhoenixAccountsInvoice.InvoiceEarmarkInsert(csvInvoiceList,
                                                                    General.GetNullableDateTime(txtDateofDispatch.Text),
                                                                    General.GetNullableString(txtBillNumber.Text),
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    General.GetNullableInteger(ddlEarmarkedCompany.SelectedCompany),
                                                                    PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    );
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


    private bool IsValidEarmark(string strDateofdispatch, string strAirwayBillnumber, string strEarmarkedCompany)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strDateofdispatch == null)
            ucError.ErrorMessage = "Date of dispatch is required.";
        if (strAirwayBillnumber.Trim().Equals(""))
            ucError.ErrorMessage = "Airway bill number is required.";
        if (strEarmarkedCompany.Trim().ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Earmark company is required.";
        else if (strEarmarkedCompany == PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
            ucError.ErrorMessage = "Originating company and Earmark company should not be same.";

        string csvInvoiceList = string.Empty;
        int Isselected = 0;
        foreach (ListItem item in CblInvoiceList.Items)
        {
            if (item.Selected)
            {
                Isselected = 1;
                csvInvoiceList += item.Text + ",";
            }
        }
        if (Isselected == 0)
            ucError.ErrorMessage = "Invoice selection is required.";

        return (!ucError.IsError);
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

            CblInvoiceList.DataSource = PhoenixAccountsInvoice.InvoiceEarmarkList(int.Parse(dr["FLDEARMARKCOMPANYID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.CompanyID, General.GetNullableGuid(strEarMarkId));
            CblInvoiceList.DataTextField = "FLDINVOICENUMBER";
            CblInvoiceList.DataValueField = "FLDINVOICECODE";
            CblInvoiceList.DataBind();

            string[] csvInvoiceList = dr["FLDINVOICECODELIST"].ToString().Split(',');
            foreach (string str in csvInvoiceList)
            {
                if (str == string.Empty) continue;
                CblInvoiceList.Items.FindByText(str).Selected = true;
            }
            ddlEarmarkedCompany.SelectedCompany = dr["FLDEARMARKCOMPANYID"].ToString();
        }
    }

    protected void PopulateInvoice(object sender, EventArgs e)
    {
        if (ddlEarmarkedCompany.SelectedCompany != "Dummy")
        {
            CblInvoiceList.DataSource = PhoenixAccountsInvoice.InvoiceEarmarkList(int.Parse(ddlEarmarkedCompany.SelectedCompany.ToString()), PhoenixSecurityContext.CurrentSecurityContext.CompanyID, null);
            CblInvoiceList.DataTextField = "FLDINVOICENUMBER";
            CblInvoiceList.DataValueField = "FLDINVOICECODE";
            CblInvoiceList.DataBind();
        }
    }
}
