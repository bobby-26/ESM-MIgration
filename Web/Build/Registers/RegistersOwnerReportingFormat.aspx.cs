using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class RegistersOwnerReportingFormat : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        SessionUtil.PageAccessRights(this.ViewState);
        lblSelectedNode.Visible = false;

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);

        MenuOwnerBudgetCodeMain.AccessRights = this.ViewState;
        MenuOwnerBudgetCodeMain.MenuList = toolbarmain.Show();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar = new PhoenixToolbar();
        
       
        toolbar.AddButton("Owner Reporting Format", "OWNERREPORT", ToolBarDirection.Right);
        toolbar.AddButton("Financial Year", "FINANCIALYEAR", ToolBarDirection.Right);
        toolbar.AddButton("Vessel List", "VESSELLIST", ToolBarDirection.Right);


        MenuBudgetTab.AccessRights = this.ViewState;
        MenuBudgetTab.MenuList = toolbar.Show();
        MenuBudgetTab.SelectedMenuIndex = 0;

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["ISBUDGETCODE"] = null;

            BindOwnerBudgetCodeTree();
            BudgetCodeEdit();
            CommittedCostsShownYNEdit();
            BindOwnerPortalDropdowns();
            //if (!string.IsNullOrEmpty(ucOwner.SelectedAddress) && ucOwner.SelectedAddress != "Dummy")
            
            OwnerPortalAccessEdit();
            //BindReportAdditionalAttList();
        }
        BindReportType();
        BindReportAdditionalAttList();       
    }
    private void BindOwnerBudgetCodeTree()
    {

        DataSet ds = new DataSet();
        ds = PhoenixRegistersBudget.ListTreeOwnerBudgetCodeGroup((General.GetNullableInteger(ucOwner.SelectedAddress)) == null ? 0 : int.Parse(ucOwner.SelectedAddress));

        tvwOwnerBudgetCode.DataTextField = "FLDOWNERBUDGETGROUP";
        tvwOwnerBudgetCode.DataValueField = "FLDOWNERBUDGETGROUPID";
        tvwOwnerBudgetCode.ParentNodeField = "FLDPARENTGROUPID";
        tvwOwnerBudgetCode.XPathField = "XPATH";
        DataSet ds1 = new DataSet();
        ds1 = PhoenixRegistersAddress.EditAddress(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode, (General.GetNullableInteger(ucOwner.SelectedAddress)) == null ? 0 : long.Parse(ucOwner.SelectedAddress));
        if (ds1.Tables[0].Rows.Count > 0)
        {
            tvwOwnerBudgetCode.RootText = ds1.Tables[0].Rows[0]["FLDCODE"].ToString();
            tvwOwnerBudgetCode.PopulateTree(ds);
        }
        else
        {
            tvwOwnerBudgetCode.RootText = "";
            tvwOwnerBudgetCode.PopulateTree(ds);
        }
    }
    protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
    {
        TreeViewSelectNodeEvent tvsne = (TreeViewSelectNodeEvent)e;
        lblSelectedNode.Visible = false;
        lblSelectedNode.Text = tvsne.SelectedNode.Value.ToString();
        BudgetCodeEdit();
    }
    protected void MenuOwnerBudgetCodeMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (string.IsNullOrEmpty(lblSelectedNode.Text) || lblSelectedNode.Text.ToString().ToUpper() == "ROOT")
                {
                    ucError.ErrorMessage = "Select Owner Budget Code to update Statemet Reference";
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersBudget.InsertOwnerReportingFormat(int.Parse(ucOwner.SelectedAddress),
                        chkCommittedCostsShownYN.Checked ? 1 : 0,
                        chkPOdescriptionShownYN.Checked ? 1 : 0  ,
                        chkShowZeroAmountVoucherYN.Checked ? 1 : 0,
                        chkReceivedDateYN.Checked ? 1: 0 );

                if (ViewState["ISBUDGETCODE"] != null && ViewState["ISBUDGETCODE"].ToString() == "1")
                {
                    if (!IsValidData())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    //update
                    PhoenixRegistersBudget.UpdateOwnerReportingFormat(int.Parse(ucOwner.SelectedAddress), txtOwnerBudgetcode.Text.Trim(),
                     new Guid(lblSelectedNode.Text), txtbudgetdesc.Text.Trim(), chkBudgeted.Checked ? 1 : 0,
                     General.GetNullableString(ddlType.SelectedValue));
                }
                else
                {
                    ucError.ErrorMessage = "Select Owner Budget Code to update Statemet Reference";
                    ucError.Visible = true;
                    return;
                }

            }

            BindOwnerBudgetCodeTree();
            CommittedCostsShownYNEdit();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BudgetTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("VESSELLIST"))
        {
            Response.Redirect("../Common/CommonBudgetVesselList.aspx?", false);
        }
        if (CommandName.ToUpper().Equals("FINANCIALYEAR"))
        {
            Response.Redirect("../Common/CommonBudgetVesselFinancialYear.aspx?", false);
        }
    }
    private bool IsValidData()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ddlType.SelectedValue) == null)
            ucError.ErrorMessage = "Statement Reference is Required.";

        return (!ucError.IsError);
    }
    protected void BudgetCodeEdit()
    {
        if (!string.IsNullOrEmpty(lblSelectedNode.Text) && lblSelectedNode.Text.ToString().ToUpper() != "ROOT")
        {
            DataSet ds = PhoenixRegistersBudget.EditOwnerReportingFormat(new Guid(lblSelectedNode.Text));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["ISBUDGETCODE"] = ds.Tables[0].Rows[0]["FLDISBUDGETCODE"].ToString();
                if (ViewState["ISBUDGETCODE"].ToString() == "1")
                {
                    txtOwnerBudgetcode.Text = ds.Tables[0].Rows[0]["FLDOWNERBUDGETGROUP"].ToString();
                    txtbudgetdesc.Text = ds.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString();
                    chkBudgeted.Checked = ds.Tables[0].Rows[0]["FLDBUDGETEDYN"].ToString() == "1" ? true : false;
                    if (General.GetNullableString(ds.Tables[0].Rows[0]["FLDSTATEMENTREFERENCE"].ToString()) == null)
                        ddlType.SelectedValue = "Monthly Report";
                    else
                        ddlType.SelectedValue = ds.Tables[0].Rows[0]["FLDSTATEMENTREFERENCE"].ToString();
                }
                else
                {
                    Reset();
                }
            }
        }
        else
        {
            Reset();
            lblSelectedNode.Text = "Root";
        }

    }
    protected void CommittedCostsShownYNEdit()
    {
        DataTable dt = PhoenixRegistersBudget.ListOwnerReportingFormat(General.GetNullableInteger(ucOwner.SelectedAddress));
        if (dt.Rows.Count > 0)
        {
            chkCommittedCostsShownYN.Checked = dt.Rows[0]["FLDCOMMITTEDCOSTSSHOWNYN"].ToString() == "1" ? true : false;
            txtStatement.Text = dt.Rows[0]["FLDSTATEMENT"].ToString();
            chkPOdescriptionShownYN.Checked = dt.Rows[0]["FLDSHOWPODESCRIPTIONYN"].ToString() == "1" ? true : false;
            chkShowZeroAmountVoucherYN.Checked = dt.Rows[0]["FLDSHOWZEROAMOUNTYN"].ToString() == "1" ? true : false;
            chkReceivedDateYN.Checked = dt.Rows[0]["FLDCCGOODSRECEIVEDYN"].ToString() == "1" ? true : false;
        }
        else
        {
            chkCommittedCostsShownYN.Checked = false;
            chkPOdescriptionShownYN.Checked = false;
            chkShowZeroAmountVoucherYN.Checked = false;
            chkReceivedDateYN.Checked = false;
        }
    }
    private void Reset()
    {
        txtbudgetdesc.Text = "";
        txtOwnerBudgetcode.Text = "";
        chkBudgeted.Checked = false;
        ddlType.SelectedValue = "Dummy";
    }
    protected void ucOwner_Onchange(object sender, EventArgs e)
    {
        BindOwnerBudgetCodeTree();
        //BudgetCodeEdit();
        CommittedCostsShownYNEdit();
        Reset();
        OwnerPortalAccessEdit();

        //BindReportType();
        BindReportAdditionalAttList();
        

    }

    //*******************************

    private void BindReportType()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSUBACCOUNT", "FLDOWNERBUDGETGROUPNAME", "FLDOWNERBUDGETCODENAME", "FLDNOTINCULDEINOWNERREPORTYN" };
        string[] alCaptions = { "Budget Code", "Owner Budget Group", "Owner Budget Code", "Not Included in Owners" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = null;
        DataTable dt = null;
        if (!string.IsNullOrEmpty(ucOwner.SelectedAddress) && ucOwner.SelectedAddress != "Dummy")
        {
            ds = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReportTypeList(int.Parse(ucOwner.SelectedAddress), ddlStatementReferenceType.SelectedValue);

            General.SetPrintOptions("dgFinancialYearSetup", "Owner Report Type", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                dgFinancialYearSetup.DataSource = ds;
                dgFinancialYearSetup.DataBind();
            }
            else
            {
                dt = ds.Tables[0];
                ShowNoRecordsFound(dt, dgFinancialYearSetup);
            }
        }
        else
        {
            ds = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReportTypeList(0,ddlStatementReferenceType.SelectedValue);
            dt = ds.Tables[0];
            ShowNoRecordsFound(dt, dgFinancialYearSetup);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void dgFinancialYearSetup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidConsolidatedPDF(((DropDownList)_gridView.FooterRow.FindControl("ddlOwnerReportType"))
                                            ,((DropDownList)_gridView.FooterRow.FindControl("ddlOwnerSubReportType"))
                                            ,((TextBox)_gridView.FooterRow.FindControl("txtSortOrder"))
                                            ,1))
                {
                    ucError.Visible = true;
                    return;
                }         

                InsertOwnerReportType(int.Parse(ucOwner.SelectedAddress)
                                      , ((DropDownList)_gridView.FooterRow.FindControl("ddlOwnerReportType")).SelectedValue
                                      , ((DropDownList)_gridView.FooterRow.FindControl("ddlOwnerReportType")).SelectedItem.Text
                                      , ((DropDownList)_gridView.FooterRow.FindControl("ddlOwnerSubReportType")).SelectedValue
                                      , ((DropDownList)_gridView.FooterRow.FindControl("ddlOwnerSubReportType")).SelectedItem.Text
                                      , int.Parse(((TextBox)_gridView.FooterRow.FindControl("txtSortOrder")).Text)
                                      , ddlStatementReferenceType.SelectedValue);
                BindReportType();
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidConsolidatedPDFupdate(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEditSortOrder")), 1))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateOwnerReportType(int.Parse(ucOwner.SelectedAddress)
                                      , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblReportTypeID")).Text
                                      , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblReportType")).Text
                                      , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubReportTypeID")).Text
                                      , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubReportType")).Text
                                      , int.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEditSortOrder")).Text)
                                      , ddlStatementReferenceType.SelectedValue);
                _gridView.EditIndex = -1;
                BindReportType();
            }

            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindReportType();
                _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteOwnerReportType(int.Parse(ucOwner.SelectedAddress), ((Label)_gridView.Rows[nCurrentRow].FindControl("lblReportTypeID")).Text, ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubReportTypeID")).Text, ddlStatementReferenceType.SelectedValue);
                BindReportType();
                //_gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
            }
            else
            {
                _gridView.EditIndex = -1;
                BindReportType();
            }
            SetPageNavigatorForReportType();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void dgFinancialYearSetup_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindReportType();
        SetPageNavigatorForReportType();
    }

    protected void dgFinancialYearSetup_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindReportType();
    }

    protected void dgFinancialYearSetup_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindReportType();
        SetPageNavigatorForReportType();
    }

    protected void dgFinancialYearSetup_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            // e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(dgFinancialYearSetup, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
        //if (e.Row.RowType == DataControlRowType.Footer)
        //{
        //    DropDownList ddlReportTypeList = (DropDownList)e.Row.FindControl("ddlOwnerReportType");
        //    if (ddlReportTypeList != null)
        //        BindReportType(ddlReportTypeList);

        //    DropDownList ddlSubReportTypeList = (DropDownList)e.Row.FindControl("ddlOwnerSubReportType");
        //    if (ddlSubReportTypeList != null)
        //    {
        //        if (ddlReportTypeList.SelectedValue == "Dummy")
        //            BindSubReportType(ddlSubReportTypeList, string.Empty);
        //        else
        //            BindSubReportType(ddlSubReportTypeList, ddlReportTypeList.SelectedValue);
        //    }
        //}
    }

    protected void dgFinancialYearSetup_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindReportType();
    }


    protected void dgFinancialYearSetup_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Get the LinkButton control in the first cell
            LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Row.Attributes["ondblclick"] = _jsDouble;
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            //if (ViewState["SORTEXPRESSION"] != null)
            //{
            //    HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
            //    if (img != null)
            //    {
            //        if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
            //            img.Src = Session["images"] + "/arrowUp.png";
            //        else
            //            img.Src = Session["images"] + "/arrowDown.png";

            //        img.Visible = true;
            //    }
            //}
        }


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
            if (cmdDelete != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
        }


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton cmdPOApprove = (ImageButton)e.Row.FindControl("cmdPOApprove");
            if (cmdPOApprove != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdPOApprove.CommandName)) cmdPOApprove.Visible = false;

            ImageButton imgAttachment = (ImageButton)e.Row.FindControl("imgAttachment");
            if (imgAttachment != null)
                if (!SessionUtil.CanAccess(this.ViewState, imgAttachment.CommandName)) imgAttachment.Visible = false;
        }

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    Label lblIsRecentFinancialYear = (Label)e.Row.FindControl("lblIsRecentFinancialYear");

        //    if (lblIsRecentFinancialYear.Text == "1")
        //    {
        //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
        //        if (db != null) db.Visible = true;
        //    }

        //}
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton cmdAdd = (ImageButton)e.Row.FindControl("cmdAdd");
            if (cmdAdd != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName)) cmdAdd.Visible = false;

            DropDownList ddlReportTypeList = (DropDownList)e.Row.FindControl("ddlOwnerReportType");
            if (ddlReportTypeList != null)
                BindReportTypeBasedFlag(ddlReportTypeList,0);

            if (ViewState["ReportType"] != null)
            {
                ddlReportTypeList.SelectedValue = ViewState["ReportType"].ToString();
            }

            DropDownList ddlSubReportTypeList = (DropDownList)e.Row.FindControl("ddlOwnerSubReportType");
            if (ddlSubReportTypeList != null)
            {
                String reportcode = ddlReportTypeList.SelectedValue == "Dummy" ? string.Empty : ddlReportTypeList.SelectedValue;
                BindSubReportType(ddlSubReportTypeList, reportcode);
            }
        }
    }

    private void SetPageNavigatorForReportType()
    {
        //cmdPrevious.Enabled = IsPreviousEnabled();
        //cmdNext.Enabled = IsNextEnabled();
        //lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        //lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        //lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
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


    protected void InsertOwnerReportType(int OwnerID, string reportCode, string reportType, string subreportCode, string subreportType, int sortOrder,string referencetype)
    {
        try
        {
            PhoenixAccountsOwnerReportDisplay.OwnerDisplayReportTypeInsert(OwnerID, reportCode, reportType, subreportCode, subreportType, sortOrder,referencetype);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void UpdateOwnerReportType(int OwnerID, string reportCode, string reportType, string subreportCode, string subreportType, int sortOrder, string referencetype)
    {
        try
        {
            PhoenixAccountsOwnerReportDisplay.OwnerDisplayReportTypeUpdate(OwnerID, reportCode, reportType, subreportCode, subreportType, sortOrder,referencetype);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void DeleteOwnerReportType(int OwnerID, string reportCode, string subreportCode, string referencetype)
    {
        try
        {
            PhoenixAccountsOwnerReportDisplay.OwnerDisplayReportTypeDelete(OwnerID, reportCode, subreportCode,referencetype);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindReportType(DropDownList ddl)
    {
        ddl.DataTextField = "FLDREPORTTYPE";
        ddl.DataValueField = "FLDREPORTCODE";
        ddl.DataSource = PhoenixAccountsOwnerReportDisplay.SOACheckingReportTypeList();
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }


    private void BindSubReportType(DropDownList ddl, string reportcode)
    {
        ddl.DataTextField = "FLDSUBREPORTTYPE";
        ddl.DataValueField = "FLDSUBREPORTCODE";
        ddl.DataSource = PhoenixAccountsOwnerReportDisplay.SOACheckingSubReportTypeList(reportcode);
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    private void BindReportTypeBasedFlag(DropDownList ddl,int Flag)
    {
        ddl.DataTextField = "FLDREPORTTYPE";
        ddl.DataValueField = "FLDREPORTCODE";
        ddl.DataSource = PhoenixAccountsOwnerReportDisplay.SOACheckingReportTypeListBasedFlag(Flag);
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }


    protected void ddlOwnerReportType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlOwnerReport = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlOwnerReport.NamingContainer;
        if (row != null)
        {
            if (row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlOwnerSubReportType = (DropDownList)row.FindControl("ddlOwnerSubReportType");
                ViewState["ReportType"] = ddlOwnerReport.SelectedValue;
                BindSubReportType(ddlOwnerSubReportType, ddlOwnerReport.SelectedValue);
            }
        }      
    }


    //*******************************

    private void BindReportAdditionalAttList()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSUBACCOUNT", "FLDOWNERBUDGETGROUPNAME", "FLDOWNERBUDGETCODENAME", "FLDNOTINCULDEINOWNERREPORTYN" };
        string[] alCaptions = { "Budget Code", "Owner Budget Group", "Owner Budget Code", "Not Included in Owners" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        int ownerID = 0;

        if (!string.IsNullOrEmpty(ucOwner.SelectedAddress) && ucOwner.SelectedAddress != "Dummy")
        {
            ownerID = int.Parse(ucOwner.SelectedAddress);
        }
        DataSet ds = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReportAditionalAttachmentList(ownerID);

        General.SetPrintOptions("gvAdditionalAttachment", "Owner Report Type", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAdditionalAttachment.DataSource = ds;
            gvAdditionalAttachment.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvAdditionalAttachment);
        }
        
        //else
        //{          
        //    ds = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReportAditionalAttachmentList(0);
        //    dt = ds.Tables[0];
        //    ShowNoRecordsFound(dt, gvAdditionalAttachment);
        //}

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void dgvAdditionalAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                if (!IsValidConsolidatedPDF(((DropDownList)_gridView.FooterRow.FindControl("ddlOwnerReportType"))
                                            , ((DropDownList)_gridView.FooterRow.FindControl("ddlOwnerSubReportType"))
                                            , ((TextBox)_gridView.FooterRow.FindControl("txtSortOrderAddl"))
                                            , 0))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertOwnerReportAdditionalAttachment(int.Parse(ucOwner.SelectedAddress)
                                      , ((DropDownList)_gridView.FooterRow.FindControl("ddlOwnerReportType")).SelectedValue
                                      , ((DropDownList)_gridView.FooterRow.FindControl("ddlOwnerReportType")).SelectedItem.Text
                                      , ((DropDownList)_gridView.FooterRow.FindControl("ddlOwnerSubReportType")).SelectedValue
                                      , ((DropDownList)_gridView.FooterRow.FindControl("ddlOwnerSubReportType")).SelectedItem.Text
                                      , int.Parse(((TextBox)_gridView.FooterRow.FindControl("txtSortOrderAddl")).Text));
                BindReportAdditionalAttList();
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidConsolidatedPDFupdate(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEditSortOrder")), 0))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateOwnerReportAdditionalAttachment(int.Parse(ucOwner.SelectedAddress)
                                      , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblReportTypeID")).Text
                                      , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblReportType")).Text
                                      , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubReportTypeID")).Text
                                      , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubReportType")).Text
                                      , int.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEditSortOrder")).Text));
                _gridView.EditIndex = -1;
                BindReportAdditionalAttList();
            }

            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindReportAdditionalAttList();
                _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteOwnerReportAdditionalAttachment(int.Parse(ucOwner.SelectedAddress), ((Label)_gridView.Rows[nCurrentRow].FindControl("lblReportTypeID")).Text, ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubReportTypeID")).Text);
                BindReportAdditionalAttList();
                //_gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
            }
            //else
            //{
            //    _gridView.EditIndex = -1;
            //    BindReportAdditionalAttList();
            //}
            //SetPageNavigatorForReportAdditional();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAdditionalAttachment_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindReportAdditionalAttList();
        SetPageNavigatorForReportType();
    }

    protected void gvAdditionalAttachment_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindReportAdditionalAttList();
    }

    protected void gvAdditionalAttachment_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindReportAdditionalAttList();
        SetPageNavigatorForReportAdditional();
    }

    protected void dgvAdditionalAttachment_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            // e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(dgFinancialYearSetup, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
        //if (e.Row.RowType == DataControlRowType.Footer)
        //{
        //    DropDownList ddlReportTypeList = (DropDownList)e.Row.FindControl("ddlOwnerReportType");
        //    if (ddlReportTypeList != null)
        //        BindReportType(ddlReportTypeList);

        //    DropDownList ddlSubReportTypeList = (DropDownList)e.Row.FindControl("ddlOwnerSubReportType");
        //    if (ddlSubReportTypeList != null)
        //    {
        //        if (ddlReportTypeList.SelectedValue == "Dummy")
        //            BindSubReportType(ddlSubReportTypeList, string.Empty);
        //        else
        //            BindSubReportType(ddlSubReportTypeList, ddlReportTypeList.SelectedValue);
        //    }
        //}
    }

    protected void gvAdditionalAttachment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindReportAdditionalAttList();
    }


    protected void gvAdditionalAttachment_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Get the LinkButton control in the first cell
            LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Row.Attributes["ondblclick"] = _jsDouble;
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            //if (ViewState["SORTEXPRESSION"] != null)
            //{
            //    HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
            //    if (img != null)
            //    {
            //        if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
            //            img.Src = Session["images"] + "/arrowUp.png";
            //        else
            //            img.Src = Session["images"] + "/arrowDown.png";

            //        img.Visible = true;
            //    }
            //}
        }


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
            if (cmdDelete != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
        }


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton cmdPOApprove = (ImageButton)e.Row.FindControl("cmdPOApprove");
            if (cmdPOApprove != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdPOApprove.CommandName)) cmdPOApprove.Visible = false;

            ImageButton imgAttachment = (ImageButton)e.Row.FindControl("imgAttachment");
            if (imgAttachment != null)
                if (!SessionUtil.CanAccess(this.ViewState, imgAttachment.CommandName)) imgAttachment.Visible = false;
        }

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    Label lblIsRecentFinancialYear = (Label)e.Row.FindControl("lblIsRecentFinancialYear");

        //    if (lblIsRecentFinancialYear.Text == "1")
        //    {
        //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
        //        if (db != null) db.Visible = true;
        //    }

        //}
        if (e.Row.RowType == DataControlRowType.Footer)
        {            
            ImageButton cmdAdd = (ImageButton)e.Row.FindControl("cmdAdd");
            if (cmdAdd != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName)) cmdAdd.Visible = false;

            DropDownList ddlReportTypeList = (DropDownList)e.Row.FindControl("ddlOwnerReportType");
            if (ddlReportTypeList != null)
                BindReportTypeBasedFlag(ddlReportTypeList,1);

            if (ViewState["ReportType"] != null)
            {
                ddlReportTypeList.SelectedValue = ViewState["ReportType"].ToString();
            }

            DropDownList ddlSubReportTypeList = (DropDownList)e.Row.FindControl("ddlOwnerSubReportType");
            if (ddlSubReportTypeList != null)
            {
                String reportcode = ddlReportTypeList.SelectedValue == "Dummy" ? string.Empty : ddlReportTypeList.SelectedValue;
                BindSubReportType(ddlSubReportTypeList, reportcode);
            }            
        }
    }

    private void SetPageNavigatorForReportAdditional()
    {
        //cmdPrevious.Enabled = IsPreviousEnabled();
        //cmdNext.Enabled = IsNextEnabled();
        //lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        //lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        //lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }


    protected void InsertOwnerReportAdditionalAttachment(int OwnerID, string reportCode, string reportType, string subreportCode, string subreportType, int sortOrder)
    {
        try
        {
            PhoenixAccountsOwnerReportDisplay.OwnerDisplayReportAditionalAttachmentInsert(OwnerID, reportCode, reportType, subreportCode, subreportType, sortOrder);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void UpdateOwnerReportAdditionalAttachment(int OwnerID, string reportCode, string reportType, string subreportCode, string subreportType, int sortOrder)
    {
        try
        {
            PhoenixAccountsOwnerReportDisplay.OwnerDisplayReportAditionalAttachmentUpdate(OwnerID, reportCode, reportType, subreportCode, subreportType, sortOrder);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void DeleteOwnerReportAdditionalAttachment(int OwnerID, string reportCode, string subreportCode)
    {
        try
        {
            PhoenixAccountsOwnerReportDisplay.OwnerDisplayReportAditionalAttachmentDelete(OwnerID, reportCode, subreportCode);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    protected void ddlOwnerReportTypeAdd_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlOwnerReport = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlOwnerReport.NamingContainer;
        if (row != null)
        {
            if (row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlOwnerSubReportType = (DropDownList)row.FindControl("ddlOwnerSubReportType");
                ViewState["ReportType"] = ddlOwnerReport.SelectedValue;
                BindSubReportType(ddlOwnerSubReportType, ddlOwnerReport.SelectedValue);
            }
        }
    }

    private void BindOwnerPortalDropdowns()
    {
        BindSubReportType(ddlVesselTrialBalance, "VTB");
        BindSubReportType(ddlVesselSummaryExpenses, "SEN");
        BindSubReportType(ddlMonthlyVariance, "MVR");
        BindSubReportType(ddlYearlyVariance, "YVR");
        BindSubReportType(ddlAccumulatedVariance, "AVR");
        BindSubReportType(ddlFundsPosition, "FDP");
        BindSubReportType(ddlYTDdetails, "YTD");
    }

    protected void chkVTB_OnCheckedChanged(object sender, EventArgs e)
    {
        if (ddlVesselTrialBalance.SelectedValue != "Dummy")
            UpdateOwnerReportPortalAccess(int.Parse(ucOwner.SelectedAddress), "VTB", ddlVesselTrialBalance.SelectedValue, chkVTB.Checked.Equals(true) ? 1 : 0);
    }

    protected void chkSME_OnCheckedChanged(object sender, EventArgs e)
    {
        if (ddlVesselSummaryExpenses.SelectedValue != "Dummy")
            UpdateOwnerReportPortalAccess(int.Parse(ucOwner.SelectedAddress), "SEN", ddlVesselSummaryExpenses.SelectedValue, chkSME.Checked.Equals(true) ? 1 : 0);
    }

    protected void chkMVR_OnCheckedChanged(object sender, EventArgs e)
    {
        if (ddlMonthlyVariance.SelectedValue != "Dummy")
            UpdateOwnerReportPortalAccess(int.Parse(ucOwner.SelectedAddress), "MVR", ddlMonthlyVariance.SelectedValue, chkMVR.Checked.Equals(true) ? 1 : 0);
    }

    protected void chkYVR_OnCheckedChanged(object sender, EventArgs e)
    {
        if (ddlYearlyVariance.SelectedValue != "Dummy")
            UpdateOwnerReportPortalAccess(int.Parse(ucOwner.SelectedAddress), "YVR", ddlYearlyVariance.SelectedValue, chkYVR.Checked.Equals(true) ? 1 : 0);
    }

    protected void chkAVR_OnCheckedChanged(object sender, EventArgs e)
    {
        if (ddlAccumulatedVariance.SelectedValue != "Dummy")
            UpdateOwnerReportPortalAccess(int.Parse(ucOwner.SelectedAddress), "AVR", ddlAccumulatedVariance.SelectedValue, chkAVR.Checked.Equals(true) ? 1 : 0);
    }

    protected void chkFPS_OnCheckedChanged(object sender, EventArgs e)
    {
        if (ddlFundsPosition.SelectedValue != "Dummy")
            UpdateOwnerReportPortalAccess(int.Parse(ucOwner.SelectedAddress), "FDP", ddlFundsPosition.SelectedValue, chkFPS.Checked.Equals(true) ? 1 : 0);
    }

    protected void chkYTD_OnCheckedChanged(object sender, EventArgs e)
    {
        if (ddlYTDdetails.SelectedValue != "Dummy")
            UpdateOwnerReportPortalAccess(int.Parse(ucOwner.SelectedAddress), "YTD", ddlYTDdetails.SelectedValue, chkYTD.Checked.Equals(true) ? 1 : 0);
    }

    protected void UpdateOwnerReportPortalAccess(int OwnerID, string reportCode, string subreportCode, int accessYN)
    {
        try
        {
            PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccesstUpdate(OwnerID, reportCode, subreportCode, accessYN);            
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ucOwner.SelectedAddress) && ucOwner.SelectedAddress != "Dummy")
        {
            BindReportAdditionalAttList();
            //OwnerPortalAccessEdit();
        }           //BindReportType();      

    }

    private void OwnerPortalAccessEdit()
    {
        if (!string.IsNullOrEmpty(ucOwner.SelectedAddress) && ucOwner.SelectedAddress != "Dummy")
        {
            DataTable dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccesstEdit(int.Parse(ucOwner.SelectedAddress));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["FLDREPORTCODE"].ToString() == "VTB")
                    {
                        ddlVesselTrialBalance.SelectedValue = dt.Rows[i]["FLDSUBREPORTCODE"].ToString();
                        chkVTB.Checked = dt.Rows[i]["FLDACCESSYN"].ToString().Equals("1") ? true : false;
                    }
                    if (dt.Rows[i]["FLDREPORTCODE"].ToString() == "SEN")
                    {
                        ddlVesselSummaryExpenses.SelectedValue = dt.Rows[i]["FLDSUBREPORTCODE"].ToString();
                        chkSME.Checked = dt.Rows[i]["FLDACCESSYN"].ToString().Equals("1") ? true : false;
                    }
                    if (dt.Rows[i]["FLDREPORTCODE"].ToString() == "MVR")
                    {
                        ddlMonthlyVariance.SelectedValue = dt.Rows[i]["FLDSUBREPORTCODE"].ToString();
                        chkMVR.Checked = dt.Rows[i]["FLDACCESSYN"].ToString().Equals("1") ? true : false;
                    }
                    if (dt.Rows[i]["FLDREPORTCODE"].ToString() == "YVR")
                    {
                        ddlYearlyVariance.SelectedValue = dt.Rows[i]["FLDSUBREPORTCODE"].ToString();
                        chkYVR.Checked = dt.Rows[i]["FLDACCESSYN"].ToString().Equals("1") ? true : false;
                    }
                    if (dt.Rows[i]["FLDREPORTCODE"].ToString() == "AVR")
                    {
                        ddlAccumulatedVariance.SelectedValue = dt.Rows[i]["FLDSUBREPORTCODE"].ToString();
                        chkAVR.Checked = dt.Rows[i]["FLDACCESSYN"].ToString().Equals("1") ? true : false;
                    }
                    if (dt.Rows[i]["FLDREPORTCODE"].ToString() == "FDP")
                    {
                        ddlFundsPosition.SelectedValue = dt.Rows[i]["FLDSUBREPORTCODE"].ToString();
                        chkFPS.Checked = dt.Rows[i]["FLDACCESSYN"].ToString().Equals("1") ? true : false;
                    }
                    if (dt.Rows[i]["FLDREPORTCODE"].ToString() == "YTD")
                    {
                        ddlYTDdetails.SelectedValue = dt.Rows[i]["FLDSUBREPORTCODE"].ToString();
                        chkYTD.Checked = dt.Rows[i]["FLDACCESSYN"].ToString().Equals("1") ? true : false;
                    }

                }
            }
            else
            {
                ddlVesselTrialBalance.SelectedValue = "Dummy";
                ddlVesselSummaryExpenses.SelectedValue = "Dummy";
                ddlMonthlyVariance.SelectedValue = "Dummy";
                ddlYearlyVariance.SelectedValue = "Dummy";
                ddlAccumulatedVariance.SelectedValue = "Dummy";
                ddlFundsPosition.SelectedValue = "Dummy";
                ddlYTDdetails.SelectedValue = "Dummy";
                chkVTB.Checked = false;                
                chkSME.Checked = false;                
                chkMVR.Checked = false;                
                chkYVR.Checked =  false;                
                chkAVR.Checked = false;                
                chkFPS.Checked = false;
                chkYTD.Checked = false;
            }
        }

    }

    private bool IsValidConsolidatedPDF(DropDownList ddlreporttype,DropDownList ddlsubreporttype,TextBox txtorder,int type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ddlStatementReferenceType.SelectedValue) == null && type==1)
            ucError.ErrorMessage = "Consolidated PDF Statement Reference Type is Required.";

        if(ddlreporttype.SelectedValue=="Dummy")
            ucError.ErrorMessage = "Report type is Required.";

        if (ddlsubreporttype.SelectedValue == "Dummy")
            ucError.ErrorMessage = "Subreport type is Required.";

        if (General.GetNullableInteger(txtorder.Text) == null)
            ucError.ErrorMessage = "Order is Required";

        return (!ucError.IsError);
    }

    private bool IsValidConsolidatedPDFupdate(TextBox txtorder, int type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ddlStatementReferenceType.SelectedValue) == null && type == 1)
            ucError.ErrorMessage = "Consolidated PDF Statement Reference Type is Required.";        

        if (General.GetNullableInteger(txtorder.Text) == null)
            ucError.ErrorMessage = "Order is Required";

        return (!ucError.IsError);
    }



    protected void ddlStatementReferenceType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindReportType();
       // BindReportAdditionalAttList();     
    }


}
