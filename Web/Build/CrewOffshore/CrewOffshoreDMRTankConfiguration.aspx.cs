using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewOffshoreDMRTankConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRTankConfiguration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvTankConfiguration')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuLocation.AccessRights = this.ViewState;
            MenuLocation.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
               
                gvTankConfiguration.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindData();
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuLocation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                //gvTankConfiguration.EditIndex = -1;
                //gvTankConfiguration.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                //SetPageNavigator();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvTankConfiguration_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

           
    //        if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            PhoenixCrewOffshoreDMRTankConfiguration.DeleteDMRTankConfiguration(
    //                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //                new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblTankPlanConfigurationid")).Text));
    //        }

    //        //SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    private bool checkvalue( string capacityat100,string product)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((capacityat100 == null) || (capacityat100 == ""))
            ucError.ErrorMessage = "Capacity at 100% is required.";

        if (General.GetNullableGuid(product)==null)
            ucError.ErrorMessage = "Product is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDLOCATIONNAME", "FLDUNITNAME", "FLDCAPACITYAT100PERCENT", "FLDCAPACITYAT85PERCENT", "FLDPRODUCTNAME", "FLDSHOWTANKYN" };
        string[] alCaptions = { "Tank", "Unit", "Capacity at 100%","Capacity at 85%","Product","Show In Tank Plan" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewOffshoreDMRTankConfiguration.DMRTankConfigurationSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            sortexpression, sortdirection, gvTankConfiguration.CurrentPageIndex+1,
            gvTankConfiguration.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvTankConfiguration", "Tank Configuration", alCaptions, alColumns, ds);
        gvTankConfiguration.DataSource = ds;
        // gvTankConfiguration.DataBind();
        gvTankConfiguration.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
       // SetPageNavigator();
    }

    //protected void gvTankConfiguration_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

    //        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

    //        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
    //        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

    //        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
    //        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

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

    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {

    //            DataRowView drv = (DataRowView)e.Row.DataItem;

    //            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //            {
    //                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //            }
             
    //            UserControlDMRProduct product = (UserControlDMRProduct)e.Row.FindControl("ucProductEdit");
    //            if (product != null)
    //            {
    //                product.ProductList = PhoenixRegistersDMROilType.ListProduct(General.GetNullableGuid(drv["FLDPRODUCTTYPEID"].ToString())); 
    //                product.SelectedProduct = drv["FLDPRODUCTID"].ToString();
    //            }
    //        }
    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //        {
    //            Label lblVessel = (Label)e.Row.FindControl("lblVessel");

    //            ImageButton ImgVesselList = (ImageButton)e.Row.FindControl("ImgVesselList");

    //            if (ImgVesselList != null)
    //            {
    //                if (lblVessel != null)
    //                {
    //                    if (lblVessel.Text != "")
    //                    {
    //                        ImgVesselList.Visible = true;
    //                        UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucVesselList");
    //                        if (uct != null)
    //                        {
    //                            ImgVesselList.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
    //                            ImgVesselList.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
    //                        }
    //                    }
    //                    else
    //                        ImgVesselList.Visible = false;
    //                }
    //            }

    //            Label lblLocationId = (Label)e.Row.FindControl("lblLocationId");
    //            Label lblLocationName = (Label)e.Row.FindControl("lblLocationName");
    //            ImageButton cmdTankHistory = (ImageButton)e.Row.FindControl("cmdTankHistory");
    //            if (cmdTankHistory != null)
    //            {
    //                cmdTankHistory.Attributes.Add("onclick", "parent.Openpopup('TankHistory', '', '../CrewOffshore/CrewOffshoreDMRTankConfigurationHistory.aspx?LocationId=" + lblLocationId.Text + "&LocationName=" + lblLocationName.Text+ "');return false;");
    //            }
    //        }
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            DataRowView drv = (DataRowView)e.Row.DataItem;
    //            CheckBoxList chkVesselListEdit = (CheckBoxList)e.Row.FindControl("chkVesselListEdit");
    //            if (chkVesselListEdit != null)
    //            {
    //                chkVesselListEdit.DataSource = PhoenixRegistersDMRTankPlanLocation.ListVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    //                chkVesselListEdit.DataTextField = "FLDVESSELNAME";
    //                chkVesselListEdit.DataValueField = "FLDVESSELID";
    //                chkVesselListEdit.DataBind();

    //                CheckBoxList chk = (CheckBoxList)e.Row.FindControl("chkVesselListEdit");
    //                foreach (ListItem li in chk.Items)
    //                {
    //                    string[] slist = drv["FLDVESSELLIST"].ToString().Split(',');
    //                    foreach (string s in slist)
    //                    {
    //                        if (li.Value.Equals(s))
    //                        {
    //                            li.Selected = true;
    //                        }
    //                    }
    //                }
    //            }

    //        }
    //        if (e.Row.RowType == DataControlRowType.Footer)
    //        {
    //            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
    //            if (db != null)
    //            {
    //                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
    //                    db.Visible = false;
    //            }
    //            CheckBoxList chkVesselListAdd = (CheckBoxList)e.Row.FindControl("chkVesselListAdd");
    //            if (chkVesselListAdd != null)
    //            {
    //                chkVesselListAdd.DataSource = PhoenixRegistersDMRTankPlanLocation.ListVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    //                chkVesselListAdd.DataTextField = "FLDVESSELNAME";
    //                chkVesselListAdd.DataValueField = "FLDVESSELID";
    //                chkVesselListAdd.DataBind();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}


    //protected void gvTankConfiguration_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}

    protected void gvTankConfiguration_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvTankConfiguration, "Edit$" + e.Row.RowIndex.ToString(), false);
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gv = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

                TableCell tbf = new TableCell();
                TableCell tbCapacity = new TableCell();
                TableCell tbproduct = new TableCell();
              

                tbf.ColumnSpan = 2;
                tbCapacity.ColumnSpan = 2;
                tbproduct.ColumnSpan = 3;
                

                tbCapacity.Text = "Capacity";

                tbCapacity.Attributes.Add("style", "text-align:center;");
                

                gv.Cells.Add(tbf);
                gv.Cells.Add(tbCapacity);
                gv.Cells.Add(tbproduct);
            

                gvTankConfiguration.Controls[0].Controls.AddAt(0, gv);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvTankConfiguration_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        _gridView.EditIndex = de.NewEditIndex;
    //        _gridView.SelectedIndex = de.NewEditIndex;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvTankConfiguration_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        if (!checkvalue((((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("ucCapacityat100PercentageEdit")).Text),
    //            ((UserControlDMRProduct)_gridView.Rows[nCurrentRow].FindControl("ucProductEdit")).SelectedProduct))
    //            return;


    //        PhoenixCrewOffshoreDMRTankConfiguration.DMRTankConfigurationUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //            PhoenixSecurityContext.CurrentSecurityContext.VesselID,
    //            General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblTankPlanConfigurationidEdit")).Text),
    //            new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblLocationId")).Text),
    //            null,
    //            General.GetNullableDecimal(((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("ucCapacityat100PercentageEdit")).Text),
    //            General.GetNullableGuid(((UserControlDMRProduct)_gridView.Rows[nCurrentRow].FindControl("ucProductEdit")).SelectedProduct),
    //            ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkShowYN")).Checked ? 1 : 0
    //            );

    //        _gridView.EditIndex = -1;
    //        BindData();
    //        //SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvTankConfiguration_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    BindData();
    //   // SetPageNavigator();
    //}

    protected void gvTankConfiguration_Sorting(object sender, GridViewSortEventArgs se)
    {
       // gvTankConfiguration.EditIndex = -1;
        //gvTankConfiguration.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    //gvTankConfiguration.SelectedIndex = -1;
    //    //gvTankConfiguration.EditIndex = -1;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //    //SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    //gvTankConfiguration.SelectedIndex = -1;
    //    //gvTankConfiguration.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    gvTankConfiguration.SelectedIndex = -1;
    //    gvTankConfiguration.EditIndex = -1;
    //    ViewState["PAGENUMBER"] = 1;
    //    BindData();
    //    SetPageNavigator();
    //}

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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDLOCATIONNAME", "FLDUNITNAME", "FLDCAPACITYAT100PERCENT", "FLDCAPACITYAT85PERCENT", "FLDPRODUCTNAME", "FLDSHOWTANKYN" };
        string[] alCaptions = { "Tank", "Unit", "Capacity at 100%", "Capacity at 85%", "Product", "Show In Tank Plan" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewOffshoreDMRTankConfiguration.DMRTankConfigurationSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"Tank Configuration.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Tank Configuration</h3></td>");
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

    //protected void gvTankConfiguration_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //    {
    //        Label lblVessel = (Label)e.Row.FindControl("lblVessel");

    //        ImageButton ImgVesselList = (ImageButton)e.Row.FindControl("ImgVesselList");

    //        if (ImgVesselList != null)
    //        {
    //            if (lblVessel != null)
    //            {
    //                if (lblVessel.Text != "")
    //                {
    //                    ImgVesselList.Visible = true;
    //                    UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucVesselList");
    //                    if (uct != null)
    //                    {
    //                        ImgVesselList.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
    //                        ImgVesselList.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
    //                    }
    //                }
    //                else
    //                    ImgVesselList.Visible = false;
    //            }
    //        }
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        CheckBoxList chkVesselListEdit = (CheckBoxList)e.Row.FindControl("chkVesselListEdit");
    //        if (chkVesselListEdit != null)
    //        {
    //            chkVesselListEdit.DataSource = PhoenixRegistersDMRTankPlanLocation.ListVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    //            chkVesselListEdit.DataTextField = "FLDVESSELNAME";
    //            chkVesselListEdit.DataValueField = "FLDVESSELID";
    //            chkVesselListEdit.DataBind();

    //            CheckBoxList chk = (CheckBoxList)e.Row.FindControl("chkVesselListEdit");
    //            foreach (ListItem li in chk.Items)
    //            {
    //                string[] slist = drv["FLDVESSELLIST"].ToString().Split(',');
    //                foreach (string s in slist)
    //                {
    //                    if (li.Value.Equals(s))
    //                    {
    //                        li.Selected = true;
    //                    }
    //                }
    //            }
    //        }
    //        UserControlDMRProduct ucProductEdit = (UserControlDMRProduct)e.Row.FindControl("ucProductEdit");
    //        if (ucProductEdit != null)
    //            ucProductEdit.SelectedProduct = drv["FLDOILTYPECODE"].ToString();

    //    }
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {

    //        CheckBoxList chkVesselListAdd = (CheckBoxList)e.Row.FindControl("chkVesselListAdd");
    //        if (chkVesselListAdd != null)
    //        {
    //            chkVesselListAdd.DataSource = PhoenixRegistersDMRTankPlanLocation.ListVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    //            chkVesselListAdd.DataTextField = "FLDVESSELNAME";
    //            chkVesselListAdd.DataValueField = "FLDVESSELID";
    //            chkVesselListAdd.DataBind();
    //        }
    //    }
    //}

    protected void gvTankConfiguration_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvTankConfiguration_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
           // GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());


            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixCrewOffshoreDMRTankConfiguration.DeleteDMRTankConfiguration(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((RadLabel)e.Item.FindControl("lblTankPlanConfigurationid")).Text));
            }
            gvTankConfiguration.Rebind();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTankConfiguration_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            LinkButton  edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

     

            if (e.Item is GridDataItem)
            {

                // DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                UserControlDMRProduct product = (UserControlDMRProduct)e.Item.FindControl("ucProductEdit");
                if (product != null)
                {
                    GridEditableItem eeditedItem = e.Item as GridEditableItem;
                    string FLDPRODUCTTYPEID = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDPRODUCTTYPEID"].ToString();
                    product.ProductList = PhoenixRegistersDMROilType.ListProduct(General.GetNullableGuid(FLDPRODUCTTYPEID));
                    product.SelectedProduct = FLDPRODUCTTYPEID;
                }
            }
            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            //{
                RadLabel lblVessel = (RadLabel)e.Item.FindControl("lblVessel");

            LinkButton ImgVesselList = (LinkButton)e.Item.FindControl("ImgVesselList");

                if (ImgVesselList != null)
                {
                    if (lblVessel != null)
                    {
                        if (lblVessel.Text != "")
                        {
                            ImgVesselList.Visible = true;
                            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucVesselList");
                            if (uct != null)
                            {
                                ImgVesselList.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                                ImgVesselList.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                            }
                        }
                        else
                            ImgVesselList.Visible = false;
                    }
                }

                RadLabel lblLocationId = (RadLabel)e.Item.FindControl("lblLocationId");
                RadLabel lblLocationName = (RadLabel)e.Item.FindControl("lblLocationName");
            LinkButton cmdTankHistory = (LinkButton)e.Item.FindControl("cmdTankHistory");
                if (cmdTankHistory != null)
                {
                cmdTankHistory.Attributes.Add("onclick", "javascript:openNewWindow('TankHistory','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRTankConfigurationHistory.aspx?LocationId=" + lblLocationId.Text + "&LocationName=" + lblLocationName.Text + "'); return false;");

                //cmdTankHistory.Attributes.Add("onclick", "parent.Openpopup('TankHistory', '', '../CrewOffshore/CrewOffshoreDMRTankConfigurationHistory.aspx?LocationId=" + lblLocationId.Text + "&LocationName=" + lblLocationName.Text + "');return false;");
                }
            //}
            if (e.Item is GridDataItem )
            {
                // DataRowView drv = (DataRowView)e.Item.DataItem;
                GridDataItem item = (GridDataItem)e.Item;
               // string s = ((DataRowView)e.Item.DataItem)["FLDVESSELLIST"].ToString();
                CheckBoxList chkVesselListEdit = (CheckBoxList)e.Item.FindControl("chkVesselListEdit");
                if (chkVesselListEdit != null)
                {
                    chkVesselListEdit.DataSource = PhoenixRegistersDMRTankPlanLocation.ListVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                    chkVesselListEdit.DataTextField = "FLDVESSELNAME";
                    chkVesselListEdit.DataValueField = "FLDVESSELID";
                    chkVesselListEdit.DataBind();

                    CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkVesselListEdit");
                    foreach (ListItem li in chk.Items)
                    {
                        string[] slist = ((DataRowView)e.Item.DataItem)["FLDVESSELLIST"].ToString().Split(',');
                        foreach (string s in slist)
                        {
                            if (li.Value.Equals(s))
                            {
                                li.Selected = true;
                            }
                        }
                    }
                }

            }
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            //    if (db != null)
            //    {
            //        if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
            //            db.Visible = false;
            //    }
            //    CheckBoxList chkVesselListAdd = (CheckBoxList)e.Row.FindControl("chkVesselListAdd");
            //    if (chkVesselListAdd != null)
            //    {
            //        chkVesselListAdd.DataSource = PhoenixRegistersDMRTankPlanLocation.ListVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            //        chkVesselListAdd.DataTextField = "FLDVESSELNAME";
            //        chkVesselListAdd.DataValueField = "FLDVESSELID";
            //        chkVesselListAdd.DataBind();
            //    }
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTankConfiguration_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            //RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.RowIndex;

            if (!checkvalue((((UserControlMaskedTextBox)eeditedItem.FindControl("ucCapacityat100PercentageEdit")).TextWithLiterals),
                ((UserControlDMRProduct)eeditedItem.FindControl("ucProductEdit")).SelectedProduct))
                return;


            PhoenixCrewOffshoreDMRTankConfiguration.DMRTankConfigurationUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                General.GetNullableGuid(((RadLabel)eeditedItem.FindControl("lblTankPlanConfigurationidEdit")).Text),
                new Guid(((RadLabel)eeditedItem.FindControl("lblLocationId")).Text),
                null,
                General.GetNullableDecimal(((UserControlMaskedTextBox)eeditedItem.FindControl("ucCapacityat100PercentageEdit")).TextWithLiterals),
                General.GetNullableGuid(((UserControlDMRProduct)eeditedItem.FindControl("ucProductEdit")).SelectedProduct),
                ((CheckBox)eeditedItem.FindControl("chkShowYN")).Checked ? 1 : 0
                );

           // _gridView.EditIndex = -1;
            BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
