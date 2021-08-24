using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreManningScale : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetManningBudgetRowSelection();

    }

    private void SetManningBudgetRowSelection()
    {
        gvManningScaleRevision.SelectedIndexes.Clear();
        if (ViewState["REVISIONID"].ToString() == "")
        {
            ViewState["REVISIONID"] = gvManningScaleRevision.Items[0].GetDataKeyValue("FLDREVISIONID").ToString();
            gvManningScaleRevision.Items[0].Selected = true;           
            BindRevision();
            gvManningScale.Rebind();
        }
        else
        {
            foreach (GridDataItem item in gvManningScaleRevision.Items)
            {
                if (item.GetDataKeyValue("FLDREVISIONID").ToString() == ViewState["REVISIONID"].ToString())
                {
                    gvManningScaleRevision.SelectedIndexes.Add(item.ItemIndex);
                    item.Selected = true;                  
                    BindRevision();
                    gvManningScale.Rebind();
                }
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Crew List", "CREWLIST");
        toolbarsub.AddButton("Compliance", "CHECK");
        toolbarsub.AddButton("Crew Format", "CREWLISTFORMAT");
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            toolbarsub.AddButton("Vessel Scale", "MANNINGSCALE");
            toolbarsub.AddButton("Manning", "MANNING");
            toolbarsub.AddButton("Budget", "BUDGET");
        }
        CrewQuery.AccessRights = this.ViewState;
        CrewQuery.MenuList = toolbarsub.Show();
        CrewQuery.SelectedMenuIndex = 4;

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreManningScale.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvManningScaleRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuRegistersRevision.AccessRights = this.ViewState;
        MenuRegistersRevision.MenuList = toolbar.Show();
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            ViewState["RPAGENUMBER"] = 1;
            ViewState["RSORTEXPRESSION"] = null;
            ViewState["RSORTDIRECTION"] = null;

            ViewState["REVISIONID"] = "";
            ViewState["REQUIRESUPDATEYN"] = "";

            ViewState["VESSELID"] = "";

            if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            {
                //UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                //UcVessel.Enabled = false;
            }
            else
            {
                if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                    txtVessel.Text = ViewState["VESSELID"].ToString();
            }
            gvManningScaleRevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvManningScale.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

          
        }
        // BindRevision();
        // BindData();

        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreManningScale.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvManningScale')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        if (ViewState["REQUIRESUPDATEYN"].ToString().Equals("1"))
            toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','RegistersOffshoreVesselManningScaleDate.aspx','medium')", "Update Effective Date for existing records", "modify.png", "UPDATEDATE");
        MenuRegistersManningScale.AccessRights = this.ViewState;
        MenuRegistersManningScale.MenuList = toolbar.Show();
        // MenuRegistersManningScale.SetTrigger(pnlManningScaleEntry);
    }

    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("CREWLIST"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreCrewList.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("CHECK"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreCrewComplianceCheck.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("CREWLISTFORMAT"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreReportCrewListFormat.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("MANNINGSCALE"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreVesselManningScale.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("BUDGET"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreBudget.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDRANKNAME", "FLDEQUIVALENTRANKNAME", "FLDOWNERSCALE", "FLDSAFESCALE", "FLDCBASCALE", "FLDCONTRACTPERIODDAYS", "FLDREMARKS" };
        string[] alCaptions = { "Rank Name", "Equivalent Rank", "Owner Scale", "Safe Scale", "CBA Scale", "Contract Period(days)", "Remarks" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersVesselManningScale.ManningScaleSearch(General.GetNullableInteger(ViewState["VESSELID"].ToString())
            , sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableGuid(ViewState["REVISIONID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=ManningScale.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel ManningScale</h3></td>");
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

    protected void RegistersManningScale_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDRANKNAME", "FLDEQUIVALENTRANKNAME", "FLDOWNERSCALE", "FLDSAFESCALE", "FLDCBASCALE", "FLDCONTRACTPERIODDAYS", "FLDREMARKS" };
        string[] alCaptions = { "Rank Name", "Equivalent Rank", "Owner Scale", "Safe Scale", "CBA Scale", "Contract Period(days)", "Remarks" };

        if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) == null)
        {
            ucError.ErrorMessage = "Choose a vessel in crew list.";
            ucError.Visible = true;
            return;
        }

        DataSet dsVessel = PhoenixRegistersVessel.EditVessel(int.Parse(ViewState["VESSELID"].ToString()));

        if (dsVessel.Tables[0].Rows.Count > 0)
        {
            DataRow drVessel = dsVessel.Tables[0].Rows[0];

            txtVessel.Text = drVessel["FLDVESSELNAME"].ToString();
        }

        DataSet ds = PhoenixRegistersVesselManningScale.ManningScaleSearch(int.Parse(ViewState["VESSELID"].ToString())
            , sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableGuid(ViewState["REVISIONID"].ToString()));

        General.SetPrintOptions("gvManningScale", "Manning Scale", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvManningScale.DataSource = ds;

            DataRow dr = ds.Tables[1].Rows[0];

            txtOwnerScaleTotal.Text = dr["FLDOWNERSCALETOTAL"].ToString();
            txtSafeScaleTotal.Text = dr["FLDSAFESCALETOTAL"].ToString();
        }
        else
        {
            gvManningScale.DataSource = ds;
        }

        gvManningScale.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {

        BindData();

    }


    private void InsertManningScale(
        string vesselid
        , string rank
        , string ownerscale
        , string safescale
        , string CBAScale
        , string contractperiod
        , string remarks
        , string contractperioddays
        , string revisionid)
    {
        if (!IsValidManningScale(rank, ownerscale, safescale, CBAScale, contractperioddays))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersVesselManningScale.InsertManningScale(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , Convert.ToInt16(vesselid)
            , Convert.ToInt16(rank)
            , null                                  // Nationality
            , Convert.ToInt16(ownerscale)
            , Convert.ToInt16(safescale)
            , Convert.ToInt16(CBAScale)
            , General.GetNullableInteger(contractperiod)
            , remarks
            , General.GetNullableInteger(contractperioddays)
            , General.GetNullableGuid(revisionid));
    }

    private void UpdateManningScale(
        string vesselid
        , string manningscaleid
        , string rank
        , string ownerscale
        , string safescale
        , string CBAScale
        , string contractperiod
        , string remarks
        , string contractperioddays)
    {

        if (!IsValidManningScale(rank, ownerscale, safescale, CBAScale, contractperioddays))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersVesselManningScale.UpdateManningScale(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , Convert.ToInt16(manningscaleid)
            , Convert.ToInt16(vesselid), Convert.ToInt16(rank)
            , null                                 // Nationality
            , Convert.ToInt16(ownerscale)
            , Convert.ToInt16(safescale)
            , Convert.ToInt16(CBAScale)
            , General.GetNullableInteger(contractperiod)
            , remarks
            , General.GetNullableInteger(contractperioddays));

        ucStatus.Text = "Manning Scale information updated";
    }
    private bool IsValidManningScalevessel(string vesselid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) == null)
            ucError.ErrorMessage = "Please switch the vessel";

        return (!ucError.IsError);
    }

    private bool IsValidManningScale(string rank, string ownerscale, string safescale, string CBAscale, string contractperiod)
    {
        Int16 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["REVISIONID"].ToString()) == null)
            ucError.ErrorMessage = "Please select the revision to add manning scale.";

        if (!Int16.TryParse(rank, out resultInt))
            ucError.ErrorMessage = "Rank is required.";

        if (!Int16.TryParse(ownerscale, out resultInt))
            ucError.ErrorMessage = "Valid Owner Scale is required.";

        if (!Int16.TryParse(safescale, out resultInt))
            ucError.ErrorMessage = "Safe Scale is required.";

        if (!Int16.TryParse(CBAscale, out resultInt))
            ucError.ErrorMessage = "CBA Scale is required.";

        if (!Int16.TryParse(contractperiod, out resultInt))
            ucError.ErrorMessage = "Contract Period is required.";

        return (!ucError.IsError);
    }

    private void DeleteManningScale(int vesselid, int manningscaleid)
    {
        PhoenixRegistersVesselManningScale.DeleteManningScale(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, manningscaleid, vesselid);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    // Manning Scale Revision

    protected void ShowExcelR()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEFFECTIVEDATE", "FLDREVISIONNO" };
        string[] alCaptions = { "Effective Date", "Revision No" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["RSORTEXPRESSION"] == null) ? null : (ViewState["RSORTEXPRESSION"].ToString());
        if (ViewState["RSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["RSORTDIRECTION"].ToString());

        if (ViewState["RROWCOUNT"] == null || Int32.Parse(ViewState["RROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["RROWCOUNT"].ToString());

        DataTable dt = PhoenixRegistersVesselManningScale.SearchManningScaleRevision(General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["RPAGENUMBER"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount);

        General.ShowExcel("Manning Scale Revision", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenuRegistersRevision_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelR();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindRevision()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEFFECTIVEDATE", "FLDREVISIONNO" };
        string[] alCaptions = { "Effective Date", "Revision No" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["RSORTEXPRESSION"] == null) ? null : (ViewState["RSORTEXPRESSION"].ToString());
        if (ViewState["RSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["RSORTDIRECTION"].ToString());

        if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) == null)
        {
            ucError.ErrorMessage = "Choose a vessel in crew list.";
            ucError.Visible = true;
            return;
        }

        DataSet dsVessel = PhoenixRegistersVessel.EditVessel(int.Parse(ViewState["VESSELID"].ToString()));
        if (dsVessel.Tables[0].Rows.Count > 0)
        {
            DataRow drVessel = dsVessel.Tables[0].Rows[0];
            ViewState["REQUIRESUPDATEYN"] = drVessel["FLDREQUIRESUPDATEYN"].ToString();
            txtVessel.Text = drVessel["FLDVESSELNAME"].ToString();
        }

        DataTable dt = PhoenixRegistersVesselManningScale.SearchManningScaleRevision(int.Parse(ViewState["VESSELID"].ToString()),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["RPAGENUMBER"].ToString()),
                            gvManningScaleRevision.PageSize,
                            ref iRowCount,
                            ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvManningScaleRevision", "Manning Scale Revision", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvManningScaleRevision.DataSource = ds;


            //if (ViewState["REVISIONID"] == null || ViewState["REVISIONID"].ToString() == "")
            //{
            //   // gvManningScaleRevision.MasterTableView.Items[0].Selected = true;
            //    ViewState["REVISIONID"] = gvManningScaleRevision.MasterTableView.DataKeyValues[0].ToString();
            //}


        }
        else
        {
            gvManningScaleRevision.DataSource = ds;
            ViewState["REVISIONID"] = "";
        }
        gvManningScaleRevision.VirtualItemCount = iRowCount;
        ViewState["RROWCOUNT"] = iRowCount;
        ViewState["RTOTALPAGECOUNT"] = iTotalPageCount;


        BindData();

    }

    //private void BindPageURL(int rowindex)
    //{
    //    try
    //    {


    //        ViewState["REVISIONID"] = gvManningScaleRevision.MasterTableView.DataKeyValues[rowindex].ToString();
    //        SetRowSelection();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private void SetRowSelection()
    //{

    //    //foreach (GridDataItem item in gvManningScaleRevision.MasterTableView.Items)
    //    //{
    //    //    if (gvManningScaleRevision.MasterTableView.Items[0].GetDataKeyValue("REVISIONID").ToString().Equals(ViewState["REVISIONID"].ToString()))
    //    //    {
    //    //        item.Selected = true;

    //    //        break;
    //    //    }
    //    //}

    //}





    private bool IsValidData(string effectivedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(effectivedate) == null)
            ucError.ErrorMessage = "Effective Date is required.";

        return (!ucError.IsError);
    }


    private Boolean IsPreviousEnabledR()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["RPAGENUMBER"];
        iTotalPageCount = (int)ViewState["RTOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabledR()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["RPAGENUMBER"];
        iTotalPageCount = (int)ViewState["RTOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }




    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

        BindRevision();

    }

    protected void gvManningScaleRevision_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvManningScaleRevision.CurrentPageIndex + 1;
            BindRevision();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    protected void gvManningScaleRevision_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADDREVISION"))
            {
                if (!IsValidManningScalevessel(txtVessel.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if (!IsValidData(((UserControlDate)e.Item.FindControl("ucEffectiveDateAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersVesselManningScale.InsertManningScaleRevision(int.Parse(ViewState["VESSELID"].ToString()),
                    DateTime.Parse(((UserControlDate)e.Item.FindControl("ucEffectiveDateAdd")).Text));

                //BindRevision();
                gvManningScaleRevision.Rebind();
                ((UserControlDate)e.Item.FindControl("ucEffectiveDateAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETEREVISION"))
            {
                ViewState["REVISIONID"] = "";
                Guid revisionid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDREVISIONID"].ToString());
                PhoenixRegistersVesselManningScale.DeleteManningScaleRevision(revisionid);
                gvManningScaleRevision.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SELECTREVISION"))
            {
                e.Item.Selected = true;
                ViewState["REVISIONID"] = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDREVISIONID"].ToString());
                //BindPageURL(nCurrentRow);

                gvManningScale.Rebind();

            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(
                    ((UserControlDate)e.Item.FindControl("ucEffectiveDateEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                // int id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"];
                Guid revisionid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDREVISIONID"].ToString());

                PhoenixRegistersVesselManningScale.UpdateManningScaleRevision(revisionid
                    , int.Parse(ViewState["VESSELID"].ToString())
                    , DateTime.Parse(((UserControlDate)e.Item.FindControl("ucEffectiveDateEdit")).Text)
                    );


                //BindRevision();
                gvManningScaleRevision.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("COPY"))
            {
                Guid revisionid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDREVISIONID"].ToString());
                PhoenixRegistersVesselManningScale.CopyPreviousRevManningScale(revisionid);
                gvManningScaleRevision.Rebind();
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvManningScaleRevision_ItemDataBound(object sender, GridItemEventArgs e)
    {


        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            LinkButton dc = (LinkButton)e.Item.FindControl("cmdCopy");
            if (dc != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, dc.CommandName))
                    dc.Visible = false;

                dc.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to copy manning scale from previous revision?')");
            }
            if (ViewState["FLDREVISIONID"] == null || ViewState["FLDREVISIONID"].ToString() == "")
            {
                LinkButton lnkEffectiveDate = (LinkButton)e.Item.FindControl("lnkEffectiveDate");
                lnkEffectiveDate.CommandName = "SELECTREVISION";
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ab = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }

    protected void gvManningScale_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvManningScale.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvManningScale_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                if (!IsValidManningScalevessel(txtVessel.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertManningScale(
                    ViewState["VESSELID"].ToString()
                    , ((UserControlRank)e.Item.FindControl("ucRankAdd")).SelectedRank.ToString()
                    , ((UserControlMaskedTextBox)e.Item.FindControl("txtOwnerScaleAdd")).Text
                    , ((UserControlMaskedTextBox)e.Item.FindControl("txtSafeScaleAdd")).Text
                    , ((UserControlMaskedTextBox)e.Item.FindControl("txtCBAScaleAdd")).Text
                    , null //((TextBox)_gridView.FooterRow.FindControl("txtContractPeriodAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text
                    , ((UserControlMaskedTextBox)e.Item.FindControl("txtContractPeriodAdd")).Text
                    , General.GetNullableString(ViewState["REVISIONID"].ToString()));

                gvManningScale.Rebind();
                ((UserControlRank)e.Item.FindControl("ucRankAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                UpdateManningScale(
                    ViewState["VESSELID"].ToString()
                    , ((RadLabel)e.Item.FindControl("lblManningScaleIdEdit")).Text
                    , ((UserControlRank)e.Item.FindControl("ucRank")).SelectedRank.ToString()
                    , ((UserControlMaskedTextBox)e.Item.FindControl("txtOwnerScaleEdit")).Text
                    , ((UserControlMaskedTextBox)e.Item.FindControl("txtSafeScaleEdit")).Text
                    , ((UserControlMaskedTextBox)e.Item.FindControl("txtCBAScaleEdit")).Text
                    , null //((TextBox)e.Item.FindControl("txtContractPeriodEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text
                    , ((UserControlMaskedTextBox)e.Item.FindControl("txtContractPeriodEdit")).Text);


                gvManningScale.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteManningScale(Int16.Parse(ViewState["VESSELID"].ToString()), Int32.Parse(((RadLabel)e.Item.FindControl("lblManningScaleId")).Text));
                gvManningScale.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                try
                {

                    if (!IsValidManningScale(((UserControlRank)e.Item.FindControl("ucRank")).SelectedRank
                      , ((UserControlMaskedTextBox)e.Item.FindControl("txtOwnerScaleEdit")).Text
                      , ((UserControlMaskedTextBox)e.Item.FindControl("txtSafeScaleEdit")).Text
                      , ((UserControlMaskedTextBox)e.Item.FindControl("txtCBAScaleEdit")).Text
                      , ((UserControlMaskedTextBox)e.Item.FindControl("txtContractPeriodEdit")).Text))
                    {
                        e.Canceled = true;
                        ucError.Visible = true;
                        return;
                    }

                    UpdateManningScale(
                        ViewState["VESSELID"].ToString()
                        , ((RadLabel)e.Item.FindControl("lblManningScaleIdEdit")).Text
                        , ((UserControlRank)e.Item.FindControl("ucRank")).SelectedRank
                        , ((UserControlMaskedTextBox)e.Item.FindControl("txtOwnerScaleEdit")).Text
                        , ((UserControlMaskedTextBox)e.Item.FindControl("txtSafeScaleEdit")).Text
                        , ((UserControlMaskedTextBox)e.Item.FindControl("txtCBAScaleEdit")).Text
                        , null //((TextBox)e.Item.FindControl("txtContractPeriodEdit")).Text
                        , ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text
                        , ((UserControlMaskedTextBox)e.Item.FindControl("txtContractPeriodEdit")).Text);


                    gvManningScale.Rebind();

                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvManningScale_SortCommand(object sender, GridSortCommandEventArgs e)
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
    }

    protected void gvManningScale_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);



        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }

        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            db = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkRankEdit");
            if (lb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, lb.CommandName))
                    lb.CommandName = "";
            }

            UserControlRank ucRank = (UserControlRank)e.Item.FindControl("ucRank");
            DataRowView drvRank = (DataRowView)e.Item.DataItem;
            if (ucRank != null) ucRank.SelectedRank = drvRank["FLDRANK"].ToString();

            LinkButton anl = (LinkButton)e.Item.FindControl("cmdEquivalentRank");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            if (anl != null) anl.Visible = SessionUtil.CanAccess(this.ViewState, anl.CommandName);
            if (anl != null)
            {
                anl.Attributes.Add("onclick", "javascript:parent.openNewWindow('MoreInfo', '', '" + Session["sitepath"] + "/Registers/RegistersManningEquivalentRank.aspx?manningscalekey=" + lblDTKey.Text + "'); return false;");
            }
            if (lblDTKey != null)
            {
                LinkButton img = (LinkButton)e.Item.FindControl("imgGroupList");
                img.Attributes.Add("onclick", "javascript:parent.openNewWindow('MoreInfo','', '" + Session["sitepath"] + "/Registers/RegistersManningEquivalentRank.aspx?readonly=1&manningscalekey=" + lblDTKey.Text + "')");
            }
        }
    }

    protected void gvManningScaleRevision_SortCommand(object sender, GridSortCommandEventArgs e)
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
    }
}
