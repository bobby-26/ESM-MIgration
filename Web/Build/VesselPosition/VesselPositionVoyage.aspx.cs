using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using System.Web;
using Telerik.Web.UI;

public partial class VesselPositionVoyage : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarvoyagelist = new PhoenixToolbar();
            toolbarvoyagelist.AddFontAwesomeButton("../VesselPosition/VesselPositionVoyage.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbarvoyagelist.AddFontAwesomeButton("javascript:CallPrint('gvVoyage')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarvoyagelist.AddFontAwesomeButton("../VesselPosition/VesselPositionVoyage.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbarvoyagelist.AddFontAwesomeButton("../VesselPosition/VesselPositionVoyage.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                toolbarvoyagelist.AddFontAwesomeButton("../VesselPosition/VesselPositionVoyageData.aspx?mode=NEW", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDVOYAGE");

            MenuVoyageList.AccessRights = this.ViewState;
            MenuVoyageList.MenuList = toolbarvoyagelist.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["FLEETID"] = "";
                BindVesselFleetList();
                UcVessel.bind();
                UcVessel.DataBind();

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    SetFeet();
                    if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToString().ToUpper() != "OWNER")
                    {
                        UcVessel.Enabled = false;
                        ddlFleet.Enabled = false;
                    }
                }

                if (Filter.CurrentVPRSVoyageFilter != null)
                {
                    NameValueCollection nvc = Filter.CurrentVPRSVoyageFilter;

                    txtCharterer.Text = nvc.Get("txtCharterer").ToString();
                    UcVessel.SelectedVessel = nvc.Get("UcVessel") != null ? nvc.Get("UcVessel") : "";
                    ViewState["PAGENUMBER"] = General.GetNullableInteger(nvc.Get("PAGENUMBER").ToString());
                    ViewState["SORTEXPRESSION"] = nvc.Get("SORTEXPRESSION").ToString();
                    ViewState["SORTDIRECTION"] = General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString());
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();

                    criteria.Clear();
                    criteria.Add("UcVessel", PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : (Request.QueryString["vesselid"] != null? Request.QueryString["vesselid"]:string.Empty));
                    criteria.Add("ddlFleet", ViewState["FLEETID"].ToString());
                    criteria.Add("txtCharterer", txtCharterer.Text);
                    criteria.Add("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
                    criteria.Add("SORTEXPRESSION", ViewState["SORTEXPRESSION"] == null ? "" : ViewState["SORTEXPRESSION"].ToString());
                    criteria.Add("SORTDIRECTION", ViewState["SORTDIRECTION"] == null ? "" : ViewState["SORTDIRECTION"].ToString());

                    Filter.CurrentVPRSVoyageFilter = criteria;
                }
                gvVoyage.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            if (IsPostBack)
            {
                NameValueCollection criteria = new NameValueCollection();

                criteria.Clear();
                string fleet = null;
                DataSet ds;
                if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
                {
                    ds = PhoenixRegistersVessel.EditVesselCrewRelatedInfo(Convert.ToInt32(UcVessel.SelectedVessel));
                    fleet = ds.Tables[0].Rows[0]["FLDTECHFLEET"].ToString();
                }
                criteria.Add("UcVessel", UcVessel.SelectedVessel);
                criteria.Add("ddlFleet", fleet);
                criteria.Add("txtCharterer", txtCharterer.Text);
                criteria.Add("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
                criteria.Add("SORTEXPRESSION", ViewState["SORTEXPRESSION"] == null ? "" : ViewState["SORTEXPRESSION"].ToString());
                criteria.Add("SORTDIRECTION", ViewState["SORTDIRECTION"] == null ? "" : ViewState["SORTDIRECTION"].ToString());

                Filter.CurrentVPRSVoyageFilter = criteria;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvVoyage.SelectedIndexes.Clear();
        gvVoyage.EditIndexes.Clear();
        gvVoyage.DataSource = null;
        gvVoyage.Rebind();
    }
    protected void gvVoyage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVoyage.CurrentPageIndex + 1;
        BindData();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
        //SetPageNavigator();
    }
    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDVOYAGENO", "FLDCOMMENCEDDATETIME", "FLDCOMMENCEDPORTANME", "FLDCOMPLETEDDATE", "FLDCOMPLETEDPORTNAME", "FLDCPFUELOILCONS", "FLDCPDIESELOILCONS" };
        string[] alCaptions = { "Vessel", "Voyage No.", "Commenced Date", "Commenced Port", "Completed Date", "Completed Port", "CP FO Cons", "CP DO Cons" };
               
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int? vesselid = null;

        NameValueCollection nvc = Filter.CurrentVPRSVoyageFilter;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        else
            vesselid = General.GetNullableInteger(nvc.Get("UcVessel").ToString());

        ds = PhoenixVesselPositionVoyageData.VoyageDataSearch(
            General.GetNullableInteger(UcVessel.SelectedVessel), General.GetNullableInteger(ViewState["FLEETID"].ToString()), null, txtCharterer.Text,
            "", nvc.Get("SORTEXPRESSION").ToString(),
            General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString()),
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"Voyage.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Voyage</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");

        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
   
    protected void VoyageList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }

        if (CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
    }

    private void ClearFilter()
    {
        UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : "";
        ddlFleet.SelectedValue = ViewState["FLEETID"].ToString() != "" ? ViewState["FLEETID"].ToString() : "Dummy";
        
        txtCharterer.Text = "";

        NameValueCollection criteria = new NameValueCollection();

        criteria.Clear();
        criteria.Add("UcVessel", UcVessel.SelectedVessel);
        criteria.Add("txtCharterer", "");
        criteria.Add("PAGENUMBER", "1");
        criteria.Add("SORTEXPRESSION", "");
        criteria.Add("SORTDIRECTION", "");
        criteria.Add("ddlFleet", ddlFleet.SelectedValue);

        Filter.CurrentVPRSVoyageFilter = criteria;

        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        SetFeet();
        Rebind();
    }
    private void SetFeet()
    {
        if (General.GetNullableInteger(UcVessel.SelectedVessel) != null)
        {
            DataSet ds = PhoenixRegistersVessel.EditVesselCrewRelatedInfo(Convert.ToInt32(UcVessel.SelectedVessel));
            DataTable dt = ds.Tables[0];
            DataRow dr = dt.Rows[0];
            ddlFleet.SelectedValue = dr["FLDTECHFLEET"].ToString();
            ViewState["FLEETID"] = dr["FLDTECHFLEET"].ToString();
        }
        else
        {
            ddlFleet.SelectedValue = "Dummy";
            ViewState["FLEETID"] = "";
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVoyage.CurrentPageIndex + 1;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
       
        if (ViewState["SORTDIRECTION"] != null)
           sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        int? vesselid = null;

        NameValueCollection nvc = Filter.CurrentVPRSVoyageFilter;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        else
            vesselid = General.GetNullableInteger(nvc.Get("UcVessel").ToString());

        ds = PhoenixVesselPositionVoyageData.VoyageDataSearch(
            General.GetNullableInteger(UcVessel.SelectedVessel), General.GetNullableInteger(ViewState["FLEETID"].ToString()), null, txtCharterer.Text,
            "", nvc.Get("SORTEXPRESSION").ToString(),
            General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString()),
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvVoyage.PageSize, ref iRowCount, ref iTotalPageCount);

        string[] alColumns = { "FLDVESSELNAME", "FLDVOYAGENO", "FLDCOMMENCEDDATETIME", "FLDCOMMENCEDPORTANME", "FLDCOMPLETEDDATE", "FLDCOMPLETEDPORTNAME", "FLDCPFUELOILCONS", "FLDCPDIESELOILCONS" };
        string[] alCaptions = { "Vessel", "Voyage No.", "Commenced Date", "Commenced Port", "Completed Date", "Completed Port","CP FO Cons", "CP DO Cons" };
               
        General.SetPrintOptions("gvVoyage", "Voyage", alCaptions, alColumns, ds);

        gvVoyage.DataSource = ds;
        gvVoyage.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvVoyage_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }

        Filter.CurrentVPRSVoyageFilter.Set("SORTEXPRESSION", ViewState["SORTEXPRESSION"].ToString());
        Filter.CurrentVPRSVoyageFilter.Set("SORTDIRECTION", ViewState["SORTDIRECTION"].ToString());

        Rebind();
    }

    protected void gvVoyage_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper() == "EDIT")
            {
                Filter.CurrentVPRSVoyageSelection = ((RadLabel)e.Item.FindControl("lblVoyageId")).Text;

                Response.Redirect("VesselPositionVoyageData.aspx", false);
            }
            else if(e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionVoyageData.DeleteVoyageData(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblVoyageId")).Text));

                Rebind();
            }
            else if(e.CommandName.ToUpper().Equals("POPULATEVOYAGE"))
            {
                DataSet ds =   PhoenixVesselPositionVoyageData.GetArrivalReportByVoyage(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblVoyageId")).Text));
                if(ds.Tables[0].Rows.Count>0)
                {
                    foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        PhoenixVesselPositionArrivalReport.PassageSummaryInsert((PhoenixSecurityContext.CurrentSecurityContext.UserCode), General.GetNullableInteger(dr["FLDVESSELID"].ToString()), General.GetNullableGuid(dr["FLDVESSELARRIVALID"].ToString()));
                    }
                }

                ucStatus.Text = "Passage Summary Calculated Successfully";
            }
            else if (e.CommandName == "Page")
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

    protected void gvVoyage_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            LinkButton cmdpassage = (LinkButton)e.Item.FindControl("cmdPopulateVoyage");
            if (cmdpassage != null)
            {
                if (cmdpassage != null) cmdpassage.Visible = PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 ? SessionUtil.CanAccess(this.ViewState, cmdpassage.CommandName) : false;
            }
        }
        
    }


    private void BindVesselFleetList()
    {
        DataSet ds = PhoenixRegistersFleet.ListFleet();

        ddlFleet.DataSource = ds;
        ddlFleet.DataTextField = "FLDFLEETDESCRIPTION";
        ddlFleet.DataValueField = "FLDFLEETID";
        ddlFleet.DataBind();
        ddlFleet.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    
}
