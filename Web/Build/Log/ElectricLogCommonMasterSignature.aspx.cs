using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogCommonMasterSignature : PhoenixBasePage
{
    string txids;
    protected void Page_Load(object sender, EventArgs e)
    {
        int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        ShowToolBar();
        if (String.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            txids = Request.QueryString["TxnId"];
        }
        if (String.IsNullOrWhiteSpace(Request.QueryString["IsAnnex"]) == false)
        {
            ViewState["ISANNEX"] = Request.QueryString["IsAnnex"];
        }
        if (!IsPostBack)
        {
            ViewState["UserCode"] = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        }
        ShowToolBar();
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (!isValidSignInput(username, password))
            {
                ucError.Visible = true;
                return;
            }
            ds = PhoenixElog.UserLogin(username, password);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                int usercode = Convert.ToInt32(dr["FLDUSERCODE"].ToString());
                ViewState["UserCode"] = Convert.ToInt32(dr["FLDUSERCODE"].ToString());
                DataSet userdetail = GetUserDetails(usercode);

                if (userdetail != null && userdetail.Tables.Count > 0 && userdetail.Tables[0].Rows.Count > 0)
                {
                    DataRow userdetailRow = userdetail.Tables[0].Rows[0];
                    if(ViewState["ISANNEX"] != null && ViewState["ISANNEX"].ToString() == "1")
                    {
                        if (userdetailRow["FLDRANKCODE"].ToString() != "CE" && ViewState["ISANNEX"].ToString() == "1")
                        {
                            throw new ArgumentException("Only Chief Engineer can do the signature");
                        }
                    }
                    else if (userdetailRow["FLDRANKCODE"].ToString() != "MST")
                    {
                        throw new ArgumentException("Only Master can do the signature");
                    }

                    txtUserCode.Text = ViewState["UserCode"].ToString();
                    lblMasterFirstName.Text = userdetailRow["FLDFIRSTNAME"].ToString();
                    lblMasterLastName.Text = userdetailRow["FLDLASTNAME"].ToString();
                    string masterName = userdetailRow["FLDFIRSTNAME"].ToString() + "   " + userdetailRow["FLDLASTNAME"].ToString();
                    string signature = String.Format("Logged in as {0}", userdetailRow["FLDFIRSTNAME"].ToString() + "   " + userdetailRow["FLDLASTNAME"].ToString());
                    lblLoggedUser.Text = signature;
                    txtUsername.Visible = false;
                    txtPassword.Visible = false;
                    lblUsername.Visible = false;
                    lblPassword.Visible = false;
                    btnLogin.Visible = false;
                    lblheading.Visible = false;
                    btnCancel.Visible = false;
                    lblLoggedUser.Visible = true;

                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("usercode", ViewState["UserCode"].ToString());
                    nvc.Add("mastername", masterName);
                    nvc.Add("signature", signature);
                    nvc.Add("txids", txids);
                    nvc.Add("signeddate", DateTime.Now.ToString());
                    Filter.MasterSignatureCriteria = nvc;
                }
                else
                {
                    throw new ArgumentException("Only Master can do the signature");
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private DataSet GetUserDetails(int usercode)
    {
        return PhoenixElog.GetSeaFarerRankName(usercode);
    }

    public bool isValidSignInput(string username, string password)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrWhiteSpace(username))
        {
            ucError.ErrorMessage = "UserName is required";
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            ucError.ErrorMessage = "Password is required";
        }
        return (!ucError.IsError);
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (isValidInput() == false)
                {
                    ucError.Visible = true;
                    return;
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('LogMasterSign', 'oilLog');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrWhiteSpace(txtUserCode.Text))
        {
            ucError.ErrorMessage = "Master login in is required";
        }
        return (!ucError.IsError);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string script = string.Format("closeTelerikWindow('LogMasterSign', '');");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
    }
}