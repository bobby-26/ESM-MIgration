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
public partial class CommonPickListAddress : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        MenuAddress.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            cblProductType.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    Convert.ToInt32(PhoenixQuickTypeCode.GENERALPRODUCTTYPE));
            cblProductType.DataBindings.DataTextField = "FLDQUICKNAME";
            cblProductType.DataBindings.DataValueField = "FLDQUICKCODE";
            cblProductType.DataBind();

            if ((Request.QueryString["addresstype"] != null) && (Request.QueryString["addresstype"] != ""))
                ViewState["addresstype"] = "," + Request.QueryString["addresstype"].ToString() + ",";
            //else
            //    Response.Redirect("../PhoenixUnderConstruction.aspx");
            if (Request.QueryString["txtsupcode"] != null)
            {
                txtCode.Text = Request.QueryString["txtsupcode"].ToString();
            }
            if (Request.QueryString["txtsupname"] != null)
            {
                txtNameSearch.Text = Request.QueryString["txtsupname"].ToString();
            }
            if ((Request.QueryString["producttype"] != null) && (Request.QueryString["productype"] != ""))
            {
                ViewState["producttype"] = "," + Request.QueryString["producttype"].ToString() + ",";

                string[] producttype = ViewState["producttype"].ToString().Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in producttype)
                {
                    if (item.Trim() != "")
                    {
                        cblProductType.SelectedValue = item;
                        //cblProductType.Items.FindByValue(item).Selected = true; //for asp control
                        cblProductType.Items[0].Selected = true;    //for telerik control
                    }
                }
            }
            if (Request.QueryString["framename"] != null)
            {
                ViewState["framename"] = Request.QueryString["framename"].ToString();
            }

            if ((Request.QueryString["emailid"] != null) && (Request.QueryString["emailid"] != ""))
                ViewState["emailid"] = Request.QueryString["emailid"].ToString();


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
            else
                producttype = "";

            string emailid = null;
            if (ViewState["emailid"] != null)
                emailid = ViewState["emailid"].ToString();

            if (Filter.CurrentCommonAddressFilterCriteria != null)
            {
                NameValueCollection addressnvc = Filter.CurrentCommonAddressFilterCriteria;
                ds = PhoenixCommonRegisters.PurchaseAddressSearch(addressnvc.Get("txtCode").ToString()
                    , addressnvc.Get("txtNameSearch").ToString(), null,
                    null, null,
                    General.GetNullableString(addressnvc.Get("txtCountryNameSearch").ToString()), addresstype,
                    General.GetNullableString(producttype), null, null, General.GetNullableString(emailid), null, null, sortexpression, sortdirection,
                    gvAddress.CurrentPageIndex + 1,
                    gvAddress.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount
                    , 0);
            }
            else
            {
                ds = PhoenixCommonRegisters.PurchaseAddressSearch(txtCode.Text
                    , txtNameSearch.Text, null,
                    null, null,
                    General.GetNullableString(txtCountryNameSearch.Text), addresstype,
                    General.GetNullableString(producttype), null, null, General.GetNullableString(emailid), null, null, sortexpression, sortdirection,
                    gvAddress.CurrentPageIndex + 1,
                    gvAddress.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount
                    , 0);
            }

            gvAddress.DataSource = ds;
            gvAddress.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

                NameValueCollection addresscriteria = new NameValueCollection();

                addresscriteria.Clear();
                addresscriteria.Add("txtNameSearch", txtNameSearch.Text);
                addresscriteria.Add("txtCode", txtCode.Text);
                addresscriteria.Add("txtCountryNameSearch", txtCountryNameSearch.Text);

                Filter.CurrentCommonAddressFilterCriteria = addresscriteria;

                StringBuilder strproducttype = new StringBuilder();

                foreach (ButtonListItem item in cblProductType.Items)
                {
                    if (item.Selected == true)
                    {
                        strproducttype.Append(item.Value.ToString());
                        strproducttype.Append(",");
                    }
                }
                if (strproducttype.Length > 1)
                {
                    strproducttype.Remove(strproducttype.Length - 1, 1);
                }

                ViewState["producttype"] = strproducttype.ToString();
                gvAddress.EditIndexes.Clear();
                gvAddress.SelectedIndexes.Clear();
                gvAddress.DataSource = null;
                BindData();
                gvAddress.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAddress_ItemCommand(object sender, GridCommandEventArgs e)
    {
        string Script = "";
        NameValueCollection nvc;

        if (e.CommandName.ToUpper().Equals("RELATION"))
        {
            RadLabel lblAddressCode = (RadLabel)e.Item.FindControl("lblAddressCode");
            Response.Redirect("../Common/CommonPickListAddressRelation.aspx?addresscode=" + lblAddressCode.Text + "&addressscreen=ADDRESS&addresstype=" + Request.QueryString["addresstype"]);
        }

        if (e.CommandName.ToUpper().Equals("PICKLIST"))
        {
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
                        Script += "fnReloadList('codehelp1');";
                    Script += "</script>" + "\n";
                }

                nvc = new NameValueCollection();
                RadLabel lblCode = (RadLabel)e.Item.FindControl("lblCode");
                nvc.Add(lblCode.ID, lblCode.Text);
                LinkButton lb = (LinkButton)e.Item.FindControl("lnkAddressName");
                nvc.Add(lb.ID, lb.Text.ToString());
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblAddressCode");
                nvc.Add(lbl.ID, lbl.Text.ToString());

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
                        Script += "fnClosePickList('codehelp1');";
                    Script += "</script>" + "\n";
                }

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblCode = (RadLabel)e.Item.FindControl("lblCode");
                nvc.Set(nvc.GetKey(1), lblCode.Text);

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkAddressName");
                nvc.Set(nvc.GetKey(2), lb.Text.ToString());

                RadLabel lbl = (RadLabel)e.Item.FindControl("lblAddressCode");
                nvc.Set(nvc.GetKey(3), lbl.Text);

                RadLabel lblemail = (RadLabel)e.Item.FindControl("lblEmail");
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
                        Script += "fnClosePickList('codehelp1');";
                    Script += "</script>" + "\n";
                }

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblCode = (RadLabel)e.Item.FindControl("lblCode");
                nvc.Set(nvc.GetKey(1), lblCode.Text);

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkAddressName");
                nvc.Set(nvc.GetKey(2), lb.Text.ToString());

                RadLabel lbl = (RadLabel)e.Item.FindControl("lblAddressCode");
                nvc.Set(nvc.GetKey(3), lbl.Text);
            }

            Filter.CurrentPickListSelection = nvc;
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        if (e.CommandName.ToUpper().Equals("PAGE"))
        {
            gvAddress.Rebind();
        }
    }
    protected void gvAddress_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
            //  && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            //{
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblEmail1");
            RadLabel lblAddressCode = (RadLabel)e.Item.FindControl("lblAddressCode");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucEmailTT");

            HtmlImage img = (HtmlImage)e.Item.FindControl("imgEmail");
            if (img != null)
                img.Attributes.Add("onclick", "openNewWindow('codehelp2','Emails of the address', '" + Session["sitepath"] + "/Registers/RegistersAddressEmail.aspx?addresscode=" + lblAddressCode.Text + "', 'medium')");

            if (lbtn != null && uct != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
            lbtn = (RadLabel)e.Item.FindControl("lblServices");
            uct = (UserControlToolTip)e.Item.FindControl("ucServicesTT");

            if (lbtn != null && uct != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
            //}
        }
    }
    protected void gvAddress_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
}
