using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
public partial class CommonPickListAddressRelation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarAddressRelation = new PhoenixToolbar();
                toolbarAddressRelation.AddButton("Relation", "RELATION", ToolBarDirection.Right);
                toolbarAddressRelation.AddButton("Address", "ADDRESS", ToolBarDirection.Right);
                MenuAddressRelation.AccessRights = this.ViewState;
                MenuAddressRelation.MenuList = toolbarAddressRelation.Show();
                MenuAddressRelation.SelectedMenuIndex = 0;

                if (Request.QueryString["ADDRESSCODE"] != null)
                {
                    ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
                }

                if (Request.QueryString["ADDRESSTYPE"] != null)
                {
                    ViewState["ADDRESSTYPE"] = Request.QueryString["ADDRESSTYPE"];
                }

                if (Request.QueryString["ADDRESSSCREEN"] != null)
                {
                    ViewState["ADDRESSSCREEN"] = Request.QueryString["ADDRESSSCREEN"];
                }
            }
                BindData();
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
            DataSet ds = PhoenixRegistersAddressRelation.ListAddressRelation(
                int.Parse(ViewState["ADDRESSCODE"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAddressRelation.DataSource = ds;
                gvAddressRelation.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AddressRelation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ADDRESS"))
            {
                if ((ViewState["ADDRESSSCREEN"].ToString()) == "ADDRESS")
                {
                    if(Request.QueryString["P"] == "1")
                        Response.Redirect("../Common/CommonPickListAddressOwner.aspx?addresstype=" + ViewState["ADDRESSTYPE"], false);
                    else
                        Response.Redirect("../Common/CommonPickListAddress.aspx?addresstype=" + ViewState["ADDRESSTYPE"], false);
                }
                else if ((ViewState["ADDRESSSCREEN"].ToString()) == "ACCSUPPLIER")
                {
                    Response.Redirect("../Common/CommonPickListAccountsSupplier.aspx?addresstype=" + ViewState["ADDRESSTYPE"],false);
                }
                else if ((ViewState["ADDRESSSCREEN"].ToString()) == "VSLSUPPLIER")
                {
                    Response.Redirect("../Common/CommonPickListVesselSupplier.aspx?addresstype=" + ViewState["ADDRESSTYPE"],false);
                }
                else
                {
                    Response.Redirect("../Common/CommonPickListAddress.aspx?addresstype=" + ViewState["ADDRESSTYPE"], false);
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAddressRelation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        string Script = "";
        NameValueCollection nvc;

        if (Request.QueryString["mode"] == "custom")
        {
            if (Request.QueryString["ignoreiframe"] != null)
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                Script += "</script>" + "\n";
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";

                if (ViewState["framename"] != null)
                    Script += "fnReloadList('codehelp1','" + ViewState["framename"].ToString() + "');";
                else
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
            }

            nvc = new NameValueCollection();

            RadLabel lblcode = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblCode");
            nvc.Add(lblcode.ID, lblcode.Text.ToString());

            LinkButton lnkAddressName = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkAddressName");
            nvc.Add(lnkAddressName.ID, lnkAddressName.Text.ToString());

            RadLabel lblAddressCode = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblAddressCode");
            nvc.Add(lblAddressCode.ID, lblAddressCode.Text.ToString());
        }
        else if (Request.QueryString["emailyn"] == "1")
        {
            if (Request.QueryString["ignoreiframe"] != null)
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                Script += "</script>" + "\n";
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (ViewState["framename"] != null)
                    Script += "fnClosePickList('codehelp1','" + ViewState["framename"].ToString() + "');";
                else
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
            }

            nvc = Filter.CurrentPickListSelection;

            RadLabel lblcode = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblCode");
            nvc.Set(nvc.GetKey(1), lblcode.Text.ToString());

            LinkButton lnkAddressName = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkAddressName");
            nvc.Set(nvc.GetKey(2), lnkAddressName.Text.ToString());

            RadLabel lblAddressCode = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblAddressCode");
            nvc.Set(nvc.GetKey(3), lblAddressCode.Text.ToString());

            RadLabel lblemail = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblEnail");
            nvc.Set(nvc.GetKey(4), lblemail.Text);
        }
        else
        {
            if (Request.QueryString["ignoreiframe"] != null)
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                Script += "</script>" + "\n";
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (ViewState["framename"] != null)
                    Script += "fnClosePickList('codehelp1','" + ViewState["framename"].ToString() + "');";
                else
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
            }

            nvc = Filter.CurrentPickListSelection;

            RadLabel lblcode = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblCode");
            nvc.Set(nvc.GetKey(1), lblcode.Text.ToString());

            LinkButton lnkAddressName = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkAddressName");
            nvc.Set(nvc.GetKey(2), lnkAddressName.Text.ToString());

            RadLabel lblAddressCode = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblAddressCode");
            nvc.Set(nvc.GetKey(3), lblAddressCode.Text.ToString());
        }

        Filter.CurrentPickListSelection = nvc;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
