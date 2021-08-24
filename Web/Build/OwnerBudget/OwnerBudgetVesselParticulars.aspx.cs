using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.OwnerBudget;
using Telerik.Web.UI;

public partial class OwnerBudgetVesselParticulars : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Report", "REPORT", ToolBarDirection.Right);
        toolbar.AddButton("Technical", "TECHNICAL", ToolBarDirection.Right);
        toolbar.AddButton("Luboil", "LUBOIL", ToolBarDirection.Right);
        toolbar.AddButton("Crew Expense", "EXPENSE", ToolBarDirection.Right);
        toolbar.AddButton("Crew Wages", "CREWWAGE", ToolBarDirection.Right);
        toolbar.AddButton("Particulars", "PARTICULARS", ToolBarDirection.Right);
        toolbar.AddButton("Revisions", "REVISION", ToolBarDirection.Right);
        toolbar.AddButton("Proposals", "PROPOSALS", ToolBarDirection.Right);

        MenuParticulars.AccessRights = this.ViewState;
        MenuParticulars.MenuList = toolbar.Show();
        MenuParticulars.SelectedMenuIndex = 5;

        PhoenixToolbar toolbarsave = new PhoenixToolbar();
        toolbarsave.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuAddEditParticulars.AccessRights = this.ViewState;
        MenuAddEditParticulars.MenuList = toolbarsave.Show();

        if (!IsPostBack)
        {
            BindYear();
            if (Request.QueryString["proposalid"] != null)
            {
                ViewState["PROPOSALID"] = Request.QueryString["proposalid"].ToString();
                BindData();
            }
            if (Request.QueryString["revisionid"] != null)
            {
                ViewState["REVISIONID"] = Request.QueryString["revisionid"].ToString();
            }
            cmdCrewComplement.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/OwnerBudget/OwnerBudgetRankOwnerScaleMapping.aspx?proposalid=" + ViewState["PROPOSALID"] + "');return false;");
        }
        BindExpensesMappingData();
    }

    protected void BindYear()
    {
        RadComboBoxItem lid = new RadComboBoxItem("--Select--", "Dummy");
        ddlYear.Items.Add(lid);

        for (int i = (DateTime.Today.Year - 10); i <= (DateTime.Today.Year) + 5; i++)
        {
            RadComboBoxItem li = new RadComboBoxItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
    }

    private void BindData()
    {
        DataSet ds = PhoenixOwnerBudget.BudgetVesselParticularsEdit(new Guid(ViewState["PROPOSALID"].ToString()));
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();

        ucPortOfRotation.QuickTypeCode = "115";
        ucPortOfRotation.bind();

        if (ds.Tables[0].Rows.Count > 0)
        {
            dt = ds.Tables[0];
            ucDate.Text = dt.Rows[0]["FLDDATE"].ToString();
            ucFlag.SelectedFlag = dt.Rows[0]["FLDFLAGID"].ToString();            
            ucDWT.Text = dt.Rows[0]["FLDDWT"].ToString();
            ucVesselName.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
            ucGRT.Text = dt.Rows[0]["FLDGRT"].ToString();
            ucVesselType.SelectedVesseltype = dt.Rows[0]["FLDVESSELTYPE"].ToString();
            ucNoOfHoldsTank.Text = dt.Rows[0]["FLDNOOFTANKS"].ToString();
            if (dt.Rows[0]["FLDYEARBUILD"].ToString() != "" && dt.Rows[0]["FLDYEARBUILD"].ToString() != null)
                ddlYear.SelectedValue = dt.Rows[0]["FLDYEARBUILD"].ToString();
            else
                ddlYear.SelectedValue= "Dummy";

            ucNoOfHatches.Text = dt.Rows[0]["FLDNOOFHATCHES"].ToString();
            rblstITFCovered.SelectedValue = dt.Rows[0]["FLDITFCOVEREDYN"].ToString();
            ucNationality.SelectedCountry = dt.Rows[0]["FLDNATIONALITY"].ToString();
            ucOverTimeHours.Text = dt.Rows[0]["FLDOVERTIMEHOURS"].ToString();
            ucPortOfRotation.SelectedQuick = dt.Rows[0]["FLDROTATIONPORT"].ToString();
            ucEngineType.SelectedEngineName = dt.Rows[0]["FLDENGINETYPE"].ToString();
            ucBHP.Text = dt.Rows[0]["FLDBHP"].ToString();
            ucNoOfSailingDay.Text = dt.Rows[0]["FLDSAILINGDAYPERANNUM"].ToString();
            ucEngineRunAT.Text = dt.Rows[0]["FLDENGINERUNAT"].ToString();
            rblstMEStrokeType.SelectedValue = dt.Rows[0]["FLDMESTROKETYPE"].ToString();
            rblstVesselInspected.SelectedValue = dt.Rows[0]["FLDVESSELINSPECTEDYN"].ToString();
            rblstNewBuilding.SelectedValue = dt.Rows[0]["FLDNEWBUILDINGYN"].ToString();
           
            ucDockingInYear.Text = dt.Rows[0]["FLDDOCKINGINYEAR"].ToString();
            rblstChinnaYard.SelectedValue = dt.Rows[0]["FLDCHINAYARDYN"].ToString();
            ucShipYard.SelectedAddress = dt.Rows[0]["FLDSHIPYARD"].ToString();
            ucSOFFRequired.Text = dt.Rows[0]["FLDSOFFREQUIRED"].ToString();
            ucJOFFRequired.Text = dt.Rows[0]["FLDJOFFREQUIRED"].ToString();
            ucTRARequired.Text = dt.Rows[0]["FLDTRAREQUIRED"].ToString();
            ucRATRequired.Text = dt.Rows[0]["FLDRATREQUIRED"].ToString();
            ucSOFFContractPeriod.Text = dt.Rows[0]["FLDSOFFCONTRACT"].ToString();
            ucJOFFContractPeriod.Text = dt.Rows[0]["FLDJOFFCONTRACT"].ToString();
            ucTRAContractPeriod.Text = dt.Rows[0]["FLDTRACONTRACT"].ToString();
            ucRATContractPeriod.Text = dt.Rows[0]["FLDRATCONTRACT"].ToString();
            rblstJapaneseOwner.SelectedValue = dt.Rows[0]["FLDJAPANESEOWNERYN"].ToString();
            ucDockingInYear2.Text = dt.Rows[0]["FLDDOCKINGINYEAR2"].ToString();
            ucDockingInYear3.Text = dt.Rows[0]["FLDDOCKINGINYEAR3"].ToString();
            ucDockingInYear4.Text = dt.Rows[0]["FLDDOCKINGINYEAR4"].ToString();
            ucDockingInYear5.Text = dt.Rows[0]["FLDDOCKINGINYEAR5"].ToString();
            txtNameOfOwner.Text = dt.Rows[0]["FLDOWNERNAME"].ToString();
            txtVictuallingCost.Text = dt.Rows[0]["FLDVICTUALLINGCOST"].ToString();

            ddlManagementType.SelectedValue = dt.Rows[0]["FLDMANAGEMENTTYPE"].ToString();
            ucSupdtAttendance.Text = dt.Rows[0]["FLDSUPDTATTENDANCE"].ToString();
           

            if (dt.Rows[0]["FLDNATIONALITY"].ToString() == "")
                ucNationality.SelectedCountry = "97";

            if ((dt.Rows[0]["FLDVESSELID"].ToString() == "") || (dt.Rows[0]["FLDVESSELID"].ToString() == "Dummy"))
            {
                ucNameofOwner.Visible = false;
                txtNameOfOwner.Visible = true;               
            }
            else
            {
                txtNameOfOwner.Visible = false;
                ucNameofOwner.Visible = true;
                ucNameofOwner.SelectedAddress = dt.Rows[0]["FLDOWNERID"].ToString();
            }
        }
        else
            if (ds.Tables[1].Rows.Count > 0)
            {
                dt1 = ds.Tables[1];
                ucVesselName.Text = dt1.Rows[0]["FLDVESSELNAME"].ToString();
                ucDate.Text = dt1.Rows[0]["FLDPROPOSALDATE"].ToString();
                ucNationality.SelectedCountry = "97";

                if ((dt1.Rows[0]["FLDVESSELID"].ToString() == "") || (dt1.Rows[0]["FLDVESSELID"].ToString() == "Dummy"))
                {
                    ucNameofOwner.Visible = false;
                    txtNameOfOwner.Visible = true;
                }
                else
                {
                    txtNameOfOwner.Visible = false;
                    ucNameofOwner.Visible = true;
                }
            }
    }
    protected void MenuParticulars_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("PROPOSALS"))
        {
            Response.Redirect("OwnerBudgetProposal.aspx");
        }
        if (CommandName.ToUpper().Equals("REVISIONS"))
        {
            Response.Redirect("OwnerBudgetProposalRevision.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"]);
        }
        if (CommandName.ToUpper().Equals("CREWWAGE"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetProposedCrewWages.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"]);
        }
        if (CommandName.ToUpper().Equals("TECHNICAL"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetTechnicalProposal.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"]);
        }
        if (CommandName.ToUpper().Equals("EXPENSE"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetCrewExpense.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
        }
        if (CommandName.ToUpper().Equals("REPORT"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetExpenseReport.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
        }
        if (CommandName.ToUpper().Equals("LUBOIL"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetLubOil.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
        }
    }

    protected void MenuAddEditParticulars_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (ViewState["PROPOSALID"] != null)
            {
                string ownername = "";
                ownername = (ucNameofOwner.SelectedAddress == "Dummy") || (ucNameofOwner.SelectedAddress == null) ? txtNameOfOwner.Text : "";

                PhoenixOwnerBudget.BudgetVesselParticularsUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["PROPOSALID"].ToString()),
                                                                         General.GetNullableInteger(""), ucVesselName.Text,
                                                                         General.GetNullableInteger(ucVesselType.SelectedVesseltype.ToString()),
                                                                         General.GetNullableInteger(ddlYear.SelectedValue), General.GetNullableDateTime(ucDate.Text),
                                                                         General.GetNullableInteger(ucNameofOwner.SelectedAddress), General.GetNullableInteger(ucFlag.SelectedFlag),
                                                                         General.GetNullableDecimal(ucDWT.Text), General.GetNullableDecimal(ucGRT.Text),
                                                                         General.GetNullableInteger(ucNoOfHoldsTank.Text), General.GetNullableInteger(ucNoOfHatches.Text),
                                                                         General.GetNullableInteger(rblstITFCovered.SelectedValue), General.GetNullableInteger(ucNationality.SelectedCountry),
                                                                         General.GetNullableInteger(ucOverTimeHours.Text), General.GetNullableInteger(ucPortOfRotation.SelectedQuick),
                                                                         General.GetNullableInteger(ucEngineType.SelectedEngineName), General.GetNullableDecimal(ucBHP.Text),
                                                                         General.GetNullableInteger(ucNoOfSailingDay.Text), General.GetNullableInteger(ucEngineRunAT.Text),
                                                                         General.GetNullableInteger(rblstMEStrokeType.SelectedValue), General.GetNullableInteger(rblstVesselInspected.SelectedValue),
                                                                         General.GetNullableInteger(rblstNewBuilding.SelectedValue), null,
                                                                         null, null,
                                                                         General.GetNullableInteger(ucSOFFRequired.Text), General.GetNullableInteger(ucJOFFRequired.Text),
                                                                         General.GetNullableInteger(ucTRARequired.Text), General.GetNullableInteger(ucRATRequired.Text),
                                                                         General.GetNullableInteger(ucSOFFContractPeriod.Text), General.GetNullableInteger(ucJOFFContractPeriod.Text),
                                                                         General.GetNullableInteger(ucTRAContractPeriod.Text), General.GetNullableInteger(ucRATContractPeriod.Text),
                                                                         General.GetNullableInteger(ucDockingInYear.Text), General.GetNullableInteger(ucDockingInYear2.Text),
                                                                         General.GetNullableInteger(ucDockingInYear3.Text), General.GetNullableInteger(ucDockingInYear4.Text),
                                                                         General.GetNullableInteger(ucDockingInYear5.Text), General.GetNullableInteger(rblstJapaneseOwner.SelectedValue),
                                                                         General.GetNullableString(ownername),
                                                                         General.GetNullableInteger(rblstChinnaYard.SelectedValue),
                                                                         General.GetNullableInteger(ucShipYard.SelectedAddress),
                                                                         General.GetNullableDecimal(txtVictuallingCost.Text),
                                                                         null,
                                                                         null,
                                                                         null,
                                                                         null,
                                                                         null,
                                                                         General.GetNullableInteger(ddlManagementType.SelectedValue),   
                                                                         General.GetNullableInteger(ucSupdtAttendance.Text)
                                                                         );
                ucStatus.Text = "Vessel particulars updated";
            }
            BindData();
        }
    }
    private void BindExpensesMappingData()
    {
        DataSet ds = new DataSet();

        ds = PhoenixOwnerBudgetRegisters.ProposalExpensesMappingList(new Guid(ViewState["PROPOSALID"].ToString()));

       
            gvContactType.DataSource = ds;
            gvContactType.DataBind();
       

    }
    
    protected void gvContactType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

          
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixOwnerBudgetRegisters.ProposalExpensesMappingUpdate(new Guid(((RadLabel)e.Item.FindControl("lblAllowanceId")).Text.ToString())
                                                           , int.Parse(((RadRadioButtonList)e.Item.FindControl("rblstYesNo")).SelectedValue)
                                                           );
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
            }
            BindExpensesMappingData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvContactType_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvContactType_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridEditableItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null && del.Visible == true) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

            }
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadRadioButtonList rblstYesNo = (RadRadioButtonList)e.Item.FindControl("rblstYesNo");
            if (rblstYesNo != null)
            {
                rblstYesNo.SelectedValue = drv["FLDYESNO"].ToString();
            }
        
        }
        


    }
    protected void gvContactType_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        BindExpensesMappingData();
        
    }
    protected void gvContactType_UpdateCommand(object sender, GridCommandEventArgs e)
    {

    }
   
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvContactType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvContactType.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}


