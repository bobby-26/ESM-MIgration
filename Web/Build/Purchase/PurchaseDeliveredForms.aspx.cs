using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseDeliveredForms : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);

                cmdHiddenSubmit.Attributes["style"] = "display:none";
                VesselConfiguration();
                
                ViewState["deliveryid"] = null;
                ViewState["orderid"] = null;
                gvFormDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && Request.QueryString["deliveryid"] != null)
                {
                    PhoenixToolbar toolbargrid = new PhoenixToolbar();
                    toolbargrid.AddImageLink("javascript:openNewWindow('Filter','','"+Session["sitepath"]+"/Purchase/PurchaseFormOrdereList.aspx?deliveryid=" + Request.QueryString["deliveryid"].ToString() + "');return false;", "Add", "add.png", "");
                    MenuOrderLineItem.MenuList = toolbargrid.Show();
                }
                if (Request.QueryString["deliveryid"] != null)
                {
                    ViewState["deliveryid"] = Request.QueryString["deliveryid"].ToString();
                    //ifMoreInfo.Attributes["src"] = "PurchaseDeliveryFormsGeneral.aspx";   

                    DataSet ds = PhoenixPurchaseDelivery.Editdelivery(new Guid(ViewState["deliveryid"].ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        MenuDelivery.Title = "Forms [" + dr["FLDFORWARDERNAME"].ToString() + " - " + dr["FLDDELIVERYNUMBER"].ToString() + " - " + dr["FLDVESSELNAME"].ToString() + "]";
                    }

                }
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("General", "GENERAL");
                toolbar.AddButton("Forms", "FORMS");

                MenuDelivery.MenuList = toolbar.Show();
                MenuDelivery.SelectedMenuIndex = 1;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuDelivery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FORMS"))
            {
                //if (ViewState["orderid"] != null)
                //    ifMoreInfo.Attributes["src"] = "PurchaseDeliveryFormsGeneral.aspx?orderid=" + ViewState["orderid"].ToString();
                //else
                //    ifMoreInfo.Attributes["src"] = "PurchaseDeliveryFormsGeneral.aspx";

            }
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                if (ViewState["deliveryid"] != null)
                    Response.Redirect("../Purchase/PurchaseDeliveryDetail.aspx?deliveryid=" + ViewState["deliveryid"].ToString());
                else
                    Response.Redirect("../Purchase/PurchaseDeliveryDetail.aspx");

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


    private void BindData()
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFormDetails.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "FLDVENDORNAME", "FLDFORMTYPENAME", "FLDFORMSTATUSNAME", "FLDCURRENCY", "FLDBUDGETNAME" };
        string[] alCaptions = { "Number", "Form Title", "Vendor", "Form Type", "Form Status", "Currency", "Budget" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());


        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string deliveryid = ViewState["deliveryid"] != null ? ViewState["deliveryid"].ToString() : "";
        DataSet ds = new DataSet();

        ds = PhoenixPurchaseDelivery.DeliveryOrderSearch(General.GetNullableGuid(deliveryid),
               sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], iRowCount,
                   ref iRowCount, ref iTotalPageCount);

        gvFormDetails.DataSource = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["orderid"] == null)
            {
                ViewState["orderid"] = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                //gvFormDetails.SelectedIndex = 0;
                //ifMoreInfo.Attributes["src"] = "PurchaseDeliveryFormsGeneral.aspx?orderid=" + ViewState["orderid"].ToString();
            }

        }
        gvFormDetails.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }



    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            //gvFormDetails.SelectedIndex = -1;
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            gvFormDetails.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void gvFormDetails_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    if (ViewState["SORTEXPRESSION"] != null)
        //    {
        //        HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
        //        if (img != null)
        //        {
        //            if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
        //                img.Src = Session["images"] + "/arrowUp.png";
        //            else
        //                img.Src = Session["images"] + "/arrowDown.png";

        //            img.Visible = true;
        //        }
        //    }
        //}
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
                        db.Visible = false;
                }
            UserControlQuick quick = (UserControlQuick)e.Item.FindControl("ucDeliveryStatus");
            if(quick != null)
            {
                quick.QuickTypeCode = "183";
                quick.bind();
                quick.SelectedQuick = drv["FLDDELIVERYSTATUSID"].ToString();
            }
        }

    }

    protected void gvFormDetails_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPurchaseDelivery.DeliveryFormDelete(
                    new Guid(ViewState["deliveryid"].ToString())
                    , new Guid(((RadLabel)e.Item.FindControl("lblOrderID")).Text));
                gvFormDetails.DataSource = null;
                gvFormDetails.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("ROWCLICK") || e.CommandName.ToUpper().Equals("SELECT"))
            {
                gvFormDetails.SelectedIndexes.Add(e.Item.ItemIndex);
                Filter.CurrentPurchaseStockType = ((RadLabel)e.Item.FindControl("lblStockType")).Text;
                BindPageURL(e.Item.ItemIndex);
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixPurchaseDelivery.DeliveryFormUpdate(
                    new Guid(ViewState["deliveryid"].ToString())
                    , new Guid(((RadLabel)e.Item.FindControl("lblOrderID")).Text)
                    , General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucDeliveryStatus")).SelectedQuick)
                    , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtDeliveryDate")).Text));
                gvFormDetails.DataSource = null;
                gvFormDetails.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvFormDetails.Rebind();

    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvFormDetails.Items[rowindex];
            ViewState["orderid"] = ((RadLabel)item.FindControl("lblOrderID")).Text;
            ViewState["DTKEY"] = ((RadLabel)item.FindControl("lbldtkey")).Text;
            Filter.CurrentPurchaseVesselSelection = int.Parse(((RadLabel)item.FindControl("lblVesselID")).Text);
            PhoenixPurchaseOrderForm.FormNumber = ((LinkButton)item.FindControl("lnkFormNumberName")).Text;
            //ifMoreInfo.Attributes["src"] = "PurchaseDeliveryFormsGeneral.aspx?orderid=" + ViewState["orderid"];
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixCommonPurchase.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }


    protected void gvFormDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
