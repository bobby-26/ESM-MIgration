using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewHotelBookingTravelHopList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Add", "ADD", ToolBarDirection.Right);
            toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
            MenuMainHopList.AccessRights = this.ViewState;
            MenuMainHopList.MenuList = toolbarmain.Show();
            
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CITYNAME"] = null;
                ViewState["VESSELID"] = null;
                ViewState["CITYID"] = null;
                ViewState["BOOKINGID"] = null;

                if (Request.QueryString["cityid"] != null)
                {
                    ViewState["BOOKINGID"] = Request.QueryString["bookingid"].ToString();
                }
                if (Request.QueryString["vesselid"] != null)
                {
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
                }
                if (Request.QueryString["cityid"] != null)
                {
                    ViewState["CITYID"] = Request.QueryString["cityid"].ToString();
                }

                txtFromDate.Text = DateTime.Now.ToString();

                gvHopList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    protected void MenuMainHopList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper() == "SEARCH")
            {
                BindData();
                gvHopList.Rebind();
            }
            if (CommandName.ToUpper() == "ADD")
            {
                StringBuilder strHopLineItemIdList = new StringBuilder();
                StringBuilder strRequestIdList = new StringBuilder();

                foreach (GridDataItem gvr in gvHopList.Items)
                {
                    RadCheckBox chkCheck = (RadCheckBox)gvr.FindControl("chkCheck");

                    if (chkCheck != null)
                    {
                        if (chkCheck.Checked == true && chkCheck.Enabled == true)
                        {
                            RadLabel lblHopId = (RadLabel)gvr.FindControl("lblHopLineItemId");
                            RadLabel lblRequestId = (RadLabel)gvr.FindControl("lblRequestId");

                            strHopLineItemIdList.Append(lblHopId.Text);
                            strHopLineItemIdList.Append(",");

                            strRequestIdList.Append(lblRequestId.Text);
                            strRequestIdList.Append(",");
                        }
                    }
                }
                if (strHopLineItemIdList.Length > 1)
                {
                    strHopLineItemIdList.Remove(strHopLineItemIdList.Length - 1, 1);
                }

                if (strRequestIdList.Length > 1)
                {
                    strRequestIdList.Remove(strRequestIdList.Length - 1, 1);
                }
                if (!IsValidHotelGuestRequest(strHopLineItemIdList.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {

                    if (ViewState["BOOKINGID"] != null)
                    {
                        PhoenixCrewHotelBookingGuests.InsertHotelBookingGuests(
                           General.GetNullableGuid(ViewState["BOOKINGID"].ToString())
                         , strHopLineItemIdList.ToString()
                         , strRequestIdList.ToString()
                        );
                    }
                    string Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += " fnReloadList();";
                    Script += "</script>" + "\n";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
                }
                //BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvHopList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvHopList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvHopList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;

            DataSet ds = new DataSet();


            ds = PhoenixCrewHotelBookingGuests.CrewTravelHopListSearch(
                General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : null)
                , General.GetNullableInteger(ViewState["CITYID"] != null ? ViewState["CITYID"].ToString() : null)
                , General.GetNullableDateTime(txtFromDate.Text), General.GetNullableDateTime(txtToDate.Text)
                , sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                gvHopList.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            gvHopList.DataSource = ds;
            gvHopList.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ViewState["CITYID"] = ds.Tables[0].Rows[0]["FLDDESTINATIONID"].ToString();             
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
  
    private bool IsValidHotelGuestRequest(string HopLineItemIdList)
    {
        if (HopLineItemIdList.Trim() == "")
        {
            ucError.ErrorMessage = "Please Select Atleast One Employee";
        }
        return (!ucError.IsError);
    }
    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvHopList.Rebind();
    }
   
    protected void gvHopList_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

}
