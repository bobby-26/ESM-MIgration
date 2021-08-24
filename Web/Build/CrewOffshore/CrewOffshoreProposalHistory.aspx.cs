using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Data;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CrewOffshoreProposalHistory : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvProposalHistory.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$lnkDoubleClick");
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$lnkDoubleClickEdit");
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            if (Request.QueryString["newapplicant"] != null)
            {
                ViewState["empid"] = Filter.CurrentNewApplicantSelection;
                ViewState["type"] = "n";
            }
            else
            {
                ViewState["empid"] = Filter.CurrentCrewSelection;
                ViewState["type"] = "p";
            }

            SetEmployeePrimaryDetails();
            gvProposalHistory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreProposalHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvProposalHistory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuProposalHistory.AccessRights = this.ViewState;
        MenuProposalHistory.MenuList = toolbar.Show();
        //MenuProposalHistory.SetTrigger(pnlProposalHistoryEntry);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        //BindData();
        //SetPageNavigator();
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        CrewCompExpTab.AccessRights = this.ViewState;
        CrewCompExpTab.MenuList = toolbarmain.Show();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = new DataTable();
        string[] alColumns = { "FLDVESSELNAME", "FLDSTATUS", "FLDUPDATEDBY", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Vessel", "Status", "Updated By", "Updated Date" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        dt = PhoenixCrewOffshoreCrewChange.SearchProposals(
            General.GetNullableInteger(ViewState["empid"].ToString())
            , sortexpression, sortdirection
            , 1
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.ShowExcel("Proposal History", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void ProposalHistory_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDSTATUS", "FLDUPDATEDBY", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Vessel", "Status", "Updated By", "Updated Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewOffshoreCrewChange.SearchProposals(
            General.GetNullableInteger(ViewState["empid"].ToString())
            , sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"]
            , gvProposalHistory.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvProposalHistory", "Proposal History", alCaptions, alColumns, ds);
        gvProposalHistory.DataSource = ds;
        gvProposalHistory.VirtualItemCount = iRowCount;

       

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(ViewState["empid"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }    

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        
    }



    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvProposalHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvProposalHistory.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProposalHistory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;
        
            if (e.CommandName.ToUpper() == "NAVIGATE")
            {
                string crewplanid = ((RadLabel)e.Item.FindControl("lblCrewPlanID")).Text;
                string crewplandtkey = ((RadLabel)e.Item.FindControl("lblCrewPlanDTKey")).Text;

                if (ViewState["type"].ToString().Equals("n"))
                {
                    Response.Redirect("../CrewOffshore/CrewOffshoreApprovalRejection.aspx?empid=" + ViewState["empid"].ToString()
                        + "&crewplanid=" + crewplanid
                        + "&newapplicant=true"
                        + "&crewplandtkey=" + crewplandtkey
                        + "&calledfrom=proposalhistory");
                }
                else
                {
                    Response.Redirect("../CrewOffshore/CrewOffshoreApprovalRejection.aspx?empid=" + ViewState["empid"].ToString()
                        + "&crewplanid=" + crewplanid
                        + "&personalmaster=true"
                        + "&crewplandtkey=" + crewplandtkey
                        + "&calledfrom=proposalhistory");
                }
            }
            if (e.CommandName == "Page")
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

    protected void gvProposalHistory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            UserControlToolTip ucToolTipWD = (UserControlToolTip)e.Item.FindControl("ucToolTipWD");
            RadLabel lblCrewPlanID = (RadLabel)e.Item.FindControl("lblCrewPlanID");
            LinkButton cmdDocChecklist = (LinkButton)e.Item.FindControl("cmdDocChecklist");
            RadLabel lblWD = (RadLabel)e.Item.FindControl("lblWD");
            LinkButton imgWD = (LinkButton)e.Item.FindControl("imgWD");
            if (cmdDocChecklist != null)
            {
                cmdDocChecklist.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','"+Session["sitepath"]+"/Options/OptionsOffshoreCrewDocumentCheckList.aspx?Crewplanid=" + lblCrewPlanID.Text +
                   "&Office=1" + "&cmdname=THREEAFORMUPLOAD'); return false;");

            }

            HtmlGenericControl html = new HtmlGenericControl();
            if (imgWD != null)
            {
                if (lblWD != null)
                {
                    if (lblWD.Text != "")
                    {
                        //imgWD.Visible = true;
                       // imgWD.ImageUrl = Session["images"] + "/te_view.png";
                        if (ucToolTipWD != null)
                        {
                            ucToolTipWD.Position = ToolTipPosition.TopCenter;
                            ucToolTipWD.TargetControlId = imgWD.ClientID;
                            //imgWD.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipWD.ToolTip + "', 'visible');");
                            //imgWD.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipWD.ToolTip + "', 'hidden');");
                        }
                    }
                    else
                    {
                        html.InnerHtml = "<span class=\"icon\" style=\"color: gray;\"><i class=\"fas fa-glasses\"></i></span>";
                        imgWD.Controls.Add(html);
                    }
                        
                }
            }

            LinkButton imgRC = (LinkButton)e.Item.FindControl("imgRC");
            if (imgRC != null)
            {
                if (dr["FLDREJECTIONCOMMENTSYN"].ToString().Equals("1") && dr["FLDPDSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 99, "APR"))
                {
                    if (imgRC != null) imgRC.Visible = true;
                }
                else
                {
                    if (imgRC != null) imgRC.Visible = false;
                }
            }
        }
    }
}
