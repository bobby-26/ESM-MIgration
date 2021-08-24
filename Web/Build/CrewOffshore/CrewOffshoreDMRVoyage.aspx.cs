using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;
using System.Web.UI;
public partial class CrewOffshoreDMRVoyage : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarvoyagelist = new PhoenixToolbar();
 
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();
                toolbarvoyagetap.AddButton("List", "VOYAGE");

                MenuVoyageTap.AccessRights = this.ViewState;
                MenuVoyageTap.MenuList = toolbarvoyagetap.Show();
                MenuVoyageTap.SelectedMenuIndex = 0;                          

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvVoyage.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                }

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
                {
                    chkProposalflag.Visible = false;
                    lblProposalFlag.Visible = false;
                }


                if (Filter.CurrentVPRSVoyageFilter != null)
                {
                    NameValueCollection nvc = Filter.CurrentVPRSVoyageFilter;

                    //UcVessel.SelectedVessel = nvc.Get("UcVessel").ToString();
                    txtCharterer.Text = nvc.Get("txtCharterer").ToString();
                    ViewState["PAGENUMBER"] = General.GetNullableInteger(nvc.Get("PAGENUMBER").ToString());
                    ViewState["SORTEXPRESSION"] = nvc.Get("SORTEXPRESSION").ToString();
                    ViewState["SORTDIRECTION"] = General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString());
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();

                    criteria.Clear();
                    criteria.Add("UcVessel", UcVessel.SelectedVessel);
                    criteria.Add("txtCharterer", txtCharterer.Text);
                    criteria.Add("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
                    criteria.Add("SORTEXPRESSION", ViewState["SORTEXPRESSION"] == null ? "" : ViewState["SORTEXPRESSION"].ToString());
                    criteria.Add("SORTDIRECTION", ViewState["SORTDIRECTION"] == null ? "" : ViewState["SORTDIRECTION"].ToString());

                    Filter.CurrentVPRSVoyageFilter = criteria;
                }

               
            }
    


            toolbarvoyagelist.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRVoyage.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbarvoyagelist.AddFontAwesomeButton("javascript:CallPrint('gvVoyage')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarvoyagelist.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRVoyage.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbarvoyagelist.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRVoyage.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");

            //toolbarvoyagelist.AddImageLink("../CrewOffshore/CrewOffshoreDMRVoyageData.aspx?mode=NEW", "Add", "add.png", "ADDVOYAGE");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
            {
                DataTable dt = PhoenixCrewOffshoreDMRVoyageData.VoyageCharterStatusFlag(int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        gvVoyage.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = true;
                       // toolbarvoyagelist.AddImageLink("../CrewOffshore/CrewOffshoreDMRVoyageData.aspx?mode=NEW", "Add", "add.png", "ADDVOYAGE");
                    }
                }
            }
            else
            { toolbarvoyagelist.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRVoyageData.aspx?mode=NEW", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDVOYAGE"); }
            MenuVoyageList.AccessRights = this.ViewState;
            MenuVoyageList.MenuList = toolbarvoyagelist.Show();

           // BindData();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoyage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVoyage.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void UcVessel_TextChangedEvent(object sender, EventArgs e)
    {
        gvVoyage.EditIndexes.Clear();
        gvVoyage.SelectedIndexes.Clear();

        gvVoyage.Rebind();
    }

    protected void chkProposalflag_OnCheckedChanged(object sender, EventArgs e)
    {
        gvVoyage.EditIndexes.Clear();
        gvVoyage.SelectedIndexes.Clear();

        gvVoyage.Rebind();
    }
    protected void gvVoyage_ItemCommand(object sender, GridCommandEventArgs e)
    {
       
      
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            gvVoyage.ExportSettings.Excel.Format = GridExcelExportFormat.Biff;
            gvVoyage.ExportSettings.IgnorePaging = true;
            gvVoyage.ExportSettings.ExportOnlyData = true;
            gvVoyage.ExportSettings.OpenInNewWindow = true;
        }
        if (e.CommandName == RadGrid.RebindGridCommandName)
        {
            gvVoyage.EditIndexes.Clear();
            gvVoyage.SelectedIndexes.Clear();
            gvVoyage.Rebind();
        }
        if (e.CommandName == "Edit")
        {
            GridDataItem item = (GridDataItem)e.Item;
            RadLabel lblVoyageId = (RadLabel)item.FindControl("lblVoyageId");
            Filter.CurrentVPRSVoyageSelection = lblVoyageId.Text;

            Response.Redirect("CrewOffshoreDMRVoyageData.aspx", false);

        }
        if (e.CommandName == "InitInsert")
        {
            Response.Redirect("CrewOffshoreDMRVoyageData.aspx?mode=NEW", false);

        }
        if (e.CommandName == "ExportToExcel")
        {
            gvVoyage.ExportSettings.ExportOnlyData = true;
            gvVoyage.ExportSettings.IgnorePaging = true;

            ShowExcel();
        }
        if (e.CommandName == "Delete")
        {
            GridDataItem item = (GridDataItem)e.Item;
            PhoenixCrewOffshoreDMRVoyageData.DeleteVoyageData(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid(((RadLabel)item.FindControl("lblVoyageId")).Text));
        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }
    protected void gvVoyage_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
           
            //(item.FindControl("btndelete") as RadImageButton).Image.Url = Session["images"] + "/te_del.png";
            // string FLDCANCELFLAG = DataBinder.Eval(e.Item.DataItem, "ISPROPOSALCHARTERYN").ToString();
            string proposedyn = DataBinder.Eval(e.Item.DataItem, "ISPROPOSALCHARTERYN").ToString();
            if (proposedyn == "1")
               // gvVoyage.MasterTableView.GetColumn("deleteColumn").Display = true;
            (item.FindControl("btndelete") as LinkButton).Visible = true;// : false;
            else
                (item.FindControl("btndelete") as LinkButton).Visible = false;
                //(item["deleteColumn"].Controls[0] as ImageButton).Enabled = false;// : false;
        }
    }

    protected void gvVoyage_InsertCommand(object sender, GridCommandEventArgs e)
    {
        Response.Redirect("CrewOffshoreDMRVoyageData.aspx", false);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvVoyage.Rebind();
        //BindData();
    }

    protected void VoyageTap_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("VOYAGEDATA"))
        {
            Response.Redirect("CrewOffshoreDMRVoyageData.aspx", false);
        }
    }

   




    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDVOYAGENO", "FLDCOMMENCEDDATETIME", "FLDCOMMENCEDPORTANME", "FLDCOMMENCEMENTLOCATIONATSEA", "FLDCOMPLETEDDATE", "FLDCOMPLETEDPORTNAME", "FLDCOMPLETIOLOCATIONATSEA", "FLDESTIMATEDCOMMENCEDATE", "FLDCHARTERERNAME" };
        string[] alCaptions = { "Vessel Name", "Charter ID", "Commenced Date", "Commenced Port", "Commenced At Sea </br> In Location", "Completed Date", "Completed Port", "Completed At Sea </br> In Location", "Estimated Start of Charter", "Charterer" };

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

        ds = PhoenixCrewOffshoreDMRVoyageData.VoyageDataSearch(
            vesselid, null, "",
            txtCharterer.Text, null, nvc.Get("SORTEXPRESSION").ToString(),
            General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString()),
             gvVoyage.CurrentPageIndex + 1,
            gvVoyage.VirtualItemCount, ref iRowCount, ref iTotalPageCount, chkProposalflag.Checked.Equals(true) ? 1 : 0);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"Charter.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Charter</h3></td>");
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

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

        ds = PhoenixCrewOffshoreDMRVoyageData.VoyageDataSearch(
            vesselid, null, "",
            txtCharterer.Text, null, nvc.Get("SORTEXPRESSION").ToString(),
            General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString()),
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvVoyage.PageSize, ref iRowCount, ref iTotalPageCount, chkProposalflag.Checked.Equals(true) ? 1 : 0);

        string[] alColumns = { "FLDVESSELNAME", "FLDVOYAGENO", "FLDCOMMENCEDDATETIME", "FLDCOMMENCEDPORTANME", "FLDCOMMENCEMENTLOCATIONATSEA", "FLDCOMPLETEDDATE", "FLDCOMPLETEDPORTNAME", "FLDCOMPLETIOLOCATIONATSEA", "FLDESTIMATEDCOMMENCEDATE", "FLDCHARTERERNAME" };
        string[] alCaptions = { "Vessel Name", "Charter ID", "Commenced Date", "Commenced Port", "Commenced At Sea </br> In Location", "Completed Date", "Completed Port", "Completed At Sea </br> In Location", "Estimated Start of Charter", "Charterer" };

        General.SetPrintOptions("gvVoyage", "Charter", alCaptions, alColumns, ds);
        gvVoyage.DataSource = ds;
        gvVoyage.VirtualItemCount = iRowCount;
      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
       
    }

  
    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

  

   
  

   
  

    

    //protected void gvVoyage_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("UPDATE"))
    //        {
    //            _gridView.EditIndex = -1;
    //          //  BindData();
    //        }
    //        if (e.CommandName.ToUpper() == "EDIT")
    //        {
    //            Filter.CurrentVPRSVoyageSelection = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVoyageId")).Text;

    //            Response.Redirect("CrewOffshoreDMRVoyageData.aspx", false);
    //        }
    //        if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            PhoenixCrewOffshoreDMRVoyageData.DeleteVoyageData(
    //                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //                General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblVoyageId")).Text));

    //            // BindData();
    //        }
    //        if (e.CommandName == "Page")
    //        {
    //            ViewState["PAGENUMBER"] = null;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvVoyage_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    //BindData();
    //}

    //protected void gvVoyage_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;

    // //   BindData();
    //}

    //protected void gvVoyage_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{

    //}

    protected void gvVoyage_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = drv["ISPROPOSALCHARTERYN"].ToString().Equals("1") ? true : false;
            }
        }
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
            BindData();
            gvVoyage.Rebind();
        }

        if (CommandName.ToUpper().Equals("RESET"))
        {
          
            ClearFilter();
        }
    }

    private void ClearFilter()
    {
        UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : "";
        txtCharterer.Text = "";

        NameValueCollection criteria = new NameValueCollection();

        criteria.Clear();
        criteria.Add("UcVessel", UcVessel.SelectedVessel);
        criteria.Add("txtCharterer", "");
        criteria.Add("PAGENUMBER", "1");
        criteria.Add("SORTEXPRESSION", "");
        criteria.Add("SORTDIRECTION", "");

        Filter.CurrentVPRSVoyageFilter = criteria;
      
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvVoyage.Rebind();
    }


}
