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

public partial class CommonPickListAddressOwner : PhoenixBasePage
{
	string strvslPricipal = "";
	protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            gvAddress.PageSize = General.ShowRecords(null);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
            MenuAddress.MenuList = toolbarmain.Show();
            

            ViewState["addresstype"] = null;
            ViewState["principal"] = null;
            if ((Request.QueryString["addresstype"] != null) && (Request.QueryString["addresstype"] != ""))
                ViewState["addresstype"] = "," + Request.QueryString["addresstype"].ToString() + ",";

            if (Request.QueryString["principal"] != null && Request.QueryString["principal"] != "")
                ViewState["principal"] = Request.QueryString["principal"].ToString();
            if (Request.QueryString["txtsupcode"] != null)
            {
                txtCode.Text = Request.QueryString["txtsupcode"].ToString(); 
            }         
            if (Request.QueryString["txtsupname"] != null)
            {
                txtNameSearch.Text = Request.QueryString["txtsupname"].ToString();
            }
           
            if (Request.QueryString["framename"] != null)
            {
               ViewState["framename"] = Request.QueryString["framename"].ToString();
            }
            if (Request.QueryString["windowname"] != null)
                ViewState["windowname"] = Request.QueryString["windowname"].ToString();

            if ((Request.QueryString["emailid"] != null) && (Request.QueryString["emailid"] != ""))
                ViewState["emailid"] = Request.QueryString["emailid"].ToString();


            

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ddlAddressType.DataSource = PhoenixRegistersAddress.AddressTypeList(General.GetNullableString(Convert.ToString(ViewState["addresstype"])));
            ddlAddressType.DataTextField = "FLDHARDNAME";
            ddlAddressType.DataValueField = "FLDHARDCODE";
            ddlAddressType.DataBind();
            ddlAddressType.SelectedValue = "131";


            if(ViewState["principal"] != null && General.GetNullableInteger(ViewState["principal"].ToString())!=null && PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() == "OWNER")
            {
				strvslPricipal = ViewState["principal"].ToString();
				ucPrincipal.SelectedAddress = ViewState["principal"].ToString();
                ucPrincipal.Enabled=false;
            }

        }

    }
    protected void gvAddress_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

            string addresstype = ddlAddressType.SelectedValue;

            if (Filter.CurrentPurchaseAddressFilterCriteria != null)
            {
                NameValueCollection addressnvc = Filter.CurrentPurchaseAddressFilterCriteria;
                ds = PhoenixCommonRegisters.SearchPurchaseAddress(addressnvc.Get("txtCode").ToString()
                    , addressnvc.Get("txtNameSearch").ToString(), null,
                    null, null,
                    null, addresstype,
                    null, null, null, null, null, null, sortexpression, sortdirection,
                    gvAddress.CurrentPageIndex+1,
                    gvAddress.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount
                    , 0
                    , General.GetNullableInteger(ucPrincipal.SelectedAddress) != null ? General.GetNullableInteger(ucPrincipal.SelectedAddress) : General.GetNullableInteger(strvslPricipal)
                    , ViewState["principal"] != null ? General.GetNullableInteger(ViewState["principal"].ToString()) : null);
            }
            else
            {
                ds = PhoenixCommonRegisters.SearchPurchaseAddress(txtCode.Text
                    , txtNameSearch.Text, null,
                    null, null,
                    null, addresstype,
                    null, null, null, null, null, null, sortexpression, sortdirection,
                    gvAddress.CurrentPageIndex + 1,
                    gvAddress.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount
                    , 0
                    , General.GetNullableInteger(ucPrincipal.SelectedAddress) != null ? General.GetNullableInteger(ucPrincipal.SelectedAddress) : General.GetNullableInteger(strvslPricipal)
                    , ViewState["principal"] != null ? General.GetNullableInteger(ViewState["principal"].ToString()) : null);
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
                gvAddress.SelectedIndexes.Clear();

                ViewState["PAGENUMBER"] = 1;

                NameValueCollection addresscriteria = new NameValueCollection();

                addresscriteria.Clear();
                addresscriteria.Add("txtNameSearch", txtNameSearch.Text);
                addresscriteria.Add("txtCode", txtCode.Text);

                Filter.CurrentPurchaseAddressFilterCriteria = addresscriteria;

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
            if (Request.QueryString["POPUP"] != null)
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (ViewState["framename"] != null)
                    Script += "populateTelerikPick('"+ViewState["windowname"].ToString()+"','"+ ViewState["framename"] .ToString()+ "','codehelp1');";
                else
                    Script += "fnClosePickList('codehelp1');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblCode = (RadLabel)e.Item.FindControl("lblCode");
                nvc.Set(nvc.GetKey(1), lblCode.Text);

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkAddressName");
                nvc.Set(nvc.GetKey(2), lb.Text.ToString());

                RadLabel lbl = (RadLabel)e.Item.FindControl("lblAddressCode");
                nvc.Set(nvc.GetKey(3), lbl.Text);
            }
            else if (Request.QueryString["mode"] == "custom")
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
    }

    protected void gvAddress_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }
    protected void gvAddress_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            RadLabel lbtn = (RadLabel)item.FindControl("lblEmail1");
            RadLabel lblAddressCode = (RadLabel)item.FindControl("lblAddressCode");
            UserControlToolTip uct = (UserControlToolTip)item.FindControl("ucEmailTT");
            LinkButton lnkAddressName = (LinkButton)item.FindControl("lnkAddressName");
            RadLabel lblNotApproved = (RadLabel)item.FindControl("lblNotApproved");
            DataRowView drv = (DataRowView)item.DataItem;
            if (lnkAddressName != null && drv["FLDCLASSOYN"].ToString() == "1" && drv["FLDPREFERREDBYPRINCIPALYN"].ToString() == "0")
            {
                lnkAddressName.Enabled = false;
                if (lblNotApproved != null)
                {
                    lblNotApproved.Text = drv["FLDEXCEPTION"].ToString();
                    lblNotApproved.Visible = true;
                }


            }


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
}
