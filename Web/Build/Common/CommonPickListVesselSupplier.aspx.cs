using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CommonPickListVesselSupplier : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuAddress.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["txtsupname"] != null)
                txtNameSearch.Text = Request.QueryString["txtsupname"].ToString();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvAddress.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string addresstype = null;
            string producttype = null;
            if (ViewState["addresstype"] != null)
                addresstype = ViewState["addresstype"].ToString();
            if (ViewState["producttype"] != null)
                producttype = ViewState["producttype"].ToString();

            ds = PhoenixCommonRegisters.VesselSupplierSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null
                , txtNameSearch.Text, General.GetNullableInteger(ddlCountry.SelectedCountry), null, null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvAddress.PageSize, ref iRowCount, ref iTotalPageCount);
            gvAddress.DataSource = ds;
            gvAddress.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvAddress.SelectedIndexes.Clear();
        gvAddress.EditIndexes.Clear();
        gvAddress.DataSource = null;
        gvAddress.Rebind();
    }
    protected void MenuAddress_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
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
    protected void gvAddress_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAddress.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAddress_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
         
            if (e.CommandName.ToUpper().Equals("RELATION"))
            {   RadLabel lblSupplierCode = (RadLabel)e.Item.FindControl("lblSupplierCode");
                Response.Redirect("../Common/CommonPickListAddressRelation.aspx?addresscode=" + lblSupplierCode.Text + "&addressscreen=VSLSUPPLIER");
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                string Script = "";
                NameValueCollection nvc;
                if (Request.QueryString["mode"] == "custom")
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";

                    nvc = new NameValueCollection();
                    RadLabel lblCode = (RadLabel)e.Item.FindControl("lblCode");
                    nvc.Add(lblCode.ID, lblCode.Text);
                    LinkButton lb = (LinkButton)e.Item.FindControl("lnkSupplierName");
                    nvc.Add(lb.ID, lb.Text.ToString());
                    RadLabel lbl = (RadLabel)e.Item.FindControl("lblSupplierCode");
                    nvc.Add(lbl.ID, lbl.Text.ToString());

                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";

                    nvc = Filter.CurrentPickListSelection;

                    RadLabel lblCode = (RadLabel)e.Item.FindControl("lblCode");
                    nvc.Set(nvc.GetKey(1), lblCode.Text);

                    LinkButton lb = (LinkButton)e.Item.FindControl("lnkSupplierName");
                    nvc.Set(nvc.GetKey(2), lb.Text.ToString());

                    RadLabel lbl = (RadLabel)e.Item.FindControl("lblSupplierCode");
                    nvc.Set(nvc.GetKey(3), lbl.Text);
                }

                Filter.CurrentPickListSelection = nvc;
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
                //   Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
}
