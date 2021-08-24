using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Collections.Specialized;
using System.Collections;

public partial class CrewPlanDetailTravel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
            CrewTab.AccessRights = this.ViewState;
            CrewTab.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["creweventid"] = "";

                if (Request.QueryString["VESSELID"] != null)
                {
                    ViewState["Vesselid"] = Request.QueryString["VESSELID"].ToString();
                }

                if (Request.QueryString["creweventid"] != null)
                {
                    ViewState["creweventid"] = Request.QueryString["creweventid"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvCCPlan.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                foreach (GridItem item in gvCCPlan.MasterTableView.Items)
                {
                    if (item is GridEditableItem)
                    {
                        GridEditableItem editableItem = item as GridDataItem;
                        editableItem.Edit = true;
                    }
                }
                gvCCPlan.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewTab_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string detailid = ",";
                string strorigincity = ",";
                string strdestinationcity = ",";
                string strdeparturedateedit = ",";
                string departureampm = ",";
                string strarrivaldateedit = ",";
                string arrivalampm = ",";
                string paymenttype = ",";
                string purposeid = ",";

                foreach (GridDataItem item in gvCCPlan.Items)
                {
                  
                    if (!IsValidTravelRequest(
                            ((UserControlMultiColumnCity)item.FindControl("ucOriginCity")).SelectedValue,
                           ((UserControlMultiColumnCity)item.FindControl("ucDestinationCity")).SelectedValue,
                            ((UserControlDate)item.FindControl("txtDepartureDateEdit")).Text,
                            ((RadComboBox)item.FindControl("ddlampmdeparture")).SelectedValue,
                            ((UserControlHard)item.FindControl("ucPaymentmode")).SelectedHard,
                           ((UserControlTravelReason)item.FindControl("ucPurpose")).SelectedReason))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    detailid += ((RadLabel)item.FindControl("lblcreweventdetailid")).Text + ",";                  
                    strorigincity += ((UserControlMultiColumnCity)item.FindControl("ucOriginCity")).SelectedValue + ",";
                    strdestinationcity += ((UserControlMultiColumnCity)item.FindControl("ucDestinationCity")).SelectedValue + ",";
                    strdeparturedateedit += ((UserControlDate)item.FindControl("txtDepartureDateEdit")).Text + ",";
                    departureampm += ((RadComboBox)item.FindControl("ddlampmdeparture")).SelectedValue + ",";
                    strarrivaldateedit += ((UserControlDate)item.FindControl("txtArrivalDate")).Text + ",";
                    arrivalampm += ((RadComboBox)item.FindControl("ddlampmarrival")).SelectedValue + ",";
                    paymenttype += ((UserControlHard)item.FindControl("ucPaymentmode")).SelectedHard + ",";
                    purposeid += ((UserControlTravelReason)item.FindControl("ucPurpose")).SelectedReason + ",";

                }

                PhoenixCrewChangeEventDetail.CrewEventOnsignerTravelReq(detailid                            
                                   , strorigincity
                                   , strdestinationcity
                                   , strdeparturedateedit
                                   , strarrivaldateedit
                                   , departureampm
                                   , arrivalampm
                                   , purposeid
                                   , paymenttype
                                   );

                PhoenixCrewChangeEventDetail.CrewEventOnsignerTravelReqInsert(new Guid(ViewState["creweventid"].ToString()));

                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidTravelRequest(string strorigincity, string strdestinationcity, string strdeparturedateedit
                                     , string departureampm, string paymenttype, string purposeid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(strorigincity.Trim()))
            ucError.ErrorMessage = "Origin is required.";

        if (string.IsNullOrEmpty(strdestinationcity.Trim()))
            ucError.ErrorMessage = "Destination is required.";

        if (General.GetNullableDateTime(strdeparturedateedit) == null)
            ucError.ErrorMessage = "Departure date is required.";

        if (string.IsNullOrEmpty(departureampm.Trim()))
            ucError.ErrorMessage = "Departure Timing not selected";

        if (paymenttype.ToString().ToUpper() == "DUMMY" || string.IsNullOrEmpty(paymenttype.Trim()))
            ucError.ErrorMessage = "Payment mode is required.";

        if (purposeid.ToString().ToUpper() == "DUMMY" || string.IsNullOrEmpty(purposeid.Trim()))
            ucError.ErrorMessage = "Travel Purpose is required.";

        return (!ucError.IsError);
    }



    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            string Vesselid = (ViewState["Vesselid"] == null) ? null : (ViewState["Vesselid"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = new DataSet();

            NameValueCollection nvc = Filter.CurrentCrewPlanEventTravelReqFilter;

            if (nvc == null)
            {
                nvc = new NameValueCollection();
                nvc.Add("EventDetailList", string.Empty);
            }

            string strcrewplanlist = General.GetNullableString(nvc != null ? nvc.Get("EventDetailList") : string.Empty);


            ds = PhoenixCrewChangeEventDetail.CrewEventOnsignerTravelReqSearch(
                                                                  strcrewplanlist
                                                                  , sortexpression, sortdirection,
                                                                   int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                   gvCCPlan.PageSize,
                                                                   ref iRowCount,
                                                                   ref iTotalPageCount);


            gvCCPlan.DataSource = ds;
            gvCCPlan.VirtualItemCount = iRowCount;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCCPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCCPlan.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCCPlan_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void gvCCPlan_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;

            DataRowView drv = (DataRowView)e.Item.DataItem;
            UserControlMultiColumnCity ucDestinationCity = (UserControlMultiColumnCity)e.Item.FindControl("ucDestinationCity");
            if (ucDestinationCity != null)
            {
                ucDestinationCity.SelectedValue = "";
                ucDestinationCity.Text = "";
                ucDestinationCity.SelectedValue = drv["FLDDESTINATIONCITYID"].ToString();
                ucDestinationCity.Text = drv["FLDDESTINATIONCITYNAME"].ToString();
            }
            UserControlMultiColumnCity ucOriginCity = (UserControlMultiColumnCity)e.Item.FindControl("ucOriginCity");
            if (ucOriginCity != null)
            {
                ucOriginCity.SelectedValue = "";
                ucOriginCity.Text = "";
                ucOriginCity.SelectedValue = drv["FLDSEAFARERAIRPORTCITYID"].ToString();
                ucOriginCity.Text = drv["FLDSEAFARERAIRPORTCITYNAME"].ToString();
            }
        }
  
    }


}