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
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;
using System.Text;

public partial class PurchaseMultipleVendorSelect : PhoenixBasePage
{
	string strvslPricipal = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuAddress.MenuList = toolbarmain.Show();
               

        if (!IsPostBack)
        {
            //cblProductType.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            //                                                        Convert.ToInt32(PhoenixQuickTypeCode.GENERALPRODUCTTYPE));
            //cblProductType.DataTextField = "FLDQUICKNAME";
            //cblProductType.DataValueField = "FLDQUICKCODE";
            //cblProductType.DataBind();
            gvAddress.PageSize = General.ShowRecords(null);
            if ((Request.QueryString["addresstype"] != null) && (Request.QueryString["addresstype"] != ""))
                ViewState["addresstype"] = "," + Request.QueryString["addresstype"].ToString() + ",";
            //else
            //    Response.Redirect("../PhoenixUnderConstruction.aspx");

            if (Request.QueryString["orderid"] != null)
            {
                ViewState["orderid"] = Request.QueryString["orderid"].ToString();
            } 

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

			BindVesselPrincipal();

			if (General.GetNullableInteger(hdnprincipalId.Value.ToString()) != null && PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() == "OWNER")
			{
				strvslPricipal = hdnprincipalId.Value.ToString();
				ucPrincipal.SelectedAddress = hdnprincipalId.Value.ToString();
				ucPrincipal.Enabled = false;
			}
		}

    }
    
    protected void MenuAddress_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += " closeTelerikWindow('codehelp1','detail','true');";
            Script += "</script>" + "\n";

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                NameValueCollection addresscriteria = new NameValueCollection();
                addresscriteria.Clear();
                addresscriteria.Add("txtNameSearch", txtNameSearch.Text);
                addresscriteria.Add("txtCode", txtCode.Text);
                addresscriteria.Add("txtCountryNameSearch", "");

                Filter.CurrentCommonAddressFilterCriteria = addresscriteria;

                StringBuilder strproducttype = new StringBuilder();

                if (strproducttype.Length > 1)
                {
                    strproducttype.Remove(strproducttype.Length - 1, 1);
                }

                ViewState["producttype"] = strproducttype.ToString();
                gvAddress.Rebind();
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string strAddress = "";
                if (ViewState["ADDRESS_CHECKED_ITEMS"] != null)
                {
                    ArrayList SelectedAddress = new ArrayList();
                    SelectedAddress = (ArrayList)ViewState["ADDRESS_CHECKED_ITEMS"];
                    if (SelectedAddress != null && SelectedAddress.Count > 0)
                    {
                        foreach (Int64 index in SelectedAddress)
                        {
                            strAddress += index.ToString() + ",";
                        }
                    }
                }

                if (strAddress != "")
                    strAddress = "," + strAddress;

                PhoenixPurchaseQuotation.QuotaitionInsertBulk(new Guid(ViewState["orderid"].ToString()), strAddress);
                gvAddress.Rebind();

                RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
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
                    gvAddress.CurrentPageIndex + 1,
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
    protected void gvAddress_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("RELATION"))
        {
            RadLabel lblAddressCode = (RadLabel)e.Item.FindControl("lblAddressCode");
            Response.Redirect("../Common/CommonPickListAddressRelation.aspx?addresscode=" + lblAddressCode.Text + "&addressscreen=ADDRESS&addresstype=" + Request.QueryString["addresstype"]);
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
            RadLabel lnkAddressName = (RadLabel)item.FindControl("lnkAddressName");
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
    protected void SaveCheckedAddressValues(Object sender, EventArgs e)
    {
        ArrayList SelectedAddress = new ArrayList();
        Int64 index;
        foreach (GridDataItem item in gvAddress.Items)
        {
            bool result = false;
            index = Int64.Parse(item.GetDataKeyValue("FLDADDRESSCODE").ToString());

            if (((RadCheckBox)(item.FindControl("chkVendorSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkDocumentSelect")).Checked;
            }

            // Check in the ViewState
            if (ViewState["ADDRESS_CHECKED_ITEMS"] != null)
                SelectedAddress = (ArrayList)ViewState["ADDRESS_CHECKED_ITEMS"];
            if (result)
            {
                if (!SelectedAddress.Contains(index))
                    SelectedAddress.Add(index);
            }
            else
                SelectedAddress.Remove(index);
            
        }
        if (SelectedAddress != null && SelectedAddress.Count > 0)
            ViewState["ADDRESS_CHECKED_ITEMS"] = SelectedAddress;
    }
    private void BindVesselPrincipal()
	{
		if (Filter.CurrentPurchaseVesselSelection > 0)
		{
			DataSet ds = PhoenixRegistersVessel.EditVessel(Filter.CurrentPurchaseVesselSelection);
			hdnprincipalId.Value = ds.Tables[0].Rows[0]["FLDPRINCIPAL"].ToString();
		}


	}
}
