using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Inspection_InspectionPNITravelDocumentCheckList1 : PhoenixBasePage
{

    //protected override void Render(HtmlTextWriter writer)
    //{

    //    string lastpartdepartment = string.Empty;
    //    string currpartdepartment = string.Empty;
    //    Table gridTable = (Table)gvChecklist.Controls[0];
    //    decimal amt = 0, amtusd = 0;
    //    for (int i = 0; i < gvChecklist.Rows.Count; i++)
    //    {
    //        GridViewRow gvr = gvChecklist.Rows[i];
    //        if (gvr.RowType == DataControlRowType.DataRow)
    //        {
    //            Label hfDepartment = (Label)gvr.FindControl("lblDepartmentId");
    //            Label hfPart = (Label)gvr.FindControl("lblPart");
    //            Label lblamt = (Label)gvr.FindControl("lblAmount");
    //            UserControlMaskNumber ucNumber = (UserControlMaskNumber)gvr.FindControl("ucAmountEdit");
    //            Label lblamtusd = (Label)gvr.FindControl("lblAmountInUSD");

    //            int rowIndex = gridTable.Rows.GetRowIndex(gvr);
    //            currpartdepartment = hfPart.Text + hfDepartment.Text;

    //            if (i == 0 || lastpartdepartment.CompareTo(currpartdepartment) == 0)
    //            {
    //                if (lblamt != null)
    //                    amt = amt + decimal.Parse(lblamt.Text);
    //                else
    //                {
    //                    amt = amt + decimal.Parse(ucNumber.Text);
    //                }
    //                amtusd = amtusd + decimal.Parse(lblamtusd.Text);
    //            }

    //            if (i == 0 || lastpartdepartment.CompareTo(currpartdepartment) != 0)
    //            {
    //                GridViewRow headerRow = new GridViewRow(rowIndex, -1, DataControlRowType.Header, DataControlRowState.Normal);
    //                TableCell headerCell = new TableCell();
    //                headerCell.Text = hfPart.Text;
    //                headerRow.Cells.Add(headerCell);

    //                headerCell = new TableCell();
    //                headerCell.Text = "Department - " + hfDepartment.Text;
    //                headerRow.Cells.Add(headerCell);

    //                headerCell = new TableCell();
    //                headerCell.Text = "&nbsp;";
    //                headerCell.ColumnSpan = 7;
    //                headerRow.Cells.Add(headerCell);

    //                gridTable.Controls.AddAt(rowIndex, headerRow);
    //                if (i > 0)
    //                {
    //                    headerRow = new GridViewRow(rowIndex, -1, DataControlRowType.DataRow, DataControlRowState.Normal);

    //                    headerCell = new TableCell();
    //                    headerCell.Text = "&nbsp;";
    //                    headerCell.ColumnSpan = 2;
    //                    headerRow.Cells.Add(headerCell);

    //                    headerCell = new TableCell();
    //                    headerCell.Text = "Total";
    //                    headerCell.Font.Bold = true;
    //                    headerRow.Cells.Add(headerCell);

    //                    headerCell = new TableCell();
    //                    headerCell.Text = "&nbsp;";
    //                    headerRow.Cells.Add(headerCell);

    //                    headerCell = new TableCell();
    //                    headerCell.Text = "&nbsp;";
    //                    headerCell.Font.Bold = true;
    //                    headerRow.Cells.Add(headerCell);



    //                    headerCell = new TableCell();
    //                    headerCell.Text = "&nbsp;";
    //                    headerRow.Cells.Add(headerCell);

    //                    headerCell = new TableCell();
    //                    headerCell.Text = amtusd.ToString();
    //                    headerCell.Font.Bold = true;
    //                    headerRow.Cells.Add(headerCell);

    //                    headerCell = new TableCell();
    //                    headerCell.Text = "&nbsp;";
    //                    headerRow.Cells.Add(headerCell);

    //                    headerCell = new TableCell();
    //                    headerCell.Text = "&nbsp;";
    //                    headerRow.Cells.Add(headerCell);



    //                    gridTable.Controls.AddAt(rowIndex, headerRow);
    //                    if (lblamt != null)
    //                        amt = decimal.Parse(lblamt.Text);
    //                    else
    //                    {
    //                        amt = decimal.Parse(ucNumber.Text);
    //                    }
    //                    amtusd = decimal.Parse(lblamtusd.Text);
    //                }
    //                lastpartdepartment = hfPart.Text + hfDepartment.Text;
    //            }

    //            if (i == gvChecklist.Rows.Count - 1)
    //            {
    //                GridViewRow headerRow = new GridViewRow(gridTable.Rows.Count, -1, DataControlRowType.DataRow, DataControlRowState.Normal);

    //                TableCell headerCell = new TableCell();
    //                headerCell.Text = "&nbsp;";
    //                //headerCell.ColumnSpan = 0;
    //                headerRow.Cells.Add(headerCell);

    //                headerCell = new TableCell();
    //                headerCell.Text = "Total";
    //                headerCell.Font.Bold = true;
    //                headerRow.Cells.Add(headerCell);

    //                headerCell = new TableCell();
    //                headerCell.Text = "&nbsp;";
    //                headerRow.Cells.Add(headerCell);

    //                headerCell = new TableCell();
    //                headerCell.Text = "&nbsp;";
    //                headerCell.Font.Bold = true;
    //                headerRow.Cells.Add(headerCell);

    //                headerCell = new TableCell();
    //                headerCell.Text = "&nbsp;";
    //                headerCell.Font.Bold = true;
    //                headerRow.Cells.Add(headerCell);



    //                headerCell = new TableCell();
    //                headerCell.Text = "&nbsp;";
    //                headerRow.Cells.Add(headerCell);

    //                headerCell = new TableCell();
    //                headerCell.Text = "&nbsp;";
    //                headerRow.Cells.Add(headerCell);

    //                headerCell = new TableCell();
    //                headerCell.Text = amtusd.ToString();
    //                headerCell.HorizontalAlign = HorizontalAlign.Right;
    //                headerRow.Cells.Add(headerCell);

    //                gridTable.Controls.AddAt(gridTable.Rows.Count, headerRow);
    //                // Update lastValue

    //            }
    //        }
    //    }

    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["PNIID"] = null;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["REFNO"] = null;
            if (Request.QueryString["PNIID"] != null)
                ViewState["PNIID"] = Request.QueryString["PNIID"].ToString();

            if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != "")
            {
                GetAgentCode();
                EditOperation();

            }
            if (!string.IsNullOrEmpty(Request.QueryString["REFNO"]))
            {
                Title1.Text = "PNI Case No:" + "(" + Request.QueryString["REFNO"] + ")";
                ViewState["REFNO"] = Request.QueryString["REFNO"];

            }
          
        }
        txtCrewId.Attributes.Add("style", "visibility:hidden");

        txtAgent.Attributes.Add("style", "visibility:hidden");
        txtcrewplanid.Attributes.Add("style", "visibility:hidden");
        txtemployeeid.Attributes.Add("style", "visibility:hidden");
        txtrankid.Attributes.Add("style", "visibility:hidden");


        //txtCrewRank.Attributes.Add("style", "visibility:hidden");


        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Medical Case", "MEDICAL");
        toolbar.AddButton("P&I Costing", "TRAVELCHECKLIST");
        MenuPNIGeneral.AccessRights = this.ViewState;
        MenuPNIGeneral.MenuList = toolbar.Show();
        // MenuPNIGeneral.SetTrigger(pnlNTBRManager);

        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        toolbar = new PhoenixToolbar();

        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionPNITravelDocumentCheckList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");

        //toolbargrid.AddImageLink("javascript:parent.Openpopup('codehelp1','','AccountsAmosInvoiceLineItem.aspx?qinvoicecode=" + ViewState["INVOICECODE"].ToString() + "'); return false;", "Add", "Add.png", "ADD");
        Menupni.AccessRights = this.ViewState;
        Menupni.MenuList = toolbargrid.Show();

        PNIListMain.AccessRights = this.ViewState;
        PNIListMain.MenuList = toolbar.Show();

        MenuPNIGeneral.SelectedMenuIndex = 1;







    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

        if (IsValidInspectionPNIOperation())
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            if (nvc != null)
            {
                txtonsignername.Text = nvc[1];
                txtcrewplanid.Text = nvc[2];
                txtemployeeid.Text = nvc[3];
                txtrankid.Text = nvc[4];



            }
            //PhoenixInspectionPNI.PNITravelSignOnOffInsert(new Guid(ViewState["PNIID"].ToString()),
            //                                            int.Parse(txtCrewId.Text.ToString()),
            //                                            int.Parse(ucSignOffPort.SelectedSeaport),
            //                                            DateTime.Parse(txtSignOffDate.Text),
            //                                            int.Parse(txtAgent.Text.ToString()),
            //                                            0, int.Parse(ddlMedicalCase.SelectedValue)
            //                                            , General.GetNullableGuid(txtcrewplanid.Text)
            //                                            , General.GetNullableInteger(txtemployeeid.Text)
            //                                            , General.GetNullableInteger(ucSignOffPortonsigner.SelectedSeaport)
            //                                            , General.GetNullableInteger(txtrankid.Text)

            //                                            );
            //PhoenixInspectionPNI.PNITravelSignOnOffInsert(new Guid(ViewState["PNIID"].ToString()),
            //                                            int.Parse(txtCrewOnSignid.Text.ToString()),
            //                                            int.Parse(ucSignOnPort.SelectedSeaport),
            //                                            DateTime.Parse(txtSignOnDate.Text),
            //                                            int.Parse(txtAgentSignOn.Text.ToString()),
            //                                            1);


            BindCheckListData();
            


        }
        else
        {
            ucError.Visible = true;
            return;
        }
        //try
        //{
        //    BindCheckListData();
        //    NameValueCollection nvc = Filter.CurrentPickListSelection;
        //    if (nvc != null)
        //    {
        //        txtcrewplanid.Text = nvc[1];
        //        txtemployeeid.Text = nvc[2];
        //        txtonsignername.Text = nvc[3];

        //    }
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }
    protected void imgShowcrew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        string str = "showPickList('spnPickListBudgetCode', 'codehelp1', '', '../Common/CommonPickListCrewChangePlan.aspx?vesselid=" + ucVessel.SelectedVessel.ToString() + "&rankid=" + ViewState["CREWRANK"].ToString() + "', false);";
        //  imgShowcrew.Attributes.Add("onclick", "return showPickList('spnPickListBudgetCode', 'codehelp1', '', '../Common/CommonPickListCrewChangePlan.aspx?vesselid=" + ucVessel.SelectedVessel.ToString() + "', true); ");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "codehelp1", str, true);

    }
    protected void MenuPNIGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("MEDICAL"))
        {
            if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != "")
            {
                Response.Redirect("../Inspection/InspectionPNIOperation.aspx?PNIID=" + ViewState["PNIID"], true);
            }
        }

    }
    protected void PNIListMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            Guid pniid = Guid.Empty;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidInspectionPNIOperation())
                {
                    PhoenixInspectionPNI.PNITravelSignOnOffInsert(new Guid(ViewState["PNIID"].ToString()),
                                                                int.Parse(txtCrewId.Text.ToString()),
                                                                int.Parse(ucSignOffPort.SelectedSeaport),
                                                                DateTime.Parse(txtSignOffDate.Text),
                                                                int.Parse(txtAgent.Text.ToString()),
                                                                0, int.Parse(ddlMedicalCase.SelectedValue)
                                                                , General.GetNullableGuid(txtcrewplanid.Text)
                                                                , General.GetNullableInteger(txtemployeeid.Text)
                                                                , General.GetNullableInteger(ucSignOffPortonsigner.SelectedSeaport)
                                                                , General.GetNullableInteger(txtrankid.Text)

                                                                );
                    //PhoenixInspectionPNI.PNITravelSignOnOffInsert(new Guid(ViewState["PNIID"].ToString()),
                    //                                            int.Parse(txtCrewOnSignid.Text.ToString()),
                    //                                            int.Parse(ucSignOnPort.SelectedSeaport),
                    //                                            DateTime.Parse(txtSignOnDate.Text),
                    //                                            int.Parse(txtAgentSignOn.Text.ToString()),
                    //                                            1);


                    BindCheckListData();

                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void EditOperation()
    {
        if (ViewState["PNIID"] != null)
        {
            DataSet ds = PhoenixInspectionPNI.PNIDocumentationCaseEdit(new Guid(ViewState["PNIID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ucVessel.Enabled = false;
                txtCrewId.Text = dr["FLDEMPLOYEEID"].ToString();
                txtCrewName.Text = dr["FLDCREWNAME"].ToString();
                txtCrewRank.Text = dr["FLDCREWRANK"].ToString();
                ViewState["CREWRANK"] = dr["FLDRANKID"].ToString();
                txtAgentNumber.Text = dr["FLDPORTAGENTCODE"].ToString();
                txtAgentName.Text = dr["FLDPORTAGENTNAME"].ToString();

                txtAgent.Text = dr["FLDPORTAGENT"].ToString();
                txtDescription.Text = dr["FLDILLNESSDESCRIPTION"].ToString();
                string AgentsCtdatails = dr["FLDPHONE"].ToString();
                txtSignOffDate.Text = dr["FLDSIGNONOFFDATE"].ToString();
                ucSignOffPort.SelectedSeaport = dr["FLDSIGNONOFFPORT"].ToString();
                ddlMedicalCase.SelectedValue = dr["FLDMEDICALCASE"].ToString();
                txtAgentsContactDetails.Text = AgentsCtdatails.Replace(@"~", "").ToString();
                txtCountry.Text = dr["FLDCOUNTRYNAME"].ToString();

                txtcrewplanid.Text = dr["FLDCREWPLANID"].ToString();
                txtemployeeid.Text = dr["FLDONSIGNERID"].ToString();
                txtonsignername.Text = dr["FLDONSIGNERNAME"].ToString();
                txtonsignerrank.Text = dr["FLDONSIGNERRANKNAME"].ToString();
                txtrankid.Text = dr["FLDONSIGNERRANK"].ToString();
                ucSignOffPortonsigner.SelectedSeaport = dr["FLDONSIGNERPORT"].ToString();
                //imgShowCrewInCharge.Visible = false;

            }

        }

    }

    private bool IsValidInspectionPNIOperation()
    {
        ucError.HeaderMessage = "Please provide the following required information";




        if (General.GetNullableDateTime(txtSignOffDate.Text) == null)
            ucError.ErrorMessage = "Sign Off Date is Required.";


        if (string.IsNullOrEmpty(ucSignOffPort.SelectedSeaport) || ucSignOffPort.SelectedSeaport.ToUpper().ToString() == "DUMMY")
            ucError.ErrorMessage = "Sign Off Port is Required";
        //if (string.IsNullOrEmpty(ucCountry.SelectedCountry) || ucCountry.SelectedCountry.ToUpper().ToString() == "DUMMY")
        //    ucError.ErrorMessage = "Sign Off Country is Required";

        if (string.IsNullOrEmpty(ddlMedicalCase.SelectedValue) || ddlMedicalCase.SelectedValue.ToUpper().ToString() == "DUMMY")
            ucError.ErrorMessage = "MedicalCase Type is Required";

        //if (string.IsNullOrEmpty(ucSignOffPortonsigner.SelectedSeaport) || ucSignOffPortonsigner.SelectedSeaport.ToUpper().ToString() == "DUMMY")
        //    ucError.ErrorMessage = "Sign Off Port is Required for Onsigner";








        return (!ucError.IsError);
    }


    private bool IsValidCheckList(decimal? amount, int? currencyid, string status)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (status == "Dummy")
            ucError.ErrorMessage = "Status is Required.";

        if (amount == null)
            ucError.ErrorMessage = "Amount is Required.";

        if (currencyid == null)
            ucError.ErrorMessage = "Currency is Required.";

        return (!ucError.IsError);
    }



    private void binddatapartd()
    {
        DataSet ds1;
        ds1 = PhoenixInspectionPNI.PNITravelCheckListDetailsListPartD
               (new Guid(ViewState["PNIID"].ToString()));
        if (ds1.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds1.Tables[0].Rows[0];
            DataSet dtpartd = new DataSet();
            dtpartd.Tables.Add();
            DataColumn column = new DataColumn();
            column.ColumnName = "FLDHEAD";
            dtpartd.Tables[0].Columns.Add(column);
            DataColumn column1 = new DataColumn();
            column1.ColumnName = "FLDSNO";
            dtpartd.Tables[0].Columns.Add(column1);
            DataColumn column2 = new DataColumn();
            column2.ColumnName = "FLDPARTDAMOUNT";
            dtpartd.Tables[0].Columns.Add(column2);
            for (int i = 0; i < 3; i++)
            {
                //ds.Tables[0].Rows[i]["FLDGROUPBY"] = ds.Tables[0].Rows[i]["FLDPART"].ToString();


                if (i == 0) { dtpartd.Tables[0].Rows.Add(); dtpartd.Tables[0].Rows[i]["FLDSNO"] = 1; dtpartd.Tables[0].Rows[i]["FLDHEAD"] = "Total Expenses Incurred "; dtpartd.Tables[0].Rows[i]["FLDPARTDAMOUNT"] = dr["FLDTOTALCLAIMABLE"].ToString(); }
                if (i == 1) { dtpartd.Tables[0].Rows.Add(); dtpartd.Tables[0].Rows[i]["FLDSNO"] = 2; dtpartd.Tables[0].Rows[i]["FLDHEAD"] = "Crew Deductible Amount"; dtpartd.Tables[0].Rows[i]["FLDPARTDAMOUNT"] = dr["FLDCREWDEDUCTABLE"].ToString(); }
                if (i == 2) { dtpartd.Tables[0].Rows.Add(); dtpartd.Tables[0].Rows[i]["FLDSNO"] = 3; dtpartd.Tables[0].Rows[i]["FLDHEAD"] = "Total Claimable Amount"; dtpartd.Tables[0].Rows[i]["FLDPARTDAMOUNT"] = dr["FLDTOTALEXPENSE"].ToString(); }

            }
            gvpartD.DataSource = dtpartd;
        }
    }

    private void BindCheckListData()
    {
        try
        {
            DataSet ds, ds1, ds2;
            ds = PhoenixInspectionPNI.PNITravelCheckListDetailsList
                 (new Guid(ViewState["PNIID"].ToString())
                 , General.GetNullableGuid(txtcrewplanid.Text));
            ds1 = PhoenixInspectionPNI.PNITravelCheckListDetailsListPartD
                (new Guid(ViewState["PNIID"].ToString()));
            ds2 = PhoenixInspectionPNI.PNITravelCheckListDetailsListPartE
               (new Guid(ViewState["PNIID"].ToString()));
            if (ds1.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds1.Tables[0].Rows[0];
                //DataTable dtpartd = new DataTable();
                //DataColumn column = new DataColumn();
                //column.ColumnName = "FLDHEAD";
                //dtpartd.Columns.Add(column);
                //DataColumn column1 = new DataColumn();
                //column1.ColumnName = "FLDSNO";
                //dtpartd.Columns.Add(column1);
                //DataColumn column2 = new DataColumn();
                //column2.ColumnName = "FLDPARTDAMOUNT";
                //dtpartd.Columns.Add(column2);
                //for (int i = 0; i < 3; i++)
                //{
                //    //ds.Tables[0].Rows[i]["FLDGROUPBY"] = ds.Tables[0].Rows[i]["FLDPART"].ToString();


                //    if (i == 0) { dtpartd.Rows.Add(); dtpartd.Rows[i]["FLDSNO"] = 1; dtpartd.Rows[i]["FLDHEAD"] = "Total Expenses Incurred "; dtpartd.Rows[i]["FLDPARTDAMOUNT"] = dr["FLDTOTALCLAIMABLE"].ToString(); }
                //    if (i == 1) { dtpartd.Rows.Add(); dtpartd.Rows[i]["FLDSNO"] = 2; dtpartd.Rows[i]["FLDHEAD"] = "Crew Deductible Amount"; dtpartd.Rows[i]["FLDPARTDAMOUNT"] = dr["FLDCREWDEDUCTABLE"].ToString(); }
                //    if (i == 2) { dtpartd.Rows.Add(); dtpartd.Rows[i]["FLDSNO"] = 3; dtpartd.Rows[i]["FLDHEAD"] = "Total Claimable Amount"; dtpartd.Rows[i]["FLDPARTDAMOUNT"] = dr["FLDTOTALEXPENSE"].ToString();}

                //}
                //gvpartD.DataSource = dtpartd;
                lblTotalClaim.Text = dr["FLDTOTALCLAIMABLE"].ToString();
                lblTotalDeduct.Text = dr["FLDCREWDEDUCTABLE"].ToString();
                lblTotalExpense.Text = dr["FLDTOTALEXPENSE"].ToString();
            }
            if (ds2.Tables[0].Rows.Count > 0)
            {
                gvChecklistPartE.DataSource = ds2;
                
            }

            string strPreviousRowID;
            if (ds.Tables[0].Rows.Count > 0)
            {
                strPreviousRowID = string.Empty;
                DataColumn column = new DataColumn();
                column.ColumnName = "FLDGROUPBY";
                ds.Tables[0].Columns.Add(column);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //ds.Tables[0].Rows[i]["FLDGROUPBY"] = ds.Tables[0].Rows[i]["FLDPART"].ToString();
                    if (ds.Tables[0].Rows[i]["FLDDEPARTMENT"].ToString() != "")
                        ds.Tables[0].Rows[i]["FLDGROUPBY"] = ds.Tables[0].Rows[i]["FLDPART"].ToString() + ":" + ds.Tables[0].Rows[i]["FLDDEPARTMENT"].ToString();

                }
                gvChecklist.DataSource = ds;

            }
            else
            {
                gvChecklist.DataSource = ds;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvChecklist_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = de.RowIndex;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {

        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void GetAgentCode()
    {
        if (ViewState["PNIID"] != null)
        {
            DataSet ds = PhoenixInspectionPNI.PNIDoctorReportEdit(new Guid(ViewState["PNIID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ViewState["txtsupcode"] = dr["FLDPORTAGENTCODE"].ToString();
            }


        }


    }





  
    protected void gvChecklistPartE_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;


        BindCheckListData();
    }

   

   
  
   
    private bool IsValidPartECheckList(decimal? amount, DateTime? date)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (amount == null)
            ucError.ErrorMessage = "Amount is Required.";

        if (date == null)
            ucError.ErrorMessage = "Date is Required.";

        return (!ucError.IsError);
    }

    protected void textcrewplanid_Changed(object sender, EventArgs e)
    {

        if (IsValidInspectionPNIOperation())
        {
            PhoenixInspectionPNI.PNITravelSignOnOffInsert(new Guid(ViewState["PNIID"].ToString()),
                                                        int.Parse(txtCrewId.Text.ToString()),
                                                        int.Parse(ucSignOffPort.SelectedSeaport),
                                                        DateTime.Parse(txtSignOffDate.Text),
                                                        int.Parse(txtAgent.Text.ToString()),
                                                        0, int.Parse(ddlMedicalCase.SelectedValue)
                                                        , General.GetNullableGuid(txtcrewplanid.Text)
                                                        , General.GetNullableInteger(txtemployeeid.Text)
                                                        , General.GetNullableInteger(ucSignOffPortonsigner.SelectedSeaport)
                                                        , General.GetNullableInteger(txtrankid.Text)

                                                        );
            //PhoenixInspectionPNI.PNITravelSignOnOffInsert(new Guid(ViewState["PNIID"].ToString()),
            //                                            int.Parse(txtCrewOnSignid.Text.ToString()),
            //                                            int.Parse(ucSignOnPort.SelectedSeaport),
            //                                            DateTime.Parse(txtSignOnDate.Text),
            //                                            int.Parse(txtAgentSignOn.Text.ToString()),
            //                                            1);


            BindCheckListData();

        }
        else
        {
            ucError.Visible = true;
            return;
        }

    }

    protected void Menupni_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

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
    protected void ShowExcel()
    {

        int iRowCount = 0;
        //int iTotalPageCount = 0;

        string[] alCaptions = {
                                "S.No",
                                "OFF-SIGNER",
                                "Department - OPERATIONS",
                                "Status",
                                "Currency",
                                "Actual Amount",
                                "Billable Amount",
                                "Amount in USD",


                              };

        string[] alColumns = {  "FLDROWNUMBER",
                                "FLDPART",
                                "FLDNAME",
                                "FLDSTATUS",
                                "FLDCURRENCY",
                                "FLDACTUALAMOUNT",
                                "FLDAMOUNT",
                                "FLDUSD",


                             };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds;
        ds = PhoenixInspectionPNI.PNITravelCheckListDetailsListReport
             (new Guid(ViewState["PNIID"].ToString())
             , 1);


        //        ds.Tables.Add(dt.Copy());

        Response.AddHeader("Content-Disposition", "attachment; filename=" + ViewState["REFNO"] + ".xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>PNI case Checklist for Operation / Welfare/Legal & Insurance /Accounts</h3></td>");
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

    protected void gvChecklist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != "")
        {

            BindCheckListData();

        }
    }

    protected void gvChecklist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;



            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidCheckList(General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAmountEdit")).Text),
                                    General.GetNullableInteger(((UserControlCurrency)e.Item.FindControl("ucCurrency")).SelectedCurrency),
                                    ((RadComboBox)e.Item.FindControl("ddlstatus")).SelectedValue))
                {
                    decimal billableamt = decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucAmountEdit")).Text);
                    decimal actualamount = decimal.Parse(((RadLabel)e.Item.FindControl("lblActualAmount")).Text);

                    if (actualamount == 0 || actualamount >= billableamt)
                    {

                        PhoenixInspectionPNI.PNITravelCheckListDetailsInsert(
                                                                  new Guid(((RadLabel)e.Item.FindControl("lblID")).Text),
                                                                  new Guid(ViewState["PNIID"].ToString()),
                                                                  int.Parse(((RadComboBox)e.Item.FindControl("ddlStatus")).SelectedValue),
                                                                  decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucAmountEdit")).Text),
                                                                  int.Parse(((UserControlCurrency)e.Item.FindControl("ucCurrency")).SelectedCurrency)
                                                                  );
                    }
                    else
                    {
                        ucError.Text = "Billable amount should be less than or equal to actual amount";
                        ucError.Visible = true;
                        return;
                    }

                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
                BindCheckListData();
                gvChecklist.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (IsValidCheckList(General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAmountEdit")).Text),
                                     General.GetNullableInteger(((UserControlCurrency)e.Item.FindControl("ucCurrency")).SelectedCurrency),
                                     ((RadComboBox)e.Item.FindControl("ddlstatus")).SelectedValue))
                {
                    string aa = (((RadLabel)e.Item.FindControl("lblID")).Text);
                    decimal billableamt = decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucAmountEdit")).Text);
                    decimal actualamount = decimal.Parse(((RadLabel)e.Item.FindControl("lblActualAmount")).Text);

                    if (actualamount == 0 || actualamount >= billableamt)
                    {


                        PhoenixInspectionPNI.PNITravelCheckListDetailsInsert(
                                                                        new Guid(((RadLabel)e.Item.FindControl("lblID")).Text),
                                                                        new Guid(ViewState["PNIID"].ToString()),
                                                                        int.Parse(((RadComboBox)e.Item.FindControl("ddlStatus")).SelectedValue),
                                                                        decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucAmountEdit")).Text),
                                                                        int.Parse(((UserControlCurrency)e.Item.FindControl("ucCurrency")).SelectedCurrency)
                                                                        );
                    }
                    else
                    {
                        ucError.Text = "Billable amount should be less than or equal to actual amount";
                        ucError.Visible = true;
                        return;
                    }
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
               
                gvChecklist.Rebind();
                BindCheckListData();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                string aa = (((RadLabel)e.Item.FindControl("lblIDEdit")).Text);
                PhoenixInspectionPNI.PNITravelCheckListDetailsDelete(new Guid(ViewState["PNIID"].ToString()),
                                                                new Guid(((RadLabel)e.Item.FindControl("lblIDEdit")).Text));
                BindCheckListData();
                gvChecklist.Rebind();
            }

            
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvChecklist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            //GridView gv = (GridView)sender;
            
            if (e.Item is GridDataItem)
            {

                UserControlCurrency ucCurrency = (UserControlCurrency)e.Item.FindControl("ucCurrency");
                if (ucCurrency != null)
                {
                    ucCurrency.CurrencyList = PhoenixRegistersCurrency.ListCurrency(null);
                    ucCurrency.DataBind();
                    ucCurrency.SelectedCurrency = ((RadLabel)e.Item.FindControl("lblCurrencyID")).Text;
                }
                RadComboBox status = (RadComboBox)e.Item.FindControl("ddlStatus");
                if (status != null)
                {
                    status.SelectedValue = ((RadLabel)e.Item.FindControl("lblStatusEdit")).Text;
                }

            }
            if (e.Item is GridDataItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                    // db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }
                RadLabel lblDtkey = (RadLabel)e.Item.FindControl("lblDTKey");
                ImageButton cmdAttachment = (ImageButton)e.Item.FindControl("cmdAttachment");
                if (cmdAttachment != null)
                {
                    cmdAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text.ToString() + "&mod="
                                        + PhoenixModule.QUALITY + "');return true;");
                    cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
                }
                ImageButton cmdNoAttachment = (ImageButton)e.Item.FindControl("cmdNoAttachment");
                if (cmdNoAttachment != null)
                {
                    cmdNoAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text.ToString() + "&mod="
                                        + PhoenixModule.QUALITY + "');return true;");
                    cmdNoAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdNoAttachment.CommandName);
                }

                DataRowView drv = (DataRowView)e.Item.DataItem;
                ImageButton iab = (ImageButton)e.Item.FindControl("cmdAttachment");
                ImageButton inab = (ImageButton)e.Item.FindControl("cmdNoAttachment");
                LinkButton idel = (LinkButton)e.Item.FindControl("cmdDelete");
                if (iab != null) iab.Visible = true;
                if (inab != null) inab.Visible = false;
                int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());
                if (n == 0)
                {
                    if (iab != null) iab.Visible = false;
                    if (inab != null) inab.Visible = true;

                }

                RadLabel lblAmountInUSD = (RadLabel)e.Item.FindControl("lblAmountInUSD");

                if (lblAmountInUSD != null && lblAmountInUSD.Text == "")
                {
                    iab.Visible = false;
                    inab.Visible = false;
                    idel.Visible = false;
                }
            }
            if (e.Item is GridDataItem)
            {
                RadLabel lbtn = (RadLabel)e.Item.FindControl("lblAmountInUSD");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipUSD1");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

            }
            UserControlCommonToolTip ucCommonToolTip = (UserControlCommonToolTip)e.Item.FindControl("ucCommonToolTip");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvpartD_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
       

           // binddatapartd();

       
    }

    protected void gvChecklistPartE_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != "")
        {

            BindCheckListData();

        }
    }

    protected void gvChecklistPartE_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

         

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidPartECheckList(General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAmountInUSDPartEEdit")).Text),
                                    General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucDate1PartE")).Text)))
                {
                    PhoenixInspectionPNI.PNITravelCheckListDetailsInsert(
                                                                new Guid(((RadLabel)e.Item.FindControl("lblIDEEdit")).Text),
                                                                new Guid(ViewState["PNIID"].ToString()),
                                                                 DateTime.Parse(((UserControlDate)e.Item.FindControl("ucDate1PartE")).Text),
                                                                decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucAmountInUSDPartEEdit")).Text)
                                                                    );

                }
                BindCheckListData();
                gvChecklistPartE.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (IsValidPartECheckList(General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAmountInUSDPartEEdit")).Text),
                                     General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucDate1PartE")).Text)))
                {
                    string aa = (((RadLabel)e.Item.FindControl("lblIDEEdit")).Text);
                    PhoenixInspectionPNI.PNITravelCheckListDetailsInsert(
                                                                    new Guid(((RadLabel)e.Item.FindControl("lblIDEEdit")).Text),
                                                                    new Guid(ViewState["PNIID"].ToString()),
                                                                     DateTime.Parse(((UserControlDate)e.Item.FindControl("ucDate1PartE")).Text),
                                                                    decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucAmountInUSDPartEEdit")).Text)
                                                                         );
                }
                else
                {
                    ucError.Visible = true;
                }
                BindCheckListData();
                gvChecklistPartE.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                string aa = (((RadLabel)e.Item.FindControl("lblIDE")).Text);
                PhoenixInspectionPNI.PNITravelCheckListDetailsPartEDelete(new Guid(ViewState["PNIID"].ToString()),
                                                                new Guid(((RadLabel)e.Item.FindControl("lblIDE")).Text));
                BindCheckListData();
                gvChecklistPartE.Rebind();
            }
           
           
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvChecklistPartE_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

           
            if (e.Item is GridDataItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                    // db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }
                RadLabel lblDTKeyE = (RadLabel)e.Item.FindControl("lblDTKeyE");
                ImageButton cmdAttachment = (ImageButton)e.Item.FindControl("cmdAttachment");
                if (cmdAttachment != null)
                {
                    cmdAttachment.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKeyE.Text.ToString() + "&mod="
                                        + PhoenixModule.QUALITY + "');return true;");
                    cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
                }
                ImageButton cmdNoAttachment = (ImageButton)e.Item.FindControl("cmdNoAttachment");
                if (cmdNoAttachment != null)
                {
                    cmdNoAttachment.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKeyE.Text.ToString() + "&mod="
                                        + PhoenixModule.QUALITY + "');return true;");
                    cmdNoAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdNoAttachment.CommandName);
                }

                DataRowView drv = (DataRowView)e.Item.DataItem;
                ImageButton iab = (ImageButton)e.Item.FindControl("cmdAttachment");
                ImageButton inab = (ImageButton)e.Item.FindControl("cmdNoAttachment");
                LinkButton idel = (LinkButton)e.Item.FindControl("cmdDelete");
                if (iab != null) iab.Visible = true;
                if (inab != null) inab.Visible = false;
                int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());
                if (n == 0)
                {
                    if (iab != null) iab.Visible = false;
                    if (inab != null) inab.Visible = true;

                }
                RadLabel lblAmountInUSD = (RadLabel)e.Item.FindControl("lblAmountInUSDPartE");
                if (lblAmountInUSD != null && lblAmountInUSD.Text == "")
                {
                    iab.Visible = false;
                    inab.Visible = false;
                    //  idel.Visible = false;
                }
            }
            if (e.Item is GridDataItem)
            {
                RadLabel lbtn = (RadLabel)e.Item.FindControl("lblAmountInUSDPartE");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipUSD2");
                if (uct != null)
                {
                    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }

            }



        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvChecklistPartE_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvChecklist_SortCommand(object sender, GridSortCommandEventArgs e)
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
