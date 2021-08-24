using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.CrewCommon;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewFamilyTravel : PhoenixBasePage
{
    private static string strEmployeeId;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["EDITROW"] = "0";
                ViewState["CURRENTROW"] = null;
                
                if (Request.QueryString["empid"] != null)
                    ViewState["empid"] = Request.QueryString["empid"].ToString();
                if (Request.QueryString["from"] != null)
                    ViewState["from"] = Request.QueryString["from"].ToString();               
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            cmdHiddenPick.Attributes.Add("style", "display:none");

            if (ViewState["from"].ToString() == "familynok")
            {
                toolbar.AddButton("Family NOK", "FAMILYNOK");
                toolbar.AddButton("Sign On/Off", "SIGNON");                
                toolbar.AddButton("Documents", "DOCUMENTS");                
                toolbar.AddButton("Travel", "TRAVEL");
                CrewMenu.AccessRights = this.ViewState;
                CrewMenu.MenuList = toolbar.Show();

                toolbar = new PhoenixToolbar();                
                toolbar.AddFontAwesomeButton("../Crew/CrewFamilyTravel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
                toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCCT')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
                MenuBreakUpAssign.AccessRights = this.ViewState;
                MenuBreakUpAssign.MenuList = toolbar.Show();

                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Generate Travel Plan", "SAVE",ToolBarDirection.Right);                
                MenuGenerateTravel.AccessRights = this.ViewState;              
                MenuGenerateTravel.MenuList = toolbar.Show();

                strEmployeeId = Filter.CurrentCrewSelection;                

            }

            if (ViewState["from"].ToString() == "travel")
            {
                strEmployeeId = ViewState["empid"].ToString();

                toolbar.AddButton("Travel Request", "TRAVELREQUEST");
                CrewMenu.MenuList = toolbar.Show();

                toolbar = new PhoenixToolbar();
                toolbar.AddFontAwesomeButton("../Crew/CrewFamilyTravel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
                toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCCT')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
                MenuBreakUpAssign.AccessRights = this.ViewState;
                MenuBreakUpAssign.MenuList = toolbar.Show();
            }
           
            BindDropDownVessel();
            CrewMenu.SelectedMenuIndex = 3;
           

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FAMILYNOK"))
            {
                Response.Redirect("CrewFamilyNok.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("MEDICAL"))
            {
                Response.Redirect("CrewFamilyMedicalDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("SIGNON"))
            {
                Response.Redirect("CrewFamilySignOn.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("OTHERDOCUMENT"))
            {
                Response.Redirect("CrewFamilyOtherDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTS"))
            {
                Response.Redirect("CrewFamilyTravelDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void GenerateTravel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                StringBuilder strFamilylist = new StringBuilder();

                foreach (GridDataItem item in gvCCT.MasterTableView.Items)
                {
               
                    RadCheckBox chkPlanFamily = (RadCheckBox)item.FindControl("chkPlanFamily");
               
                    if (chkPlanFamily.Checked == true)
                    {

                        RadLabel lblFamilyId = (RadLabel)item.FindControl("lblFamilyId");
                        strFamilylist.Append(lblFamilyId.Text);
                        strFamilylist.Append(",");
                    }


                }
                if (!IsValidTrvelRequest(strFamilylist.ToString(), ddlProposedVessel.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }

                if (strFamilylist.ToString().Trim() != "")
                {
                    PhoenixCrewTravelRequest.InsertFamilyTravelRequest(
                        Convert.ToInt32(strEmployeeId),
                        strFamilylist.ToString(), General.GetNullableInteger(ddlProposedVessel.SelectedValue)
                    );

                    BindData();
                    gvCCT.Rebind();
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


    protected void BindDropDownVessel()
    {
        ddlProposedVessel.Items.Clear();
        ddlProposedVessel.DataSource = PhoenixCrewTravelRequest.EmployeeTravelVesselList(General.GetNullableInteger(strEmployeeId));
        ddlProposedVessel.DataTextField = "FLDVESSELNAME";
        ddlProposedVessel.DataValueField = "FLDVESSELID";
        ddlProposedVessel.DataBind();

        BindData();
    }


    protected void gvCCT_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCCT.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvCCT_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvCCT_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SORT")
            return;

        if (e.CommandName.ToUpper() == "SELECT")
        {
            
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDROWNUMBER", "FLDNAME","FLDVESSELNAME","FLDONSIGNERYESNO", "FLDDATEOFBIRTH", "FLDPASSPORTNUMBER", "FLDOTHERVISADETAILS",
                                "FLDORIGIN", "FLDDESTINATION", "FLDTRAVELDATE", "FLDARRIVALDATE"};
            string[] alCaptions = { "S.No", "Name","Vessel", "On/Off-Signer", "D.O.B.", "PP No", "VISA Details", "Origin", "Destination", "Departure",
                                    "Arrival"};

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewTravelRequest.SearchFamilyTravelRequest(
                Convert.ToInt32(strEmployeeId),
                General.GetNullableGuid(Request.QueryString["travelid"] != null ? Request.QueryString["travelid"].ToString() : ""),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvCCT.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                General.GetNullableInteger(ddlProposedVessel.SelectedValue));

            General.SetPrintOptions("gvCCT", "Travel Plan", alCaptions, alColumns, ds);

            gvCCT.DataSource = ds.Tables[0];
            gvCCT.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
    protected void ShowExcelTravelPlan()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDNAME","FLDVESSELNAME", "FLDONSIGNERYESNO", "FLDDATEOFBIRTH", "FLDPASSPORTNUMBER", "FLDOTHERVISADETAILS",
                                "FLDORIGIN", "FLDDESTINATION", "FLDTRAVELDATE", "FLDARRIVALDATE"};
        string[] alCaptions = { "S.No", "Name","Vessel", "On/Off-Signer", "D.O.B.", "PP No", "VISA Details", "Origin", "Destination", "Departure",
                                    "Arrival"};

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixCrewTravelRequest.SearchFamilyTravelRequest(
                Convert.ToInt32(strEmployeeId),
                General.GetNullableGuid(Request.QueryString["travelid"] != null ? Request.QueryString["travelid"].ToString() : ""),
                sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("Travel_Plan", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        
    }

    protected void BreakUpAssign_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelTravelPlan();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidTrvelRequest(string strFamilylist, string vessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(strFamilylist))
            ucError.ErrorMessage = "Please select atleast one family member.";

        if (General.GetNullableInteger(vessel) == null || vessel == "")
            ucError.ErrorMessage = "Vessel is required";

        return (!ucError.IsError);
    }
    
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
      

    }



}
