using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Export2XL;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class DryDockProject : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetProjectRowSelection();

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Project", "PROJECT");
            toolbarmain.AddButton("Quotation", "QUOTATION");
            toolbarmain.AddButton("Supt Job Remarks", "JOBREMARKS");
            toolbarmain.AddButton("Supt Feedback", "SUPTFEEDBACK");
          

            MenuProjectGeneral.AccessRights = this.ViewState;
            MenuProjectGeneral.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockProject.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvProject')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DryDock/DryDockProjectAdd.aspx?vslid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "');", "New Project", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
                toolbargrid.AddFontAwesomeButton("../DryDock/DryDockProject.aspx", "Refresh Project", "<i class=\"fas fa-sync\"></i>", "REFRESH");
                ViewState["VSLID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }

            MenuProjectGrid.AccessRights = this.ViewState;
            MenuProjectGrid.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarlineitemgrid = new PhoenixToolbar();
            toolbarlineitemgrid.AddFontAwesomeButton("../DryDock/DryDockProject.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            MenuProjectLineItemGrid.AccessRights = this.ViewState;
            MenuProjectLineItemGrid.MenuList = toolbarlineitemgrid.Show();
            MenuProjectLineItemGrid.Visible = true;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                //ViewState["CURRENTINDEX"] = 1;
                ViewState["PROJECTID"] = "";
                ViewState["EXCHANGERATEID"] = "";
                ViewState["VSLID"] = "";
                ViewState["ORDERID"] = "";
                if (Request.QueryString["vesselid"] != null)
                {
                    ViewState["VSLID"] = Request.QueryString["vesselid"];
                }
                if(Filter.CurrentSelectedDryDockProject !=null)
                {
                    ViewState["ORDERID"] = Filter.CurrentSelectedDryDockProject;
                }
                else if (Request.QueryString["orderid"] != null)
                {
                    ViewState["ORDERID"] = Request.QueryString["orderid"];
                }
                ViewState["SELECTEDITEM"] = "";

                //tblPagingLineItem.Visible = false;


                //Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");
                gvProject.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvProjectLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            MenuProjectGeneral.SelectedMenuIndex = 0;
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller1", "<script>scrollToVal('divScroll1', 'hdnScroll1');</script>");
            // BindData();
            //BindDataProjectLineItem();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ProjectGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            //if (CommandName.ToUpper().Equals("FIND"))
            //{
            //    ViewState["PAGENUMBER"] = 1;
            //    //  gvProjectLineItem.CurrentPageIndex = 0;
            //    BindData();
            //}
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                //if (Filter.CurrentSelectedDryDockProject == null)
                //{
                //    ucError.ErrorMessage = "Please select a Project";
                //    ucError.Visible = true;
                //    return;
                //}
                //PhoenixDryDock2XL.Export2XLDryDockProject(General.GetNullableGuid(Filter.CurrentSelectedDryDockProject), int.Parse(ViewState["VSLID"].ToString()));

                ShowExcel();
            }
            if (Filter.CurrentSelectedDryDockProject == null && CommandName.ToUpper() != "ADD")
            {
                ShowError();
                return;
            }
            else
            {
                if (CommandName.ToUpper().Equals("REFRESH"))
                {
                    PhoenixDryDockOrder.InsertDryDockOrderLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                        General.GetNullableGuid(Filter.CurrentSelectedDryDockProject.ToString()));
                    BindDataProjectLineItem();
                }

                if (CommandName.ToUpper().Equals("QUOTATION"))
                {
                    Response.Redirect("../DryDock/DryDockQuotation.aspx?vslid=" + ViewState["VSLID"], false);
                }
                if (CommandName.ToUpper().Equals("SUPTFEEDBACK"))
                {
                    Response.Redirect("../DryDock/DryDockSuptRemarks.aspx?vslid=" + ViewState["VSLID"] + "&orderid=" + Filter.CurrentSelectedDryDockProject, false);
                }
                if (CommandName.ToUpper().Equals("JOBREMARKS"))
                {
                    Response.Redirect("../DryDock/DryDockJobSuptRemarks.aspx?vslid=" + ViewState["VSLID"] + "&orderid=" + Filter.CurrentSelectedDryDockProject, false);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ProjectLineItemGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                PhoenixDryDock2XL.Export2XLDryDockProjectLineItem(General.GetNullableGuid(Filter.CurrentSelectedDryDockProject), int.Parse(ViewState["VSLID"].ToString()));
                //ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvProject_PreRender(object sender, EventArgs e)
    //{
    //    if (ViewState["SELECTEDITEM"].ToString() == "" && gvProject.Items.Count != 0)
    //    {
    //        gvProject.Items[0].Selected = true;
    //        Filter.CurrentSelectedDryDockProject = gvProject.MasterTableView.Items[0].GetDataKeyValue("FLDORDERID").ToString();

    //        RadLabel lblVesselId = (RadLabel)gvProject.MasterTableView.Items[0].FindControl("lblVesselId");
    //        ViewState["VSLID"] = lblVesselId.Text;
    //        BindDataProjectLineItem();
    //        gvProjectLineItem.Rebind();
    //    }
    //}

    protected void ShowExcel()
    {


        string[] alColumns = { "FLDVESSELNAME", "FLDNUMBER", "FLDTITLE", "FLDCATEGORYNAME", "FLDFROMDATE", "FLDTODATE", "FLDACTUALSTARTDATE", "FLDACTUALFINISHDATE", "FLDDESCRIPTION", "FLDCREATEDDATE" };
        string[] alCaptions = { "Vessel", "Project ID", "Title", "Docking Category", "Est From", "Est To", "Started on ", "Completed on", "Status", "Created Date" };

        int rowcount = 0;
        DataSet ds = PhoenixDryDockOrder.DryDockOrderSearch
                   (PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                   null
                   , PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? General.GetNullableInteger("1") : null, 1, gvProject.PageSize, ref rowcount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DryDockProject.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Dry Dock Project</h3></td>");
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
                if (alColumns[i] == "FLDFROMDATE" || alColumns[i] == "FLDTODATE" || alColumns[i] == "FLDACTUALSTARTDATE" || alColumns[i] == "FLDACTUALFINISHDATE" || alColumns[i] == "FLDCREATEDDATE")
                {
                    Response.Write(General.GetDateTimeToString(dr[alColumns[i]].ToString()));
                }
                else
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
        try
        {

            string[] alColumns = { "FLDVESSELNAME", "FLDNUMBER", "FLDTITLE", "FLDCATEGORYNAME", "FLDFROMDATE", "FLDTODATE", "FLDACTUALSTARTDATE", "FLDACTUALFINISHDATE", "FLDDESCRIPTION", "FLDCREATEDDATE" };
            string[] alCaptions = { "Vessel", "Project ID", "Title", "Docking Category", "Est From", "Est To", "Started on ", "Completed on", "Status", "Created Date" };
            int rowcount = 0;

            DataSet ds = PhoenixDryDockOrder.DryDockOrderSearch
                  (PhoenixSecurityContext.CurrentSecurityContext.VesselID
                   , null
                   , PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? General.GetNullableInteger("1") : null
                  , gvProject.CurrentPageIndex + 1
                  , gvProject.PageSize
                  , ref rowcount);
            General.SetPrintOptions("gvProject", "Project", alCaptions, alColumns, ds);

            gvProject.DataSource = ds;
            gvProject.VirtualItemCount = rowcount;
            if (string.IsNullOrEmpty(Filter.CurrentSelectedDryDockProject))
            {
                if (!string.IsNullOrEmpty(ViewState["ORDERID"].ToString()))
                {
                    Filter.CurrentSelectedDryDockProject = ViewState["ORDERID"].ToString();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    { 
                    DataRow dr = ds.Tables[0].Rows[0];
                    //gvProject.Items[0].Selected = true;
                    gvProject.SelectedIndexes.Clear();
                    gvProject.SelectedIndexes.Add(0);
                    ViewState["ORDERID"] = dr["FLDORDERID"].ToString();
                    Filter.CurrentSelectedDryDockProject = dr["FLDORDERID"].ToString();
                    }
                }
            }
            //SetProjectRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    //protected void gvProject_SelectIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    gvProject.SelectedIndex = se.NewSelectedIndex;
    //    Filter.CurrentSelectedDryDockProject = ((Label)gvProject.Rows[se.NewSelectedIndex].FindControl("lblProjectid")).Text;
    //    ViewState["DTKEY"] = ((Label)gvProject.Rows[se.NewSelectedIndex].FindControl("lbldtkey")).Text;
    //    ViewState["VSLID"] = ((Label)gvProject.Rows[se.NewSelectedIndex].FindControl("lblVesselId")).Text;
    //    BindData();
    //    ViewState["PAGENUMBER"] = 1;
    //    BindDataProjectLineItem();
    //}

    //protected void gvProject_RowCommand(object sender, GridViewCommandEventArgs gce)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nRow = int.Parse(gce.CommandArgument.ToString());
    //        RadLabel l = (RadLabel)_gridView.Rows[nRow].FindControl("lblProjectid");
    //        RadLabel vsl = (RadLabel)_gridView.Rows[nRow].FindControl("lblVesselId");

    //        if (gce.CommandName.ToUpper().Equals("SPARE"))
    //        {
    //            Response.Redirect("../DryDock/DryDockPurchase.aspx?vslid=" + vsl.Text + "&projectid=" + l.Text, false);
    //        }
    //        if (gce.CommandName.ToUpper().Equals("ESTIMATE"))
    //        {
    //            PhoenixDryDock2XL.Export2XLDryDockEstimate(new Guid(l.Text), int.Parse(vsl.Text));
    //        }
    //        if (gce.CommandName.ToUpper().Equals("INITIATE"))
    //        {
    //            PhoenixDryDockOrder.CreateStandardJobWorkRequest(new Guid(l.Text), int.Parse(vsl.Text));
    //            PhoenixDryDockOrder.InitiateDryDockOrder(new Guid(l.Text), int.Parse(vsl.Text));
    //        }
    //        Filter.CurrentSelectedDryDockProject = l.Text;
    //        ViewState["VSLID"] = vsl.Text;
    //        BindDataProjectLineItem();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvProject_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    try
    //    {

    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            if (ViewState["SORTEXPRESSION"] != null)
    //            {
    //                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //                if (img != null)
    //                {
    //                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                        img.Src = Session["images"] + "/arrowUp.png";
    //                    else
    //                        img.Src = Session["images"] + "/arrowDown.png";

    //                    img.Visible = true;
    //                }
    //            }
    //        }
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {

    //            ImageButton lb = (ImageButton)e.Row.FindControl("cmdEdit");
    //            if (lb != null)
    //            {
    //                lb.Visible = SessionUtil.CanAccess(this.ViewState, lb.CommandName);
    //                lb.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', '../DryDock/DryDockProjectAdd.aspx?vslid=" + drv["FLDVESSELID"].ToString() + "&ProjectID=" + drv["FLDORDERID"].ToString() + "');return false;");
    //            }


    //            ImageButton ib = (ImageButton)e.Row.FindControl("cmdExport2XL");
    //            if (ib != null)
    //            {
    //                ib.Visible = SessionUtil.CanAccess(this.ViewState, ib.CommandName);
    //            }
    //            ImageButton sp = (ImageButton)e.Row.FindControl("cmdSpare");
    //            if (sp != null)
    //            {
    //                sp.Visible = SessionUtil.CanAccess(this.ViewState, sp.CommandName);
    //            }
    //            ImageButton est = (ImageButton)e.Row.FindControl("cmdEstimate");
    //            if (est != null)
    //            {
    //                est.Visible = SessionUtil.CanAccess(this.ViewState, est.CommandName);
    //            }

    //            Label lblProjectid = (Label)e.Row.FindControl("lblProjectid");

    //            string jvscript = "javascript:parent.Openpopup('codehelp1','','../Options/OptionsDrydock.ashx?methodname=DRYDOCKEXPORT2XL&exportoption=project&projectid=" + lblProjectid.Text + "&vslid=" + drv["FLDVESSELID"].ToString() + "&usercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "'); return true;";
    //            if (ib != null) ib.Attributes.Add("onclick", jvscript);

    //            ImageButton initiate = (ImageButton)e.Row.FindControl("cmdInitiate");
    //            if (initiate != null)
    //            {
    //                initiate.Attributes.Add("onclick", "return fnConfirmDelete(event ,'Are you sure you want Initiate Project'); return false;");
    //                initiate.Visible = SessionUtil.CanAccess(this.ViewState, initiate.CommandName);
    //                if (drv["FLDSTATUS"].ToString() == "0")
    //                {
    //                    initiate.Visible = false;
    //                }
    //            }
    //            ImageButton att = (ImageButton)e.Row.FindControl("cmdAttachment");
    //            if (att != null)
    //            {
    //                if (drv["FLDISATTACHMENT"].ToString() == "1")
    //                    att.ImageUrl = Session["images"] + "/attachment.png";
    //                else
    //                    att.ImageUrl = Session["images"] + "/no-attachment.png";
    //                att.Attributes["onclick"] = "javascript:Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"] + "&mod="
    //                                                    + PhoenixModule.DRYDOCK + "'); return false;";
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    try
    //    {
    //        dt.Rows.Add(dt.NewRow());
    //        gv.DataSource = dt;
    //        gv.DataBind();

    //        int colcount = gv.Columns.Count;
    //        gv.Rows[0].Cells.Clear();
    //        gv.Rows[0].Cells.Add(new TableCell());
    //        gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //        gv.Rows[0].Cells[0].Font.Bold = true;
    //        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //        gv.Rows[0].Attributes["onclick"] = "";
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvProject.Rebind();
            gvProjectLineItem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (ViewState["PROJECTID"] == null || ViewState["PROJECTID"].ToString() == "")
            {
                ShowError();
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void ShowError()
    {
        ucError.HeaderMessage = "Navigation Error";
        ucError.ErrorMessage = "Please select a Project to view the Details";
        ucError.Visible = true;
    }

    private void BindDataProjectLineItem()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDNUMBER", "FLDTITLE", "FLDJOBTYPE", "FLDJOBDESCRIPTION" };
            string[] alCaptions = { "Number", "Title", "Job Type", "Job Description" };

            DataSet ds;
            if (Filter.CurrentSelectedDryDockProject == null || ViewState["VSLID"].ToString() == string.Empty)
                return;

            ds = PhoenixDryDockOrder.ListDryDockOrderLineItem
                (int.Parse(ViewState["VSLID"].ToString()),
                new Guid(Filter.CurrentSelectedDryDockProject.ToString()), null,
                gvProjectLineItem.CurrentPageIndex + 1,
                  gvProjectLineItem.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvProjectLineItem", "Dry Dock Project", alCaptions, alColumns, ds);
            gvProjectLineItem.DataSource = ds;
            gvProjectLineItem.VirtualItemCount = iRowCount;

            //tblPagingLineItem.Visible = true;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    //protected void gvProjectLineItem_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = de.NewEditIndex;
    //    BindDataProjectLineItem();

    //    //SetPageNavigator();
    //}

    //protected void gvProjectLineItem_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    ViewState["SORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
    //        ViewState["SORTDIRECTION"] = 1;
    //    else
    //        ViewState["SORTDIRECTION"] = 0;


    //    BindDataProjectLineItem();
    //}

    //protected void gvProjectLineItem_SelectIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    // gvProjectLineItem.SelectedIndex = se.NewSelectedIndex;
    //    BindDataProjectLineItem();
    //}

    //protected void gvProjectLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
    //{

    //    if (e.CommandName.ToString().ToUpper() == "SORT")
    //        return;

    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = int.Parse(e.CommandArgument.ToString());

    //}
    //protected void gvProjectLineItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        PhoenixDryDockOrder.UpdateDryDockOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //             int.Parse(ViewState["VSLID"].ToString()),
    //             new Guid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblOrderidEdit")).Text),
    //             new Guid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblOrderLineidEdit")).Text),
    //             General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantityEdit")).Text),
    //             General.GetNullableInteger(((UserControlUnit)_gridView.Rows[nCurrentRow].FindControl("ucUnitEdit")).SelectedUnit),
    //             General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtUnitPriceEdit")).Text),
    //             General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtGrossPriceEdit")).Text),
    //             General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtDiscountEdit")).Text),
    //             General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtNetPriceEdit")).Text),
    //             (byte?)General.GetNullableInteger(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkOwnerExclude")).Checked ? "1" : "0"),
    //             General.GetNullableString(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlResponsibility")).Text)
    //             );

    //        _gridView.EditIndex = -1;
    //        BindDataProjectLineItem();
    //       // SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvProjectLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    BindDataProjectLineItem();
    //    //  SetPageNavigator();
    //}

    //protected void gvProjectLineItem_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            DataRowView drv = (DataRowView)e.Row.DataItem;
    //            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //            if (db != null)
    //            {
    //                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //                if (drv["FLDJOBDETAILID"].ToString() != string.Empty)
    //                {
    //                    db.Visible = false;

    //                }
    //            }
    //            RadLabel Componentcount = (RadLabel)e.Row.FindControl("lblComponent");
    //            if (drv["FLDCOMPONENTCOUNT"].ToString() == "0")
    //                Componentcount.Text = "";
    //            DropDownList ddlresponsibility = (DropDownList)e.Row.FindControl("ddlResponsibility");
    //            if (ddlresponsibility != null)
    //            {
    //                ddlresponsibility.SelectedValue = drv["FLDMULTISPECID"].ToString();
    //            }
    //            ImageButton imgEdit = (ImageButton)e.Row.FindControl("cmdEdit");
    //            if (imgEdit != null)
    //            {
    //                if (!SessionUtil.CanAccess(this.ViewState, imgEdit.CommandName)) imgEdit.Visible = false;
    //            }
    //            RadLabel lbljoddetailid = (RadLabel)e.Row.FindControl("lblJobDetailid");
    //            RadLabel lblDescription = (RadLabel)e.Row.FindControl("lblDescription");
    //            RadLabel lblSerialno = (RadLabel)e.Row.FindControl("lblSerialno");
    //            if (lbljoddetailid.Text == "")
    //            {
    //                //lblDescription.Font.Bold = true;
    //                //imgEdit.Visible = false;
    //                e.Row.CssClass = "datagrid_footerstyle";
    //            }
    //            UserControlUnit ucUnit = (UserControlUnit)e.Row.FindControl("ucUnitEdit");
    //            if (ucUnit != null) ucUnit.SelectedUnit = drv["FLDUNIT"].ToString();
    //            if (drv["FLDSTATUS"].ToString() == "0")
    //                e.Row.ForeColor = System.Drawing.Color.Red;

    //            HtmlImage img = (HtmlImage)e.Row.FindControl("imgManagerRemarks");
    //            if (img != null)
    //            {
    //                img.Attributes.Add("onclick", "Openpopup('MoreInfo','','DryDockRemarks.aspx?manager=1&orderid=" + drv["FLDORDERID"].ToString() + "&orderlineid=" + drv["FLDORDERLINEID"].ToString() + "&vslid=" + drv["FLDVESSELID"].ToString() + "','xlarge')");
    //                if (drv["FLDMANAGERREMARKS"].ToString().Trim().Equals("")) img.Src = Session["images"] + "/no-remarks.png";
    //            }
    //            img = (HtmlImage)e.Row.FindControl("imgOwnerRemarks");
    //            if (img != null)
    //            {
    //                img.Attributes.Add("onclick", "Openpopup('MoreInfo','','DryDockRemarks.aspx?owner=1&orderid=" + drv["FLDORDERID"].ToString() + "&orderlineid=" + drv["FLDORDERLINEID"].ToString() + "&vslid=" + drv["FLDVESSELID"].ToString() + "','xlarge')");
    //                if (drv["FLDOWNERREMARKS"].ToString().Trim().Equals("")) img.Src = Session["images"] + "/no-remarks.png";
    //            }
    //            img = (HtmlImage)e.Row.FindControl("imgSuptRemarks");
    //            if (img != null)
    //            {
    //                img.Attributes.Add("onclick", "Openpopup('MoreInfo','','DryDockRemarks.aspx?supt=1&orderid=" + drv["FLDORDERID"].ToString() + "&orderlineid=" + drv["FLDORDERLINEID"].ToString() + "&vslid=" + drv["FLDVESSELID"].ToString() + "','xlarge')");
    //                if (drv["FLDSUPTREMARKS"].ToString().Trim().Equals("")) img.Src = Session["images"] + "/no-remarks.png";
    //            }
    //        }

    //        if (e.Row.RowState == DataControlRowState.Edit || (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate)))
    //        {

    //            DropDownList ddl = (DropDownList)e.Row.FindControl("ddlResponsibility");
    //            string jobdetailid = ((RadLabel)e.Row.FindControl("lblJobDetailid")).Text;

    //            if (jobdetailid == null || jobdetailid == "") { ddl.Enabled = true; } else { ddl.Enabled = false; }

    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void gvProjectLineItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        PhoenixDryDockOrder.DeleteDryDockOrderLine(int.Parse(ViewState["VSLID"].ToString()),
    //             new Guid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblOrderid")).Text),
    //             new Guid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblJobId")).Text)
    //            );

    //        _gridView.EditIndex = -1;
    //        BindDataProjectLineItem();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    private void SetRowSelection()
    {

        foreach (GridDataItem item in gvProjectLineItem.MasterTableView.Items)
        {
            if (gvProject.MasterTableView.Items[0].GetDataKeyValue("JOBID").ToString().Equals(ViewState[""].ToString()))
            {

                ViewState["DTKEY"] = ((RadLabel)item.FindControl("lbldtkey")).Text;
            }
        }
    }
    private void SetProjectRowSelection()
    {
        gvProject.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvProject.Items)
        {
            if (item.GetDataKeyValue("FLDORDERID").ToString() == ViewState["ORDERID"].ToString())
            {
                gvProject.SelectedIndexes.Add(item.ItemIndex);
                ViewState["VSLID"] = ((RadLabel)item.FindControl("lblVesselId")).Text;
                item.Selected = true;
                gvProjectLineItem.Rebind();
            }
            //    // long Id1 = (long)gvProject.MasterTableView.Items[0].GetDataKeyValue("Id1");
            //    foreach (GridDataItem item in rgvForm.Items)
            //{

            //    if (item.GetDataKeyValue("FLDORDERID").ToString() == ViewState["orderid"].ToString())
            //    {
            //        // rgvForm.SelectedIndexes.Add(item.ItemIndex);
            //        //PhoenixPurchaseOrderForm.FormNumber = item.GetDataKeyValue("FLDFORMNO").ToString();
            //        //Filter.CurrentPurchaseVesselSelection = int.Parse(item.GetDataKeyValue("FLDVESSELID").ToString());
            //        //Filter.CurrentPurchaseStockType = item.GetDataKeyValue("FLDSTOCKTYPE").ToString();
            //        //ViewState["DTKEY"] = (item["Budget"].FindControl("lbldtkey") as Label).Text;
            //        //Filter.CurrentPurchaseStockClass = (item["Budget"].FindControl("lblStockId") as Label).Text;
            //        //BindSendDate();
            //    }

            //}

            //if (gvProject.MasterTableView.Items[0].GetDataKeyValue("FLDORDERID").Equals(Filter.CurrentSelectedDryDockProject))
            //{

            //    ViewState["VSLID"] = ((RadLabel)item.FindControl("lblVesselId")).Text;
            //    item.Selected = true;
            //    gvProjectLineItem.Rebind();

            //}
        }
    }



    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    try
    //    {
    //        //gvProjectLineItem.SelectedIndex = -1;
    //        //gvProjectLineItem.EditIndex = -1;
    //        if (ce.CommandName == "prev")
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //        else
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //        ViewState["JOBID"] = null;
    //        BindDataProjectLineItem();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvProject_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadGrid g = (RadGrid)sender;
                GridDataItem i = (GridDataItem)e.Item;
                LinkButton lb = (LinkButton)e.Item.FindControl("cmdEdit");
                if (lb != null)
                {
                    lb.Visible = SessionUtil.CanAccess(this.ViewState, lb.CommandName);
                    lb.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/DryDock/DryDockProjectAdd.aspx?vslid=" + DataBinder.Eval(e.Item.DataItem, "FLDVESSELID") + "&ProjectID=" + DataBinder.Eval(e.Item.DataItem, "FLDORDERID").ToString() + "');return false;");
                }


                LinkButton ib = (LinkButton)e.Item.FindControl("cmdExport2XL");
                if (ib != null)
                {
                    ib.Visible = SessionUtil.CanAccess(this.ViewState, ib.CommandName);
                }
                LinkButton sp = (LinkButton)e.Item.FindControl("cmdSpare");
                if (sp != null)
                {
                    sp.Visible = SessionUtil.CanAccess(this.ViewState, sp.CommandName);
                }
                RadLabel lblProjectid = (RadLabel)e.Item.FindControl("lblProjectid");
                LinkButton est = (LinkButton)e.Item.FindControl("cmdEstimate");
                string jscript = "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Options/OptionsDrydock.ashx?methodname=DRYDOCKEXPORT2XL&exportoption=estimate&projectid=" + lblProjectid.Text + "&vslid=" + DataBinder.Eval(e.Item.DataItem, "FLDVESSELID") + "&usercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "'); return false;";

                if (est != null)
                {
                    est.Visible = SessionUtil.CanAccess(this.ViewState, est.CommandName);
                    est.Attributes.Add("onclick", jscript);
                }

                if (Filter.CurrentSelectedDryDockProject != null)
                {
                   
                        if (i.GetDataKeyValue("FLDORDERID").ToString() == Filter.CurrentSelectedDryDockProject)
                        {
                            g.SelectedIndexes.Clear();
                            i.Selected = true;
                        }
                   

                }


                string jvscript = "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Options/OptionsDrydock.ashx?methodname=DRYDOCKEXPORT2XL&exportoption=project&projectid=" + lblProjectid.Text + "&vslid=" + DataBinder.Eval(e.Item.DataItem, "FLDVESSELID") + "&usercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "'); return false;";

                if (ib != null) ib.Attributes.Add("onclick", jvscript);

                LinkButton initiate = (LinkButton)e.Item.FindControl("cmdInitiate");
                if (initiate != null)
                {
                    initiate.Attributes.Add("onclick", "return fnConfirmDelete(event ,'Are you sure you want Initiate Project'); return false;");
                    initiate.Visible = SessionUtil.CanAccess(this.ViewState, initiate.CommandName);
                    if (DataBinder.Eval(e.Item.DataItem, "FLDSTATUS").ToString() == "0")
                    {
                        initiate.Visible = false;
                    }
                }

                LinkButton att = (LinkButton)e.Item.FindControl("cmdAttachment");
                if (att != null)
                {
                    if (DataBinder.Eval(e.Item.DataItem, "FLDISATTACHMENT").ToString() == "0")
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" style=\"color: gray;\"><i class=\"fas fa-paperclip\"></i></span>";
                        att.Controls.Add(html);
                    }
                    att.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + DataBinder.Eval(e.Item.DataItem, "FLDDTKEY") + "&mod="
                                                        + PhoenixModule.DRYDOCK + "'); return false;";
                }

                LinkButton CI = (LinkButton)e.Item.FindControl("linkbtncostinccured");
                if (CI != null)
                {
                    CI.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/DryDock/DrydockCostIncurred.aspx?projectid=" + DataBinder.Eval(e.Item.DataItem, "FLDORDERID") + "&vesselid=" + DataBinder.Eval(e.Item.DataItem, "FLDVESSELID") + "'); return false;";
                }

                LinkButton CS = (LinkButton)e.Item.FindControl("linkbtncostsummary");
                if (CS != null)
                {
                    //CS.Attributes["onclick"] = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=20&reportcode=DRYDOCKCOSTSUMMARY&projectid=" + DataBinder.Eval(e.Item.DataItem, "FLDORDERID")  + "&showmenu=0&showword=NO&showexcel=NO'); return false;";

                    CS.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/DryDock/DrydockCostSummary.aspx?projectid=" + DataBinder.Eval(e.Item.DataItem, "FLDORDERID") + "&vesselid=" + DataBinder.Eval(e.Item.DataItem, "FLDVESSELID") + "','false','350px','170px'); return false;";

                    int yn = 0;
                    PhoenixDryDockOrder.DrydockPOApproved(General.GetNullableGuid(DataBinder.Eval(e.Item.DataItem, "FLDORDERID").ToString()), General.GetNullableInteger(DataBinder.Eval(e.Item.DataItem, "FLDVESSELID").ToString()), ref yn);
                    if (yn == 1)
                        CS.Visible = true;
                }


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProject_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvProject_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("PAGE"))
            { return; }

            RadLabel l = (RadLabel)e.Item.FindControl("lblProjectid");
            RadLabel vsl = (RadLabel)e.Item.FindControl("lblVesselId");

            if (e.CommandName.ToUpper().Equals("SPARE"))
            {
                Response.Redirect("../DryDock/DryDockPurchase.aspx?vslid=" + vsl.Text + "&projectid=" + l.Text, false);
                return;
            }
            //if (e.CommandName.ToUpper().Equals("ESTIMATE"))
            //{
            //    PhoenixDryDock2XL.Export2XLDryDockEstimate(new Guid(l.Text), int.Parse(vsl.Text));
            //    return;
            //}
            if (e.CommandName.ToUpper().Equals("INITIATE"))
            {
                PhoenixDryDockOrder.CreateStandardJobWorkRequest(new Guid(l.Text), int.Parse(vsl.Text));
                PhoenixDryDockOrder.InitiateDryDockOrder(new Guid(l.Text), int.Parse(vsl.Text));
                gvProject.Rebind();
                return;
            }
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["SELECTEDITEM"] = "1";
                
            }
            Filter.CurrentSelectedDryDockProject = l.Text;
            ViewState["ORDERID"] = l.Text;
            ViewState["VSLID"] = vsl.Text;
            gvProjectLineItem.Rebind();

            //gvProject.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProjectLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataProjectLineItem();
    }

    protected void gvProjectLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            string jobdetailid = "";
            if (e.Item is GridDataItem && !e.Item.IsInEditMode)
            {

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    RadLabel budgetcode = (RadLabel)e.Item.FindControl("lblbudgetcode");

                    if (DataBinder.Eval(e.Item.DataItem, "FLDJOBDETAILID").ToString() != string.Empty)
                    {
                        db.Visible = false;
                        budgetcode.Visible = false;

                    }
                }

                RadLabel Componentcount = (RadLabel)e.Item.FindControl("lblComponent");
                if (DataBinder.Eval(e.Item.DataItem, "FLDCOMPONENTCOUNT").ToString() == "0")
                    Componentcount.Text = "";
                RadDropDownList ddlresponsibility = (RadDropDownList)e.Item.FindControl("ddlResponsibility");
                if (ddlresponsibility != null)
                {
                    ddlresponsibility.SelectedValue = DataBinder.Eval(e.Item.DataItem, "FLDMULTISPECID").ToString();
                }


                LinkButton imgEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (imgEdit != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgEdit.CommandName)) imgEdit.Visible = false;
                }
                RadLabel lbljoddetailid = (RadLabel)e.Item.FindControl("lblJobDetailid");
                RadLabel lblDescription = (RadLabel)e.Item.FindControl("lblDescription");
                RadLabel lblSerialno = (RadLabel)e.Item.FindControl("lblSerialno");
                //if (lbljoddetailid.Text == "")
                //{
                //    //lblDescription.Font.Bold = true;
                //    //imgEdit.Visible = false;
                //    e.Item.CssClass = "datagrid_footerstyle";
                //}
                UserControlUnit ucUnit = (UserControlUnit)e.Item.FindControl("ucUnitEdit");
                if (ucUnit != null) ucUnit.SelectedUnit = DataBinder.Eval(e.Item.DataItem, "FLDUNIT").ToString();
                if (DataBinder.Eval(e.Item.DataItem, "FLDSTATUS").ToString() == "0")
                    e.Item.ForeColor = System.Drawing.Color.Red;

                HtmlGenericControl html = new HtmlGenericControl();


                LinkButton img = (LinkButton)e.Item.FindControl("imgManagerRemarks");
                if (img != null)
                {
                    img.Attributes.Add("onclick", "openNewWindow('MoreInfo','','" + Session["sitepath"] + "/DryDock/DryDockRemarks.aspx?manager=1&orderid=" + DataBinder.Eval(e.Item.DataItem, "FLDORDERID").ToString() + "&orderlineid=" + DataBinder.Eval(e.Item.DataItem, "FLDORDERLINEID").ToString() + "&vslid=" + DataBinder.Eval(e.Item.DataItem, "FLDVESSELID").ToString() + "','xlarge')");
                    if (DataBinder.Eval(e.Item.DataItem, "FLDMANAGERREMARKS").ToString().Trim().Equals(""))
                    {
                        html.InnerHtml = "<span class=\"icon\" style=\"color: gray;\"><i class=\"fas fa-glasses\"></i></span>";
                        img.Controls.Add(html);
                    }
                }
                img = (LinkButton)e.Item.FindControl("imgOwnerRemarks");
                if (img != null)
                {
                    img.Attributes.Add("onclick", "openNewWindow('MoreInfo','','" + Session["sitepath"] + "/DryDock/DryDockRemarks.aspx?owner=1&orderid=" + DataBinder.Eval(e.Item.DataItem, "FLDORDERID").ToString() + "&orderlineid=" + DataBinder.Eval(e.Item.DataItem, "FLDORDERLINEID").ToString() + "&vslid=" + DataBinder.Eval(e.Item.DataItem, "FLDVESSELID").ToString() + "','xlarge')");
                    if (DataBinder.Eval(e.Item.DataItem, "FLDOWNERREMARKS").ToString().Trim().Equals(""))
                    {
                        html.InnerHtml = "<span class=\"icon\" style=\"color: gray;\"><i class=\"fas fa-glasses\"></i></span>";
                        img.Controls.Add(html);
                    }
                }
                img = (LinkButton)e.Item.FindControl("imgSuptRemarks");
                if (img != null)
                {
                    img.Attributes.Add("onclick", "openNewWindow('MoreInfo','','" + Session["sitepath"] + "/DryDock/DryDockRemarks.aspx?supt=1&orderid=" + DataBinder.Eval(e.Item.DataItem, "FLDORDERID").ToString() + "&orderlineid=" + DataBinder.Eval(e.Item.DataItem, "FLDORDERLINEID").ToString() + "&vslid=" + DataBinder.Eval(e.Item.DataItem, "FLDVESSELID").ToString() + "','xlarge')");
                    if (DataBinder.Eval(e.Item.DataItem, "FLDSUPTREMARKS").ToString().Trim().Equals(""))
                    {
                        html.InnerHtml = "<span class=\"icon\" style=\"color: gray;\"><i class=\"fas fa-glasses\"></i></span>";
                        img.Controls.Add(html);
                    }
                }
                
            }
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                jobdetailid = ((RadLabel)e.Item.FindControl("lblJobDetailidedit")).Text;
                Guid? orderid = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOrderidedit")).Text);
                RadDropDownList ddl = (RadDropDownList)e.Item.FindControl("ddlResponsibility");
                ddl.SelectedValue = DataBinder.Eval(e.Item.DataItem, "FLDMULTISPECID").ToString();
                RadDropDownList ddl1 = (RadDropDownList)e.Item.FindControl("ddlbudgetcode");
                ddl1.DataSource = PhoenixDryDockOrder.DrydockBudgetcodeList(orderid);
                ddl1.DataBind();
                ddl1.SelectedValue = DataBinder.Eval(e.Item.DataItem, "FLDBUDGETID").ToString();
                if (jobdetailid == null || jobdetailid == "")
                { if (ddl != null) ddl.Enabled = true;
                    if (ddl1 != null) ddl1.Enabled = true;
                }
                else
                {
                    if (ddl != null) ddl.Enabled = false;
                    if (ddl1 != null) ddl1.Enabled = false;

                   
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProjectLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try {
            if (e.CommandName.ToString().Equals("DELETE"))
            {
                PhoenixDryDockOrder.DeleteDryDockOrderLine(int.Parse(ViewState["VSLID"].ToString()),
                     new Guid(((RadLabel)e.Item.FindControl("lblOrderid")).Text),
                     new Guid(((RadLabel)e.Item.FindControl("lblJobId")).Text)
                    );

                gvProject.Rebind();
                gvProjectLineItem.Rebind();
                //SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProjectLineItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {


            PhoenixDryDockOrder.UpdateDryDockOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 int.Parse(ViewState["VSLID"].ToString()),
                 new Guid(((RadLabel)e.Item.FindControl("lblOrderidEdit")).Text),
                 new Guid(((RadLabel)e.Item.FindControl("lblOrderLineidEdit")).Text),
                 General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("txtQuantityEdit")).Text),
                 General.GetNullableInteger(((UserControlUnit)e.Item.FindControl("ucUnitEdit")).SelectedUnit),
                 General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("txtUnitPriceEdit")).Text),
                 General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("txtGrossPriceEdit")).Text),
                 General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("txtDiscountEdit")).Text),
                 General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("txtNetPriceEdit")).Text),
                 (byte?)General.GetNullableInteger(((CheckBox)e.Item.FindControl("chkOwnerExclude")).Checked ? "1" : "0"),
                 General.GetNullableString(((RadDropDownList)e.Item.FindControl("ddlResponsibility")).SelectedValue),
                 General.GetNullableInteger(((RadDropDownList)e.Item.FindControl("ddlbudgetcode")).SelectedValue),
                 General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("txtapprovedpriceedit")).Text) );

            gvProjectLineItem.Rebind();

            gvProject.Rebind();

            //BindDataProjectLineItem();
            // SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvProject_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    gvProjectLineItem.Rebind();
    //}


}

