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
public partial class CrewTravelVisaRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);


            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                lblBookingId.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = null;

                gvTravelVisa.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelVisaRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTravelVisa')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelVisaFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelVisaRequest.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelVisaLineItem.aspx", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MenuTravelVisa.AccessRights = this.ViewState;
            MenuTravelVisa.MenuList = toolbargrid.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuTravelVisa_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvTravelVisa.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["TRAVELVISAID"] = null;
                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentTravelVisaFilterCriteria = null;
                gvTravelVisa.CurrentPageIndex = 0;
                BindData();
                gvTravelVisa.Rebind();
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
            string[] alColumns = { "FLDREFERENCENO", "FLDVESSELNAME", "FLDAGENTNAME", "FLDREMARKS", "FLDVISADATE" };
            string[] alCaptions = { "Number", "Vessel", "Agent", "Remarks", "Visa Date" };

            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentTravelVisaFilterCriteria;

            ds = PhoenixCrewTravelVisa.SearchCrewTravelVisa(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                 , nvc != null ? nvc.Get("txtReferenceNo") : null
                 , General.GetNullableInteger(nvc != null ? nvc["ucVessel"] : string.Empty)
                 , General.GetNullableInteger(nvc != null ? nvc["ucCountry"] : string.Empty)
                 , General.GetNullableInteger(nvc != null ? nvc["txtAgentId"] : string.Empty)
                 , null, null
                 , sortexpression, sortdirection, 1, gvTravelVisa.PageSize, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Visa Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
            string[] alColumns = { "FLDREFERENCENO", "FLDVESSELNAME", "FLDAGENTNAME", "FLDREMARKS", "FLDVISADATE" };
            string[] alCaptions = { "Number", "Vessel", "Agent", "Remarks", "Visa Date" };
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


            NameValueCollection nvc = Filter.CurrentTravelVisaFilterCriteria;

            ds = PhoenixCrewTravelVisa.SearchCrewTravelVisa(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                 , nvc != null ? nvc.Get("txtReferenceNo") : null
                 , General.GetNullableInteger(nvc != null ? nvc["ucVessel"] : string.Empty)
                 , General.GetNullableInteger(nvc != null ? nvc["ucCountry"] : string.Empty)
                 , General.GetNullableInteger(nvc != null ? nvc["txtAgentId"] : string.Empty)
                 , null, null
                 , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvTravelVisa.PageSize, ref iRowCount, ref iTotalPageCount);


            gvTravelVisa.DataSource = ds;
            gvTravelVisa.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];


                if (ViewState["TRAVELVISAID"] == null)
                {
                    ViewState["TRAVELVISAID"] = ds.Tables[0].Rows[0]["FLDTRAVELVISAID"].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                    lblBookingId.Text = ViewState["TRAVELVISAID"].ToString();

                    if (ViewState["TRAVELVISAID"] != null)
                        ViewState["PAGEURL"] = "../Crew/CrewTravelVisaLineItem.aspx";
                }
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            General.SetPrintOptions("gvTravelVisa", "Visa Request", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvTravelVisa_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelVisa.CurrentPageIndex + 1;
        BindData();
    }


    protected void gvTravelVisa_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                string travelvisaid = ((RadLabel)e.Item.FindControl("lblTravelVisaId")).Text;
                string ReferenceNo = ((RadLabel)e.Item.FindControl("lblReferenceNo")).Text;

                ViewState["TRAVELVISAID"] = travelvisaid;

                Response.Redirect("CrewTravelVisaLineItem.aspx?travelvisaid=" + travelvisaid, false);

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

    protected void gvTravelVisa_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("uclblRemarksTT");
            if (lbtn != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lbtn.ClientID;
            }        
        }

    }



    protected void gvTravelVisa_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
               && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                Label lbtn = (Label)e.Row.FindControl("lblAgentName");
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucAgentNameTT");

            }

            LinkButton lnkReferenceNumberName = (LinkButton)e.Row.FindControl("lnkReferenceNumberName");

            Label referenceno = (Label)e.Row.FindControl("lblReferenceNumber");

        }


    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvTravelVisa.Rebind();
    }



}
