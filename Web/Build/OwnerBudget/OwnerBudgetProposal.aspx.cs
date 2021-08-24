using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.OwnerBudget;
using Telerik.Web.UI;

public partial class OwnerBudgetProposal : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbarbudgetproposal = new PhoenixToolbar();
        toolbarbudgetproposal.AddFontAwesomeButton("../OwnerBudget/OwnerBudgetProposal.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        toolbarbudgetproposal.AddFontAwesomeButton("javascript:CallPrint('gvBudgetProposal')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbarbudgetproposal.AddFontAwesomeButton("../OwnerBudget/OwnerBudgetProposal.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbarbudgetproposal.AddFontAwesomeButton("../OwnerBudget/OwnerBudgetProposal.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");
        toolbarbudgetproposal.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','Proposal Add/Edit','OwnerBudget/OwnerBudgetProposalAddEdit.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDPROPOSAL");        
        MenuBudgetProposalTap.AccessRights = this.ViewState;
        MenuBudgetProposalTap.MenuList = toolbarbudgetproposal.Show();


        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvBudgetProposal.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            BindData();
        }
        BindData();
    }
    private void ClearFilter()
    {
        txtVesselName.Text = "";
        txtProposalTitle.Text = "";
        ucFromProposedDate.Text = "";
        ucToProposedDate.Text = "";
        BindData();
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
        ds = PhoenixOwnerBudget.BudgetProposalSearch(General.GetNullableString(txtVesselName.Text)
                                                                , General.GetNullableString(txtProposalTitle.Text)
                                                                , General.GetNullableDateTime(ucFromProposedDate.Text)
                                                                , General.GetNullableDateTime(ucToProposedDate.Text)
                                                                , gvBudgetProposal.CurrentPageIndex+1
                                                                , gvBudgetProposal.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                );

        
        string[] alCaptions = { "Vessel Name","Proposal Title","Proposal Date"};
        string[] alColumns = { "FLDVESSELNAME", "FLDPROPOSALTITLE", "FLDPROPOSALDATE"};
        General.SetPrintOptions("gvBudgetProposal", "Budget Proposal", alCaptions, alColumns,ds);
    
            gvBudgetProposal.DataSource = ds;
        gvBudgetProposal.VirtualItemCount = iRowCount;
     
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvBudgetProposal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBudgetProposal.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BudgetProposal_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            BindData();
        }
        if (CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
            gvBudgetProposal.Rebind();
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }

    protected void gvBudgetProposal_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           
            if(e.CommandName =="Page" || e.CommandName == "ChangePageSize")
            {
                return;
            }
            string revisionid = ((RadLabel)e.Item.FindControl("lblRevisionId")).Text;
            string proposalid = ((RadLabel)e.Item.FindControl("lblProposalId")).Text;

            if (e.CommandName.ToUpper() == "REVISION")
            {
                Response.Redirect("OwnerBudgetProposalRevision.aspx?proposalid="+proposalid+"&revisionid=" + revisionid, false);
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixOwnerBudget.BudgetProposalDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , new Guid(proposalid)
                                                                    );
                BindData();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvBudgetProposal_DeleteCommand(object sender, GridCommandEventArgs de)
    {
        BindData();
    }

    protected void gvBudgetProposal_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton edt = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edt != null) edt.Visible = false;
            if (edt != null)
            {
                RadLabel lblid = (RadLabel)e.Item.FindControl("lblProposalId");
                edt.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'OwnerBudgetProposalAddEdit.aspx?revisionid=" + lblid.Text + "'); return false;");
            }
        }

        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            int? vesselid = General.GetNullableInteger(drv["FLDVESSELID"].ToString());
            RadLabel lblExistingNew = (RadLabel)e.Item.FindControl("lblExistingNew");
            if (lblExistingNew != null)
            {
                if (vesselid != null)
                    lblExistingNew.Text= "Existing";
                else
                    lblExistingNew.Text = "New";
            }
        }

    }
 
   
    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Vessel Name", "Proposal Title", "Proposal Date" };
        string[] alColumns = { "FLDVESSELNAME", "FLDPROPOSALTITLE", "FLDPROPOSALDATE" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixOwnerBudget.BudgetProposalSearch(General.GetNullableString(txtVesselName.Text)
                                                                , General.GetNullableString(txtProposalTitle.Text)
                                                                , General.GetNullableDateTime(ucFromProposedDate.Text)
                                                                , General.GetNullableDateTime(ucToProposedDate.Text)
                                                                , 1
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                );
        

        Response.AddHeader("Content-Disposition", "attachment; filename=\"BudgetProposal.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Budget Proposal</h3></td>");
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
}

