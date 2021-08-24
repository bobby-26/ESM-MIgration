using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Telerik.Web.UI;

public partial class PurchaseFormOrdereList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                gvFormDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                PhoenixToolbar toolbarmain = new PhoenixToolbar();

                toolbarmain.AddButton("Close", "CLOSE",ToolBarDirection.Right);
                toolbarmain.AddButton("Save", "DELIVERY", ToolBarDirection.Right);
                toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
                
                
                MenuOrderFormMain.AccessRights = this.ViewState; 
                MenuOrderFormMain.MenuList = toolbarmain.Show();
                //MenuOrderFormMain.SetTrigger(pnlOrderForm);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["orderid"] = null;
                ViewState["PAGEURL"] = null;
                ViewState["orderidlist"] = null;
                if (Request.QueryString["deliveryid"] != null)
                {
                    ViewState["deliveryid"] = Request.QueryString["deliveryid"].ToString();                    
                }               
            }        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
     
    protected void gvFormDetails_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        gvFormDetails.Rebind();
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList();";
                Script += "</script>" + "\n";
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
            }
            else if (CommandName.ToUpper().Equals("DELIVERY"))
            {
                CheckCheckedList();
                if ((ViewState["orderidlist"] != null) && (ViewState["orderidlist"].ToString().Length > 1))
                {
                    PhoenixPurchaseDelivery.UpdateOrderdelivery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,new Guid(ViewState["deliveryid"].ToString())  , ViewState["orderidlist"].ToString());
                    ViewState["orderidlist"] = null;
                    gvFormDetails.Rebind();
                }
                else
                {
                    ucError.ErrorMessage = "No order for delivered, Please select order in the list.";
                    ucError.Visible = true;
                }
                                
            }
            else if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvFormDetails.Rebind();
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
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFormDetails.CurrentPageIndex + 1;
        int iRowCount = 10;
        int iTotalPageCount =10;
        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "FLDVENDORNAME", "FLDFORMTYPENAME", "FLDFORMSTATUSNAME", "FLDCURRENCY", "FLDBUDGETNAME" };
        string[] alCaptions = { "Number", "Form Title", "Vendor", "Form Type", "Form Status", "Currency", "Budget" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = new DataSet();
        ds = PhoenixPurchaseOrderForm.OrderFormSearch(int.Parse(Filter.CurrentPurchaseVesselSelection.ToString())
                        ,General.GetNullableString(ddlStockType.SelectedValue) 
                        ,General.GetNullableInteger(ucAddress.SelectedAddress)
                        ,General.GetNullableInteger(ucAddressForwarder.SelectedAddress )
                        ,sortexpression
                        ,sortdirection
                        ,(int)ViewState["PAGENUMBER"]
                        , gvFormDetails.PageSize
                        , ref iRowCount
                        ,ref iTotalPageCount
                        ,General.GetNullableString(txtFormNo.Text)
                        , General.GetNullableString(txtFormTitle.Text));
        gvFormDetails.DataSource = ds;
        gvFormDetails.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
           
    }

 

 
    protected void gvFormDetails_ItemDataBound(Object sender, GridItemEventArgs e)
    {
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }      
      
    }

   
    private void CheckCheckedList()
    {
        string selectedordered = ",";
        if ((ViewState["orderidlist"] != null) && ViewState["orderidlist"].ToString().Length > 1)
            selectedordered = "";
        // Set the value here
        foreach (GridDataItem item in gvFormDetails.MasterTableView.Items)
        {
            RadCheckBox chk = (RadCheckBox)item.FindControl("chkSelect");
            if(chk!=null && chk.Checked == true)
            {
                selectedordered = selectedordered + ((RadLabel)(item.FindControl("lblNumber"))).Text + ",";
            }          
        }

        if (selectedordered.Length > 1)
        {
            if( ViewState["orderidlist"]==null)
                ViewState["orderidlist"] = selectedordered;
            else
                ViewState["orderidlist"] = ViewState["orderidlist"].ToString() + selectedordered;
        }
    }
    
    protected void gvFormDetails_RowCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void SaveCheckedForms(object sender, EventArgs e)
    {
        //CheckCheckedList();
    }
    protected void ddlStockType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvFormDetails.Rebind();
    }
    protected void ucAddressForwarder_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvFormDetails.Rebind();
    }
    protected void ucAddress_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvFormDetails.Rebind();
    }

    protected void gvFormDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
