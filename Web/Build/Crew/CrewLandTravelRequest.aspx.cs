using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;
public partial class CrewLandTravelRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewLandTravelRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvLandTravelRequest')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewLandTravelFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewLandTravelRequest.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewLandTravelRequestGeneral.aspx", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MenuLandTravel.AccessRights = this.ViewState;
            MenuLandTravel.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                lblRequestId.Attributes.Add("style", "display:none");
            
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;              
                ViewState["PAGEURL"] = null;                
                gvLandTravelRequest.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void MenuLandTravel_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvLandTravelRequest.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["requestid"] = null;
                ViewState["ROWCOUNT"] = null;
                ViewState["PAGENUMBER"] = 1;

                Filter.CurrentLandTravelFilterCriteria = null;
                gvLandTravelRequest.CurrentPageIndex = 0;
                BindData();
                gvLandTravelRequest.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDREFERENCENO", "FLDCITYNAME", "FLDFROMPLACE", "FLDTOPLACE", "FLDREQUESTEDDATE", "FLDTRAVELDATE", "FLDSTATUSNAME" };
            string[] alCaptions = { "Number", "City", "From Place", "To Place", "Requested Date", "Travel Date", "Status" };
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentLandTravelFilterCriteria;

            ds = PhoenixCrewLandTravelRequest.LandTravelRequestSearch(
                     nvc != null ? nvc.Get("txtReferenceNo") : null
                 , General.GetNullableInteger(nvc != null ? nvc.Get("txtcityid") : string.Empty)
                 , General.GetNullableDateTime(nvc != null ? nvc.Get("ucFromDate") : string.Empty)
                 , General.GetNullableDateTime(nvc != null ? nvc.Get("ucToDate") : string.Empty)
                 , General.GetNullableDateTime(nvc != null ? nvc.Get("ucReqFromDate") : string.Empty)
                 , General.GetNullableDateTime(nvc != null ? nvc.Get("ucReqToDate") : string.Empty)
                 , General.GetNullableInteger(nvc != null ? nvc.Get("ucTypeOfDuty") : string.Empty)
                 , General.GetNullableString(nvc != null ? nvc.Get("ddlStatus") : string.Empty)
                 , sortexpression, sortdirection
                 , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                 , gvLandTravelRequest.PageSize,
                 ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Land Travel", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDREFERENCENO", "FLDCITYNAME", "FLDFROMPLACE", "FLDTOPLACE", "FLDREQUESTEDDATE", "FLDTRAVELDATE", "FLDSTATUSNAME" };
            string[] alCaptions = { "Number", "City", "From Place", "To Place", "Requested Date", "Travel Date", "Status" };
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentLandTravelFilterCriteria;

            ds = PhoenixCrewLandTravelRequest.LandTravelRequestSearch(
                     nvc != null ? nvc.Get("txtReferenceNo") : null
                 , General.GetNullableInteger(nvc != null ? nvc.Get("txtcityid") : string.Empty)
                 , General.GetNullableDateTime(nvc != null ? nvc.Get("ucFromDate") : string.Empty)
                 , General.GetNullableDateTime(nvc != null ? nvc.Get("ucToDate") : string.Empty)
                 , General.GetNullableDateTime(nvc != null ? nvc.Get("ucReqFromDate") : string.Empty)
                 , General.GetNullableDateTime(nvc != null ? nvc.Get("ucReqToDate") : string.Empty)
                 , General.GetNullableInteger(nvc != null ? nvc.Get("ucTypeOfDuty") : string.Empty)
                 , General.GetNullableString(nvc != null ? nvc.Get("ddlStatus") : string.Empty)
                 , sortexpression, sortdirection
                 , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                 , gvLandTravelRequest.PageSize,
                 ref iRowCount, ref iTotalPageCount);

            gvLandTravelRequest.DataSource = ds;
            gvLandTravelRequest.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                
                if (ViewState["requestid"] == null)
                {
                    ViewState["requestid"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();                   
                    lblRequestId.Text = ViewState["requestid"].ToString();
                  
                    if (ViewState["requestid"] != null)
                        ViewState["PAGEURL"] = "../Crew/CrewHotelBookingGeneral.aspx";
                }
            }          
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            General.SetPrintOptions("gvLandTravelRequest", "Land Travel", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

        BindData();
        gvLandTravelRequest.Rebind();
    }
   
    protected void gvLandTravelRequest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLandTravelRequest.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvLandTravelRequest_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                string requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                string ReferenceNo = ((RadLabel)e.Item.FindControl("lblReferenceNo")).Text;
                
                ViewState["requestid"] = requestid;

                Response.Redirect("CrewLandTravelRequestGeneral.aspx?requestid=" + requestid, false);
                
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

    protected void gvLandTravelRequest_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
}
