using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Text;

public partial class CrewListAddressAgent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "ADDVENDOR", ToolBarDirection.Right);
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuAddress.AccessRights = this.ViewState;
        MenuAddress.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            cblProductType.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                  Convert.ToInt32(PhoenixQuickTypeCode.GENERALPRODUCTTYPE));
            cblProductType.DataBindings.DataTextField = "FLDQUICKNAME";
            cblProductType.DataBindings.DataValueField = "FLDQUICKCODE";
            cblProductType.DataBind();

            if ((Request.QueryString["producttype"] != null) && (Request.QueryString["productype"] != ""))
            {
                ViewState["producttype"] = "," + Request.QueryString["producttype"].ToString() + ",";

                string[] producttype = ViewState["producttype"].ToString().Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in producttype)
                {
                    if (item.Trim() != "")
                    {
                        cblProductType.SelectedValue = item;
                        cblProductType.Items[0].Selected = true;    //for telerik control
                    }
                }
            }

            if (Request.QueryString["travelid"] != null && Request.QueryString["travelid"] != "")
            {
                foreach (ButtonListItem li in cblProductType.Items)
                {
                    li.Selected = (li.Text == "TRAVEL AGENCY") ? true : false;
                }

            }

             if (Request.QueryString["framename"] != null)
            {
                ViewState["framename"] = Request.QueryString["framename"].ToString();
            }
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvAddress.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
                BindData();
                gvAddress.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList();";
                Script += "</script>" + "\n";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
            }

            else if (CommandName.ToUpper().Equals("ADDVENDOR"))
            {
                if (Request.QueryString["Neededid"] != null && Request.QueryString["Neededid"] != "")
                {
                    foreach (GridDataItem gvr in gvAddress.Items)
                    {
                        if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)(gvr.FindControl("chkSelect"))).Checked == true)
                        {
                            RadLabel lbl = (RadLabel)(gvr.FindControl("lblAddressCode"));
                            PhoenixCrewWorkingGearQuotation.InsertSupplier(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , new Guid(Request.QueryString["Neededid"].ToString()), Convert.ToInt32(lbl.Text));
                        }
                    }
                }
                else
                {
                    foreach (GridDataItem gvr in gvAddress.Items)
                    {
                        if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)(gvr.FindControl("chkSelect"))).Checked == true)
                        {
                            RadLabel lbl = (RadLabel)(gvr.FindControl("lblAddressCode"));
                            PhoenixCrewTravelQuote.InsertCrewTravelAgent(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , new Guid(Request.QueryString["travelid"].ToString()), Convert.ToInt32(lbl.Text));
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1','',true);", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

            string addresstype = "135";
            if (Request.QueryString["Neededid"] != null && Request.QueryString["Neededid"] != "")
            {
                addresstype = "130,131,132";
            }

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

            ViewState["producttype"] = General.GetNullableInteger(strproducttype.ToString());

            string producttype = null;
            if (ViewState["addresstype"] != null)
                addresstype = ViewState["addresstype"].ToString();
            if (ViewState["producttype"] != null)
                producttype = ViewState["producttype"].ToString();

            ds = PhoenixCommonRegisters.AddressSearch(txtCode.Text, txtNameSearch.Text
                            , null, null, null, General.GetNullableString(txtCountryNameSearch.Text)
                            , addresstype
                            , producttype
                            , null, null, null, null, null, sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            gvAddress.PageSize,
                            ref iRowCount,
                            ref iTotalPageCount);


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


    protected void gvAddress_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblAddressCode");
                if (Request.QueryString["Neededid"] != null && Request.QueryString["Neededid"] != "")
                {
                    PhoenixCrewWorkingGearQuotation.InsertSupplier(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(Request.QueryString["Neededid"].ToString())
                        , Convert.ToInt32(lbl.Text));
                }
                else
                {
                    PhoenixCrewTravelQuote.InsertCrewTravelAgent(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(Request.QueryString["travelid"].ToString())
                        , Convert.ToInt32(lbl.Text));
                }


                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1','',true);", true);
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
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
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAddress.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvAddress_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    
}
