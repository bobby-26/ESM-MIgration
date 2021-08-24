using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersVesselMedicalTestMap : PhoenixBasePage
{
	protected override void Render(HtmlTextWriter writer)
	{
		foreach (GridViewRow r in gvDocumentsRequired.Rows)
		{
			if (r.RowType == DataControlRowType.DataRow)
			{
				Page.ClientScript.RegisterForEventValidation(gvDocumentsRequired.UniqueID, "Edit$" + r.RowIndex.ToString());
			}
		}		
		base.Render(writer);
	}
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersVesselMedicalTestMap.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvDocumentsRequired')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersDocumentsRequired.AccessRights = this.ViewState;
            MenuRegistersDocumentsRequired.MenuList = toolbar.Show();
            //MenuRegistersDocumentsRequired.SetTrigger(pnlDocumentsRequiredEntry);            

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
          
			toolbar = new PhoenixToolbar();
            toolbar.AddButton("Course", "COURSE");
			toolbar.AddButton("Medical", "MEDICAL");
            DataSet dsVessel = PhoenixRegistersVessel.EditVessel(Int16.Parse(Filter.CurrentVesselMasterFilter));
            DataRow drVessel = dsVessel.Tables[0].Rows[0];
			txtVesselName.Text= drVessel["FLDVESSELNAME"].ToString();
            ucVesselType.SelectedVesseltype = drVessel["FLDTYPE"].ToString();
            ucVesselType.Enabled = false;
            ucMedicals.SelectedHard = drVessel["FLDMEDICALREQUIRED"].ToString();
            ucMedicals.Enabled = false;
            MenuFlag.AccessRights = this.ViewState;
            MenuFlag.MenuList = toolbar.Show();
            MenuFlag.SelectedMenuIndex = 1;
			
        }
		BindData();
		SetPageNavigator();
    }
	//protected void DocumentTypeSelection(object sender, EventArgs e)
	//{
	//    ViewState["DOCTYPE"] = ucDocumentType.SelectedHard;
	//    BindCourse();
	//}
	//protected void BindCourse()
	//{
		
	//    BindData();
	//}
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCODE", "FLDNAMEOFMEDICAL"};
		string[] alCaptions = { "Code", "Medical Test"};
       

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		 ds = PhoenixRegistersDocumentMedical.VesselMedicalSearch(General.GetNullableInteger(ucRank.SelectedRank),General.GetNullableInteger(ucVesselType.SelectedVesseltype)
			,Int32.Parse(ViewState["PAGENUMBER"].ToString()),
			iRowCount,
			ref iRowCount,
			ref iTotalPageCount,
            General.GetNullableInteger(ucMedicals.SelectedHard)
			);

        Response.AddHeader("Content-Disposition", "attachment; filename=MedicalTest.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Medical Test</h3></td>");
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

    protected void RegistersDocumentsRequired_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCODE", "FLDNAMEOFMEDICAL" };
        string[] alCaptions = { "Code", "Medical Test" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersDocumentMedical.VesselMedicalSearch(General.GetNullableInteger(ucRank.SelectedRank), General.GetNullableInteger(ucVesselType.SelectedVesseltype)
            , Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(ucMedicals.SelectedHard)
            );


        General.SetPrintOptions("gvDocumentsRequired", "Medical Test", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDocumentsRequired.DataSource = ds;
            gvDocumentsRequired.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvDocumentsRequired);
        }
		
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void Flag_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        UserControlTabs ucTabs = (UserControlTabs)sender;

        if (dce.CommandName.ToUpper().Equals("COURSE"))
        {
            Response.Redirect("../Registers/RegistersDocumentRequiredCourse.aspx?launchedfrom=" + Request.QueryString["launchedfrom"].ToString());
        }
        else if (dce.CommandName.ToUpper().Equals("MEDICAL"))
        {
            Response.Redirect("../Registers/RegistersVesselMedicalTestMap.aspx?launchedfrom=" + Request.QueryString["launchedfrom"].ToString());
        }
        else if (dce.CommandName.ToUpper().Equals("CORRESPONDENCE"))
        {
            Response.Redirect("../Registers/RegistersVesselCorrespondence.aspx", false);
        }
        else if (dce.CommandName.ToUpper().Equals("CHATBOX"))
        {
            Response.Redirect("../Registers/RegistersVesselChatBox.aspx?vesselid=" + Filter.CurrentVesselMasterFilter, false);
        }   
    }

    protected void gvDocumentsRequired_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["CURRENTROW"] = -1;
        BindData();
    }

    protected void gvDocumentsRequired_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDocumentsRequired, "Edit$" + e.Row.RowIndex.ToString(), false);            
        }
    }

    protected void gvDocumentsRequired_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			if (e.CommandName.ToUpper().Equals("ADD"))
			{
                if (!IsValidDocumentsRequired(
                                     ((DropDownList)_gridView.FooterRow.FindControl("ddlMedicalTestAdd")).SelectedValue
                                    , ((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucExpiryPeriodAdd")).Text
                                    , ((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucAgefromAdd")).Text
                                    , ((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucAgeToAdd")).Text
                                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersDocumentMedical.InsertVesselMedicalTest(int.Parse(((DropDownList)_gridView.FooterRow.FindControl("ddlMedicalTestAdd")).SelectedValue)
                                                    , int.Parse(ucRank.SelectedRank)
                                                    , int.Parse(ucVesselType.SelectedVesseltype)
                                                    , null
                                                    ,int.Parse(ucMedicals.SelectedHard)
                                                    , General.GetNullableInteger(((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucExpiryPeriodAdd")).Text)
                                                    , General.GetNullableInteger(((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucAgefromAdd")).Text)
                                                    , General.GetNullableInteger(((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucAgeToAdd")).Text));

				BindData();
			}
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDocumentsRequired(
                                    ((Label)_gridView.Rows[nCurrentRow].FindControl("lblMedicalTestId")).Text
                                    , ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucExpiryPeriodEdit")).Text
                                    , ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAgefromEdit")).Text
                                    , ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAgeToEdit")).Text
                                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersDocumentMedical.InsertVesselMedicalTest(int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMedicalTestId")).Text)
                                                    , int.Parse(ucRank.SelectedRank)
                                                    , int.Parse(ucVesselType.SelectedVesseltype)
                                                    , General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselMdicalTestMapid")).Text)
                                                    , int.Parse(ucMedicals.SelectedHard)
                                                    , General.GetNullableInteger(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucExpiryPeriodEdit")).Text)
                                                    , General.GetNullableInteger(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAgefromEdit")).Text)
                                                    , General.GetNullableInteger(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucAgeToEdit")).Text));

                _gridView.EditIndex = -1;
                BindData();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersDocumentMedical.DeleteVesselMedicalTest(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselMdicalTestMapid")).Text));
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDocumentsRequired_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvDocumentsRequired.SelectedIndex = -1;
        gvDocumentsRequired.EditIndex = -1;
        ViewState["CURRENTROW"] = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvDocumentsRequired_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
			ImageButton dbedit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
			if (dbedit != null)
			{
				dbedit.Visible = SessionUtil.CanAccess(this.ViewState, dbedit.CommandName);
			}

            UserControlMaskNumber ucExpiryPeriodEdit = (UserControlMaskNumber)e.Row.FindControl("ucExpiryPeriodEdit");

            if (ucExpiryPeriodEdit != null)
            {
                ucExpiryPeriodEdit.Text = (drv["FLDEXPIRYYN"].ToString() == "1") ? drv["FLDEXPIRYPERIOD"].ToString() : "";
                ucExpiryPeriodEdit.CssClass = (drv["FLDEXPIRYYN"].ToString() == "1") ? "input" : "readonlytextbox";
                ucExpiryPeriodEdit.ReadOnly = (drv["FLDEXPIRYYN"].ToString() == "1") ? false : true;
            }
	
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlMedicalTestAdd = (DropDownList)e.Row.FindControl("ddlMedicalTestAdd");
            ddlMedicalTestAdd.DataSource = PhoenixRegistersDocumentMedical.ListDocumentMedical();
            ddlMedicalTestAdd.DataBind();
        }
    }
	protected void gvDocumentsRequired_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			ViewState["CURRENTROW"] = e.RowIndex;

		
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvDocumentsRequired_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		gvDocumentsRequired.SelectedIndex = e.NewSelectedIndex;
	}

	protected void gvDocumentsRequired_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		BindData();
		SetPageNavigator();
	}

	protected void gvDocumentsRequired_RowEditing(object sender, GridViewEditEventArgs de)
	{
		try
		{
			GridView _gridView = (GridView)sender;

			_gridView.EditIndex = de.NewEditIndex;
			ViewState["CURRENTROW"] = de.NewEditIndex;
			_gridView.SelectedIndex = de.NewEditIndex;

			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvDocumentsRequired.SelectedIndex = -1;
        gvDocumentsRequired.EditIndex = -1;
        ViewState["CURRENTROW"] = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvDocumentsRequired.SelectedIndex = -1;
        gvDocumentsRequired.EditIndex = -1;
        ViewState["CURRENTROW"] = -1;

        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvDocumentsRequired.SelectedIndex = -1;
        gvDocumentsRequired.EditIndex = -1;
        ViewState["CURRENTROW"] = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }



	private bool IsValidDocumentsRequired(string medicaltestid,string expiryperiod,string agefrom,string ageto)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (General.GetNullableInteger(ucRank.SelectedRank)==null)
			ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableInteger(ucVesselType.SelectedVesseltype) == null)
            ucError.ErrorMessage = "Vessel Type is required.";

        if (General.GetNullableInteger(ucMedicals.SelectedHard) == null)
            ucError.ErrorMessage = "Medical Type is required.";


        if (General.GetNullableInteger(medicaltestid) == null)
            ucError.ErrorMessage = "Medical Test is required.";
        else
        {
            DataSet ds = new DataSet();
            ds = PhoenixRegistersDocumentMedical.EditDocumentMedical(int.Parse(medicaltestid));

            if (ds.Tables[0].Rows[0]["FLDEXPIRYYN"].ToString() == "1")
            {
                if (General.GetNullableInteger(expiryperiod) == null)
                    ucError.ErrorMessage = "Expiry Period is required.";   
            } 
        }
        if (General.GetNullableInteger(agefrom) == null)
            ucError.ErrorMessage = "Age From is required.";

        if (General.GetNullableInteger(ageto) == null)
            ucError.ErrorMessage = "Age To is required.";
                
		return (!ucError.IsError);
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
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void ddlMedicalTestAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gr = (GridViewRow)((DataControlFieldCell)((DropDownList)sender).Parent).Parent;
        UserControlMaskNumber ucExpiryPeriodAdd = (UserControlMaskNumber)gr.FindControl("ucExpiryPeriodAdd");

        DataSet ds = new DataSet();

        if (General.GetNullableInteger(((DropDownList)sender).SelectedValue) != null)
        {
            ds = PhoenixRegistersDocumentMedical.EditDocumentMedical(int.Parse(((DropDownList)sender).SelectedValue));

            if (ds.Tables[0].Rows.Count>0)
            {
                ucExpiryPeriodAdd.ReadOnly = (ds.Tables[0].Rows[0]["FLDEXPIRYYN"].ToString() == "1") ? false : true;
                ucExpiryPeriodAdd.CssClass = (ds.Tables[0].Rows[0]["FLDEXPIRYYN"].ToString() == "1") ? "input" : "readonlytextbox";
            }
        }
    }
}
